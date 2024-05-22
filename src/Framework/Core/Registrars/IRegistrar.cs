using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    public interface IRegistrar
    {
        Type Target { get; }
        internal GameManager GameManager { get; set; }
        internal Scene Scene { get; set; }
        internal void RegistrationObject(IComponent component);
        internal void DeregistrationObject(IComponent component);
    }
}
