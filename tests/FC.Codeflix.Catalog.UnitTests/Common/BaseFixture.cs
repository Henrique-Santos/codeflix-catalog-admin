using Bogus;

namespace FC.Codeflix.Catalog.UnitTests.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }

    protected BaseFixture()
    {
        Faker = new Faker("pt_BR");
    }

    public static bool GetRandomBoolean() 
    {
        return new Random().NextDouble() < 0.5;
    } 
}