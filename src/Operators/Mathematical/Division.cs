namespace Sesamo.Operators.Mathematical
{
    public class Division : Mathematics, IOperator
    {
        private const string _chain = @"/";
        public override Chain Chain => new Chain(_chain);

        public Division()
        {
        }

        public Division(int lineNumber)
        {
            Linha = lineNumber;
        }
    }
}