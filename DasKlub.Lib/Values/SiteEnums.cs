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

using System.ComponentModel;

namespace DasKlub.Lib.Values
{
    /// <summary>
    ///     Enumerations created for use in the site
    /// </summary>
    public static class SiteEnums
    {
        public enum AcknowledgementType
        {
            /// <summary>
            ///     Applaud
            /// </summary>
            A,

            /// <summary>
            ///     beat down
            /// </summary>
            B
        }

        public enum ApplicationVariableNames
        {
            LogError,
            ErrorCount
        }

        public enum ArtistPropertyType
        {
            /// <summary>
            ///     meta description
            /// </summary>
            MD,

            /// <summary>
            ///     long description
            /// </summary>
            LD,

            /// <summary>
            ///     photo
            /// </summary>
            PH,

            /// <summary>
            ///     thumb photo
            /// </summary>
            TH
        }

        public enum CommentStatus
        {
            /// <summary>
            ///     unconfirmed
            /// </summary>
            [Description("Unconfirmed")] U,

            /// <summary>
            ///     confirmed
            /// </summary>
            [Description("Confirmed")] C
        }

        /// <summary>
        ///     All the cookie names
        /// </summary>
        public enum CookieName
        {
            Usersetting
        }

        /// <summary>
        ///     Names of values used for cookies
        /// </summary>
        public enum CookieValue
        {
            language,
        }

        /// <summary>
        ///     All allowable country codes as ISO
        /// </summary>
        /// <see>http://www.iso.org/iso/english_country_names_and_code_elements</see>
        public enum CountryCodeISO
        {
            [Description("Unknown")] U0 = 0,
            RD,
            [Description("UnitedStates")] US,

            [Description("Australia")] AU,
            [Description("Chile")] CL,
            [Description("Colombia")] CO,
            [Description("Ecuador")] EC,

            /// <summary>
            ///     Canada
            [Description("Canada")] CA,
            [Description("NewZealand")] NZ,
            [Description("Germany")] DE,
            [Description("Italy")] IT,
            [Description("Spain")] ES,
            [Description("UnitedKingdom")] UK,
            [Description("Albania")] AL,
            [Description("Andorra")] AD,
            [Description("Armenia")] AM,
            [Description("Austria")] AT,
            [Description("Philippines")] PH,
            [Description("Belarus")] BY,
            [Description("Belgium")] BE,

            [Description("Bulgaria")] BG,
            [Description("Hungary")] HU,
            [Description("Croatia")] HR,
            [Description("Cyprus")] CY,
            [Description("CzechRepublic")] CZ,
            [Description("Denmark")] DK,
            [Description("Estonia")] EE,
            [Description("Finland")] FI,
            [Description("France")] FR,
            [Description("Georgia")] GE,
            [Description("Greece")] GR,
            [Description("Cuba")] CU,
            [Description("SouthKorea")] KR,
            [Description("NorthKorea")] KP,
            [Description("Iceland")] IS,
            [Description("Ireland")] IE,
            [Description("Latvia")] LV,
            [Description("Liechtenstein")] LI,
            [Description("Lithuania")] LT,
            [Description("Luxembourg")] LU,
            [Description("Malta")] MT,
            [Description("Monaco")] MC,
            [Description("Montenegro")] ME,
            [Description("Netherlands")] NL,
            [Description("Norway")] NO,
            [Description("Poland")] PL,
            [Description("Portugal")] PT,
            [Description("Romania")] RO,
            [Description("Russia")] RU,
            [Description("SanMarino")] SM,
            [Description("Serbia")] RS,
            [Description("Slovakia")] SK,
            [Description("Slovenia")] SI,
            [Description("Sweden")] SE,
            [Description("Switzerland")] CH,
            [Description("Turkey")] TR,
            [Description("Ukraine")] UA,
            [Description("Jersey")] JE,
            [Description("Singapore")] SG,
            [Description("India")] IN,
            [Description("Venezuela")] VE,
            [Description("Belize")] BZ,
            [Description("CostaRica")] CR,
            [Description("ElSalvador")] SV,
            [Description("Guatemala")] GT,

            [Description("Honduras")] HN,

            [Description("Ghana")] GH,
            [Description("FaroeIslands")] FO,
            [Description("Moldova")] MD,
            [Description("SouthAfrica")] ZA,
            [Description("Guam")] GU,
            [Description("Macedonia")] MK,
            [Description("PuertoRico")] PR,
            [Description("VaticanCity")] VA,
            [Description("Argentina")] AR,
            [Description("Brazil")] BR,
            [Description("DominicanRepublic")] DO,
            [Description("Greenland")] GL,

            [Description("Japan")] JP,
            [Description("Pakistan")] PK,
            [Description("AmericanSamoa")] AS,
            [Description("Guernsey")] GG,
            [Description("SriLanka")] LK,
            [Description("FrenchGuiana")] GF,
            [Description("MarshallIslands")] MH,
            [Description("Thailand")] TH,
            [Description("Mayotte")] YT,
            [Description("Guyana")] GY,
            [Description("Uruguay")] UY,
            [Description("Nicaragua")] NI,
            [Description("Suriname")] SR,
            [Description("IsleOfMan")] IM,
            [Description("Peru")] PE,
            [Description("Mexico")] MX,
            [Description("Paraguay")] PY,
            [Description("Reunion")] RE,
            [Description("VirginIslands")] VI,
            [Description("Bangladesh")] BD,
            [Description("Guadeloupe")] GP,
            [Description("Martinique")] MQ,
            [Description("Panama")] PA,
            [Description("SyrianArabRepublic")] SY,
            [Description("China")] CN,
            [Description("Iran")] IR,
            [Description("Iraq")] IQ
        }

      
       
        public enum FormContentTypes
        {
            unknown = 0,
            [Description("application/x-www-form-urlencoded")] URLENCODE,
            [Description("multipart/form-data")] MULTIPART
        }
 
        public enum HTTPTypes
        {
            unknown = 0,
            PUT,
            GET,
            POST
        }


        /// <summary>
        ///     Different types of errors related to membership login
        /// </summary>
        public enum MembershipFailureTypes
        {
            unknown = 0,
            password,
            passwordAnswer
        }


        public enum MultiPropertyType
        {
            VIDEO,
            PRODUCT
        }

        /// <summary>
        ///     Different statuses an order can be
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            ///     Unprocessed
            /// </summary>
            U,

            /// <summary>
            ///     Paid
            /// </summary>
            P,

            /// <summary>
            ///     Shipped
            /// </summary>
            S,

            /// <summary>
            ///     Refunded
            /// </summary>
            R
        }


        public enum PropertyTypeCode
        {
            UNKNO = 0,

            /// <summary>
            ///     type of video
            /// </summary>
            VIDTP,

            /// <summary>
            ///     Language
            /// </summary>
            LANGE,
            HUMAN,

            /// <summary>
            ///     Music genre
            /// </summary>
            GENRE,

            /// <summary>
            ///     Difficulty level
            /// </summary>
            DIFFC,
            FOOTG,

            /// <summary>
            ///     Guitar type
            /// </summary>
            GUITR,
            ALBUM,
            [Description("Description")] DESCR,
            SALEA,
            [Description("Sizing Details")] SIZES,

            /// <summary>
            ///     Product Video
            /// </summary>
            PRODV,

            /// <summary>
            ///     Google Taxonomy
            /// </summary>
            GOOGT,

            /// <summary>
            ///     GTIN
            /// </summary>
            GTINN,

            /// <summary>
            ///     Gender
            /// </summary>
            GENDR,

            /// <summary>
            ///     Age group
            /// </summary>
            AGEGR,

            /// <summary>
            ///     Condition
            /// </summary>
            CONDI
        }


        /// <summary>
        ///     All the query strings used in the site
        /// </summary>
        public enum QueryStringNames
        {
            language,
            videoType,
            footageType,
            personType,
            stat_update_comment_rsp,
            most_applauded_status_update_id,
            comment_page,
            video,
            act_type,
            status_com_id,
            comment_msg,
            all_comments,
            status_update,
            param_type,
            stat_update_rsp,
 
            status_update_id,
            video_playlist,
 
            menu,
            begin_playlist,
 
            currentvidid,
 
            playlist,
            country_iso,
            region,
            city,
 
            begindate,
 
            random,
            vid,
 
            pg,
 
            email
        }

        public enum ResponseType
        {
            /// <summary>
            ///     unknown
            /// </summary>
            U,

            /// <summary>
            ///     Comment
            /// </summary>
            C,

            /// <summary>
            ///     Applaud
            /// </summary>
            A,

            /// <summary>
            ///     beat down
            /// </summary>
            B,

            /// <summary>
            ///     Delete Applaud
            /// </summary>
            D,

            /// <summary>
            ///     Delete Beatdown
            /// </summary>
            E
        }

        /// <summary>
        ///     Roles that users who log into the
        ///     this system can belong to
        /// </summary>
        public enum RoleTypes
        {
            Unknown = 0,
            cyber_girl,
            admin,
            supporter 
        }

      
        public enum SiteBrandType
        {
            /// <summary>
            /// User writing invite
            /// </summary>
            UWRIT,
            /// <summary>
            ///     Script at the bottom of the homepage
            /// </summary>
            HOMEB,
            /// <summary>
            /// Mission statement
            /// </summary>
            MISSN,
            /// <summary>
            ///     Business address
            /// </summary>
            ADDRS,

            /// <summary>
            ///     Status update side message
            /// </summary>
            STATU,

            /// <summary>
            ///     homepage script
            /// </summary>
            HOMES,

            /// <summary>
            ///     Terms and condtions
            /// </summary>
            TERMS,

            /// <summary>
            ///     Privacy policy
            /// </summary>
            PRIPL,

            /// <summary>
            ///     About us
            /// </summary>
            ABOUT,

            /// <summary>
            ///     Home featured user
            /// </summary>
            HOMEF,

            /// <summary>
            ///     Home center/ canvas
            /// </summary>
            HOMEC,
            /// <summary>
            ///     Home title
            /// </summary>
            HOMET,
            /// <summary>
            ///     Radio stations -desktop
            /// </summary>
            RADIO,

            /// <summary>
            ///     Radio stations mobile
            /// </summary>
            RADIM,

            /// <summary>
            ///     Site tag line
            /// </summary>
            TAGLN,

            /// <summary>
            ///     CSS style
            /// </summary>
            STYLE,

            /// <summary>
            ///     Main store
            /// </summary>
            STORE,

            /// <summary>
            ///     To the side of the store
            /// </summary>
            STRSD,

            /// <summary>
            ///     Google analytics script head
            /// </summary>
            GOOGH,

            /// <summary>
            ///     Google analytics script body
            /// </summary>
            GOOGB,

            /// <summary>
            ///     Google search
            /// </summary>
            GOOGS,

            /// <summary>
            ///     486 x 60 ad 1/3
            /// </summary>
            A4861,

            /// <summary>
            ///     486 x 60 ad 1/3
            /// </summary>
            A4862,

            /// <summary>
            ///     486 x 60 ad 1/3
            /// </summary>
            A4863,

            /// <summary>
            ///     160 x 600 ad 1/3
            /// </summary>
            A1601,

            /// <summary>
            ///     160 x 600 ad 2/3
            /// </summary>
            A1602,

            /// <summary>
            ///     160 x 600 ad 3/2
            /// </summary>
            A1603,

            /// <summary>
            ///     120 x 60 ad 1/3
            /// </summary>
            A1201,

            /// <summary>
            ///     120 x 60 ad 2/3
            /// </summary>
            A1202,

            /// <summary>
            ///     120 x 60 ad 3/2
            /// </summary>
            A1203,

            /// <summary>
            ///     234 x 60 ad 1/3
            /// </summary>
            A2341,

            /// <summary>
            ///     234 x 60 ad 2/3
            /// </summary>
            A2342,

            /// <summary>
            ///     234 x 60 ad 3/2
            /// </summary>
            A2343,

            /// <summary>
            ///     728x90 ad 3/2
            /// </summary>
            A7281,

            /// <summary>
            ///     728x90 ad 2/3
            /// </summary>
            A7282,

            /// <summary>
            ///     728x90 ad 3/2
            /// </summary>
            A7283,
            /// <summary>
            /// 300 x 250 (TODO: CHANGE KEY AND SITE DB ENTRY)
            /// </summary>
            A2501,
            /// <summary>
            /// 728x90
            /// </summary>
            A728
        }


        public enum SiteLanguages
        {
            /// <summary>
            ///     English
            /// </summary>
            [Description("English")] EN,
            ///// <summary>
            ///// Turkish (TODO: RESOLVE STRANGE ISSUE WITH LETTER I AND OTHERS)
            ///// </summary>
            //[Description("Turkish")]
            //TR,
            /// <summary>
            ///     Arabic
            /// </summary>
            [Description("Arabic")] AR,

            /// <summary>
            ///     Chinese
            /// </summary>
            [Description("Chinese")] ZH,

            /// <summary>
            ///     Danish
            /// </summary>
            [Description("Danish")] DA,

            /// <summary>
            ///     Hungarian
            /// </summary>
            [Description("Hungarian")] HU,

            /// <summary>
            ///     Czech
            /// </summary>
            [Description("Czech")] CS,

            /// <summary>
            ///     Finnish
            /// </summary>
            [Description("Finnish")] FI,

            /// <summary>
            ///     Korean
            /// </summary>
            [Description("Korean")] KO,

            /// <summary>
            ///     Greek
            /// </summary>
            [Description("Greek")] EL,

            /// <summary>
            ///     Swedish
            /// </summary>
            [Description("Swedish")] SV,

            /// <summary>
            ///     Japanese
            /// </summary>
            [Description("Japanese")] JA,

            /// <summary>
            ///     Dutch
            /// </summary>
            [Description("Dutch")] NL,

            /// <summary>
            ///     German
            /// </summary>
            [Description("German")] DE,

            /// <summary>
            ///     German
            /// </summary>
            [Description("Italian")] IT,

            /// <summary>
            ///     Spanish
            /// </summary>
            [Description("Spanish")] ES,

            /// <summary>
            ///     Russian
            /// </summary>
            [Description("Russian")] RU,

            /// <summary>
            ///     French
            /// </summary>
            [Description("French")] FR,

            /// <summary>
            ///     Portuguese
            /// </summary>
            [Description("Portuguese")] PT,

            /// <summary>
            ///     German
            /// </summary>
            [Description("Polish")] PL
            ///// <summary>
            ///// Latin (it's really LA but it's not supported)
            ///// </summary>
            //[Description("Latin")]
            //FO
        }

        public enum SubContest
        {
            /// <summary>
            ///     Unknown
            /// </summary>
            U,

            /// <summary>
            ///     Overall Vote
            /// </summary>
            O,

            /// <summary>
            ///     Judges Choice
            /// </summary>
            J,

            /// <summary>
            ///     Single male
            /// </summary>
            M,

            /// <summary>
            ///     Single Female
            /// </summary>
            F,

            /// <summary>
            ///     Female & Female
            /// </summary>
            D,

            /// <summary>
            ///     Male & Male
            /// </summary>
            W,

            /// <summary>
            ///     Male & Female
            /// </summary>
            C,

            /// <summary>
            ///     Group
            /// </summary>
            G,

            /// <summary>
            ///     Band Selection
            /// </summary>
            B
        }
    }
}