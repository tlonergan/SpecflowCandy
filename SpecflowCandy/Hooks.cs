using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Assist.ValueRetrievers;
using TechTalk.SpecFlow.Infrastructure;

using DateTimeValueRetriever = SpecflowCandy.ValueRetrievers.DateTimeValueRetriever;
using StringValueRetriever = SpecflowCandy.ValueRetrievers.StringValueRetriever;

namespace SpecflowCandy;

[Binding]
public sealed class Hooks
{
    private readonly ISpecFlowOutputHelper _outputHelper;

    public Hooks(ISpecFlowOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [BeforeScenario(Order = int.MinValue)]
    public void BeforeScenario(ScenarioContext scenarioContext, ISpecFlowOutputHelper outputHelper)
    {
        outputHelper.WriteLine("In Before");

        Service.Instance.ValueRetrievers.Register(new NullValueRetriever("{{null}}"));

        Service.Instance.ValueRetrievers.Unregister<TechTalk.SpecFlow.Assist.ValueRetrievers.StringValueRetriever>();
        Service.Instance.ValueRetrievers.Unregister<StringValueRetriever>();
        var stringValueRetriever = new StringValueRetriever(scenarioContext, outputHelper);
        Service.Instance.ValueRetrievers.Register(stringValueRetriever);


        Service.Instance.ValueRetrievers.Unregister<TechTalk.SpecFlow.Assist.ValueRetrievers.DateTimeValueRetriever>();
        Service.Instance.ValueRetrievers.Unregister<DateTimeValueRetriever>();
        var dateValueRetriever = new DateTimeValueRetriever(scenarioContext, outputHelper);
        Service.Instance.ValueRetrievers.Register(dateValueRetriever);
    }

    [After(Order = int.MaxValue)]
    public void ScenarioCleanUp(ScenarioContext scenarioContext)
    {
        scenarioContext.CleanUp();
    }

    [StepArgumentTransformation]
    public List<string> ListStringTransformation(Table value)
    {
        _outputHelper.WriteLine("In List<string> Transform");
        return value.CreateSet<string>().ToList();
    }
}
