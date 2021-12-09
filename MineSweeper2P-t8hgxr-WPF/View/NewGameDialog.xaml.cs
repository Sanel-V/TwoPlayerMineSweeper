using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MineSweeper2P_t8hgxr_WPF.View
{
    /// <summary>
    /// Interaction logic for NewGameDialog.xaml
    /// </summary>
    public partial class NewGameDialog : Window
    {
        public Int32 GameSize
        {
            get
            {
                if((bool)radioSmall.IsChecked)
                {
                    return 6;
                }else
                {
                    //if medium is checked, otherwise big is checked
                    return (bool)radioMedium.IsChecked ? 10 : 16;
                }
                
            }
        }
        public NewGameDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
