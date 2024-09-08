namespace ZZZ.Framework.Assets
{
    public class Asset : Disposable, IAsset
    {
        [ContentSerializerIgnore]
        public virtual string Name { get; set; }
    }
}
