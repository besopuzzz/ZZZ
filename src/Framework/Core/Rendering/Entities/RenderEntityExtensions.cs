//using ZZZ.Framework.Core.Rendering.Components;

//namespace ZZZ.Framework.Core.Rendering.Entities
//{
//    public static class RenderEntityExtensions
//    {
//        public static IEnumerable<RenderEntity> Sort(this IEnumerable<RenderEntity> renderEntities)
//        {
//            var entities = new List<RenderEntity>(renderEntities);

//            entities.Sort((x, y) => Comparer<int>.Default.Compare(x.Order, y.Order));

//            return entities;
//        }

//        public static void RenderFromCamera(this IEnumerable<RenderEntity> renderEntities, ICamera camera, SpriteBatch spriteBatch)
//        {
//            foreach (var sortLayer in Enum.GetValues<SortLayer>())
//            {
//                if(sortLayer == SortLayer.None | !camera.Layer.HasFlag(sortLayer))
//                    continue;

//                camera.Render(spriteBatch);

//                foreach (var item in renderEntities)
//                {
//                    if (item.SortLayer == SortLayer.None | !item.Enabled | !camera.Layer.HasFlag(item.SortLayer))
//                        continue;

//                    item.Render(spriteBatch, camera);
//                }

//                spriteBatch.End();
//            }
//        }
//    }
//}
