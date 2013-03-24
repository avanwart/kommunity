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

namespace BootBaronLib.Enums
{
    /// <summary>
    /// Enumerations created for use in the site
    /// </summary>
    public class SiteEnums
    {

        public enum SiteBrandType
        {
            
            /// <summary>
            /// Script at the bottom of the homepage
            /// </summary>
            HOMEB,
            /// <summary>
            /// Business address
            /// </summary>
            ADDRS,
            /// <summary>
            /// Status update side message
            /// </summary>
            STATU,
            /// <summary>
            /// homepage script
            /// </summary>
            HOMES,
            /// <summary>
            /// Terms and condtions
            /// </summary>
            TERMS,
            /// <summary>
            /// Privacy policy
            /// </summary>
            PRIPL,
            /// <summary>
            /// About us
            /// </summary>
            ABOUT,
            /// <summary>
            /// Home featured user
            /// </summary>
            HOMEF,
            /// <summary>
            /// Home center/ canvas
            /// </summary>
            HOMEC,
            /// <summary>
            /// Radio stations -desktop
            /// </summary>
            RADIO,
            /// <summary>
            /// Radio stations mobile
            /// </summary>
            RADIM,
            /// <summary>
            /// Site tag line
            /// </summary>
            TAGLN,
            /// <summary>
            /// CSS style
            /// </summary>
            STYLE,
            /// <summary>
            /// Main store
            /// </summary>
            STORE,
            /// <summary>
            /// To the side of the store
            /// </summary>
            STRSD,
            /// <summary>
            /// Google analytics script head
            /// </summary>
            GOOGH,
            /// <summary>
            /// Google analytics script body
            /// </summary>
            GOOGB,
            /// <summary>
            /// Google search
            /// </summary>
            GOOGS,
            /// <summary>
            /// 486 x 60 ad 1/3
            /// </summary>
            A4861,
            /// <summary>
            /// 486 x 60 ad 1/3
            /// </summary>
            A4862,
            /// <summary>
            /// 486 x 60 ad 1/3
            /// </summary>
            A4863,
            /// <summary>
            ///  160 x 600 ad 1/3
            /// </summary>
            A1601,
            /// <summary>
            ///  160 x 600 ad 2/3
            /// </summary>
            A1602,
            /// <summary>
            ///  160 x 600 ad 3/2
            /// </summary>
            A1603,
            /// <summary>
            ///  120 x 60 ad 1/3
            /// </summary>
            A1201,
            /// <summary>
            ///   120 x 60 ad 2/3
            /// </summary>
            A1202,
            /// <summary>
            ///   120 x 60 ad 3/2
            /// </summary>
            A1203,
            /// <summary>
            ///  234 x 60 ad 1/3
            /// </summary>
            A2341,
            /// <summary>
            ///   234 x 60 ad 2/3
            /// </summary>
            A2342,
            /// <summary>
            ///   234 x 60 ad 3/2
            /// </summary>
            A2343,
            /// <summary>
            ///   728x90 ad 3/2
            /// </summary>
            A7281,
            /// <summary>
            ///  728x90 ad 2/3
            /// </summary>
            A7282,
            /// <summary>
            ///  728x90 ad 3/2
            /// </summary>
            A7283,

            
        }



        public enum CommentStatus
        {
            /// <summary>
            /// unconfirmed
            /// </summary>
            [Description("Unconfirmed")]
            U,
            /// <summary>
            /// confirmed
            /// </summary>
            [Description("Confirmed")]
            C
        }


        public enum SubContest
        {
            /// <summary>
            /// Unknown
            /// </summary>
            U,
            /// <summary>
            /// Overall Vote
            /// </summary>
            O,
            /// <summary>
            /// Judges Choice
            /// </summary>
            J,
            /// <summary>
            /// Single male
            /// </summary>
            M,
            /// <summary>
            /// Single Female
            /// </summary>
            F,
            /// <summary>
            /// Female & Female
            /// </summary>
            D,
            /// <summary>
            /// Male & Male
            /// </summary>
            W,
            /// <summary>
            /// Male & Female
            /// </summary>
            C,
            /// <summary>
            /// Group
            /// </summary>
            G,
            /// <summary>
            /// Band Selection
            /// </summary>
            B

        }

        /// <summary>
        /// Different statuses an order can be
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            /// Unprocessed
            /// </summary>
            U,
            /// <summary>
            /// Paid
            /// </summary>
            /// 
            P,
            /// <summary>
            /// Shipped
            /// </summary>
            S,
            /// <summary>
            /// Refunded
            /// </summary>
            R
        }


     
      

        public enum ArtistPropertyType
        {
            /// <summary>
            ///  meta description
            /// </summary>
            MD,
            /// <summary>
            /// long description
            /// </summary>
            LD,
            /// <summary>
            /// photo
            /// </summary>
            PH,
            /// <summary>
            /// thumb photo
            /// </summary>
            TH
        }

        public enum MultiPropertyType
        {
            VIDEO,
            PRODUCT

        }

       


        /// <summary>
        /// Different types of errors related to membership login
        /// </summary>
        public enum MembershipFailureTypes
        {
            unknown = 0,
            password,
            passwordAnswer
        }


        public enum MultiPropertyNames
        {
            [Description("Male")]
            Male,
            [Description("Female")]
            Female,
            [Description("Male Male")]
            MaleMale,
            [Description("Female Female")]
            FemaleFemale,
            [Description("Male Female")]
            MaleFemale,
            [Description("Dance")]
            Dance,
            [Description("Music")]
            Music,
            [Description("Live")]
            Live,
            [Description("Image")]
            Image,
            [Description("Raw")]
            Raw,
            [Description("Edited")]
            Edited,
            [Description("Tutorial")]
            Tutorial,
            [Description("Robot")]
            Robot,
            [Description("Unknown?")]
            Unknown,
            [Description("Interview")]
            Interview,
            [Description("Show")]
            Show
        }
        public enum PhotoSizeType : short
        {

            /// <summary>
            /// unknown
            /// </summary>
            U = 0,

            /// <summary>
            /// Thumb nail on product page 75 x 100 image  
            /// </summary>
            Smallest = 1,
            /// <summary>
            /// Thumbnail in list 112 x 150 Image thumb 
            /// </summary>
            Small = 2,
            /// <summary>
            /// Main product display 300 x 400 main
            /// </summary>
            Medium = 3,
            /// <summary>
            /// Full size  600 x 800 image
            /// </summary>
            Large = 4
        }

     


        public enum PropertyTypeCode
        {
            UNKNO = 0,
            /// <summary>
            /// type of video
            /// </summary>
            VIDTP,
            /// <summary>
            /// Language
            /// </summary>
            LANGE,
            HUMAN,
            /// <summary>
            /// Music genre
            /// </summary>
            GENRE,
            /// <summary>
            /// Difficulty level
            /// </summary>
            DIFFC,
            FOOTG,
            /// <summary>
            /// Guitar type
            /// </summary>
            GUITR,
            ALBUM,
            [Description("Description")]
            DESCR,
            SALEA,
            [Description("Sizing Details")]
            SIZES,
            /// <summary>
            /// Product Video
            /// </summary>
            PRODV,
            /// <summary>
            /// Google Taxonomy
            /// </summary>
            GOOGT,
            /// <summary>
            /// GTIN
            /// </summary>
            GTINN,
            /// <summary>
            /// Gender
            /// </summary>
            GENDR,
            /// <summary>
            /// Age group
            /// </summary>
            AGEGR,
            /// <summary>
            /// Condition
            /// </summary>
            CONDI

        }


 


        public enum ApplicationVariableNames
        {
            unknown = 0,
            logError,
            errorCount
        }

   



        /// <summary>
        /// ISO currency
        /// </summary>
        public enum CurrencyTypes
        {
            /// <summary>
            /// Unknown currency
            /// </summary>
            UNK = 0,
            /// <summary>
            /// Brazilian Real
            /// </summary>
            [Description ("R$")]
            BRL,
            /// <summary>
            /// Mexican Peso
            /// </summary>
            [Description("$")]
            MXN,
            /// <summary>
            /// United States Dollars
            /// </summary>
            [Description("$")]
            USD,
            /// <summary>
            /// Canadian Dollar
            /// </summary>
            [Description("$")]
            CAD,
            /// <summary>
            /// Australian Dollar
            /// </summary>
            [Description("$")]
            AUD,
            /// <summary>
            /// Euro
            /// </summary>
            [Description("€")]
            EUR,
            /// <summary>
            /// Great British Pound
            /// </summary>
            [Description("£")]
            GBP
        }

    
  


        public enum FormContentTypes
        {
            unknown = 0,
            [Description("application/x-www-form-urlencoded")]
            URLENCODE,
            [Description("multipart/form-data")]
            MULTIPART
        }

        public enum HTTPTypes
        {
            unknown = 0,
            PUT,
            GET, 
            POST
        }

        /// <summary>
        /// Roles that users who log into the 
        /// this system can belong to
        /// </summary>
        public enum RoleTypes
        {
            unknown = 0,
            cyber_girl,
            admin,
            judge
        }

 

        /// <summary>
        /// Billing = B, Shipping = S, Home = H
        /// </summary>
        public enum AddressType
        {
            unknown = 0, 
            /// <summary>
            /// Billing address
            /// </summary>
            B, 
            /// <summary>
            /// Shipping address
            /// </summary>
            S, 
            /// <summary>
            /// Home address
            /// </summary>
            H,
            /// <summary>
            /// Operating
            /// </summary>
            O
        }
 
 


        /// <summary>
        /// All the query strings used in the site
        /// </summary>
        public enum QueryStringNames
        {
            unknown = 0,
            genreType,
            language,
            videoType,
            footageType,
            personType,
            difficultyLevel,
            stat_update_comment_rsp,
            most_applauded_status_update_id,
            comment_page,
            video,
            act_type,
            guitarType,
            vid_request,
            status_com_id,
            comment_msg,
            all_comments,
            status_update,
            param_type,
            stat_update_rsp,
            chat_message,
            chat_msg,
            status_update_id,
            video_playlist,
            exit_room,
            chat_user_list,
            menu,
            begin_playlist,
            cat,
            currentvidid,
            srt,
            next,
            playlist,
            country_iso,
            region,
            city,
            event_day,
            begindate,
            enddate,
            chat,
            user,
            random,
            vid,
            errorlist,
            historyid,
            folder,
            filepath,
            key,
            /// <summary>
            /// Preview
            /// </summary>
            test,
            /// <summary>
            /// Page Info ID
            /// </summary>
            id,
            /// <summary>
            /// The service option key
            /// </summary>
            sok,
            /// <summary>
            /// Is this a downsell?
            /// </summary>
            isdsale,
            pg,
            /// <summary>
            /// The request to obtain a domain
            /// </summary>
            obtain,
            /// <summary>
            /// The action being performed
            /// </summary>
            action,
            /// <summary>
            /// ContactUserID in a system
            /// </summary>
            cuid,

            /// <summary>
            /// Requested domain name,
            /// </summary>
            domain,
            /// <summary>
            /// An order id (intended for use with web builder)
            /// </summary>
            order_id,
            /// <summary>
            /// The type of documentation
            /// </summary>
            doctype,

            dco,
            /// <summary>
            /// The item code see: ItemCode enum
            /// </summary>
            itm,
            /// <summary>
            /// does this exit (0 sets no downsell)
            /// </summary>
            ex,
            /// <summary>
            /// The version number
            /// </summary>
            vn,

            /// <summary>
            /// subid1
            /// </summary>
            s1,
            /// <summary>
            /// subid2
            /// </summary>
            s2,
            /// <summary>
            /// subid3
            /// </summary>
            s3,
            s4,
            /// <summary>
            /// clickid
            /// </summary>
            clickid,
            /// <summary>
            /// Item property
            /// </summary>
            itmprop,
            result,
            /// <summary>
            /// A URL to redirect the user to
            /// </summary>
            rurl,
            type,
            group,
            show,
            /// <summary>
            /// 1 enables the user who signed up to go to the upsell
            /// </summary>
            upsell,
            /// <summary>
            /// The ID of the user
            /// </summary>
            contactuserid,
            info,
            request,
            email
        }




        public enum SignUpVars
        {
            unknown = 0,
            /// <summary>
            /// itm
            /// </summary>
            prod,
            /// <summary>
            /// vn
            /// </summary>
            pgv,
            a1,
            a2,
            a3,
            a4,
            cid,
            /// <summary>
            /// Group
            /// </summary>
            lid,
            ex,
            sok,
            rurl,
            isdsale,
            urlprev,
            contactuserid,
            urlid,
            /// <summary>
            /// Item property
            /// </summary>
            nametype,
            /// <summary>
            /// Downsell ID variable for url 
            /// </summary>
            xurl
        }





        /// <summary>
        /// Names of values requested from form posts
        /// </summary>
        public enum FormRequestValues
        {
            unknown = 0,
            username,
            stateSelect,
            countrySelect,
            cuID,
            typeId,
            itm,
            type,
            isSale,
            email,
            password,
            companyCode,
            companyKey,
            commentCode,
            subid,
            subid2,
            subid3,
            cc_number, 
            exp_mm, 
            exp_yyyy, 
            cvv,
            commentDesc,
            /// <summary>
            /// should be: ISO 8601, see: http://en.wikipedia.org/wiki/ISO_8601
            /// </summary>
            createDate,
            firstName,
            lastName,
            phone,
            streetAddress,
            postalCode,
            countryCodeISO,
            ipAddress,
            regionCode,
            cityName,
            isCustomer,
            addressline1,
            addressline2


        }

      

 
 
        /// <summary>
        /// All allowable country codes as ISO
        /// </summary>
        /// <see>http://www.iso.org/iso/english_country_names_and_code_elements</see>
        public enum CountryCodeISO
        {
            [Description("Unknown")]
            U0 = 0,
            RD,
            [Description("UnitedStates")]
            US,

            [Description("Australia")]
            AU,
            [Description("Chile")]
            CL,
            [Description("Colombia")]
            CO,
            [Description("Ecuador")]
            EC,
            /// <summary>
            /// Canada

            [Description("Canada")]
            CA,
            [Description("NewZealand")]
            NZ,
            [Description("Germany")]
            DE,
            [Description("Italy")]
            IT,
            [Description("Spain")]
            ES,
            [Description("UnitedKingdom")]
            UK,
            [Description("Albania")]
            AL,
            [Description("Andorra")]
            AD,
            [Description("Armenia")]
            AM,
            [Description("Austria")]
            AT,
            [Description("Philippines")]
            PH,
            [Description("Belarus")]
            BY,
            [Description("Belgium")]
            BE,
      
            [Description("Bulgaria")]
            BG,
            [Description("Hungary")]
            HU,
            [Description("Croatia")]
            HR,
            [Description("Cyprus")]
            CY,
            [Description("CzechRepublic")]
            CZ,
            [Description("Denmark")]
            DK,
            [Description("Estonia")]
            EE,
            [Description("Finland")]
            FI,
            [Description("France")]
            FR,
            [Description("Georgia")]
            GE,
            [Description("Greece")]
            GR,
            [Description("Cuba")]
            CU,
            [Description("SouthKorea")]
            KR,
            [Description("NorthKorea")]
            KP,
            [Description("Iceland")]
            IS,
            [Description("Ireland")]
            IE,
            [Description("Latvia")]
            LV,
            [Description("Liechtenstein")]
            LI,
            [Description("Lithuania")]
            LT,
            [Description("Luxembourg")]
            LU,
            [Description("Malta")]
            MT,
            [Description("Monaco")]
            MC,
            [Description("Montenegro")]
            ME,
            [Description("Netherlands")]
            NL,
            [Description("Norway")]
            NO,
            [Description("Poland")]
            PL,
            [Description("Portugal")]
            PT,
            [Description("Romania")]
            RO,
            [Description("Russia")]
            RU,
            [Description("SanMarino")]
            SM,
            [Description("Serbia")]
            RS,
            [Description("Slovakia")]
            SK,
            [Description("Slovenia")]
            SI,
            [Description("Sweden")]
            SE,
            [Description("Switzerland")]
            CH,
            [Description("Turkey")]
            TR,
            [Description("Ukraine")]
            UA,
            [Description("Jersey")]
            JE,
            [Description("Singapore")]
            SG,
            [Description("India")]
            IN,
            [Description("Venezuela")]
            VE,
            [Description("Belize")]
            BZ,
            [Description("CostaRica")]
            CR,
            [Description("ElSalvador")]
            SV,
            [Description("Guatemala")]
            GT,

            [Description("Honduras")]
            HN,

            [Description("Ghana")]
            GH,
            [Description("FaroeIslands")]
            FO,
            [Description("Moldova")]
            MD,
            [Description("SouthAfrica")]
            ZA,
            [Description("Guam")]
            GU,
            [Description("Macedonia")]
            MK,
            [Description("PuertoRico")]
            PR,
            [Description("VaticanCity")]
            VA,
            [Description("Argentina")]
            AR,
            [Description("Brazil")]
            BR,
            [Description("DominicanRepublic")]
            DO,
            [Description("Greenland")]
            GL,
            
            [Description("Japan")]
            JP,
            [Description("Pakistan")]
            PK,
            [Description("AmericanSamoa")]
            AS,
            [Description("Guernsey")]
            GG,
            [Description("SriLanka")]
            LK,
            [Description("FrenchGuiana")]
            GF,
            [Description("MarshallIslands")]
            MH,
            [Description("Thailand")]
            TH,
            [Description("Mayotte")]
            YT,
            [Description("Guyana")]
            GY,
            [Description("Uruguay")]
            UY,
            [Description("Nicaragua")]
            NI,
            [Description("Suriname")]
            SR,
            [Description("IsleOfMan")]
            IM,
            [Description("Peru")]
            PE,
            [Description("Mexico")]
            MX,
            [Description("Paraguay")]
            PY,
            [Description("Reunion")]
            RE,
            [Description("VirginIslands")]
            VI,
            [Description("Bangladesh")]
            BD,
            [Description("Guadeloupe")]
            GP,
            [Description("Martinique")]
            MQ,
            [Description("Panama")]
            PA,
            [Description("SyrianArabRepublic")]
            SY,
            [Description("China")]
            CN,
            [Description("Iran")]
            IR,
            [Description("Iraq")]
            IQ
        }


        public enum SiteLanguages
        {
            /// <summary>
            /// English
            /// </summary>
            [Description("English")]
            EN,
            ///// <summary>
            ///// Turkish (TODO: RESOLVE STRANGE ISSUE WITH LETTER I AND OTHERS)
            ///// </summary>
            //[Description("Turkish")]
            //TR,
            /// <summary>
            /// Arabic
            /// </summary>
            [Description("Arabic")]
            AR,
            /// <summary>
            /// Chinese
            /// </summary>
            [Description("Chinese")]
            ZH,
            /// <summary>
            /// Danish
            /// </summary>
            [Description("Danish")]
            DA,
            /// <summary>
            /// Hungarian
            /// </summary>
            [Description("Hungarian")]
            HU,
            /// <summary>
            /// Czech
            /// </summary>
            [Description("Czech")]
            CS,
            /// <summary>
            /// Finnish
            /// </summary>
            [Description("Finnish")]
            FI,
            /// <summary>
            /// Korean
            /// </summary>
            [Description("Korean")]
            KO,
            /// <summary>
            /// Greek
            /// </summary>
            [Description("Greek")]
            EL,
            /// <summary>
            /// Swedish
            /// </summary>
            [Description("Swedish")]
            SV,
            /// <summary>
            /// Japanese
            /// </summary>
            [Description("Japanese")]
            JA,
            /// <summary>
            /// Dutch
            /// </summary>
            [Description("Dutch")]
            NL,
            /// <summary>
            /// German
            /// </summary>
            [Description("German")]
            DE,
            /// <summary>
            /// German
            /// </summary>
            [Description("Italian")]
            IT,
            /// <summary>
            /// Spanish
            /// </summary>
            [Description("Spanish")]
            ES,
            /// <summary>
            /// Russian
            /// </summary>
            [Description("Russian")]
            RU,
            /// <summary>
            /// French
            /// </summary>
            [Description("French")]
            FR,
            /// <summary>
            /// Portuguese
            /// </summary>
            [Description("Portuguese")]
            PT,
            /// <summary>
            /// German
            /// </summary>
            [Description("Polish")]
            PL
            ///// <summary>
            ///// Latin (it's really LA but it's not supported)
            ///// </summary>
            //[Description("Latin")]
            //FO
        }

        /// <summary>
        /// All of the available credit card types
        /// </summary>
        public enum CreditCardType
        {
            unknown = 0,
            [Description("Visa")]
            VI,
            [Description("Master Card")]
            MC,
            [Description("Discover")]
            DS,
            [Description("AMEX")]
            AM 
        }
 

        /// <summary>
        /// All the cookie names
        /// </summary>
        public enum CookieName
        {
            unknown = 0, 
            usersetting,
            cart
        }

        /// <summary>
        /// Names of values used for cookies
        /// </summary>
        public enum CookieValue
        {
            unknown = 0,
            language,
            itemcode,
            referrerurl,
            eqsitekey,
            cartid,
            shippingCountry,

        }

        /// <summary>
        /// Session variables in the site
        /// </summary>
        public enum SessionName
        {
            unknown = 0,
            ccinfo,
            shopping_cart,
            authed_user
        }



 
        public enum ResponseType
        {
            /// <summary>
            /// unknown
            /// </summary>
            U,
            /// <summary>
            /// Comment
            /// </summary>
            C,
            /// <summary>
            /// Applaud
            /// </summary>
            A,
            /// <summary>
            /// beat down
            /// </summary>
            B,
            /// <summary>
            /// Delete Applaud
            /// </summary>
            D,
            /// <summary>
            /// Delete Beatdown
            /// </summary>
            E

           
        }



        public enum AcknowledgementType
        {
            /// <summary>
            /// unknown
            /// </summary>
            U,
           
            /// <summary>
            /// Applaud
            /// </summary>
            A,
            /// <summary>
            /// beat down
            /// </summary>
            B


        }
 
    }
}