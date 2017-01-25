using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sharp2D.Engine.Common.Components;
using Sharp2D.Engine.Common.Components.Sprites;
using Sharp2D.Engine.Utility;
using SharpTetris.World.Blocks;

namespace SharpTetris.World.UserControls
{
    public class BlockHandler : SharpComponent
    {
        private static readonly Vector2 RootPosition = new Vector2(0, -300);
        private List<BlockType> _remaining;

        private Random _random;
        private TimeSpan _goDownTimer;
        private TimeSpan _goLeftTimer;
        private TimeSpan _goRightTimer;

        public TetrisGameWorld GameWorld { get; set; }

        public BigBlock CurrentBlock { get; set; }

        public override void Initialize()
        {
            _remaining = new List<BlockType>();
            _random = new Random();

            FillBlockList();
        }

        private void FillBlockList()
        {
            Array values = Enum.GetValues(typeof(BlockType));
            foreach (BlockType value in values.Cast<BlockType>().Where(value => value != BlockType.None))
                _remaining.Add(value);
        }

        public void CreateNewBlock()
        {
            int index = _random.Next(0, _remaining.Count);
            BlockType blockType = _remaining[index];
            _remaining.RemoveAt(index);

            CreateOfType(blockType);

            if (_remaining.Count == 0)
                FillBlockList();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyPressed(Keys.LeftShift))
            {
                GameWorld.SaveBlock(CurrentBlock);
                return;
            }

            if (InputManager.IsKeyPressed(Keys.Space))
            {
                CurrentBlock.HardDropped = true;
                bool canMove;
                do
                {
                    canMove = CurrentBlock.MoveBlock(0, 1);
                } while (canMove);
                GameWorld.PerformTick();
                return;
            }

            if (InputManager.IsKeyDown(Keys.Down) && _goDownTimer <= TimeSpan.Zero)
            {
                _goDownTimer = TimeSpan.FromMilliseconds(100);
                CurrentBlock.MoveBlock(0, 1);
                CurrentBlock.MovedThisUpdate = true;
                return;
            }

            if (InputManager.IsKeyDown(Keys.Left) && _goLeftTimer <= TimeSpan.Zero && !CurrentBlock.HardDropped)
            {
                _goLeftTimer = TimeSpan.FromMilliseconds(150);
                CurrentBlock.MoveBlock(-1, 0);
            }
            if (InputManager.IsKeyDown(Keys.Right) && _goRightTimer <= TimeSpan.Zero && !CurrentBlock.HardDropped)
            {
                _goRightTimer = TimeSpan.FromMilliseconds(150);
                CurrentBlock.MoveBlock(1, 0);
            }

            if (InputManager.IsKeyReleased(Keys.Left))
                _goLeftTimer = TimeSpan.Zero;
            if (InputManager.IsKeyReleased(Keys.Right))
                _goRightTimer = TimeSpan.Zero;

            if (_goDownTimer > TimeSpan.Zero)
                _goDownTimer -= gameTime.ElapsedGameTime;

            if (_goLeftTimer > TimeSpan.Zero)
                _goLeftTimer -= gameTime.ElapsedGameTime;

            if (_goRightTimer > TimeSpan.Zero)
                _goRightTimer -= gameTime.ElapsedGameTime;

            if (InputManager.IsKeyPressed(Keys.Up) && !CurrentBlock.HardDropped)
                CurrentBlock.Rotate(RotateType.Clockwise);
        }

        public override void Draw(SpriteBatch batch, GameTime time)
        {

        }

        public void CreateOfType(BlockType stashedBlock)
        {
            var mapPosition = new Vector2(5, 1);
            Sprite sprite = Sprite.Load("Block");
            CurrentBlock = new BigBlock(GameWorld, mapPosition, RootPosition, sprite, stashedBlock);
            GameWorld.Add(CurrentBlock);
        }
    }

    public enum RotateType
    {
        Clockwise,
        CounterClock
    }
}