using System.Text;
using DisCatSharp;
using DisCatSharp.Entities;
using DisCatSharp.Enums;
using DisCatSharp.EventArgs;
using TxtCreatorBot.Services;

namespace TxtCreatorBOT.Events.Buttons;

[EventHandler]
public class TicketButton 
{
    private readonly BotService _botService;
    private readonly ConfigService _configService;

    public TicketButton(BotService botService, ConfigService configService)
    {
        _botService = botService;
        _configService = configService;
    }

    [Event(DiscordEvent.ComponentInteractionCreated)]
    public async Task TicketCreateAsync(DiscordClient client, ComponentInteractionCreateEventArgs ctx)
    {
        if (!ctx.Id.StartsWith("ticket")) return;
        var name = ctx.Id.Split(".")[1];
    
     
        var user = ctx.User;
        var guild = ctx.Guild;
        if (guild.Channels.FirstOrDefault(channel => channel.Value.Name == $"{name}-{user.Id}").Value != null)
        {
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, _botService.CreateInteractionEmbed("Błąd!", "Posiadasz już taki ticket!", "red", ephemeral: true));
            return;
        }

        var channel = await guild.CreateTextChannelAsync($"{name}-{user.Id}",
            guild.Channels.First(channel => channel.Key == _configService.TicketCategoryId).Value);
        await channel.AddOverwriteAsync((DiscordMember)user, Permissions.AccessChannels);
        var message =
            new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed($"Witaj {user.Username}",
                    "Opisz problem lub aplikuj na partnera/twórcę. Na odpowiedź poczekaj cierpliwie.")).AddComponents(
                    new DiscordButtonComponent(ButtonStyle.Danger, $"close.{user.Id}", "Zamknij"))
                .WithAllowedMention(new UserMention(user.Id)).WithContent(user.Mention);
        await channel.SendMessageAsync(message);
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            _botService.CreateInteractionEmbed("Sukces!", $"Pomyślnie stworzyłeś ticket! {channel.Mention}", ephemeral: true));
    }

    [Event(DiscordEvent.ComponentInteractionCreated)]
    public async Task TicketCloseAsync(DiscordClient client, ComponentInteractionCreateEventArgs ctx)
    {
        if (!ctx.Id.StartsWith("close")) return;
        var memberId = ulong.Parse(ctx.Id.Split(".")[1]);
        var channel = ctx.Channel;
        var author = ctx.User;
    
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, _botService.CreateInteractionEmbed("Sukces!", "Pomyślnie zamknąłeś ticket!", ephemeral: true));
        var member = ctx.Guild.Members[memberId];
        if (member != null)
        {
            await channel.DeleteOverwriteAsync(member);
            await using var stream = GenerateStreamFromString(await FormatTicketContent(channel));
            var message = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed(
                    "Twój ticket został zamknięty!",
                    $"Zamknął go użytkownik {author.Mention}. W załączniku jest również jego zapis."))
                .WithFile($"{channel.Name}.txt", stream);
            try
            {
                await member.SendMessageAsync(message);
            }
            catch (Exception) {}
        }
        
        if (!channel.Name.ToLower().StartsWith("zamknięty")) await channel.ModifyAsync(action => action.Name = $"zamknięty-{ctx.Channel.Name}");
        var adminMessage = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed("Panel administracji",
            $"Ticket został zamknięty przez: {author.Mention}")).AddComponents(
            new DiscordButtonComponent(ButtonStyle.Primary, "remove", "Usuń"),
            new DiscordButtonComponent(ButtonStyle.Primary, $"reopen.{memberId}", "Otwórz ponownie"),
            new DiscordButtonComponent(ButtonStyle.Primary, "save", "Zapisz")
        );
        await channel.SendMessageAsync(adminMessage);
    }
    
    [Event(DiscordEvent.ComponentInteractionCreated)]
    public async Task TicketRemoveAsync(DiscordClient client, ComponentInteractionCreateEventArgs ctx)
    {
        if (ctx.Id != "remove") return;
        await ctx.Channel.DeleteAsync();
    }
    
    [Event(DiscordEvent.ComponentInteractionCreated)]
    public async Task TicketReopenAsync(DiscordClient client, ComponentInteractionCreateEventArgs ctx)
    {
        if (!ctx.Id.StartsWith("reopen")) return;
        var channel = ctx.Channel;
        var memberId = ulong.Parse(ctx.Id.Split(".")[1]);
        if (ctx.Guild.Members[memberId] != null)
        {
            await channel.AddOverwriteAsync(ctx.Guild.Members[memberId], Permissions.AccessChannels);
        }
    
        await ctx.Message.DeleteAsync();
        await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, _botService.CreateInteractionEmbed("Sukces!", "Pomyślnie otworzyłeś ponownie ticket!", ephemeral: true));
    }
    
    [Event(DiscordEvent.ComponentInteractionCreated)]
    public async Task TicketSaveAsync(DiscordClient client, ComponentInteractionCreateEventArgs ctx)
    {
        if (ctx.Id != "save") return;
        var channel = ctx.Channel;
        var user = ctx.User;
        await using var stream = GenerateStreamFromString(await FormatTicketContent(channel));
        var message = new DiscordMessageBuilder().AddEmbed(_botService.CreateEmbed("Zapis ticketu!",
                "Oto zapis ticketa."))
            .WithFile($"{channel.Name}.txt", stream);
        try
        {
      
            await ((DiscordMember)user).SendMessageAsync(message);
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, _botService.CreateInteractionEmbed("Sukces!", "Sprawdź wiadomość prywatną od bota.", ephemeral: true));
        }
        catch (Exception)
        {
            await ctx.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,_botService.CreateInteractionEmbed("Błąd!", "Masz zablokowane wiadomości prywatne", "red", ephemeral: true));
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