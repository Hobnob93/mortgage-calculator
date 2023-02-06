using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MortgageCalculator.Core.Config;
using MortgageCalculator.Core.Interfaces;
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

    private async Task SeedHouseData()
    {
        await CreateDocumentInCollection(_collectionNames.Houses, new House
        {
            Address1 = "48 Whitley Mead",
            Address2 = "Stoke Gifford",
            City = "Bristol",
            Postcode = "BS34 8XT",
            PurchasedValue = 262500m,
            EstimatedValue = 269240m
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
            AmountBorrowed = 250373m,
            InterestRate = 3.73,
            StartDate = new DateOnly(2021, 9, 30),
            FullTermLength = 35,
            FixedTermEndDate = new DateOnly(2023, 8, 30),
            FixedTermLength = 2,
            MonthlyPayment = 1060.85m
        });
    }

    private async Task SeedPaymentsData()
    {
        var mortgage = await GetSingleFromCollection<Mortgage>(_collectionNames.Mortgages, _ => true);
        var mortgagePaymentTemplate = new MortgagePayment
        {
            IsOverPayment = false,
            Amount = mortgage.MonthlyPayment,
            PaidTo = new MortgagePaymentTo
            {
                Provider = mortgage.Provider,
                StartDate = mortgage.StartDate,
                FixedTermEndDate = mortgage.FixedTermEndDate
            }
        };

        var ownerKyle = await GetSingleFromCollection<Owner>(_collectionNames.Owners, o => o.FirstName == "Kyle");
        var ownerAmber = await GetSingleFromCollection<Owner>(_collectionNames.Owners, o => o.FirstName == "Amber");

        await CreateDocumentsInCollection(_collectionNames.MortgagePayments, mortgagePaymentTemplate with
        {
            Amount = 1825.37m,
            PaidOn = new DateOnly(2021, 10, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2021, 11, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2021, 12, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 1, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 2, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 2, 11),
            Amount = 200m,
            Owner = ownerKyle,
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 2, 28),
            Amount = 100m,
            Owner = ownerAmber,
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 3, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 3, 23),
            Amount = 100m,
            Owner = ownerAmber,
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 4, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 4, 25),
            Amount = 100m,
            Owner = ownerAmber,
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 5, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 5, 23),
            Amount = 100m,
            Owner = ownerAmber,
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 6, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 6, 23),
            Amount = 100m,
            Owner = ownerAmber,
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 7, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 7, 22),
            Amount = 100m,
            Owner = ownerAmber,
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 8, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 9, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 10, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 11, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 12, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2023, 1, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2023, 2, 1)
        });
    }

    private async Task SeedUsefulLinksData()
    {
        await CreateDocumentInCollection(_collectionNames.UsefulLinks, new UsefulLink
        {
            Name = "Mortgage vs Savings",
            Icon = "<rect fill=\"none\" height=\"24\" width=\"24\"/><g><path d=\"M19.83,7.5l-2.27-2.27c0.07-0.42,0.18-0.81,0.32-1.15C17.96,3.9,18,3.71,18,3.5C18,2.67,17.33,2,16.5,2 c-1.64,0-3.09,0.79-4,2l-5,0C4.46,4,2,6.46,2,9.5S4.5,21,4.5,21l5.5,0v-2h2v2l5.5,0l1.68-5.59L22,14.47V7.5H19.83z M13,9H8V7h5V9z M16,11c-0.55,0-1-0.45-1-1c0-0.55,0.45-1,1-1s1,0.45,1,1C17,10.55,16.55,11,16,11z\"/></g>"
        });
    }
}
