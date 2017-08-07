using System;

namespace ExpressionParser.Core
{
    public class ConstantExpression : IExpressionNode
    {
        private double _value;

        public ConstantExpression(double value)
        {
            _value = value;
        }

        public double Evaluate()
        {
            return _value;
        }
    }
}
