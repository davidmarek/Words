// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParserTests;

using FluentAssertions;
using System.Linq;
using System.Reflection;
using WiktionaryParser;
using WiktionaryParser.Models;

public class PageParserTests
{
    [Fact]
    public void ParseWord_Sprechen()
    {
        var parser = new PageParser();
        Page page = new Page(0, "sprechen", GetFile("Data.sprechen.txt"));
        var word = parser.ParseWord(page);
        word.Should().BeOfType<Verb>();
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
