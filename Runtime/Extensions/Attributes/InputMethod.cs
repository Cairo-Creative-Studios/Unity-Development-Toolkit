using System;

namespace UDT.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InputMethod : Attribute
    {
        public string name;
        
        public InputMethod(string name)
        {
            this.name = name;
        }
    }
}