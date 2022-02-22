using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper2Pt8hgxr.Model
{
    public class MineSweeperBoard
    {
        public static Boolean DEBUG = false;

        #region Fields

        private MineSweeperField[,] gameBoard;

        #endregion

        #region Properties
        public Int32 BoardSize { 
        get 
            {
                if (gameBoard != null)
                {
                    return gameBoard.GetLength(0);
                }
                else return 0;
            } 
        }
        #endregion

        #region Operators
        public MineSweeperField this[int x, int y]
        {
            get => gameBoard[x, y];
            //set => SetValue(key, value);
        }
        #endregion

        #region Methods
        public Boolean HasBomb(Int32 x, Int32 y)
        {
            if (x < BoardSize && x >= 0 && y >= 0 && y < BoardSize)
            {
                return gameBoard[x, y].HasBomb;
            }
            return false;
        }

        public void Reveal(Int32 x, Int32 y)
        {
            if (x < BoardSize && x >= 0 && y >= 0 && y < BoardSize)
            {
                if (gameBoard[x, y].Revealed == false)
                {
                    
                    Stack<Tuple<Int32, Int32>> stack = new Stack<Tuple<Int32, Int32>>();
                    stack.Push(new Tuple<int, int>(x, y));
                    while (stack.Count > 0)
                    {
                        Tuple<int, int> coords = stack.Pop();
                        int k = coords.Item1;
                        int l = coords.Item2;
                        if (k < BoardSize && k >= 0 && l >= 0 && l < BoardSize)
                        {
                            {
                                gameBoard[k, l].Reveal();
                                if (gameBoard[k, l].Value == 0 && !gameBoard[k, l].HasBomb)
                                {
                                    for (int i = k - 1; i <= k + 1; i++)
                                    {
                                        for (int j = l - 1; j <= l + 1; j++)
                                        {
                                            if (i < BoardSize && i >= 0 && j >= 0 && j < BoardSize)
                                            {
                                                if (gameBoard[i, j].Revealed == false)
                                                    stack.Push(new Tuple<int, int>(i, j));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }
        }

        public void HideAllFields()
        {
            if(BoardSize > 0)
            {
                for (int i = 0; i < BoardSize; i++)
                {
                    for (int j = 0; j < BoardSize; j++)
                    {
                        gameBoard[i, j].Hide();
                    }
                }
            }
            
        }

        public Boolean OnlyBombsLeft()
        {
            int i = 0;
            int j = 0;
            bool onlyBombsLeft = true;
            while(i < BoardSize && onlyBombsLeft)
            {
                j = 0;
                while (j < BoardSize && onlyBombsLeft)
                {
                    onlyBombsLeft = (gameBoard[i, j].Revealed || (gameBoard[i, j].HasBomb));
                    j++;
                }    
                i++;
            }
            return (i >= BoardSize && j >= BoardSize);
        }

        public void ResizeBoard(int boardSize)
        {
            gameBoard = new MineSweeperField[boardSize, boardSize];
            
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    gameBoard[i, j] = new MineSweeperField();
                }
            }
        }

        public void PlaceBomb(int x, int y)
        {
            if (x < BoardSize && x >= 0 && y >= 0 && y < BoardSize)
            {
                gameBoard[x, y].PlaceBomb();
            }
        }
        public void PlaceBombs(Int32 bombCount)
        {

            Random r;
            if(DEBUG)
            {
                //Set seed to constant to always get same random sequence
                r = new Random(1);
            }else
            {
                r = new Random();
            }
            Int32 currentBombCount = 0;
            while (currentBombCount < bombCount)
            {
                Int32 randX = r.Next(0, BoardSize);
                Int32 randY = r.Next(0, BoardSize);
                if(!gameBoard[randX,randY].HasBomb)
                {
                    gameBoard[randX, randY].PlaceBomb();
                    currentBombCount++;
                }
            }
        }

        public void UpdateFieldValues()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    //Check if current field has bombs in vicinity
                    Int32 sorroundingBombCount = 0;
                    for (int k = i-1; k <= i+1; k++)
                    {
                        for (int l = j-1; l <= j+1; l++)
                        {
                            //Check if in bounds
                            if(k >= 0 && k < BoardSize && l >= 0 && l < BoardSize)
                            {
                                //Skip yourself
                                if (!(k == i && l == j))
                                {
                                    if(gameBoard[k,l].HasBomb)
                                    {
                                        sorroundingBombCount++;
                                    }
                                }
                            }
                        }
                    }
                    gameBoard[i, j].Value = sorroundingBombCount;
                }
            }
        }

        public void SetupBoard(Int32 bombCount)
        {
            PlaceBombs(bombCount);
            UpdateFieldValues();
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    builder.Append(gameBoard[i,j]);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        #endregion

        #region Constructors

        public MineSweeperBoard() { }

        public MineSweeperBoard(Int32 boardSize)
        {
            gameBoard = new MineSweeperField[boardSize, boardSize];
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    gameBoard[i, j] = new MineSweeperField();
                }
            }
        }
        public MineSweeperBoard(MineSweeperBoard that) 
        {

            //TODO: perfom deep copy
            gameBoard = that.gameBoard;
            
        }

        #endregion



    }
}
