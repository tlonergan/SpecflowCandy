using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecflowCandy.ValueRetrievers;

public class StringValueRetriever : IValueRetriever
{
    private readonly ScenarioContext _context;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public StringValueRetriever(ScenarioContext context, ISpecFlowOutputHelper outputHelper)
    {
        _context = context;
        _outputHelper = outputHelper;
    }

    public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        string value = keyValuePair.Value;
        bool canRetrieve = propertyType == typeof(string) && !string.IsNullOrWhiteSpace(value);

        return canRetrieve;
    }

    public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        string value = keyValuePair.Value;
        return TokenValueReplacements.GetStringValue(_context, value, _outputHelper);
    }
}

[Binding]
public class StringTransforms
{
    private readonly ScenarioContext _context;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public StringTransforms(ScenarioContext context, ISpecFlowOutputHelper outputHelper)
    {
        _context = context;
        _outputHelper = outputHelper;
    }

    [StepArgumentTransformation]
    public string StringTransformation(string value)
    {
        return TokenValueReplacements.GetStringValue(_context, value, _outputHelper);
    }
}

