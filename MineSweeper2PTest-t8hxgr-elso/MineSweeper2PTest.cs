using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Model.EventArguments;
using MineSweeper2Pt8hgxr.Persistence;
using Moq;
using System.Threading.Tasks;
using System.IO;
using System;
namespace MineSweeper2PTest_t8hxgr_elso
{
    [TestClass]
    public class MineSweeper2PTest
    {
        private MineSweeper2PModel gameModel;
        private MineSweeper2PGameState gameState;
        private MineSweeperBoard emptyBoard;
        private Mock<IMineSweeperDataAccess> mockDataAccess;

        [TestInitialize]
        public void Initialize()
        {
            emptyBoard = new MineSweeperBoard(16);

            MineSweeperBoard gameBoard = new MineSweeperBoard(10);
            gameBoard.PlaceBombs(25);
            gameBoard.UpdateFieldValues();

            gameState = new MineSweeper2PGameState(gameBoard, PlayerEnum.PlayerOne);

            mockDataAccess = new Mock<IMineSweeperDataAccess>();
            //return gameState if path is 'validPath'
            mockDataAccess.Setup(mock => mock.LoadAsync("emptyBoardPath")).Returns(() => Task.FromResult(new MineSweeper2PGameState(emptyBoard, PlayerEnum.PlayerOne)));
            mockDataAccess.Setup(mock => mock.LoadAsync("gameStatePath")).Returns(() => Task.FromResult(gameState));
            gameModel = new MineSweeper2PModel(mockDataAccess.Object);
            gameModel.GameOver += new EventHandler<MineSweeperGameOverEventArgs>(TestModelGameOver);

        }



        


        //MineSweeperField Tests
        [TestMethod]
        public void TestMineSweeperFieldValue()
        {
            MineSweeperField field = new MineSweeperField();
            Assert.AreEqual(0, field.Value);
            field.Value = 2;
            Assert.AreEqual(2, field.Value);
        }

        [TestMethod]
        public void TestMineSweeperFieldIfRevealed()
        {
            MineSweeperField field = new MineSweeperField();
            Assert.AreEqual(false, field.Revealed);
            field.Reveal();
            Assert.AreEqual(true, field.Revealed);
            field = new MineSweeperField(true);
            Assert.AreEqual(true, field.Revealed);
            field.Hide();
            Assert.AreEqual(false, field.Revealed);
        }

        [TestMethod]
        public void TestMineSweeperFieldIfHasBomb()
        {
            MineSweeperField field = new MineSweeperField();
            Assert.AreEqual(false, field.HasBomb);
            field.PlaceBomb();
            Assert.AreEqual(true, field.HasBomb);
        }

        [TestMethod]
        public void TestMineSweeperFieldToString()
        {
            MineSweeperField field = new MineSweeperField();
            Assert.AreEqual("#", field.ToString());
            field.Reveal();
            Assert.AreEqual("0", field.ToString());
            field.Hide();
            field.PlaceBomb();
            Assert.AreEqual("x", field.ToString());
            field.Reveal();
            Assert.AreEqual("X", field.ToString());

        }

        //MineSweeperBoard Tests

        [TestMethod]
        public void TestBoardHasBomb()
        {
            Assert.AreEqual(false, emptyBoard.HasBomb(0,0));
            emptyBoard.PlaceBomb(0, 0);
            Assert.AreEqual(true, emptyBoard.HasBomb(0, 0));


        }

        [TestMethod]
        public void TestBoardIndexing()
        {
            emptyBoard.PlaceBomb(-1, 0);
            emptyBoard.HasBomb(30, 40);
            emptyBoard.Reveal(-1, -1);
            bool allEmpty = true;
            int i = 0;
            int j = 0;

            while(i < emptyBoard.BoardSize && allEmpty)
            {
                while(j < emptyBoard.BoardSize && allEmpty)
                {
                    allEmpty = (emptyBoard[i, j].Value == 0) && (emptyBoard[i, j].Revealed == false) && (emptyBoard[i, j].HasBomb == false);
                    j++;
                }
                i++;
            }
            Assert.AreEqual(true, allEmpty);
        }

        [TestMethod]
        public void TestBoardReveal()
        {
            Assert.AreEqual(false, emptyBoard[11, 15].Revealed);
            emptyBoard.PlaceBomb(4, 4);
            emptyBoard.UpdateFieldValues();
            emptyBoard.Reveal(11, 15);
            System.Diagnostics.Debug.WriteLine(emptyBoard.ToString());
            //0-as mezok es veluk hataron levo nem nulla mezok felfedve
            Assert.AreEqual(true, emptyBoard[11, 15].Revealed);
            Assert.AreEqual(true, emptyBoard[4, 5].Revealed);
            Assert.AreEqual(1, emptyBoard[4, 5].Value);
            //Az egy bomba nincs felfedve, mert korbeveszik az 1-esek
            Assert.AreEqual(false, emptyBoard[4, 4].Revealed);
            Assert.AreEqual(true, emptyBoard[4, 4].HasBomb);
            //Ha nincs rejtett nem bomba mezo, igazat ad
            Assert.AreEqual(true, emptyBoard.OnlyBombsLeft());
        }
        [TestMethod]
        public void TestBoardSize()
        {
            Assert.AreEqual(16, emptyBoard.BoardSize);
            emptyBoard.ResizeBoard(10);
            Assert.AreEqual(10, emptyBoard.BoardSize);

        }

        [TestMethod]
        public void TestBoardPlaceBombs()
        {
            Int32 bombCount = 0;
            for (int i = 0; i < gameState.BoardSize; i++)
            {
                for (int j = 0; j < gameState.BoardSize; j++)
                {
                    if(gameState.GameBoard.HasBomb(i,j))
                    {
                        bombCount++;
                    }
                }
            }

            Assert.AreEqual(25, bombCount);
        }

        [TestMethod]
        public void TestBoardOnlyBombsLeft()
        {
            emptyBoard.PlaceBomb(4, 4);
            emptyBoard.PlaceBomb(4, 6);
            emptyBoard.UpdateFieldValues();
            Assert.AreEqual(false, emptyBoard.OnlyBombsLeft());
            emptyBoard.Reveal(0, 0);
            Assert.AreEqual(false, emptyBoard.OnlyBombsLeft());
            System.Diagnostics.Debug.WriteLine(emptyBoard.ToString());
            Assert.AreEqual(false, emptyBoard[4,5].Revealed);
            emptyBoard.Reveal(4, 5);
            Assert.AreEqual(true, emptyBoard.OnlyBombsLeft());
            Assert.AreEqual(true, emptyBoard[4, 5].Revealed);

        }

        [TestMethod]
        public void TestBoardUpdateFieldValues()
        {
            emptyBoard.PlaceBomb(0, 0);
            emptyBoard.UpdateFieldValues();
            Assert.AreEqual(1, emptyBoard[0, 1].Value);
            Assert.AreEqual(0, emptyBoard[0, 2].Value);
            emptyBoard.PlaceBomb(1, 0);
            emptyBoard.UpdateFieldValues();
            Assert.AreEqual(2, emptyBoard[0, 1].Value);
            Assert.AreEqual(0, emptyBoard[0, 2].Value);
            Assert.AreEqual(1, emptyBoard[2, 0].Value);
            Assert.AreEqual(1, emptyBoard[2, 1].Value);
        }
        //Model tests
        [TestMethod]
        public void TestModelNextPlayer()
        {
            Assert.AreEqual(PlayerEnum.PlayerOne, gameModel.CurrentPlayer);   
            Assert.AreEqual(PlayerEnum.PlayerTwo, gameModel.NextPlayer(gameModel.CurrentPlayer));
        }
        [TestMethod]
        public void TestModelNewGame()
        {
            gameModel.NewGame((MineSweeper2PModel.GameSize)10);
            Assert.AreEqual(PlayerEnum.PlayerOne, gameModel.CurrentPlayer);
            
        }
        [TestMethod]
        public async Task TestModelRevealField()
        {
            await gameModel.LoadGameAsync("emptyBoardPath");
            gameModel.RevealField(0, 0);

        }

        //Persistence tests
        [TestMethod]
        public async Task TestModelLoadGame()
        {
            gameModel.NewGame((MineSweeper2PModel.GameSize)10);

            await gameModel.LoadGameAsync("emptyBoardPath");
            Assert.AreEqual(16, gameModel.gameBoard.BoardSize);

            bool allEmpty = true;
            int i = 0;
            int j = 0;

            while (i < emptyBoard.BoardSize && allEmpty)
            {
                while (j < emptyBoard.BoardSize && allEmpty)
                {
                    allEmpty = (emptyBoard[i, j].Value == 0) && (emptyBoard[i, j].Revealed == false) && (emptyBoard[i, j].HasBomb == false);
                    j++;
                }
                i++;
            }
            Assert.AreEqual(true, allEmpty);

        }
        //Event tests


        private void TestModelGameOver(object sender, MineSweeperGameOverEventArgs e)
        {
            //Dontetlen
            Assert.AreEqual(true, e.GameTied);
            //Utolso jatekos
            Assert.AreEqual(PlayerEnum.PlayerOne, e.LastPlayer);
        }
    }
}
