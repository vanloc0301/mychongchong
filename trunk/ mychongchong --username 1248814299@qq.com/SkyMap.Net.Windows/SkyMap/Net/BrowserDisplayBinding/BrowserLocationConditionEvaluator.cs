namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;
    using System.Text.RegularExpressions;

    public class BrowserLocationConditionEvaluator : IConditionEvaluator
    {
        public bool IsValid(object caller, Condition condition, Codon codon)
        {
            HtmlViewPane pane = (HtmlViewPane) caller;
            string input = pane.Url.ToString();
            string pattern = condition.Properties["urlRegex"];
            string str3 = condition.Properties["options"];
            if ((str3 != null) && (str3.Length > 0))
            {
                return Regex.IsMatch(input, pattern, (RegexOptions) Enum.Parse(typeof(RegexOptions), str3, true));
            }
            return Regex.IsMatch(input, pattern);
        }
    }
}

