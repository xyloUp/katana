namespace Katana
{
    internal static class Katana
    {
        /*
         * if ur seeing this u decompiled katana (congrats ig even tho it was easy af)
         * for whatever reason u decompiled katana (to show ur freinds ur a pro hacker man, to see that its not a token grabber) wtv
         * i just wanna say from the bottom of my heart, your a fat bitch xD
         * btw katana was made solely by xylo#6666 in like 3 hours and i did it cus why not xD xD
         */
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