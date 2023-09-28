using System.Text.RegularExpressions;

namespace SpecflowCandy.ValueRetrievers;

public static class DateComputation
{
    private static readonly Regex _datePaternRegularExpression =
        new(@"^(Now|now){1}(\((mst|est|pst|cst|phx)\))?((\s)*((\+|\-){1}(\s)*([0-9]+)([s]|[m]|[d]|[M]|[y]){1})?)?");

    private static readonly Dictionary<string, string> timezones = new Dictionary<string, string>
    {
        { "mst", "America/Denver" },
        { "cst", "America/Chicago" },
        { "pst", "America/Los_Angeles" },
        { "est", "America/New_York" },
        { "phx", "America/Phoenix" },
        { "utc", UTCIdentifier }
    };

    private static readonly Dictionary<string, Func<DateTime, int, DateTime>> dateFunctions =
        new()
        {
            { "s", (baseDate, operand) => baseDate.AddSeconds(operand) },
            { "m", (baseDate, operand) => baseDate.AddMinutes(operand) },
            { "d", (baseDate, operand) => baseDate.AddDays(operand) },
            { "M", (baseDate, operand) => baseDate.AddMonths(operand) },
            { "y", (baseDate, operand) => baseDate.AddYears(operand) }
        };

    private static readonly string UTCIdentifier = "UTC";

    public static DateTime GetDateTimeValueFromToken(string value)
    {
        Match patternMatch = _datePaternRegularExpression.Match(value);
        if (!patternMatch.Success)
        {
            return default;
        }

        GroupCollection matchGroups = patternMatch.Groups;
        string timezoneAbbreviation = matchGroups[3].Value;
        string mathOperator = matchGroups[7].Value;
        string operandString = matchGroups[9].Value;
        string datePartIndicator = matchGroups[10].Value;

        DateTime dateValue = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(timezoneAbbreviation) && timezones.ContainsKey(timezoneAbbreviation))
        {
            string timezoneIdentifier = timezones[timezoneAbbreviation];
            var timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezoneIdentifier);

            Console.WriteLine($"Supplied Timezone: {timezoneAbbreviation}. Resolved Timezone: {timezoneInfo.StandardName}. Original Date: {dateValue}");

            DateTime adjustedDate = TimeZoneInfo.ConvertTimeFromUtc(dateValue, timezoneInfo);
            Console.WriteLine($"Before: {dateValue}. After {timezoneIdentifier} Timezone Conversion: {adjustedDate}");

            dateValue = DateTime.SpecifyKind(adjustedDate, timezoneIdentifier == UTCIdentifier ? DateTimeKind.Utc : DateTimeKind.Unspecified);
        }

        if (string.IsNullOrWhiteSpace(operandString))
        {
            return dateValue;
        }

        int operand = int.Parse($"{mathOperator}{operandString}");

        if (dateFunctions.ContainsKey(datePartIndicator))
        {
            return dateFunctions[datePartIndicator](dateValue, operand);
        }

        return default;
    }
}
