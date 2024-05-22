namespace ZZZ.Framework
{
    /// <summary>
    /// <inheritdoc/>
    /// При сериализации в XML бибилиотеки Monogame элементы коллекции
    /// записывается как <see cref="ContentSerializerAttribute.SharedResource"/> со значением true.
    /// </summary>
    /// <typeparam name="T"><inheritdoc/></typeparam>
    public class SharedList<T> : List<T>
    {
    }
}
