using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

using Sharp2D.Engine.Bootstrapping;
using Sharp2D.Engine.Common;
using Sharp2D.Engine.Common.Components.Sprites;
using Sharp2D.Engine.Common.Scene;
using Sharp2D.Engine.Common.UI.Controls;
using Sharp2D.Engine.Common.UI.Enums;
using Sharp2D.Engine.Common.World;
using Sharp2D.Engine.Common.World.Camera;
using Sharp2D.Engine.Helper;
using Sharp2D.Engine.Infrastructure.Serialization;
using Sharp2D.Engine.Utility;
using Sharp2D.Windows;
using SharpTetris.UI;
using SharpTetris.World;
using Label = Sharp2D.Engine.Common.UI.Controls.Label;

namespace SharpTetris
{
    public class TetrisGame : Sharp2DWindowsApp
    {
        public TetrisGame()
            : base(new DefaultGameBootstrapper())
        {
        }

        //public override Sharp2DGame CreateGame()
        //{
        //    Sharp2DGame game = base.CreateGame();
        //    game.ContentLoaded += OnContentLoaded;

        //    game.Graphics.PreparingDeviceSettings += (sender, args) =>
        //    {
        //        Form form = (Form)Control.FromHandle(game.Window.Handle);
        //        float screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
        //        float screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
        //        float gameWidth = game.Graphics.PreferredBackBufferWidth;
        //        float gameHeight = game.Graphics.PreferredBackBufferHeight;
        //        form.SetDesktopLocation((int)(screenWidth / 2 - gameWidth / 2), (int)(screenHeight / 2 - gameHeight / 2));
        //    };

        //    return game;
        //}

        public override IGameHost CreateGame(SharpGameManager gameManager)
        {
            var game = base.CreateGame(gameManager);
            SharpGameManager.ContentLoaded += (sender, args) => this.OnContentLoaded();
            return game;
        }

        private void OnContentLoaded()
        {
            var sceneData = new SceneData();
            var tetrisGameWorld = new TetrisGameWorld
            {
                Children =
                {
                    new Camera {MainCamera = true}
                },
            };
            tetrisGameWorld.Components.Add(Sprite.Load("bgimg"));
            tetrisGameWorld.Sprite.CenterObject();
            sceneData.WorldRoot.Add(tetrisGameWorld);

            var label = new Label(new FontDefinition("TNR", 24), new Vector2(1000, 70))
            {
                Text = "Score: 0",
                Tint = Color.White,
                Alignment = TextAlignment.Center
            };
            var tetrisGameUi = new TetrisGameUi
            {
                World = tetrisGameWorld,
                ScoreLabel = label,
                Children =
                {
                    label
                }
            };
            sceneData.UiRoot.Add(tetrisGameUi);

            GameManager.StartScene = new Scene { SceneData = sceneData };
        }
    }
}