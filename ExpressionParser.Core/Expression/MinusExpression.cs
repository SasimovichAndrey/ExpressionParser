using System;

namespace ExpressionParser.Core
{
    public class MinusExpression : IExpressionNode
    {
        private IExpressionNode _leftExpression;
        private IExpressionNode _rightExpression;

        public MinusExpression(IExpressionNode leftExpression, IExpressionNode rightExpression)
        {
            _leftExpression = leftExpression;
            _rightExpression = rightExpression;
        }

        public double Evaluate()
        {
            return _leftExpression.Evaluate() - _rightExpression.Evaluate();
        }
    }
}
