using System.Drawing;
using DSharpPlus.Entities;

namespace TxtCreatorBot.Services;

public class BotService
{
    public DiscordEmbedBuilder CreateEmbed(string title, string description, string color = "", string image = "", string thumbnail = "")
    {
        DiscordColor discordColor;
        if (!string.IsNullOrWhiteSpace(color))
        {
            var colorFromName = Color.FromName(color);
            discordColor = new DiscordColor(colorFromName.R, colorFromName.G, colorFromName.B);
        }
        else
        {
            discordColor = new DiscordColor(8, 159, 255);
        }
        
        var embed = new DiscordEmbedBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithImageUrl(image)
            .WithThumbnail(thumbnail)
            .WithColor(discordColor)
            .WithTimestamp(DateTimeOffset.UtcNow);

        return embed;
    }
}