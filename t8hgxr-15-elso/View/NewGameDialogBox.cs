using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MineSweeper2Pt8hgxr.View
{
    public partial class NewGameDialogBox : Form
    {
        public Int32 Result { get; set; } = 6;
        public NewGameDialogBox()
        {
            InitializeComponent();
        }

        private void dialogOKButton_Click(object sender, EventArgs e)
        {
            if (boardSizeChooser.Text.Equals("Small"))
            {
                Result = 6;
            }else
            {
                if (boardSizeChooser.Text.Equals("Medium"))
                {
                    Result = 10;
                }
                else
                {
                    if(boardSizeChooser.Text.Equals("Large"))
                    {
                        Result = 16;
                    }
                }
                
            }
           

        }

        private void dialogBoxCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
