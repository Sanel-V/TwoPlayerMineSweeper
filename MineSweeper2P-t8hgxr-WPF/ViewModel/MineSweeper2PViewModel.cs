using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Model.EventArguments;
namespace MineSweeper2P_t8hgxr_WPF.ViewModel
{
    public class MineSweeper2PViewModel : ViewModelBase
    {
        #region Fields

        private MineSweeper2PModel model;

        #endregion

        #region Properties
            public PlayerEnum CurrentPlayer
            {
                get { return model.CurrentPlayer; }
               
            }
            public Int32 BoardSize { get { return model.BoardSize; } }
            
            public ObservableCollection<MineSweeperFieldViewModel> Fields { get; set; }

        #endregion

        #region Events

        public event EventHandler NewGame;

        public event EventHandler LoadGame;

        public event EventHandler SaveGame;

        #endregion

        #region Commands
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }

        #endregion

        #region Constructors
        public MineSweeper2PViewModel(MineSweeper2PModel model)
        {
            this.model = model;
            model.GameOver += new EventHandler<MineSweeperGameOverEventArgs>(Model_GameOver);
            model.RefreshBoard += new EventHandler<MineSweeperRefreshBoardEventArgs>(Model_RefreshTable);
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());


            Fields = new ObservableCollection<MineSweeperFieldViewModel>();
            MineSweeperBoard board = model.gameBoard;   
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    MineSweeperField field = board[i, j];

                    Fields.Add(new MineSweeperFieldViewModel
                        (
                         field,
                         i,
                         j,
                         new DelegateCommand(param => Reveal(Convert.ToInt32(param)))
                        )
                    );
                }
            }
            OnPropertyChanged("CurrentPlayer");
        }
        #endregion

        #region Methods

        private void Reveal(Int32 index)
        {
           //MineSweeperFieldViewModel vmField = 
           Fields[index].Field.Reveal();

            
            OnPropertyChanged("CurrentPlayer");

        }

        #endregion

        #region Event handlers
        
        private void Model_GameOver(object sender, MineSweeperGameOverEventArgs e)
        {
            throw new NotImplementedException("Model_GameOver");
        }

        private void Model_RefreshTable(object sender, MineSweeperRefreshBoardEventArgs e)
	    {
            OnPropertyChanged("CurrentPlayer");
        }
        #endregion

        #region Event methods
        private void OnNewGame()
        {
            NewGame.Invoke(this, EventArgs.Empty);

        }
        private void OnLoadGame()
        {
            LoadGame.Invoke(this, EventArgs.Empty);
        }
        private void OnSaveGame()
        {
            SaveGame.Invoke(this, EventArgs.Empty);
        }
        #endregion

    }
}
