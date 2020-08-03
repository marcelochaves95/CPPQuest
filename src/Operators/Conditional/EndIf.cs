namespace Sesamo.Operators.Conditional
{
    public class EndIf : Operator
    {
        private const string _chain = "endif";
        public override Chain Chain => new Chain(_chain);

        public EndIf()
        {
        }

        public EndIf(int lineNumber)
        {
            Line = lineNumber;
        }
    }
}