using System.Collections;
using System.ComponentModel;
using System.IO;

namespace Console.App.Installer.Helper
{
    [RunInstaller(true)]
    public partial class CustomInstaller : System.Configuration.Install.Installer
    {
        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            // Get user input value and update App.config here
            var targetDir = Context.Parameters["targetdir"];
            var connectionstring = Context.Parameters["connectionstring"];

            var targetExe = Path.Combine(targetDir, "Console.App.exe");
            var appConfig = new AppConfig(targetExe);
            appConfig.SetConnectionString("ConnectionString", connectionstring);
            appConfig.Save();
        }
    }
}