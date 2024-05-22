using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Dynamics;

namespace ZZZ.Framework.Core.Physics
{
    internal static class FixtureExtentions
    {
        public static Fixture CreateWithoutCloneShape(Shape shape)
        {
            var fixture = (Fixture)Activator.CreateInstance(typeof(Fixture), true)!;

            Type type = typeof(Fixture);
            type.GetProperty(nameof(fixture.Shape))!.SetValue(fixture, shape);
            type.GetProperty(nameof(fixture.Proxies))!.SetValue(fixture, new FixtureProxy[shape.ChildCount]);

            return fixture;
        }

    }
}
