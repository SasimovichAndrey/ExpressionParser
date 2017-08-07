using System;

namespace ExpressionParser.Core
{
    public class AddExpression : IExpressionNode
    {
        private IExpressionNode _leftExpression;
        private IExpressionNode _rightExpression;

        public AddExpression(IExpressionNode leftExpression, IExpressionNode rightExpression)
        {
            _leftExpression = leftExpression;
            _rightExpression = rightExpression;
        }

        public double Evaluate()
        {
            return _leftExpression.Evaluate() + _rightExpression.Evaluate();
        }
    }
}
