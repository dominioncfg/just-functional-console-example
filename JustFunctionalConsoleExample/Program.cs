using JustFunctional.Core;

string expression = AskAndGetInputExpression();
Dictionary<string, decimal> variablesWithValues = AskAndGetInputVariablesWithValues();


var functionFactory = FunctionFactoryBuilder.ConfigureFactory(options =>
{
    options    
        .WithEvaluationContextVariablesProvider()
        .WithCompiledEvaluator();
});

var variables = new PredefinedVariablesProvider(variablesWithValues.Keys.ToArray());
TryCreateFunctionResult createResult = functionFactory.TryCreate(expression, variables);

if (!createResult.Success)
{
    Console.WriteLine($"The expression could not be parsed because: {string.Join(", ", createResult.Errors)}");
    return;
}
var evalutaionContext = new EvaluationContext(variablesWithValues);
var result = await createResult.Function.EvaluateAsync(evalutaionContext);

Console.WriteLine(result);
Console.ReadLine();


string AskAndGetInputExpression()
{
    Console.WriteLine("Enter the expression to evaluate: (E.g. X+Y)");
    var expression = Console.ReadLine() ?? throw new Exception("You need to suply a value");
    return expression;
}

Dictionary<string, decimal> AskAndGetInputVariablesWithValues()
{
    Console.WriteLine("Enter the expression's variables: (E.g. X=3;Y=4)");
    var splittedVariables = Console.ReadLine()?
        .Split(';', StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
    var variablesWithValues = splittedVariables
        .Select(x => x.Split('=', StringSplitOptions.RemoveEmptyEntries))
        .ToDictionary(x => x.First(), v => decimal.Parse(v.Last()));
    return variablesWithValues;
}