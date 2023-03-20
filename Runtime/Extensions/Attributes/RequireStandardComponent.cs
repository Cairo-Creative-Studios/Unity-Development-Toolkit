using System;

[AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
public class RequireStandardComponent : Attribute
{
    public System.Type type;
    
    public RequireStandardComponent(System.Type type)
    {
        this.type = type;
    }
}