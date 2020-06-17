using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SolarArrayClass : BuildingClass 
{
    public SolarArrayClass()
    {
        this.metalCost = 100.0f;
        this.energy = 30.0f;
        this.buildTime = 10.0f;
        this.health = 100.0f;
        this.name="Solar Array";
        this.GO = (GameObject) Resources.Load<GameObject>("Prefabs/Buildings/SolarArray");
    }
}
