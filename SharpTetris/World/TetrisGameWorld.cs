using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sharp2D.Engine.Common.World;
using SharpTetris.World.Blocks;
using SharpTetris.World.EventArgs;
using SharpTetris.World.UserControls;

namespace SharpTetris.World
{
    public class TetrisGameWorld : WorldObject
    {
        public static readonly Vector2 BlockSize = new Vector2(32, 32);

        private TimeSpan _currentRemaining;
        private int _score;
        private BlockType _stashedBlock;
        private TimeSpan _tickDelay;
        private bool _usedSave;

        private GhostBlock _ghostBlock;

        public TetrisGameWorld()
        {
            Blocks = new List<Block>();

            _tickDelay = TimeSpan.FromMilliseconds(500);
            Map = new Block[10, 20];
        }

        public Block[,] Map { get; private set; }

        public List<Block> Blocks { get; private set; }

        public BlockHandler BlockHandler { get; private set; }

        public int Score
        {
            get { return _score; }
            set
            {
                int oldScore = _score;
                _score = value;

                if (oldScore != _score)
                    OnScoreChanged(new ScoreChangedArgs(oldScore, _score));
            }
        }

        public BlockType StashedBlock
        {
            get { return _stashedBlock; }
            private set
            {
                BlockType oldType = _stashedBlock;
                _stashedBlock = value;
                OnStashedBlockChanged(new StashedBlockChangedArgs(oldType, _stashedBlock));
            }
        }

        public override void Initialize()
        {
            _currentRemaining = _tickDelay;

            Components.Add(BlockHandler = new BlockHandler { GameWorld = this });

            ScoreChanged += (sender, args) =>
            {
                if (args.NewScore % 1000 == 0)
                    _tickDelay = _tickDelay - TimeSpan.FromMilliseconds(50);
            };

            base.Initialize();
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            _currentRemaining -= time.ElapsedGameTime;

            if (_currentRemaining < TimeSpan.Zero)
                PerformTick();
        }

        public void PerformTick()
        {
            if (!Children.Any(x => x is BigBlock))
            {
                if (_ghostBlock != null)
                    Remove(_ghostBlock);

                BlockHandler.CreateNewBlock();

                _ghostBlock = new GhostBlock(BlockHandler.CurrentBlock);
                Insert(0, _ghostBlock);
                return;
            }

            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (BigBlock bigBlock in Children.Where(x => x is BigBlock).ToArray())
            {
                bool moveBlock = bigBlock.CanMove(0, 1);

                if (!bigBlock.MovedThisUpdate)
                    moveBlock = bigBlock.MoveBlock(0, 1);

                if (!moveBlock || bigBlock.HardDropped)
                {
                    FinishBlock(bigBlock);

                    while (HandleFinishedLines())
                    {
                    }
                }

                bigBlock.HardDropped = false;
                bigBlock.MovedThisUpdate = false;
            }

            _currentRemaining = _tickDelay;
            _usedSave = false;
        }

        private bool HandleFinishedLines()
        {
            for (int y = Map.GetLength(1) - 1; y >= 0; y--)
            {
                int length = Map.GetLength(0);
                bool lineFilled = true;

                for (int x = 0; x < length; x++)
                {
                    if (Map[x, y] == null)
                        lineFilled = false;
                }

                if (lineFilled)
                {
                    ClearLine(y);
                    return true;
                }
            }

            return false;
        }

        private void ClearLine(int line)
        {
            int length = Map.GetLength(0);

            for (int x = 0; x < length; x++)
            {
                Block block = Map[x, line];
                Children.Remove(block);
                Map[x, line] = null;
            }

            for (int y = line - 1; y >= 0; y--)
            {
                for (int x = 0; x < length; x++)
                {
                    Block block = Map[x, y];
                    if (block == null)
                        continue;

                    block.MapPosition = new Vector2(block.MapPosition.X, block.MapPosition.Y + 1);
                    block.Move(0, BlockSize.Y);
                }
            }

            Score += 250;
        }

        private void FinishBlock(BigBlock bigBlock)
        {
            if (_ghostBlock != null)
                Remove(_ghostBlock);

            BigBlock currentBlock = BlockHandler.CurrentBlock;
            IList<Block> blocks = currentBlock.Children.Cast<Block>().ToList();

            foreach (Block block in blocks)
            {
                block.LocalPosition = block.GlobalPosition;
                Children.Add(block);
            }

            Children.Remove(bigBlock);

            BlockHandler.CurrentBlock = null;
            BlockHandler.CreateNewBlock();

            _ghostBlock = new GhostBlock(BlockHandler.CurrentBlock);
            Insert(0, _ghostBlock);
        }

        public event EventHandler<ScoreChangedArgs> ScoreChanged;

        protected virtual void OnScoreChanged(ScoreChangedArgs e)
        {
            EventHandler<ScoreChangedArgs> handler = ScoreChanged;
            if (handler != null) handler(this, e);
        }

        public void SaveBlock(BigBlock currentBlock)
        {
            if (_usedSave)
                return;

            if (_ghostBlock != null)
                Remove(_ghostBlock);

            _usedSave = true;

            Children.Remove(currentBlock);

            foreach (Block child in currentBlock.Children.OfType<Block>())
                Map[(int)child.MapPosition.X, (int)child.MapPosition.Y] = null;

            if (StashedBlock != BlockType.None)
                BlockHandler.CreateOfType(StashedBlock);

            StashedBlock = currentBlock.BlockType;

            _ghostBlock = new GhostBlock(BlockHandler.CurrentBlock);
            Insert(0, _ghostBlock);
        }

        public event EventHandler<StashedBlockChangedArgs> StashedBlockChanged;

        protected virtual void OnStashedBlockChanged(StashedBlockChangedArgs e)
        {
            EventHandler<StashedBlockChangedArgs> handler = StashedBlockChanged;
            if (handler != null) handler(this, e);
        }
    }
}