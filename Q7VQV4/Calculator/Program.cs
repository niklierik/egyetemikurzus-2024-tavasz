using System.Reflection;
using Calculator.Evaluators;
using Calculator.Evaluators.ExpressionEvals;
using Calculator.Interpreters;
using Calculator.IO;
using Calculator.IO.Logging;
using Calculator.State;
using Calculator.State.Methods;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Parser;
using Calculator.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHost = Calculator.IO.IHost;

HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder(args);
hostApplicationBuilder.Services.AddHostedService<InterpreterProgram>();

var typeCollector = new TypeCollector();

Assembly assembly = typeof(ISubEvaluator).Assembly;
IReadOnlyList<Type> subEvaluatorTypes = typeCollector.GetSubEvaluators(assembly);
IReadOnlyList<Type> binaryOperators = typeCollector.GetBinaryOps(assembly);
IReadOnlyList<Type> unaryOperators = typeCollector.GetUnaryOps(assembly);

foreach (Type type in subEvaluatorTypes)
{
    hostApplicationBuilder.Services.AddSingleton(type);
}

foreach (Type type in binaryOperators)
{
    hostApplicationBuilder.Services.AddSingleton(type);
}

foreach (Type type in unaryOperators)
{
    hostApplicationBuilder.Services.AddSingleton(type);
}

hostApplicationBuilder.Services.AddSingleton<ILexer, Lexer>();
hostApplicationBuilder.Services.AddSingleton<IParser, Parser>();
hostApplicationBuilder.Services.AddSingleton<IEvaluator, Evaluator>();
hostApplicationBuilder.Services.AddSingleton<IHost, ConsoleHost>();
hostApplicationBuilder.Services.AddSingleton<ILogManager, LogManager>();
hostApplicationBuilder.Services.AddSingleton<ILogTargetProvider, LogTargetProvider>();
hostApplicationBuilder.Services.AddSingleton<INodePrettyPrinter, NodePrettyPrinter>();
hostApplicationBuilder.Services.AddSingleton<IInterpreter, Interpreter>();
hostApplicationBuilder.Services.AddSingleton<IJsonService, JsonService>();
hostApplicationBuilder.Services.AddSingleton<
    IConfigLoader<InterpreterConfig>,
    InterpreterConfigLoader
>();
hostApplicationBuilder.Services.AddSingleton<ITypeCollector>(typeCollector);

hostApplicationBuilder.Services.AddSingleton<IConfigMethod, ConfigMethod>();
hostApplicationBuilder.Services.AddSingleton<IListOperatorsMethod, ListOperatorsMethod>();

using var app = hostApplicationBuilder.Build();

app.Start();
