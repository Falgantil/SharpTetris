using System.Linq;
using Microsoft.Xna.Framework;
using Sharp2D.Engine.Common.Components.Sprites;
using Sharp2D.Engine.Common.World;
using Sharp2D.Engine.Helper;
using SharpTetris.World.Helpers;

namespace SharpTetris.World.Blocks
{
    public class GhostBlock : WorldObject
    {
        public BigBlock BlockGhoster { get; private set; }

        public GhostBlock(BigBlock blockGhoster)
        {
            if (blockGhoster == null)
                return;

            BlockGhoster = blockGhoster;

            BlockHelper.CreateBlock(blockGhoster.BlockType, (position, color) =>
            {
                Sprite sprite = Sharp2D.Engine.Common.Components.Sprites.Sprite.Load("Block");
                sprite.CenterObject();
                Add(new WorldObject(position * TetrisGameWorld.BlockSize, sprite));
            });

            BlockGhoster.Rotated += (sender, i) =>
            {
                foreach (WorldObject child in Children.OfType<WorldObject>())
                    child.LocalPosition = SharpMathHelper.Rotate(child.LocalPosition, Vector2.Zero, i);
            };
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (BlockGhoster == null)
                return;

            LocalPosition = BlockGhoster.GlobalPosition;

            int distanceToMove = 0;

            while (true)
            {
                distanceToMove++;
                if (BlockGhoster.CanMove(0, distanceToMove))
                    LocalPosition += new Vector2(0, TetrisGameWorld.BlockSize.Y);
                else
                    break;
            }
        }
    }
}