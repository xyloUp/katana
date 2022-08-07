namespace Katana
{
    internal sealed class CreateChannels
    {
        private readonly Random r = new();

        private void CreateOne(in string name, ref ushort count)
        {
            Dictionary<object, object> JSON = new()
            {
                { "name", name }
            };
            using (HttpRequestMessage req = new(HttpMethod.Post, $"https://discord.com/api/v{r.Next(7, 10)}/guilds/{Utils.guild}/channels?reason={Settings.reason}")
            {
                Content = JSON.Serialize() // ext merthod
            })
            {
                req.Headers.Add("Authorization", Utils.auth);
                using(HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                {
                    res.WriteAPI($"Created Channel {name}", $"Couldn't Create Channel {name}", ref count);
                }
            }
        }

        public void CreateAll()
        {
            Console.Title = "Katana ; Creating Channels";
            List<Thread> threads = new();
            Console.Clear();
            ushort count = 0;
            string channelNames = MainUtils.SlowReadLine("Channel Names -> ");
            channelNames = channelNames.isNullOrEmpty() ? Settings.channelNames : channelNames;
            Console.Clear();
            ushort amount = Convert.ToUInt16(MainUtils.SlowReadLine("Amount Of Channles To Create -> "));
            Console.Clear();
            Thread.Sleep(750);
            for (int j = 0; j < amount; ++j)
            {
                Thread t = new(() => CreateOne(channelNames, ref count));
                t.Start();
                threads.Add(t);
            }
            threads.JoinAll(); // ext method
            Console.WriteLine($"Created {count} Channels With The Name {channelNames}");
            Utils.InvokeExecutionFinishedEvent();
        }
    }

    internal sealed class DeleteChannels
    {
        private readonly Random r = new();

        private void DeleteOne(in string id, ref ushort count)
        {
            using (HttpRequestMessage req = new(HttpMethod.Delete, $"https://discord.com/api/v{r.Next(7, 10)}/channels/{id}?reason={Settings.reason}"))
            {
                req.Headers.Add("Authorization", Utils.auth);
                using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                {
                    res.WriteAPI($"Deleted Channel {id}", $"Couldn't Delete Channel {id}", ref count);
                }
            }
        }

        public void DeleteAll()
        {
            Console.Title = "Katana ; Deleting Channels";
            List<Thread> threads = new();
            ushort count = 0;
            Console.Clear();
            Thread.Sleep(750); // intentionally placed sometimes it freezes at the start so dont remove
            foreach(string channelId in new Scrape()._Scrape("channels"))
            {
                Thread t = new(() => DeleteOne(channelId, ref count));
                t.Start();
                threads.Add(t);
            }
            threads.JoinAll(); // ext method
            Console.WriteLine($"Deleted {count} Channels");
            Utils.InvokeExecutionFinishedEvent();
        }
    }
}