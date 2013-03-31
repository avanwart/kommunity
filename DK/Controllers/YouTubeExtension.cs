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

using System.Xml.Serialization;

namespace DasKlub.Controllers
{

    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public partial class entry
    {

        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public entry()
        {

            //xmlns.Add(@"msdata", @"urn:schemas-microsoft-com:xml-msdata");
            xmlns.Add("xmlns", "http://www.w3.org/2005/Atom");
            xmlns.Add("media", "http://search.yahoo.com/mrss/");
            xmlns.Add("gd", "http://schemas.google.com/g/2005");
            xmlns.Add("yt", "http://gdata.youtube.com/schemas/2007'");
 
         

        }


        private string idField;

        private string publishedField;

        private string updatedField;

        private string titleField;

        private entryCategory[] categoryField;

        private entryContent[] contentField;

        private entryLink[] linkField;

        private entryAuthor[] authorField;


      

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string published
        {
            get
            {
                return this.publishedField;
            }
            set
            {
                this.publishedField = value;
            }
        }

        /// <remarks/>
        public string updated
        {
            get
            {
                return this.updatedField;
            }
            set
            {
                this.updatedField = value;
            }
        }

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [XmlElement("category")]
        public entryCategory[] category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        [XmlElement("content")]
        public entryContent[] content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }

        /// <remarks/>
        [XmlElement("link")]
        public entryLink[] link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        [XmlElement("author")]
        public entryAuthor[] author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }




        private rating _rating;

        /// <remarks/>
        [XmlElement("rating")]
        public rating rating
        {
            get
            {
                return this._rating;
            }
            set
            {
                this._rating = value;
            }
        }
    }


 
    [System.SerializableAttribute()]
    [XmlType(AnonymousType = true, Namespace =
        "http://gdata.youtube.com/schemas/2007")]
    [XmlRoot(Namespace = "http://gdata.youtube.com/schemas/2007", 
        IsNullable = false)]
    public partial class rating
    {

        private string numDislikesField;

        private string numLikesField;

        /// <remarks/>
        [XmlAttribute()]
        public string numDislikes
        {
            get
            {
                return this.numDislikesField;
            }
            set
            {
                this.numDislikesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string numLikes
        {
            get
            {
                return this.numLikesField;
            }
            set
            {
                this.numLikesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class entryCategory
    {

        private string schemeField;

        private string termField;

        private string labelField;

        /// <remarks/>
        [XmlAttribute()]
        public string scheme
        {
            get
            {
                return this.schemeField;
            }
            set
            {
                this.schemeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string term
        {
            get
            {
                return this.termField;
            }
            set
            {
                this.termField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class entryContent
    {

        private string typeField;

        private string srcField;

        /// <remarks/>
        [XmlAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string src
        {
            get
            {
                return this.srcField;
            }
            set
            {
                this.srcField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class entryLink
    {

        private string relField;

        private string typeField;

        private string hrefField;

        /// <remarks/>
        [XmlAttribute()]
        public string rel
        {
            get
            {
                return this.relField;
            }
            set
            {
                this.relField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public partial class entryAuthor
    {

        private string nameField;

        private string uriField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public partial class NewDataSet
    {

        private entry[] itemsField;

        /// <remarks/>
        [XmlElement("entry")]
        public entry[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }


     


    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.google.com/g/2005")]
    [XmlRoot(Namespace = "http://schemas.google.com/g/2005", IsNullable = false)]
    public partial class comments
    {

        private commentsFeedLink[] feedLinkField;

        /// <remarks/>
        [XmlElement("feedLink")]
        public commentsFeedLink[] feedLink
        {
            get
            {
                return this.feedLinkField;
            }
            set
            {
                this.feedLinkField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.google.com/g/2005")]
    public partial class commentsFeedLink
    {

        private string hrefField;

        private string countHintField;

        /// <remarks/>
        [XmlAttribute()]
        public string href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countHint
        {
            get
            {
                return this.countHintField;
            }
            set
            {
                this.countHintField = value;
            }
        }
    }



     

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://gdata.youtube.com/schemas/2007")]
    [XmlRoot(Namespace = "http://gdata.youtube.com/schemas/2007", IsNullable = false)]
    public partial class accessControl
    {

        private string actionField;

        private string permissionField;

        /// <remarks/>
        [XmlAttribute()]
        public string action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string permission
        {
            get
            {
                return this.permissionField;
            }
            set
            {
                this.permissionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://gdata.youtube.com/schemas/2007")]
    [XmlRoot(Namespace = "http://gdata.youtube.com/schemas/2007", IsNullable = false)]
    public partial class duration
    {

        private string secondsField;

        /// <remarks/>
        [XmlAttribute()]
        public string seconds
        {
            get
            {
                return this.secondsField;
            }
            set
            {
                this.secondsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://gdata.youtube.com/schemas/2007")]
    [XmlRoot(Namespace = "http://gdata.youtube.com/schemas/2007", IsNullable = false)]
    public partial class statistics
    {

        private string favoriteCountField;

        private string viewCountField;

        /// <remarks/>
        [XmlAttribute()]
        public string favoriteCount
        {
            get
            {
                return this.favoriteCountField;
            }
            set
            {
                this.favoriteCountField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string viewCount
        {
            get
            {
                return this.viewCountField;
            }
            set
            {
                this.viewCountField = value;
            }
        }
    }




}
