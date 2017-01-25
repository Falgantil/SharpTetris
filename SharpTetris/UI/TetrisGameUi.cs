using System;
using Microsoft.Xna.Framework;
using Sharp2D.Engine.Common;
using Sharp2D.Engine.Common.Components.Sprites;
using Sharp2D.Engine.Common.ObjectSystem;
using Sharp2D.Engine.Common.UI.Controls;
using Sharp2D.Engine.Common.World;
using SharpTetris.World;
using SharpTetris.World.Blocks;
using SharpTetris.World.EventArgs;
using SharpTetris.World.Helpers;

namespace SharpTetris.UI
{
    public class TetrisGameUi : GameObject
    {
        public TetrisGameUi()
        {

        }

        public Label ScoreLabel { get; set; }

        public TetrisGameWorld World { get; set; }

        public override void Initialize()
        {
            base.Initialize();

            World.ScoreChanged += World_ScoreChanged;
            World.StashedBlockChanged += World_StashedBlockChanged;

            _currentStashed = new WorldObject(new Vector2(890, 110), null);
            Add(_currentStashed);
        }

        private WorldObject _currentStashed;

        private void World_StashedBlockChanged(object sender, StashedBlockChangedArgs stashedBlockChangedArgs)
        {
            Vector2 blockSize = TetrisGameWorld.BlockSize;

            _currentStashed.Children.Clear();
            Action<Vector2, Color> action = (position, color) => _currentStashed.Add(new WorldObject(position*blockSize, GetSprite(color)));
            BlockHelper.CreateBlock(stashedBlockChangedArgs.NewType, action);
        }

        private static Sprite GetSprite(Color color)
        {
            Sprite sprite = Sprite.Load("Block");
            sprite.Tint = color;
            return sprite;
        }

        private void World_ScoreChanged(object sender, ScoreChangedArgs scoreChangedArgs)
        {
            ScoreLabel.Text = string.Format("Score: {0}", scoreChangedArgs.NewScore);
        }
    }
}