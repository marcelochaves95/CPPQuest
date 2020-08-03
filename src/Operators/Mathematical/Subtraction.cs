namespace Sesamo.Operators.Mathematical
{
    public class Subtraction : Mathematics
    {
        private const string _chain = "-";
        public override Chain Chain => new Chain(_chain);

        public Subtraction()
        {
        }

        public Subtraction(int lineNumber)
        {
            Line = lineNumber;
        }
    }
}