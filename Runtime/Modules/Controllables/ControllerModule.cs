using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UDT.Core.Controllables
{
    public class ControllerModule : Singleton<ControllerModule>
    {
        /// <summary>
        /// A list of all controllers
        /// </summary>
        [Tooltip("A list of all controllers")]
        public List<Controller> Controllers { get; private set; } = new List<Controller>();
        /// <summary>
        /// A dictionary of player IDs and their respective controllers
        /// </summary>
        [Tooltip("A dictionary of player IDs and their respective controllers")]
        public SerializableDictionary<int, Controller> playerControllerPairs = new SerializableDictionary<int, Controller>();
        /// <summary>
        /// A list of all AI controllers
        /// </summary>
        [Tooltip("A list of all AI controllers")]
        public List<AIController> AIControllers { get; private set; } = new List<AIController>();
        /// <summary>
        /// A list of all the Controller Data loaded from the Resources folder
        /// </summary>
        [Tooltip("A list of all the Controller Data loaded from the Resources folder")]
        public ControllerData[] data = new ControllerData[]{};

        public override void Init()
        {
            data = Resources.LoadAll<ControllerData>("") as ControllerData[];
        }

        /// <summary>
        /// Add the controller to the list
        /// </summary>
        /// <param name="controller"></param>
        public static void AddController(Controller controller)
        {
            Instance.Controllers.Add(controller);
            if(controller.data.controllerType == ControllerData.ControllerType.Player)
                Instance.playerControllerPairs.Add(Instance.playerControllerPairs.Keys.Count, controller as PlayerController);
        }
        
        /// <summary>
        /// Remove the controller from the list
        /// </summary>
        /// <param name="controller"></param>
        public static void RemoveController(Controller controller)
        {
            Instance.Controllers.Remove(controller);
        }

        public static PlayerController CreatePlayerController(string dataName)
        {
            return CreateControllerFromData(dataName) as PlayerController;
        }
        
        /// <summary>
        /// Create the Controller from Data by name
        /// </summary>
        /// <param name="dataName"></param>
        /// <returns></returns>
        public static Controller CreateControllerFromData(string dataName)
        {
            ControllerData data = Instance.data.First(data => data.name == dataName);
            
            if(data == null)
            {
                return null;
            }
            
            return CreateControllerFrmData(data);
        }
        
        /// <summary>
        /// Create the Controller from Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Controller CreateControllerFrmData(ControllerData data)
        {
            if(data.controllerType == ControllerData.ControllerType.Player)
                return CreatePlayerController(data);
            if(data.controllerType == ControllerData.ControllerType.AI)
                return CreateAIController(data);
            return null;
        }

        /// <summary>
        /// Create the Player Controller from Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Controller CreatePlayerController(ControllerData data)
        {
            if (data.inputActionAsset == null)
            {
                Debug.LogError("Cannot create player controller without an input action asset", data);
                return null;
            } 
            GameObject controllerObject = new GameObject(data.name);
            PlayerController controller = controllerObject.AddComponent<PlayerController>();
            controller.data = data;
            controller.SetInputAsset(data.inputActionAsset);
            controller.SetInputMap(data.inputMap);
            controller.InitController();
            return controller;
        }
        
        /// <summary>
        /// Create the AI Controller from Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Controller CreateAIController(ControllerData data)
        {
            GameObject controllerObject = new GameObject(data.name);
            AIController controller = controllerObject.AddComponent<AIController>();
            controller.data = data;
            return controller;
        }
        
        /// <summary>
        /// Get the controller of the player with the given ID
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public static Controller GetPlayerController(int playerID)
        {
            return Instance.playerControllerPairs[playerID];
        }
    }
}