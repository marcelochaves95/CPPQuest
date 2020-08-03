namespace Sesamo.Operators.Mathematical
{
    public class Multiplication : Mathematics
    {
        private const string _chain = "*";
        public override Chain Chain => new Chain(_chain);

        public Multiplication()
        {
        }

        public Multiplication(int lineNumber)
        {
            Line = lineNumber;
        }
    }
}