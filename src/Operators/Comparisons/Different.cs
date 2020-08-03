namespace Sesamo.Operators.Comparisons
{
    public class Different : Comparison
    {
        private const string _chain = "!=";
        public override Chain Chain => new Chain(_chain);

        public Different()
        {
        }

        public Different(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}