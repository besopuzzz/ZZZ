namespace ZZZ.Framework.Attributes
{
    public interface IComponentAttribute<TAttribute>
        where TAttribute : IComponentAttribute<TAttribute>
    {
        void Initialize(AttributeInfo<TAttribute> attributeInfo);
    }
}
