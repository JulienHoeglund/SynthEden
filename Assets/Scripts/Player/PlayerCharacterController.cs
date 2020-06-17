using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Com.Avlor.SynthEden;

public class PlayerCharacterController : MonoBehaviourPunCallbacks, IPunObservable {
    
    public float Health = 100f;
    public float speed = 10.0f;
    private float translation;
    private float straffe;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float distToGround = 0.0f;
    public Rigidbody projectile; 
    public bool wasBuilding;    
    
    [SerializeField]
    private GameObject beam;
    
    public GameObject PlayerUiPrefab;
    bool IsFiring;
    
    public static GameObject LocalPlayerInstance; 

    #region IPunObservable Implementation
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {    
        if(stream.IsWriting)
        {
            stream.SendNext(IsFiring);
            stream.SendNext(Health);
        }
        else
        {
            this.IsFiring = (bool) stream.ReceiveNext();
            this.Health = (float) stream.ReceiveNext();
        }
    }
    #endregion

    void Awake(){
        PlayerUiPrefab = (GameObject) Resources.Load("Prefabs/Player/PlayerUI");
        if(transform.parent.GetComponent<PhotonView>().IsMine){
            PlayerCharacterController.LocalPlayerInstance = this.gameObject;
        }
        else{
            gameObject.GetComponent<AudioListener>().enabled = false ;   
        }
        Cursor.lockState = CursorLockMode.Locked;		

        DontDestroyOnLoad(this.gameObject);
    }
    void Start () {
        #if UNITY_5_4_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
        {
            if(this != null)
                this.CalledOnLevelWasLoaded(scene.buildIndex);
        };
        #endif
        if(PlayerUiPrefab != null)
        {
            GameObject _uiGO = Instantiate(PlayerUiPrefab);
            _uiGO.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            _uiGO.name = gameObject.name + "healthUI1";
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
        // turn off the cursor
        wasBuilding=false;

        if(!transform.parent.GetComponent<PhotonView>().IsMine){
            gameObject.GetComponent<Camera>().enabled=false; //If this isn't the local player, deactivate the camera comp
        }
        else{
            gameObject.GetComponent<Camera>().enabled=true; 

        }
        
    }
	 bool IsGrounded(){
        return Physics.Raycast(transform.position, -Vector3.up, (float)distToGround + 0.1f);
    }
	void Update () {
        if(!transform.parent.GetComponent<PhotonView>().IsMine && PhotonNetwork.IsConnected)
            return;
        else
            ProcessInputs();
        
        translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        //transform.parent.Translate(straffe, 0, translation);
        //transform.parent.transform.eulerAngles = new Vector3(0f, yaw, 0f);
        transform.eulerAngles = new Vector3(pitch, 0f, 0.0f);
        
        if (Input.GetKeyDown("escape")) {
            /* 
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
            */
            if(Cursor.lockState == CursorLockMode.Locked){
                Cursor.lockState = CursorLockMode.None;       
            }
            else{
                Cursor.lockState = CursorLockMode.Locked;       
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(!photonView.IsMine)
            return;
        if(!other.name.Contains("Beam"))
            return;
        Health -= 1f;
    }
    void OnTriggerStay(Collider other)
    {
        if(!photonView.IsMine)
            return;
        if(!other.name.Contains("Beam"))
            return;
        Health -= 1f * Time.deltaTime;
    }
    
    void ProcessInputs()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(!IsFiring)
            {
                IsFiring = true;
            }
        }
        if(Input.GetButtonUp("Fire1"))
        {
            if(IsFiring)
            {
                IsFiring = false;
            }
        }
    }
    
    #if !UNITY_5_4_OR_NEWER
    /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
    #endif


    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
        GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
        _uiGo.name = gameObject.name + "healthUI2";
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }
}