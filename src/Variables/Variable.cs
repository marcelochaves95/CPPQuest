using System.Collections.Generic;

namespace Sesamo.Variables
{
    public class Variable
    {
        private List<Value> _variables;
        public List<Value> Variables => _variables;

        public Variable(List<Value> variables)
        {
            _variables = variables;
        }

        public Value GetVariable(string name)
        {
            Value value = null;
            foreach (Value variable in _variables)
            {
                if (variable.VariableName == name)
                {
                    value = variable;
                    break;
                }
            }

            return value;
        }

        public void AddVariable(Value variable)
        {
            _variables.Add(variable);
        }

        public bool ContainsVariable(Value variable)
        {
            bool validator = false;
            foreach (Value value in _variables)
            {
                if (value.VariableName == variable.VariableName)
                {
                    validator = true;
                    break;
                }
            }

            return validator;
        }

        public bool ContainsVariable(string variable)
        {
            Value value = new Value(variable, "0", null);
            return ContainsVariable(value);
        }
    }
}