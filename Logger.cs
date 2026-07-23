using System;
using System.Diagnostics;

namespace ColoredLogger
{
    public class Logger
    {
        public static void Log(string info, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    LogColors.DefaultForeground = LogColors.brightBlueRGB;
                    Log(LogColors.BrightBlue(info));
                    break;
                case LogLevel.Detail:
                    LogColors.DefaultForeground = LogColors.brightBlackRGB;
                    Log(LogColors.BrightBlack(info));
                    break;
                case LogLevel.Warning:
                    LogColors.DefaultForeground = LogColors.brightYellowRGB;
                    Log(LogColors.BrightYellow(info));
                    break;
                case LogLevel.Error:
                    LogColors.DefaultForeground = LogColors.brightRedRGB;
                    Log(LogColors.BrightRed(info));
                    break;
                default:
                    Log(LogColors.BrightWhite(info));
                    break;
            }
        }

        public static void LogRaw(string text) => Console.WriteLine(text);
        public static void Log(string info)
        {

            LogReal(info);
            // since we change the color when we log, logging without us changing color means the app crashed, so we should set the color to red
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        private static void LogReal(string info)
        {
            // replace return to normals with the current color so we can change the color of texts
            string text = Indent + info;
            Console.WriteLine(text);
        }

        private static readonly Stopwatch appTimer = Stopwatch.StartNew();

        private static Stack<double> startTimes = new Stack<double>();

        public static void BeginTimingBlock()
        {
            startTimes.Push(appTimer.Elapsed.TotalSeconds);
        }

        public static double EndTimingBlock()
        {
            if (startTimes.Count == 0) return 0.0;

            return appTimer.Elapsed.TotalSeconds - startTimes.Pop();
        }
        public static string EndTimingBlockFormatted()
        {
            return $"{EndTimingBlock():F3}ms";
        }
        private const int IndentLenght = 2;
        private static int IndentLevel = 0;
        private static string Indent = "";
        public static void PushIndentLevel()
        {
            IndentLevel++;
            Indent = new(' ', IndentLevel * IndentLenght);
        }
        public static void PopIndentLevel()
        {
            IndentLevel--;
            Indent = new(' ', IndentLevel * IndentLenght);
        }

        private static Stack<long> startMemoryUsages = new Stack<long>();
        private static long CurrMemory => GC.GetTotalMemory(forceFullCollection: false);
        public static void BeginMemoryBlock()
        {
            startMemoryUsages.Push(CurrMemory);
        }
        public static long EndMemoryBlock()
        {
            return CurrMemory - startMemoryUsages.Pop();
        }
        public static string EndMemoryBlockFormatted()
        {
            long delta = EndMemoryBlock();
            return FormatBytes(delta) + " RAM";
        }
        public static string FormatBytes(long bytes)
        {
            bool isNegative = (bytes < 0);
            bytes = Math.Abs(bytes);
            const long KB = 1024;
            const long MB = 1024 * KB;
            const long GB = 1024 * MB;

            string negative = "";
            if (isNegative)
                negative = "-";

            if (bytes >= GB) return $"{negative}{bytes / (double)GB:F2} GB";
            if (bytes >= MB) return $"{negative}{bytes / (double)MB:F2} MB";
            if (bytes >= KB) return $"{negative}{bytes / (double)KB:F2} KB";

            return $"{negative}{bytes} B";
        }

        public static void PrintEmptyLine() => Console.WriteLine();

        public static void PrintTestColors()
        {
            var lb = new LogBuilder();

            // print all default foreground colors
            lb
            .K().BGW().Write("Black").BGK().Write(" ")
            .R().Write("Red ")
            .G().Write("Green ")
            .Y().Write("Yellow ")
            .B().Write("Blue ")
            .M().Write("Magenta ")
            .C().Write("Cyan ")
            .W().WriteLine("White");

            lb
            .BK().Write("BrightBlack ")
            .BR().Write("BrightRed ")
            .BG().Write("BrightGreen ")
            .BY().Write("BrightYellow ")
            .BB().Write("BrightBlue ")
            .BM().Write("BrightMagenta ")
            .BC().Write("BrightCyan ")
            .BW().WriteLine("BrightWhite");

            lb.Log();

            // Print 32 squares with rainbow colors
            Console.WriteLine();
            var rainbowColors = new (int r, int g, int b)[]
            {
                (255,0,0), (255,127,0), (255,255,0), (127,255,0),
                (0,255,0), (0,255,127), (0,255,255), (0,127,255),
                (0,0,255), (127,0,255), (255,0,255), (255,0,127),
                (255,64,0), (255,191,0), (191,255,0), (64,255,0),
                (0,255,64), (0,255,191), (0,191,255), (0,64,255),
                (64,0,255), (191,0,255), (255,0,191), (255,0,64),
                (128,0,128), (128,0,255), (0,128,128), (0,128,255),
                (128,128,0), (255,128,0), (128,0,0), (0,128,0)
            };

            int n = 64;
            var lbSquares = new LogBuilder();

            for (int i = 0; i < n; i++)
            {
                float hue = i / (float)n; // 0.0 to 1.0
                (double R, double G, double B) = HslToRgb(hue, 1, 1);
                lbSquares.Col(
                    (int)(R * 255),
                    (int)(G * 255),
                    (int)(B * 255)
                    ).Write("■");
            }


            lbSquares.WriteLine(""); // New line at the end
            lbSquares.Log();
        }

        public static (double R, double G, double B) HslToRgb(double h, double s, double l)
        {
            // Normalize hue to [0, 1] range
            h = (h % 360.0) / 360.0;
            if (h < 0) h += 1.0;

            s = Math.Clamp(s, 0.0, 1.0);
            l = Math.Clamp(l, 0.0, 1.0);

            double r = l;
            double g = l;
            double b = l;

            if (s != 0.0)
            {
                double max = l < 0.5 ? l * (1.0 + s) : (l + s) - (l * s);
                double min = 2.0 * l - max;

                r = HueToRgbChannel(min, max, h + 1.0 / 3.0);
                g = HueToRgbChannel(min, max, h);
                b = HueToRgbChannel(min, max, h - 1.0 / 3.0);
            }

            return (
                r,
                g,
                b
            );
        }

        private static double HueToRgbChannel(double min, double max, double h)
        {
            if (h < 0.0) h += 1.0;
            if (h > 1.0) h -= 1.0;

            if (h * 6.0 < 1.0) return min + (max - min) * 6.0 * h;
            if (h * 2.0 < 1.0) return max;
            if (h * 3.0 < 2.0) return min + (max - min) * (2.0 / 3.0 - h) * 6.0;

            return min;
        }

    }

    public enum LogLevel
    {
        Info,
        Detail,
        Warning,
        Error,
    }

    public class LogColors
    {
        // https://en.wikipedia.org/wiki/ANSI_escape_code#Colors
        public static string Rgb(object source, int? fr = null, int? fg = null, int? fb = null, int? br = null, int? bg = null, int? bb = null)
        {
            string text = source?.ToString() ?? "";
            string ansiCode = "";
            if (((int?[])[fr, fg, fb]).Any(v => v.HasValue))
                ansiCode = GetANSICodeFG((int)fr!, (int)fg!, (int)fb!);

            if (((int?[])[br, bg, bb]).Any(v => v.HasValue))
                ansiCode += GetANSICodeBG((int)br!, (int)bg!, (int)bb!);

            return $"{ansiCode}{text}{CURR_DEFAULT}";
        }

        public static string GetANSICodeFG(int fr, int fg, int fb)
        {
            string fgCode = $"\u001b[38;2;{fr};{fg};{fb}m";
            return fgCode;
        }
        public static string GetANSICodeBG(int br, int bg, int bb)
        {
            string bgCode = $"\u001b[48;2;{br};{bg};{bb}m";
            return bgCode;
        }

        public static string GetANSICode(int fr, int fg, int fb, int br, int bg, int bb)
            => GetANSICodeFG(fr, fg, fb) + GetANSICodeBG(br, bg, bb);

        public static (int r, int g, int b) DefaultForeground
        {
            get; set
            {
                field = value;
                CURR_DEFAULT = $"\u001b[38;2;{value.r};{value.g};{value.b}m";
            }
        }
        public static string CURR_DEFAULT = "\u001b[0m";
        public static string RESET = "\u001b[0m";

        #region Pre Colors
        public static (int r, int g, int b) blackRGB = (0, 0, 0);
        public static (int r, int g, int b) redRGB = (255, 0, 0);
        public static (int r, int g, int b) greenRGB = (0, 255, 0);
        public static (int r, int g, int b) yellowRGB = (255, 255, 0);
        public static (int r, int g, int b) blueRGB = (0, 0, 255);
        public static (int r, int g, int b) magentaRGB = (255, 0, 255);
        public static (int r, int g, int b) cyanRGB = (0, 255, 255);
        public static (int r, int g, int b) whiteRGB = (255, 255, 255);

        public static (int r, int g, int b) brightBlackRGB = (85, 85, 85);
        public static (int r, int g, int b) brightRedRGB = (255, 85, 85);
        public static (int r, int g, int b) brightGreenRGB = (85, 255, 85);
        public static (int r, int g, int b) brightYellowRGB = (255, 255, 85);
        public static (int r, int g, int b) brightBlueRGB = (85, 85, 255);
        public static (int r, int g, int b) brightMagentaRGB = (255, 85, 255);
        public static (int r, int g, int b) brightCyanRGB = (85, 255, 255);
        public static (int r, int g, int b) brightWhiteRGB = (255, 255, 255);
        #endregion

        #region Colors
        public static string Black(object text) => Rgb(text, blackRGB.r, blackRGB.g, blackRGB.b);
        public static string Red(object text) => Rgb(text, redRGB.r, redRGB.g, redRGB.b);
        public static string Green(object text) => Rgb(text, greenRGB.r, greenRGB.g, greenRGB.b);
        public static string Yellow(object text) => Rgb(text, yellowRGB.r, yellowRGB.g, yellowRGB.b);
        public static string Blue(object text) => Rgb(text, blueRGB.r, blueRGB.g, blueRGB.b);
        public static string Magenta(object text) => Rgb(text, magentaRGB.r, magentaRGB.g, magentaRGB.b);
        public static string Cyan(object text) => Rgb(text, cyanRGB.r, cyanRGB.g, cyanRGB.b);
        public static string White(object text) => Rgb(text, whiteRGB.r, whiteRGB.g, whiteRGB.b);

        public static string BrightBlack(object text) => Rgb(text, brightBlackRGB.r, brightBlackRGB.g, brightBlackRGB.b);
        public static string BrightRed(object text) => Rgb(text, brightRedRGB.r, brightRedRGB.g, brightRedRGB.b);
        public static string BrightGreen(object text) => Rgb(text, brightGreenRGB.r, brightGreenRGB.g, brightGreenRGB.b);
        public static string BrightYellow(object text) => Rgb(text, brightYellowRGB.r, brightYellowRGB.g, brightYellowRGB.b);
        public static string BrightBlue(object text) => Rgb(text, brightBlueRGB.r, brightBlueRGB.g, brightBlueRGB.b);
        public static string BrightMagenta(object text) => Rgb(text, brightMagentaRGB.r, brightMagentaRGB.g, brightMagentaRGB.b);
        public static string BrightCyan(object text) => Rgb(text, brightCyanRGB.r, brightCyanRGB.g, brightCyanRGB.b);
        public static string BrightWhite(object text) => Rgb(text, brightWhiteRGB.r, brightWhiteRGB.g, brightWhiteRGB.b);
        #region Aliases
        public static string K(object text) => Black(text);
        public static string R(object text) => Red(text);
        public static string G(object text) => Green(text);
        public static string Y(object text) => Yellow(text);
        public static string B(object text) => Blue(text);
        public static string M(object text) => Magenta(text);
        public static string C(object text) => Cyan(text);
        public static string W(object text) => White(text);

        public static string BK(object text) => BrightBlack(text);
        public static string BR(object text) => BrightRed(text);
        public static string BG(object text) => BrightGreen(text);
        public static string BY(object text) => BrightYellow(text);
        public static string BB(object text) => BrightBlue(text);
        public static string BM(object text) => BrightMagenta(text);
        public static string BC(object text) => BrightCyan(text);
        public static string BW(object text) => BrightWhite(text);
        #endregion
        #endregion

        #region Background Colors
        public static string BGBlack(object text) => Rgb(text, br: blackRGB.r, bg: blackRGB.g, bb: blackRGB.b);
        public static string BGRed(object text) => Rgb(text, br: redRGB.r, bg: redRGB.g, bb: redRGB.b);
        public static string BGGreen(object text) => Rgb(text, br: greenRGB.r, bg: greenRGB.g, bb: greenRGB.b);
        public static string BGYellow(object text) => Rgb(text, br: yellowRGB.r, bg: yellowRGB.g, bb: yellowRGB.b);
        public static string BGBlue(object text) => Rgb(text, br: blueRGB.r, bg: blueRGB.g, bb: blueRGB.b);
        public static string BGMagenta(object text) => Rgb(text, br: magentaRGB.r, bg: magentaRGB.g, bb: magentaRGB.b);
        public static string BGCyan(object text) => Rgb(text, br: cyanRGB.r, bg: cyanRGB.g, bb: cyanRGB.b);
        public static string BGWhite(object text) => Rgb(text, br: whiteRGB.r, bg: whiteRGB.g, bb: whiteRGB.b);

        public static string BGBrightBlack(object text) => Rgb(text, br: brightBlackRGB.r, bg: brightBlackRGB.g, bb: brightBlackRGB.b);
        public static string BGBrightRed(object text) => Rgb(text, br: brightRedRGB.r, bg: brightRedRGB.g, bb: brightRedRGB.b);
        public static string BGBrightGreen(object text) => Rgb(text, br: brightGreenRGB.r, bg: brightGreenRGB.g, bb: brightGreenRGB.b);
        public static string BGBrightYellow(object text) => Rgb(text, br: brightYellowRGB.r, bg: brightYellowRGB.g, bb: brightYellowRGB.b);
        public static string BGBrightBlue(object text) => Rgb(text, br: brightBlueRGB.r, bg: brightBlueRGB.g, bb: brightBlueRGB.b);
        public static string BGBrightMagenta(object text) => Rgb(text, br: brightMagentaRGB.r, bg: brightMagentaRGB.g, bb: brightMagentaRGB.b);
        public static string BGBrightCyan(object text) => Rgb(text, br: brightCyanRGB.r, bg: brightCyanRGB.g, bb: brightCyanRGB.b);
        public static string BGBrightWhite(object text) => Rgb(text, br: brightWhiteRGB.r, bg: brightWhiteRGB.g, bb: brightWhiteRGB.b);


        #region Background Aliases
        public static string BGK(object text) => BGBlack(text);
        public static string BGR(object text) => BGRed(text);
        public static string BGG(object text) => BGGreen(text);
        public static string BGY(object text) => BGYellow(text);
        public static string BGB(object text) => BGBlue(text);
        public static string BGM(object text) => BGMagenta(text);
        public static string BGC(object text) => BGCyan(text);
        public static string BGW(object text) => BGWhite(text);

        public static string BGBK(object text) => BGBrightBlack(text);
        public static string BGBR(object text) => BGBrightRed(text);
        public static string BGBG(object text) => BGBrightGreen(text);
        public static string BGBY(object text) => BGBrightYellow(text);
        public static string BGBB(object text) => BGBrightBlue(text);
        public static string BGBM(object text) => BGBrightMagenta(text);
        public static string BGBC(object text) => BGBrightCyan(text);
        public static string BGBW(object text) => BGBrightWhite(text);
        #endregion
        #endregion

    }
}
