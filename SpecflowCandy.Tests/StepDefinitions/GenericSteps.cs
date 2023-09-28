using Microsoft.Extensions.Configuration;
using SpecflowCandy.Tests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecflowCandy.Tests.StepDefinitions;

[Binding]
public class GenericSteps
{
    private readonly ScenarioContext _context;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public GenericSteps(ScenarioContext context, ISpecFlowOutputHelper outputHelper)
    {
        _context = context;
        _outputHelper = outputHelper;
    }

    [Before]
    public void Before()
    {
        //Service.Instance.ValueRetrievers.Register(new GuidValueRetriever(_context));
        //Service.Instance.ValueRetrievers.Register(new StringValueRetriever(_context, _outputHelper));
        //Service.Instance.ValueRetrievers.Register(new DateTimeValueRetriever(_context));

        new ConfigurationBuilder().AddJsonFile("specflow.json").Build();
    }

    [When(@"I store an object at key ""(.*)"", created from")]
    public void WhenIStoreAnObjectAtKeyCreatedFrom(string objectPlaceholder, Table table)
    {
        ValueEntity valueEntity = table.CreateInstance<ValueEntity>();

        _outputHelper.WriteLine($"Value Object Received: {Environment.NewLine}{valueEntity}");

        _context.Add(objectPlaceholder, valueEntity);
    }
}
