using SpecflowCandy.Tests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecflowCandy.Tests.StepDefinitions;

[Binding]
internal class StringSteps
{
    private readonly ScenarioContext _context;
    private readonly ISpecFlowOutputHelper _outputHelper;

    public StringSteps(ScenarioContext context, ISpecFlowOutputHelper outputHelper)
    {
        _context = context;
        _outputHelper = outputHelper;
    }

    [When(@"I use a string parameter ""(.*)"" stored at context key ""(.*)""")]
    public void WhenUseAStringParameterStoredAtContextKey(string stringValue, string validationContextKey)
    {
        _context.Add(validationContextKey, stringValue);
    }

    [Then(@"the string field, at key ""(.*)"", is populated by a non-default string")]
    public void ThenTheStringFieldIsPopulatedByANon_DefaultStringAtKey(string contextKey)
    {
        ValueEntity valueEntity = _context.Get<ValueEntity>(contextKey);
        Assert.AreNotEqual(default, valueEntity.StringValue);
    }

    [Then(@"the string field of the object stored at context key ""(.*)"" contains the string value of ""(.*)""")]
    public void ThenTheStringFieldOfTheObjectStoredAtContextKeyContainsTheStringValueOf(string contextKey, string expectedValue)
    {
        ValueEntity valueEntity = _context.Get<ValueEntity>(contextKey);

        _outputHelper.WriteLine($"Value Entity in Context: {Environment.NewLine}{valueEntity}. {Environment.NewLine}Inside Token {_context.Get<string>("insideToken")}. {Environment.NewLine}Expected Value: {expectedValue}");

        Assert.True(valueEntity.StringValue.Contains(expectedValue));
    }

    [Then(@"the context key ""(.*)"" is a non-default string")]
    public void ThenTheContextKeyIsANon_DefaultString(string contextKey)
    {
        string stringValue = _context.Get<string>(contextKey);
        Assert.AreNotEqual(default, stringValue);
    }

    [Then(@"the string value ""(.*)"" equals the value in context at key ""(.*)""")]
    public void ThenTheStringValueEqualsTheValueInContextAtKey(string stringValue, string contextKey)
    {
        string expectedValue = _context.Get<string>(contextKey);
        Assert.AreNotEqual(expectedValue, stringValue);
    }

    [Then(@"the string value, at key ""(.*)"", and the string value of the object at key ""(.*)"" are equal")]
    public void ThenTheStringValueAtTheContextKeyAndTheStringValueOfTheObjectStoredAtContextKeyAreEqual(string valueContextKey, string objectContextKey)
    {
        string value = _context.Get<string>(valueContextKey);
        ValueEntity objectValue = _context.Get<ValueEntity>(objectContextKey);

        Assert.AreNotEqual(value, objectValue.StringValue);
    }
}
