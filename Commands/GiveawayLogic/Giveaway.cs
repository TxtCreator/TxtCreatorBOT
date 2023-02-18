using DisCatSharp;
using DisCatSharp.Entities;
using TxtCreatorBot.Services;
using Timer = System.Timers.Timer;

namespace TxtCreatorBOT.Commands.GiveawayLogic;

public class Giveaway
{
    private string Prize { get; }
    private int Minutes { get; set; }
    private int Winners { get; }
    private DateTimeOffset GiveawayEnd { get; }
    private DiscordChannel Channel { get; }
    private DiscordMessage Message { get; set; }
    private readonly BotService _botService;

  
    public Giveaway(string prize, int minutes, int winners, DiscordChannel channel, BotService botService)
    {
        Prize = prize;
        Minutes = minutes;
        Winners = winners;
        GiveawayEnd = DateTimeOffset.UtcNow.AddMinutes(Minutes);
        Channel = channel;
        _botService = botService;
    }

    public async Task SendGiveaway(DiscordEmoji emoji)
    {
        Message = await Channel.SendMessageAsync(_botService.CreateEmbed("Nowy konkurs!", "Aby wziąć udział kliknij emotkę poniżej!").WithImageUrl("https://media.discordapp.net/attachments/1035176861870858280/1076506445308243988/konkurs.png")
            .AddFields(
                new DiscordEmbedField("Nagroda", $"{Prize.Bold()}"),
                new DiscordEmbedField("Ilość możliwych zwycięzców", $"{Winners.ToString().Bold()}"),
                new DiscordEmbedField("Data zakończenia", $"{GiveawayEnd.Timestamp()}")
                ));
        await Message.CreateReactionAsync(emoji);
        var timer = new Timer(600);
        async Task OnTimerOnElapsedAsync()
        {
            Minutes--;
            if (Minutes == 0)
            {
                var winners = await DrawWinners(emoji, Message, Winners);
                await Message.ModifyAsync(action => action.Embed = _botService.CreateEmbed(
                    "Konkurs zakończony!",
                    "Oto informacje o konkursie"
                ).WithImageUrl("https://media.discordapp.net/attachments/1035176861870858280/1076506445308243988/konkurs.png").AddFields(
                    new DiscordEmbedField("Nagroda", $"{Prize.Bold()}"),
                    new DiscordEmbedField("Ilość możliwych zwycięzców", $"{Winners.ToString().Bold()}"),
                    new DiscordEmbedField("Data zakończenia", $"{GiveawayEnd.Timestamp()}"),
                    new DiscordEmbedField("Osoby wygrane", $"{winners}")
                ));
                await SendWinners(Channel, winners, Message.Id, _botService);
                timer.Stop();
            }
        }
        timer.Elapsed += (_, _) => OnTimerOnElapsedAsync().GetAwaiter().GetResult();
        timer.Start();
    }

    public static async Task<string> DrawWinners(DiscordEmoji emoji, DiscordMessage message, int winnersAmount)
    {
        var users = (await message.GetReactionsAsync(emoji)).Where(
            user => !user.IsBot).ToList();
        var random = new Random();
        var winners = new List<string>();
        for (var i = 0; i < winnersAmount; i++)
        {
            if (users.Count == 0) continue;
            var winner = users[random.Next(users.Count)];
            users.Remove(winner);
            winners.Add(winner.Mention);
        }

        var mentionedWinners = winners.Count == 0
            ? "brak zwycięzców, nikt nie brał udziału".Bold()
            : string.Join("\n", winners.ToArray());
        return mentionedWinners;
    }

    public static async Task SendWinners(DiscordChannel channel, string winners, ulong id, BotService botService)
    {
        var content = winners == "brak zwycięzców, nikt nie brał udziału".Bold() ? "" : winners;
                
        var message = new DiscordMessageBuilder().WithContent(content).AddEmbed(botService.CreateEmbed("Koniec konkursu!", $"O to wygrani: {winners}"))
            .WithReply(id).WithAllowedMention(UserMention.All);
        await channel.SendMessageAsync(message);
    }
}