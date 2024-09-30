using System.ComponentModel;

namespace ZZZ.Framework.Assets.Physics
{
    public sealed class Material : Asset, INotifyPropertyChanged
    {
        /// <summary>
        /// Трение материала.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Friction { get; set; } = 1f;

        /// <summary>
        /// Упругость материала.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Restitution { get; set; } = 0f;

        /// <summary>
        /// Плотность материала.
        /// </summary>
        [ContentSerializer(Optional = true)]
        public float Density { get; set; } = 0.1f;

        public static Material Default {
            get
            {
                if(defaultMaterial == null)
                    defaultMaterial = new Material();

                return defaultMaterial;
            }
        }

        private static Material defaultMaterial = new Material();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
