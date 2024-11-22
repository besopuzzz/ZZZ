using ZZZ.Framework.Components;
using ZZZ.Framework.Designing.UnityStyle.Components;

namespace ZZZ.Framework.Designing.UnityStyle.Systems
{
    /// <summary>
    /// Представляет абстрактный класс анализаторной системы Unity стиля.
    /// Система проверяет компоненты и пропускает дальше только активные компоненты <see cref="Component.Enabled"/>.
    /// </summary>
    public abstract class StartStopSystem : System
    {
        private readonly List<Component> enabled = new List<Component>();
        private readonly List<Component> toAdd = new List<Component>();
        private readonly List<Component> passed = new List<Component>();

        protected void Validate()
        {
            CheckEnabled();

            PassEnabled();

            CheckAndPassDisabled();
        }

        protected override void Input(IEnumerable<Component> components)
        {
            toAdd.AddRange(components);
        }

        protected override void Output(Component component)
        {
            if (toAdd.Remove(component)) // Not passed. Added and removed on one frame.
                return;

            base.Output(component);

            if (component is IStopComponent stopComponent)
                stopComponent.Stop();

            passed.Remove(component);
        }

        protected override void Dispose(bool disposing)
        {
            enabled.Clear();
            toAdd.Clear();
            passed.Clear();

            base.Dispose(disposing);
        }

        private void CheckEnabled()
        {
            if (toAdd.Count > 0) // Check new Components
            {
                int index = 0;

                while (index < toAdd.Count)
                {
                    var component = toAdd[index];

                    if (!component.Enabled) 
                    {
                        index++; // Disabled? Iterate next

                        continue;
                    }

                    // Enabled component

                    enabled.Add(component); // It enabled component and ready to pass

                    toAdd.RemoveAt(index); // Now component is not "new"
                }
            }

        }

        private void PassEnabled()
        {
            foreach (var component in enabled.Where(x => x is IStartupComponent).Cast<IStartupComponent>()) // For every enabled call Start
                component.Startup();

            base.Input(enabled); // Pass to other system (Update, Render, etc)

            passed.AddRange(enabled); // Now components is passed

            enabled.Clear(); // Forgot enabled
        }

        private void CheckAndPassDisabled()
        {
            if (passed.Count == 0) // Check passed on disabled state
                return;

            int index = 0;

            while (index < passed.Count)
            {
                var component = passed[index];

                if (component.Enabled)
                {
                    index++;

                    continue; // Enabled? Iterate next
                }

                // Disabled component

                Output(component); // Pass remove (Update, Render, etc)

                toAdd.Add(component); // Now it is "new" component and need pass all path again
            }
        }
    }
}
