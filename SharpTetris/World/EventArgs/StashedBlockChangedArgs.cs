using SharpTetris.World.Blocks;

namespace SharpTetris.World.EventArgs
{
    public class StashedBlockChangedArgs : System.EventArgs
    {
        public BlockType OldType { get; set; }

        public BlockType NewType { get; set; }

        public StashedBlockChangedArgs(BlockType oldType, BlockType newType)
        {
            OldType = oldType;
            NewType = newType;
        }

        public StashedBlockChangedArgs()
        {
            
        }
    }
}