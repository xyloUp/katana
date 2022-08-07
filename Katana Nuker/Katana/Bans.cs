namespace Katana
{
    internal sealed class Ban
    {
        private readonly Random r = new();

        private void BanOne(in string id, ref ushort count)
        {
            using (HttpRequestMessage req = new(HttpMethod.Put, $"https://discord.com/api/v{r.Next(7, 10)}/guilds/{Utils.guild}/bans/{id}?reason={Settings.reason}"))
            {
                req.Headers.Add("Authorization", Utils.auth);
                using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                {
                    res.WriteAPI($"Banned {id}", $"Unable To Ban {id}", ref count);
                }
            }
        }

        public void BanAll()
        {
            Console.Title = "Katana ; Banning Members";
            List<Thread> threads = new();
            Console.Clear();
            ushort count = 0;
            bool banIds = MainUtils.SlowReadLine("Ban Ids[y/N]: ").ToLower() != "n";
            Console.Clear();
            Thread.Sleep(750);
            if (banIds is false)
            {
                foreach (string id in new Scrape()._Scrape("members"))
                {
                    Thread t = new Thread(() => BanOne(id, ref count));
                    t.Start();
                    threads.Add(t);
                }
            }
            else
            {
                foreach (string id in new Scrape()._Scrape("ids"))
                {
                    Thread t = new Thread(() => BanOne(id, ref count));
                    t.Start();
                    threads.Add(t);
                }
            }
            threads.JoinAll(); // ext method
            Console.WriteLine($"Banned {count} Members");
            Utils.InvokeExecutionFinishedEvent();
        }
    }

    internal sealed class Unban
    {
        private readonly Random r = new();

        private void UnbanOne(in string id, ref ushort count)
        {
            using (HttpRequestMessage req = new(HttpMethod.Delete, $"https://discord.com/api/v{r.Next(7, 10)}/guilds/{Utils.guild}/bans/{id}?reason={Settings.reason}"))
            {
                req.Headers.Add("Authorization", Utils.auth);
                using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                {
                    res.WriteAPI($"Unbanned {id}", $"Unable To Unban {id}");
                }
            }
        }

        public void UnbanAll()
        {
            Console.Title = "Katana ; Unbanning Members";
            List<Thread> threads = new();
            Console.Clear();
            ushort count = 0;
            bool unbanOne = MainUtils.SlowReadLine("Unban Everyone[y?N] -> ").ToLower() != "y";
            Console.Clear();
            Thread.Sleep(750);
            if (unbanOne is false)
            {
                foreach (string id in new Scrape()._Scrape("bans"))
                {
                    Thread t = new Thread(() => UnbanOne(id, ref count));
                    t.Start();
                    threads.Add(t);
                }
                foreach (Thread thread in threads) // ext method didnt work ??
                {
                    thread.Join();
                }
                Console.WriteLine($"Unbanned {count} Members");
            }
            else
            {
                string id = MainUtils.SlowReadLine("ID To Unban");
                new Thread(() => UnbanOne(id, ref count)).Start();
            }
            Utils.InvokeExecutionFinishedEvent();
        }
    }
}