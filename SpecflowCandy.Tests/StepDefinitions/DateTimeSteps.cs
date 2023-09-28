using SpecflowCandy.Tests.Models;
using TechTalk.SpecFlow;

namespace SpecflowCandy.Tests.StepDefinitions;

[Binding]
public class DateTimeSteps
{
    private readonly ScenarioContext _context;

    public DateTimeSteps(ScenarioContext context)
    {
        _context = context;
    }

    [When(@"I use a datetime parameter ""([^""]*)"" stored at context key ""([^""]*)""")]
    public void WhenIUseADatetimeParameterStoredAtContextKey(DateTime dateValue, string validationValueContextKey)
    {
        _context.Add(validationValueContextKey, dateValue);
    }

    [Then(@"the date field, at key ""([^""]*)"", is populated by a non-default date time")]
    public void ThenTheDateFieldAtKeyIsPopulatedByANon_DefaultDateTime(string contextKey)
    {
        ValueEntity? valueEntity = _context.Get<ValueEntity>(contextKey);
        Assert.AreNotEqual(default, valueEntity.DateTimeValue);
    }

    [Then(@"the context key ""([^""]*)"" is a non-default datetime")]
    public void ThenTheContextKeyIsANon_DefaultDatetime(string contextKey)
    {
        DateTime dateValue = _context.Get<DateTime>(contextKey);
        Assert.AreNotEqual(default, dateValue);
    }

    [Then(@"the date value ""([^""]*)"" equals the value in context at key ""([^""]*)""")]
    public void ThenTheDateValueEqualsTheValueInContextAtKey(DateTime dateValue, string validationContextKey)
    {
        DateTime dateValueFromContext = _context.Get<DateTime>(validationContextKey);
        Assert.AreEqual(dateValue, dateValueFromContext);
    }

    [Then(@"these dates are not equal ""([^""]*)"" and ""([^""]*)""")]
    public void ThenTheseDatesAreNotEqualAnd(string firstDate, string secondDate)
    {
        Assert.AreNotEqual(firstDate, secondDate);
    }
}
