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
        
        private InputActionAsset _inputActionAsset;
        private InputActionMap _playerActionMap;
        
        private Player _player;

        private CinemachineVirtualCameraBase _cameraRig;
        public List<CinemachineVirtualCameraBase> cameraRigs = new List<CinemachineVirtualCameraBase>();
        
        /// <summary>
        /// Set the InputActionAsset to the one given
        /// </summary>
        /// <param name="inputActionAsset"></param>
        public void SetInputAsset(InputActionAsset inputActionAsset)
        {
            this._inputActionAsset = inputActionAsset;
        }
        
        /// <summary>
        /// Set the InputActionMap to the one with the given name
        /// </summary>
        /// <param name="name"></param>
        public void SetInputMap(string name)
        {
            _playerActionMap = _inputActionAsset.FindActionMap(name);
        }
        
        public override void InitController()
        {
            playerInput = GetComponent<PlayerInput>();
            if (playerInput.currentActionMap != null) _playerActionMap = playerInput.currentActionMap;
            if(_inputActionAsset != null) _playerActionMap = _inputActionAsset.actionMaps[0];

            if(_playerActionMap == null)
            {
                Debug.LogError("InputActionMap is null", this);
                return;
            }
            foreach (InputAction action in _playerActionMap.actions)
            {
                action.Enable();
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
            foreach (StandardObject controllable in Controllables)
            {
                if(((MonoBehaviour)controllable).enabled)
                    controllable.controllerValues.OnInputAction(context);
                
                try
                {
                    //Set inputs to Byte array
                    for (int i = 0; i < _playerActionMap.actions.Count; i++)
                        controllable.controllerValues.InputByte = (byte)(i << Convert.ToInt16(context.ReadValue<bool>()));
                }
                catch
                {
                    //ignore
                }
            }
        }
    }
}