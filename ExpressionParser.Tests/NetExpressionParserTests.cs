using ExpressionParser.Core.Parser;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionParser.Tests
{
    [TestFixture]
    public class NetExpressionParserTests
    {
        private IExpressionParser _parser;
        private string _strExp;

        [SetUp]
        public void Setup()
        {

            List<IOperator> operators = new List<IOperator>();
            operators.Add(new OperatorImpl("+", 0, (left, right) => Expression.Add(left, right)));
            operators.Add(new OperatorImpl("-", 0, (left, right) => Expression.Subtract(left, right)));
            operators.Add(new OperatorImpl("*", 1, (left, right) => Expression.Multiply(left, right)));
            operators.Add(new OperatorImpl("/", 2, (left, right) => Expression.Divide(left, right)));
            _parser = new NetExpressionParser(operators);
            _strExp = "";
        }

        [Test]
        public void TestConstParse()
        {
            // int
            _strExp = "54";
            var resultExp = _parser.Parse(_strExp);

            Assert.AreEqual(54, resultExp.Evaluate());

            // double
            _strExp = "54.99";
            resultExp = _parser.Parse(_strExp);

            Assert.AreEqual(54.99, resultExp.Evaluate());
        }

        [Test]
        public void TestSimpleAddition()
        {
            _strExp = "2+2";
            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(4, res);
        }

        [Test]
        public void TestSimpleSubsctruction()
        {
            _strExp = "5-2-1";
            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(2, res);
        }

        [Test]
        public void TestSubsctructionFoollowedByAddition()
        {
            _strExp = "7-3+2";
            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(6, res);
        }

        [Test]
        public void TestAdditionFollowedBySubstruction()
        {
            _strExp = "7+3-2";
            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(8, res);
        }

        [Test]
        public void TestMultiply()
        {
            _strExp = "9-2*3";
            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(3, res);
        }

        [Test]
        public void TestDivide()
        {
            _strExp = "9-6/3";
            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(7, res);
        }

        [Test]
        public void TestSimpleBrackets()
        {
            _strExp = "(3+2)";

            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(5, res);
        }

        [Test]
        public void TestBrackets()
        {
            _strExp = "7-(3+2)";

            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(2, res);
        }

        [Test]
        public void TestDoubleBrackets()
        {
            _strExp = "((3+2))";

            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(5, res);

            _strExp = "7-((3+2))";
            resultExp = _parser.Parse(_strExp);

            res = resultExp.Evaluate();

            Assert.AreEqual(2, res);
        }

        [Test]
        public void TestSpaces()
        {
            _strExp = "2 + 2";
            var resultExp = _parser.Parse(_strExp);

            var res = resultExp.Evaluate();

            Assert.AreEqual(4, res);
        }
    }
}
