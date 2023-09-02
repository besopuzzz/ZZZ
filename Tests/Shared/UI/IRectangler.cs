using Microsoft.Xna.Framework;
using ZZZ.Framework;

public interface IRectangler : IComponent
{
    Rectangle Local { get; set; }
    Rectangle World { get;}
    IRectangler Parent { get; }

    event RectanglerEventHandler LocalChanged;
    event RectanglerEventHandler WorldChanged;

    void SetWorld(Rectangle world);
    void SetLocal(Rectangle local);
}

public delegate void RectanglerEventHandler(IRectangler sender, Rectangle args);