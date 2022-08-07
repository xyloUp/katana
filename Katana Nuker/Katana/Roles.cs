namespace Katana
{
    internal sealed class CreateRoles
    {
        private readonly Random r = new();

        private void CreateOne(in string name, ref ushort count)
        {
            Dictionary<object, object> JSON = new()
            {
                { "name", name }, 
                { "color", r.Next(1000000,9999999) }
            };
            using (HttpRequestMessage req = new(HttpMethod.Post, $"https://discord.com/api/v{r.Next(7, 10)}/guilds/{Utils.guild}/roles?reason={Settings.reason}")
            {
                Content = JSON.Serialize() // ext merthod
            })
            {
                req.Headers.Add("Authorization", Utils.auth);
                using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                {
                    res.WriteAPI($"Created Role {name}", $"Couldn't Create Role {name}", ref count);
                }
            }
        }

        public void CreateAll()
        {
            Console.Title = "Katana ; Creating Roles";
            List<Thread> threads = new();
            Console.Clear();
            ushort count = 0;
            string roleNames = MainUtils.SlowReadLine("Role Names -> ");
            roleNames = roleNames.isNullOrEmpty() ? Settings.roleNames : roleNames;
            Console.Clear();
            ushort amount = Convert.ToUInt16(MainUtils.SlowReadLine("Amount Of Roles Create -> "));
            Console.Clear();
            Thread.Sleep(750);
            for (int j = 0; j < amount; ++j)
            {
                Thread t = new(() => CreateOne(roleNames, ref count));
                t.Start();
                threads.Add(t);
            }
            threads.JoinAll(); // ext method
            Console.WriteLine($"Created {count} Roles With The Name {roleNames}");
            Utils.InvokeExecutionFinishedEvent();
        }
    }

    internal sealed class DeleteRoles
    {
        private readonly Random r = new();

        private void DeleteOne(in string id, ref ushort count)
        {
            using (HttpRequestMessage req = new(HttpMethod.Delete, $"https://discord.com/api/v{r.Next(7, 10)}/guilds/{Utils.guild}/roles/{id}?reason={Settings.reason}"))
            {
                req.Headers.Add("Authorization", Utils.auth);
                using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                {
                    res.WriteAPI($"Deleted Role {id}", $"Couldn't Delete Role {id}", ref count);
                }
            }
        }

        public void DeleteAll()
        {
            Console.Title = "Katana ; Deleting Roles";
            List<Thread> threads = new();
            ushort count = 0;
            Console.Clear();
            Thread.Sleep(750); // intentionally placed sometimes it freezes at the start so dont remove
            foreach (string roleId in new Scrape()._Scrape("roles"))
            {
                Thread t = new(() => DeleteOne(roleId, ref count));
                t.Start();
                threads.Add(t);
            }
            threads.JoinAll(); // ext method
            Console.WriteLine($"Deleted {count} Roles");
            Utils.InvokeExecutionFinishedEvent();
        }
    }
}