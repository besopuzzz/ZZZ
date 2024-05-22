namespace ZZZ.Framework.Assets.Physics
{
    public sealed class Material : Asset
    {
        /// <summary>
        /// Трение материала.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Friction { get; set; } = 0.1f;

        /// <summary>
        /// Упругость материала.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Restitution { get; set; } = 0.0001f;

        /// <summary>
        /// Плотность материала.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Density { get; set; } = 1f;
    }
}
