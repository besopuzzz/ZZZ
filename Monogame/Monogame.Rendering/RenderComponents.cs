using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.Rendering
{
    internal class RenderComponents : IDisposable
    {
        public RenderTarget2D RenderTarget2D { get;private set; }

        private GraphicsDevice graphicsDevice;
        private struct DrawableJournalEntry
        {
            private readonly int AddOrder;

            public readonly IRenderComponent Component;

            public DrawableJournalEntry(IRenderComponent component, int addOrder)
            {
                Component = component;
                AddOrder = addOrder;
            }

            public override int GetHashCode()
            {
                return Component.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (!(obj is DrawableJournalEntry))
                {
                    return false;
                }

                return Component == ((DrawableJournalEntry)obj).Component;
            }

            internal static int CompareAddJournalEntry(DrawableJournalEntry x, DrawableJournalEntry y)
            {
                int num = 0;
                if (num == 0)
                {
                    num = x.AddOrder - y.AddOrder;
                }

                return num;
            }
        }

        private readonly List<IRenderComponent> _drawableComponents = new List<IRenderComponent>();

        private readonly List<IRenderComponent> _visibleComponents = new List<IRenderComponent>();

        private bool _isVisibleCacheInvalidated = true;

        private readonly List<DrawableJournalEntry> _addDrawableJournal = new List<DrawableJournalEntry>();

        private readonly List<int> _removeDrawableJournal = new List<int>();

        private int _addDrawableJournalCount;
        private bool disposedValue;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            RenderTarget2D = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, false, SurfaceFormat.Bgra4444, DepthFormat.None);
        }

        public void Begin(SpriteSortMode sortMode = SpriteSortMode.FrontToBack, BlendState blendState = null,
            SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null,
            Effect effect = null, Matrix? transformMatrix = null)
        {
            graphicsDevice.SetRenderTarget(RenderTarget2D);

            graphicsDevice.Clear(Color.Transparent);
            Renderer.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }

        public void Draw()
        {
            if (_removeDrawableJournal.Count > 0)
            {
                ProcessRemoveDrawableJournal();
            }

            if (_addDrawableJournal.Count > 0)
            {
                ProcessAddDrawableJournal();
            }

            if (_isVisibleCacheInvalidated)
            {
                _visibleComponents.Clear();
                for (int i = 0; i < _drawableComponents.Count; i++)
                {
                    if (_drawableComponents[i].Enabled)
                    {
                        _visibleComponents.Add(_drawableComponents[i]);
                    }
                }

                _isVisibleCacheInvalidated = false;
            }

            for (int j = 0; j < _visibleComponents.Count; j++)
            {
                _visibleComponents[j].Draw();
            }

            if (_isVisibleCacheInvalidated)
            {
                _visibleComponents.Clear();
            }
        }
        public void End()
        {
            Renderer.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public void Add(IRenderComponent component)
        {
            _addDrawableJournal.Add(new DrawableJournalEntry(component, _addDrawableJournalCount++));
            _isVisibleCacheInvalidated = true;
        }
        public void Remove(IRenderComponent component)
        {
            if (!_addDrawableJournal.Remove(new DrawableJournalEntry(component, -1)))
            {
                int num = _drawableComponents.IndexOf(component);
                if (num >= 0)
                {
                    component.EnabledChanged -= (sender, value) => Component_VisibleChanged(sender, EventArgs.Empty);
                    _removeDrawableJournal.Add(num);
                    _isVisibleCacheInvalidated = true;
                }
            }
        }
        public bool Contains(IRenderComponent renderComponent) => _drawableComponents.Contains(renderComponent);
        public void Clear()
        {
            for (int i = 0; i < _drawableComponents.Count; i++)
            {
                _drawableComponents[i].EnabledChanged -= (sender, value) => Component_VisibleChanged(sender, EventArgs.Empty);
            }

            _addDrawableJournal.Clear();
            _removeDrawableJournal.Clear();
            _drawableComponents.Clear();
            _isVisibleCacheInvalidated = true;
        }
        private void Component_VisibleChanged(object sender, EventArgs e)
        {
            _isVisibleCacheInvalidated = true;
        }
        private void Component_DrawOrderChanged(object sender, EventArgs e)
        {
            IRenderComponent drawable = (IRenderComponent)sender;
            int item = _drawableComponents.IndexOf(drawable);
            _addDrawableJournal.Add(new DrawableJournalEntry(drawable, _addDrawableJournalCount++));
            drawable.EnabledChanged -= (sender, value) => Component_VisibleChanged(sender, EventArgs.Empty);
            _removeDrawableJournal.Add(item);
            _isVisibleCacheInvalidated = true;
        }
        private void ProcessRemoveDrawableJournal()
        {
            _removeDrawableJournal.Sort();
            for (int num = _removeDrawableJournal.Count - 1; num >= 0; num--)
            {
                _drawableComponents.RemoveAt(_removeDrawableJournal[num]);
            }

            _removeDrawableJournal.Clear();
        }
        private void ProcessAddDrawableJournal()
        {
            _addDrawableJournal.Sort(DrawableJournalEntry.CompareAddJournalEntry);
            _addDrawableJournalCount = 0;
            int i = 0;
            for (int j = 0; j < _drawableComponents.Count; j++)
            {
                if (i >= _addDrawableJournal.Count)
                {
                    break;
                }

                IRenderComponent component = _addDrawableJournal[i].Component;
                component.EnabledChanged += (sender, value) => Component_VisibleChanged(sender, EventArgs.Empty);
                _drawableComponents.Insert(j, component);
                i++;
            }

            for (; i < _addDrawableJournal.Count; i++)
            {
                IRenderComponent component2 = _addDrawableJournal[i].Component;
                component2.EnabledChanged += (sender, value) => Component_VisibleChanged(sender, EventArgs.Empty);
                _drawableComponents.Add(component2);
            }

            _addDrawableJournal.Clear();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RenderTarget2D.Dispose();
            }
            graphicsDevice = null;
            RenderTarget2D = null;

            disposedValue = true;
        }
        ~RenderComponents()
        {
            if (disposedValue)
                return;

            Dispose(disposing: false);
        }

        public void Dispose()
        {
            if (disposedValue)
                return;

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
