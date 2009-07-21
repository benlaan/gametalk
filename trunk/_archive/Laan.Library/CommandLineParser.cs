using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Laan.Utilities
{
    public class Arguments : Dictionary<string, string>
    {
        public Arguments() : base()
        {

        }

        public new string this[string key]
        {
            get { return base.ContainsKey(key) ? base[key] : null; }
            set { base[key] = value; }
        }
    }

    public class CommandLineParser
    {
        public CommandLineParser()
        {
            _switches = new char[] { '/', '-' };
            _arguments = new Arguments();
        }

        private Arguments _arguments;
        private char[]                     _switches;

        public void Parse(string[] args)
        {
            int iIndex = 0;
            while(iIndex < args.Length)
            {
                string data = "";

                string name = args[iIndex];
                TrimSwitch(ref name);

                if (args.Length > iIndex + 1)
                {
                    string value = args[iIndex + 1];
                    bool hasSwitch = IsSwitch(ref value);

                    if (!hasSwitch)
                    {
                        data = value;
                        iIndex++;
                    }
                }
                _arguments[name] = data;

                iIndex ++;
            }
        }

        public void Parse(string args)
        {
            // match quoted text if possible otherwise match until a space is found
            Regex rex = new Regex(@"""[^""]*""|[^ ]+", RegexOptions.IgnorePatternWhitespace);
            MatchCollection matches = rex.Matches(args);

            // define array
            string[] result = new string[matches.Count];

            // populate matched values into result
            int index = 0;
            foreach(Match match in matches)
                result[index++] = match.Value.Trim('"');

            Parse(result);
        }

        private bool IsSwitch(ref string arg)
        {
            bool result = false;

            char switchChar = (arg.ToCharArray()[0]);

            foreach (char item in _switches)
                result |= (item == switchChar);

            if (result)
                TrimSwitch(ref arg);

            return result;
        }

        private void TrimSwitch(ref string arg)
        {
            arg = arg.TrimStart(_switches);
        }

        public Arguments Arguments
        {
            get { return _arguments; }
        }

        public char[] Switches
        {
            get { return _switches; }
            set { _switches = value; }
        }
    }
}
