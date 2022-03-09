using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper2Pt8hgxr.Model.EventArguments
{
    public class MineSweeperGameOverEventArgs : EventArgs
    {
        public Boolean GameTied { get; private set; } = false;

        public PlayerEnum LastPlayer { get; private set; }

        public MineSweeperGameOverEventArgs(PlayerEnum lastPlayer, Boolean gameTied = false)
        {
            GameTied = gameTied;
            LastPlayer = lastPlayer;
        }
    }
}
