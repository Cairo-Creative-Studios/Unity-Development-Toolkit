using UDT.Data;

namespace UDT.Core
{
    /// <summary>
    /// A Runtime Object is a Singleton that can be used to manage the state of the game's Runtime.
    /// Extend this class to create custom Runtimes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Runtime<T> : Singleton<T>, IFSM where T : Runtime<T>
    {        
        public Tree<IStateNode> states { get; set; }
        public Tree<IStateNode> stateTree = new Tree<IStateNode>();

        public void InitMachine()
        {
            stateTree = states;
        }

        /// <summary>
        /// Set the State of the Runtime
        /// </summary>
        /// <param name="path">The Path to the desired State</param>
        public void SetState(string path)
        {
            StateMachineModule.SetState(this, path);
        }
        
        /// <summary>
        /// This Static Method spawns any Runtime Classes that are exist in Scripts as soon as the Runtime starts.
        /// That said, you should limit yourself to only one Runtime. If you would like to create seperate management
        /// scripts, use Systems instead. 
        /// </summary>
        void Awake()
        {
            Instance.enabled = true;
            StateMachineModule.AddStateMachine(this);
            stateTree = states;
        }
    }
}