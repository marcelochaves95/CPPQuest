namespace Sesamo.Operators.Mathematical
{
    public class Addition : Mathematics
    {
        private const string _chain = "+";
        public override Chain Chain => new Chain(_chain);

        public Addition()
        {
        }

        public Addition(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}