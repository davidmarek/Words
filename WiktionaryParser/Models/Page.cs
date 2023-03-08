// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser.Models;

public readonly record struct Page(int Namespace, string Title, string Text);