//using ZZZ.Framework.Components;

//namespace ZZZ.Framework.Assets.Pipeline.Readers
//{
//    public class GameObjectReader : ContentTypeReader<GameContainer>
//    {
//        protected override GameObject Read(ContentReader input, GameContainer existingInstance)
//        {
//            if (existingInstance == null)
//                existingInstance = Activator.CreateInstance(TargetType, true) as GameContainer;

//            existingInstance.Name = input.ReadObject<string>();
//            existingInstance.Enabled = input.ReadObject<bool>();

//            foreach (var component in input.ReadObject<IEnumerable<Component>>())
//            {
//                existingInstance.AddComponent(component);
//            }

//            foreach (var container in input.ReadObject<IEnumerable<GameObject>>())
//            {
//                existingInstance.AddGameObject(container);
//            }

//            return existingInstance;
//        }
//    }
//}
