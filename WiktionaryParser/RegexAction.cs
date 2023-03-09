// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class RegexAction
    {
        private readonly Regex regex;
        private readonly Action<Match, IDictionary<string, string>> action;

        public RegexAction(string regex, Action<Match, IDictionary<string, string>> action)
        {
            this.regex = new Regex(regex);
            this.action = action;
        }

        public bool Execute(string line, IDictionary<string, string> properties)
        {
            var matches = this.regex.Matches(line);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    this.action(match, properties);
                }

                return true;
            }

            return false;
        }
    }
}