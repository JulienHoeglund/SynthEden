using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WallClass : BuildingClass 
{
    public WallClass()
    {
        this.metalCost = 300.0f;
        this.energy = 0.0f;
        this.buildTime = 10.0f;
        this.health = 100.0f;
        this.name="Wall";
        this.GO = (GameObject) Resources.Load<GameObject>("Prefabs/Buildings/Wall");
    }
}
