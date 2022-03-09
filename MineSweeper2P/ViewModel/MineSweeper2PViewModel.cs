using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Model.EventArguments;
namespace MineSweeper2P_t8hgxr_WPF.ViewModel
{
    public class MineSweeper2PViewModel : ViewModelBase
    {
        #region Fields

        private MineSweeper2PModel model;

        private PlayerEnum currentPlayer;
        private Int32 boardSize;

        private bool canSave;
        private ObservableCollection<MineSweeperFieldViewModel> fields;
        #endregion

        #region Properties
        public PlayerEnum CurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                if(currentPlayer != value)
                {
                    currentPlayer = value;
                    OnPropertyChanged();
                }
            }
        }
        public Int32 BoardSize
        {     
            get 
            { 
                return boardSize; 
            } 
            set 
            { 
                if(boardSize != value)
                {
                    boardSize = value;
                    OnPropertyChanged();
                }
            }
        }
            
        public bool CanSave
        {
            get
            {
                return canSave;
            }
            set
            {
                if(canSave != value)
                {
                    canSave = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<MineSweeperFieldViewModel> Fields 
        { 
            get { return fields; } 
            set { if (fields != value) { fields = value; OnPropertyChanged(); } } 
        }

        #endregion

        #region Events

        public event EventHandler NewGameEvent;

        public event EventHandler LoadGameEvent;

        public event EventHandler SaveGameEvent;

        public event EventHandler<MineSweeperGameOverEventArgs> GameOverEvent;

        #endregion

        #region Commands
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }

        #endregion

        #region Constructors
        public MineSweeper2PViewModel(MineSweeper2PModel model)
        {
            CanSave = false;
            this.model = model;
            model.GameOver += new EventHandler<MineSweeperGameOverEventArgs>(Model_GameOver);
            model.RefreshBoard += new EventHandler<MineSweeperRefreshBoardEventArgs>(Model_RefreshTable);
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());


        }
        
        #endregion

        #region Methods

        public void NewGame(Int32 gameSize)
        {
            CanSave = true;
            Fields = new ObservableCollection<MineSweeperFieldViewModel>();
            
            BoardSize = gameSize;
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    //MineSweeperField field = board[i, j];

                    Fields.Add(new MineSweeperFieldViewModel
                        (
                         //field,
                         i * BoardSize + j,
                         i,
                         j,
                         new DelegateCommand(param => Reveal(Convert.ToInt32(param)))
                        )
                    );
                }
            }
            model.NewGame((MineSweeper2PModel.GameSize)gameSize);
        }

        public void LoadGame()
        {
            CanSave = true;
            Fields = new ObservableCollection<MineSweeperFieldViewModel>();

            BoardSize = model.BoardSize;
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    //MineSweeperField field = board[i, j];

                    Fields.Add(new MineSweeperFieldViewModel
                        (
                         //field,
                         i * BoardSize + j,
                         i,
                         j,
                         new DelegateCommand(param => Reveal(Convert.ToInt32(param)))
                        )
                    );
                }
            }
            ForceRefresh();
        }

        private void Reveal(Int32 index)
        {
            var fieldVM = Fields[index];
            model.RevealField(fieldVM.X, fieldVM.Y);

            OnPropertyChanged("CurrentPlayer");

        }

        public void ForceRefresh()
        {
            Model_RefreshTable(this, new MineSweeperRefreshBoardEventArgs());
        }

        #endregion

        #region Event handlers
        
        private void Model_GameOver(object sender, MineSweeperGameOverEventArgs e)
        {
            CanSave = false;
            foreach (var field in Fields)
            {
                field.IsEnabled = false;
                if(field.HasBomb)
                {
                    field.Revealed = true;
                    field.Text = "X";
                    
                }
            }
            OnGameOver(e);
        }

        private void Model_RefreshTable(object sender, MineSweeperRefreshBoardEventArgs e)
        {
            if(!CanSave)
            {
                return;
            }
            CurrentPlayer = model.CurrentPlayer;

            if (Fields != null)
            { 
                for (int i = 0; i < model.BoardSize; i++)
                {
                    for (int j = 0; j < model.BoardSize; j++)
                    {
                        var field = model.gameBoard[i, j];
                        var fieldVM = Fields[i * model.BoardSize + j];
                        fieldVM.Revealed = field.Revealed;
                        fieldVM.IsEnabled = !fieldVM.Revealed;
                        fieldVM.HasBomb = field.HasBomb;
                        fieldVM.Value = field.Value;
                        fieldVM.Text = fieldVM.changeText();
                        
                    }
                }
            }
            
        }
        #endregion

        #region Event methods
        private void OnNewGame()
        {
            NewGameEvent.Invoke(this, EventArgs.Empty);

        }
        private void OnLoadGame()
        {
            LoadGameEvent.Invoke(this, EventArgs.Empty);
        }
        private void OnSaveGame()
        {
            SaveGameEvent.Invoke(this, EventArgs.Empty);
        }

        private void OnGameOver(MineSweeperGameOverEventArgs e)
        {
            GameOverEvent.Invoke(this, e);
        }

        #endregion

    }
}
