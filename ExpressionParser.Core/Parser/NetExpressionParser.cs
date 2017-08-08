using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionParser.Core.Parser
{
    public class NetExpressionParser : IExpressionParser
    {
        private IEnumerable<IOperator> _operators;

        public NetExpressionParser(IEnumerable<IOperator> operators)
        {
            _operators = operators;
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

            var maxPriority = _operators.Select(o => o.Priority).Max();
            for (int priority = 0; priority <= maxPriority; priority++)
            {
                var ops = _operators.Where(opr => opr.Priority == priority);
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

                    if (ops.Any(op1 => op1.Operator == str[index].ToString()))
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
            var oprtr = _operators.SingleOrDefault(o => o.Operator == op);

            if (oprtr != null)
            {
                var node = oprtr.GetExpression(left, right);
                return node;
            }
            else
            {
                throw new ApplicationException("Unknown operator");
            }
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
