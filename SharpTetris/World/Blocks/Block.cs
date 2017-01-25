using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Sharp2D.Engine.Common;
using Sharp2D.Engine.Common.Components.Sprites;
using Sharp2D.Engine.Common.World;

namespace SharpTetris.World.Blocks
{
    public class Block : WorldObject
    {
        private readonly TetrisGameWorld _gameWorld;
        private Vector2 _mapPosition;

        public Vector2 MapPosition
        {
            get { return _mapPosition; }
            set
            {
                int x = (int)_mapPosition.X, y = (int)_mapPosition.Y;
                if (_gameWorld.Map[x, y] == this)
                    _gameWorld.Map[x, y] = null;

                _mapPosition = new Vector2((int)Math.Round(value.X), (int)Math.Round(value.Y));

                x = (int)_mapPosition.X;
                y = (int)_mapPosition.Y;
                _gameWorld.Map[x, y] = this;
            }
        }

        public Block(TetrisGameWorld gameWorld, Vector2 mapPosition, Vector2 localPosition, Sprite sprite, params WorldObject[] children)
            : base(localPosition, sprite, children)
        {
            _gameWorld = gameWorld;
            _mapPosition = mapPosition;
        }

        public Block()
        {
        }
    }
}
