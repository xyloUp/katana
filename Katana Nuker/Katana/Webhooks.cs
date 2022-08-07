namespace Katana
{
    internal sealed class Webhooks
    {
        private void CreateOne(in string name, in string id)
        {
            using(HttpRequestMessage req = new(HttpMethod.Post, $"https://discord.com/api/v9/channels/{id}/webhooks?reason={Settings.reason}")
            {
                Content = new Dictionary<object, object>() { { "name", name } }.Serialize()
            })
            {
                req.Headers.Add("Authorization", Utils.auth);
                using(HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                {
                    res.WriteAPI($"Created Webhook {name}", $"Couldn't Create Webhook {name}");
                }
            }
        } 

        private void CreateAll()
        {
            Console.Title = "Katana ; Creating Webhooks";
            List<Thread> threads = new();
            Console.Clear();
            string webhookNames = MainUtils.SlowReadLine("Webhook Names -> ");
            webhookNames = webhookNames.isNullOrEmpty() ? Settings.webhookNames : webhookNames;
            Thread.Sleep(750);
            foreach(string id in new Scrape()._Scrape("channels"))
            {
                Thread t = new(() => CreateOne(webhookNames, id));
                t.Start();
                threads.Add(t);
            }
            threads.JoinAll(); // ext method
        }

        private void SendOne(in string hook, in string msg)
        {
            Dictionary<object, object> JSON = new() { { "content", msg }, { "nonce", null! }, { "tts", false } };
            for(ushort i = 0; i < Settings.msgAmountPerHook; ++i)
            {
                using (HttpRequestMessage req = new(HttpMethod.Post, hook) { Content = JSON.Serialize() })
                {
                    using (HttpResponseMessage res = Utils.client.SendAsync(req).Result)
                    {
                        res.WriteAPI($"Sent Message {msg}", $"Couldn't Send Message {msg}");
                    }
                }
            }
        }

        private void SendAll()
        {
            Console.Title = "Katana ; Spamming Webhooks";
            List<Thread> threads = new();
            Console.Clear();
            string msgCnt = MainUtils.SlowReadLine("Message Content -> ");
            msgCnt = msgCnt.isNullOrEmpty() ? Settings.msgContent : msgCnt;
            foreach (string hook in new Scrape()._Scrape("webhooks"))
            {
                Thread t = new(() => SendOne($"https://discord.com/api/webhooks/{hook}", msgCnt));
                t.Start();
                threads.Add(t);
            }
            threads.JoinAll(); // ext method
        }

        public void Spam()
        {
            if (Settings.createAndSpam)
                CreateAll();
            SendAll();
            Utils.InvokeExecutionFinishedEvent();
        }
    }
}