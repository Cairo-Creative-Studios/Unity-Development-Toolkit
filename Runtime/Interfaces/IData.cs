using System;

namespace UDT.Core
{
    public interface IData
    {
        public Type DataType { get; set; }
        public bool Initialized { get; set; }
    }
    
    public interface IData<T> : IData where T : Data
    {
        public Type DataType
        {
            get
            {
                return Data.GetType();
            }

            set
            {
                // Do Nothing
            }
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