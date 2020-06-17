using UnityEngine;
using System.Collections;

public class CameraManagerScript : MonoBehaviour {
    public Camera firstPersonCamera;
    public Camera overheadCamera;
    public bool ThirdPersonView;
    
    public void ShowOverheadView() {
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
    }
    
    public void ShowFirstPersonView() {
        firstPersonCamera.enabled = true;
        overheadCamera.enabled = false;
    }
    
    void Start()
    {
    }
    void Update()
    {
        if(Input.GetKeyDown("p"))
            ThirdPersonView = !ThirdPersonView;

        if(ThirdPersonView)
            ShowOverheadView();
        else
            ShowFirstPersonView();
    }
}