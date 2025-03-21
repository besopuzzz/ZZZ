

//namespace ZZZ.Framework
//{
//    public sealed class EngineSystem : System
//    {
//        private readonly List<Transformer> transformers = new List<Transformer>();

//        protected override void Input(IEnumerable<Component> components)
//        {
//            transformers.AddRange(components.Where(x=>x is Transformer).Cast<Transformer>());

//            base.Input(components);
//        }

//        protected override void Output(Component component)
//        {
//            if (component is Transformer transformer)
//                transformers.Remove(transformer);

//            base.Output(component);
//        }

//        public void Reset()
//        {
//            foreach (var transformer in transformers)
//                transformer.HasChanges = false;
//        }
//    }
//}
