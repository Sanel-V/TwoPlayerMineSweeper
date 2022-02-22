using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MineSweeper2P_t8hgxr.ViewModel;

namespace MineSweeper2P_t8hgxr_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        public double TileWidth
        {
            get; set;

        }
        public GamePage()
        {
            InitializeComponent();
        }

        public void SetTileSize()
        {

        }
    }
}