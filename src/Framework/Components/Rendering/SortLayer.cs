namespace ZZZ.Framework.Core.Rendering
{
    [Flags]
    public enum SortLayer
    {
        Layer1 = 0,
        Layer2 = 1,
        Layer3 = Layer2 << 1,
        Layer4 = Layer3 << 1,
        Layer5 = Layer4 << 1,
        Layer6 = Layer5 << 1,
        Layer7 = Layer6 << 1,
        Layer8 = Layer7 << 1,
        Layer9 = Layer8 << 1,
        Layer10 = Layer9 << 1,
        Layer11 = Layer10 << 1,
        Layer12 = Layer11 << 1,
        Layer13 = Layer12 << 1,
        Layer14 = Layer13 << 1,
        Layer15 = Layer14 << 1,
        Layer16 = Layer15 << 1,
        Layer17 = Layer16 << 1,
        Layer18 = Layer17 << 1,
        Layer19 = Layer18 << 1,
        Layer20 = Layer19 << 1,
        Layer21 = Layer20 << 1,
        Layer22 = Layer21 << 1,
        Layer23 = Layer22 << 1,
        Layer24 = Layer23 << 1,
        Layer25 = Layer24 << 1,
        Layer26 = Layer25 << 1,
        Layer27 = Layer26 << 1,
        Layer28 = Layer27 << 1,
        Layer29 = Layer28 << 1,
        Layer30 = Layer29 << 1,
        Layer31 = Layer30 << 1,
        Layer32 = Layer31 << 1,
        All = int.MaxValue
    }
}
