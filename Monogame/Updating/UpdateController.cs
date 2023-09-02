using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Monogame.Updating
{
    public class UpdateController : MonogameController<IUpdateComponent>
    {
        private UpdateComponents components = new UpdateComponents();

        protected override void Dispose(bool disposing)
        {
            components.Clear();

            base.Dispose(disposing);
        }

        protected override void Reception(IUpdateComponent component)
        {
            if (!components.Contains(component))
                components.Add(component);

            base.Reception(component);
        }

        protected override void Departure(IUpdateComponent component)
        {
            components.Remove(component);

            base.Departure(component);
        }

        protected override void Startup()
        {
            base.Startup();
        }

        protected override void Update(GameTime gameTime)
        {
            components.Update(gameTime);
        }
    }
}
