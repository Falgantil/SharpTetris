namespace SharpTetris.World.Blocks
{
    public class CollisionDetail
    {
        public bool Collided { get; set; }

        public Block Block { get; set; }

        public bool OutsideBounds { get; set; }

        public CollisionDetail(bool collided, Block block, bool outsideBounds)
        {
            Collided = collided;
            Block = block;
            OutsideBounds = outsideBounds;
        }

        public CollisionDetail()
        {

        }

        public bool CanMove
        {
            get { return !Collided && !OutsideBounds; }
        }
    }
}