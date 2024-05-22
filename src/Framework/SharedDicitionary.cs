namespace ZZZ.Framework
{
    /// <summary>
    /// <inheritdoc/>. При сериализации в XML бибилиотеки Monogame элементы коллекции
    /// записывается как <see cref="ContentSerializerAttribute.SharedResource"/> со значением true.
    /// </summary>
    /// <typeparam name="T"><inheritdoc/></typeparam>
    /// <typeparam name="K"><inheritdoc/></typeparam>
    public class SharedDicitionary<T,K> : Dictionary<T, K>
    {
    }

}
