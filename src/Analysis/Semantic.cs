using System.Collections.Generic;
using System.Data;
using System.Text;
using Sesamo.Intermediaries;
using Sesamo.Operators.Comparisons;
using Sesamo.Operators.Conditional;
using Sesamo.Operators.Mathematical;
using Sesamo.Tokens;
using Sesamo.Variables;

namespace Sesamo.Analysis
{
    public class Semantic
    {
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => _errorMessage = value;
        }

        private Parser _semanticAnalysis;
        public Parser SemanticAnalysis => _semanticAnalysis;

        private readonly Intermediate _intermediate = new Intermediate();
        public Intermediate Intermediate => _intermediate;

        public DataTable getCodigoIntermediario()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Codicao");
            dataTable.Columns.Add("Expressao");
            dataTable.Columns.Add("ExpCondicaoNaoAtendida");

            foreach (IntermediateExpression code in _intermediate.Codigo)
            {
                DataRow dataRow = dataTable.NewRow();
                StringBuilder exp = new StringBuilder();

                foreach (Token token in code.Condicao)
                {
                    exp.Append(token.Texto);
                    exp.Append(" ");
                }
                dataRow["Condition"] = exp.ToString();

                exp = new StringBuilder();

                foreach (Token token in code.Expressao)
                {
                    exp.Append(token.Texto);
                    exp.Append(" ");
                }
                dataRow["Expression"] = exp.ToString();

                exp = new StringBuilder();

                foreach (Token token in code.ExpressaoCondicaoNaoAtendida)
                {
                    exp.Append(token.Texto);
                    exp.Append(" ");
                }
                dataRow["ExpressionConditionNotMet"] = exp.ToString();
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public bool Validate(Parser semanticAnalysis)
        {
            _semanticAnalysis = semanticAnalysis;
            bool validator = true;
            bool insideIf = false;
            bool insideThen = false;
            bool insideElse = false;
            IntermediateExpression expression = new IntermediateExpression();
            for (int i = 0; i < semanticAnalysis.LexicalAnalysis.SourceCode.Count; i++)
            {
                Token token = semanticAnalysis.LexicalAnalysis.SourceCode[i];
                Token previousToken = null;
                Token nextToken = null;
                int line = token.Linha;
                if (i > 0)
                {
                    previousToken = semanticAnalysis.LexicalAnalysis.SourceCode[i - 1];
                }

                if (i < semanticAnalysis.LexicalAnalysis.SourceCode.Count - 1)
                {
                    nextToken = semanticAnalysis.LexicalAnalysis.SourceCode[i + 1];
                }

                // TODO: Check this if
                // Original expression: if (token is Mathematics && token is Comparison)
                if (token is Mathematics || token is Comparison)
                {
                    if (token is Bigger || token is Less || token is BiggerOrEqual || token is LessOrEqual)
                    {
                        if ((previousToken as Value).Tipo != Types.Dec && (previousToken as Value).Tipo != Types.Hex && (previousToken as Value).Tipo != Types.Bin)
                        {
                            string valueName = (previousToken as Value).NomeVariavel == "" ? (previousToken as Value).ValorVariavel : (previousToken as Value).NomeVariavel;
                            _errorMessage = $"The {valueName} value types cannot be compared on the line: {line}.";
                            validator = false;
                            break;
                        }
                    }
                    else
                    {
                        _errorMessage = $"It's not possible to perform arithmetic operation with values of type {(token as Value).Tipo}. Line error: {line}.";
                        validator = false;
                        break;
                    }
                }

                if (token is Comparison)
                {
                    if ((previousToken as Value).Tipo != Types.Dec || (previousToken as Value).Tipo != Types.Hex || (previousToken as Value).Tipo != Types.Bin)
                    {
                        if ((previousToken as Value).Tipo != (nextToken as Value).Tipo)
                        {
                            _errorMessage = $"The value types must be the same when comparing on the line: {line}.";
                            validator = false;
                            break;
                        }
                        else
                        {
                            if (!(token is Equal))
                            {
                                _errorMessage = $"It is not possible to perform a numerical comparison with values of type {(previousToken as Value).Tipo} and {((Value) nextToken).Tipo}. Line error: {line}.";
                                validator = false;
                                break;
                            }
                        }
                    }
                }

                if (previousToken != null)
                {
                    if (previousToken.Linha != token.Linha)
                    {
                        if (expression.Expressao.Count > 0 || expression.ExpressaoCondicaoNaoAtendida.Count > 0)
                        {
                            _intermediate.AdicionarExpressao(expression);

                            IntermediateExpression expressionTemp = new IntermediateExpression();
                            if ((insideThen || insideElse) && !(token is EndIf))
                            {
                                expressionTemp.Condicao = expression.getCopiaCondicao();
                            }

                            expression = expressionTemp;
                        }

                        if (token is EndIf && expression.Condicao.Count > 0)
                        {
                            expression.Condicao = new List<Token>();
                        }
                    }
                }

                switch (token)
                {
                    case If _:
                        insideIf = true;
                        insideThen = false;
                        insideElse = false;
                        continue;
                    case Then _:
                        insideIf = false;
                        insideThen = true;
                        insideElse = false;
                        continue;
                    case Else _:
                        insideIf = false;
                        insideThen = false;
                        insideElse = true;
                        continue;
                    case EndIf _:
                        insideIf = false;
                        insideThen = false;
                        insideElse = false;
                        continue;
                }

                if (insideIf)
                {
                    expression.AdicionarTokenEmCondicao(token);
                }
                else if (insideThen)
                {
                    expression.AdicionarTokenEmExpressao(token);
                }
                else if (insideElse)
                {
                    expression.AdicionarTokenEmExpressaoCondicaoNaoAtendida(token);
                }
                else
                {
                    expression.AdicionarTokenEmExpressao(token);
                }
            }

            if (expression.Expressao.Count > 0 || expression.ExpressaoCondicaoNaoAtendida.Count > 0)
            {
                _intermediate.AdicionarExpressao(expression);
            }

            return validator;
        }
    }
}