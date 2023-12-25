using WowConfigCopy.Api.Core;
using WowConfigCopy.Api.Interfaces;

namespace WowConfigCopy.Api.Services
{
    public class GenericApiService : IGenericApiService
    {
        private readonly BlizzardApiClientFactory _apiClientFactory;

        public GenericApiService(BlizzardApiClientFactory apiClientFactory)
        {
            _apiClientFactory = apiClientFactory;
        }

        public async Task<string> GetCharacterFaction(string characterName, string realm, string region)
        {
            try
            {
                var apiClient = _apiClientFactory.CreateClient(region);
                await apiClient.AuthenticateAsync("clientId", "clientSecret");

                var endpoint = $"profile/wow/character/{realm}/{characterName}";
                var jsonResponse = await apiClient.CallApiAsync(endpoint);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving character faction: " + ex.Message);
            }
        }
    }
}