namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Create;

public class CreateCategoryTestDataGenerator 
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryApiTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add([
                        fixture.GetExampleInputWithName(fixture.GetInvalidShortName()),
                        "Name must be longer than 3 characters"
                    ]);
                    break;
                case 1:
                    invalidInputsList.Add([
                        fixture.GetExampleInputWithName(fixture.GetInvalidTooLongName()),
                        "Name must be less than 255 characters"
                    ]);
                    break;
                case 2:
                    invalidInputsList.Add([
                        fixture.GetExampleInputWithDescription(fixture.GetInvalidTooLongDescription()),
                        "Description must be less than 10000 characters"
                    ]);
                    break;
                default:
                    break;
            }
        }

        return invalidInputsList;
    }
}