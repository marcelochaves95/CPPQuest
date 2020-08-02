using Sesamo.Tokens;

namespace Sesamo.Operators
{
    public abstract class Operador : Token, IOperador
    {
        #region IOperador Members

        public abstract Cadeia Cadeia { get; }

        #endregion
    }
}