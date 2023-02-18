using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.Extensions;
using TxtCreatorBot.Services;

namespace TxtCreatorBot.Commands;

public class VerificationCommand : ApplicationCommandsModule
{
    private readonly BotService _botService;

    public VerificationCommand(BotService botService)
    {
        _botService = botService;
    }

    [SlashCommand("weryfikacja", "Tworzy wiadomość do weryfikacji.")]
    public async Task VerificationAsync(InteractionContext ctx)
    {
        var message = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed("Weryfikacja",
                "Zweryfikuj się poniżej, aby odblokować wszystkie kanały.\nDokonująć weryfikacji oznajmiasz zapoznanie się i przestrzeganie regulaminu.").WithImageUrl("https://cdn.discordapp.com/attachments/920442986339377193/1062506390461100052/weryfikacja_japierdole_uwu.png"))
            .AddComponents(new DiscordButtonComponent(ButtonStyle.Primary, "@verification", "Weryfikacja"));
        await ctx.Channel.SendMessageAsync(message);
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Pomyślnie wysłałeś wiadomość!"), true);
    }
}