using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DroneClass : BuildingClass 
{
    public DroneClass()
    {
        this.metalCost = 50.0f;
        this.metalProd = 100.0f;
        this.energy = -15.0f;
        this.buildTime = 10.0f;
        this.health = 100.0f;
        this.name="Drone";
        this.GO = (GameObject) Resources.Load<GameObject>("Prefabs/Buildings/Drone");
    }
}
