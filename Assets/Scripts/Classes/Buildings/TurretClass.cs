using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TurretClass : BuildingClass 
{
    public TurretClass()
    {
        this.metalCost = 150.0f;
        this.energy = -30.0f;
        this.buildTime = 10.0f;
        this.health = 100.0f;
        this.name="Turret";
        this.GO = (GameObject) Resources.Load<GameObject>("Prefabs/Buildings/Turret");
    }
}
