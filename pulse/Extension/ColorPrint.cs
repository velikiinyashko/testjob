namespace pulse.Extension
{
    public static class ColorPrint
    {
        public static void PrintLineColor(this string PrintText, ConsoleColor Color)
        {
            Console.ForegroundColor = Color;
            Console.WriteLine(PrintText);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
