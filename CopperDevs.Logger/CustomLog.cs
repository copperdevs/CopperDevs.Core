namespace CopperDevs.Logger
{
    public readonly struct CustomLog
    {
        public readonly AnsiColors.Names MainColor;
        public readonly AnsiColors.Names BackgroundColor;
        public readonly string prefix;

        public CustomLog(AnsiColors.Names mainColor, AnsiColors.Names backgroundColor, string prefix)
        {
            MainColor = mainColor;
            BackgroundColor = backgroundColor;
            this.prefix = prefix;
        }
    }
}
