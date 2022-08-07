namespace Katana
{
    /// <summary>
    /// leave this as it is dont change lmao (unless adding ids to wlUsers, thats fine)
    /// </summary>
    internal static class Settings 
    {
        public static List<string> wlUsers { get; private set; } = new()
        {
            "920051517514453022"
        }; // users that will not be affected by katana (unless its pruning)
        public const string logo = @"
         ____  __.          __                           
        |    |/ _|_____   _/  |_ _____     ____  _____   
        |      <  \__  \  \   __\\__  \   /    \ \__  \  
        |    |  \  / __ \_ |  |   / __ \_|   |  \ / __ \_
        |____|__ \(____  / |__|  (____  /|___|  /(____  /
                \/     \/             \/      \/      \/
";
        public const string table = @"

        +----------------+-----------------+
        | Katana Options |   Katana Nuker  |
        +----------------+-----------------+
        |              1 | Ban             |
        |              2 | Spam Webhooks   |
        |              3 | Spam Channels   |
        |              4 | Delete Channels |
        |              5 | Spam Roles      |
        |              6 | Delete Roles    |
        |              7 | Unban           |
        |              8 | Prune Members   |
        +----------------+-----------------+
";
        public const bool appearOfflineWhileConnectToGateway = true; // makes bot appear offline 
        public const bool establishGatewayConnection = true; // if the token provided is a bot establish a connection to a discord ws (benefits: better scraping ; negatives: might be slower)
        public const string reason = "Katana W | Xylo W"; // the reason showing up in audit logs
        public const string channelNames = "XyloW"; // the name of the channels if the name the user provides is invalid
        public const string roleNames = "https://github.com/xyloUp/katana"; // the name of the roles if the name the user provides is invalid
        public const string webhookNames = "https://github.com/xyloUp/katana"; // the name of the webhooks if the name the user provides is invalid
        public const string msgContent = "Katana Up | xylo#6666 ~ https://github.com/xyloUp/katana"; // msg sent on webhook if invalid msg provided by user
        public const bool createAndSpam = true; // if true will create webhooks and then spam if false will span pre-existing webhooks
        public const ushort msgAmountPerHook = 1000; // amount of msgs sent per hook (note  dint put below 1k cus of rl)
    }
}