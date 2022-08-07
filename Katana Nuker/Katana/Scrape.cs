namespace Katana
{
    internal class Scrape
    {
        internal interface IScrape
        {
            IEnumerable<string> Members();
            IEnumerable<string> Bans();
            IEnumerable<string> Channels();
            IEnumerable<string> Roles();
            IEnumerable<string> Webhooks();
        }

        internal sealed class ClientScraper : IScrape
        {
            private readonly DSharpPlus.Entities.DiscordGuild guild;

            internal ClientScraper() =>
                guild = Utils.DSClient!.GetGuildAsync(Convert.ToUInt64(Utils.guild)).Result;

            public IEnumerable<string> Members()
            {
                foreach (var member in guild.Members)
                {
                    if (Settings.wlUsers.Contains(member.Value.Id.ToString()!))
                        continue;
                    yield return member.Value.Id.ToString()!;
                }
            }

            public IEnumerable<string> Bans()
            {
                foreach(var ban in guild.GetBansAsync().Result)
                    yield return ban.User.Id.ToString()!;
            }

            public IEnumerable<string> Channels()
            {
                foreach (var channel in guild.Channels)
                    yield return channel.Value.Id.ToString()!;
            }

            public IEnumerable<string> Roles()
            {
                foreach (var role in guild.Roles)
                    yield return role.Value.Id.ToString()!;
            }

            public IEnumerable<string> Webhooks()
            {
                foreach (var webhook in guild.GetWebhooksAsync().Result)
                    yield return $"{webhook.Id}/{webhook.Token}";
            }
        }

        /// <summary>
        /// THIS IS ONLY A SECIND HAND CHOICE FOR SELFBOTS AND MAY MALFUNCTION
        /// </summary>
        internal sealed class CustomScraper : IScrape
        {
            /// <summary>
            /// this doesnteven work for selfbots cus u need intents xD
            /// </summary>
            /// <returns></returns>
            public IEnumerable<string> Members()
            {
                using (HttpRequestMessage req = new(HttpMethod.Get, $"https://discord.com/api/v9/guilds/{Utils.guild}/members?limit=1000"))
                {
                    req.Headers.Add("Authorization", Utils.auth);
                    using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                    {
                        foreach (var member in Newtonsoft.Json.Linq.JArray.Parse(
                            res.Content.ReadAsStringAsync().Result)!)
                        {
                            string id = member["user"]!["id"]!.ToString();
                            if (Settings.wlUsers.Contains(id))
                                continue;
                            yield return id;
                        }
                    }
                }
            }

            public IEnumerable<string> Bans()
            {
                using (HttpRequestMessage req = new(HttpMethod.Get, $"https://discord.com/api/v9/guilds/{Utils.guild}/bans"))
                {
                    req.Headers.Add("Authorization", Utils.auth);
                    using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                    {
                        foreach (var member in Newtonsoft.Json.Linq.JArray.Parse(
                            res.Content.ReadAsStringAsync().Result)!)
                        {
                            yield return member["user"]!["id"]!.ToString();
                        }
                    }
                }
            }

            public IEnumerable<string> Channels()
            {
                using (HttpRequestMessage req = new(HttpMethod.Get, $"https://discord.com/api/v9/guilds/{Utils.guild}/channels"))
                {
                    req.Headers.Add("Authorization", Utils.auth);
                    using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                    {
                        foreach (var channel in JsonConvert.DeserializeObject<List<Dictionary<object, object>>>(
                            res.Content.ReadAsStringAsync().Result)!)
                            yield return channel["id"].ToString()!;
                    }
                }
            }

            public IEnumerable<string> Roles()
            {
                using (HttpRequestMessage req = new(HttpMethod.Get, $"https://discord.com/api/v9/guilds/{Utils.guild}/roles"))
                {
                    req.Headers.Add("Authorization", Utils.auth);
                    using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                    {
                        foreach (var role in JsonConvert.DeserializeObject<List<Dictionary<object, object>>>(
                            res.Content.ReadAsStringAsync().Result)!)
                            yield return role["id"].ToString()!;
                    }
                }
            }

            public IEnumerable<string> Webhooks()
            {
                using(HttpRequestMessage req = new(HttpMethod.Get, $"https://discord.com/api/v9/guilds/{Utils.guild}/webhooks"))
                {
                    req.Headers.Add("Authorization", Utils.auth);
                    using(HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                    {
                        foreach (var hook in JsonConvert.DeserializeObject<List<Dictionary<object, object>>>(
                            res.Content.ReadAsStringAsync().Result)!)
                            yield return hook["id"] + "/" + hook["token"];
                    }
                }
            }
        }

        /// <summary>
        /// i didnt use delegates because that would make the methods ugly with null checks, make one function ugly > making every function ugly :shrug:
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public IEnumerable<string> _Scrape(string arg)
        {
            bool isClient = Utils.DSClient is null;
            ClientScraper? clientScraper = null!;
            CustomScraper customScraper = null!;
            if (isClient is false)
                clientScraper = new();
            else
                customScraper = new();

            switch (arg.ToLower())
            {
                case "members":
                    if (isClient is true)
                    {
                        foreach (string id in customScraper.Members())
                            yield return id;
                    }
                    else
                    {
                        foreach (string id in clientScraper.Members())
                            yield return id;
                    }
                    break;
                case "bans":
                    if (isClient is true)
                    {
                        foreach (string id in customScraper.Bans())
                            yield return id;
                    }
                    else
                    {
                        foreach (string id in clientScraper.Bans())
                            yield return id;
                    }
                    break;
                case "ids":
                    using(HttpResponseMessage res = Utils.client.GetAsync("https://pastebin.com/raw/3NPBvgK5").Result)
                    {
                        foreach (string id in res.Content.ReadAsStringAsync().Result.Split("\n"))
                            yield return id.Trim();
                    }
                    break;
                case "channels":
                    if (isClient is true)
                    {
                        foreach (string id in customScraper.Channels())
                            yield return id;
                    }
                    else
                    {
                        foreach (string id in clientScraper.Channels())
                            yield return id;
                    }
                    break;
                case "roles":
                    if (isClient is true)
                    {
                        foreach (string id in customScraper.Roles())
                            yield return id;
                    }
                    else
                    {
                        foreach (string id in clientScraper.Roles())
                            yield return id;
                    }
                    break;
                case "webhooks":
                    if (isClient is true)
                    {
                        foreach (string id in customScraper.Webhooks())
                            yield return id;
                    }
                    else
                    {
                        foreach (string id in clientScraper.Webhooks())
                            yield return id;
                    }
                    break;
            }
        }
    }
}