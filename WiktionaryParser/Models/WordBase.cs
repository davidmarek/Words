// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser.Models;

public record WordBase : IWord
{
    public WordBase(string word, PartOfSpeech partOfSpeech)
    {
        this.Word = word;
        this.PartOfSpeech = partOfSpeech;
    }

    public string Word { get; init; }

    public PartOfSpeech PartOfSpeech { get; init; }
}