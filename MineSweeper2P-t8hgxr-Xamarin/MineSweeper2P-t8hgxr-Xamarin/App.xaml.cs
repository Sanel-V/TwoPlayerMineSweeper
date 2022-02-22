using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MineSweeper2P_t8hgxr_Xamarin.View;
using MineSweeper2P_t8hgxr.ViewModel;
using MineSweeper2Pt8hgxr.Model;
using MineSweeper2Pt8hgxr.Persistence;
using System.Threading.Tasks;
using MineSweeper2Pt8hgxr.Model.EventArguments;
using ELTE.Sudoku.Persistence;
using ELTE.Sudoku.Model;
using ELTE.Sudoku.View;
using ELTE.Sudoku.ViewModel;

namespace MineSweeper2P_t8hgxr_Xamarin
{
    public partial class App : Application
    {
        private IMineSweeperDataAccess dataAccess;
        private MineSweeper2PModel model;
        private MineSweeper2PViewModel viewModel;

        private IStore store;
        private StoredGameBrowserModel storedGameBrowserModel;
        private StoredGameBrowserViewModel storedGameBrowserViewModel;
        
        private NavigationPage mainPage;
        private GamePage gamePage;
        private NewGamePopup newGamePopup;
        private LoadGamePage loadGamePage;
        private SaveGamePage saveGamePage;

        public App()
        {
            InitializeComponent();


            dataAccess = DependencyService.Get<IMineSweeperDataAccess>();

            model = new MineSweeper2PModel(dataAccess);

            viewModel = new MineSweeper2PViewModel(model);
            viewModel.NewGameEvent += new EventHandler(ViewModel_NewGame);
            viewModel.LoadGameEvent += new EventHandler(ViewModel_LoadGame);
            viewModel.SaveGameEvent += new EventHandler(ViewModel_SaveGame);
            viewModel.GameOverEvent += new EventHandler<MineSweeperGameOverEventArgs>(ViewModel_GameOver);

            store = DependencyService.Get<IStore>(); // a perzisztencia betöltése az adott platformon
            storedGameBrowserModel = new StoredGameBrowserModel(store);
            storedGameBrowserViewModel = new StoredGameBrowserViewModel(storedGameBrowserModel);
            storedGameBrowserViewModel.GameLoading += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameLoading);
            storedGameBrowserViewModel.GameSaving += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameSaving);

            loadGamePage = new LoadGamePage();
            loadGamePage.BindingContext = storedGameBrowserViewModel;

            saveGamePage = new SaveGamePage();
            saveGamePage.BindingContext = storedGameBrowserViewModel;



            gamePage = new GamePage();
            viewModel.NewGame(10);
            gamePage.BindingContext = viewModel;
            newGamePopup = new NewGamePopup();
            newGamePopup.NewGamePopupDone += NewGamePopupDone;


            mainPage = new NavigationPage(gamePage);
            MainPage = mainPage;
            //MainPage = gamePage;
        }

        private async void StoredGameBrowserViewModel_GameSaving(object sender, StoredGameEventArgs e)
        {
            await mainPage.PopAsync(); // visszanavigálunk


            try
            {
                // elmentjük a játékot
                await model.SaveGameAsync(e.Name);
            }
            catch { }

            await MainPage.DisplayAlert("MineSweeper 2 player", "Sikeres mentés.", "OK");

        }

        private async void StoredGameBrowserViewModel_GameLoading(object sender, StoredGameEventArgs e)
        {
            await mainPage.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await model.LoadGameAsync(e.Name);
            }
            catch
            {
                await MainPage.DisplayAlert("MineSweeper 2 player", "Sikertelen betöltés.", "OK");
            }
            viewModel.LoadGame();
        }

        private async void NewGamePopupDone(object sender, bool startNewGame)
        {
            if(startNewGame)
            {
                viewModel.NewGame(newGamePopup.ChosenGameSize);
            }
            await mainPage.Navigation.PopAsync();
        }

        private async void ViewModel_GameOver(object sender, MineSweeperGameOverEventArgs e)
        {

            model.NewGame(MineSweeper2PModel.GameSize.Medium);
            await model.SaveGameAsync("AutoSave");

            
            if (e.GameTied)
            {
                await mainPage.DisplayAlert("Game Over", "A játék döntetlen!", "OK");
                
            }
            else
            {
                await mainPage.DisplayAlert("Game Over", model.NextPlayer(e.LastPlayer).ToString() + " győzött", "OK");
            }
            
        }

        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            await storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await mainPage.PushAsync(saveGamePage); // átnavigálunk a lapra

        }

        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            await storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await mainPage.PushAsync(loadGamePage); // átnavigálunk a lapra
        }

        private async void ViewModel_NewGame(object sender, EventArgs e)
        {
            await mainPage.Navigation.PushAsync(newGamePopup);
        }

        protected override void OnStart()
        {
            viewModel.NewGame(10);
        }

        protected override void OnSleep()
        {
            try
            {
                
                // elmentjük a játékot, ha nincs vége
                if(viewModel.CanSave)
                    Task.Run(async () => await model.SaveGameAsync("AutoSave"));
                else
                {
                    OnStart();
                }
            }
            catch { }
        }

        protected override void OnResume()
        {
            try
            {
                Task.Run(async () =>
                {
                    if(viewModel.CanSave)
                        await model.LoadGameAsync("AutoSave");
                    else
                    {
                        OnStart();
                    }
                });
            }
            catch { }
            viewModel.LoadGame();
        }
    }
}