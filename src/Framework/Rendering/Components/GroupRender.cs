using ZZZ.Framework;
using ZZZ.Framework.Components;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Rendering.Components;

public class GroupRender : Component, IGroupRenderer
{
    public SortLayer LayerMask => SortLayer.All;

    public RenderContext Context => context;

    public int Order { get; set; }

    public SortLayer Layer { get; set; }

    private RenderContext context;

    protected override void Awake()
    {
        context = Services.Get<IRenderManager>().CreateInstance();

        base.Awake();
    }

    void IRenderer.Render(IRenderProvider provider)
    {
        context.Render(RenderContext.RenderMode.ToOneLayer, Camera.MainCamera);
    }

}