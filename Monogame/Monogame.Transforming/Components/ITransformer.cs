namespace ZZZ.Framework.Monogame.Transforming.Components
{
    public interface ITransformer : IComponent
    {
        Transform2D Local { get; set; }
        Transform2D World { get; set; }
        ITransformer Parent { get; }

        event TransformerEventHandler LocalChanged;
        event TransformerEventHandler WorldChanged;
        
        void SetWorld(Transform2D world);
        void SetLocal(Transform2D local);
    }

    public delegate void TransformerEventHandler(ITransformer sender, Transform2D args);
}
