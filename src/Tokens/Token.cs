using Sesamo.Operators;
using Sesamo.Variables;

namespace Sesamo.Tokens
{
    public abstract class Token
    {
        private int _linha;
        public int Linha
        {
            get { return _linha; }
            set { _linha = value; }
        }

        private string _texto;
        public string Texto
        {
            get
            {
                if (this is Value)
                {
                    _texto = ((Value) this).NomeVariavel != null ? ((Value) this).NomeVariavel : ((Value) this).ValorVariavel.ToString();
                }
                else
                {
                    _texto = ((Operator) this).Chain.Value;
                }

                return _texto;
            }
        }
    }
}