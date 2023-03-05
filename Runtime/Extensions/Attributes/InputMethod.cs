using System;
using UDT.Core.Controllables;

namespace UDT.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InputMethod : Attribute
    {
        public InputMethodLinker.Link.InputType inputType;
        public string name;
        
        public InputMethod(string name, InputMethodLinker.Link.InputType inputType = InputMethodLinker.Link.InputType.Button)
        {
            this.name = name;
            this.inputType = inputType;
        }
    }
}