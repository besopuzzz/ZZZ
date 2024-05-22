namespace ZZZ.Framework.Core.Rendering
{
    public sealed class SortLayerArgs
    {
        public SortLayer OldLayer { get; }
        public SortLayer NewLayer { get; }

        public SortLayerArgs(SortLayer oldLayer, SortLayer newLayer)
        {
            OldLayer = oldLayer;
            NewLayer = newLayer;
        }
    }
}
