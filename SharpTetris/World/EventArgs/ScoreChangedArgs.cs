namespace SharpTetris.World.EventArgs
{
    public class ScoreChangedArgs : System.EventArgs
    {
        public ScoreChangedArgs(int oldScore, int newScore)
        {
            OldScore = oldScore;
            NewScore = newScore;
        }

        public ScoreChangedArgs()
        {
        }

        public int OldScore { get; set; }

        public int NewScore { get; set; }
    }
}