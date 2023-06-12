using System;
using UDT.DataTypes;
using UnityEngine;

namespace UDT.Core
{
    public class StateMachineStandardComponent : StandardComponent, IFSM
    {
        public bool IsNull { get; }
        public ComponentDataBase Data { get; set; }
        public Type System { get; set; }
        public StandardObject Object { get; set; }
        
        public virtual void OnInstantiate()
        {
        }

        public virtual void OnFree()
        {
        }

        public Tree<IStateNode> states { get; set; }
        public void InitMachine()
        {
            stateTree = states;
        }

        public Tree<IStateNode> stateTree;
    }
}