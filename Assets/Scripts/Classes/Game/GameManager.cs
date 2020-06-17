using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.Avlor.SynthEden
{
	
	public class GameManager : MonoBehaviourPunCallbacks
	{
		#region Public Fields
		public bool	OfflineMode;
		public static GameManager Instance;
		public GameObject playerPrefab;
		public GameObject lastPlayer;

		#endregion
		
		#region Photon Callbacks
		void Awake()
		{
			if(OfflineMode)
				PhotonNetwork.OfflineMode = true;
			playerPrefab = Resources.Load<GameObject>("Prefabs/Player/Player");
		}
		void Start()
		{
			Instance = this;
			if(playerPrefab == null)
				Debug.LogError("PlayerPrefab missing", this);
			else
			{
				if(PlayerCharacterController.LocalPlayerInstance == null)
				{
					Debug.LogFormat("Instantiating new player in {0}", SceneManager.GetActiveScene().name);
					lastPlayer = PhotonNetwork.Instantiate("Prefabs/Player/"+this.playerPrefab.name, new Vector3(34.68f, 5.0f, -281.47f), Quaternion.identity, 0); //identity is no rotation, alignex with world or parent axis
					lastPlayer.name = "Player" + GameObject.FindGameObjectsWithTag("Player").Length;
				}
				else //if the local player is already here 
				{
					Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName); 
				}
			}
		}
		public override void OnLeftRoom()
		{
			SceneManager.LoadScene(0);
		}

		public override void OnPlayerEnteredRoom(Player other)
		{
		    Debug.LogFormat("{0} joined the game", other.NickName); // not seen if you're the player connecting


    		if (PhotonNetwork.IsMasterClient)
    		{
        		Debug.LogFormat("LOADARENA()", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

		        //LoadArena();
    		}
		}


		public override void OnPlayerLeftRoom(Player other)
		{
    		Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

		    if (PhotonNetwork.IsMasterClient)
    		{
	        	Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

	        	LoadArena();
    		}
		}

		#endregion

		#region Public Methods

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		#endregion

		#region Private Methods

		void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
			
			Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount); 
			
			PhotonNetwork.LoadLevel("Map"); //loaded only by the master client
		}

		#endregion
	}
}

