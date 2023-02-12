using System.Reflection;
using DSharpPlus;
using DSharpPlus.ButtonCommands;
using DSharpPlus.ButtonCommands.Extensions;
using DSharpPlus.Entities;
using DSharpPlus.ModalCommands;
using DSharpPlus.ModalCommands.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using TxtCreatorBot.Extend;
using TxtCreatorBot.Services;

var discord = new DiscordClient(new DiscordConfiguration()
{
    Token = args[0],
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.All
});

var services = new ServiceCollection() 
    .AddSingleton(ConfigService.Create(args[1]))
    .AddSingleton<BotService>()
    .BuildServiceProvider();

#region commands

var slash = discord.UseSlashCommands(new SlashCommandsConfiguration()
{
    Services = services
});
var buttons = discord.UseButtonCommands(new ButtonCommandsConfiguration() {
    ArgumentSeparator = ".",
    Prefix = "@",
    Services = services
});
var modals = discord.UseModalCommands(new ModalCommandsConfiguration()
{
    Seperator = ".",
    Prefix = "@",
    Services = services
});

#endregion

#region register commands

var assembly = Assembly.GetExecutingAssembly();
slash.RegisterCommands(assembly);
buttons.RegisterButtons(assembly);
modals.RegisterModals(assembly);
discord.RegisterEvents(assembly, services);

#endregion

#region error handle

slash.SlashCommandErrored += async (_, eventArgs) =>
{
    Console.WriteLine(eventArgs.Exception);
    await eventArgs.Context.CreateResponseAsync(services.GetRequiredService<BotService>().CreateEmbed("Błąd!", $"Coś poszło nie tak: `{eventArgs.Exception.Message}`", "red"), true);
};
buttons.ButtonCommandErrored += async (_, eventArgs) =>
{
    Console.WriteLine(eventArgs.Exception);
    await eventArgs.Context.CreateResponseAsync(services.GetRequiredService<BotService>().CreateEmbed("Błąd!", $"Coś poszło nie tak: `{eventArgs.Exception.Message}`", "red"), true);
};
modals.ModalCommandErrored += async (_, eventArgs) =>
{
    Console.WriteLine(eventArgs.Exception);
    await eventArgs.Context.CreateResponseAsync(services.GetRequiredService<BotService>().CreateEmbed("Błąd!", $"Coś poszło nie tak: `{eventArgs.Exception.Message}`", "red"), true);
};

#endregion

await discord.ConnectAsync(new DiscordActivity("rozkazów.", ActivityType.ListeningTo));
await Task.Delay(-1);