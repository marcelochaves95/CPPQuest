using Sesamo.Tokens;

namespace Sesamo.Operators
{
    public abstract class Operator : Token
    {
        public abstract Chain Chain { get; }
    }
}