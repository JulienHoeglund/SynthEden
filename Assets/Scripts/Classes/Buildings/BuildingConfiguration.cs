using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuildingConfiguration : MonoBehaviourPunCallbacks
{
	public float metalCost;
    public float metalProd;
	public float energy;
	public float buildTime; 
	public float health;	
	public bool isActivated;
	public string GOname;
	public GameObject GO;

    //TO FIX : When the server instantiates a new drone on another client, the building config component is not added
    //and results in a null reference
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        PhotonView photonView = gameObject.GetComponent<PhotonView>();
        this.photonView.RPC("enableRenderer", RpcTarget.AllBufferedViaServer);                
         */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    [PunRPC]
    public void enableRenderer()
    {
        GO.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    } 
    */
}
