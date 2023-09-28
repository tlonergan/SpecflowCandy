using TechTalk.SpecFlow;

namespace SpecflowCandy;

public static class ScenarioContextExtentions
{
    private static readonly string scenarioContextCleanUpKey = "scenarioCleanUpKey";

    public static void AddCleanupStep(this ScenarioContext scenarioContext, Action cleanupAction)
    {
        if (!scenarioContext.TryGetValue(scenarioContextCleanUpKey, out List<Action> cleanUpActions))
        {
            cleanUpActions = new List<Action>();
            scenarioContext.Add(scenarioContextCleanUpKey, cleanUpActions);
        }

        cleanUpActions.Add(cleanupAction);
    }

    public static void CleanUp(this ScenarioContext scenarioContext)
    {
        if (!scenarioContext.TryGetValue(scenarioContextCleanUpKey, out List<Action> cleanUpActions))
        {
            return;
        }

        for (int i = cleanUpActions.Count - 1; i >= 0; i--)
        {
            Action cleanUpAction = cleanUpActions[i];
            cleanUpAction.Invoke();
        }
    }
}
