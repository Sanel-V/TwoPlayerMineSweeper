using System;
using System.Collections.Generic;
using System.Text;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Persistence;

namespace MineSweeper2P_t8hgxr.ViewModel
{
    public class MineSweeperFieldViewModel : ViewModelBase
    {
        #region Fields

        private Int32 fieldValue;
        private Boolean revealed;
        private Boolean hasBomb;

        private String text;
        private Boolean isEnabled;

        #endregion

        #region Properties

        public Int32 Value
        {
            get { return fieldValue; }
            set
            {
                if (fieldValue != value)
                {
                    fieldValue = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean Revealed
        {
            get
            {
                return revealed;
            }
            set
            { 
                if (revealed != value)
                {
                    revealed = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean HasBomb
        {
            get
            {
                return hasBomb;
            }
            
            set
            {
                if (hasBomb != value)
                {
                    hasBomb = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 Number { get; set; }
        public Int32 X { get; set; }
        public Int32 Y { get; set; }

        public Boolean IsEnabled
        {
            get
            {
                //If revealed, disable
                return isEnabled;
            }
            set
            {
                if(isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public String Text 
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Commands
        public DelegateCommand RevealCommand { get; set; }

        #endregion

        #region Constructors

        public MineSweeperFieldViewModel(
            Int32 number,Int32 x, Int32 y, 
            DelegateCommand command = null)
        {
            Number = number;
            X = x;
            Y = y;
            RevealCommand = command;
            Text = changeText();

        }
        #endregion

        #region Methods

        public String changeText()
        {
            if (Revealed)
            {
                if (HasBomb)
                {
                    return "X";
                }
                return Value != 0 ? Value.ToString() : "";
            }
            return "";
        }
        #endregion

    }
}
