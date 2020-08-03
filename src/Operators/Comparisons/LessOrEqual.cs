namespace Sesamo.Operators.Comparisons
{
    public class LessOrEqual : Comparison
    {
        private const string _chain = "<=";
        public override Chain Chain => new Chain(_chain);

        public LessOrEqual()
        {
        }

        public LessOrEqual(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}