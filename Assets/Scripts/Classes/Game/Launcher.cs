using UnityEngine;
using Photon.Pun;
using System;

using Photon.Realtime;

namespace Com.Avlor.SynthEden{

	public class Launcher : MonoBehaviourPunCallbacks
	{
    	#region Private Serializable Fields
		[SerializeField]
		private Byte maxPlayersPerRoom = 8;	
		#endregion
		
		#region Private Fields
		bool isConnecting;
		//This client's version number. Users are separated in each version.
		String gameVersion ="0.1";
		[SerializeField]
		private GameObject controlPanel;
		[SerializeField]
		private GameObject progressLabel;
		#endregion			

		#region MonoBehaviour CallBacks

		//called during early init phase
		void Awake()
		{
			// this makes sure we can use PhotonNetwork.LoadLevel() on the master
			// client and all clients in the same room load the same level 
			
			PhotonNetwork.AutomaticallySyncScene = true;
		}
		
		void Start()
		{
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
		}
		#endregion

		#region Public Methods
		
		public void Connect()
		{
			isConnecting = true;
			progressLabel.SetActive(true);
			controlPanel.SetActive(false);
		
			if(PhotonNetwork.IsConnected)
			{
				//Join or create if no room available
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{
				PhotonNetwork.GameVersion = gameVersion;
				PhotonNetwork.ConnectUsingSettings();	
			}
		}
		#endregion

		#region MonoBehaviourPunCallbacks CallBacks

		public override void OnConnectedToMaster()
		{
			Debug.Log("OnConnectedToMaster()");
			
			if(isConnecting)
				PhotonNetwork.JoinRandomRoom();
		}

		public override void OnDisconnected(DisconnectCause cause)
		{
			progressLabel.SetActive(true);
			controlPanel.SetActive(false);
			Debug.LogWarningFormat("OnDisconnected() was called with reason {0}", cause);
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.Log("OnJoinRandomFailed() - PhotonNetwork.CreateRoom()");

			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom});
		}
		public override void OnJoinedRoom()
		{
			Debug.Log("OnJoinedRandom() - This client is now in a room.");

			PhotonNetwork.LoadLevel("Map");
		}
		#endregion
	}
}