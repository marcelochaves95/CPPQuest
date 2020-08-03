using System.Collections.Generic;
using System.Data;
using System.Text;
using Sesamo.Intermediaries;
using Sesamo.Operators.Comparisons;
using Sesamo.Operators.Logical;
using Sesamo.Operators.Mathematical;
using Sesamo.Tokens;
using Sesamo.Variables;

namespace Sesamo.Compilations
{
    public class Compiler
    {
        private List<string> _errorMessages = new List<string>();
        public List<string> ErrorMessages
        {
            get => _errorMessages;
            set => _errorMessages = value;
        }

        public void Execute(Intermediate code)
        {
            foreach (IntermediateExpression expression in code.Code)
            {
                ExecuteExpression(expression);
            }
        }

        private void ExecuteExpression(IntermediateExpression expression)
        {
            if (expression.Condition.Count > 0)
            {
                if (IsConditionExpressionValid(expression.Condition))
                {
                    if (expression.Expression.Count > 0)
                    {
                        ExecuteInstruction(expression.Expression);
                    }
                }
                else
                {
                    if (expression.UnmetConditionExpression.Count > 0)
                    {
                        ExecuteInstruction(expression.UnmetConditionExpression);
                    }
                }
            }
            else
            {
                ExecuteInstruction(expression.Expression);
            }
        }

        private bool IsConditionExpressionValid(List<Token> condition)
        {
            bool validator = true;
            StringBuilder builder = new StringBuilder();
            foreach (Token token in condition)
            {
                switch (token)
                {
                    case Value value:
                        builder.Append(value.ValorVariavel);
                        break;
                    case Mathematics _:
                    case Logic _:
                    case Comparison _:
                        builder.Append(token.Text);
                        break;
                }

                builder.Append(" ");

                validator = ValidateBoolean(builder.ToString());
            }
            
            return validator;
        }

        private void ExecuteInstruction(List<Token> instruction)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builderText = new StringBuilder();
            foreach (Token token in instruction)
            {
                switch (token)
                {
                    case Value value:
                        builder.Append(value.ValorVariavel);
                        builderText.Append(value.Text);
                        break;
                    case Mathematics _:
                        builder.Append(token.Text);
                        builderText.Append(token.Text);
                        break;
                    case Comparison _:
                        builder.Append(token.Text);
                        builderText.Append(token.Text);
                        break;
                }

                builder.Append(" ");
                builderText.Append(" ");
            }

            bool validator = ValidateBoolean(builder.ToString());

            if (!validator)
            {
                _errorMessages.Add($"Rule violated: {builderText} ({builder})");
            }
        }

        private bool ValidateBoolean(string instruction)
        {
            instruction = instruction.Replace('"'.ToString(), "'");
            DataTable table = new DataTable();
            table.Columns.Add("expression", string.Empty.GetType(), instruction);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return bool.Parse((string) row["expression"]);
        }
    }
}