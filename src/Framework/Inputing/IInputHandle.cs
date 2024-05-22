using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZZZ.Framework.Inputing
{
    public interface IInputHandle
    {
        bool Clicked { get; }
        Vector2 Position { get; }
    }
}
