using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingClass
{
	public float metalCost;
    public float metalProd;
	public float energy;
	public float buildTime;
	public float health;	
	public bool isActivated;
	public string name;
	public GameObject GO;
    public BuildingConfiguration config;

    public BuildingClass(){
        this.metalProd=0.0f;
    }
    
    public void activate(bool activated, GameObject player){
        PlayerInventory p = player.GetComponent<PlayerInventory>();                 
        AudioSource source = GO.GetComponent<AudioSource>();      
        
        if(activated){
            if(GO!=null){
                GO.GetComponent<BuildingConfiguration>().isActivated=true;
            }
        	p.Energy(energy); //activate energy consumption
            if(source!=null)
                source.mute=false;
        }
    	else{
            if(GO!=null)
                GO.GetComponent<BuildingConfiguration>().isActivated=false;
            if(source!=null)
                source.mute=true;
        }
    }
   
}
