using System;
using System.Collections.Generic;
using Cinemachine;
using UDT.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UDT.Core.Controllables
{
    /// <summary>
    /// The Player Controller gets input from the player and passes it to the controllable.
    /// </summary>
    [AddComponentMenu("Controllers/Player Controller")]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : Controller
    {
        public bool displayCursor = false;
        [SerializeField] private PlayerInput playerInput;
        
        private InputActionAsset inputActionMap;
        private InputActionMap _playerActionMap;
        
        private Player _player;

        private CinemachineVirtualCameraBase _cameraRig;
        public List<CinemachineVirtualCameraBase> cameraRigs = new List<CinemachineVirtualCameraBase>();
        
        public void Init(Player player, IComponentControllable controllable)
        {
            _player = player;
            if(controllable!= null)
                Controllables.Add(controllable);
        }
        
        public override void InitController()
        {
            _playerActionMap = data.inputActionMap.actionMaps[0];
            
            playerInput = GetComponent<PlayerInput>();
            if (playerInput.currentActionMap != null) _playerActionMap = playerInput.currentActionMap;
            if(inputActionMap != null) _playerActionMap = inputActionMap.FindActionMap("Player");

            if(_playerActionMap == null)
            {
                Debug.LogError("InputActionMap is null", this);
                return;
            }
            foreach (InputAction action in _playerActionMap.actions)
            {
                action.performed += OnInputAction;
                action.started += OnInputAction;
                action.canceled += OnInputAction;
            }
        }

        private void Update()
        {
            if (displayCursor)
            {
                Cursor.visible = true; 
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false; 
                Cursor.lockState = CursorLockMode.Locked;
            }
            
            CinemachineBrain.SoloCamera = _cameraRig;
        }

        public bool IsPlayer(Player player)
        {
            return this._player == player;
        }
        
        public int GetPlayerId()
        {
            return _player.playerID;
        }
        
        public void SetCameraRig(CinemachineVirtualCameraBase cameraRig)
        {
            //Add the Camera Rig to the list of camera rigs
            cameraRigs.Add(cameraRig);
            
            //Set the current Camera rig to the latest one in the list
            _cameraRig = cameraRigs[^1];
        }

        public void RemoveCameraRig(CinemachineVirtualCameraBase cameraRig)
        {
            //Remove the Camera Rig from the list of camera rigs
            if(cameraRigs.Contains(cameraRig))
                cameraRigs.Remove(cameraRig);
            
            //Set the current Camera rig to the latest one in the list
            _cameraRig = cameraRigs[^1];
        }
        
        public void OnInputAction(InputAction.CallbackContext context)
        {
            //Set inputs to SerializedInputMap
            if(context.valueType == typeof(bool))
                inputMap.SetInput(context.action.name, context.ReadValue<bool>());
            if(context.valueType == typeof(float))
                inputMap.SetInput(context.action.name, context.ReadValue<float>());
            if(context.valueType == typeof(Vector2))
                inputMap.SetInput(context.action.name, context.ReadValue<Vector2>());

            //Set inputs to Controllables
            foreach (IComponentControllable controllable in Controllables)
            {
                if(((MonoBehaviour)controllable).enabled)
                    controllable.OnInputAction(context);
                
                try
                {
                    //Set inputs to Byte array
                    for (int i = 0; i < _playerActionMap.actions.Count; i++)
                        controllable.inputByte = (byte)(i << Convert.ToInt16(context.ReadValue<bool>()));
                }
                catch
                {
                    //ignore
                }
            }
        }
    }
}