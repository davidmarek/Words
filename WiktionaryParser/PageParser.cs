// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using WiktionaryParser.Models;

    class ListItem
    {
        public ListItem(int index, string content)
        {
            this.Index = index;
            this.Content = content;
        }

        public int Index { get; set; }

        public string Content { get; set; }

    }

    class Context
    {
        private string? context = null;
        private string lastKey = "default";
        private Dictionary<string, List<ListItem>> items = new Dictionary<string, List<ListItem>>();

        public void AddKey(string key)
        {
            this.lastKey = key;
        }

        public void AddItem(ListItem item)
        {
            if (!this.items.ContainsKey(this.lastKey))
            {
                this.items[this.lastKey] = new List<ListItem> { item };
            } 
            else
            {
                this.items[this.lastKey].Add(item);
            }
        }

        public Dictionary<string, List<ListItem>> GetItems()
        {
            return this.items;
        }
    }

    public class PageParser
    {
        private const string PraesensIch = "Präsens_ich";
        private const string PraesensDu = "Präsens_du";
        private const string PraesensErSieEs = "Präsens_er, sie, es";
        private const string PraeteriumIch = "Präteritum_ich";
        private const string Partizip2 = "Partizip II";
        private const string Konjuktiv2Ich = "Konjunktiv II_ich";
        private const string ImperativSingular = "Imperativ Singular";
        private const string ImperativSingularAsterisk = "Imperativ Singular*";
        private const string ImperativPlural = "Imperativ Plural";
        private const string HilfsVerb = "Hilfsverb";
        private const string HilfsVerbAsterisk = "Hilfsverb*";

        private readonly List<RegexAction> regexActions;
        private readonly List<IWord> parsedWords;

        private readonly string verbFormNamesRegex = string.Join("|", new[]
        {
            PraesensIch,
            PraesensDu,
            PraesensErSieEs,
            PraeteriumIch,
            Partizip2,
            Konjuktiv2Ich,
            ImperativSingular,
            ImperativSingularAsterisk,
            ImperativPlural,
            HilfsVerb,
            HilfsVerbAsterisk,
        }).Replace("*", "\\*");

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
                new RegexAction(@"^=== \{\{Wortart\|Verb\|Deutsch\}\}(, ''(?<verbType>.*)'')? ===$", (match, properties) =>
                {
                    properties["partOfSpeech"] = "Verb";
                }),
                new RegexAction(@"^\|(?<case>Nominativ|Genitiv|Dativ|Akkusativ) (?<number>Singular|Plural)=(?<word>.*)$", (match, properties) =>
                {
                    var caseType = match.Groups["case"].Value.ToLower();
                    var number = match.Groups["number"].Value.ToLower();
                    properties[$"{caseType}_{number}"] = match.Groups["word"].Value;
                }),
                new RegexAction($@"^\|(?<verbFormName>{this.verbFormNamesRegex})=(?<verbForm>.*)$", (match, properties) => {
                    var verbFormName = match.Groups["verbFormName"].Value;
                    var verbForm = match.Groups["verbForm"].Value;
                    properties[verbFormName] = verbForm;
                }),
            };
            this.parsedWords = new List<IWord>();
        }

        public IWord? ParseWord(Page page)
        {
            var lines = page.Text.Split("\n").Select(line => line.Trim());
            var properties = new Dictionary<string, string>();
            string context = null;
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
            } else if (properties["partOfSpeech"] == "Verb")
            {
                this.parsedWords.Add(new Verb(properties["title"], new Conjugations
                {
                    PraesensIch = properties.ContainsKey(PraesensIch) ? properties[PraesensIch] : null,
                    PraesensDu = properties.ContainsKey(PraesensDu) ? properties[PraesensDu] : null,
                    PraesensErSieEs = properties.ContainsKey(PraesensErSieEs) ? properties[PraesensErSieEs] : null,
                    PraeteriumIch = properties.ContainsKey(PraeteriumIch) ? properties[PraeteriumIch] : null,
                    Partizip2 = properties.ContainsKey(Partizip2) ? properties[Partizip2] : null,
                    Konjuktiv2Ich = properties.ContainsKey(Konjuktiv2Ich) ? properties[Konjuktiv2Ich] : null,
                    ImperativSingular = properties.ContainsKey(ImperativSingular) ? properties[ImperativSingular] : null,
                    ImperativSingularAsterisk = properties.ContainsKey(ImperativSingularAsterisk) ? properties[ImperativSingularAsterisk] : null,
                    ImperativPlural = properties.ContainsKey(ImperativPlural) ? properties[ImperativPlural] : null,
                    HilfsVerb = properties.ContainsKey(HilfsVerb) ? properties[HilfsVerb] : null,
                    HilfsVerbAsterisk = properties.ContainsKey(HilfsVerbAsterisk) ? properties[HilfsVerbAsterisk] : null,
                }));
            }
        }

        private static Gender ParseGender(string gender) => gender switch
            {
                "n" => Gender.Neuter,
                "f" => Gender.Feminine,
                "m" => Gender.Masculin,
                _ => throw new Exception("Invalid gender"),
            };
    }
}