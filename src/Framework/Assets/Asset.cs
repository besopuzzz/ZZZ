namespace ZZZ.Framework.Assets
{
    public class Asset : Disposable, IAsset
    {
        [ContentSerializerIgnore]
        public string Name { get; set; } = "";
    }
}
