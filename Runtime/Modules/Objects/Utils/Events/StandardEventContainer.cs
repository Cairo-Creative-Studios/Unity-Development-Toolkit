namespace UDT.Core
{
    public class StandardEventContainer
    {
        public SerializableDictionary<string, object> Events = new SerializableDictionary<string, object>();

        public StandardEventContainer(StandardEvent[] events)
        {
            for(int i = 0; i < events.Length; i++)
            {
                Events.Add(events[i].Name, events[i]);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void Invoke(string name)
        {
            if (Events.ContainsKey(name))
            {
                StandardEvent e = (StandardEvent)Events[name];
                e.Invoke();
            }
        }
    }
}