using Sesamo.Tokens;

namespace Sesamo.Variables
{
    public class Value : Token
    {
        private string _variableName;
        public string VariableName
        {
            get => _variableName;
            set => _variableName = value;
        }

        private string _variableValue;
        public string VariableValue
        {
            get => _variableValue;
            set => _variableValue = value;
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => _type = value;
        }

        public Value(string variableName, string variableValue, string typeValue)
        {
            _variableName = variableName;
            _variableValue = variableValue;
            _type = typeValue;
        }

        public Value(string variableValue, string typeValue, int lineNumber)
        {
            _variableValue = variableValue;
            _type = typeValue;
            Line = lineNumber;
        }

        public Value Copy()
        {
            Value copy = new Value(VariableValue, Type, Line)
            {
                VariableName = VariableName
            };

            return copy;
        }
    }
}