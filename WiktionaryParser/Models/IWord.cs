// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser.Models;

public interface IWord
{
    PartOfSpeech PartOfSpeech { get; init; }

    string Word { get; init; }
}