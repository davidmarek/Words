// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser.Models;

public record Conjugations
{
    public string? PraesensIch { get; init; }

    public string? PraesensDu { get; init; }

    public string? PraesensErSieEs { get; init; }

    public string? PraeteriumIch { get; init; }

    public string? Partizip2 { get; init; }

    public string? Konjuktiv2Ich { get; init; }

    public string? ImperativSingular { get; init; }

    public string? ImperativSingularAsterisk { get; init; }

	public string? ImperativPlural { get;init; }

    public string? HilfsVerb { get; init; }

    public string? HilfsVerbAsterisk { get; init; }
}