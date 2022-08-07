namespace Katana
{
    internal class DSharpClient
    {
        /// <summary>
        /// Setting Up Optional DSharpClient Config
        /// </summary>
        internal DSharpClient(ref DiscordClient DSClient, in string token)
        {
            DSClient = new(new()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.None
            });
        }
    }
}