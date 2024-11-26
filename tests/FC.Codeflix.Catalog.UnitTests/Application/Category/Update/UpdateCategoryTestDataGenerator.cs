namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Update;

public class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new UpdateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 4;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add([
                        fixture.GetInvalidInputShortName(),
                        "Name must be longer than 3 characters"
                    ]);
                    break;
                case 1:
                    invalidInputsList.Add([
                        fixture.GetInvalidInputTooLongName(),
                        "Name must be less than 255 characters"
                    ]);
                    break;
                case 2:
                    invalidInputsList.Add([
                        fixture.GetInvalidInputTooLongDescription(),
                        "Description must be less than 10000 characters"
                    ]);
                    break;
                default:
                    break;
            }
        }

        return invalidInputsList;
    }

    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();
        for (int indice = 0; indice < times; indice++)
        {
            var category = fixture.GetExampleCategory();
            var input = fixture.GetValidInput(category.Id);
            yield return new object[] { category, input };
        }
    }
}