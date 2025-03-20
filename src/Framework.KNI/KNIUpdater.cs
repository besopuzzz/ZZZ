using Microsoft.Xna.Framework;
using ZZZ.Framework.KNI;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Updating;

namespace ZZZ.Framework.Extensions.SystemComponents
{
    internal sealed class KNIUpdater : GameComponent, IKNISystem
    {
        public System System { get; }
        public ISystemUpdater Updater { get; }

        public KNIUpdater(ISystemUpdater systemUpdater, Game game) : base(game)
        {
            System = systemUpdater as System;
            Updater = systemUpdater;
        }

        public override void Update(GameTime gameTime)
        {
            Updater.Update(gameTime.ElapsedGameTime);
        }
    }
}
