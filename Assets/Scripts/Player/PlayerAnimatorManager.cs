using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Avlor.SynthEden{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        private Animator animator;

        #region MonoBehaviour Callbacks
        void Start()
        {
            animator = GetComponent<Animator>();
            if(!animator)
                Debug.LogError("PlayerAnimatorManager missing Animator component", this);    
        }

        void Update()
        {
            if(!photonView.IsMine && PhotonNetwork.IsConnected) 
                return;
            
            if(!animator)
                return;
            
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if(stateInfo.IsName("Base Layer.HumanoidRun"))
            {
                if(Input.GetButton("Jump"))
                    animator.SetTrigger("Jump");
            }

            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            if(v>0){
                animator.SetFloat("VSpeed", h*h + v*v); //squared to have a positive value
            }
            else
                animator.SetFloat("VSpeed", v*v); 

            if(h>0)
                animator.SetFloat("HSpeed", h*h + v*v); //squared to have a positive value
            else
                animator.SetFloat("HSpeed", v*v);     
        }
        #endregion
    }
}
