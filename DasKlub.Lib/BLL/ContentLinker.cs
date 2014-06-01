using System;
using System.Linq;
using System.Text;
using DasKlub.Lib.BOL.ArtistContent;

namespace DasKlub.Lib.BLL
{
    public class ContentLinker
    {
        private static string InsertBandLinks(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            string[] bands = input.Split(',');
            var sb = new StringBuilder(100);
            int total = 0;

            foreach (Artist art in from b1 in bands where !string.IsNullOrWhiteSpace(b1) select new Artist(b1))
            {
                total++;

                sb.AppendFormat(total == bands.Length ? @"{0} " : @"{0}, ",
                    art.ArtistID > 0 ? art.HyperLinkToArtist : art.Name);
            }

            return sb.ToString();
        }

        public static string InsertBandLinks(string input, bool enforceCase)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            if (!enforceCase)
            {
                return InsertBandLinks(input);
            }
            var arts = new Artists();
            arts.GetAll();

            return arts.Where(a1 => !a1.IsHidden)
                .Aggregate(input, (current, a1) => current.Replace(a1.Name, a1.HyperLinkToArtist));
        }

        public static string ReplaceString(string str, string oldValue, string newValue, StringComparison comparison)
        {
            var sb = new StringBuilder(100);
            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);

            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }
    }
}