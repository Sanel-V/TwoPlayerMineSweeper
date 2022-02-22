using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MineSweeper2P_t8hgxr_Xamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewGamePopup : ContentPage
    {
        public event EventHandler<bool> NewGamePopupDone;
        public int ChosenGameSize
        {
            get
            {
                return (int)this.sizePicker.SelectedItem;
            }
        }
        public NewGamePopup()
        {
            InitializeComponent();

        }
        private void OkButton_Clicked(object sender, EventArgs e)
        {
            OnNewGamePopupDone(true);
            
        }
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            OnNewGamePopupDone(false);
        }

        public void OnNewGamePopupDone(bool startNewGame)
        {
            NewGamePopupDone?.Invoke(this, startNewGame);
        }
    }
}