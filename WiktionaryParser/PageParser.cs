// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Transactions;
    using WiktionaryParser.Models;

    public class PageParser
    {
        private readonly List<RegexAction> regexActions;
        private readonly List<IWord> parsedWords;

        public PageParser()
        {
            this.regexActions = new List<RegexAction>
            {
                new RegexAction(@"^== (?<title>.*) \(\{\{Sprache\|(?<language>.*)\}\}\) ==$", (match, properties) =>
                {
                    if (properties.Count > 0)
                    {
                        this.AddWord(properties);
                        properties.Clear();
                    }

                    properties["title"] = match.Groups["title"].Value;
                    properties["language"] = match.Groups["language"].Value;
                }),
                new RegexAction(@"^=== \{\{Wortart\|(?<partOfSpeech>.*)\|Deutsch\}\}, \{\{(?<gender>.*)\}\} ===$", (match, properties) =>
                {
                    properties["partOfSpeech"] = match.Groups["partOfSpeech"].Value;
                    properties["gender"] = match.Groups["gender"].Value;
                }),
                new RegexAction(@"^\|(?<case>Nominativ|Genitiv|Dativ|Akkusativ) (?<number>Singular|Plural)=(?<word>.*)$", (match, properties) =>
                {
                    var caseType = match.Groups["case"].Value.ToLower();
                    var number = match.Groups["number"].Value.ToLower();
                    properties[$"{caseType}_{number}"] = match.Groups["word"].Value;
                }),
            };
            this.parsedWords = new List<IWord>();
        }

        public IWord? ParseWord(Page page)
        {
            var lines = page.Text.Split("\n").Select(line => line.Trim());
            var properties = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.StartsWith("{{") || line.StartsWith("==") || line.StartsWith("|"))
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

            if (properties.Count > 0)
            {
                this.AddWord(properties);
            }

            if (this.parsedWords.Count > 0)
            {
                if (this.parsedWords.Count > 1)
                {
                    throw new Exception("Too many german words");
                }

                return this.parsedWords[0];
            }

            return null;
        }

        private void AddWord(IDictionary<string, string> properties)
        {
            if (properties["language"] != "Deutsch")
            {
                return;
            }

            if (properties["partOfSpeech"] == "Substantiv")
            {
                this.parsedWords.Add(new Noun(properties["title"], ParseGender(properties["gender"]), new Declensions
                {
                    NominativSingular = properties["nominativ_singular"],
                    NominativPlural = properties["nominativ_plural"],
                    GenitivSingular = properties["genitiv_singular"],
                    GenitivPlural = properties["genitiv_plural"],
                    DativSingular = properties["dativ_singular"],
                    DativPlural = properties["dativ_plural"],
                    AkkusativSingular = properties["akkusativ_singular"],
                    AkkusativPlural = properties["akkusativ_plural"],
                }));
            }
        }

        private Gender ParseGender(string gender) => gender switch
            {
                "n" => Gender.Neuter,
                "f" => Gender.Feminine,
                "m" => Gender.Masculin,
                _ => throw new Exception("Invalid gender")
            };
    }
}