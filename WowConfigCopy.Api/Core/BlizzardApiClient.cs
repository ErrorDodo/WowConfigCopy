namespace WowConfigCopy.Api.Core;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class BlizzardApiClient
{
    private readonly HttpClient _httpClient;
    private string _accessToken;

    public BlizzardApiClient(string region)
    {
        var baseUri = region switch
        {
            "EU" => "https://eu.api.blizzard.com/",
            "US" => "https://us.api.blizzard.com/",
            _ => throw new ArgumentException("Invalid region")
        };

        _httpClient = new HttpClient { BaseAddress = new Uri(baseUri) };
    }

    public async Task AuthenticateAsync(string clientId, string clientSecret)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://us.battle.net/oauth/token")
        {
            Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            })
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsAsync<dynamic>();
        _accessToken = responseContent.access_token;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
    }
    
    public async Task<string> CallApiAsync(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API request failed: {response.StatusCode} - {errorContent}");
            }

            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("HTTP request exception occurred: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred: " + ex.Message);
        }
    }

}
