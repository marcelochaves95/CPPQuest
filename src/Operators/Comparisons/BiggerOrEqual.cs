namespace Sesamo.Operators.Comparisons
{
    public class BiggerOrEqual : Comparison, IOperator
    {
        private const string _chain = ">=";
        public override Chain Chain => new Chain(_chain);

        public BiggerOrEqual()
        {
        }

        public BiggerOrEqual(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}