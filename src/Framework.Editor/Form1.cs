using ZZZ.Framework;
using ZZZ.Framework.Core;
using ZZZ.Framework.Core.Registrars;
using ZZZ.Framework.Core.Transforming;
using ZZZ.Framework.Rendering.Components;

namespace Framework.Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            GameManagerSettings gameSettings = new GameManagerSettings();

            gameSettings.Registrars.Add(new InitializeRegistrar());
            gameSettings.Registrars.Add(new UpdateRegistrar());
            gameSettings.Registrars.Add(new PhysicRegistrar());
            //gameSettings.Registrars.Add(new RenderRegistrar());
            gameSettings.Registrars.Add(new WorldRenderer());
            gameSettings.Registrars.Add(new UIRegistrar());
            gameSettings.Registrars.Add(new TransformerRegistrar());

            var main = new GameManager(viewControl1.ViewControl, gameSettings);



            SceneLoader.Load(new Scene());

            GameObject gameObject = new GameObject();
            //var gb = gameObject.AddGameObject(new GameObject()).AddComponent<TestComponent>();
            SceneLoader.CurrentScene.AddGameObject(gameObject);

            SceneLoader.CurrentScene.AddGameObject(new GameObject()).AddComponent<Camera>().Owner.AddComponent<Transformer>().Local = new Transform2D(700, 800);
        }
    }
}
