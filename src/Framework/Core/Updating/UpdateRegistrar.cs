using ZZZ.Framework.Core.Registrars;
using ZZZ.Framework.Core.Updating.Components;

namespace ZZZ.Framework.Core.Updating
{
    /// <summary>
    /// Представляет регистратор, который вызывает метод обновления <see cref="IUpdateComponent.Update(GameTime)"/> у компонента.
    /// </summary>
    /// <remarks>Вызов обновления будет выполняться согласно порядку экземпляра <see cref="UpdateRegistrar.OrderTypes"/>
    /// и включенному свойству <see cref="IComponent.Enabled"/>.</remarks>
    public class UpdateRegistrar : BaseRegistrar<IUpdateComponent>, IAnyRegistrar<IUpdateComponent>
    {
        /// <summary>
        /// Получает экземпляр сортировщика обновляемых компонентов.
        /// </summary>
        public UpdateOrderList OrderTypes { get; }

        private readonly UpdateComponents updateComponentZs;

        /// <summary>
        /// Представляет экземпляр обновляющего регистратора.
        /// </summary>
        public UpdateRegistrar()
        {
            OrderTypes = new UpdateOrderList();
            updateComponentZs = new UpdateComponents();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                updateComponentZs.Clear();
            }

            base.Dispose(disposing);
        }

        void IAnyRegistrar<IUpdateComponent>.Reception(IUpdateComponent component)
        {
            updateComponentZs.Add(new UpdateComponent(OrderTypes.Get(component.GetType()), component));
        }

        void IAnyRegistrar<IUpdateComponent>.Departure(IUpdateComponent component)
        {
            updateComponentZs.Remove(updateComponentZs[x => x.Component == component]);
        }

        protected override void Update(GameTime gameTime)
        {
            updateComponentZs.Invalidate();

            foreach (var item in updateComponentZs)
            {
                item.Update(gameTime);
            }
        }
    }
}
