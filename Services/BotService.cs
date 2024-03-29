﻿using System.Drawing;
using DisCatSharp.Entities;

namespace TxtCreatorBot.Services;

public class BotService
{
    public DiscordEmbedBuilder CreateEmbed(string title, string description, string color = "")
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
            .WithColor(discordColor)
            .WithTimestamp(DateTimeOffset.UtcNow);

        return embed;
    }
}