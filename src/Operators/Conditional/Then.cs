namespace Sesamo.Operators.Conditional
{
    public class Then : Operator
    {
        private const string _chain = "then";
        public override Chain Chain => new Chain(_chain);

        public Then()
        {
        }

        public Then(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}