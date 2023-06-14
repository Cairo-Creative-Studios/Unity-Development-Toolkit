using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UDT.DataTypes;
using UDT.Reflection;
using UnityEngine.Serialization;

namespace UDT.Core
{
    /// <summary>
    /// The State Machine Modules allows Hierarchical, Class Based State Machines to be constructed within any class. The Module handles Method calls within the Nested State Class Instances that are generated for Scripts that are State Machine Enabled.
    /// </summary>
    public class StateMachineModule : Singleton<StateMachineModule>
    {
        /// <summary>
        /// All the Machines in the State Machine Module
        /// </summary>
        [Tooltip("All the State Machines that have been created with the State Machine Module")]
        private List<IFSM> machines = new List<IFSM>();

        /// <summary>
        /// Update the State Machine Module
        /// </summary>
        void Update()
        {
            foreach (IFSM machine in machines)
            {
                var node = machine.states.currentNode.value;
                if(node.GetType().IsAssignableFrom(typeof(State)))
                    ((State)node).Update();
            }
        }

        /// <summary>
        /// Creates a State Machine for the Given Instance
        /// </summary>
        /// <param name="root">Root.</param>
        public static void AddStateMachine(IFSM createdMachine)
        {
            //Create the new State Machine
            createdMachine.states = createdMachine.GetNestedClassesAsTree<IStateNode, State>(true);
            
            if(createdMachine.states.rootNode.children == null) return;
            
            Type[] transitions = createdMachine.GetNestedTypesOfBaseType<IStateNode>();
            List<Transition> transitionInstances = new();
            
            foreach (var transition in transitions)
            {
                transitionInstances.Add(Activator.CreateInstance(transition, new object[]{createdMachine}) as Transition);
            }
            createdMachine.transitions = transitionInstances.ToArray();

            createdMachine.states.currentNode = createdMachine.states.rootNode;
            
            //Add a reference to the Tree Node in the Created State
            foreach (Node<IStateNode> node in createdMachine.states.Flatten())
            {
                if (node.value is State stateNode)
                {
                    stateNode.node = node;
                    stateNode.Context = node.parent.value;

                    var path = "";
                    if (node.parent == null)
                    {
                        path = node.parent.value + "/" + path;
                    }
                    
                    stateNode.Path = path;
                }
            }

            //Call the Enter Method on the active State
            var nodeValue = createdMachine.states.currentNode.value;
            if(nodeValue.GetType().IsAssignableFrom(typeof(State)))
                ((State)nodeValue).Enter();
            
            createdMachine.InitMachine();

            //Add the created State Machine to the List of Machines
            Instance.machines.Add(createdMachine);
        }

        /// <summary>
        /// Gets the State of the Object
        /// </summary>
        /// <returns>The state.</returns>
        /// <param name="root">Root.</param>
        public static object GetState<T>(T root)
        {
            IFSM machine = GetMachine(root);
            Tree<IStateNode> states = machine.states;

            return states[states.cursor];
        }

        /// <summary>
        /// Sets the State of a given State Machine enabled Object based on the Type of the State
        /// </summary>
        /// <param name="machine"></param>
        /// <typeparam name="TState"></typeparam>

        public static void SetState<TState>(IFSM machine)
        {
            var stateNodes = machine.states.Flatten();
            
            foreach (var stateNode in stateNodes)
            {
                if (stateNode.value is TState)
                {
                    var currentState = machine.states.currentNode.value;
            
                    if(currentState.GetType().IsAssignableFrom(typeof(State)))
                        ((State)currentState).Exit();

                    machine.states.currentNode = stateNode;
                    currentState = stateNode.value;
                    
                    if (typeof(State).IsAssignableFrom(currentState.GetType()))
                        ((State)currentState).Enter();
                    
                    return;
                }
            }
            
            Debug.LogError("Type " + typeof(TState) + " is not a nested Type of "+ machine);
        }

        /// <summary>
        /// Gets a Machine that has been enabled through the State Machine Module
        /// </summary>
        /// <returns>The machine.</returns>
        /// <param name="root">Root.</param>
        private static IFSM GetMachine<T>(T root)
        {
            foreach (IFSM machine in Instance.machines)
            {
                if ((object)machine == (object)root)
                    return machine;
            }

            return null;
        }
    }

    /// <summary>
    /// The State Machine Interface
    /// </summary>
    public interface IFSM : IStateNode
    {
        /// <summary>
        /// The State Machine Tree contains all the State classes within the core Object's collection of States. 
        /// </summary>
        public Tree<IStateNode> states { get; set; }
        public Transition[] transitions { get; set; }

        public void InitMachine();
        
        public void InitMachine(Tree<IStateNode> states)
        {
            this.states = states;

            //Step the Tree Forward into the First State
            states.StepForward(0);
        }
        
        public IStateNode _GetState()
        {
            return states.currentNode.value;
        }
        
        
        public T _GetState<T>() where T : IStateNode
        {
            var stateList = this.states.Flatten();
            for(int i = 0; i < stateList.Length; i++)
            {
                if (stateList[i] is T)
                {
                    return (T)(object)stateList[i];
                }
            }
            return default;
        }
        
        /// <summary>
        /// Calls a Method on the Current State, and it's Children
        /// </summary>
        /// <param name="methodNamem"></param>
        /// <param name="parameters"></param>
        public object CallStateMethod(string methodName, object[] parameters = null)
        {
            var state = states.currentNode.value;
            return state.CallMethod("methodName", parameters);
        }

        public void Transition<TPreviousState, TNextState>()
        {
            Transition usedTransition = null;
            
            foreach (var transition in transitions)
            {
                if (transition.previousStateType == typeof(TPreviousState) && transition.nextStateType == typeof(TNextState))
                {
                    usedTransition = transition;
                    break;
                }
            }
            
            usedTransition?.OnTransition(this);
        }
    }

    public interface IStateNode
    {
        
    }
    
    public class State : IStateNode
    {
        public Node<IStateNode> node = null;
        public object Context;
        public string Path;
        
        public virtual void Enter()
        {
        }
        public virtual void Update()
        {
        }
        public virtual void Exit()
        {
        }
    }

    [Serializable]
    public class State<T> : State
    {
        public new T Context;

        public void CoreUpdate()
        {

        }

        public string GetState()
        {
            return node.value.ToString();
        }

        /// <summary>
        /// Sets the State to the State with the given path
        /// </summary>
        /// <param name="statePath">State path.</param>
        public void SetState(string statePath)
        {
            Tree<IStateNode> tree = node.tree;
            tree.Reset();

            bool exists = true;

            List<object> activeStates = new List<object>();
            object lastState = tree.currentNode.value;

            for (int i = 0; i < statePath.TokenCount('/'); i++)
            {
                if (exists)
                {
                    //Step the Tree Forward into the Nest
                    exists = tree.StepForward(statePath.TokenAt(i, '/'));
                    activeStates.Add(tree.currentNode.value);
                }
                else
                {
                    //If the Requested State wasn't found, call an error set the Active State back to this one and end the Search
                    Debug.LogError("Given State Path is invalid, the given State Hiearchy does not exist: (" +
                                   statePath + ")");
                    tree.currentNode = node;
                    return;
                }
            }

            //Call Exit and Enter Methods of appropriate States
            if(lastState.GetType().IsAssignableFrom(typeof(State)))
                ((State)lastState).Exit();
            foreach (object state in activeStates) if(state is State) (state as State).Enter();
        }

        /// <summary>
        /// Gets the Value of a Field in the State Hiearchy. If two States contain the same Variable, the top-most value will be used.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="fieldName">Field name.</param>
        public object GetField(string fieldName)
        {
            Node<IStateNode>[] hiearchy = node.GetHiearchy();

            foreach (Node<IStateNode> parent in hiearchy)
            {
                return parent.value.GetField(fieldName);
            }

            return null;
        }

        /// <summary>
        /// Sets the value of a Field in the State Hiearchy. If two States contain the same Variable, the top-most value will be used
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <param name="value">Value.</param>
        public void SetField(string fieldName, object value)
        {
            Node<IStateNode>[] hiearchy = node.GetHiearchy();

            foreach (Node<IStateNode> parent in hiearchy)
            {
                parent.value.SetField(fieldName, value);
                return;
            }
        }

        /// <summary>
        /// Calls a Method in the State hierarchy. If two States contain the same Method name, the top-most Method will be called.
        /// </summary>
        /// <returns>The method.</returns>
        /// <param name="methodName">Method name.</param>
        /// <param name="parameters">Parameters.</param>
        public object CallMethod(string methodName, object[] parameters)
        {
            Node<IStateNode>[] hiearchy = node.GetHiearchy();

            foreach (Node<IStateNode> parent in hiearchy)
            {
                return parent.value.CallMethod(methodName, parameters);
            }

            return null;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }

    public abstract class Transition
    {
        public IFSM Machine;
        public Type previousStateType;
        public Type nextStateType;
        
        public Transition(IFSM Machine)
        {
            this.Machine = Machine;
            OnTransition(Machine);
        }

        public abstract void OnTransition(IFSM Machine);
    }
    
    /// <summary>
    /// Extend this class to create a Transition between two States, and call CompleteTransition() when the Transition is completed.
    /// </summary>
    /// <typeparam name="TPreviousState"></typeparam>
    /// <typeparam name="TNextState"></typeparam>
    public abstract class Transition<T, TPreviousState, TNextState> : Transition where T : IFSM where TPreviousState : State where TNextState : State
    {
        public T Context;
        public TPreviousState previousState;
        public TNextState nextState;

        public Transition(IFSM Machine) : base(Machine)
        {
            Context = (T)Machine;
            previousState = Machine._GetState<TPreviousState>();
            nextState = Machine._GetState<TNextState>();
            previousStateType = typeof(TPreviousState);
            nextStateType = typeof(TNextState);
        }
        
        public virtual void CompleteTransition()
        {
            StateMachineModule.SetState<TNextState>(Machine);
        }
    }
}
