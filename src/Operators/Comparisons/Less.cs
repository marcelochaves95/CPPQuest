namespace Sesamo.Operators.Comparisons
{
    public class Less : Comparison
    {
        private const string _chain = "<";
        public override Chain Chain => new Chain(_chain);

        public Less()
        {
        }

        public Less(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}