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
using System.Data;

namespace BootBaronLib.Interfaces
{
    /// <summary>
    /// Implementing this means that the object outputs a ul with li's 
    /// </summary>
    public interface IUnorderdList 
    {
        /// <summary>
        /// When this is set to true the items are completed with a ul open close
        /// otherwise they are just li's
        /// </summary>
        bool IncludeStartAndEndTags { get; set; }

        string ToUnorderdList {get;}
    }
}
