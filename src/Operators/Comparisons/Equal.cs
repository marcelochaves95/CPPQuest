namespace Sesamo.Operators.Comparisons
{
    public class Equal : Comparison
    {
        private const string _chain = "==";
        public override Chain Chain => new Chain(_chain);

        public Equal()
        {
        }

        public Equal(int lineNumber)
        {
            Line = lineNumber;
        }
    }
}