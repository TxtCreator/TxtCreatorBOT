using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using TxtCreatorBot.Services;

namespace TxtCreatorBot.Commands;

public class WarnsCommand : ApplicationCommandsModule
{
    private readonly UserService _userService;
    private readonly BotService _botService;

    public WarnsCommand(UserService userService, BotService botService)
    {
        _userService = userService;
        _botService = botService;
    }
    
    [SlashCommand("ostrzeżenia", "Dodaje lub usuwa użytkownikowi podaną ilość ostrzeżeń.")]
    public async Task WarnsAsync(InteractionContext ctx, [Option("czynność", "Podaj co chcesz zrobić.")] State state, [Option("użytkownik", "Podaj użytkownika.")] DiscordUser user, [Option("ilość", "Podaj ilość")] long amount)
    {
        var member = (DiscordMember)user;
        var message = "Pomyślnie dodałeś ostrzeżenia!";
        if (state == State.Remove)
        {
            amount = -amount;
            message = "Pomyślnie odjąłeś ostrzeżenia!";
        }
        var isSuccess = await _userService.AddWarnsAsync(member.Id, amount);
        if (state == State.Add && (await _userService.GetUserModelAsync(member.Id)).Warns >= 3)
        {
            try
            {
                await member.SendMessageAsync(_botService.CreateEmbed("Ups..", "Dostałeś bana za osiągnięcie 3 ostrzeżeń!", "red"));
            } catch (Exception) {}
            await member.BanAsync(reason: "Osiągnięto 3 lub więcej ostrzeżeń.");
        }

        if (isSuccess)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, _botService.CreateInteractionEmbed("Sukces!",
                message, ephemeral: true));
        }
        else
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,_botService.CreateInteractionEmbed("Błąd!",
                "Coś poszło nie tak, najprawdopodobniej chciałeś dodać/odjąć ostrzeżenia na minusowej liczbie.", "red", ephemeral: true));
        }
    }
}

public enum State
{
    [ChoiceName("dodaj")]
    Add,
    [ChoiceName("usuń")]
    Remove
}