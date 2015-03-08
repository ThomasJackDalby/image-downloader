using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Input;

namespace ImageDownloaderConsole
{
    public class ImageDownloadTools
    {
        public static string DownloadWebsiteHtml(string url)
        {
            string htmlcode = "";
            WebClient client = new WebClient();
            try { htmlcode = client.DownloadString(url); }
            catch (Exception e) { }
            return htmlcode;
        }

        public static List<HtmlNode> ExtractAllImageTags(HtmlDocument htmlSnippet)
        {
            if (htmlSnippet.DocumentNode == null)
            {
                Console.WriteLine("error downloading html of url");
                return null;
            }
            List<HtmlNode> hrefNodes = new List<HtmlNode>();

            foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                if (att.Value.Contains(".jpg") && link.InnerText != "")  hrefNodes.Add(link);
            }

            return hrefNodes;
        }

        public static void DownloadImage(HtmlNode hrefNode, string directory)
        {
            Image image = DownloadImageFromUrl(hrefNode.Attributes["href"].Value);
            image = WriteTitleToImage(image, hrefNode.InnerText);
            if (image == null) return;
            int num = 1;
            string filename = String.Format("Image {0:dd_MM_yyyy} {1:000}.jpg", DateTime.Now, num);
            string savedirectory = directory + @"/" + filename;
            while (File.Exists(savedirectory))
            {
                num++;
                filename = String.Format("Image {0:dd_MM_yyyy} {1:000}.jpg", DateTime.Now, num);
                savedirectory = directory + @"/" + filename;
            }
            image.Save(savedirectory);
        }

        public static Image DownloadImageFromUrl(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

                using (HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (Stream stream = httpWebReponse.GetResponseStream())
                    {
                        return Image.FromStream(stream);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured downloading image");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static Image WriteTitleToImage(Image image, string title)
        {
            Bitmap bmp = new Bitmap(image);

            int margin = 20;

            RectangleF rectf = new RectangleF(margin, margin, image.Width - margin, image.Height - margin);

            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(title, new Font("Tahoma", 12), Brushes.Black, rectf);

            g.Flush();

            return (Image)bmp;
        }

        //public static bool FastBitmapCompare(Bitmap bmp1, Bitmap bmp2)
        //{
        //    bool equals = true;
        //    Rectangle rect = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
        //    BitmapData bmpData1 = bmp1.LockBits(rect, ImageLockMode.ReadOnly, bmp1.PixelFormat);
        //    BitmapData bmpData2 = bmp2.LockBits(rect, ImageLockMode.ReadOnly, bmp2.PixelFormat);
        //    unsafe
        //    {
        //        byte* ptr1 = (byte*)bmpData1.Scan0.ToPointer();
        //        byte* ptr2 = (byte*)bmpData2.Scan0.ToPointer();
        //        int width = rect.Width * 3; // for 24bpp pixel data
        //        for (int y = 0; equals && y < rect.Height; y++)
        //        {
        //            for (int x = 0; x < width; x++)
        //            {
        //                if (*ptr1 != *ptr2)
        //                {
        //                    equals = false;
        //                    break;
        //                }
        //                ptr1++;
        //                ptr2++;
        //            }
        //            ptr1 += bmpData1.Stride - width;
        //            ptr2 += bmpData2.Stride - width;
        //        }
        //    }
        //    bmp1.UnlockBits(bmpData1);
        //    bmp2.UnlockBits(bmpData2);
        //    return equals;
        //}
    }
}
