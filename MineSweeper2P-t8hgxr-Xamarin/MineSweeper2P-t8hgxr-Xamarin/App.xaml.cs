using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MineSweeper2P_t8hgxr_Xamarin.View;

namespace MineSweeper2P_t8hgxr_Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new GamePage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
