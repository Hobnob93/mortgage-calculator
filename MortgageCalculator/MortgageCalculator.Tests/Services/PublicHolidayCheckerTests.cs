using MortgageCalculator.Core.Services;

namespace MortgageCalculator.Tests.Services;

[TestFixture]
public class PublicHolidayCheckerTests
{
    [TestCase(1777, 3, 31)]
    [TestCase(2023, 4, 10)]
    public void EasterMonday_ShouldBeCorrect(int year, int expectedMonth, int expectedDay)
    {
        var sut = CreateSut();

        var easterMonday = sut.EasterMonday(year);

        easterMonday.Year.Should().Be(year);
        easterMonday.Month.Should().Be(expectedMonth);
        easterMonday.Day.Should().Be(expectedDay);
    }

    private PublicHolidayChecker CreateSut()
    {
        return new PublicHolidayChecker();
    }
}
