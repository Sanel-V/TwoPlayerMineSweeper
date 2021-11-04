using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MineSweeper2Pt8hgxr.Model;

namespace MineSweeper2Pt8hgxr.Persistence
{
    public class MineSweeperDataAccess : IMineSweeperDataAccess
    {
        public async Task<MineSweeper2PGameState> LoadAsync(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String line = await reader.ReadLineAsync();
                    String[] headerSplit = line.Split(" ");
                    Int32 boardSize = Convert.ToInt32(headerSplit[0]);
                    PlayerEnum currentPlayer = (PlayerEnum)Convert.ToInt32(headerSplit[1]);
                    MineSweeperBoard gameBoard = new MineSweeperBoard();
                    if(boardSize != 0)
                    {
                        gameBoard = new MineSweeperBoard(boardSize);
                    }else
                    {
                        throw new InvalidDataException("Board size is 0");
                    }
                    for (int i = 0; i < boardSize; i++)
                    {
                        line = await reader.ReadLineAsync();
                        String[] boardValues = line.Split(' ');
                        for (int j = 0; j < boardSize; j++)
                        {
                            if(boardValues[j].Equals("X"))
                            {
                                gameBoard.PlaceBomb(i, j);
                                gameBoard.Reveal(i, j);
                            }else
                            if (boardValues[j].Equals("x"))
                            {
                                gameBoard.PlaceBomb(i, j);
                            }else
                            if (boardValues[j].Equals("#"))
                            {
                                
                            }else
                            {
                                gameBoard.Reveal(i, j);
                            }
                        }
                        gameBoard.UpdateFieldValues();

                    }
                    return new MineSweeper2PGameState(gameBoard, currentPlayer);
                }
            }catch
            {
                throw new Exception();
            }
        }

        public async Task SaveAsync(string path, MineSweeper2PGameState gameState)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path)) // fájl megnyitása
                {
                    await writer.WriteLineAsync(gameState.PrintHeaderData());
                    await writer.WriteAsync(gameState.PrintBoard());
                }
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
