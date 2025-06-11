using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Injecting.Substances.MethodListener;

namespace ZZZ.Framework.Injecting
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class LogEventerAttribute : MethodListenerAttribute
    {
    }
}
