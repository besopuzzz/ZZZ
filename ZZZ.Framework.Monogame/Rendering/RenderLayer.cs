namespace ZZZ.Framework.Monogame.Rendering
{
    [Flags]
    public enum RenderLayer
    {
        All = First | Second | Third | Fourth | Fivth | Sixth | Seventh | Eighth | Ninth | Tenth,
        First = 1 << 0,
        Second = 1 << 1,
        Third = 1 << 2,
        Fourth = 1 << 3,
        Fivth = 1 << 4,
        Sixth = 1 << 5,
        Seventh = 1 << 6,
        Eighth = 1 << 7,
        Ninth = 1 << 8,
        Tenth = 1 << 9,
    }
}
