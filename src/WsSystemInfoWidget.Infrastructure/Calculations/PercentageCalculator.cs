namespace WsSystemInfoWidget.Infrastructure.Calculations;

/// <summary>
/// Pure used/total to percentage math, kept free of any OS dependency so it's
/// directly unit-testable.
/// </summary>
public static class PercentageCalculator
{
    public static double CalculateUsedPercentage(double usedAmount, double totalAmount)
    {
        if (totalAmount <= 0d)
        {
            return 0d;
        }

        var percentage = usedAmount / totalAmount * 100d;
        return Math.Clamp(percentage, 0d, 100d);
    }
}
