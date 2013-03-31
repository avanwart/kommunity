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
using System.Text;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;

namespace BootBaronLib.AppSpec.DasKlub.BLL
{
    public class ContentLinker
    {
        public static string InsertBandLinks(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;


            string[] bands = input.Split(',');

            var sb = new StringBuilder(100);
            Artist art;

            int total = 0;

            foreach (string b1 in bands)
            {
                if (string.IsNullOrWhiteSpace(b1)) continue;

                art = new Artist(b1);

                total++;

                if (art.ArtistID > 0)
                {
                    if (total == bands.Length)
                    {
                        sb.AppendFormat(@"{0} ", art.HyperLinkToArtist);
                    }
                    else
                    {
                        sb.AppendFormat(@"{0}, ", art.HyperLinkToArtist);
                    }
                }
                else
                {
                    if (total == bands.Length)
                    {
                        sb.AppendFormat(@"{0} ", art.Name);
                    }
                    else
                    {
                        sb.AppendFormat(@"{0}, ", art.Name);
                    }
                }
            }

            return sb.ToString();

            //Artists arts = new Artists();
            //arts.GetAll();

            //string[] bands;

            //foreach (Artist a1 in arts)
            //{
            //    if (a1.IsHidden || input.Contains(a1.HyperLinkToArtist)) continue;

            //    bands = input.Split(',');

            //    foreach (string b1 in bands)
            //    {
            //        input += ReplaceString(b1, a1.Name, a1.HyperLinkToArtist, StringComparison.CurrentCultureIgnoreCase);
            //    }
            //}

            //return input;
        }


        public static string InsertBandLinks(string input, bool enforceCase)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            if (!enforceCase)
            {
                return InsertBandLinks(input);
            }
            else
            {
                var arts = new Artists();
                arts.GetAll();

                foreach (Artist a1 in arts)
                {
                    if (a1.IsHidden) continue;


                    input = input.Replace(a1.Name, a1.HyperLinkToArtist);

                    //if (!string.IsNullOrEmpty(a1.Name))
                    //{
                    //    input = input.Replace(a1.Name, a1.HyperLinkToArtist);
                    //}
                    //else
                    //{
                    //    input = input.Replace(a1.Name, a1.HyperLinkToArtist);
                    //}
                }
                return input;
            }
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