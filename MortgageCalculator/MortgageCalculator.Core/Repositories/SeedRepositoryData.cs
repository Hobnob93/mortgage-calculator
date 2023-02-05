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
            EstimatedValue = 301000m
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
            EndDate = new DateOnly(2056, 6, 30),
            FullTermLength = 35,
            FixedTermEndDate = new DateOnly(2023, 8, 30),
            FixedTermLength = 2,
            HasRemortgaged = false,
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
                EndDate = mortgage.EndDate
            }
        };

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
            PaidOn = new DateOnly(2022, 3, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 4, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 5, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 6, 1)
        }, mortgagePaymentTemplate with
        {
            PaidOn = new DateOnly(2022, 7, 1)
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
}
