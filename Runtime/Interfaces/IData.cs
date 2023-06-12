namespace UDT.Core
{
    public interface IData
    {
        public bool Initialized { get; set; }
    }
    
    public interface IData<T> : IData where T : Data
    {
        public T _Data
        {
            get
            {
                return (T)CoreModule.GetStaticData(typeof(T));
            }
            set
            {
                Data = value;
            }
        }

        public static T Data;
    }
}