using System.Text;
using System.Text.RegularExpressions;
using Sesamo.Operators;
using Sesamo.Operators.Comparisons;
using Sesamo.Operators.Conditional;
using Sesamo.Operators.Logical;
using Sesamo.Operators.Mathematical;
using Sesamo.Tokens;
using Sesamo.Variables;

namespace Sesamo.Analysis
{
    public class Parser
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => _errorMessage = value;
        }

        private Lexical _lexicalAnalysis;
        public Lexical LexicalAnalysis => _lexicalAnalysis;

        private string GetChainRegularExpression()
        {
            return @"(\" + '"'.ToString() + @"(\w|\.|\:|\,|\-|\+|\*|\/|\&|\(|\)|\%|\$|\#|\@|\!|\?|\<|\>|\;|\ )*\" + '"'.ToString() + @")|\w+";
        }

        private string GetAllowsVariablePointRegularExpression()
        {
            return @"(\.\w+)*";
        }

        private string GetOperatorsComparisonRegularExpression()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"(\");
            stringBuilder.Append(new Equal().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(@"\");
            stringBuilder.Append(new Different().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(@"\");
            stringBuilder.Append(new Bigger().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(@"\");
            stringBuilder.Append(new Less().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(@"\");
            stringBuilder.Append(new BiggerOrEqual().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(@"\");
            stringBuilder.Append(new LessOrEqual().Chain.Value);

            stringBuilder.Append(@")");

            return stringBuilder.ToString();
        }

        private string GetOperatorsLogicalRegularExpression()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(@"(");

            stringBuilder.Append(new And().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(new Or().Chain.Value);

            stringBuilder.Append(@")");

            return stringBuilder.ToString();
        }
        
        private string GetOperatorsMathematicalRegularExpression(bool withEndSpace)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (withEndSpace)
            {
                stringBuilder.Append(@"((");
            }
            else
            {
                stringBuilder.Append(@"(\s(");
            }

            stringBuilder.Append(@"\");
            stringBuilder.Append(new Addition().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(@"\");
            stringBuilder.Append(new Subtraction().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(@"\");
            stringBuilder.Append(new Multiplication().Chain.Value);

            stringBuilder.Append(@"|");

            stringBuilder.Append(@"\");
            stringBuilder.Append(new Less().Chain.Value);

            if (withEndSpace)
            {
                stringBuilder.Append(@")\s\w+" + GetAllowsVariablePointRegularExpression() + @"\s)*");
            }
            else
            {
                stringBuilder.Append(@")\s\w+" + GetAllowsVariablePointRegularExpression() + @"\)*");
            }

            return stringBuilder.ToString();
        }

        private string GetExpressionsRegularExpression()
        {
            StringBuilder sb = new StringBuilder();
            string mathematical = GetOperatorsMathematicalRegularExpression(true);
            string mathematicalWithEndSpace = GetOperatorsMathematicalRegularExpression(false);
            string comparison = GetOperatorsComparisonRegularExpression();

            sb.Append(@"^" + GetChainRegularExpression() + GetAllowsVariablePointRegularExpression() + @"\s");
            sb.Append(mathematical);
            sb.Append(comparison);
            sb.Append(@"+\s" + GetChainRegularExpression() + GetAllowsVariablePointRegularExpression() + @"");
            sb.Append(mathematicalWithEndSpace);
            sb.Append(@"$");

            return sb.ToString();
        }

        private string GetIfThenRegularExpression()
        {
            StringBuilder sb = new StringBuilder();
            string comparison = GetOperatorsComparisonRegularExpression();
            string logical = GetOperatorsLogicalRegularExpression();
            string mathematical = GetOperatorsMathematicalRegularExpression(true);

            sb.Append(@"if\s" + GetChainRegularExpression() + GetAllowsVariablePointRegularExpression() + @"\s");
            sb.Append(mathematical);
            sb.Append(comparison);
            sb.Append(@"+\s" + GetChainRegularExpression() + GetAllowsVariablePointRegularExpression() + @"\s(");
            sb.Append(mathematical);
            sb.Append(logical);
            sb.Append(@"+\s" + GetChainRegularExpression() + GetAllowsVariablePointRegularExpression() + @"\s");
            sb.Append(mathematical);
            sb.Append(comparison);
            sb.Append(@"+\s" + GetChainRegularExpression() + GetAllowsVariablePointRegularExpression() + @"\s");
            sb.Append(mathematical);
            sb.Append(@")*then");

            return sb.ToString();
        }

        public bool Validate(Lexical lexicalAnalysis)
        {
            _lexicalAnalysis = lexicalAnalysis;
            bool validator = true;
            bool insideIf = false;
            bool insideThen = false;
            bool insideElse = false;
            string contentIf = "";
            string contentThenPerLine = "";
            string contentElsePerLine = "";
            string read = "";
            int line = 0;
            for (int i = 0; i < lexicalAnalysis.SourceCode.Count; i++)
            {
                Token token = lexicalAnalysis.SourceCode[i];
                if (line != token.Line)
                {
                    read = "";
                    if (token is Operator && !(token is If) && !(token is Else) && !(token is EndIf))
                    {
                        _errorMessage = $"Syntax error: Incorrect use of the operator at the beginning of the expression. Line: {token.Line}.";
                        validator = false;
                        break;
                    }
                }

                line = token.Line;
                if (i < lexicalAnalysis.SourceCode.Count - 1)
                {
                    Token nextToken = lexicalAnalysis.SourceCode[i - 1];
                    if (nextToken.Line != line)
                    {
                        if (token is Comparison || token is Mathematics || token is If)
                        {
                            _errorMessage = $"Syntax error: Incorrect use of the operator at the end of the expression. Line: {token.Line}.";
                            validator = false;
                            break;
                        }
                    }
                }

                if (token is Value value)
                {
                    if (read == "" || read == "O")
                    {
                        read = "V";
                    }
                    else
                    {
                        _errorMessage = $"Syntax error: {value.NomeVariavel} symbol used incorrectly on the line: {line}.";
                        validator = false;
                        break;
                    }
                }
                else if (token is Operator)
                {
                    if (read == "" || read == "V")
                    {
                        read = "O";
                    }
                    else
                    {
                        _errorMessage = $"Syntax error: Operator used incorrectly on the line: {line}.";
                        validator = false;
                        break;
                    }
                }

                switch (token)
                {
                    case If _:
                        insideIf = true;
                        insideThen = false;
                        insideElse = false;
                        break;
                    case Then _:
                        insideIf = false;
                        insideThen = true;
                        insideElse = false;
                        break;
                    case Else _:
                        insideIf = false;
                        insideThen = false;
                        insideElse = true;
                        break;
                    case EndIf _:
                        insideIf = false;
                        insideThen = false;
                        insideElse = false;
                        break;
                }

                Token previousToken = null;
                Token nextToken1 = null;

                if (i > 0)
                {
                    previousToken = lexicalAnalysis.SourceCode[i - 1];
                }

                if (i < lexicalAnalysis.SourceCode.Count - 1)
                {
                    nextToken1 = lexicalAnalysis.SourceCode[i + 1];
                }

                if (token is Logic logic  && !insideIf)
                {
                    _errorMessage = $"Syntax error: Logical operator {logic.Chain.Value} outside conditional operator on line: {line}.";
                    validator = false;
                    break;
                }

                if (insideIf)
                {
                    if (token is If @if)
                    {
                        contentIf += @if.Chain.Value;
                    }
                    else if (token is Comparison comparison)
                    {
                        contentIf += comparison.Chain.Value;
                    }
                    else if (token is Mathematics mathematics)
                    {
                        contentIf += mathematics.Chain.Value;
                    }
                    else if (token is Logic logic1)
                    {
                        contentIf += logic1.Chain.Value;
                    }
                    else if (token is Value value1)
                    {
                        if (value1.NomeVariavel != null)
                        {
                            contentIf += value1.NomeVariavel;
                        }
                        else
                        {
                            contentIf += value1.ValorVariavel;
                        }
                    }
                    else
                    {
                        _errorMessage = $"Syntax error: Conditional Operator {new If().Chain.Value} with unrecognized symbol, identified in line: {line}.";
                        validator = false;
                        break;
                    }

                    contentIf += " ";
                }
                else if (insideThen)
                {
                    if (contentIf != "")
                    {
                        contentIf += new Else().Chain.Value;
                        string ER = GetIfThenRegularExpression();
                        Match match = Regex.Match(contentIf, ER);

                        if (match.Success)
                        {
                            contentIf = "";
                        }
                        else
                        {
                            _errorMessage = $"Syntax error: Conditional Operator {new If().Chain.Value} with unrecognized symbol, identified in line: {line}.";
                            validator = false;
                            break;
                        }
                    }
                    else
                    {
                        if (token is Comparison comparison)
                        {
                            contentThenPerLine += comparison.Chain.Value;
                        }
                        else if (token is Mathematics mathematics)
                        {
                            contentThenPerLine += mathematics.Chain.Value;
                        }
                        else if (token is Logic logic1)
                        {
                            contentThenPerLine += logic1.Chain.Value;
                        }
                        else if (token is Value value1)
                        {
                            if (value1.NomeVariavel != null)
                            {
                                contentThenPerLine += value1.NomeVariavel;
                            }
                            else
                            {
                                contentThenPerLine += value1.ValorVariavel;
                            }
                        }
                        else
                        {
                            _errorMessage = $"Syntax error: Conditional Operator {new Then().Chain.Value} with unrecognized symbol, identified in line: {line}.";
                            validator = false;
                            break;
                        }
                    }

                    if (previousToken != null)
                    {
                        if (!(token is Then) && token.Line != nextToken1.Line || nextToken1 is Else || nextToken1 is EndIf)
                        {
                            string ER = GetExpressionsRegularExpression();
                            Match match = Regex.Match(contentThenPerLine, ER);

                            if (match.Success)
                            {
                                contentThenPerLine = "";
                            }
                            else
                            {
                                _errorMessage = $"Syntax error: Conditional Operator {new Then().Chain.Value} with unrecognized symbol, identified in line: {line}.";
                                validator = false;
                                break;
                            }
                        }
                    }

                    if (contentThenPerLine != "")
                    {
                        contentThenPerLine += " ";
                    }
                }
                else if (insideElse)
                {
                    if (token is Comparison comparison)
                    {
                        contentElsePerLine += comparison.Chain.Value;
                    }
                    else if (token is Mathematics mathematics)
                    {
                        contentElsePerLine += mathematics.Chain.Value;
                    }
                    else if (token is Logic logic1)
                    {
                        contentElsePerLine += logic1.Chain.Value;
                    }
                    else if (token is Value value1)
                    {
                        if (value1.NomeVariavel != null)
                        {
                            contentElsePerLine += value1.NomeVariavel;
                        }
                        else
                        {
                            contentElsePerLine += value1.ValorVariavel;
                        }
                    }
                    else if (!(token is Else))
                    {
                        _errorMessage = $"Syntax error: Conditional Operator {new Else().Chain.Value} with unrecognized symbol, identified in line: {line}.";
                        validator = false;
                        break;
                    }
                    
                    if (previousToken != null)
                    {
                        if (!(token is Else) && token.Line != nextToken1.Line || nextToken1 is EndIf)
                        {
                            if (contentElsePerLine.Trim() != "")
                            {
                                string ER = GetExpressionsRegularExpression();
                                Match match = Regex.Match(contentElsePerLine, ER);

                                if (match.Success)
                                {
                                    contentElsePerLine = "";
                                }
                                else
                                {
                                    _errorMessage = $"Syntax error: Conditional Operator {new Else().Chain.Value} with unrecognized symbol, identified in line: {line}.";
                                    validator = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (contentElsePerLine != "")
                    {
                        contentElsePerLine += " ";
                    }
                }
                else
                {
                    if (contentIf != "")
                    {
                        _errorMessage = $"Syntax error: Conditional Operator {new If().Chain.Value} without closing the {new Then().Chain.Value} operator identified in line: {line}.";
                        validator = false;
                        break;
                    }
                }
            }

            return validator;
        }
    }
}