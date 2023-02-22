using UnityEngine;
using System.Linq;

namespace UDT.Reflection
{
    public static class AnimatorExtensions
    {
        /// <summary>
        /// Fills this Animation's Parameters with the Values of another Animator, if they have the same Parameters
        /// </summary>
        /// <param name="fill"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        public static Animator CopyParametersFromAnother(this Animator fill, Animator copy)
        {
            foreach (AnimatorControllerParameter parameter in copy.parameters)
            {
                if(fill.parameters.Contains(parameter))
                    switch (parameter.type)
                    {
                        case (AnimatorControllerParameterType.Bool):
                            fill.SetBool(parameter.name, copy.GetBool(parameter.name));
                            break;
                        case (AnimatorControllerParameterType.Float):
                            fill.SetFloat(parameter.name, copy.GetFloat(parameter.name));
                            break;
                        case (AnimatorControllerParameterType.Int):
                            fill.SetInteger(parameter.name, copy.GetInteger(parameter.name));
                            break;
                    }
            }

            return fill;
        }

        /// <summary>
        /// Fills this Animator's Parameters with the Values of another, by attempting to copy Parameters with the same values.
        /// </summary>
        /// <param name="fill"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        public static Animator LooselyCopyParametersFromAnother(this Animator fill, Animator copy)
        {
            foreach (AnimatorControllerParameter parameter in copy.parameters)
            {
                try
                {
                    switch (parameter.type)
                    {
                        case (AnimatorControllerParameterType.Bool):
                            fill.SetBool(parameter.name, copy.GetBool(parameter.name));
                            break;
                        case (AnimatorControllerParameterType.Float):
                            fill.SetFloat(parameter.name, copy.GetFloat(parameter.name));
                            break;
                        case (AnimatorControllerParameterType.Int):
                            fill.SetInteger(parameter.name, copy.GetInteger(parameter.name));
                            break;
                    }
                }
                catch
                {
                    //Ignored, to prevent any mismatching Parameter Errors
                }
            }

            return fill;
        }
    }
}