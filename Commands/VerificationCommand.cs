using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using TxtCreatorBot.Services;

namespace TxtCreatorBot.Commands;

public class VerificationCommand : ApplicationCommandModule
{
    private readonly BotService _botService;

    public VerificationCommand(BotService botService)
    {
        _botService = botService;
    }

    [SlashCommand("weryfikacja", "Tworzy embeda.")]
    public async Task VerificationCommandAsync(InteractionContext ctx)
    {
        var message = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed("Weryfikacja",
                "Zweryfikuj się poniżej, aby odblokować wszystkie kanały.\nDokonująć weryfikacji oznajmiasz zapoznanie się i przestrzeganie regulaminu.", image:"https://cdn.discordapp.com/attachments/920442986339377193/1062506390461100052/weryfikacja_japierdole_uwu.png"))
            .AddComponents(new DiscordButtonComponent(ButtonStyle.Primary, "@verification", "Weryfikacja"));
        await ctx.Channel.SendMessageAsync(message);
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Pomyślnie wysłałeś wiadomość!"), true);
    }
}