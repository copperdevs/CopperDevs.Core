namespace CopperDevs.Core.Data;

public static class ConsoleColors
{
    private static string ForeColor(int code) => $"\u001B[38;5;{code}m";
    private static string BackColor(int code) => $"\u001B[48;5;{code}m";

    public static string Reset => "\u001B[0m";
    public static string Bold => "\u001B[1m";

    public static string Black => "\u001B[30m";
    public static string BlackBackground => "\u001B[40m";

    public static string BrightBlack => "\u001B[90m";
    public static string BrightBlackBackground => "\u001B[100m";

    public static string Red => "\u001B[31m";
    public static string RedBackground => "\u001B[41m";

    public static string BrightRed => "\u001B[91m";
    public static string BrightRedBackground => "\u001B[101m";

    public static string Green => "\u001B[32m";
    public static string GreenBackground => "\u001B[42m";

    public static string BrightGreen => "\u001B[92m";
    public static string BrightGreenBackground => "\u001B[102m";

    public static string Yellow => "\u001B[33m";
    public static string YellowBackground => "\u001B[43m";
    public static string BrightYellow => "\u001B[93m";
    public static string BrightYellowBackground => "\u001B[103m";

    public static string Blue => "\u001B[34m";
    public static string BlueBackground => "\u001B[44m";

    public static string BrightBlue => "\u001B[94m";
    public static string BrightBlueBackground => "\u001B[104m";

    public static string Purple => "\u001B[35m";
    public static string PurpleBackground => "\u001B[45m";

    public static string BrightPurple => "\u001B[95m";
    public static string BrightPurpleBackground => "\u001B[105m";
    public static string Cyan => "\u001B[36m";
    public static string CyanBackground => "\u001B[46m";

    public static string BrightCyan => "\u001B[96m";
    public static string BrightCyanBackground => "\u001B[106m";

    public static string White => "\u001B[37m";
    public static string WhiteBackground => "\u001B[47m";

    public static string BrightWhite => "\u001B[97m";
    public static string BrightWhiteBackground => "\u001B[107m";

    public static string Gray => ForeColor(244);
    public static string GrayBackground => BackColor(244);

    public static string Orange => ForeColor(202);
    public static string OrangeBackground => BackColor(202);

    public static string Pink => ForeColor(200);
    public static string PinkBackground => BackColor(200);

    public static string CutePink => ForeColor(205);
    public static string CutePinkBackground => BackColor(205);

    public static string Aqua => ForeColor(45);
    public static string AquaBackground => BackColor(45);

    public static string Gold => ForeColor(220);
    public static string GoldBackground => BackColor(220);

    public static string LightGreen => ForeColor(82);
    public static string LightGreenBackground => BackColor(82);

    public static string LightBlue => ForeColor(39);
    public static string LightBlueBackground => BackColor(39);

    public static string Magenta => ForeColor(13);
    public static string MagentaBackground => BackColor(13);

    public static string LightCyan => ForeColor(51);
    public static string LightCyanBackground => BackColor(51);

    public static string LightGray => ForeColor(247);
    public static string LightGrayBackground => BackColor(247);

    public static string DarkRed => ForeColor(88);
    public static string DarkRedBackground => BackColor(88);

    public static string DarkGreen => ForeColor(22);
    public static string DarkGreenBackground => BackColor(22);

    public static string DarkBlue => ForeColor(19);
    public static string DarkBlueBackground => BackColor(19);

    public static string DarkYellow => ForeColor(136);
    public static string DarkYellowBackground => BackColor(136);

    public static string DarkPurple => ForeColor(55);
    public static string DarkPurpleBackground => BackColor(55);

    public static string GetColor(Names color)
    {
        return color switch
        {
            Names.Black => Black,
            Names.Red => Red,
            Names.Green => Green,
            Names.Yellow => Yellow,
            Names.Blue => Blue,
            Names.Purple => Purple,
            Names.Cyan => Cyan,
            Names.White => White,
            Names.BrightBlack => BrightBlack,
            Names.BrightRed => BrightRed,
            Names.BrightGreen => BrightGreen,
            Names.BrightYellow => BrightYellow,
            Names.BrightBlue => BrightBlue,
            Names.BrightPurple => BrightPurple,
            Names.BrightCyan => BrightCyan,
            Names.BrightWhite => BrightWhite,
            Names.Gray => Gray,
            Names.Orange => Orange,
            Names.Pink => Pink,
            Names.CutePink => CutePink,
            Names.Aqua => Aqua,
            Names.Gold => Gold,
            Names.LightGreen => LightGreen,
            Names.LightBlue => LightBlue,
            Names.Magenta => Magenta,
            Names.LightCyan => LightCyan,
            Names.LightGray => LightGray,
            Names.DarkRed => DarkRed,
            Names.DarkGreen => DarkGreen,
            Names.DarkBlue => DarkBlue,
            Names.DarkYellow => DarkYellow,
            Names.DarkPurple => DarkPurple,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public static string GetBackgroundColor(Names color)
    {
        return color switch
        {
            Names.Black => BlackBackground,
            Names.Red => RedBackground,
            Names.Green => GreenBackground,
            Names.Yellow => YellowBackground,
            Names.Blue => BlueBackground,
            Names.Purple => PurpleBackground,
            Names.Cyan => CyanBackground,
            Names.White => WhiteBackground,
            Names.BrightBlack => BrightBlackBackground,
            Names.BrightRed => BrightRedBackground,
            Names.BrightGreen => BrightGreenBackground,
            Names.BrightYellow => BrightYellowBackground,
            Names.BrightBlue => BrightBlueBackground,
            Names.BrightPurple => BrightPurpleBackground,
            Names.BrightCyan => BrightCyanBackground,
            Names.BrightWhite => BrightWhiteBackground,
            Names.Gray => GrayBackground,
            Names.Orange => OrangeBackground,
            Names.Pink => PinkBackground,
            Names.CutePink => CutePinkBackground,
            Names.Aqua => AquaBackground,
            Names.Gold => GoldBackground,
            Names.LightGreen => LightGreenBackground,
            Names.LightBlue => LightBlueBackground,
            Names.Magenta => MagentaBackground,
            Names.LightCyan => LightCyanBackground,
            Names.LightGray => LightGrayBackground,
            Names.DarkRed => DarkRedBackground,
            Names.DarkGreen => DarkGreenBackground,
            Names.DarkBlue => DarkBlueBackground,
            Names.DarkYellow => DarkYellowBackground,
            Names.DarkPurple => DarkPurpleBackground,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public enum Names
    {
        Black,
        Red,
        Green,
        Yellow,
        Blue,
        Purple,
        Cyan,
        White,
        BrightBlack,
        BrightRed,
        BrightGreen,
        BrightYellow,
        BrightBlue,
        BrightPurple,
        BrightCyan,
        BrightWhite,
        Gray,
        Orange,
        Pink,
        CutePink,
        Aqua,
        Gold,
        LightGreen,
        LightBlue,
        Magenta,
        LightCyan,
        LightGray,
        DarkRed,
        DarkGreen,
        DarkBlue,
        DarkYellow,
        DarkPurple,
    }
}