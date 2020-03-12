using Discord.WebSocket;
using Guide.Configuration;
using Guide.Connection;
using Guide.Handlers;
using Guide.Json;
using Guide.Logging;
using Guide.Language;
using Lamar;
using Guide.Services;

namespace Guide
{
    public static class InversionOfControl
    {
        private static Container container;

        public static Container Container
        {
            get
            {
                return GetOrInitContainer();
            }
        }

        private static Container GetOrInitContainer()
        {
            if(container is null)
            {
                InitializeContainer();
            }

            return container;
        }

        public static void InitializeContainer()
        {
            container = new Container(c =>
            {
                c.For<IConnection>().Use<DiscordConnection>();
                c.For<IConfiguration>().Use<ConfigManager>();
                c.For<ICommandHandler>().Use<DiscordCommandHandler>();
                c.For<ILogger>().Use<ConsoleLogger>();
                c.ForSingletonOf<IJsonStorage>().UseIfNone<JsonStorage>();
                c.ForSingletonOf<ILanguage>().UseIfNone<JsonLanguage>();
                c.ForSingletonOf<WelcomeMessageService>().UseIfNone<WelcomeMessageService>();
                c.ForSingletonOf<VoiceChatService>().UseIfNone<VoiceChatService>();
                c.ForSingletonOf<DiscordSocketClient>().UseIfNone(DiscordSocketClientFactory.GetDefault());
            });
        }
    }
}
