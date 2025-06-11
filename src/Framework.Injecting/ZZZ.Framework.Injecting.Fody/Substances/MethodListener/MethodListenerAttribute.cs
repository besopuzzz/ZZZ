using System;
using System.Collections.Generic;
using System.Text;

namespace ZZZ.Framework.Injecting.Substances.MethodListener
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class MethodListenerAttribute : Attribute
    {

        protected virtual void OnEntry(MethodInfoArgs args)
        {

        }

        protected virtual void OnExit(MethodInfoArgs args) 
        {

        }

        protected virtual void OnException(MethodInfoArgs args)
        {

        }

        protected virtual void OnSuccess(MethodInfoArgs args)
        {

        }

        internal void Entry(MethodInfoArgs args)
        {
            OnEntry(args);
        }

        internal void Exit(MethodInfoArgs args)
        {
            OnExit(args);
        }
        internal void Exception(MethodInfoArgs args)
        {
            OnException(args);
        }
        internal void Success(MethodInfoArgs args)
        {
            OnSuccess(args);
        }
    }
}
