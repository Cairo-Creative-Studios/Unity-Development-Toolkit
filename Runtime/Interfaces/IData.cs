using System;

namespace UDT.Core
{
    public interface IData
    {
        public bool Initialized { get; set; }
    }
    
    public interface IData<T> : IData where T : Data
    {
        public Type GetDataType() 
        {
            return Data.GetType();
        }

        public T Data
        {
            get
            {
                return (T)CoreModule.GetStaticData(typeof(T));
            }
            set
            {
                _data = value;
            }
        }

        public static T _data;
    }
}