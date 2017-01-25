using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Sharp2D.Engine.Common;
using Sharp2D.Engine.Common.Components.Sprites;
using Sharp2D.Engine.Common.World;
using Sharp2D.Engine.Helper;
using SharpTetris.World.Helpers;
using SharpTetris.World.UserControls;

namespace SharpTetris.World.Blocks
{
    public class BigBlock : WorldObject
    {
        private readonly TetrisGameWorld _gameWorld;

        public BigBlock(TetrisGameWorld gameWorld, Vector2 mapPosition, Vector2 localPosition, Sprite blockSprite,
            BlockType blockType)
            : base(localPosition, null)
        {
            _gameWorld = gameWorld;
            MapPosition = mapPosition;
            BlockSprite = blockSprite;
            BlockType = blockType;

            GenerateBlock();
        }

        public Sprite BlockSprite { get; set; }

        public BlockType BlockType { get; set; }

        public Vector2 MapPosition { get; set; }

        public bool MovedThisUpdate { get; set; }

        public bool HardDropped { get; set; }

        private void GenerateBlock()
        {
            BlockHelper.CreateBlock(BlockType, (position, color) => AddBlock(MapPosition + position, position, color));
        }

        private void AddBlock(Vector2 mapPosition, Vector2 position, Color color)
        {
            float x = position.X * BlockSprite.Width;
            float y = position.Y * BlockSprite.Height;
            Sprite sprite = Sprite.Load(BlockSprite.AssetName);
            sprite.Tint = color;
            var newBlock = new Block(_gameWorld, mapPosition, V2(x, y), sprite);
            newBlock.Initialize();
            newBlock.Sprite.CenterObject();
            Add(newBlock);
        }

        public bool MoveBlock(float x, float y)
        {
            bool canMove = CanMove(x, y);

            if (!canMove)
                return false;

            Move(TetrisGameWorld.BlockSize.X * x, TetrisGameWorld.BlockSize.Y * y);

            MapPosition += V2(x, y);

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (Block block in Children)
                block.MapPosition += V2(x, y);

            return true;
        }

        public bool CanMove(float x, float y)
        {
            if (!Children.Any())
                return false;

            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (Block block in Children)
            {
                var blockX = block.MapPosition.X + x;
                var blockY = block.MapPosition.Y + y;
                var collisionDetail = block.CanMove(blockX, blockY, _gameWorld.Map);

                if ((collisionDetail.Collided && !Children.Contains(collisionDetail.Block) || collisionDetail.OutsideBounds))
                    return false;
            }

            return true;
        }

        public event EventHandler<int> Rotated;

        protected virtual void OnRotated(int e)
        {
            EventHandler<int> handler = Rotated;
            if (handler != null) handler(this, e);
        }

        public bool Rotate(RotateType rotateType)
        {
            if (BlockType == BlockType.O)
                return false;

            float angleDegree = (rotateType == RotateType.Clockwise ? 90 : -90);

            for (int i = 0; i < 4; i++)
            {
                var block = (Block)Children[i];
                Vector2 blockMapPos = SharpMathHelper.Rotate(block.MapPosition, MapPosition, angleDegree);
                var collisionDetail = block.CanMove(blockMapPos.X, blockMapPos.Y, _gameWorld.Map);
                if (collisionDetail.OutsideBounds || (collisionDetail.Collided && !Children.Contains(collisionDetail.Block)))
                    return false;
            }

            Debug.WriteLine("Rotating: {0}", BlockType);

            for (int i = 0; i < 4; i++)
            {
                var block = (Block)Children[i];
                Vector2 blockMapPos = SharpMathHelper.Rotate(block.MapPosition, MapPosition, angleDegree);
                block.MapPosition = blockMapPos;
                block.LocalPosition = SharpMathHelper.Rotate(block.LocalPosition, Vector2.Zero, angleDegree);
            }

            OnRotated((int) angleDegree);

            return true;
        }
    }
}