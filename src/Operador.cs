namespace Linguagem
{
    public class Operador : Token, IOperador
    {
        #region IOperador Members

        public abstract Cadeia Cadeia { get; }

        #endregion
    }
}