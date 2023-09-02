


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using ZZZ.Framework;
using ZZZ.Framework.Monogame.Content;
using ZZZ.Framework.Monogame.Rendering;
using ZZZ.Framework.Monogame.Rendering.Components;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Tiled.Components;
using ZZZ.Framework.Monogame.Tiled.Components.Rendering;
using ZZZ.Framework.Monogame.Tiled.Content;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

public class PrefabsMenuRenderer : UpdateComponent
{
    public List<IPrefabView> Prefabs { get; set; }
    public Point Size { get; set; } = new Point(10, 8);

    private Transformer transformer;
    private Sprite menuSprite;
    private Font font;
    private Point? selectedPoint = null;
    private TextRenderer textRenderer;
    private Transformer textTransformer;
    private Tilemap tilemap;
    private Tilemap backgroundMap;

    protected override void Startup()
    {
        menuSprite = AssetManager.Instance.Load<Sprite>("UI/menu");
        transformer = GetComponent<Transformer>();
        font = new Font(AssetManager.Instance.Load<SpriteFont>("DiagnosticsFont"));
        tilemap = new Tilemap();
        backgroundMap = new Tilemap();
        textRenderer = new TextRenderer(font);



        Container gameObject = new Container();
        gameObject.Name = "Menu";

        var gb1 = gameObject.AddContainer(new Container());
        gb1.Name = "Background menu";
        gb1.AddComponent(backgroundMap);

        var gb2 = gameObject.AddContainer(new Container());
        gb2.Name = "Prefabs menu";
        gb2.AddComponent(tilemap);


        AddContainer(gameObject);


        var gb3 = new Container();
        gb3.AddComponent(textRenderer);

        AddContainer(gb3);
        textTransformer = gb3.GetComponent<Transformer>();


        for (int y = 0; y < Size.Y; y++)
        {
            for (int x = 0; x < Size.X; x++)
            {
                Tile backgroundtile = new Tile(menuSprite[2]);
                backgroundMap.AddTile(new Point(x, y), backgroundtile);

                int index = y * 10 + x;

                if (index >= Prefabs.Count)
                    continue;

                var prefab = Prefabs[index];

                backgroundtile.Tag = prefab;

                Tile tile = new Tile(prefab.Preview);
                tilemap.AddTile(new Point(x, y), tile);
            }
        }

        base.Startup();
    }


    protected override void Update(GameTime gameTime)
    {
        MouseState mouse = Mouse.GetState();

        Vector2 position = (Transform2D.CreateTranslation( mouse.Position.ToVector2()) / transformer.World).Position + backgroundMap.GetCellSize() / 2;
        Point point = backgroundMap.GetPointFromPosition(position);

        Tile tile = backgroundMap.GetTile(point);

        if (point == selectedPoint)
        {
            textTransformer.Local = Transform2D.CreateTranslation(new Vector2(-backgroundMap.GetCellSize().X / 2, 10f)) * new Transform2D(position);
            return;
        }

        if (selectedPoint != null)
        {
            var selectedTile = backgroundMap.GetTile(selectedPoint.Value);

            if (selectedTile != tile)
            {
                selectedTile.Color = Color.White;
                backgroundMap.Refresh(selectedPoint.Value);
                textRenderer.Text.Clear();
                selectedPoint = null;
            }
        }

        if (tile == null)
            return;


        selectedPoint = point;
        tile.Color = Color.Gray;
        backgroundMap.Refresh(backgroundMap.GetPointFromPosition(position));

        IPrefabView prefabView = tile.Tag as IPrefabView;

        if (prefabView == null)
            return;

        textRenderer.Text.Append(prefabView.Name);
        textRenderer.Origin = textRenderer.Font.MeasureString(textRenderer.Text) / 2f;


        base.Update(gameTime);
    }
}