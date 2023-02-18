using System.Reflection;
using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.Entities;
using DisCatSharp.Extensions;
using DisCatSharp.Extensions.Button;
using DisCatSharp.Extensions.Button.Extensions;
using DisCatSharp.Extensions.Modal;
using DisCatSharp.Extensions.Modal.Extensions;
using DisCatSharp.Interactivity;
using DisCatSharp.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TxtCreatorBOT.Database;
using TxtCreatorBot.Services;

var services = new ServiceCollection() 
    .AddSingleton(ConfigService.Create(args[1]))
    .AddSingleton<BotService>()
    .AddTransient<UserService>()
    .AddDbContext<TxtCreatorDbContext>(ServiceLifetime.Transient, ServiceLifetime.Transient)
    .BuildServiceProvider();

var client = new DiscordClient(new DiscordConfiguration()
{
    Token = args[0],
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.All,
    MobileStatus = true,
    ServiceProvider = services
});

using (var serviceScope = services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<TxtCreatorDbContext>();
    dbContext.Database.Migrate();
}

#region create all modules

var slash = client.UseApplicationCommands(new ApplicationCommandsConfiguration()
{
    ServiceProvider = services,
    EnableDefaultHelp = false
});
var buttons = client.UseButtonCommands(new ButtonCommandsConfiguration() {
    ServiceProvider = services,
    ArgumentSeparator = ".",
    Prefix = "@"
});
var modals = client.UseModalCommands(new ModalCommandsConfiguration()
{
    ServiceProvider = services,
    ArgumentSeparator = ".",
    Prefix = "@"
});
client.UseInteractivity(
    new InteractivityConfiguration()
    {
        AckPaginationButtons = true
    });

#endregion

#region register all modules

var assembly = Assembly.GetExecutingAssembly();
slash.RegisterGlobalCommands(assembly);
buttons.RegisterButtons(assembly);
modals.RegisterModals(assembly);
client.RegisterEventHandlers(assembly);

#endregion

#region error handling

slash.SlashCommandErrored += async (_, eventArgs) =>
{
    Console.WriteLine(eventArgs.Exception);
    await eventArgs.Context.CreateResponseAsync(new DiscordEmbedBuilder().WithTitle("Błąd!").WithDescription($"Coś poszło nie tak: `{eventArgs.Exception.Message}`").WithColor(new DiscordColor(255, 0, 0)), true);
};
buttons.ButtonCommandErrored += async (_, eventArgs) =>
{
    Console.WriteLine(eventArgs.Exception);
    await eventArgs.Context.CreateResponseAsync(new DiscordEmbedBuilder().WithTitle("Błąd!").WithDescription($"Coś poszło nie tak: `{eventArgs.Exception.Message}`").WithColor(new DiscordColor(255, 0, 0)), true);};
modals.ModalCommandErrored += async (_, eventArgs) =>
{
    Console.WriteLine(eventArgs.Exception);
    await eventArgs.Context.CreateResponseAsync(new DiscordEmbedBuilder().WithTitle("Błąd!").WithDescription($"Coś poszło nie tak: `{eventArgs.Exception.Message}`").WithColor(new DiscordColor(255, 0, 0)), true);};

#endregion

await client.ConnectAsync(new DiscordActivity("rozkazów.", ActivityType.ListeningTo));
await Task.Delay(-1);