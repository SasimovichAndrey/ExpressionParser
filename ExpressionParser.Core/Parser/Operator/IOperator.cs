using System.Linq.Expressions;

namespace ExpressionParser.Core.Parser
{
    public interface IOperator
    {
        string Operator { get; }
        int Priority { get;}
        Expression GetExpression(Expression left, Expression right);
    }
}
