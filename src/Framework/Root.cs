using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Assets.Pipeline.Readers;
using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    internal sealed class Root : Container
    {
        public override bool Enabled { get => true; set { } }

        private IGameInstance game;

        public Root(IGameInstance gameInstance)
        {
            _ = new AssetManager(gameInstance.Services.GetService<IAssetProvider>());
            _ = new SceneLoader(this);
        }

        protected override void OnComponentAdding<T>(T component)
        {
            if (component is RootComponent rootComponent)
                rootComponent.InternalGameInstance = game;

            base.OnComponentAdding(component);
        }

        protected override void OnComponentRemoved<T>(T component)
        {
            base.OnComponentRemoved(component);

            if (component is RootComponent rootComponent)
                rootComponent.InternalGameInstance = game;
        }
    }
}
