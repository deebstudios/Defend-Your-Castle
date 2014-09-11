using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Defend_Your_Castle
{
    // Global enum to represent the game state
    public enum GameState : byte { Screen, InGame, Paused }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Reference to GamePage.xaml
        public GamePage GamePage;

        //Time the game is active
        public static float activeTime;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
            graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;
            Content.RootDirectory = "Content";

            // Get the resolution scale factor
            ResolutionScaleFactor = new Vector2((ScreenSize.X / Window.ClientBounds.Width), (ScreenSize.Y / Window.ClientBounds.Height));

            // Show the mouse on the game screen
            IsMouseVisible = true;

            // Enable the tap gesture
            TouchPanel.EnabledGestures = GestureType.Tap;
        }

        static Game1()
        {
            activeTime = 0f;

            // Get the screen size
            ScreenSize = new Vector2(640, 384);

            // Get half the screen size
            ScreenHalf = (ScreenSize / 2);
        }

        protected override void OnDeactivated(object sender, System.EventArgs args)
        {
            // Code pausing here
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

            //TestSong = Content.Load<Song>("Music\\Mario Party - Peaceful Mushroom Village");
            //TestSound = Content.Load<SoundEffect>("Sounds/test");

            //SoundManager.PlaySong(TestSong);
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

            // Unload all content here so that the game does not throw an exception when the "Quit" button is clicked
        }

        public GameState GameState
        {
            get { return gameState; }
            set
            {
                gameState = value;

                if (value == GameState.InGame)
                {
                    // Remove the KeyDown event from the game window
                    Windows.UI.Xaml.Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
                }
                else
                {
                    // Handle the KeyDown event for the game window
                    Windows.UI.Xaml.Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
                }
            }
        }

        private void ShowCanvas_Screen()
        {
            GamePage.CurrentScreen.Visibility = Visibility.Visible;
            GamePage.HUD.Visibility = Visibility.Collapsed;
        }

        private void ShowCanvas_InGame()
        {
            GamePage.HUD.Visibility = Visibility.Visible;
            GamePage.CurrentScreen.Visibility = Visibility.Collapsed;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            if (MenuScreens.Count > 0) GetCurrentScreen().CursorMove(e.VirtualKey);
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
                    ShowCanvas_Screen();

                    break;
                case GameState.InGame:
                    ShowCanvas_InGame();

                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            //Update active time
            activeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check which game state the player is in
            switch (GameState)
            {
                case GameState.Screen: // Update the current screen
                    //GetCurrentScreen().Update(this);

                    break;
                case GameState.InGame: // Update the in-game objects
                    //Level.Update(this);

                    break;
                case GameState.Paused: // Don't update any in-game objects
                    //Level.Update(this);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null);

            // Check which game state the player is in
            switch (GameState)
            {
                case GameState.Screen: // Draw the current screen
                    //GetCurrentScreen().Draw(spriteBatch);

                    break;
                case GameState.InGame: // Draw the in-game objects
                    //Level.Draw(spriteBatch);

                    break;
                case GameState.Paused: // Draw the in-game objects and as dark color overlay
                    //Level.Draw(spriteBatch);

                    // Draw color overlay

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {
            // Readjust the resolution scale factor based on the new screen resolution/size

            // TODO: This is not perfect. It doesn't properly take into account the snapping of the game
            // TODO: This is not perfect. It doesn't properly take into account the snapping of the game
            // TODO: This is not perfect. It doesn't properly take into account the snapping of the game
            ResolutionScaleFactor = new Vector2((ScreenSize.X / Window.ClientBounds.Width), (ScreenSize.Y / Window.ClientBounds.Height));
        }


    }
}
