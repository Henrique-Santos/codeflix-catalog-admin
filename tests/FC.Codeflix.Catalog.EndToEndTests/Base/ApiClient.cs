using System.Text;
using System.Text.Json;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

public class ApiClient 
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(string url, object payload) where TOutput : class
    {
        TOutput? output = null;

        var input = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, input);
        
        var outputString = await response.Content.ReadAsStringAsync();

        if (!string.IsNullOrWhiteSpace(outputString))
        {
            output = JsonSerializer.Deserialize<TOutput>(outputString, GetOptions());
        }

        return (response, output);
    }

    private static JsonSerializerOptions GetOptions()
    {
        return new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}