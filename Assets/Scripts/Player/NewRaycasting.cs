using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NewRaycasting : MonoBehaviourPunCallbacks
{
    public float maxGatheringDistanceAllowed;
	public float gatheringRate;
    public int maxBuildingDistanceAllowed; 
    private float TargetDistance;
    public int buildingNumber;
    private Text buildingText; 
    private Text buildingPrice; 
    public int numberOfBuildings;   
    public BuildingClass selectedType;
    public GameObject selectedGO;
    public InstanceManager instanceManager;
    public PhotonView photonView;
    public float fireRate;
    private float nextFire;

    public BuildingClass getBuildingInstance()
    {
        switch(buildingNumber)
        {
            case 0:
                return new DroneClass();
            case 1:
                return new ArmoryClass();
            /*
            case 2:
                return new ArmoryClass();
             case 3:
                return new TurretClass();
            case 4:
                return new WallClass();
            */
            default : 
                return new DroneClass();
        }
    }
    void Start()
    {
        buildingNumber = 0;
        buildingText = gameObject.GetComponent<PlayerInventory>().playerCanvas.transform.Find("BuildingText").GetComponent<Text>();
        buildingPrice = gameObject.GetComponent<PlayerInventory>().playerCanvas.transform.Find("BuildingPrice").GetComponent<Text>();
        numberOfBuildings = 5;
        instanceManager = GameObject.Find("BuildingManager").GetComponent<InstanceManager>();
        photonView = transform.parent.GetComponent<PhotonView>();
        selectedGO = null;
        fireRate = 0.1f;
    }

    void Update()
    {
        if(transform.parent.GetComponent<PhotonView>().IsMine && PhotonNetwork.IsConnected){ 
            selectedType = getBuildingInstance();
            buildingText.text = selectedType.name;   
            buildingPrice.text = selectedType.metalCost.ToString();  

            if(Input.GetKeyDown("b") && buildingNumber < numberOfBuildings - 1){
                    buildingNumber++;
                    selectedType = getBuildingInstance();   
                    buildingText.text = selectedType.name;
                    Debug.Log("Scrolled through buildings");
                }
                else if(Input.GetKeyDown("v") && buildingNumber > 0){
                    buildingNumber--;
                    selectedType = getBuildingInstance();   
                    buildingText.text = selectedType.name;
                    Debug.Log("Scrolled through buildings");
                }
                
            float metalCost = selectedType.metalCost;
            RaycastHit hit;        
            if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit)){
                TargetDistance = hit.distance;
                //Gather 
                if (Input.GetAxis("Fire2") > 0f){
                    if(TargetDistance<=maxGatheringDistanceAllowed && hit.collider.tag=="Metal"){
                        if(hit.collider.gameObject.GetComponent<MetalResources>().amount>0){
                            hit.collider.gameObject.GetComponent<MetalResources>().getResource(gameObject, gatheringRate);
                        }            
                    }
                }
                //SCROLL
                
                //BUILDING PLACEMENT
                if(Input.GetKeyDown("c") && GetComponent<PlayerInventory>().metal >= metalCost && selectedGO == null){
                    selectedType = getBuildingInstance();            
                    selectedGO = instanceManager.instantiate(selectedType, gameObject, hit.point, false); //Assign instance instead of the prefab
                    selectedType.GO = selectedGO; // BuildingClass (non MonoBehaviour class) has a GameObject 
                    selectedGO.layer=2;
                    buildingText.text = selectedType.name;
                    photonView.RPC("enableRenderer", RpcTarget.Others,selectedGO.GetComponent<PhotonView>().ViewID,false);                
                }
                if(selectedGO != null){
                    if(TargetDistance < maxBuildingDistanceAllowed && TargetDistance > 3){
                        selectedGO.transform.position = new Vector3 ((float) hit.point.x,0.0f,(float) hit.point.z);     
                        Renderer[] rs = selectedGO.GetComponentsInChildren<Renderer>();
                        foreach(Renderer r in rs)
                            r.enabled = true;
                        //ROTATE
                        if(Input.GetKey("r"))
                            selectedGO.transform.Rotate(0.0f, 3.0f, 0.0f, Space.Self);
                        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey("r"))
                            selectedGO.transform.eulerAngles= new Vector3(selectedGO.transform.eulerAngles.x,selectedGO.transform.eulerAngles.y+90.0f, selectedGO.transform.eulerAngles.z);
                        //PLACE
                        if(Input.GetMouseButtonDown(0)){
                            gameObject.GetComponent<PlayerInventory>().Metal(metalCost * -1);
                            if(selectedGO.GetComponent<BuildingConfiguration>() != null){
                                selectedType.GO = selectedGO;
                                selectedType.activate(true, gameObject);
                                photonView.RPC("enableRenderer", RpcTarget.Others,selectedGO.GetComponent<PhotonView>().ViewID,true);                
                            }
                            selectedGO = null;
                            selectedType = getBuildingInstance(); //get a new BuildingClass object
                            if(GetComponent<PlayerInventory>().metal >= metalCost)
                                selectedGO = instanceManager.instantiate(selectedType, gameObject, hit.point, false);
                        }
                        //CANCEL
                        else if(Input.GetAxis("Fire2") > 0f && selectedGO.scene.name != null){
                            PhotonNetwork.Destroy(selectedGO);
                        }
                    }//DISTANCE
                    else{
                        Renderer[] rs = selectedGO.GetComponentsInChildren<Renderer>();
                        foreach(Renderer r in rs)
                            r.enabled = false;
                    }      
                }
            }//NOT POINTING TO FLOOR
            else if(selectedGO !=null){
                Renderer[] rs = selectedGO.GetComponentsInChildren<Renderer>();
                foreach(Renderer r in rs)
                r.enabled = false;
            }

            if(GetComponent<PlayerInventory>().metal < metalCost){
                buildingText.color = new Color(1.0f, 0.0f, 0.0f);
            }
            else{
                buildingText.color = new Color(1.0f, 1.0f, 1.0f);
            }
            //SHOOT
            if(Input.GetAxis("Fire1") > 0.1f && Time.time > nextFire){
                nextFire = Time.time + fireRate;
                Vector3 point = gameObject.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.height, Screen.width, 0));
                photonView.RPC("createProjectile",RpcTarget.All,point);
            }
        }
    }
    [PunRPC]
    public void enableRenderer(int viewID, bool val)
    {
        GameObject obj = PhotonView.Find(viewID).gameObject;
        if(obj != null){
            obj.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = val;
        }
    }
    [PunRPC]
    public void createProjectile(Vector3 spawnPoint)
    {
        GameObject projectile = (GameObject) Instantiate(Resources.Load("Prefabs/Player/LaserProjectile"), spawnPoint, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 1500.0f);
        projectile.transform.LookAt(spawnPoint);
        projectile.transform.rotation = Quaternion.LookRotation(transform.forward);
        Destroy(projectile, 2.0f);
    }
}
