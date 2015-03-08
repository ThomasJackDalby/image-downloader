using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageDownloaderConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------ Image Downloader ------");
            Console.WriteLine("URL? ");
            string url = Console.ReadLine();
            url = "http://www.reddit.com/r/earthporn";

            Console.WriteLine("Output Directory? ");
            string directory = Console.ReadLine();

            string html = ImageDownloadTools.DownloadWebsiteHtml(url);

            if (html == "")
            {
                Console.WriteLine("Error downloading html");
                return;
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            List<HtmlNode> hrefNodes = ImageDownloadTools.ExtractAllImageTags(document);

            Console.WriteLine(String.Format("Found {0} images...", hrefNodes.Count));
            foreach (HtmlNode node in hrefNodes) Console.WriteLine(String.Format("{0} - {1}", hrefNodes.IndexOf(node), node.InnerText));

            Console.WriteLine("Download? (y/n)");
            string result = Console.ReadLine();
            if (result == "y")
            {
                foreach (HtmlNode node in hrefNodes) ImageDownloadTools.DownloadImage(node, directory);
            }
            Console.WriteLine("------ Finished ------");
            Console.ReadLine();
        }
    }
}
