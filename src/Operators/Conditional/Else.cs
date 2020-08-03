namespace Sesamo.Operators.Conditional
{
    public class Else : Operator
    {
        private const string _chain = "else";
        public override Chain Chain => new Chain(_chain);

        public Else()
        {
        }

        public Else(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}