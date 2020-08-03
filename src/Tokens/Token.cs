using Sesamo.Operators;
using Sesamo.Variables;

namespace Sesamo.Tokens
{
    public abstract class Token
    {
        private int _line;
        public int Line
        {
            get => _line;
            set => _line = value;
        }

        private string _text;
        public string Text
        {
            get
            {
                if (this is Value)
                {
                    _text = (this as Value)?.NomeVariavel != null ? (this as Value)?.NomeVariavel : (this as Value)?.ValorVariavel;
                }
                else
                {
                    _text = (this as Operator)?.Chain.Value;
                }

                return _text;
            }
        }
    }
}