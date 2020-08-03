namespace Sesamo.Operators.Mathematical
{
    public class Division : Mathematics
    {
        private const string _chain = @"/";
        public override Chain Chain => new Chain(_chain);

        public Division()
        {
        }

        public Division(int lineNumber)
        {
            Line = lineNumber;
        }
    }
}