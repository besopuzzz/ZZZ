using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    public class Scene : GameObject
    {
        internal GameManager GameManager { get; set; }

        protected internal override void RegistrationComponent<T>(T component)
        {
            GameManager.RegistrationComponent(component);
        }

        protected internal override void DeregistrationComponent<T>(T component)
        {
            GameManager.UnregistrationComponent(component);
        }

        protected internal override Scene GetCurrentScene()
        {
            return this;
        }

        internal void Run()
        {
            Awake();
        }

        public static class ModuleInitializer
        {
            public static void Initialize()
            {

            }
        }


        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Scene"/>.
        /// </summary>
        public Scene()
        {

        }
    }
}