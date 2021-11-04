using System;
using System.Collections.Generic;
using System.Text;
using MineSweeper2Pt8hgxr.Model;
namespace MineSweeper2Pt8hgxr.Persistence
{
    public class MineSweeper2PGameState
    {
        public MineSweeperBoard GameBoard { get; private set; }
        public PlayerEnum CurrentPlayer { get; private set; }

        public Int32 BoardSize { get { return GameBoard.BoardSize; } }
        public MineSweeper2PGameState(MineSweeperBoard board, PlayerEnum currentPlayer)
        {
            GameBoard = board;
            CurrentPlayer = currentPlayer;
        }

        /*
        public MineSweeper2PGameState()
        {
            //Board will be EMPTY, don't use unless you set this manually
            GameBoard = new MineSweeperBoard();
            CurrentPlayer = PlayerEnum.PlayerOne;
        }
        */
        public String PrintHeaderData() 
        { 
            return BoardSize.ToString() + " " + CurrentPlayer.ToString();
        }
        public String PrintBoard() 
        {
            return GameBoard.ToString(); 
        }

        //boardSize CurrentPlayer
        //board content....
        //.
        //.
        /*  
            X = revealed bomb
            x = unrevealed bomb
            #
        = unrevealed field
            [0..9] revealed field
        */
    }
}
