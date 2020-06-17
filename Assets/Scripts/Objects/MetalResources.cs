using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class MetalResources : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public float amount;
    public bool highlighted;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
 
    [PunRPC] 
    public void lowerResources(float val){
        amount -= val;
        if(amount<=0){
            PhotonNetwork.Destroy(gameObject); // Non master clients cannot destroy so we use an RPC 
        }
    } 
    public void getResource(GameObject receiver, float rate){
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("lowerResources", RpcTarget.MasterClient, rate);
        receiver.SendMessage("Metal",rate);
    }
    public void highlight(){
        gameObject.GetComponent<Renderer>().material.color=Color.red;
    }
    public void revertMaterial(){
        gameObject.GetComponent<Renderer>().material=(Material) Resources.Load<Material>("Materials/Mineral");
    }
}
