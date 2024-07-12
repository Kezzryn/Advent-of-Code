namespace AoC_2017_Day_15;

internal class Generator(int factor, long startValue)
{
    const int MODULO = 2147483647;

    private readonly int _factor = factor;
    private int _divisor = 1;
    private long _value = startValue;
    private readonly long _startvalue = startValue;

    public void ResetForPart2(int newDivisor)
    {
        _value = _startvalue;
        _divisor = newDivisor;
    }

    public ushort NextNum()
    {
        do
        {
            _value = (_value * _factor) % MODULO;
        } while (_value % _divisor != 0);

        return (ushort)_value;
    }
}