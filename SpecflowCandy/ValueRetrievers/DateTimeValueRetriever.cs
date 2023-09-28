using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecflowCandy.ValueRetrievers;

public class DateTimeValueRetriever : IValueRetriever
{
    private readonly ScenarioContext _context;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public DateTimeValueRetriever(ScenarioContext context, ISpecFlowOutputHelper outputHelper)
    {
        _context = context;
        _outputHelper = outputHelper;
    }

    public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        bool canRetrieve = propertyType == typeof(DateTime);
        return canRetrieve;
    }

    public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
    {
        return TokenValueReplacements.GetDateTimeValue(_context, keyValuePair.Value);
    }
}

[Binding]
public class DateTimeTransforms
{
    private readonly ScenarioContext _context;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public DateTimeTransforms(ScenarioContext context, ISpecFlowOutputHelper outputHelper)
    {
        _context = context;
        _outputHelper = outputHelper;
    }

    [StepArgumentTransformation]
    public DateTime DateTimeTransformation(string value)
    {
        return TokenValueReplacements.GetDateTimeValue(_context, value);
    }
}

