using System;
using System.Collections.Generic;
using System.Text;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Persistence;

namespace MineSweeper2P_t8hgxr_WPF.ViewModel
{
    public class MineSweeperFieldViewModel : ViewModelBase
    {
        #region Fields
        //Board field to wrap
        private MineSweeperField field;
        #endregion

        #region Properties
        public MineSweeperField Field
        {
            get { return field; }
            /*private set 
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged();
                }
            }*/
        }

        public Int32 Value
        { 
            get
            {
                return field.Value;
            }

            set 
            {
                if (field.Value != value)
                {
                    field.Value = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean Revealed
        {
            get
            {
                return field.Revealed;
            }
            /*
            set
            {
                if (field.Revealed != value)
                {
                    field.Revealed = value;
                    OnPropertyChanged();
                }
            }*/
        }
        public Boolean HasBomb
        {
            get
            {
                return field.HasBomb;
            }
            /*
            set
            {
                if (hasBomb != value)
                {
                    hasBomb = value;
                    OnPropertyChanged();
                }
            }*/
        }

        public Int32 X { get; set; }
        public Int32 Y { get; set; }

        public String Text 
        {
            get
            {
                if(Revealed)
                {
                    if(HasBomb)
                    {
                        return "X";
                    }
                    return Value != 0 ? Value.ToString() : "" ;
                }
                return "";
                
            }
        }
        #endregion

        #region Commands
        public DelegateCommand RevealCommand { get; set; }

        #endregion

        #region Constructors

        public MineSweeperFieldViewModel
            (
            MineSweeperField field, 
            Int32 x, 
            Int32 y, 
            DelegateCommand command = null)
        {
            this.field = field;
            X = x;
            Y = y;
            RevealCommand = command;
        }
        #endregion
    }
}
