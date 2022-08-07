namespace Katana
{
    class _Main
    {
        private static ushort numberOfTimes_MainRunFuncRan = 0;

        private static void Show()
        {
            Console.Clear();
            Console.ResetColor();
            Console.Title = "Katana ; Menu";
            Console.WriteLine($"{Settings.logo}\n\n\n{Settings.table}");
        }

        public static void Run()
        {
            if (numberOfTimes_MainRunFuncRan == 0)
            {
                Input.Token();
                Input.GuildID();
                ++numberOfTimes_MainRunFuncRan;
            }
            Show();
            ushort choice = Convert.ToUInt16(MainUtils.Readline("> "));
            if(!new ushort[8] {1, 2, 3, 4, 5, 6, 7, 8}.Contains(choice))
            {
                MainUtils.SlowWriteLine("Invalid Choice Parsed", ConsoleColor.Red);
                Thread.Sleep(2000);
                Run();
            }

            switch(choice)
            {
                case 1:
                    new Thread(() => new Ban().BanAll()).Start();
                    break;
                case 2:
                    new Thread(() => new Webhooks().Spam()).Start();
                    break;
                case 3:
                    new Thread(() => new CreateChannels().CreateAll()).Start();
                    break;
                case 4:
                    new Thread(() => new DeleteChannels().DeleteAll()).Start();
                    break;
                case 5:
                    new Thread(() => new CreateRoles().CreateAll()).Start();
                    break;
                case 6:
                    new Thread(() => new DeleteRoles().DeleteAll()).Start();
                    break;
                case 7:
                    new Thread(() => new Unban().UnbanAll()).Start();
                    break;
                case 8:
                    new Thread(() =>
                    {
                        using(HttpRequestMessage req = new(HttpMethod.Post, $"https://discord.com/api/v9/guilds/{Utils.guild}/prune")
                        {
                            Content = new Dictionary<object, object>() { { "days", 7 } }.Serialize()
                        })
                        {
                            req.Headers.Add("Authorization", Utils.auth);
                            using(HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                            {
                                res.WriteAPI($"Pruned Members", "Couldn't Prune Members");
                            }
                        }
                        Utils.InvokeExecutionFinishedEvent();
                    }).Start();
                    break;
            }
        }
    }
}