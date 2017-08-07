using System;
using System.Linq.Expressions;

namespace ExpressionParser.Core
{
    class NetExpressionNodeAdapter : IExpressionNode
    {
        private Expression _netExp;

        public NetExpressionNodeAdapter(Expression exp)
        {
            _netExp = exp;
        }

        public double Evaluate()
        {
            var res = Expression.Lambda<Func<double>>(_netExp).Compile().Invoke();

            return res;
        }
    }
}
