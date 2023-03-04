// Copyright (c) David Marek. All rights reserved.

namespace WiktionaryParser
{
    using System.Xml;

    public readonly record struct Page(int Namespace, string Title, string Text);

    public class PageSplitter
    {
        public async IAsyncEnumerable<Page> SplitPages(string dumpFileName)
        {
            using XmlReader xmlReader = XmlReader.Create(dumpFileName, new XmlReaderSettings { Async = true });
            while (await xmlReader.ReadAsync())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element when xmlReader.Name == "page":
                        Console.WriteLine(xmlReader.Name);
                        var page = ReadSubtreeXml(xmlReader);
                        if (page.Namespace == 0)
                        {
                            yield return page;
                        }
                        break;
                    default:
                        Console.WriteLine(xmlReader.Name);
                        break;
                }
            }
        }

        private static Page ReadSubtreeXml(XmlReader xmlReader)
        {
            using var subtreeReader = xmlReader.ReadSubtree();
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(subtreeReader);
            var ns = int.Parse(xmlDocument.GetElementsByTagName("ns").OfType<XmlNode>().Single().InnerText);
            var title = xmlDocument.GetElementsByTagName("title").OfType<XmlNode>().Single().InnerText;
            var text = xmlDocument.GetElementsByTagName("text").OfType<XmlNode>().Single().InnerText;
            return new Page(ns, title, text);
        }
    }
}