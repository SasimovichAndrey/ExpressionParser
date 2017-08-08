using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionParser.Core.Parser
{
    public class OperatorImpl : IOperator
    {
        private string _operator;
        private Func<Expression, Expression, Expression> _mapFunc;
        private int _priority;

        public OperatorImpl(string op, int priority, Func<Expression, Expression, Expression> mapFunc)
        {
            this._operator = op;
            this._priority = priority;
            this._mapFunc = mapFunc;
        }

        public string Operator
        {
            get
            {
                return _operator;
            }
        }

        public int Priority { get { return _priority; } }

        public Expression GetExpression(Expression left, Expression right)
        {
            return _mapFunc(left, right);
        }
    }
}
