using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Avlor.SynthEden
{
    
    public class PlayerUI : MonoBehaviour
    {
        #region Public Fields

        private PlayerCharacterController target;

        [SerializeField]
        private Text playerNameText;
        
        [SerializeField]
        private Slider playerHealthSlider;

        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        #endregion
        
        #region Public Fields
        
        float characterControllerHeight = 0f;
        Transform targetTransform;
        Vector3 targetPosition;

        #endregion

        #region MonoBehaviour Callbacks
        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Player1").GetComponent<Transform>(),false); // using Find is unoptimized
        }
        void update()
        {
            if(playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health; //Health of the Player (parent of PlayerCharacterController)
            }

            if(target == null){
                Debug.LogWarning("NO CHARACTER CONTROLLER, DESTROYED UI");
                Destroy(this.gameObject); 
                return;
            }
        }
        void LateUpdate()
        {
        // #Critical
        // Follow the Target GameObject on screen.
            if (targetTransform!=null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint (targetPosition) + screenOffset;
            }
        }
        #endregion

        #region Public Methods

    public void SetTarget (PlayerCharacterController _target)
    {
        if(_target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> controller for PlayerUI.SetTarget.", this);
            return;
        }
        //cache references for efficiency
        target = _target;

        CharacterController characterController = _target.GetComponent<CharacterController> ();
        // Get data from the Player that won't change during the lifetime of this Component
        if (characterController != null)
        {
            characterControllerHeight = characterController.height;
        }
        if(playerNameText != null)
        {
            playerNameText.text = "Player"; 
        }
    }

        #endregion
    }
}
