namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет метод, обрабатывающий события, с указанными типами данных.
    /// </summary>
    /// <typeparam name="TSender">Тип отправителя.</typeparam>
    /// <typeparam name="TEventArgs">Тип параметров.</typeparam>
    /// <param name="sender">Экземпляр отправителя.</param>
    /// <param name="e">Экземпляр параметров.</param>
    public delegate void EventHandler<TSender, TEventArgs>(TSender sender, TEventArgs e);
}
