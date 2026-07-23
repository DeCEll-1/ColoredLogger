using System.Text;

namespace ColoredLogger
{
    public class LogBuilder
    {
        public LogBuilder((int r, int g, int b)? fg = null, (int r, int g, int b)? bg = null)
        {
            defaultFGColor = fg ?? LogColors.whiteRGB;
            defaultBGColor = bg ?? LogColors.blackRGB;
        }

        private StringBuilder logBuilderSB = new();

        public LogBuilder Write(object txt, int padding = 0)
        {
            string text = (txt.ToString() ?? "");
            if (padding > 0)
                text = text.PadLeft(padding);
            else if (padding < 0)
                text = text.PadRight(Math.Abs(padding));

            logBuilderSB.Append(text);
            return this;
        }
        public LogBuilder WriteLine(object text)
        {
            logBuilderSB.AppendLine(text.ToString() ?? "");
            return this;
        }

        public LogBuilder DefaultFGFromLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    this.defaultFGColor = LogColors.brightBlueRGB;
                    break;
                case LogLevel.Detail:
                    this.defaultFGColor = LogColors.brightBlackRGB;
                    break;
                case LogLevel.Warning:
                    this.defaultFGColor = LogColors.brightYellowRGB;
                    break;
                case LogLevel.Error:
                    this.defaultFGColor = LogColors.brightRedRGB;
                    break;
            }
            return this;
        }

        public (int r, int g, int b) defaultFGColor = LogColors.whiteRGB;
        public (int r, int g, int b) defaultBGColor = LogColors.blackRGB;

        public LogBuilder NewLine() => WriteLine("");
        public LogBuilder NL() => WriteLine("");

        #region Foreground Colors
        public LogBuilder Default()
        { (int r, int g, int b) rgb = this.defaultFGColor; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }

        public LogBuilder Color(int r, int g, int b)
            => Write(LogColors.GetANSICodeFG(r, g, b));

        public LogBuilder Black()
        { (int r, int g, int b) rgb = LogColors.blackRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder Red()
        { (int r, int g, int b) rgb = LogColors.redRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder Green()
        { (int r, int g, int b) rgb = LogColors.greenRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder Yellow()
        { (int r, int g, int b) rgb = LogColors.yellowRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder Blue()
        { (int r, int g, int b) rgb = LogColors.blueRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder Magenta()
        { (int r, int g, int b) rgb = LogColors.magentaRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder Cyan()
        { (int r, int g, int b) rgb = LogColors.cyanRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder White()
        { (int r, int g, int b) rgb = LogColors.whiteRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }

        public LogBuilder BrightBlack()
        { (int r, int g, int b) rgb = LogColors.brightBlackRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BrightRed()
        { (int r, int g, int b) rgb = LogColors.brightRedRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BrightGreen()
        { (int r, int g, int b) rgb = LogColors.brightGreenRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BrightYellow()
        { (int r, int g, int b) rgb = LogColors.brightYellowRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BrightBlue()
        { (int r, int g, int b) rgb = LogColors.brightBlueRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BrightMagenta()
        { (int r, int g, int b) rgb = LogColors.brightMagentaRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BrightCyan()
        { (int r, int g, int b) rgb = LogColors.brightCyanRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BrightWhite()
        { (int r, int g, int b) rgb = LogColors.brightWhiteRGB; return Write(LogColors.GetANSICodeFG(rgb.r, rgb.g, rgb.b)); }

        #region Foreground Aliases
        public LogBuilder Col(int r, int g, int b)
            => Color(r, g, b);


        public LogBuilder D() => Default();
        public LogBuilder K() => Black();
        public LogBuilder R() => Red();
        public LogBuilder G() => Green();
        public LogBuilder Y() => Yellow();
        public LogBuilder B() => Blue();
        public LogBuilder M() => Magenta();
        public LogBuilder C() => Cyan();
        public LogBuilder W() => White();

        public LogBuilder BK() => BrightBlack();
        public LogBuilder BR() => BrightRed();
        public LogBuilder BG() => BrightGreen();
        public LogBuilder BY() => BrightYellow();
        public LogBuilder BB() => BrightBlue();
        public LogBuilder BM() => BrightMagenta();
        public LogBuilder BC() => BrightCyan();
        public LogBuilder BW() => BrightWhite();
        #endregion
        #endregion

        #region Background Colors
        public LogBuilder BGColor(int r, int g, int b)
            => Write(LogColors.GetANSICodeBG(r, g, b));

        public LogBuilder BGDefault()
        { (int r, int g, int b) rgb = this.defaultFGColor; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }

        public LogBuilder BGBlack()
        { (int r, int g, int b) rgb = LogColors.blackRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGRed()
        { (int r, int g, int b) rgb = LogColors.redRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGGreen()
        { (int r, int g, int b) rgb = LogColors.greenRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGYellow()
        { (int r, int g, int b) rgb = LogColors.yellowRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGBlue()
        { (int r, int g, int b) rgb = LogColors.blueRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGMagenta()
        { (int r, int g, int b) rgb = LogColors.magentaRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGCyan()
        { (int r, int g, int b) rgb = LogColors.cyanRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGWhite()
        { (int r, int g, int b) rgb = LogColors.whiteRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }

        public LogBuilder BGBrightBlack()
        { (int r, int g, int b) rgb = LogColors.brightBlackRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGBrightRed()
        { (int r, int g, int b) rgb = LogColors.brightRedRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGBrightGreen()
        { (int r, int g, int b) rgb = LogColors.brightGreenRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGBrightYellow()
        { (int r, int g, int b) rgb = LogColors.brightYellowRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGBrightBlue()
        { (int r, int g, int b) rgb = LogColors.brightBlueRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGBrightMagenta()
        { (int r, int g, int b) rgb = LogColors.brightMagentaRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGBrightCyan()
        { (int r, int g, int b) rgb = LogColors.brightCyanRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }
        public LogBuilder BGBrightWhite()
        { (int r, int g, int b) rgb = LogColors.brightWhiteRGB; return Write(LogColors.GetANSICodeBG(rgb.r, rgb.g, rgb.b)); }

        #region Background Aliases
        public LogBuilder BGCol(int r, int g, int b)
            => BGColor(r, g, b);

        public LogBuilder BGD() => BGDefault();
        public LogBuilder BGK() => BGBlack();
        public LogBuilder BGR() => BGRed();
        public LogBuilder BGG() => BGGreen();
        public LogBuilder BGY() => BGYellow();
        public LogBuilder BGB() => BGBlue();
        public LogBuilder BGM() => BGMagenta();
        public LogBuilder BGC() => BGCyan();
        public LogBuilder BGW() => BGWhite();

        public LogBuilder BGBK() => BGBrightBlack();
        public LogBuilder BGBR() => BGBrightRed();
        public LogBuilder BGBG() => BGBrightGreen();
        public LogBuilder BGBY() => BGBrightYellow();
        public LogBuilder BGBB() => BGBrightBlue();
        public LogBuilder BGBM() => BGBrightMagenta();
        public LogBuilder BGBC() => BGBrightCyan();
        public LogBuilder BGBW() => BGBrightWhite();
        #endregion
        #endregion

        public override string ToString()
        {
            string res = logBuilderSB.ToString();
            return res;
        }

        public void Log()
        {
            Logger.Log(this.ToString());
        }


    }
}
