

using System.Text.Json;

namespace TxtCreatorBot.Services;

public class ConfigService
{
    public static ConfigService Create(string name)
    {
        return JsonSerializer.Deserialize<ConfigService>(File.OpenRead($"{name}.json"))!;
    }
    
    public ulong TicketCategoryId { get; init; }
    public ulong LobbyChannelId { get; init; }
    public ulong VerificationRoleId { get; init; }
    public ulong PropositionChannelId { get; init; }
}