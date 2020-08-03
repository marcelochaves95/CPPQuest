namespace Sesamo.Operators.Logical
{
    public class Or : Logic
    {
        private const string _chain = "or";
        public override Chain Chain => new Chain(_chain);

        public Or()
        {
        }

        public Or(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}