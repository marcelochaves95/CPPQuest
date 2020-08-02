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
            for (int pos1 = 0; pos1 < code.Length; pos1++)
            {
                char letter = code[pos1];
                char previousLetter = new char();
                char nextLetter = new char();

                if (pos1 > 0)
                {
                    previousLetter = code[pos1 - 1];
                }

                if (pos1 < code.Length - 1)
                {
                    nextLetter = code[pos1 + 1];
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
                        SourceCode.Add(new Value(value, Types.Txt, line));
                    }
                    else if (Int64.TryParse(value, out long convertedNumber))
                    {
                        SourceCode.Add(new Value(convertedNumber.ToString(), Types.Dec, line));
                    }
                    else if (variable.ExisteVariavel(value))
                    {
                        Value valueVariable = variable.getVariavel(value).Copia();
                        valueVariable.Linha = line;
                        SourceCode.Add(valueVariable);
                    }
                    else if (new If().Chain.Valor == value)
                    {
                        SourceCode.Add(new If(line));
                    }
                    else if (new Then().Chain.Valor == value)
                    {
                        SourceCode.Add(new Then(line));
                    }
                    else if (new Else().Chain.Valor == value)
                    {
                        SourceCode.Add(new Else(line));
                    }
                    else if (new EndIf().Chain.Valor == value)
                    {
                        SourceCode.Add(new EndIf(line));
                    }
                    else if (new Equal().Chain.Valor == value)
                    {
                        SourceCode.Add(new Equal(line));
                    }
                    else if (new Different().Chain.Valor == value)
                    {
                        SourceCode.Add(new Different(line));
                    }
                    else if (new Bigger().Chain.Valor == value)
                    {
                        SourceCode.Add(new Bigger(line));
                    }
                    else if (new Less().Chain.Valor == value)
                    {
                        SourceCode.Add(new Less(line));
                    }
                    else if (new BiggerOrEqual().Chain.Valor == value)
                    {
                        SourceCode.Add(new BiggerOrEqual(line));
                    }
                    else if (new LessOrEqual().Chain.Valor == value)
                    {
                        SourceCode.Add(new LessOrEqual(line));
                    }
                    else if (new Addition().Chain.Valor == value)
                    {
                        SourceCode.Add(new Addition(line));
                    }
                    else if (new Subtraction().Chain.Valor == value)
                    {
                        SourceCode.Add(new Subtraction(line));
                    }
                    else if (new Multiplication().Chain.Valor == value)
                    {
                        SourceCode.Add(new Multiplication(line));
                    }
                    else if (new Division().Chain.Valor == value)
                    {
                        SourceCode.Add(new Division(line));
                    }
                    else if (new Or().Chain.Valor == value)
                    {
                        SourceCode.Add(new Or(line));
                    }
                    else if (new And().Chain.Valor == value)
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