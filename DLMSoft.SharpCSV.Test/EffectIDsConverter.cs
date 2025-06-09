namespace DLMSoft.SharpCSV.Test;

class EffectIDsConverter : CSVConverter<string[]> {
    public override string[]? Parse(string input)
    {
        var items = input.Split(':');
        return [..from i in items select i.Trim()];
    }

    public override string GetString(string[]? input)
    {
        if (input == null || input.Length == 0) return string.Empty;
        return string.Join(':', input);
    }
}