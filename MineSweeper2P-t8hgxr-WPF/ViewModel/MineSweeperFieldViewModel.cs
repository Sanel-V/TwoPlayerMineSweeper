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
        private Int32 fieldValue;
        private Boolean revealed;
        private Boolean hasBomb;
        #endregion

        #region Properties
        public Int32 Value
        { 
            get
            {
                return fieldValue;
            }

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

        public Int32 X { get; set; }
        public Int32 Y { get; set; }

        public DelegateCommand RevealCommand { get; set; }

        #endregion


        #region Constructors

        /*
        public MineSweeperFieldViewModel()
        {
            Value = 0;
            Revealed = 
        }
        */
        public MineSweeperFieldViewModel(MineSweeperField field, Int32 x, Int32 y, DelegateCommand command = null)
        {
            this.field = field;
            Value = field.Value;
            Revealed = field.Revealed;
            HasBomb = field.HasBomb;
            X = x;
            Y = y;
            RevealCommand = command;
        }
        #endregion
    }
}
