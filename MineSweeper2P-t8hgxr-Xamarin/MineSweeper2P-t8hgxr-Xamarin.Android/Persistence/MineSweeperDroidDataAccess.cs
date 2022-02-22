﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Persistence;
using MineSweeper2Pt8hgxr.Droid.Persistence;

[assembly: Dependency(typeof(MineSweeperDroidDataAccess))]
namespace MineSweeper2Pt8hgxr.Droid.Persistence
{
    public class MineSweeperDroidDataAccess : IMineSweeperDataAccess
    {
        public async Task<MineSweeper2PGameState> LoadAsync(string path)
        {
            try
            {
                String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
                using (StreamReader reader = new StreamReader(filePath))
                {
                    

                    String line = await reader.ReadLineAsync();
                    String[] headerSplit = line.Split(' ');
                    Int32 boardSize = Convert.ToInt32(headerSplit[0]);

                    PlayerEnum currentPlayer = (PlayerEnum)Enum.Parse(typeof(PlayerEnum), headerSplit[1]);
                    MineSweeperBoard gameBoard;// = new MineSweeperBoard();
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
                        String boardValues = line;
                        for (int j = 0; j < boardSize; j++)
                        {
                            if(boardValues[j].Equals('X'))
                            {
                                gameBoard.PlaceBomb(i, j);
                                gameBoard.Reveal(i, j);
                            }else
                            if (boardValues[j].Equals('x'))
                            {
                                gameBoard.PlaceBomb(i, j);
                            }else
                            {
                                if (!boardValues[j].Equals('#'))
                                {
                                    gameBoard[i,j].Reveal();
                                    //
                                }
                                
                            }
                            
                        }
                        

                    }
                    gameBoard.UpdateFieldValues();
                    return new MineSweeper2PGameState(gameBoard, currentPlayer);
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveAsync(string path, MineSweeper2PGameState gameState)
        {
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath)) // fájl megnyitása
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
