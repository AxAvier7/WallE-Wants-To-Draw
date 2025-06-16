public class ExValue
{
    public ValueType Type { get; }
    private readonly int _intValue;
    private readonly bool _boolValue;
    private readonly string _stringValue;

    public ExValue(int value)
    {
        Type = ValueType.Number;
        _intValue = value;
        _boolValue = value != 0;
    }

    public ExValue(bool value)
    {
        Type = ValueType.Boolean;
        _boolValue = value;
        _intValue = value ? 1 : 0;
    }

    public ExValue(string value)
    {
        Type = ValueType.String;
        _stringValue = value;
        _intValue = 0;
        _boolValue = false;
    }

    public int AsInt() => Type == ValueType.Number ? _intValue : (_boolValue ? 1 : 0);
    public bool AsBool() => Type == ValueType.Boolean ? _boolValue : (_intValue != 0);
    public string AsString() => Type == ValueType.String ? _stringValue : throw new System.InvalidCastException("ExValue is not a string");

    public static implicit operator ExValue(int value) => new ExValue(value);
    public static implicit operator ExValue(bool value) => new ExValue(value);
    public static implicit operator ExValue(string value) => new ExValue(value);

    public enum ValueType { Number, Boolean, String };
}
