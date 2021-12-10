 using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Model.EventArguments;
using MineSweeper2Pt8hgxr.Persistence;
using MineSweeper2P_t8hgxr_WPF.ViewModel;
using MineSweeper2P_t8hgxr_WPF.View;
using Microsoft.Win32;

namespace MineSweeper2P_t8hgxr_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields
        private IMineSweeperDataAccess dataAccess;
        private MineSweeper2PModel model;
        private MineSweeper2PViewModel viewModel;
        private MainWindow view;
        #endregion

        #region Constructors
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
        #endregion

        #region Event Handlers
        private void App_Startup(object sender, StartupEventArgs e)
        {
            dataAccess = new MineSweeperDataAccess();
            
            model = new MineSweeper2PModel(dataAccess);

            viewModel = new MineSweeper2PViewModel(model);
            viewModel.NewGameEvent  += new EventHandler(ViewModel_NewGame);
            viewModel.LoadGameEvent += new EventHandler(ViewModel_LoadGame);
            viewModel.SaveGameEvent += new EventHandler(ViewModel_SaveGame);
            viewModel.GameOverEvent += new EventHandler<MineSweeperGameOverEventArgs>(ViewModel_GameOver);

            view = new MainWindow();
            view.DataContext = viewModel;
            view.Show();
        }

        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Save files (*.sav)|*.sav|txt files (*.txt)|*.txt|All files (*) |*";
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    // játék mentése
                    await model.SaveGameAsync(saveDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        "Játék mentése sikertelen!" + 
                        Environment.NewLine + 
                        "Hibás az elérési út, " +
                        "vagy a könyvtár nem írható.", 
                        "Hiba!", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error
                        );
                }
            }
        }

        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Save files (*.sav)|*.sav|txt files (*.txt)|*.txt|All files (*) |*";

            if (openFileDialog.ShowDialog() == true) // ha kiválasztottunk egy fájlt
            {
                try
                {
                    // játék betöltése
                    await model.LoadGameAsync(openFileDialog.FileName);
                    /*
                    boardLayout.Controls.Clear();
                    saveGameItem.Enabled = true;
                    
                    GenerateTable(gameModel.BoardSize);
                    GameModel_RefreshBoard(sender, new MineSweeperRefreshBoardEventArgs());
                    */
                    //throw new NotImplementedException("ViewModel_LoadGame");
                    ;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace + "Játék betöltése sikertelen!" + Environment.NewLine + "Hibás az elérési út, vagy a fájlformátum.", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Error);

                }

                viewModel.LoadGame();
            }
        }

        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            NewGameDialog newGameDialog = new NewGameDialog();
            if(newGameDialog.ShowDialog() == true)
            {
                viewModel.NewGame(newGameDialog.GameSize);
                
                //throw new NotImplementedException("ViewModel_NewGame");

            }
        }

        private void ViewModel_GameOver(object sender, MineSweeperGameOverEventArgs e)
        {
            if (e.GameTied)
            {
                MessageBox.Show("A játék döntetlen!",
                                "Two player minesweeper",
                                MessageBoxButton.OK,
                                MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show(model.NextPlayer(e.LastPlayer).ToString() + " győzött", "Two player minesweeper", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

        }
        #endregion
    }
}
