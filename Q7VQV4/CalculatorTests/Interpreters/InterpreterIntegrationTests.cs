using System.Reflection;
using Calculator;
using Calculator.Evaluators;
using Calculator.Interpreters;
using Calculator.IO;
using Calculator.IO.Logging;
using Calculator.State;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Parser;
using Calculator.Utils;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CalculatorTests.Interpreters;

public class InterpreterIntegrationTestFixture
{
    public string Expression { get; set; } = "";
    public object? Expected { get; set; }
}

public class InterpreterIntegrationTests
{
    IInterpreter<InterpreterState> _interpreter;
    InterpreterState _state;

    [SetUp]
    public void SetUp()
    {
        var serviceCollection = new ServiceCollection();
        Assembly assembly = typeof(InterpreterProgram).Assembly;

        var typeCollector = new TypeCollector();

        IReadOnlyList<Type> subEvaluatorTypes = typeCollector.GetSubEvaluators(assembly);
        IReadOnlyList<Type> binaryOperators = typeCollector.GetBinaryOps(assembly);
        IReadOnlyList<Type> unaryOperators = typeCollector.GetUnaryOps(assembly);

        foreach (Type type in subEvaluatorTypes)
        {
            serviceCollection.AddSingleton(type);
        }

        foreach (Type type in binaryOperators)
        {
            serviceCollection.AddSingleton(type);
        }

        foreach (Type type in unaryOperators)
        {
            serviceCollection.AddSingleton(type);
        }

        serviceCollection.AddSingleton<ILexer, Lexer>();
        serviceCollection.AddSingleton<IParser, Parser>();
        serviceCollection.AddSingleton<IEvaluator, Evaluator>();
        serviceCollection.AddSingleton<IInterpreter<InterpreterState>, Interpreter>();

        serviceCollection.AddSingleton<ITypeCollector, TypeCollector>();

        var nodePrettyPrinterMock = new Mock<INodePrettyPrinter>();
        serviceCollection.AddSingleton<INodePrettyPrinter>(nodePrettyPrinterMock.Object);

        var hostMock = new Mock<IHost>();
        serviceCollection.AddSingleton<IHost>(hostMock.Object);

        var logManagerMock = new Mock<ILogManager>();
        serviceCollection.AddSingleton<ILogManager>(logManagerMock.Object);

        var logTargetProviderMock = new Mock<ILogTargetProvider>();
        logTargetProviderMock
            .Setup(logTargetProvider => logTargetProvider.GetLogTargets())
            .Returns([]);
        serviceCollection.AddSingleton<ILogTargetProvider>(logTargetProviderMock.Object);

        _state = new();
        _state.Variables.Add("someVariable", 3.0);
        _state.Variables.Add("anotherVariable", "asd");
        var stateLoaderMock = new Mock<IStateLoader<InterpreterState>>();
        stateLoaderMock
            .Setup(stateLoader => stateLoader.LoadState(It.IsAny<string>()))
            .ReturnsAsync(_state);
        stateLoaderMock.Setup(stateLoader =>
            stateLoader.SaveState(It.IsAny<string>(), It.IsAny<InterpreterState>())
        );

        serviceCollection.AddSingleton<IStateLoader<InterpreterState>>(stateLoaderMock.Object);

        IServiceProvider provider = serviceCollection.BuildServiceProvider();

        _interpreter = provider.GetService<IInterpreter<InterpreterState>>()!;

        Assert.That(_interpreter, Is.Not.Null);
    }

    [DatapointSource]
    internal InterpreterIntegrationTestFixture[] fixtures =
    [
        new() { Expression = "2 + 3", Expected = 5.0 },
        new() { Expression = "4 - 6", Expected = -2.0 },
        new() { Expression = "10 * 3", Expected = 30.0 },
        new() { Expression = "3 / 2", Expected = 1.5 },
        new() { Expression = "4 ^ 2", Expected = 16.0 },
        new() { Expression = "+4", Expected = 4.0 },
        new() { Expression = "-6", Expected = -6.0 },
        new() { Expression = "2 + 4 * -5 / 2 - 4", Expected = -12.0 },
        new() { Expression = "2 + 4 * -5 / 2 ^ 3 - 4", Expected = -4.5 },
        new() { Expression = "(2 + 4) * (-5 / 2 ^ (3 - 4))", Expected = -60.0 },
        new() { Expression = "true", Expected = true },
        new() { Expression = "false", Expected = false },
        new() { Expression = "null", Expected = null },
        new() { Expression = "someVariable", Expected = 3.0 },
        new() { Expression = "someVariable + 3", Expected = 6.0 },
        new() { Expression = "anotherVariable", Expected = "asd" },
        new() { Expression = "missingVariable", Expected = null },
        new() { Expression = "a = 3", Expected = 3.0 },
    ];

    [Theory]
    public async Task Execute(InterpreterIntegrationTestFixture fixture)
    {
        await _interpreter.Init();
        var actual = await _interpreter.Execute(fixture.Expression);

        Assert.That(actual, Is.EqualTo(fixture.Expected).Within(0.000001));
    }
}
