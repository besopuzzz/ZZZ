using System.ComponentModel.Design;

namespace ZZZ.Framework
{
    public sealed class Services : Disposable
    {
        internal static Services Instance
        {
            get
            {
                if (instance == null)
                    throw new InvalidOperationException("Services not initialized!");

                return instance;
            }
        }

        private ServiceContainer serviceContainer;
        private static Services instance;

        internal Services(ServiceContainer services) 
        {
            if (instance != null)
                throw new InvalidOperationException("Services already initialize!");

            instance = this;

            serviceContainer = services;
        }

        public static T Get<T>()
        {
            return (T)Instance.serviceContainer.GetService(typeof(T));
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                serviceContainer.Dispose();
            }

            serviceContainer = null;
            instance = null;

            base.Dispose(disposing);
        }
    }
}
