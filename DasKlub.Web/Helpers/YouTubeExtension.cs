using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DasKlub.Web.Helpers
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public class entry
    {
        private rating _rating;
        private entryAuthor[] authorField;
        private entryCategory[] categoryField;

        private entryContent[] contentField;
        private string idField;

        private entryLink[] linkField;
        private string publishedField;
        private string titleField;
        private string updatedField;
        [XmlNamespaceDeclarations] public XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();

        public entry()
        {
            xmlns.Add("xmlns", "http://www.w3.org/2005/Atom");
            xmlns.Add("media", "http://search.yahoo.com/mrss/");
            xmlns.Add("gd", "http://schemas.google.com/g/2005");
            xmlns.Add("yt", "http://gdata.youtube.com/schemas/2007'");
        }


        /// <remarks />
        public string id
        {
            get { return idField; }
            set { idField = value; }
        }

        /// <remarks />
        public string published
        {
            get { return publishedField; }
            set { publishedField = value; }
        }

        /// <remarks />
        public string updated
        {
            get { return updatedField; }
            set { updatedField = value; }
        }

        /// <remarks />
        public string title
        {
            get { return titleField; }
            set { titleField = value; }
        }

        /// <remarks />
        [XmlElement("category")]
        public entryCategory[] category
        {
            get { return categoryField; }
            set { categoryField = value; }
        }

        /// <remarks />
        [XmlElement("content")]
        public entryContent[] content
        {
            get { return contentField; }
            set { contentField = value; }
        }

        /// <remarks />
        [XmlElement("link")]
        public entryLink[] link
        {
            get { return linkField; }
            set { linkField = value; }
        }

        /// <remarks />
        [XmlElement("author")]
        public entryAuthor[] author
        {
            get { return authorField; }
            set { authorField = value; }
        }


        /// <remarks />
        [XmlElement("rating")]
        public rating rating
        {
            get { return _rating; }
            set { _rating = value; }
        }
    }


    [Serializable]
    [XmlType(AnonymousType = true, Namespace =
        "http://gdata.youtube.com/schemas/2007")]
    [XmlRoot(Namespace = "http://gdata.youtube.com/schemas/2007",
        IsNullable = false)]
    public class rating
    {
        private string numDislikesField;

        private string numLikesField;

        /// <remarks />
        [XmlAttribute]
        public string numDislikes
        {
            get { return numDislikesField; }
            set { numDislikesField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string numLikes
        {
            get { return numLikesField; }
            set { numLikesField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryCategory
    {
        private string labelField;
        private string schemeField;

        private string termField;

        /// <remarks />
        [XmlAttribute]
        public string scheme
        {
            get { return schemeField; }
            set { schemeField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string term
        {
            get { return termField; }
            set { termField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string label
        {
            get { return labelField; }
            set { labelField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryContent
    {
        private string srcField;
        private string typeField;

        /// <remarks />
        [XmlAttribute]
        public string type
        {
            get { return typeField; }
            set { typeField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string src
        {
            get { return srcField; }
            set { srcField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryLink
    {
        private string hrefField;
        private string relField;

        private string typeField;

        /// <remarks />
        [XmlAttribute]
        public string rel
        {
            get { return relField; }
            set { relField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string type
        {
            get { return typeField; }
            set { typeField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string href
        {
            get { return hrefField; }
            set { hrefField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    public class entryAuthor
    {
        private string nameField;

        private string uriField;

        /// <remarks />
        public string name
        {
            get { return nameField; }
            set { nameField = value; }
        }

        /// <remarks />
        public string uri
        {
            get { return uriField; }
            set { uriField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
    [XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
    public class NewDataSet
    {
        private entry[] itemsField;

        /// <remarks />
        [XmlElement("entry")]
        public entry[] Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }
    }


    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.google.com/g/2005")]
    [XmlRoot(Namespace = "http://schemas.google.com/g/2005", IsNullable = false)]
    public class comments
    {
        private commentsFeedLink[] feedLinkField;

        /// <remarks />
        [XmlElement("feedLink")]
        public commentsFeedLink[] feedLink
        {
            get { return feedLinkField; }
            set { feedLinkField = value; }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.google.com/g/2005")]
    public class commentsFeedLink
    {
        private string countHintField;
        private string hrefField;

        /// <remarks />
        [XmlAttribute]
        public string href
        {
            get { return hrefField; }
            set { hrefField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string countHint
        {
            get { return countHintField; }
            set { countHintField = value; }
        }
    }


    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://gdata.youtube.com/schemas/2007")]
    [XmlRoot(Namespace = "http://gdata.youtube.com/schemas/2007", IsNullable = false)]
    public class accessControl
    {
        private string actionField;

        private string permissionField;

        /// <remarks />
        [XmlAttribute]
        public string action
        {
            get { return actionField; }
            set { actionField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string permission
        {
            get { return permissionField; }
            set { permissionField = value; }
        }
    }

    /// <remarks />
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://gdata.youtube.com/schemas/2007")]
    [XmlRoot(Namespace = "http://gdata.youtube.com/schemas/2007", IsNullable = false)]
    public class duration
    {
        private string secondsField;

        /// <remarks />
        [XmlAttribute]
        public string seconds
        {
            get { return secondsField; }
            set { secondsField = value; }
        }
    }

    /// <remarks />
    [GeneratedCode("xsd", "4.0.30319.1")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://gdata.youtube.com/schemas/2007")]
    [XmlRoot(Namespace = "http://gdata.youtube.com/schemas/2007", IsNullable = false)]
    public class statistics
    {
        private string favoriteCountField;

        private string viewCountField;

        /// <remarks />
        [XmlAttribute]
        public string favoriteCount
        {
            get { return favoriteCountField; }
            set { favoriteCountField = value; }
        }

        /// <remarks />
        [XmlAttribute]
        public string viewCount
        {
            get { return viewCountField; }
            set { viewCountField = value; }
        }
    }
}