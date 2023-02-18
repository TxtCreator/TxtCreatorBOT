using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.Extensions;
using TxtCreatorBOT.Commands.GiveawayLogic;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Commands;

public class GiveawayCommand : ApplicationCommandsModule
{
    private readonly BotService _botService;
    private readonly ConfigService _configService;

    public GiveawayCommand(BotService botService, ConfigService configService)
    {
        _botService = botService;
        _configService = configService;
    }

    [SlashCommand("konkurs", "Tworzy i wysyła konkurs.")]
    public async Task GiveawayAsync(InteractionContext ctx,
        [Option("kanał","Podaj kanał.")] [ChannelTypes(ChannelType.Text)] DiscordChannel channel,
        [Option("nagroda", "Wpisz nagrodę.")] string prize,
        [Option("dni", "Wpisz dni.")] [MinimumValue(0)] int days,
        [Option("godziny", "Wpisz godziny.")] [MinimumValue(0)] int hours,
        [Option("minuty", "Wpisz minuty.")] [MinimumValue(0)] int minutes,
        [Option("wygrani", "Wpisz ile osób może wygrać.")] [MinimumValue(1)] int winners
        )
    {
        var time = days * 24 * 60 + hours * 60 + minutes;
        if (time == 0)
        {
            await ctx.CreateResponseAsync(_botService.CreateEmbed("Błąd!", "Nie podałeś dobrze czasu.", "red"), true);
            return;
        }
        var giveaway = new Giveaway(prize, time, winners, channel, _botService);
        await giveaway.SendGiveaway(DiscordEmoji.FromName(ctx.Client, _configService.EmojiName));
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", $"Pomyślnie stworzyłeś konkurs na {channel.Mention}"), true);
    }
}