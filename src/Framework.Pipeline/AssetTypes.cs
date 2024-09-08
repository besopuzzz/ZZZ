using System.Collections.ObjectModel;
using System.Reflection;

namespace ZZZ.KNI.Content.Pipeline
{
    public static class AssetTypes
    {
        public static ReadOnlyCollection<Type> Assets
        {
            get
            {
                if (types == null)
                    types = GetTypes();

                return types;
            }
        }

        private static readonly object _lock = new object();
        private static ReadOnlyCollection<Type> types;
        private static ReadOnlyCollection<Type> GetTypes()
        {
            lock (_lock)
            {
                if (types == null)
                {
                    List<Type> list = new List<Type>();
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly assembly in assemblies)
                    {
                        try
                        {
                            Type[] types = assembly.GetTypes();
                            foreach (Type type in types)
                            {
                                if (type == typeof(IAsset))
                                    continue;

                                if (type == typeof(Asset))
                                    continue;

                                if (typeof(IAsset).IsAssignableFrom(type))
                                {
                                    list.Add(type);
                                }
                            }
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            Console.WriteLine("Warning: " + ex.Message);
                        }
                    }

                    types = new ReadOnlyCollection<Type>(list);
                }
            }

            return types;
        }
    }
}
