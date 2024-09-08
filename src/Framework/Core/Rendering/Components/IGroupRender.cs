


using ZZZ.Framework;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Core.Rendering.Components;
using ZZZ.Framework.Core.Rendering.Entities;

public interface IGroupRender : IRender
{
    public SortLayer LocalLayer { get; }
    void Render(EntityRenderer renderContext);
}

public class GroupRender : Component, IGroupRender
{
    public int Order => 0;
    public SortLayer LocalLayer => SortLayer.All;

    public SortLayer Layer
    {
        get => layer;
        set
        {
            layer = value;
        }
    }

    public event EventHandler<SortLayerArgs> LayerChanged;

    private SortLayer layer = SortLayer.Layer1;

    protected override void Awake()
    {


        base.Awake();
    }

    public void Render(EntityRenderer renderContext)
    {

    }

    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(transformMatrix: Matrix.CreateScale(1.5f) * Matrix.CreateTranslation(10f, 5f, 0f));
        //throw new NotImplementedException();
    }
}