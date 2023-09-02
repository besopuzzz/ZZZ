using Microsoft.Xna.Framework;
using ZZZ.Framework;
using ZZZ.Framework.Monogame.Extentions;

public class Rectangler : Component, IRectangler
{
    public Rectangle Local
    {
        get => local;
        set
        {
            if (local == value)
                return;

            local = value;
            LocalChanged?.Invoke(this, local);

            world = local.Sum(parent.World);
            WorldChanged?.Invoke(this, world);
        }
    }
    public Rectangle World
    {
        get => world;
    }
    public Rectangler Parent => parent;

    IRectangler IRectangler.Parent => parent;

    public event RectanglerEventHandler LocalChanged;
    public event RectanglerEventHandler WorldChanged;

    private Rectangler parent;
    private Rectangle local = new Rectangle();
    private Rectangle world = new Rectangle();

    private static readonly Rectangler empty = new Rectangler(new Rectangle(0,0,800,600));

    public Rectangler()
    {
        parent = empty;
    }
    public Rectangler(Rectangle rectangle) : this()
    {
        local = rectangle;
        world = rectangle;
    }

    protected override void Startup()
    {
        parent = Owner?.Owner?.GetComponent<Rectangler>();

        if (parent == null)
            parent = empty;

        AddWorld();
        parent.WorldChanged += Parent_WorldChanged;

        base.Startup();
    }
    protected override void Shutdown()
    {
        parent.WorldChanged -= Parent_WorldChanged;
        RemoveWorld();

        base.Shutdown();
    }

    private void Parent_WorldChanged(IRectangler sender, Rectangle args)
    {
        world = local.Sum(parent.World);
        WorldChanged?.Invoke(this, world);
    }
    private void AddWorld()
    {
        world = local.Sum(parent.World);
    }
    private void RemoveWorld()
    {
        world = local;
    }
    public void SetWorld(Rectangle world)
    {
        this.world = world;
    }
    public void SetLocal(Rectangle local)
    {
        this.local = local;
    }
}