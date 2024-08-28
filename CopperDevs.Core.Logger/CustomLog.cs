namespace CopperDevs.Core.Logger
{
    public struct CustomLog
    {
        public AnsiColors.Names MainColor;
        public AnsiColors.Names BackgroundColor;
        public string prefix;

        public CustomLog(AnsiColors.Names mainColor, AnsiColors.Names backgroundColor, string prefix)
        {
            MainColor = mainColor;
            BackgroundColor = backgroundColor;
            this.prefix = prefix;
        }
    }
}