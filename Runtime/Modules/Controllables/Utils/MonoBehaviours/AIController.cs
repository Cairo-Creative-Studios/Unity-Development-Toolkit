namespace UDT.Core.Controllables
{
    /// <summary>
    /// The Base class for AI Controllers.
    /// </summary>
    public class AIController : Controller
    {
        private void Update()
        {
            foreach (ControllableComponent controllable in Controllables)
            {
                //Possess Controllable if not possessed
                if(!controllable.isPossessed)
                    controllable.Possess(this);
                
                //Call Input Action on Controllable
                if(inputMap.performedInputs.Count > 0)
                    foreach (var input in inputMap.performedInputs)
                    {
                        controllable.OnInputAction(input);
                    }
            }
        }
    }
}