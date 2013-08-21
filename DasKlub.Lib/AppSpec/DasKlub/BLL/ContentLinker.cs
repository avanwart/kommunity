//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Linq;
using System.Text;
using DasKlub.Lib.AppSpec.DasKlub.BOL.ArtistContent;

namespace DasKlub.Lib.AppSpec.DasKlub.BLL
{
    public class ContentLinker
    {
        private static string InsertBandLinks(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            var bands = input.Split(',');
            var sb = new StringBuilder(100);
            var total = 0;

            foreach (var art in from b1 in bands where !string.IsNullOrWhiteSpace(b1) select new Artist(b1))
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

            return arts.Where(a1 => !a1.IsHidden).Aggregate(input, (current, a1) => current.Replace(a1.Name, a1.HyperLinkToArtist));
        }

        public static string ReplaceString(string str, string oldValue, string newValue, StringComparison comparison)
        {
            var sb = new StringBuilder(100);
            var previousIndex = 0;
            var index = str.IndexOf(oldValue, comparison);

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