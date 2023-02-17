using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Interfaces;
using MortgageCalculator.Core.Documents;
using MortgageCalculator.Core.Models;

namespace MortgageCalculator.Core.Repositories;

public class SeedRepositoryData : MongoRepositoryBase, ISeedRepositoryData
{
    private readonly CollectionNamesConfig _collectionNames;

	public SeedRepositoryData(IMongoDatabase mongoDatabase, IOptions<CollectionNamesConfig> collectionNames)
		: base(mongoDatabase)
	{
        _collectionNames = collectionNames.Value;
	}

    public async Task SeedData()
    {
        var houses = await GetAllFromCollection<House>(_collectionNames.Houses);
        if (!houses.Any())
        {
            await SeedHouseData();
        }

        var owners = await GetAllFromCollection<Owner>(_collectionNames.Owners);
        if (!owners.Any())
        {
            await SeedOwnerData();
        }

        var mortgages = await GetAllFromCollection<Mortgage>(_collectionNames.Mortgages);
        if (!mortgages.Any())
        {
            await SeedMortgageData();
        }

        var payments = await GetAllFromCollection<MortgagePayment>(_collectionNames.MortgagePayments);
        if (!payments.Any())
        {
            await SeedPaymentsData();
        }

        var usefulLinks = await GetAllFromCollection<UsefulLink>(_collectionNames.UsefulLinks);
        if (!usefulLinks.Any())
        {
            await SeedUsefulLinksData();
        }
    }

    private string NewGuid()
    {
        return Guid.NewGuid().ToString();
    }

    private async Task SeedHouseData()
    {
        await CreateDocumentInCollection(_collectionNames.Houses, new House
        {
            Address1 = "48 Whitley Mead",
            Address2 = "Stoke Gifford",
            City = "Bristol",
            Postcode = "BS34 8XT",
            PurchasedValue = 262500m,
            EstimatedValue = 296240m
        });
    }

    private async Task SeedOwnerData()
    {
        await CreateDocumentsInCollection(_collectionNames.Owners, new Owner
        {
            FirstName = "Kyle",
            LastName = "Hobdey"
        }, new Owner
        {
            FirstName = "Amber",
            LastName = "Gilmour"
        });
    }

    private async Task SeedMortgageData()
    {
        var house = await GetSingleFromCollection<House>(_collectionNames.Houses, _ => true);

        await CreateDocumentInCollection(_collectionNames.Mortgages, new Mortgage
        {
            Name = "Halifax 2021",
            Provider = "Halifax",
            AmountBorrowed = 249374m,            
            Opened = new DateOnly(2021, 9, 14),
            FullTermLength = 35,
            FirstPaymentAmount = 1494.08m,
            FirstPaymentDate = new DateOnly(2021, 10, 1),
            House = house,
            InterestPeriods = new() 
            {
                new InterestPeriod
                {
                    Id = NewGuid(),
                    From = new DateOnly(2021, 9, 14),
                    To = new DateOnly(2023, 9, 30),
                    InterestRate = 3.73,
                    MonthlyPayment = 1060.85m
                }
            }
        });
    }

    private async Task SeedPaymentsData()
    {
        var mortgage = await GetSingleFromCollection<Mortgage>(_collectionNames.Mortgages, _ => true);

        await CreateDocumentsInCollection(_collectionNames.MortgagePayments,
            (new[]
            {
                (PaidOn: new DateOnly(2022, 2, 11), Amount: 200m, IsOverPay: true),
                (PaidOn: new DateOnly(2022, 2, 28), Amount: 100m, IsOverPay: true),
                (PaidOn: new DateOnly(2022, 3, 23), Amount: 100m, IsOverPay: true),
                (PaidOn: new DateOnly(2022, 4, 25), Amount: 100m, IsOverPay: true),
                (PaidOn: new DateOnly(2022, 5, 23), Amount: 100m, IsOverPay: true),
                (PaidOn: new DateOnly(2022, 6, 23), Amount: 100m, IsOverPay: true),
                (PaidOn: new DateOnly(2022, 7, 22), Amount: 100m, IsOverPay: true)
            }).Select(x => new MortgagePayment
            {
                Id = NewGuid(),
                Amount = x.Amount,
                PaidOn = x.PaidOn,
                IsOverPayment = x.IsOverPay,
                PaidTo = new MortgagePaymentTo
                {
                    Id = mortgage.Id,
                    Name = mortgage.Name,
                    StartDate = mortgage.Opened
                }
            }).ToArray());
    }

    private async Task SeedUsefulLinksData()
    {
        await CreateDocumentsInCollection(_collectionNames.UsefulLinks,
            new UsefulLink
            {
                Name = "MSE - Mortgage vs Savings",
                IconName = "Savings",
                Icon = "<rect fill=\"none\" height=\"24\" width=\"24\"/><g><path d=\"M19.83,7.5l-2.27-2.27c0.07-0.42,0.18-0.81,0.32-1.15C17.96,3.9,18,3.71,18,3.5C18,2.67,17.33,2,16.5,2 c-1.64,0-3.09,0.79-4,2l-5,0C4.46,4,2,6.46,2,9.5S4.5,21,4.5,21l5.5,0v-2h2v2l5.5,0l1.68-5.59L22,14.47V7.5H19.83z M13,9H8V7h5V9z M16,11c-0.55,0-1-0.45-1-1c0-0.55,0.45-1,1-1s1,0.45,1,1C17,10.55,16.55,11,16,11z\"/></g>",
                Href = "https://www.moneysavingexpert.com/mortgages/mortgages-vs-savings/"
            }, new UsefulLink
            {
                Name = "MSE - Interest Rates 2023",
                IconName = "YouTube",
                Icon = "<path d=\"M10 15l5.19-3L10 9v6m11.56-7.83c.13.47.22 1.1.28 1.9.07.8.1 1.49.1 2.09L22 12c0 2.19-.16 3.8-.44 4.83-.25.9-.83 1.48-1.73 1.73-.47.13-1.33.22-2.65.28-1.3.07-2.49.1-3.59.1L12 19c-4.19 0-6.8-.16-7.83-.44-.9-.25-1.48-.83-1.73-1.73-.13-.47-.22-1.1-.28-1.9-.07-.8-.1-1.49-.1-2.09L2 12c0-2.19.16-3.8.44-4.83.25-.9.83-1.48 1.73-1.73.47-.13 1.33-.22 2.65-.28 1.3-.07 2.49-.1 3.59-.1L12 5c4.19 0 6.8.16 7.83.44.9.25 1.48.83 1.73 1.73z\"/>",
                Href = "https://www.youtube.com/watch?v=RlBs9UXiiB8"
            });
    }
}
