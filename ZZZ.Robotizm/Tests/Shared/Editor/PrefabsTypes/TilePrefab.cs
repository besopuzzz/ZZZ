using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Transforming;

public class TilePrefab : IPrefabView
{
    public string Name { get; set; }

    public Sprite Preview { get; set; }
    public Transform2D Local { get; set; }
}