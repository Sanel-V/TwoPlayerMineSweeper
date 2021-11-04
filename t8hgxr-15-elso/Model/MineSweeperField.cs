using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper2Pt8hgxr.Model
{
   
    public class MineSweeperField
    {
        public static Boolean DEBUG = false;

        #region Properties
        public Int32 Value { get; set; } = 0;
        public Boolean Revealed { get; private set; } = false;
        public Boolean HasBomb { get; private set; } = false;

        #endregion

        #region Methods
        public void Reveal() 
        {
            if(!Revealed)
            {
                Revealed = true;
            }
        }

        public void Hide()
        {
            Revealed = false;
        }
        public void PlaceBomb()
        {
            HasBomb = true;
        }

        public override string ToString()
        {
            if(HasBomb)
            {
                if (Revealed)
                {
                    return "X";
                }
                return "x";   
            }
            if(Revealed)
            {
                return Value.ToString();
            }
            return "#";
        }

        #endregion

        #region Constructors
        public MineSweeperField()
        {
            Revealed = DEBUG;
        }
        public MineSweeperField(Boolean revealed = false)
        {
            Revealed = revealed;
        }

        //Value based copy
        public MineSweeperField(MineSweeperField that)
        {
            Revealed = that.Revealed;
            HasBomb = that.HasBomb;
        }

        
        #endregion
    }
}
