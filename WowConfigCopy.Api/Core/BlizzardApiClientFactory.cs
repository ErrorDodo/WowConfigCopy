namespace WowConfigCopy.Api.Core;

public class BlizzardApiClientFactory
{
    public BlizzardApiClient CreateClient(string region)
    {
        return new BlizzardApiClient(region);
    }
}