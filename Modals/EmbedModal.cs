using DSharpPlus.ModalCommands;
using DSharpPlus.ModalCommands.Attributes;
using TxtCreatorBot.Services;

namespace TxtCreatorBot.Modals;

public class EmbedModal : ModalCommandModule
{
    private readonly BotService _botService;

    public EmbedModal(BotService botService)
    {
        _botService = botService;
    }
    
    [ModalCommand("embed")]
    public async Task EmbedModalAsync(ModalContext ctx, ulong id, string title, string description, string color, string image, string thumbnail)
    {
        await (await ctx.Guild.GetChannelsAsync()).First(channel => channel.Id == id).SendMessageAsync(_botService.CreateEmbed(title, description, color, image, thumbnail));
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Pomyślnie stworzyłeś embed!"), true);
    }
}