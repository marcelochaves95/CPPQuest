using System;
using System.Collections.Generic;
using Sesamo.Operators.Comparisons;
using Sesamo.Operators.Conditional;
using Sesamo.Operators.Logical;
using Sesamo.Operators.Mathematical;
using Sesamo.Tokens;
using Sesamo.Variables;

namespace Sesamo.Analysis
{
    public class Lexical
    {
        private string _space = "º";
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => _errorMessage = value;
        }

        private Variable variable = null;
        public Variable Variable => variable;

        private List<Token> _sourceCode;
        public List<Token> SourceCode
        {
            get
            {
                if (_sourceCode == null)
                {
                    _sourceCode = new List<Token>();
                }

                return _sourceCode;
            }
        }

        public bool Validate(string code, List<Value> variables)
        {
            string reassembledCode = "";
            bool insideString = false;
            for (int i = 0; i < code.Length; i++)
            {
                char letter = code[i];
                char previousLetter = new char();
                char nextLetter = new char();

                if (i > 0)
                {
                    previousLetter = code[i - 1];
                }

                if (i < code.Length - 1)
                {
                    nextLetter = code[i + 1];
                }

                if (letter == '"')
                {
                    if (!insideString)
                    {
                        insideString = true;
                    }
                }

                if (letter == '\n' && insideString)
                {
                    break;
                }
                else if (insideString)
                {
                    reassembledCode += letter.ToString();
                }
                else if (letter == ' ')
                {
                    if (previousLetter != ' ' && previousLetter != '\n' && nextLetter != '\n')
                    {
                        reassembledCode += _space;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (insideString)
                {
                    _errorMessage = "String of values not closed.";
                    return false;
                }

                code = reassembledCode;
                code = code.Replace("\n", _space + "\n" + _space);
                string[] tokens = code.Split(Convert.ToChar(_space));
                int line = 1;
                foreach (string token in tokens)
                {
                    string value = token != "\n" ? token.Trim() : token;
                    if (value == "\n")
                    {
                        line++;
                        continue;
                    }
                    else if (value == "")
                    {
                        continue;
                    }

                    variable = new Variable(variables);
                    if (value[0] == '"')
                    {
                        SourceCode.Add(new Value(value, Types.Text, line));
                    }
                    else if (Int64.TryParse(value, out long convertedNumber))
                    {
                        SourceCode.Add(new Value(convertedNumber.ToString(), Types.Decimal, line));
                    }
                    else if (variable.ExisteVariavel(value))
                    {
                        Value valueVariable = variable.getVariavel(value).Copy();
                        valueVariable.Line = line;
                        SourceCode.Add(valueVariable);
                    }
                    else if (new If().Chain.Value == value)
                    {
                        SourceCode.Add(new If(line));
                    }
                    else if (new Then().Chain.Value == value)
                    {
                        SourceCode.Add(new Then(line));
                    }
                    else if (new Else().Chain.Value == value)
                    {
                        SourceCode.Add(new Else(line));
                    }
                    else if (new EndIf().Chain.Value == value)
                    {
                        SourceCode.Add(new EndIf(line));
                    }
                    else if (new Equal().Chain.Value == value)
                    {
                        SourceCode.Add(new Equal(line));
                    }
                    else if (new Different().Chain.Value == value)
                    {
                        SourceCode.Add(new Different(line));
                    }
                    else if (new Bigger().Chain.Value == value)
                    {
                        SourceCode.Add(new Bigger(line));
                    }
                    else if (new Less().Chain.Value == value)
                    {
                        SourceCode.Add(new Less(line));
                    }
                    else if (new BiggerOrEqual().Chain.Value == value)
                    {
                        SourceCode.Add(new BiggerOrEqual(line));
                    }
                    else if (new LessOrEqual().Chain.Value == value)
                    {
                        SourceCode.Add(new LessOrEqual(line));
                    }
                    else if (new Addition().Chain.Value == value)
                    {
                        SourceCode.Add(new Addition(line));
                    }
                    else if (new Subtraction().Chain.Value == value)
                    {
                        SourceCode.Add(new Subtraction(line));
                    }
                    else if (new Multiplication().Chain.Value == value)
                    {
                        SourceCode.Add(new Multiplication(line));
                    }
                    else if (new Division().Chain.Value == value)
                    {
                        SourceCode.Add(new Division(line));
                    }
                    else if (new Or().Chain.Value == value)
                    {
                        SourceCode.Add(new Or(line));
                    }
                    else if (new And().Chain.Value == value)
                    {
                        SourceCode.Add(new And(line));
                    }
                    else if (value == "")
                    {
                        continue;
                    }
                    else
                    {
                        _errorMessage = $"{value} symbol not recognized on line {line}.";
                    }
                }
            }

            return true;
        }
    }
}