using System.Reflection;
using Calculator.Evaluators;
using Calculator.Evaluators.ExpressionEvals;
using Calculator.Interpreters;
using Calculator.IO;
using Calculator.IO.Logging;
using Calculator.Syntax.Lexing;
using Calculator.Syntax.Parser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHost = Calculator.IO.IHost;

HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder(args);
hostApplicationBuilder.Services.AddHostedService<InterpreterProgram>();

Assembly assembly = typeof(ISubEvaluator).Assembly;
IReadOnlyList<Type> subEvaluatorTypes = TypeCollector.GetSubEvaluators(assembly);
IReadOnlyList<Type> binaryOperators = TypeCollector.GetBinaryOps(assembly);
IReadOnlyList<Type> unaryOperators = TypeCollector.GetUnaryOps(assembly);

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

using var app = hostApplicationBuilder.Build();

app.Start();
