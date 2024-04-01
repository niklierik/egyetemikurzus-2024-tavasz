using System.Reflection;
using Calculator.State;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Tokens;
using Calculator.Utils;
using Moq;

namespace CalculatorTests.Syntax.Lexing;

public class LexerTest
{
    private Lexer _lexer;

    [SetUp]
    public void SetUp()
    {
        var typeCollectorMock = new Mock<ITypeCollector>();
        typeCollectorMock
            .Setup(typeCollector => typeCollector.GetConstantStringTokens(It.IsAny<Assembly>()))
            .Returns(
                [
                    typeof(AddToken),
                    typeof(SubtractToken),
                    typeof(MultiplyToken),
                    typeof(DivideToken),
                    typeof(OpenBracketToken),
                    typeof(CloseBracketToken)
                ]
            );

        var jsonService = new Mock<IJsonService>();
        _lexer = new Lexer(typeCollectorMock.Object, jsonService.Object);
    }

    [Category("LexString()")]
    [Test(Description = "Returns a single EOF token")]
    public void LexString_EmptyString()
    {
        string input = "";

        var result = _lexer.LexString(input);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.TypeOf(typeof(EndToken)));
    }

    [Category("LexString()")]
    [Test(Description = "Returns token expressing a math expression with whitespaces")]
    public void LexString_BasicMathExpression()
    {
        string input = "26 + 3.2 * (-4 - 585.23) / 0.123";

        var result = _lexer.LexString(input);

        Assert.That(result, Has.Count.EqualTo(21));

        Assert.That(result[0], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[0]).Value, Is.EqualTo(26));

        Assert.That(result[1], Is.TypeOf(typeof(WhitespaceToken)));

        Assert.That(result[2], Is.TypeOf(typeof(AddToken)));

        Assert.That(result[3], Is.TypeOf(typeof(WhitespaceToken)));

        Assert.That(result[4], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[4]).Value, Is.EqualTo(3.2));

        Assert.That(result[5], Is.TypeOf(typeof(WhitespaceToken)));

        Assert.That(result[6], Is.TypeOf(typeof(MultiplyToken)));

        Assert.That(result[7], Is.TypeOf(typeof(WhitespaceToken)));

        Assert.That(result[8], Is.TypeOf(typeof(OpenBracketToken)));

        Assert.That(result[9], Is.TypeOf(typeof(SubtractToken)));

        Assert.That(result[10], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[10]).Value, Is.EqualTo(4));

        Assert.That(result[11], Is.TypeOf(typeof(WhitespaceToken)));

        Assert.That(result[12], Is.TypeOf(typeof(SubtractToken)));

        Assert.That(result[13], Is.TypeOf(typeof(WhitespaceToken)));

        Assert.That(result[14], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[14]).Value, Is.EqualTo(585.23));

        Assert.That(result[15], Is.TypeOf(typeof(CloseBracketToken)));

        Assert.That(result[16], Is.TypeOf(typeof(WhitespaceToken)));

        Assert.That(result[17], Is.TypeOf(typeof(DivideToken)));

        Assert.That(result[18], Is.TypeOf(typeof(WhitespaceToken)));

        Assert.That(result[19], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[19]).Value, Is.EqualTo(0.123));

        Assert.That(result[20], Is.TypeOf(typeof(EndToken)));
    }

    [Category("LexString()")]
    [Test(Description = "Returns tokens without whitespaces")]
    public void LexString_BasicMathExpressionWithoutWhitespaces()
    {
        string input = "26 + 3.2 * (-4 - 585.23) / 0.123";

        var result = _lexer.LexString(input, true);

        Assert.That(result, Has.Count.EqualTo(13));

        Assert.That(result[0], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[0]).Value, Is.EqualTo(26));

        Assert.That(result[1], Is.TypeOf(typeof(AddToken)));

        Assert.That(result[2], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[2]).Value, Is.EqualTo(3.2));

        Assert.That(result[3], Is.TypeOf(typeof(MultiplyToken)));

        Assert.That(result[4], Is.TypeOf(typeof(OpenBracketToken)));

        Assert.That(result[5], Is.TypeOf(typeof(SubtractToken)));

        Assert.That(result[6], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[6]).Value, Is.EqualTo(4));

        Assert.That(result[7], Is.TypeOf(typeof(SubtractToken)));

        Assert.That(result[8], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[8]).Value, Is.EqualTo(585.23));

        Assert.That(result[9], Is.TypeOf(typeof(CloseBracketToken)));

        Assert.That(result[10], Is.TypeOf(typeof(DivideToken)));

        Assert.That(result[11], Is.TypeOf(typeof(NumberLiteralToken)));
        Assert.That(((NumberLiteralToken)result[11]).Value, Is.EqualTo(0.123));

        Assert.That(result[12], Is.TypeOf(typeof(EndToken)));
    }
}
