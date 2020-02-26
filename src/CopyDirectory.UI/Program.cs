using System;
using System.Windows.Forms;
using Unity;

namespace CopyDirectory.UI
{
    static class Program
    {

        private static IUnityContainer _container;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            _container = UnityConfig.RegisterDependencies(new UnityContainer());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(_container.Resolve<Main>());
        }
    }
}
