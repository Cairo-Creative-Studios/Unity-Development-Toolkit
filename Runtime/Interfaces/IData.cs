using System;

namespace UDT.Core
{
    public interface IData
    {
        public bool Initialized { get; set; }
    }
    
    public interface IData<T> : IData where T : Data
    {
        public T Data { get; set; }
    }
}