using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InstanceManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject instantiate(BuildingClass buildingType, GameObject Owner, Vector3 position, bool activated){
        GameObject instance = (GameObject) PhotonNetwork.Instantiate("Prefabs/Buildings/"+ buildingType.GO.name, position, Quaternion.identity, 0);
        BuildingConfiguration config = instance.AddComponent<BuildingConfiguration>();
        instance.AddComponent<Owner>();
        instance.GetComponent<Owner>().owner=Owner;
		config.metalCost = buildingType.metalCost;
        config.metalProd = buildingType.metalProd;
        config.energy = buildingType.energy;
        config.buildTime = buildingType.buildTime;
        config.health = buildingType.health;
        config.name=buildingType.name;
        config.GO = instance;
        config.isActivated = activated;

    	return (GameObject) instance; 
    }
}
