namespace WiktionaryParser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum PartOfSpeech
    {
        Noun, Verb
    }

    public enum Gender
    {
        Masculin,
        Feminine,
        Neuter,
    }

    public readonly record struct Declensions (string NominativeSingular, string NominativePlural, string GenitivSingular, string GenitivPlural, string DativSingular, string DativPlural, string AkkusativSingular, string AkkusativPlural);

    public record NounProperies (Gender Gender, Declensions Declensions);

    public readonly record struct WordPage(string Word, PartOfSpeech PartOfSpeech);

    public class PageParser
    {
    }
}
