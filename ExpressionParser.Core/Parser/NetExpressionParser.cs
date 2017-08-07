using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionParser.Core.Parser
{
    public class NetExpressionParser : IExpressionParser
    {
        private IDictionary<string, int> _operaorPriorities = new Dictionary<string, int>();

        public NetExpressionParser()
        {
            _operaorPriorities.Add("+", 2);
            _operaorPriorities.Add("-", 2);
            _operaorPriorities.Add("*", 1);
            _operaorPriorities.Add("/", 1);
        }

        public IExpressionNode Parse(string str)
        {
            var exprNode = _Parse(str);
            return new NetExpressionNodeAdapter(exprNode);
        }

        public Expression _Parse(string str)
        {
            var op = "";
            int leftEndIndex = -1;
            int rightStartIndex = - 1;
            int endIndex = str.Length - 1;

            if (str.Last() == ')' && str.IndexOf('(') != 0)
            {
                op = str[str.IndexOf('(') - 1].ToString();
                leftEndIndex = str.IndexOf('(') - 2;
                rightStartIndex = str.IndexOf('(') + 1;
                endIndex = str.Length - 2;
            }
            else
            {
                if (str.IndexOf('(') == 0 && str.Last() == ')')
                {
                    str = str.Substring(1, str.Length - 2);
                    endIndex -= 2;
                }

                for (int priority = 2; priority >= 0; priority--)
                {
                    var ops = _operaorPriorities.Where(kv => kv.Value == priority).Select(kv => kv.Key);
                    for (var index = str.Length - 1; index >= 0; index--)
                    {
                        if (ops.Any(op1 => op1 == str[index].ToString()))
                        {
                            op = str[index].ToString();
                            leftEndIndex = index - 1;
                            rightStartIndex = index + 1;
                            break;
                        }
                    }

                    if (op != "")
                    {
                        break;
                    }
                }
            }

            if (op != "")
            {
                var leftStr = str.Substring(0, leftEndIndex + 1);
                var rightStr = str.Substring(rightStartIndex, endIndex - rightStartIndex + 1);

                var leftExpr = _Parse(leftStr);
                var rightExpr = _Parse(rightStr);

                return _GetNonConstantExpression(op, leftExpr, rightExpr);
            }
            else
            {
                return _ParseConstant(str);
            }
        }

        private Expression _GetNonConstantExpression(string op, Expression left, Expression right)
        {
            Expression node = null;

            switch (op)
            {
                case "+":
                    node = Expression.Add(left, right);
                    break;
                case "-":
                    node = Expression.Subtract(left, right);
                    break;
                case "*":
                    node = Expression.Multiply(left, right);
                    break;
                case "/":
                    node = Expression.Divide(left, right);
                    break;
                default:
                    throw new ApplicationException("Unknown operator");
            }

            return node;
        }

        private System.Linq.Expressions.ConstantExpression _ParseConstant(string str)
        {
            var cStr = "";
            var strEnumerator = str.GetEnumerator();
            while (strEnumerator.MoveNext() && (char.IsDigit(strEnumerator.Current) || strEnumerator.Current == '.'))
            {
                cStr += strEnumerator.Current;
            }

            var value = double.Parse(cStr);

            return Expression.Constant(value);
        }
    }
}
