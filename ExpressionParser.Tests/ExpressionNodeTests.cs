using ExpressionParser.Core;
using NUnit.Framework;

namespace ExpressionParser.Tests
{
    [TestFixture]
    public class ExpressionNodeTests
    {
        [Test]
        public void TestConstantExpression()
        {
            IExpressionNode constExpression = new ConstantExpression(5);

            var res = constExpression.Evaluate();

            Assert.AreEqual(res, 5);
        }

        [Test]
        public void TestPlusExpression()
        {
            IExpressionNode const1 = new ConstantExpression(5);
            IExpressionNode const2 = new ConstantExpression(15);
            IExpressionNode plusExpr = new AddExpression(const1, const2);

            var result = plusExpr.Evaluate();
            Assert.AreEqual(20, result);
        }

        [Test]
        public void TestMinusExpression()
        {
            IExpressionNode const1 = new ConstantExpression(5);
            IExpressionNode const2 = new ConstantExpression(15);
            IExpressionNode plusExpr = new AddExpression(const1, const2);

            IExpressionNode const3 = new ConstantExpression(3);
            IExpressionNode const4 = new ConstantExpression(14);
            IExpressionNode plusExpr2 = new AddExpression(const3, const4);

            IExpressionNode minusExpr = new MinusExpression(plusExpr, plusExpr2);

            double res = minusExpr.Evaluate();
            Assert.AreEqual(res, 3);
        }
    }
}
