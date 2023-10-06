using ArcadeGames.Views;
using LIB;
using LIB.Settings;
using LIB.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArcadeGames
{
    public class Programm
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();
            app.InitializeComponent();
            app.SplashScreen = new System.Windows.SplashScreen(Assembly.GetExecutingAssembly(), "SplashScreen.png");
            GlobalSettings.LoadGlobalSettings();
            SoundsManagment.ChangeBackground(SoundsManagment.MainBackground);
            app.Run();
        }
    }
}
