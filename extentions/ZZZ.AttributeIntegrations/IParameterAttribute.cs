namespace ZZZ.AttributeIntegrations
{
    public interface IParameterAttribute
    {
        public Type[] Parameters { get; }
    }
    public interface IParameterAttribute<T> : IParameterAttribute
    {

    }
    public interface IParameterAttribute<T1, T2> : IParameterAttribute
    {

    }
}