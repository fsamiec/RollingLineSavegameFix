using System.Globalization;

namespace RollingLineSavegameFix.Services
{
    public class ParseAndAddFloatValue : IParseAndAddFloatValue
    {
        public string For(string input, float addition) => (float.Parse(input, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US")) + addition).ToString(CultureInfo.GetCultureInfo("en-US"));
    }
}
