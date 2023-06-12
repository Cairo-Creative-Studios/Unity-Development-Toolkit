namespace UDT.Core
{
    public interface IStaticData
    {
        public bool Initialized { get; set; }
        public void Init();
    }
    
    public interface IStaticData<T> : IStaticData where T : StaticData
    {
        public static T Data { get; set; }
    }
}