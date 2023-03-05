using System;

namespace UDT.Attributes
{
    public class InputMethod : Attribute
    {
        public string name;
        
        public InputMethod(string name)
        {
            this.name = name;
        }
    }
}