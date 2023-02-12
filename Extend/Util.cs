using System.Reflection;
using DSharpPlus;
using DSharpPlus.ButtonCommands;
using DSharpPlus.Entities;
using Emzi0767.Utilities;
using Microsoft.Extensions.Logging;

namespace TxtCreatorBot.Extend;

public static class Util
{
    public static void RegisterEvents(this DiscordClient discord, Assembly assembly, IServiceProvider services = null!)
    {
        foreach (var type in assembly.DefinedTypes.Where(t => t.IsPublic && t.IsAssignableTo(typeof(EventHandler))))
        {
            foreach (var method in type.GetMethods())
            {
                var attr = method.GetCustomAttribute<EventHandlerAttribute>();
                if (attr == null) continue;
                Task OnEvent(DiscordClient client, object e)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var parameters = new List<object>();
                            foreach (var param in method.GetParameters())
                            {
                                if (param.ParameterType == typeof(DiscordClient))
                                    parameters.Add(client);
                                else if (param.ParameterType.IsAssignableTo(typeof(AsyncEventArgs)))
                                    parameters.Add(e);
                                else
                                    parameters.Add(services.GetService(param.ParameterType)!);
                            }

                            await (Task)method.Invoke(null, parameters.ToArray())!;
                        }
                        catch (Exception ex)
                        {
                            client.Logger.LogError($"Uncaught error in event handler thread: {ex}");
                            client.Logger.LogError(ex.StackTrace);
                        }
                    });
                    return Task.CompletedTask;
                }
                
                switch (attr.Type)
                {
                    case EventType.ClientErrored:
                        discord.ClientErrored += OnEvent;
                        break;
                    case EventType.SocketErrored:
                        discord.SocketErrored += OnEvent;
                        break;
                    case EventType.SocketOpened:
                        discord.SocketOpened += OnEvent;
                        break;
                    case EventType.SocketClosed:
                        discord.SocketClosed += OnEvent;
                        break;
                    case EventType.Ready:
                        discord.Ready += OnEvent;
                        break;
                    case EventType.Resumed:
                        discord.Resumed += OnEvent;
                        break;
                    case EventType.ChannelCreated:
                        discord.ChannelCreated += OnEvent;
                        break;
                    case EventType.ChannelUpdated:
                        discord.ChannelUpdated += OnEvent;
                        break;
                    case EventType.ChannelDeleted:
                        discord.ChannelDeleted += OnEvent;
                        break;
                    case EventType.DmChannelDeleted:
                        discord.DmChannelDeleted += OnEvent;
                        break;
                    case EventType.ChannelPinsUpdated:
                        discord.ChannelPinsUpdated += OnEvent;
                        break;
                    case EventType.GuildCreated:
                        discord.GuildCreated += OnEvent;
                        break;
                    case EventType.GuildAvailable:
                        discord.GuildAvailable += OnEvent;
                        break;
                    case EventType.GuildUpdated:
                        discord.GuildUpdated += OnEvent;
                        break;
                    case EventType.GuildDeleted:
                        discord.GuildDeleted += OnEvent;
                        break;
                    case EventType.GuildUnavailable:
                        discord.GuildUnavailable += OnEvent;
                        break;
                    case EventType.MessageCreated:
                        discord.MessageCreated += OnEvent;
                        break;
                    case EventType.PresenceUpdated:
                        discord.PresenceUpdated += OnEvent;
                        break;
                    case EventType.GuildBanAdded:
                        discord.GuildBanAdded += OnEvent;
                        break;
                    case EventType.GuildBanRemoved:
                        discord.GuildBanRemoved += OnEvent;
                        break;
                    case EventType.GuildEmojisUpdated:
                        discord.GuildEmojisUpdated += OnEvent;
                        break;
                    case EventType.GuildIntegrationsUpdated:
                        discord.GuildIntegrationsUpdated += OnEvent;
                        break;
                    case EventType.GuildMemberAdded:
                        discord.GuildMemberAdded += OnEvent;
                        break;
                    case EventType.GuildMemberRemoved:
                        discord.GuildMemberRemoved += OnEvent;
                        break;
                    case EventType.GuildMemberUpdated:
                        discord.GuildMemberUpdated += OnEvent;
                        break;
                    case EventType.GuildRoleCreated:
                        discord.GuildRoleCreated += OnEvent;
                        break;
                    case EventType.GuildRoleUpdated:
                        discord.GuildRoleUpdated += OnEvent;
                        break;
                    case EventType.GuildRoleDeleted:
                        discord.GuildRoleDeleted += OnEvent;
                        break;
                    case EventType.MessageAcknowledged:
                        discord.MessageAcknowledged += OnEvent;
                        break;
                    case EventType.MessageUpdated:
                        discord.MessageUpdated += OnEvent;
                        break;
                    case EventType.MessageDeleted:
                        discord.MessageDeleted += OnEvent;
                        break;
                    case EventType.MessagesBulkDeleted:
                        discord.MessagesBulkDeleted += OnEvent;
                        break;
                    case EventType.TypingStarted:
                        discord.TypingStarted += OnEvent;
                        break;
                    case EventType.UserSettingsUpdated:
                        discord.UserSettingsUpdated += OnEvent;
                        break;
                    case EventType.UserUpdated:
                        discord.UserUpdated += OnEvent;
                        break;
                    case EventType.VoiceStateUpdated:
                        discord.VoiceStateUpdated += OnEvent;
                        break;
                    case EventType.VoiceServerUpdated:
                        discord.VoiceServerUpdated += OnEvent;
                        break;
                    case EventType.GuildMembersChunked:
                        discord.GuildMembersChunked += OnEvent;
                        break;
                    case EventType.UnknownEvent:
                        discord.UnknownEvent += OnEvent;
                        break;
                    case EventType.MessageReactionAdded:
                        discord.MessageReactionAdded += OnEvent;
                        break;
                    case EventType.MessageReactionRemoved:
                        discord.MessageReactionRemoved += OnEvent;
                        break;
                    case EventType.MessageReactionsCleared:
                        discord.MessageReactionsCleared += OnEvent;
                        break;
                    case EventType.WebhooksUpdated:
                        discord.WebhooksUpdated += OnEvent;
                        break;
                    case EventType.Heartbeated:
                        discord.Heartbeated += OnEvent;
                        break;
                    case EventType.InviteCreate:
                        discord.InviteCreated += OnEvent;
                        break;
                }
            }
        }
    }
    
/// <summary>
    /// Creates a response to this interaction.
    /// <para>You must create a response within 3 seconds of this interaction being executed; if the command has the potential to take more than 3 seconds, use <see cref="M:DSharpPlus.ModalCommands.ModalContext.DeferAsync(System.Boolean)" /> at the start, and edit the response later.</para>
    /// </summary>
    /// <param name="type">The type of the response.</param>
    /// <param name="builder">The data to be sent, if any.</param>
    public static Task CreateResponseAsync(
        this ButtonContext context,
        InteractionResponseType type,
        DiscordInteractionResponseBuilder? builder = null)
    {
        return context.Interaction.CreateResponseAsync(type, builder);
    }

    /// <inheritdoc cref="M:DSharpPlus.ModalCommands.ModalContext.CreateResponseAsync(DSharpPlus.InteractionResponseType,DSharpPlus.Entities.DiscordInteractionResponseBuilder)" />
    public static Task CreateResponseAsync(this ButtonContext context, DiscordInteractionResponseBuilder? builder) => CreateResponseAsync(context, InteractionResponseType.ChannelMessageWithSource, builder);

    /// <summary>
    /// Creates a response to this interaction.
    /// <para>You must create a response within 3 seconds of this interaction being executed; if the command has the potential to take more than 3 seconds, use <see cref="M:DSharpPlus.ModalCommands.ModalContext.DeferAsync(System.Boolean)" /> at the start, and edit the response later.</para>
    /// </summary>
    /// <param name="content">Content to send in the response.</param>
    /// <param name="embed">Embed to send in the response.</param>
    /// <param name="ephemeral">Whether the response should be ephemeral.</param>
    public static Task CreateResponseAsync(this ButtonContext context, string content, DiscordEmbed embed, bool ephemeral = false) =>
        CreateResponseAsync(context, new DiscordInteractionResponseBuilder().WithContent(content).AddEmbed(embed).AsEphemeral(ephemeral));

    /// <inheritdoc cref="M:DSharpPlus.ModalCommands.ModalContext.CreateResponseAsync(System.String,DSharpPlus.Entities.DiscordEmbed,System.Boolean)" />
    public static Task CreateResponseAsync(this ButtonContext context, string content, bool ephemeral = false) => CreateResponseAsync(context, new DiscordInteractionResponseBuilder().WithContent(content).AsEphemeral(ephemeral));

    /// <inheritdoc cref="M:DSharpPlus.ModalCommands.ModalContext.CreateResponseAsync(System.String,DSharpPlus.Entities.DiscordEmbed,System.Boolean)" />
    public static Task CreateResponseAsync(this ButtonContext context, DiscordEmbed embed, bool ephemeral = false) => CreateResponseAsync(context, new DiscordInteractionResponseBuilder().AddEmbed(embed).AsEphemeral(ephemeral));

    /// <summary>Creates a deferred response to this interaction.</summary>
    /// <param name="ephemeral">Whether the response should be ephemeral.</param>
    public static Task DeferAsync(this ButtonContext context, bool ephemeral = false) =>
        CreateResponseAsync(context, InteractionResponseType.DeferredChannelMessageWithSource, new DiscordInteractionResponseBuilder().AsEphemeral(ephemeral));

    /// <summary>Edits the interaction response.</summary>
    /// <param name="builder">The data to edit the response with.</param>
    /// <param name="attachments">Attached files to keep.</param>
    /// <returns></returns>
    public static Task<DiscordMessage> EditResponseAsync(this ButtonContext context, 
        DiscordWebhookBuilder builder,
        IEnumerable<DiscordAttachment>? attachments = null)
    {
        return context.Interaction.EditOriginalResponseAsync(builder, attachments);
    }

    /// <summary>Deletes the interaction response.</summary>
    /// <returns></returns>
    public static Task DeleteResponseAsync(this ButtonContext context) => context.Interaction.DeleteOriginalResponseAsync();

    /// <summary>Creates a follow up message to the interaction.</summary>
    /// <param name="builder">The message to be sent, in the form of a webhook.</param>
    /// <returns>The created message.</returns>
    public static Task<DiscordMessage> FollowUpAsync(this ButtonContext context, 
        DiscordFollowupMessageBuilder builder)
    {
        return context.Interaction.CreateFollowupMessageAsync(builder);
    }

    /// <summary>Edits a followup message.</summary>
    /// <param name="followupMessageId">The id of the followup message to edit.</param>
    /// <param name="builder">The webhook builder.</param>
    /// <param name="attachments">Attached files to keep.</param>
    /// <returns></returns>
    public static Task<DiscordMessage> EditFollowupAsync(
        this ButtonContext context, 
        ulong followupMessageId,
        DiscordWebhookBuilder builder,
        IEnumerable<DiscordAttachment>? attachments = null)
    {
        return context.Interaction.EditFollowupMessageAsync(followupMessageId, builder, attachments);
    }

    /// <summary>Deletes a followup message.</summary>
    /// <param name="followupMessageId">The id of the followup message to delete.</param>
    /// <returns></returns>
    public static Task DeleteFollowupAsync(this ButtonContext context, ulong followupMessageId) => context.Interaction.DeleteFollowupMessageAsync(followupMessageId);

    /// <summary>Gets the original interaction response.</summary>
    /// <returns>The original interaction response.</returns>
    public static Task<DiscordMessage> GetOriginalResponseAsync(this ButtonContext context) => context.Interaction.GetOriginalResponseAsync();
}