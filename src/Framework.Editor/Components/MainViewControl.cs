namespace ZZZ.Framework.Editor.Components
{
    public partial class MainViewControl : UserControl
    {
        public ViewControl ViewControl => viewControl1;

        public MainViewControl()
        {
            InitializeComponent();
        }

        protected override void OnCreateControl()
        {
            if (DesignMode)
                return;

            var scene = SceneLoader.CurrentScene;

            var go = scene.FindComponent<Transformer>();

            Binding binding = new Binding("Text", go, "Local");

            toolStripStatusLabel1.DataBindings.Add(binding);

            base.OnCreateControl();
        }
    }
}
