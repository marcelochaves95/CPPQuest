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
                    _text = (this as Value)?.VariableName != null ? (this as Value)?.VariableName : (this as Value)?.VariableValue;
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