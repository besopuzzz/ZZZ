using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Rendering;

public class GroupRender : RenderComponent
{
    public SortLayer LayerMask => SortLayer.All;

    internal override IList<RenderComponent> DownNeighbors => empty;

    private RenderContext localContext;
    private static IList<RenderComponent> empty = Enumerable.Empty<RenderComponent>().ToList();

    protected override void Render(RenderContext renderContext)
    {
        if (localContext == null)
            localContext = new RenderContext(renderContext);

        foreach (var child in base.DownNeighbors)
            CheckAndAddToQueue(child);

        localContext.RenderQueue(RenderContext.RenderMode.ToOneLayer);
    }

    void CheckAndAddToQueue(RenderComponent component)
    {
        if (LayerMask.HasFlag(component.Layer) && component.Enabled)
            localContext.AddToQueue(component);

        foreach (var child in component.DownNeighbors)
            CheckAndAddToQueue(child);
    }
}