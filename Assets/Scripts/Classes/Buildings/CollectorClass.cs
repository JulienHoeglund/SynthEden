using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class CollectorClass : MonoBehaviourPunCallbacks
{

    public int maxDistance;
	private Collider zone;
    private Collider[] oldHitColliders;
    
    // Start is called before the first frame update
    void Start()
    {	
    	zone = gameObject.GetComponent<Collider>();
        oldHitColliders = new Collider[0];
        maxDistance = 5;
    }

    // Update is called once per frame
    void Update()
    {   
        if(photonView.IsMine){
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 5);
            if(gameObject.GetComponent<BuildingConfiguration>().isActivated){
                for (int i=0; i < hitColliders.Length; i++){
                    if(hitColliders[i].gameObject.tag == "Metal" && hitColliders[i].gameObject.GetComponent<MetalResources>().amount>0){
                        hitColliders[i].gameObject.GetComponent<MetalResources>().getResource(gameObject.GetComponent<Owner>().owner, 1f * Time.deltaTime);
                    }
                }	    
            }
            else{
                for (int i=0; i < hitColliders.Length; i++){
                    if(hitColliders[i].gameObject.tag == "Metal"){
                        hitColliders[i].gameObject.GetComponent<MetalResources>().highlight();
                    }
                }            
            }
            revertOldColliders(hitColliders);
            oldHitColliders=hitColliders;
        }
    }
    void OnDestroy(){
        revertOldCollidersOnDestroy();
    }
    public void revertOldColliders(Collider[] hitColliders){
        bool present;
        for(int j=0; j < oldHitColliders.Length - 1;j++){
            present=false;
            for(int i=0; i < hitColliders.Length - 1;i++){
                if(oldHitColliders[j] == hitColliders[i])
                    present=true;
            }   
            if(!present && oldHitColliders[j] != null)
                if(oldHitColliders[j].gameObject != null)
                    if(oldHitColliders[j].gameObject.GetComponent<MetalResources>() != null)
                        oldHitColliders[j].gameObject.GetComponent<MetalResources>().revertMaterial();
        }
    }
    public void revertOldCollidersOnDestroy(){
            for(int j=0; j<oldHitColliders.Length - 1 ;j++){
                if(oldHitColliders[j] != null)
                    if(oldHitColliders[j].gameObject != null)
                        if(oldHitColliders[j].gameObject.GetComponent<MetalResources>() != null)
                            oldHitColliders[j].gameObject.GetComponent<MetalResources>().revertMaterial();
            }
    }
}
