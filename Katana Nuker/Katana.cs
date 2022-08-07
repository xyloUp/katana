namespace Katana
{
    internal static class Katana
    {
        internal static void Main(params string[] args)
        {
            try
            {
                Utils.OnExecutionFinished += _Main.Run;
                _Main.Run();
            } catch { Main(); }
        }
    }
}
