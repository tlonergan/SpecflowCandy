namespace SpecflowCandy.Tests.Models;

public class ValueEntity
{
    public Guid GuidValue { get; set; }
    public Guid? GuidNullableValue { get; set; }
    public string? StringValue { get; set; }
    public DateTime DateTimeValue { get; set; }

    public override string ToString()
    {
        return
            $"Value Entity:{Environment.NewLine}\tGuid Value: {GuidValue}.{Environment.NewLine}\tNullable Guid Value: {GuidNullableValue}.{Environment.NewLine}\tString Value: {StringValue}.{Environment.NewLine}\tDate Time Value: {DateTimeValue}.";
    }
}
