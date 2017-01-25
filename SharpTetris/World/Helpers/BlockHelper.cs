using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SharpTetris.World.Blocks;

namespace SharpTetris.World.Helpers
{
    public static class BlockHelper
    {
        public static CollisionDetail CanMove(this Block block, float x, float y, Block[,] map)
        {
            var intX = (int)Math.Round(x);
            var intY = (int)Math.Round(y);

            if (0 > intX || map.GetLength(0) <= intX)
                return new CollisionDetail(false, null, true);
            if (0 > intY || map.GetLength(1) <= intY)
                return new CollisionDetail(false, null, true);

            Block mapBlock = map[intX, intY];

            if (mapBlock != null && mapBlock != block)
                return new CollisionDetail(true, mapBlock, false);

            return new CollisionDetail(false, null, false);
        }

        public static void CreateBlock(BlockType type, Action<Vector2, Color> action)
        {
            switch (type)
            {
                case BlockType.L:
                    action(new Vector2(0, -1), Color.Orange);
                    action(new Vector2(0, 0), Color.Orange);
                    action(new Vector2(0, 1), Color.Orange);
                    action(new Vector2(1, 1), Color.Orange);
                    break;
                case BlockType.I:
                    action(new Vector2(0, -1), Color.DeepSkyBlue);
                    action(new Vector2(0, 0), Color.DeepSkyBlue);
                    action(new Vector2(0, 1), Color.DeepSkyBlue);
                    action(new Vector2(0, 2), Color.DeepSkyBlue);
                    break;
                case BlockType.T:
                    action(new Vector2(0, -1), Color.Purple);
                    action(new Vector2(0, 0), Color.Purple);
                    action(new Vector2(1, 0), Color.Purple);
                    action(new Vector2(-1, 0), Color.Purple);
                    break;
                case BlockType.O:
                    action(new Vector2(0, 1), Color.Yellow);
                    action(new Vector2(0, 0), Color.Yellow);
                    action(new Vector2(1, 0), Color.Yellow);
                    action(new Vector2(1, 1), Color.Yellow);
                    break;
                case BlockType.J:
                    action(new Vector2(0, -1), Color.Blue);
                    action(new Vector2(0, 0), Color.Blue);
                    action(new Vector2(0, 1), Color.Blue);
                    action(new Vector2(-1, 1), Color.Blue);
                    break;
                case BlockType.S:
                    action(new Vector2(1, 0), Color.Green);
                    action(new Vector2(0, 0), Color.Green);
                    action(new Vector2(0, 1), Color.Green);
                    action(new Vector2(-1, 1), Color.Green);
                    break;
                case BlockType.Z:
                    action(new Vector2(-1, 0), Color.Red);
                    action(new Vector2(0, 0), Color.Red);
                    action(new Vector2(0, 1), Color.Red);
                    action(new Vector2(1, 1), Color.Red);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}
