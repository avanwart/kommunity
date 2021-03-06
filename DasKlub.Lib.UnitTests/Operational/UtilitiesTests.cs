﻿using DasKlub.Lib.Configs;
using DasKlub.Lib.Operational;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DasKlub.Lib.UnitTests.Operational
{
    [TestClass]
    public class UtilitiesTests
    {
        private string _expectedDoman;

        [TestInitialize]
        public void Init()
        {
            _expectedDoman = GeneralConfigs.SiteDomain;
        }

        
        [TestMethod]
        public void ConvertTextToHTML_EndsWithBrackParen_DisplaysNoLink()
        {
            // arrange
            const string input = "[see: http://i.imgur.com/PksccO4.jpg]";

            // act
            var actual = Utilities.ConvertTextToHtml(input);

            // assert 
            const string expected = @"[see: http://i.imgur.com/PksccO4.jpg]";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTextToHTML_EndsWithCloseParenShort_DisplaysUrlWithoutParen()
        {
            // arrange
            const string input = "Contact: Ciwana (http://dasklub.com/ciwana) for more details.";

            // act
            var actual = Utilities.ConvertTextToHtml(input);

            // assert 
            const string expected = @"Contact: Ciwana (<a href=""http://dasklub.com/ciwana"">http://dasklub.com/ciwana</a> for more details.";

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ConvertTextToHTML_EndsWithCloseParen_DisplaysUrlWithoutParen()
        {
            // arrange
            const string input = "(see: http://i.imgur.com/PksccO4.jpg)";

            // act
            var actual = Utilities.ConvertTextToHtml(input);

            // assert 
            const string expected = @"(see: <a target=""_blank"" href=""http://i.imgur.com/PksccO4.jpg"">http://i.imgur.com/PksccO4....</a>";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertTextToHTML_SameExternalLink_DisplaysCorrectly()
        {
            // arrange
            string text = @"Hey all!

Brand new track from Reactor7x - Sick of it all, which is foretaste of slowly upcoming full-length album - powerful dark electro / aggrotech / electro-industrial song.

You can grab it for free or use ""name your price"" to support us.
http://reactor7x.bandcamp.com/album/sick-of-it-all
All remixes will be available online from 4/05/2014 - more info here https://www.facebook.com/events/227708670757860

Track available on:
Facebook: http://www.facebook.com/reactor7x
Soundcloud: http://soundcloud.com/1unatic/reactor7x-sick-of-it-all
Bandcamp: http://reactor7x.bandcamp.com/album/sick-of-it-all
Spotify: http://play.spotify.com/track/1BOnTBEjZ9bTtedWDmdpK6
Deezer: http://www.deezer.com/track/77750205
iTunes: http://itunes.apple.com/ca/album/sick-of-it-all-single/id863056054
Amazon: http://www.amazon.com/Sick-of-it-all/dp/B00JRIQ4LU/ref=sr_1_2?s=dmusic&ie=UTF8&qid=1398078516&sr=1-2&keywords=Reactor7x";

            // act
            string result = Utilities.ConvertTextToHtml(text);

            string expected = @"Hey all!<br />
<br />
Brand new track from Reactor7x - Sick of it all, which is foretaste of slowly upcoming full-length album - powerful dark electro / aggrotech / electro-industrial song.<br />
<br />
You can grab it for free or use ""name your price"" to support us.<br />
<a target=""_blank"" href=""http://reactor7x.bandcamp.com/album/sick-of-it-all"">http://reactor7x.bandcamp.c...</a><br />
All remixes will be available online from 4/05/2014 - more info here <a target=""_blank"" href=""https://www.facebook.com/events/227708670757860"">https://www.facebook.com/ev...</a><br />
<br />
Track available on:<br />
Facebook: <a target=""_blank"" href=""http://www.facebook.com/reactor7x"">http://www.facebook.com/rea...</a><br />
Soundcloud: <a target=""_blank"" href=""http://soundcloud.com/1unatic/reactor7x-sick-of-it-all"">http://soundcloud.com/1unat...</a><br />
Bandcamp: <a target=""_blank"" href=""http://reactor7x.bandcamp.com/album/sick-of-it-all"">http://reactor7x.bandcamp.c...</a><br />
Spotify: <a target=""_blank"" href=""http://play.spotify.com/track/1BOnTBEjZ9bTtedWDmdpK6"">http://play.spotify.com/tra...</a><br />
Deezer: <a target=""_blank"" href=""http://www.deezer.com/track/77750205"">http://www.deezer.com/track...</a><br />
iTunes: <a target=""_blank"" href=""http://itunes.apple.com/ca/album/sick-of-it-all-single/id863056054"">http://itunes.apple.com/ca/...</a><br />
Amazon: <a target=""_blank"" href=""http://www.amazon.com/Sick-of-it-all/dp/B00JRIQ4LU/ref=sr_1_2?s=dmusic&ie=UTF8&qid=1398078516&sr=1-2&keywords=Reactor7x"">http://www.amazon.com/Sick-...</a>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTextToHTML_InternalLinkLinks_DoesNotOpenInNewWindow()
        {
            // arrange
            string text = string.Format(@"Das Cabaret Fledermaus Präsentiert den 
BALL DER SCHWARZEN MASKEN 2014
Deuxvolt live @ Wien (AT)
Friday 28th February 2014 - 22:00 PM
Info: {0}/


1-2 INDUSTRIAL DANCERS ARE REALLY WELCOME, ON THE STAGE

Contact me for more informations here: www.justdeux.com

Just Deux

Poster: {0}/propaganda/Deuxvolt_tour_2014_01_wien.png
note: Thanks to DasKlub community", _expectedDoman);

            // act
            string result = Utilities.ConvertTextToHtml(text);

            // assert 
            string expected = string.Format(@"Das Cabaret Fledermaus Präsentiert den <br />
BALL DER SCHWARZEN MASKEN 2014<br />
Deuxvolt live @ Wien (AT)<br />
Friday 28th February 2014 - 22:00 PM<br />
Info: <a href=""{0}/"">{0}/</a><br />
<br />
<br />
1-2 INDUSTRIAL DANCERS ARE REALLY WELCOME, ON THE STAGE<br />
<br />
Contact me for more informations here: www.justdeux.com<br />
<br />
Just Deux<br />
<br />
Poster: <a href=""{0}/propaganda/Deuxvolt_tour_2014_01_wien.png"">{0}/propagan...</a><br />
note: Thanks to DasKlub community", _expectedDoman);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTextToHTML_TextWithLinks_ContainsHrefsCorrectly()
        {
            // arrange
            string text = @"Das Cabaret Fledermaus Präsentiert den 
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
            string result = Utilities.ConvertTextToHtml(text);

            // assert 
            string expected = @"Das Cabaret Fledermaus Präsentiert den <br />
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
        public void ConvertTextToHTML_TextWithHttpsYouTubeLinks_ContainsHrefsCorrectly()
        {
            // arrange
            string text = @"blah blah
 
https://www.youtube.com/watch?v=v0HBy6JxweE";

            // act
            string result = Utilities.ConvertTextToHtml(text);

            // assert 
            string expected = @"blah blah<br />
 <br />
<div class=""you_tube_iframe""><iframe width=""300"" height=""200"" src=""http://www.youtube.com/embed/v0HBy6JxweE?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTextToHTML_TextWithRegularYouTubeLinks_ContainsHrefsCorrectly()
        {
            // arrange
            string text = @"blah blah
 
http://www.youtube.com/watch?v=v0HBy6JxweE";

            // act
            string result = Utilities.ConvertTextToHtml(text);

            // assert 
            string expected = @"blah blah<br />
 <br />
<div class=""you_tube_iframe""><iframe width=""300"" height=""200"" src=""http://www.youtube.com/embed/v0HBy6JxweE?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTextToHTML_TextWithShortYouTubeLinks_ContainsHrefsCorrectly()
        {
            // arrange
            string text = @"blah blah
 
http://youtu.be/v0HBy6JxweE";

            // act
            string result = Utilities.ConvertTextToHtml(text);

            // assert 
            string expected = @"blah blah<br />
 <br />
<div class=""you_tube_iframe""><iframe width=""300"" height=""200"" src=""http://www.youtube.com/embed/v0HBy6JxweE?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertTextToHTML_TextWithYouTubeProfileLinks_ContainsHrefsCorrectly()
        {
            // arrange
            string text = @"blah blah
 
https://www.youtube.com/user/dasklubber";

            // act
            string result = Utilities.ConvertTextToHtml(text);

            // assert 
            string expected = @"blah blah<br />
 <br />
<a target=""_blank"" href=""https://www.youtube.com/user/dasklubber"">https://www.youtube.com/use...</a>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ExtractYouTubeVideoKey_NonYouTubeLink_ReturnsNull()
        {
            // arrange
            string text = @"https://something.com";

            // act
            string result = Utilities.ExtractYouTubeVideoKey(text);

            // assert 
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ExtractYouTubeVideoKey_HttpNoWwwYouTubeLink_ReturnsVideoKey()
        {
            // arrange
            string text = "http://youtube.com/watch?v=v0HBy6JxweE";

            // act
            string result = Utilities.ExtractYouTubeVideoKey(text);

            // assert 
            Assert.AreEqual("v0HBy6JxweE", result);
        }

        [TestMethod]
        public void ExtractYouTubeVideoKey_HttpYouTubeLink_ReturnsVideoKey()
        {
            // arrange
            string text = "http://www.youtube.com/watch?v=v0HBy6JxweE";

            // act
            string result = Utilities.ExtractYouTubeVideoKey(text);

            // assert 
            Assert.AreEqual("v0HBy6JxweE", result);
        }

        [TestMethod]
        public void ExtractYouTubeVideoKey_HttpsYouTubeLink_ReturnsVideoKey()
        {
            // arrange
            string text = "https://www.youtube.com/watch?v=v0HBy6JxweE";

            // act
            string result = Utilities.ExtractYouTubeVideoKey(text);

            // assert 
            Assert.AreEqual("v0HBy6JxweE", result);
        }

        [TestMethod]
        public void ExtractYouTubeVideoKey_HttpsNoWwwYouTubeLink_ReturnsVideoKey()
        {
            // arrange
            string text = "https://youtube.com/watch?v=v0HBy6JxweE";

            // act
            string result = Utilities.ExtractYouTubeVideoKey(text);

            // assert 
            Assert.AreEqual("v0HBy6JxweE", result);
        }
    }
}