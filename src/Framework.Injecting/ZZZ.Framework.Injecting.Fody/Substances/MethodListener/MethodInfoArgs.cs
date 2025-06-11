using System;
using System.Reflection;

namespace ZZZ.Framework.Injecting.Substances.MethodListener
{
    public class MethodInfoArgs : EventArgs
    {
        public object Instance { get; }

        public MethodInfo MethodInfo { get; }


        public MethodInfoArgs(object instance, MethodInfo methodInfo) 
        {
            Instance = instance;

            MethodInfo = methodInfo;
        }
    }
}
