using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NetBaseClass : BuildingClass 
{
    public NetBaseClass()
    {
        this.metalCost = -100.0f;
        this.energy = 30.0f;
        this.buildTime = 10.0f;
        this.health = 100.0f;
        this.name="NetBase";
        this.GO = (GameObject) Resources.Load<GameObject>("Prefabs/Buildings/NetBase.prefab");
    }
}
