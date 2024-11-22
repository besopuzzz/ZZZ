//using ZZZ.Framework.Components;

//namespace ZZZ.Framework.Assets.Pipeline.Readers
//{
//    public class SceneReader : ContentTypeReader<Scene>
//    {
//        protected override Scene Read(ContentReader input, Scene existingInstance)
//        {
//            if (existingInstance == null)
//                existingInstance = new Scene();

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
