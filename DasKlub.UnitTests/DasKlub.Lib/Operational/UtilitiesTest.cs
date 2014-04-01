using DasKlub.Lib.Operational;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasKlub.UnitTests.DasKlub.Lib.Operational
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void ConvertTextToHTML_TextWithLinks_ContainsHrefsCorrectly()
        {
            // arrange
            var text = @"Das Cabaret Fledermaus Präsentiert den 
BALL DER SCHWARZEN MASKEN 2014
Deuxvolt live @ Wien (AT)
Friday 28th February 2014 - 22:00 PM
Info: http://www.deuxvolt.com/


1-2 INDUSTRIAL DANCERS ARE REALLY WELCOME, ON THE STAGE

Contact me for more informations here: www.justdeux.com

Just Deux

Poster: http://www.deuxvolt.com/propaganda/Deuxvolt_tour_2014_01_wien.png
note: Thanks to DasKlub community";



            // act
            var result = Utilities.ConvertTextToHtml(text);

            // assert 
            var expected = @"Das Cabaret Fledermaus Präsentiert den <br />
BALL DER SCHWARZEN MASKEN 2014<br />
Deuxvolt live @ Wien (AT)<br />
Friday 28th February 2014 - 22:00 PM<br />
Info: <a target=""_blank"" href=""http://www.deuxvolt.com/"">http://www.deuxvolt.com/</a><br />
<br />
<br />
1-2 INDUSTRIAL DANCERS ARE REALLY WELCOME, ON THE STAGE<br />
<br />
Contact me for more informations here: www.justdeux.com<br />
<br />
Just Deux<br />
<br />
Poster: <a target=""_blank"" href=""http://www.deuxvolt.com/propaganda/Deuxvolt_tour_2014_01_wien.png"">http://www.deuxvolt.com/pro...</a><br />
note: Thanks to DasKlub community";

            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void ConvertTextToHTML_TextWithYouTubeLinks_ContainsHrefsCorrectly()
        {
            // arrange
            var text = @"blah blah
 
http://www.youtube.com/watch?v=v0HBy6JxweE";



            // act
            var result = Utilities.ConvertTextToHtml(text);

            // assert 
            var expected = @"blah blah<br />
 <br />
<div class=""you_tube_iframe""><iframe width=""300"" height=""200"" src=""http://www.youtube.com/embed/v0HBy6JxweE?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>";

            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void ConvertTextToHTML_TextWithYouTubeProfileLinks_ContainsHrefsCorrectly()
        {
            // arrange
            var text = @"blah blah
 
https://www.youtube.com/user/dasklubber";



            // act
            var result = Utilities.ConvertTextToHtml(text);

            // assert 
            var expected = @"blah blah<br />
 <br />
<a target=""_blank"" href=""https://www.youtube.com/user/dasklubber"">https://www.youtube.com/use...</a>";
                                                                      
            Assert.AreEqual(expected, result);

        }
    }
}
