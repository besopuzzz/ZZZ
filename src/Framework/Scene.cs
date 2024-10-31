using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    public class Scene : GameObject
    {
        public override GameObject Owner { get => null; internal set { } }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Scene"/>.
        /// </summary>
        public Scene()
        {

        }
    }
}