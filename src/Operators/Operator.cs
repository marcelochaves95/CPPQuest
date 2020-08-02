using Sesamo.Tokens;

namespace Sesamo.Operators
{
    public abstract class Operator : Token, IOperator
    {
        #region IOperador Members

        public abstract Chain Chain { get; }

        #endregion
    }
}