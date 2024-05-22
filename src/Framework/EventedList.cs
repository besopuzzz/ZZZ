namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет строго типизированный список <see cref="EventedList{T}"/> с событиями добавления и удаления.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventedList<T> : List<T>
    {
        /// <summary>
        /// Вызывает событие, когда добавили элемент <typeparamref name="T"/>.
        /// </summary>
        public event EventHandler<EventedList<T>, T> ItemAdded;

        /// <summary>
        /// Вызывает событие, когда удалили элемент <typeparamref name="T"/>.
        /// </summary>
        public event EventHandler<EventedList<T>, T> ItemRemoved;

        /// <inheritdoc cref="List{T}.Clear"/>
        public virtual new void Clear()
        {
            foreach (var item in ToArray())
            {
                Remove(item);
            }
        }

        /// <inheritdoc cref="List{T}.Insert"/>
        public virtual new void Insert(int index, T item)
        {
            if (IndexOf(item) != -1)
                return;

            base.Insert(index, item);
            ItemAdded?.Invoke(this, item);
        }

        /// <inheritdoc cref="List{T}.Remove"/>
        public virtual new bool Remove(T item)
        {
            if (item == null)
                return true;

            var result = base.Remove(item);

            if (result)
                ItemRemoved?.Invoke(this, item);

            return result;
        }

        /// <inheritdoc cref="List{T}.Add"/>
        public virtual new void Add(T item)
        {
            Insert(Count, item);
        }
    }
}
