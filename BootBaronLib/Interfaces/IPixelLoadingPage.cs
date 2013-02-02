//  Copyright 2012 
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

namespace BootBaronLib.Interfaces
{
    /// <summary>
    /// Implement this to prove that this page can load a pixel and update the user 
    /// accordingly
    /// </summary>
    public interface IPixelLoadingPage
    {
        /// <summary>
        /// Specifies that the page will load the pixel
        /// </summary>
        void LoadPixel();
    }
}
