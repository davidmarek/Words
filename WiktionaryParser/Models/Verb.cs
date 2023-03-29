// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser.Models;

public record Verb : WordBase
{
    public Verb(string word, Conjugations conjugations)
        : base(word, PartOfSpeech.Verb)
    {
        this.Conjugations = conjugations;
    }

    public Conjugations Conjugations { get; init; }
}