using DasKlub.Lib.Operational;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DasKlub.Lib.UnitTests.Operational
{
    [TestClass]
    public class ConverterTests
    {
        [TestInitialize]
        public void Init()
        {
           
        }

        [TestMethod]
        public void GetFixedLengthString_LongString_ReturnsTrucatedString()
        {
            // arrange
            const string text = @"R4ND0MN355 posted up a status earlier about a contest another site (in this case, a Facebook page) was doing and there were a few comments on there and I wanted to add the following but it would not let me (probably because it has become a bit of a long winded post, but besides, it is probably better posted here).

I think there are two mindsets here and is applicable to a few things in life, not just to the 'scene' (for want of a better word).

The first mind set goes... oh look someone doing something similar, but as there is limited audience, this will encroach on what I'm doing and there fore is a rival and threat as it will split / or decrease my potential audience.

The alternative view point is to go... hey look, someone else is doing something similar... that's cool as that means it's a potential to expand what I'm doing by cross-promoting. There is potential that we can add to their audience and there is potential that their audience can add to ours. Further more, together, both increase their reaches and are able to draw more people into the circle... aka expansion of the scene.

From my experience back when I was running pubs (mind you, I'm not saying this is the right way, everyone has their own way of working), I found pubs and clubs based on alternative scenes actually tended to work better when the later model was followed rather than the former. I think the same is applicable here.
";
            // act
            var result = FromString.GetFixedLengthString(text, 160);

            const string expected = @"R4ND0MN355 posted up a status earlier about a contest another site (in this case, a Facebook page) was doing and there were a few comments on there and I wanted";

            Assert.AreEqual(expected, result);
        }
    }
}