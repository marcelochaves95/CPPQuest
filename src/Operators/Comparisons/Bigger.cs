namespace Sesamo.Operators.Comparisons
{
    public class Bigger : Comparison, IOperator
    {
        private const string _chain = ">";
        public override Chain Chain => new Chain(_chain);

        public Bigger()
        {
        }

        public Bigger(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}