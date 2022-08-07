namespace Katana
{
    internal interface IKatanaChoice
    {
        protected abstract void One(in string name = null!);
        internal void All();
    }

    internal sealed class Input
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        internal static void Token()
        {
        Token:
            Console.Clear();
            if (!Console.Title.ToLower().Contains("token"))
                Console.Title = "Katana ; Login ~ Token";
            string token = MainUtils.SlowReadLine("Token -> ");
            string auth = HeadersAndGuild.Auth(token);
            if (auth is not null)
            {
                Utils.auth = auth;
                if (auth.Contains("Bot"))
                {
                    if (Settings.establishGatewayConnection)
                    {
                        new DSharpClient(ref Utils.DSClient!, token);
                        Utils.DSClient?.StartUp();
                    }
                }
            }
            else
            {
                MainUtils.SlowWriteLine("Invalid Token Parsed", ConsoleColor.Red);
                Thread.Sleep(2000);
                goto Token;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        internal static void GuildID()
        {
        GuildID:
            Console.Clear();
            if (!Console.Title.ToLower().Contains("guild"))
                Console.Title = "Katana ; Login ~ Guild";
            string guild = HeadersAndGuild.Guild(MainUtils.SlowReadLine("Guild ID -> "));
            if (guild is not null)
                Utils.guild = guild;
            else
            {
                MainUtils.SlowWriteLine("Invalid Guild Parsed", ConsoleColor.Red);
                Thread.Sleep(2000);
                goto GuildID;
            }
        }
    }

    internal static class HeadersAndGuild
    {
        internal static string Auth(in string token)
        {
            {
                using (HttpRequestMessage req = new(HttpMethod.Get, "https://discord.com/api/v9/users/@me"))
                {
                    req.Headers.Add("Authorization", token);
                    using(HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                    {
                        if (Enumerable.Range(200, 100).Contains((int)res.StatusCode))
                            return token;
                    }
                }
            }
            {
                using (HttpRequestMessage req = new(HttpMethod.Get, "https://discord.com/api/v9/users/@me"))
                {
                    req.Headers.Add("Authorization", $"Bot {token}");
                    using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                    {
                        if (Enumerable.Range(200, 100).Contains((int)res.StatusCode))
                            return $"Bot {token}";
                    }
                }
            }
            return null!;
        }

        internal static string Guild(in string guild)
        {
            using(HttpRequestMessage req = new(HttpMethod.Get, $"https://discord.com/api/v9/guilds/{guild}"))
            {
                req.Headers.Add("Authorization", Utils.auth);
                using(HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                {
                    if (Enumerable.Range(200, 100).Contains((int)res.StatusCode))
                        return guild;
                }
            }
            return null!;
        }
    }
}