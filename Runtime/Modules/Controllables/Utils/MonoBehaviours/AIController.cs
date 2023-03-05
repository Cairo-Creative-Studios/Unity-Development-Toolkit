namespace UDT.Core.Controllables
{
    /// <summary>
    /// The Base class for AI Controllers.
    /// </summary>
    public class AIController : Controller
    {
        private void Awake()
        {
            foreach (StandardObject controllable in Controllables)
            {
                //Possess Controllable if not possessed
                if(!controllable.controllerValues.IsPossessed)
                    controllable.controllerValues.Possess(this);
                
                //Call Input Action on Controllable
                if(inputMap.performedInputs.Count > 0)
                    foreach (var input in inputMap.performedInputs)
                    {
                        controllable.controllerValues.OnInputAction(input);
                    }
            }
        }
    }
}