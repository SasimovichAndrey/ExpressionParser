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
            str = _RemoveSpaces(str);
            var exprNode = _Parse(str);
            return new NetExpressionNodeAdapter(exprNode);
        }

        private string _RemoveSpaces(string str)
        {
            str = str.Replace(" ", "");

            return str;
        }

        public Expression _Parse(string str)
        {
            str = _RemoveUselessBrackets(str);

            var op = "";
            int opIndex= -1;

            for (int priority = 2; priority >= 0; priority--)
            {
                var ops = _operaorPriorities.Where(kv => kv.Value == priority).Select(kv => kv.Key);
                for (var index = str.Length - 1; index >= 0; index--)
                {
                    if(str[index] == ')')
                    {
                        index--;
                        var bracketsCount = 1;
                        while(bracketsCount != 0)
                        {
                            if(str[index] == '(')
                            {
                                bracketsCount--;
                            }
                            if(str[index] == ')')
                            {
                                bracketsCount++;
                            }

                            index--;
                        }
                    }

                    if (ops.Any(op1 => op1 == str[index].ToString()))
                    {
                        op = str[index].ToString();
                        opIndex = index;
                        break;
                    }
                }

                if (op != "")
                {
                    break;
                }
            }

            if (op != "")
            {
                var leftStr = str.Substring(0, opIndex);
                var rightStr = str.Substring(opIndex + 1);

                var leftExpr = _Parse(leftStr);
                var rightExpr = _Parse(rightStr);

                return _GetNonConstantExpression(op, leftExpr, rightExpr);
            }
            else
            {
                return _ParseConstant(str);
            }
        }

        private string _RemoveUselessBrackets(string str)
        {
            while(str.First()=='(' && str.Last() == ')')
            {
                str = str.Substring(1, str.Length - 2);
            }

            return str;
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
