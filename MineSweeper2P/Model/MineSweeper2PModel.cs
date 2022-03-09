using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MineSweeper2Pt8hgxr.Model.EventArguments;
using MineSweeper2Pt8hgxr.Persistence;

namespace MineSweeper2Pt8hgxr.Model
{
    public class MineSweeper2PModel
    {
        #region Model specific enums
        public enum GameSize
        {
            Small = 6,
            Medium = 10,
            Large = 16
        }
        #endregion

        #region Properties
        public PlayerEnum CurrentPlayer { get; private set; } = PlayerEnum.PlayerOne;
        public Int32 BoardSize { get { return gameBoard.BoardSize; } }

        #endregion

        #region Fields
        public MineSweeperBoard gameBoard;
        private IMineSweeperDataAccess dataAccess;

        #endregion

        #region Methods
        public PlayerEnum NextPlayer(PlayerEnum player)
        {
            if(player.Equals(PlayerEnum.PlayerOne))
            {
                return PlayerEnum.PlayerTwo;
            }else
            {
                return PlayerEnum.PlayerOne;
            }
        }
        public void NewGame(GameSize gameSize)
        {
            Int32 sizeOfGame = Convert.ToInt32(gameSize);
            gameBoard = new MineSweeperBoard(sizeOfGame);
            //Approx. 25% of fields will be bombs
            gameBoard.SetupBoard((Int32)Math.Ceiling((double)(sizeOfGame * sizeOfGame) * 0.25));
            CurrentPlayer = PlayerEnum.PlayerOne;
            OnRefreshBoard();
        }
        //TODO: add loadGame and saveGame functions
        public async Task LoadGameAsync(String path)
        {
            if (dataAccess == null)
                throw new NullReferenceException("No data access is provided.");

            MineSweeper2PGameState gameState;
            gameState = await dataAccess.LoadAsync(path);

            gameBoard = gameState.GameBoard;
            CurrentPlayer = gameState.CurrentPlayer;
           
        }

        public async Task SaveGameAsync(String path)
        {
            if (dataAccess == null)
                throw new NullReferenceException("No data access is provided.");
            
            await dataAccess.SaveAsync(path, new MineSweeper2PGameState(gameBoard, CurrentPlayer));
        }

        public void RevealField(Int32 x, Int32 y)
        {
            gameBoard.Reveal(x, y);
            
            if (gameBoard.OnlyBombsLeft())
            {
                OnRefreshBoard();
                OnGameOver(true, CurrentPlayer);
            }
            else
            {
                if (gameBoard[x, y].HasBomb)
                {
                    OnGameOver(false, CurrentPlayer);
                }
                else
                {
                    CurrentPlayer = NextPlayer(CurrentPlayer);
                }
                OnRefreshBoard();
            }
          
            
            
        }

        #endregion

        #region Events
        public event EventHandler<MineSweeperGameOverEventArgs> GameOver;
        public event EventHandler<MineSweeperRefreshBoardEventArgs> RefreshBoard;

        #endregion

        #region EventHandlers

        private void OnGameOver(Boolean gameTied, PlayerEnum lastPlayer)
        {
            GameOver?.Invoke(this, new MineSweeperGameOverEventArgs(lastPlayer, gameTied));
        }
        private void OnRefreshBoard()
        {
            RefreshBoard?.Invoke(this, new MineSweeperRefreshBoardEventArgs());
        }
        #endregion

        #region Constructors

        //Inject data access
        public MineSweeper2PModel(IMineSweeperDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        #endregion

    }
}
