namespace WowConfigCopy.Api.Interfaces;

public interface IGenericApiService
{
    Task<string> GetCharacterFaction(string characterName, string realm, string region);
}