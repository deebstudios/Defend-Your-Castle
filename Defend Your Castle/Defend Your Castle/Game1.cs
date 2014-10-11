using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Defend_Your_Castle
{
    // Global enum to represent the game state
    public enum GameState : byte { Screen, InGame, Paused, LevelEnd, Shop }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        // Reference to GamePage.xaml
        public GamePage GamePage;

        //The number of frames per second the game runs
        public const float FPS = 60f;

        //Time the game is active
        private static float activeTime;

        // Stack of MenuScreen objects
        private Stack<MenuScreen> MenuScreens;

        // Keeps track of the current game state
        private GameState gameState;

        // Screen size of the game
        public static readonly Vector2 ScreenSize;

        // Half the screen size of the game
        public static readonly Vector2 ScreenHalf;

        // The scale factor that converts actual screen coordinates to game screen coordinates
        public static Vector2 ResolutionScaleFactor;

        // Stores the game state before the full screen notice
        private GameState PrevGameState;

        // Checks whether the player has saved data on game load
        public static bool HasSavedData;

        //The level
        public Level level;

        // The shop
        public Shop shop;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            //graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;

            Content.RootDirectory = "Content";

            // Get the resolution scale factor
            ResolutionScaleFactor = new Vector2((ScreenSize.X / Window.ClientBounds.Width), (ScreenSize.Y / Window.ClientBounds.Height));

            // Show the mouse on the game screen
            IsMouseVisible = true;

            // Store the global touch state
            Input.TouchState = TouchPanel.GetState(Window);

            // Enable the tap gesture
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        static Game1()
        {
            activeTime = 0f;

            // Get the screen size
            ScreenSize = new Vector2(640, 480);

            // Get half the screen size
            ScreenHalf = (ScreenSize / 2);
        }

        public static float ActiveTime
        {
            get { return activeTime; }
        }

        protected override void OnDeactivated(object sender, System.EventArgs args)
        {
            // Check if the user is ingame
            if (GameState == GameState.InGame)
            {
                // Pause the game
                PauseGame();
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            // Create a new stack of MenuScreen objects
            MenuScreens = new Stack<MenuScreen>();

            // Set the game state to indicate the player is viewing a screen
            GameState = GameState.Screen;
            
            // Load the volume settings
            SoundManager.LoadVolumeSettings();

            // Handle the ClientSizeChanged event for the game window
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadAssets.LoadContent(Content);

            SoundManager.PlaySong(LoadAssets.TestSong);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            
            // Stop all songs
            SoundManager.StopSong();

            // Unload all content so that the game does not throw an exception when the "Quit" button is clicked
            Content.Unload();
        }

        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        private void ShowGrid_Screen()
        {
            GamePage.CurrentScreen.Visibility = Visibility.Visible;
            GamePage.GameHUD.Visibility = Visibility.Collapsed;
            GamePage.LevelEnd.Visibility = Visibility.Collapsed;
            GamePage.Shop.Visibility = Visibility.Collapsed;
        }

        private void ShowGrid_InGame()
        {
            GamePage.GameHUD.Visibility = Visibility.Visible;
            GamePage.CurrentScreen.Visibility = Visibility.Collapsed;
            GamePage.LevelEnd.Visibility = Visibility.Collapsed;
            GamePage.Shop.Visibility = Visibility.Collapsed;
        }

        private void ShowGrid_LevelEnd()
        {
            GamePage.LevelEnd.Visibility = Visibility.Visible;
            GamePage.CurrentScreen.Visibility = Visibility.Collapsed;
            GamePage.GameHUD.Visibility = Visibility.Collapsed;
            GamePage.Shop.Visibility = Visibility.Collapsed;
        }

        private void ChangePauseMenuState(Visibility visibility)
        {
            GamePage.PauseMenu.Visibility = visibility;
        }

        private void ChangeFullScreenNoticeState(Visibility visibility)
        {
            // Set the visibility of the full screen notice
            GamePage.FullScreenNotice.Visibility = visibility;

            // Check if the full screen notice will be shown
            if (visibility == Visibility.Visible)
            {
                // Store the current game state if it is not Screen (the default)
                if (GameState != GameState.Screen) PrevGameState = GameState;

                // Change the game state to screen
                ChangeGameState(GameState.Screen);

                // Hide the CurrentScreen Grid
                GamePage.CurrentScreen.Visibility = Visibility.Collapsed;
            }
            else // The full screen notice will be hidden
            {
                // Change the game state to what it was before
                ChangeGameState(PrevGameState);

                // If the previous game state was paused, show the HUD
                if (PrevGameState == GameState.Paused) ShowGrid_InGame();

                // Set the stored game state back to the default
                PrevGameState = GameState.Screen;
            }
        }

        private void ChangeLevelStartAnimState(bool ShouldPause)
        {
            // Check if the level animation is active
            if (GamePage.LevelStart_Anim.GetCurrentState() == Windows.UI.Xaml.Media.Animation.ClockState.Active)
            {
                // It is, so check if the animation should be paused, and pause it if so
                if (ShouldPause == true)
                    GamePage.LevelStart_Anim.Pause();
                else // The animation should not be paused, so resume it
                    GamePage.LevelStart_Anim.Resume();
            }
        }

        public void AddScreen(MenuScreen screen)
        {
            MenuScreens.Push(screen);
            screen.ShowScreen();
        }

        public MenuScreen GetCurrentScreen()
        {
            // Return the next screen if one exists; otherwise, return null to indicate that there are no screens left
            return ((MenuScreens.Count > 0) ? MenuScreens.Peek() : null);
        }

        public void RemoveScreen()
        {
            // Check if another screen exists
            if (MenuScreens.Count > 0)
            {
                // Remove the next screen
                MenuScreens.Pop();

                // Check if the current screen exists
                if (GetCurrentScreen() != null)
                {
                    // Show the current screen
                    MenuScreens.Peek().ShowScreen();
                }
                else // No screen exists
                {
                    // Clear all of the children from the Canvas
                    GamePage.CurrentScreen.Children.Clear();
                }
            }
        }

        public void ChangeGameState(GameState state)
        {
            // Set the game state to the specified state
            GameState = state;

            // Switch the main canvas depending on the new game state
            switch (GameState)
            {
                case GameState.Screen:
                    ShowGrid_Screen();
                    ChangePauseMenuState(Visibility.Collapsed);

                    break;
                case GameState.InGame:
                    ChangePauseMenuState(Visibility.Collapsed);
                    ChangeLevelStartAnimState(false);
                    ShowGrid_InGame();

                    break;
                case GameState.Paused:
                    ChangePauseMenuState(Visibility.Visible);
                    ChangeLevelStartAnimState(true);

                    break;
                case GameState.LevelEnd:
                    ShowGrid_LevelEnd();
                    ChangePauseMenuState(Visibility.Collapsed);

                    break;
                case GameState.Shop:
                    GamePage.ShowShop();

                    break;
            }
        }

        public void StartGame()
        {
            // Remove the Title Screen
            RemoveScreen();

            // Create a new level
            level = new Level(new Player(GamePage), this);
            level.AddPlayerHelper(new Archer(0));

            // Create a new shop
            shop = new Shop(GamePage, level.GetPlayer);

            // Set the player to in-game
            ChangeGameState(GameState.InGame);
        }

        public void PauseGame()
        {
            // Set the game to paused
            ChangeGameState(GameState.Paused);
        }

        public void ContinueGame()
        {
            // Remove the Title Screen
            RemoveScreen();

            // Load the player's saved game data
            LoadData();

            // Set the player to shop
            ChangeGameState(GameState.Shop);

            // Show the shop
            GamePage.ShowShop();
        }

        // Loads the player's saved game data
        public async void LoadData()
        {
            // Read the game data
            object[] GameData = await Data.LoadGameData(GamePage, this);

            // Get the shop
            shop = (Shop)GameData[0];

            // Get the level
            level = (Level)GameData[1];
        }

        protected override void Update(GameTime gameTime)
        {
            //Update active time if the game is not paused
            if (GameState == GameState.InGame) activeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check which game state the player is in
            switch (GameState)
            {
                case GameState.Screen: // Update the current screen
                    //GetCurrentScreen().Update(this);

                    break;
                case GameState.InGame: // Update the in-game objects
                    level.Update();
                    
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        ChangeGameState(GameState.Paused);
                    }

                    break;
                case GameState.Paused: // Don't update any in-game objects
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        ChangeGameState(GameState.InGame);
                    }

                    break;
                case GameState.LevelEnd:
                case GameState.Shop:
                    
                    break;
            }

            // Update the global touch state
            Input.TouchState = TouchPanel.GetState(Window);

            #if DEBUG
                //Debug commands
                Debug.Update();
            #endif

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            Matrix scaleMatrix = Matrix.CreateScale(new Vector3(graphics.PreferredBackBufferWidth / ScreenSize.X, graphics.PreferredBackBufferHeight / ScreenSize.Y, 1f));

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, scaleMatrix);

            // Check which game state the player is in
            switch (GameState)
            {
                case GameState.Screen: // Draw the current screen
                    // Do nothing since the screens are drawn through XAML

                    break;
                case GameState.InGame: // Draw the in-game objects
                    level.Draw(spriteBatch);

                    break;
                case GameState.Paused: // Draw the in-game objects and a dark color overlay
                    level.Draw(spriteBatch);

                    // Draw color overlay
                    spriteBatch.Draw(LoadAssets.ScalableBox, new Vector2(0, 0), null, new Color(Color.Black, 35), 0f, new Vector2(0, 0), new Vector2(ScreenSize.X, ScreenSize.Y), SpriteEffects.None, 1f);

                    break;
                case GameState.LevelEnd:
                case GameState.Shop:

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            // Readjust the resolution scale factor based on the new screen resolution/size
            ResolutionScaleFactor = new Vector2((ScreenSize.X / Window.ClientBounds.Width), (ScreenSize.Y / Window.ClientBounds.Height));

            // Check if the user is NOT in full screen mode
            if (ApplicationView.GetForCurrentView().IsFullScreen == false)
            {
                // If the user is in-game, pause the game
                if (GameState == GameState.InGame) PauseGame();

                // Show the full screen notice
                ChangeFullScreenNoticeState(Visibility.Visible);
            }
            else // The user is in full screen mode
            {
                // Hide the full screen notice
                ChangeFullScreenNoticeState(Visibility.Collapsed);
            }
        }


    }
}