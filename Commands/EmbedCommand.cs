using System.Threading.Tasks;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.ApplicationCommands.Attributes;
using DisCatSharp.ApplicationCommands.Context;
using DisCatSharp.Entities;
using DisCatSharp.Enums;

namespace TxtCreatorBot.Commands;

public class EmbedCommand : ApplicationCommandsModule
{
    [SlashCommand("embed", "Tworzy embeda.")]
    public async Task EmbedAsync(InteractionContext ctx, [Option("kanał", "Podaj kanał na, który ma wysłać embed.")] [ChannelTypes(ChannelType.Text)] DiscordChannel channel)
    {
        var modal = new DiscordInteractionModalBuilder()
            .WithTitle("Stwórz embed")
            .WithCustomId($"@embed.{channel.Id}")
            .AddTextComponent(new DiscordTextComponent(TextComponentStyle.Small, "title", "Tytuł"))
            .AddTextComponent(new DiscordTextComponent(TextComponentStyle.Paragraph,"description", "Treść"))
            .AddTextComponent(new DiscordTextComponent(TextComponentStyle.Small,"color", "Kolor", required: false))
            .AddTextComponent(new DiscordTextComponent(TextComponentStyle.Small,"image", "Link do obrazka", required: false))
            .AddTextComponent(new DiscordTextComponent(TextComponentStyle.Small,"thumbnail", "Link do obrazka stopki", required: false));

        await ctx.CreateModalResponseAsync(modal);
    }
}