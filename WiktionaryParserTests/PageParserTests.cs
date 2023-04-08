// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParserTests;

using FluentAssertions;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using WiktionaryParser;
using WiktionaryParser.Models;

public class PageParserTests
{
    [Fact]
    public void ParseWord_VerbSprechen()
    {
        var parser = new PageParser();
        Page page = new Page(0, "sprechen", GetFile("Data.sprechen.txt"));
        var word = parser.ParseWord(page);
        word.Should().BeOfType<Verb>().And.BeEquivalentTo(new Verb("sprechen", new Conjugations
            {
                PraesensIch = "spreche",
                PraesensDu = "sprichst",
                PraesensErSieEs = "spricht",
                PraeteriumIch = "sprach",
                Partizip2 = "gesprochen",
                Konjuktiv2Ich = "spräche",
                ImperativSingular = "sprich",
                ImperativPlural = "sprecht",
                HilfsVerb = "haben",
            }));
    }

    [Fact]
    public void ParseWord_VerbLaufen()
    {
        var parser = new PageParser();
        Page page = new Page(0, "laufen", GetFile("Data.laufen.txt"));
        var word = parser.ParseWord(page);
        word.Should().BeOfType<Verb>().And.BeEquivalentTo(new Verb("laufen", new Conjugations
        {
            PraesensIch = "laufe",
            PraesensDu = "läufst",
            PraesensErSieEs = "läuft",
            PraeteriumIch = "lief",
            Partizip2 = "gelaufen",
            Konjuktiv2Ich = "liefe",
            ImperativSingular = "lauf",
            ImperativSingularAsterisk = "laufe",
            ImperativPlural = "lauft",
            HilfsVerb = "sein",
            HilfsVerbAsterisk = "haben",
        }));
    }

    private static string GetFile(string path)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourcePath = assembly.GetManifestResourceNames().Single(name => name.EndsWith(path));
        using Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
        using StreamReader reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
