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

            saveGameItem.Enabled = false;

            boardLayout.Margin = new Padding(0);
            boardLayout.Padding = new Padding(5,5,5,31);

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
            openFileDialog.Filter = "Save files (*.sav)|*.sav|txt files (*.txt)|*.txt|All files (*) |*";

            if (openFileDialog.ShowDialog() == DialogResult.OK) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await gameModel.LoadGameAsync(openFileDialog.FileName);
                    boardLayout.Controls.Clear();
                    saveGameItem.Enabled = true;
                    GenerateTable(gameModel.BoardSize);
                    GameModel_RefreshBoard(sender, new MineSweeperRefreshBoardEventArgs());
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show( ex.Message + ex.StackTrace + "Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                //SetupTable();
            }
        }

        private async void SaveGameItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Save files (*.sav)|*.sav|txt files (*.txt)|*.txt|All files (*) |*";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // játék mentése
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
                    if(boardLayout != null)
                    {
                        boardLayout.Controls.Clear();
                    }
                    saveGameItem.Enabled = true;

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
                    Button button = (Button)boardLayout.Controls[i * gameModel.BoardSize + j];
                    
                    if (gameModel.gameBoard[i,j].Revealed)
                    {
                        button.FlatStyle = FlatStyle.Flat;
                        button.Enabled = false;
                        if(gameModel.gameBoard[i, j].Value == 0)
                        {
                            button.Text = "";
                            
                        }
                        else
                            button.Text = gameModel.gameBoard[i, j].ToString();
                    }else
                        button.Text = "";

                }
            }
            label1.Text = "Current player: " + gameModel.CurrentPlayer;
        }

        private void GameModel_GameOver(object sender, MineSweeperGameOverEventArgs e)
        {
            saveGameItem.Enabled = false;
            boardLayout.Enabled = false;
            for (int i = 0; i < gameModel.BoardSize; i++)
            {
                for (int j = 0; j < gameModel.BoardSize; j++)
                {
                    if (gameModel.gameBoard.HasBomb(i, j))
                    {
                        boardLayout.Controls[i * gameModel.BoardSize + j].BackColor = Color.Red;
                        boardLayout.Controls[i * gameModel.BoardSize + j].Text = "X";
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
            boardLayout.Visible = true;
            boardLayout.Enabled = true;
            boardLayout.ColumnCount = gameSize;
            boardLayout.RowCount = gameSize;

            boardLayout.ColumnStyles.Clear();
            boardLayout.RowStyles.Clear();
            
            for (int i = 0; i < gameSize; i++)
            {
                boardLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50));
                
            }
            for (int i = 0; i < gameSize; i++)
            {
                boardLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50));
            }
            
            for (Int32 i = 0; i < gameSize; i++)
                for (Int32 j = 0; j < gameSize; j++)
                {

                    Button button = new Button();
                    button.Size = new Size(50, 50);
                    button.MouseClick += new MouseEventHandler(ButtonGrid_MouseClick);
                    button.Dock = DockStyle.Fill;
                    button.Font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold);
                    button.Margin = new Padding(0);
                    button.Padding = new Padding(0);
                    
                    boardLayout.Controls.Add(button, j, i);

                }
        }

        private void ButtonGrid_MouseClick(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            Int32 position = boardLayout.Controls.GetChildIndex(button);
            Int32 x = position / gameModel.BoardSize;
            Int32 y = position % gameModel.BoardSize;
            gameModel.RevealField(x, y);
        }
    }

}
