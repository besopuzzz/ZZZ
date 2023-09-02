using ZZZ.Framework.Monogame.Audio;
using ZZZ.Framework.Monogame.FarseerPhysics.Components;
using ZZZ.Framework.Monogame.FarseerPhysics.Diagnostics.Components;
using ZZZ.Framework.Monogame.Rendering;
using ZZZ.Framework.Monogame.Updating;

namespace ZZZ.Framework.Monogame.Extentions
{
    public static class SceneExtentions
    {
        public static Scene UseFarseer(this Scene scene)
        {
            bool isDebudMode = false;

#if DEBUG
            isDebudMode = true;
#endif
            scene.AddComponent(new WorldController() { Debug = isDebudMode, DrawOrder = 1 });


            return scene;
        }
        public static Scene UseUpdateSystem(this Scene scene)
        {
            scene.AddComponent(new UpdateController() );

            return scene;
        }
        public static Scene UseSoundSystem(this Scene scene)
        {
            scene.AddComponent(new SoundController());

            return scene;
        }
        public static Scene UseRenderingSystem(this Scene scene)
        {
            scene.AddComponent(new RenderController());

            return scene;
        }
        public static Scene UseZZZ(this Scene scene)
        {
            scene.
                UseFarseer().
                UseUpdateSystem().
                UseRenderingSystem().
                UseSoundSystem();

            return scene;
        }
    }
}
