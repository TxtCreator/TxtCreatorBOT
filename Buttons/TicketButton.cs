using System.Text;
using DSharpPlus;
using DSharpPlus.ButtonCommands;
using DSharpPlus.Entities;
using TxtCreatorBOT.Extensions;
using TxtCreatorBot.Services;

namespace TxtCreatorBot.Buttons;

public class TicketButton : ButtonCommandModule
{
    private readonly BotService _botService;
    private readonly ConfigService _configService;

    public TicketButton(BotService botService, ConfigService configService)
    {
        _botService = botService;
        _configService = configService;
    }

    [ButtonCommand("ticket")]
    public async Task TicketCreateAsync(ButtonContext ctx, string name)
    {
        var user = ctx.User;
        if (ctx.Guild.Channels.FirstOrDefault(channel => channel.Value.Name == $"{name}-{user.Id}").Value != null)
        {
            await ctx.CreateResponseAsync(_botService.CreateEmbed("Błąd!", "Posiadasz już taki ticket!", "red"), true);
            return;
        }

        var channel = await ctx.Guild.CreateTextChannelAsync($"{name}-{user.Id}",
            ctx.Guild.Channels.First(channel => channel.Key == _configService.TicketCategoryId).Value);
        await channel.AddOverwriteAsync((DiscordMember)ctx.User, Permissions.AccessChannels);
        var message =
            new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed($"Witaj {user.Username}",
                    "Opisz problem lub aplikuj na partnera/twórcę. Na odpowiedź poczekaj cierpliwie.")).AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Danger, $"@close.{user.Id}", "Zamknij"))
                .WithAllowedMention(new UserMention(user.Id)).WithContent(user.Mention);
        await channel.SendMessageAsync(message);
        await ctx.CreateResponseAsync(
            _botService.CreateEmbed("Sukces!", $"Pomyślnie stworzyłeś ticket! {channel.Mention}"), true);
    }

    [ButtonCommand("close")]
    public async Task TicketCloseAsync(ButtonContext ctx, ulong memberId)
    {
        var channel = ctx.Channel;
        var author = ctx.User;

        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Pomyślnie zamknąłeś ticket!"), true);
        var member = ctx.Guild.Members[memberId];
        if (member != null)
        {
            await channel.DeleteOverwriteAsync(member);
            await using var stream = GenerateStreamFromString(await FormatTicketContent(channel));
            var message = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed(
                    "Twój ticket został zamknięty!",
                    $"Zamknął go użytkownik {author.Mention}. W załączniku jest również jego zapis."))
                .AddFile($"{channel.Name}.txt", stream);
            try
            {
                await member.SendMessageAsync(message);
            }
            catch (Exception) {}
        }
        
        if (!channel.Name.ToLower().StartsWith("zamknięty")) await channel.ModifyAsync(action => action.Name = $"zamknięty-{ctx.Channel.Name}");
        var adminMessage = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed("Panel administracji",
            $"Ticket został zamknięty przez: {author.Mention}")).AddComponents(
            new DiscordButtonComponent(ButtonStyle.Primary, "@remove", "Usuń"),
            new DiscordButtonComponent(ButtonStyle.Primary, $"@reopen.{memberId}", "Otwórz ponownie"),
            new DiscordButtonComponent(ButtonStyle.Primary, "@save", "Zapisz")
        );
        await channel.SendMessageAsync(adminMessage);
    }

    [ButtonCommand("remove")]
    public async Task TicketRemoveAsync(ButtonContext ctx)
    {
        await ctx.Channel.DeleteAsync();
    }

    [ButtonCommand("reopen")]
    public async Task TicketReopenAsync(ButtonContext ctx, ulong memberId)
    {
        var channel = ctx.Channel;
        if (ctx.Guild.Members[memberId] != null)
        {
            await channel.AddOverwriteAsync(ctx.Guild.Members[memberId], Permissions.AccessChannels);
        }

        await ctx.Message.DeleteAsync();
        await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Pomyślnie otworzyłeś ponownie ticket!"),
            true);
    }
    
    [ButtonCommand("save")]
    public async Task TicketSaveAsync(ButtonContext ctx)
    {
        var channel = ctx.Channel;
        var user = ctx.User;
        await using var stream = GenerateStreamFromString(await FormatTicketContent(channel));
        var message = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed("Zapis ticketu!",
                "Oto zapis ticketa."))
            .AddFile($"{channel.Name}.txt", stream);
        try
        {
      
            await ((DiscordMember)user).SendMessageAsync(message);
            await ctx.CreateResponseAsync(_botService.CreateEmbed("Sukces!", "Sprawdź wiadomość prywatną od bota."), true);
        }
        catch (Exception)
        {
            await ctx.CreateResponseAsync(_botService.CreateEmbed("Błąd!", "Masz zablokowane wiadomości prywatne", "red"), true);
        }
       
    }

    private static async Task<string> FormatTicketContent(DiscordChannel channel)
    {
        var builder = new StringBuilder();
        foreach (var discordMessage in (await channel.GetMessagesAsync()).Reverse())
        {
            if (string.IsNullOrWhiteSpace(discordMessage.Content) || discordMessage.Author.IsBot) continue;
            builder.Append(discordMessage.Author.Username).Append(" : ").Append(discordMessage.Content).Append('\n');
        }

        if (string.IsNullOrWhiteSpace(builder.ToString()))
        {
            builder.Append("Brak wiadomości");
        }
        return builder.ToString();
    }

    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

}