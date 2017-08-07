using ExpressionParser.Core;

namespace ExpressionParser.Core.Parser
{
    public interface IExpressionParser
    {
        IExpressionNode Parse(string str);
    }
}
