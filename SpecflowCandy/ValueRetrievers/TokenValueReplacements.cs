using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using Bogus.DataSets;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;

namespace SpecflowCandy.ValueRetrievers;

public static class TokenValueReplacements
{
    public static readonly string Prefix = "{{";
    public static readonly string Suffix = "}}";
    public static readonly char TypeNameSeparator = ':';

    public static readonly string RandomValue = $"{Prefix}random{Suffix}";
    public static readonly string DefaultValue = $"{Prefix}default{Suffix}";
    public static readonly string NullValue = $"{Prefix}null{Suffix}";

    private static readonly string NowTokenBody = "now";
    private static readonly string NowToken = $"{Prefix}{NowTokenBody}{Suffix}";

    public static readonly Dictionary<string, Func<ScenarioContext, string, object>> TypeValueGetters =
        new() { { "guid", (context, value) => GetGuidValue(context, value) }, { "string", (context, value) => GetStringValue(context, value) }, { "datetime", (context, value) => GetDateTimeValue(context, value) } };
    
    public static string GetStringValue(ScenarioContext context, string value, ISpecFlowOutputHelper? outputHelper = null)
    {
        if (!value.HasToken())
        {
            return value;
        }

        string valueLower = value.ToLower();
        if (valueLower.Equals(RandomValue))
        {
            return GetNewStringValue();
        }

        if (valueLower.Equals(DefaultValue))
        {
            return default;
        }

        if (valueLower.Equals(NullValue))
        {
            return null;
        }

        string token = GetToken(value);

        string? innervalueTokenValue = null;
        if (token.Contains(TypeNameSeparator.ToString()))
        {
            string tokenKey = token.GetTokenKey();
            string[] tokenSplit = tokenKey.Split(TypeNameSeparator);
            if (tokenSplit.Length == 2)
            {
                string tokenType = tokenSplit[1];
                string subtoken = $"{{{{{tokenSplit[0]}}}}}";

                Func<ScenarioContext, string, object> getValue = (innerContext, innerValue) =>
                {
                    innerContext.Add(innerValue.GetTokenKey(), tokenType);
                    return tokenType;
                };

                if (TypeValueGetters.ContainsKey(tokenType))
                {
                    getValue = TypeValueGetters[tokenType];
                }

                innervalueTokenValue = getValue(context, subtoken).ToString();
            }
        }

        bool hasTokenValue = TryGetTokenValue(context, value, GetNewStringValue, out string tokenValue);
        if (hasTokenValue && string.IsNullOrWhiteSpace(innervalueTokenValue))
        {
            return tokenValue;
        }

        if (string.IsNullOrWhiteSpace(innervalueTokenValue))
        {
            outputHelper?.WriteLine($"Value provided ({value}) has an internal token ({token}), retrieving or generating token value.");
            innervalueTokenValue = GetTokenValue(context, token, GetNewStringValue);
        }

        return value.Replace(token, innervalueTokenValue);
    }

    private static string GetNewStringValue()
    {
        var rants = new Rant();
        var commerce = new Commerce();

        string randomSentences = rants.Review(commerce.Product());
        return randomSentences;
    }

    public static Guid GetGuidValue(ScenarioContext context, string value)
    {
        string valueLower = value.ToLower();
        if (valueLower == RandomValue)
        {
            return Guid.NewGuid();
        }

        if (valueLower == DefaultValue)
        {
            return default;
        }

        bool hasTokenValue = TryGetTokenValue(context, value, Guid.NewGuid, out Guid tokenValue);
        if (hasTokenValue)
        {
            return tokenValue;
        }

        if (!Guid.TryParse(value, out Guid guidValue))
        {
            throw new Exception($"Could not resolve, generate, or parse a guid value for {value}.");
        }

        return guidValue;
    }

    public static DateTime GetDateTimeValue(ScenarioContext context, string value)
    {
        string rawValueCaseInsensitive = value.ToLower();
        string tokenKey = value.GetTokenKey();

        if (value.Contains(TypeNameSeparator))
        {
            string[] tokenSplit = tokenKey.Split(TypeNameSeparator);
            if (tokenSplit.Length == 2)
            {
                string contextKey = tokenSplit[0];
                string valueInstructions = tokenSplit[1];

                DateTime innerValue = GetDateTimeValue(context, $"{Prefix}{valueInstructions}{Suffix}");
                context.Add(contextKey, innerValue);
                return innerValue;
            }
        }

        if (rawValueCaseInsensitive == RandomValue)
        {
            return new Date().Future().ToUniversalTime();
        }

        if (rawValueCaseInsensitive == NowToken)
        {
            DateTime now = DateTime.UtcNow;
            return now;
        }

        if (rawValueCaseInsensitive.StartsWith($"{Prefix}{NowTokenBody}"))
        {
            return DateComputation.GetDateTimeValueFromToken(tokenKey.Trim());
        }

        bool hasTokenValue = TryGetTokenValue(context, value, () => new Date().Future().ToUniversalTime(), out DateTime tokenValue);
        if (hasTokenValue)
        {
            return tokenValue;
        }

        if (!DateTime.TryParse(value, out DateTime parsedValue))
        {
            throw new Exception($"Count not resolve, generate, or parse a date value for {value}.");
        }

        return parsedValue;
    }

    private static string GetToken(string value)
    {
        bool hasToken = value.HasToken(out string token);
        if (!hasToken)
        {
            return value;
        }

        return token;
    }

    private static bool HasToken(this string value)
    {
        return value.HasToken(out _);
    }

    private static bool HasToken(this string value, out string token)
    {
        Match regexMatch = Regex.Match(value, @"\{\{.*.\}\}");
        token = regexMatch.Value;
        return regexMatch.Success;
    }

    private static bool TryGetTokenValue<T>(
        ScenarioContext context,
        string value,
        Func<T> getNewValue,
        out T tokenValue)
    {
        tokenValue = default;
        if (value.IsToken())
        {
            tokenValue = GetTokenValue(context, value, getNewValue);
            return true;
        }

        return false;
    }

    private static bool IsToken(this string value)
    {
        return value.StartsWith(Prefix) && value.EndsWith(Suffix);
    }

    private static T GetTokenValue<T>(ScenarioContext context, string value, Func<T> getNewValue)
    {
        T tokenValue;
        string token = value.GetTokenKey();

        if (context.ContainsKey(token))
        {
            tokenValue = context.Get<T>(token);
            return tokenValue;
        }

        tokenValue = getNewValue();
        context.Add(token, tokenValue);

        Console.WriteLine($"Token placed into context. {{{token}:{tokenValue}}}");

        return tokenValue;
    }

    private static string GetTokenKey(this string token)
    {
        string tokenKey = token.Replace(Prefix, string.Empty)
            .Replace(Suffix, string.Empty);

        return tokenKey;
    }
}
