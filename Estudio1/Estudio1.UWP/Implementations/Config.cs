
using Xamarin.Forms;

[assembly: Dependency(typeof(Estudio1.UWP.Implementations.Config))]

namespace Estudio1.UWP.Implementations
{
    using Estudio1.Interfaces;
    public class Config : IConfig
    {
        string directoryDB;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                    directoryDB = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                return directoryDB;
            }
        }
    }
}