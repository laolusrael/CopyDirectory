using CopyDirectory.Library;
using NLog;
using Unity;

namespace CopyDirectory.UI
{
    public class UnityConfig
    {
        /// <summary>
        /// A central static method to register all dependencies
        /// </summary>
        /// <param name="container">Unity container instance</param>
        /// <returns>Returns the Unity Container with dependency registry</returns>
        public static IUnityContainer RegisterDependencies(IUnityContainer container)
        {
            container.RegisterType<ICopier, Copier>();
            container.RegisterFactory<ILogger>(factory => LogManager.GetCurrentClassLogger());
            return container;
        }
    }
}
