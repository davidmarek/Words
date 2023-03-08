// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Transactions;
    using WiktionaryParser.Models;

    class RegexAction
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

    public class PageParser
    {
        public PageParser()
        {
            this.regexActions = new List<RegexAction>
            {
                new RegexAction(@"^== (?<title>.*) \(\{\{Sprache\|(?<language>.*)\}\}\) ==$", (match, properties) =>
                {
                    properties["title"] = match.Groups["title"].Value;
                    properties["language"] = match.Groups["language"].Value;
                }),
            };
        }

        private readonly List<RegexAction> regexActions;

        public IWord ParseWord(Page page)
        {
            var lines = page.Text.Split("\n").Select(line => line.Trim());
            var properties = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.StartsWith("{{") || line.StartsWith("=="))
                {
                    foreach (var regexAction in this.regexActions)
                    {
                        if (regexAction.Execute(line, properties))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}