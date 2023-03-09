// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser.Models;

public record Declensions
{
    public string? NominativSingular { get; init; }

    public string? NominativPlural { get; init; }

    public string? GenitivSingular { get; init; }

    public string? GenitivPlural { get; init; }

    public string? DativSingular { get; init; }

    public string? DativPlural { get; init; }

    public string? AkkusativSingular { get; init; }

    public string? AkkusativPlural { get; init; }
}