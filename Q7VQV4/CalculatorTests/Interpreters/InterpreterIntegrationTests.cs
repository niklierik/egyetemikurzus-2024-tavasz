using System.Reflection;
using Calculator.Evaluators;
using Calculator.Interpreters;
using Calculator.IO;
using Calculator.IO.Logging;
using Calculator.State;
using Calculator.State.Methods;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Parser;
using Calculator.Utils;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CalculatorTests.Interpreters;

public class InterpreterIntegrationTestFixture
{
    public Dictionary<string, object> Variables { get; set; } = new();

    public string Expression { get; set; } = "";
    public object? Expected { get; set; }
}

public class InterpreterIntegrationTests
{
    IInterpreter _interpreter;
    InterpreterConfig _config;

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
        serviceCollection.AddSingleton<IInterpreter, Interpreter>();
        serviceCollection.AddSingleton<IJsonService, JsonService>();
        serviceCollection.AddSingleton<IConfigMethod, ConfigMethod>();
        serviceCollection.AddSingleton<IListOperatorsMethod, ListOperatorsMethod>();

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

        _config = new();
        var stateLoaderMock = new Mock<IConfigLoader<InterpreterConfig>>();
        stateLoaderMock
            .Setup(stateLoader => stateLoader.Load(It.IsAny<string>()))
            .ReturnsAsync(_config);
        stateLoaderMock.Setup(stateLoader =>
            stateLoader.Save(It.IsAny<string>(), It.IsAny<InterpreterConfig>())
        );

        serviceCollection.AddSingleton<IConfigLoader<InterpreterConfig>>(stateLoaderMock.Object);

        IServiceProvider provider = serviceCollection.BuildServiceProvider();

        _interpreter = provider.GetService<IInterpreter>()!;

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
        new() { Expression = "someVariable = 3", Expected = 3.0 },
        new() { Expression = "anotherVariable = \"asd\"", Expected = "asd" },
        new()
        {
            Variables = new() { { "anotherVariable", "asd" } },
            Expression = "anotherVariable",
            Expected = "asd"
        },
        new()
        {
            Variables = new() { { "anotherVariable", 2.0 } },
            Expression = "anotherVariable + 3",
            Expected = 5.0
        },
        new() { Expression = "missingVariable", Expected = null },
        new() { Expression = "a = 3", Expected = 3.0 },
        new() { Expression = "\"asd\"", Expected = "asd" },
    ];

    [Theory]
    public async Task Execute(InterpreterIntegrationTestFixture fixture)
    {
        await _interpreter.Init();

        if (fixture.Variables is not null)
        {
            foreach (var (key, value) in fixture.Variables)
            {
                _interpreter.State.Variables[key] = value;
            }
        }

        var actual = await _interpreter.Execute(fixture.Expression);

        Assert.That(actual, Is.EqualTo(fixture.Expected).Within(0.000001));
    }
}
