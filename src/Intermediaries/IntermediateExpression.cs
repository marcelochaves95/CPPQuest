using System.Collections.Generic;
using Sesamo.Tokens;

namespace Sesamo.Intermediaries
{
    public class IntermediateExpression
    {
        private readonly List<Token> _expression = new List<Token>();
        public List<Token> Expression => _expression;

        private List<Token> _condition = new List<Token>();
        public List<Token> Condition
        {
            get => _condition;
            set => _condition = value;
        }

        private readonly List<Token> _unmetConditionExpression = new List<Token>();
        public List<Token> UnmetConditionExpression => _unmetConditionExpression;

        public bool ExpressionUnderCondition => _condition.Count > 0;

        public IntermediateExpression()
        {
        }

        public IntermediateExpression(List<Token> expression, List<Token> unmetConditionExpression, List<Token>condition)
        {
            _expression = expression;
            _unmetConditionExpression = unmetConditionExpression;
            _condition = condition;
        }

        public void AddTokenInExpression(Token token)
        {
            _expression.Add(token);
        }

        public void AddTokenInCondition(Token token)
        {
            _condition.Add(token);
        }

        public void AddTokenInUnmetConditionExpression(Token token)
        {
            _unmetConditionExpression.Add(token);
        }

        public List<Token> GetConditionCopy()
        {
            List<Token> conditions = new List<Token>();
            foreach (Token token in Condition)
            {
                conditions.Add(token);
            }

            return conditions;
        }
    }
}