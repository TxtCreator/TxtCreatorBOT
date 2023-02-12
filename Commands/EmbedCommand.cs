using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace TxtCreatorBot.Commands;

public class EmbedCommand : ApplicationCommandModule
{
    [SlashCommand("embed", "Tworzy embeda.")]
    public async Task EmbedCommandAsync(InteractionContext ctx, [Option("kanał", "Podaj kanał na, który ma wysłać embed.")] [ChannelTypes(ChannelType.Text)] DiscordChannel channel)
    {
        var modal = new DiscordInteractionResponseBuilder()
            .WithTitle("Stwórz embed")
            .WithCustomId($"@embed.{channel.Id}")
            .AddComponents(new TextInputComponent("Tytuł", "title"))
            .AddComponents(new TextInputComponent("Treść", "description",
                style: TextInputStyle.Paragraph))
            .AddComponents(new TextInputComponent("Kolor", "color", required: false))
            .AddComponents(new TextInputComponent("Link do obrazka", "image", required: false))
            .AddComponents(new TextInputComponent("Link do obrazka stopki", "thumbnail", required: false));

        await ctx.CreateResponseAsync(InteractionResponseType.Modal, modal);
    }
}