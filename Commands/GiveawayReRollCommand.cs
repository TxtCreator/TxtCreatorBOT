using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.Extensions;
using TxtCreatorBOT.Commands.GiveawayLogic;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Commands;

public class GiveawayReRollCommand : ApplicationCommandsModule
{
    private readonly BotService _botService;
    private readonly ConfigService _configService;

    public GiveawayReRollCommand(BotService botService, ConfigService configService)
    {
        _botService = botService;
        _configService = configService;
    }
    
    [SlashCommand("przelosuj", "Wybiera od nowa podaną ilość osób.")]
    public async Task GiveawayReRollAsync(InteractionContext ctx,
        [Option("kanał", "Podaj kanał.")] [ChannelTypes(ChannelType.Text)] DiscordChannel channel,
        [Option("id", "Podaj id wiadomości.")] string id,
        [Option("wygrani", "Wpisz ile osób ma zostać wygranymi.")] [MinimumValue(1)] int winnersAmount
    )
    {
        DiscordMessage message;
        try
        {
            message = await channel.GetMessageAsync(ulong.Parse(id));
        }
        catch (Exception)
        {
            await ctx.CreateResponseAsync(_botService.CreateEmbed("Błąd!", "Nie istnieje taka wiadomość.", "red"), true);
            return;
        }

        var winners = await Giveaway.DrawWinners(DiscordEmoji.FromName(ctx.Client, _configService.EmojiName), message, winnersAmount);
        await Giveaway.SendWinners(message.Channel, winners, message.Id, _botService);
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Pomyślnie zmieniłeś zwycięzców."), true);
    }
}