using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInventory: MonoBehaviourPunCallbacks
{
    public GameObject playerCanvas;
	private Text metalText;
    private Text energyText;
    public float metal;
    private float energy;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        metal = 50;
        energy = 0;
        health = 100;
        playerCanvas = (GameObject) Instantiate(Resources.Load("Prefabs/Player/PlayerUICanvas")); 
        DontDestroyOnLoad(playerCanvas);
        playerCanvas.name = gameObject.name+" Canvas";
        if(!transform.parent.GetComponent<PhotonView>().IsMine)
            playerCanvas.SetActive(false);
        metalText = playerCanvas.transform.Find("MetalVal").GetComponent<Text>();
        energyText = playerCanvas.transform.Find("EnergyVal").GetComponent<Text>();
        metalText.text=metal.ToString("F0");  
        energyText.text=energy.ToString("F0")+"/m";   
}
    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
            PhotonNetwork.Destroy(gameObject);
    }
    public void Metal(float value){
        metal+=value;
        metalText.text=metal.ToString("F0");  //buggy
    }
    public void Energy(float value){
        energy+=value;
        energyText.text=energy.ToString("F0")+"/m";  
    }
}
