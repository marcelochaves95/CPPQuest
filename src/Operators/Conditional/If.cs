namespace Sesamo.Operators.Conditional
{
    public class If : Operator
    {
        private const string _chain = "if";
        public override Chain Chain => new Chain(_chain);

        public If()
        {
        }

        public If(int lineNumber)
        {
            Line = lineNumber;
        }
    }
}