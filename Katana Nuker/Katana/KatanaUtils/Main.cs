namespace Katana
{
    internal static class MainUtils
    {
        public static string Readline(in string txt)
        {
            Console.Write(txt);
            return Console.ReadLine()!;
        }

        public static string SlowReadLine(in string txt)
        {
            for(int i = 0; i < txt.Length; i++)
            {
                Console.Write(txt[i]);
                Thread.Sleep(5);
            }
            return Console.ReadLine()!;
        }

        public static void SlowWriteLine(in string txt, in ConsoleColor color)
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < txt.Length; i++)
            {
                Console.Write(txt[i]);
                Thread.Sleep(5);
            }
            Console.ResetColor();
        }

        public static void WriteLine(in string txt, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(txt);
            Console.ResetColor();
        }

        public static void WriteAPI(this HttpResponseMessage res, string sMsg, string usMsg, ref ushort count)
        {
            string txt = res.Content.ReadAsStringAsync().Result;
            if (Enumerable.Range(200, 100).Contains((int)res.StatusCode))
            {
                WriteLine(sMsg, ConsoleColor.Green);
                ++count;
            } 
            else if (txt.Contains("retry_after"))
            {
                var secs = JsonConvert.DeserializeObject<Dictionary<object, object>>(txt)!["retry_after"];
                WriteLine($"Rate Limited For {secs}s", ConsoleColor.Red);
            }
            else if (res.Content.ReadAsStringAsync().Result.ToLower().Contains("missing permissions"))
            {
                WriteLine("Missing Permissions...", ConsoleColor.Red);
            }
            else
            {
#if DEBUG
                WriteLine($"{usMsg} | Status Code -> {(int)res.StatusCode}", ConsoleColor.Red);
#else
                WriteLine(usMsg, ConsoleColor.Red);
#endif
            }
        }

        public static void WriteAPI(this HttpResponseMessage res, string sMsg, string usMsg)
        {
            string txt = res.Content.ReadAsStringAsync().Result;
            if (Enumerable.Range(200, 100).Contains((int)res.StatusCode))
            {
                WriteLine(sMsg, ConsoleColor.Green);
            }
            else if (txt.Contains("retry_after"))
            {
                var secs = JsonConvert.DeserializeObject<Dictionary<object, object>>(txt)!["retry_after"];
                WriteLine($"Rate Limited For {secs}s", ConsoleColor.Red);
            }
            else if (res.Content.ReadAsStringAsync().Result.ToLower().Contains("missing permissions"))
            {
                WriteLine("Missing Permissions...", ConsoleColor.Red);
            }
            else
            {
#if DEBUG
                WriteLine($"{usMsg} | Status Code -> {(int)res.StatusCode}", ConsoleColor.Red);
#else
                WriteLine(usMsg, ConsoleColor.Red);
#endif
            }
        }

        internal static async Task StartUp(this DiscordClient DSClient)
        {
            await DSClient.ConnectAsync();
            await Task.Delay(-1);
        }

        internal static void JoinAll(this List<Thread> threads)
        {
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        internal static bool isNullOrEmpty(this string str)
        {
            if (str is null || str.Trim().Length == 0)
                return true;
            foreach(char _char in str)
            {
                if ("abcdefghijklmnopqrstuvwxyz1234567890!|\\$£%^&*()_-=+;:'@]}[{/?.>,<".Contains(_char))
                    return false;
            }
            return true;
        }

        internal static StringContent Serialize(this Dictionary<object, object> dict) =>
            new(JsonConvert.SerializeObject(dict), System.Text.Encoding.UTF8, "application/json");
    }
}