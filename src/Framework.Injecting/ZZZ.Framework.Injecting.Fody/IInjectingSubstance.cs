using Mono.Cecil;

namespace ZZZ.Framework.Injecting
{
    public interface IInjectingSubstance
    {
        void Inject(ModuleDefinition moduleDefinition);
    }
}
