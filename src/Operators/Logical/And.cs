namespace Sesamo.Operators.Logical
{
    public class And : Logic
    {
        private const string _chain = "and";
        public override Chain Chain => new Chain(_chain);

        public And()
        {
        }

        public And(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}