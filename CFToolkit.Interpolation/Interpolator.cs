using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CFToolkit.Interpolation
{
    public partial class Interpolator
    {
        private static readonly Regex regInterpolation = new Regex("\\${([A-Za-z0-9_]+)(:([^{}]*))?}");

        public Interpolator(Configuration configuration = null)
        {
            configuration = configuration ?? Configuration.Default;
        }

        public string Interpolate<T>(string source, IDictionary<string, T> withDictionary)
        {
            if (source == null) return null;
            foreach (var match in regInterpolation.Matches(source).OfType<Match>().Reverse())
            {
                string varName = match.Groups[1].Value;

                if (withDictionary.ContainsKey(varName))
                {
                    string format = match.Groups[3].Value;
                    object varVal = withDictionary[varName];
                    string formatted = varVal == null ? "" : Format(varVal, format);
                    source = source.Remove(match.Index, match.Length).Insert(match.Index, formatted);
                }
            }
            return source;
        }

        public string Interpolate(string source, object withObject)
        {
            if (source == null) return null;
            IDictionary<string, object> variables = withObject.GetType().GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(withObject));
            return Interpolate(source, withDictionary: variables);
        }

        private static string Format(object obj, string format)
        {
            if (string.IsNullOrWhiteSpace(format)) return obj.ToString();
            return string.Format("{0:" + format + "}", obj);
        }
    }
}
