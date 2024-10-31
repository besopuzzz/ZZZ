using ZZZ.Framework.Components;

namespace ZZZ.Framework.Assets.Pipeline.Readers
{
    public class GameObjectReader : ContentTypeReader<GameObject>
    {
        protected override GameObject Read(ContentReader input, GameObject existingInstance)
        {
            if (existingInstance == null)
                existingInstance = Activator.CreateInstance(TargetType, true) as GameObject;

            existingInstance.Name = input.ReadObject<string>();
            existingInstance.Enabled = input.ReadObject<bool>();

            foreach (var component in input.ReadObject<IEnumerable<Component>>())
            {
                existingInstance.AddComponent(component);
            }

            foreach (var container in input.ReadObject<IEnumerable<GameObject>>())
            {
                existingInstance.AddGameObject(container);
            }

            return existingInstance;
        }
    }
}
