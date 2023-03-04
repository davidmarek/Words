namespace WiktionaryParserTests
{
    using WiktionaryParser;

    public class PageSplitterTests
    {
        [Fact]
        public async Task SplitPages_WiktionaryDump()
        {
            var splitter = new PageSplitter();
            var pages = splitter.SplitPages("C:/Users/damarek/Downloads/dewiktionary/dewiktionary-20230220-pages-articles-multistream.xml");
            await foreach (var page in pages) {
                Console.WriteLine(page);
            }
        }
    }
}