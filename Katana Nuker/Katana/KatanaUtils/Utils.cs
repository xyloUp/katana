namespace Katana
{
    internal delegate void OnFinish();

    internal ref struct Utils
    {
        public static readonly HttpClient client = new();
        public static DiscordClient? DSClient = null!;
        public static string? auth { get; internal set; }
        public static string? guild { get; internal set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static event OnFinish OnExecutionFinished;

        public static void InvokeExecutionFinishedEvent()
        {
            Thread.Sleep(1750);
            OnExecutionFinished?.Invoke();
        }
    }
}