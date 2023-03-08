// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser.Models;
public record Noun : WordBase
{
    public Noun(string word, Gender gender, Declensions declensions)
        : base(word, PartOfSpeech.Noun)
    {
        this.Gender = gender;
        this.Declensions = declensions;
    }

    public Gender Gender { get; init; }

    public Declensions Declensions { get; init; }
}