namespace UDT.Core
{
    public interface IStaticData
    {
        public bool Initialized { get; set; }
    }
    
    public interface IStaticData<T> : IStaticData where T : StaticData
    {
        public T Data { get; set; }
    }
}