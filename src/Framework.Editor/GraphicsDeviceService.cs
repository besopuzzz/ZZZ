using Microsoft.Xna.Framework.Graphics;

internal class GraphicsDeviceService : IGraphicsDeviceService
{
    public GraphicsDevice GraphicsDevice { get; private set; }

    public event EventHandler<EventArgs> DeviceCreated;
    public event EventHandler<EventArgs> DeviceDisposing;
    public event EventHandler<EventArgs> DeviceReset;
    public event EventHandler<EventArgs> DeviceResetting;

    private static GraphicsDeviceService singletonInstance;
    private static int referenceCount;
    private readonly PresentationParameters parameters;

    private GraphicsDeviceService(IntPtr windowHandle, int width, int height, GraphicsProfile graphicsProfile)
    {
        parameters = new PresentationParameters
        {
            BackBufferWidth = Math.Max(width, 1),
            BackBufferHeight = Math.Max(height, 1),
            BackBufferFormat = SurfaceFormat.Color,
            DepthStencilFormat = DepthFormat.Depth24,
            DeviceWindowHandle = windowHandle,
            PresentationInterval = PresentInterval.Immediate,
            IsFullScreen = false
        };

        GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, graphicsProfile, parameters);
    }

    public static GraphicsDeviceService AddRef(IntPtr windowHandle, int width, int height, GraphicsProfile graphicsProfile)
    {
        if (Interlocked.Increment(ref referenceCount) == 1)
        {
            singletonInstance = new GraphicsDeviceService(windowHandle, width, height, graphicsProfile);
        }
        return singletonInstance;
    }

    public void Release(bool disposing)
    {
        if (Interlocked.Decrement(ref referenceCount) != 0)
            return;
        if (disposing)
        {
            DeviceDisposing?.Invoke(this, EventArgs.Empty);
            GraphicsDevice.Dispose();
        }
        GraphicsDevice = null;
    }

    public void ResetDevice(int width, int height)
    {
        DeviceResetting?.Invoke(this, EventArgs.Empty);
        parameters.BackBufferWidth = width;
        parameters.BackBufferHeight = height;
        GraphicsDevice.Reset(parameters);
        DeviceReset?.Invoke(this, EventArgs.Empty);
    }
}