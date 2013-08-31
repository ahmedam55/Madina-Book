using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace bsmAnalysis.Models
{
    public class Text:Notify
    {
        public string body { get; set; }
        public DateTime timeAdded { get; set; }




        public static string linkyPictureInBox(string body)
        {
            StringBuilder messageBody = new StringBuilder(body);

            Regex linkParser = new Regex(@"\b(?:http://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            List<string> links = new List<string>();
            foreach (Match m in linkParser.Matches(messageBody.ToString()))
            {
                messageBody.Replace(m.Value, "<a href='" + m.Value + "'>" + m.Value + "</a>");
                links.Add(m.Value);
              //  Console.WriteLine(m.Value);

            }
            if (links.Count != 0)
            {
                string html;
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    html = client.DownloadString(links[0].ToString());
                }

                var imgs = Regex.Matches(html, @"<img\s[^>]*>(?:\s*?</img>)?", RegexOptions.IgnoreCase);
                //for (int i = 0; i < imgs.Count; i++)
                //{

                //    Console.WriteLine(imgs[i]);
                //}

                string pictureInBox = @"<div class='well' style='max-height: 200px;max-width: 250px; margin: 0 auto 10px;'><a href='" + links[0] + "'>" + GetTitle(html) + GetThumb(links[0]) + "</a></div>";//imgs[0].ToString().Replace("<img", "<img class='thumb' ")
                messageBody.Append(pictureInBox);
            }
           

            return messageBody.ToString() ;
        }

        //public string linkyHrefs(string links)
        //{
        //    return "will be implemented";
        //}

        public string tagUser(string user)
        {
            return "will be implemented";
        }
        static string GetTitle(string file)
        {
            Match m = Regex.Match(file, @"<title>\s*(.+?)\s*</title>");
            if (m.Success)
            {
                var title = m.Groups[1].Value.Replace("&#x202b;", "").Replace("&#x202c;&lrm;","");

                Encoding windows1251 = Encoding.UTF8;
                Encoding utf8 = Encoding.GetEncoding("windows-1256");
                byte[] utfBytes = utf8.GetBytes(title);
                byte[] isoBytes = Encoding.Convert(utf8, windows1251, utfBytes);
                string msg = windows1251.GetString(isoBytes);
               

                //Byte[] bytes = System.Text.Encoding.Default.GetBytes(title);
                //String yourString = System.Text.Encoding.UTF8.GetString(bytes);


                return title;
            }
            else
            {
                return "";
            }
        }
        static string GetThumb(string links0)
        {
           
            if (links0.Contains("youtube") && links0.Contains("watch"))
            {
                var paramss = Regex.Split(links0, "v=");
               
               
                var youtubeId = paramss[1].Substring(0, 11);
              
                string youtubeThumb = "<img class='thumb' src='http://i4.ytimg.com/vi/" + youtubeId + "/mqdefault.jpg' >";
                return youtubeThumb;
               
            }
            else
            {
                return "<img class='thumb' src='http://immediatenet.com/t/m?Size=1024x768&URL=" + links0 + "' >";
            }
        }

      

    }
}