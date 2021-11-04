using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Model.EventArguments;
using MineSweeper2Pt8hgxr.Persistence;

namespace MineSweeper2Pt8hgxr.View
{
    public partial class MineSweeper2PView : Form
    {
        private IMineSweeperDataAccess dataAccess;
        private MineSweeper2PModel gameModel;

        private Button[,] buttonGrid;
        public MineSweeper2PView()
        {
            InitializeComponent();
        }

        private void GameForm_Load(Object sender, EventArgs e)
        {
            dataAccess = new MineSweeperDataAccess();

            gameModel = new MineSweeper2PModel(dataAccess);
            gameModel.GameOver += GameModel_GameOver;
            gameModel.RefreshBoard += GameModel_RefreshBoard;

            newGameItem.Click += NewGameItem_Click;
            saveGameItem.Click += SaveGameItem_Click;
            loadGameItem.Click += LoadGameItem_Click;
            exitItem.Click += ExitItem_Click;
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Biztosan ki szeretne lépni?", "Sudoku játék", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // ha igennel válaszol
                Close();
            }
        }

        private async void LoadGameItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await gameModel.LoadGameAsync(openFileDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                //SetupTable();
            }
        }

        private async void SaveGameItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játé mentése
                    await gameModel.SaveGameAsync(saveDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Játék mentése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a könyvtár nem írható.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void NewGameItem_Click(object sender, EventArgs e)
        {
            using(NewGameDialogBox dialog = new NewGameDialogBox())
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    if(buttonGrid != null)
                        foreach (var button in buttonGrid)
                        {
                            Controls.Remove(button);
                        }
                    Int32 gameSize = dialog.Result;
                    GenerateTable(gameSize);
                    gameModel.NewGame((MineSweeper2PModel.GameSize)gameSize);
                    //GenerateTable();
                }
            }
        }

        private void GameModel_RefreshBoard(object sender, MineSweeperRefreshBoardEventArgs e)
        {
            for (int i = 0; i < gameModel.BoardSize; i++)
            {
                for (int j = 0; j < gameModel.BoardSize; j++)
                {
                    if(gameModel.gameBoard[i,j].Revealed)
                    {
                        buttonGrid[i, j].Enabled = false;
                        if(gameModel.gameBoard[i, j].Value == 0)
                        {
                            buttonGrid[i, j].Text = "";
                            buttonGrid[i, j].FlatStyle = FlatStyle.Flat;
                        }else
                            buttonGrid[i, j].Text = gameModel.gameBoard[i, j].ToString();
                    }else
                        buttonGrid[i, j].Text = "";

                }
            }
            label1.Text = "Current player: " + gameModel.CurrentPlayer;
        }

        private void GameModel_GameOver(object sender, MineSweeperGameOverEventArgs e)
        {
            for (int i = 0; i < gameModel.BoardSize; i++)
            {
                for (int j = 0; j < gameModel.BoardSize; j++)
                {
                    if (gameModel.gameBoard.HasBomb(i, j))
                    {
                        buttonGrid[i, j].BackColor = Color.Red;
                        buttonGrid[i, j].Text = "X";
                    }
                }
            }
            if(e.GameTied)
            {
                MessageBox.Show("A játék döntetlen!",
                                "Two player minesweeper",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Asterisk);
            }else
            {
                MessageBox.Show(gameModel.NextPlayer(e.LastPlayer).ToString() + " győzött", "Two player minesweeper", MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
            }
            
        }

        private void GenerateTable(Int32 gameSize)
        {
            buttonGrid = new Button[gameSize, gameSize];
            for (Int32 i = 0; i < gameSize; i++)
                for (Int32 j = 0; j < gameSize; j++)
                {
                    buttonGrid[i, j] = new Button();
                    buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i); // elhelyezkedés
                    buttonGrid[i, j].Size = new Size(50, 50); // méret
                    buttonGrid[i, j].Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold); // betűtípus
                    buttonGrid[i, j].TabIndex = 100 + i * gameSize + j; // a gomb számát a TabIndex-ben tároljuk
                    //buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus
                    buttonGrid[i, j].MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    // közös eseménykezelő hozzárendelése minden gombhoz
                    Controls.Add(buttonGrid[i, j]);
                    // felvesszük az ablakra a gombot
                }
        }

        private void ButtonGrid_MouseClick(object sender, MouseEventArgs e)
        {
            Int32 x = ((sender as Button).TabIndex - 100) / gameModel.BoardSize;
            Int32 y = ((sender as Button).TabIndex - 100) % gameModel.BoardSize;
            gameModel.RevealField(x, y);
        }
    }

}
