using MortgageCalculator.Core.Services;

namespace MortgageCalculator.Tests.Services;

[TestFixture]
public class PublicHolidayCheckerTests
{
    [TestCase(2022, 1, 3)]
    [TestCase(2023, 1, 2)]
    [TestCase(2024, 1, 1)]
    public void NewYearsDay(int year, int expectedMonth, int expectedDay)
    {
        var sut = CreateSut();

        var newYearsDay = sut.NewYearsDay(year);

        newYearsDay.Year.Should().Be(year);
        newYearsDay.Month.Should().Be(expectedMonth);
        newYearsDay.Day.Should().Be(expectedDay);
    }

    [TestCase(2023, 4, 7)]
    [TestCase(2024, 3, 29)]
    [TestCase(2025, 4, 18)]
    public void GoodFriday_ShouldBeCorrect(int year, int expectedMonth, int expectedDay)
    {
        var sut = CreateSut();

        var goodFriday = sut.GoodFriday(year);

        goodFriday.Year.Should().Be(year);
        goodFriday.Month.Should().Be(expectedMonth);
        goodFriday.Day.Should().Be(expectedDay);
    }

    [TestCase(1777, 3, 31)]
    [TestCase(2023, 4, 10)]
    [TestCase(2024, 4, 1)]
    public void EasterMonday_ShouldBeCorrect(int year, int expectedMonth, int expectedDay)
    {
        var sut = CreateSut();

        var easterMonday = sut.EasterMonday(year);

        easterMonday.Year.Should().Be(year);
        easterMonday.Month.Should().Be(expectedMonth);
        easterMonday.Day.Should().Be(expectedDay);
    }

    [TestCase(2023, 1)]
    [TestCase(2024, 6)]
    [TestCase(2025, 5)]
    public void MayDay_ShouldBeCorrect(int year, int expectedDay)
    {
        var sut = CreateSut();

        var mayDay = sut.MayDay(year);

        const int expectedMonth = 5;
        mayDay.Year.Should().Be(year);
        mayDay.Month.Should().Be(expectedMonth);
        mayDay.Day.Should().Be(expectedDay);
    }

    [TestCase(2023, 29)]
    [TestCase(2024, 27)]
    [TestCase(2025, 26)]
    public void SpringDay_ShouldBeCorrect(int year, int expectedDay)
    {
        var sut = CreateSut();

        var springDay = sut.SpringDay(year);

        const int expectedMonth = 5;
        springDay.Year.Should().Be(year);
        springDay.Month.Should().Be(expectedMonth);
        springDay.Day.Should().Be(expectedDay);
    }

    [TestCase(2022, 29)]
    [TestCase(2023, 28)]
    [TestCase(2024, 26)]
    public void SummerDay_ShouldBeCorrect(int year, int expectedDay)
    {
        var sut = CreateSut();

        var summerDay = sut.SummerDay(year);

        const int expectedMonth = 8;
        summerDay.Year.Should().Be(year);
        summerDay.Month.Should().Be(expectedMonth);
        summerDay.Day.Should().Be(expectedDay);
    }

    [TestCase(2020, 25)]
    [TestCase(2021, 27)]
    [TestCase(2022, 27)]
    public void ChristmasDay_ShouldBeCorrect(int year, int expectedDay)
    {
        var sut = CreateSut();

        var christmasDay = sut.ChristmasDay(year);

        const int expectedMonth = 12;
        christmasDay.Year.Should().Be(year);
        christmasDay.Month.Should().Be(expectedMonth);
        christmasDay.Day.Should().Be(expectedDay);
    }

    [TestCase(2020, 28)]
    [TestCase(2021, 28)]
    [TestCase(2022, 26)]
    public void BoxingDay_ShouldBeCorrect(int year, int expectedDay)
    {
        var sut = CreateSut();

        var boxingDay = sut.BoxingDay(year);

        const int expectedMonth = 12;
        boxingDay.Year.Should().Be(year);
        boxingDay.Month.Should().Be(expectedMonth);
        boxingDay.Day.Should().Be(expectedDay);
    }

    [TestCase(2022, 1, 3)]      // New Years Day
    [TestCase(2024, 3, 29)]     // Good Friday
    [TestCase(1777, 3, 31)]     // Easter Monday
    [TestCase(2024, 5, 6)]      // May Day
    [TestCase(2025, 5, 26)]     // Spring Day
    [TestCase(2022, 8, 29)]     // Summer Day
    [TestCase(2021, 12, 27)]    // Christmas Day
    [TestCase(2020, 12, 28)]    // Boxing Day
    public void IsBankHoliday_OnAGivenDay_ThatIsABankHoliday_ShouldReturnTrue(int year, int month, int day)
    {
        var sut = CreateSut();
        var date = new DateOnly(year, month, day);

        var isBankHoliday = sut.IsBankHoliday(date);

        isBankHoliday.Should().BeTrue();
    }

    [TestCase(2023, 2, 12)]
    [TestCase(1987, 6, 30)]
    [TestCase(1993, 10, 11)]
    public void IsBankHoliday_OnAGivenDay_ThatIsNotABankHoliday_ShouldReturnFalse(int year, int month, int day)
    {
        var sut = CreateSut();
        var date = new DateOnly(year, month, day);

        var isBankHoliday = sut.IsBankHoliday(date);

        isBankHoliday.Should().BeFalse();
    }

    private PublicHolidayChecker CreateSut()
    {
        return new PublicHolidayChecker();
    }
}
