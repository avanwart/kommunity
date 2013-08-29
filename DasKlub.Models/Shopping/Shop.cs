using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasKlub.Lib.Operational;
using DasKlub.Models.Domain;
using DasKlub.Models.Forum;

namespace DasKlub.Models.Shopping
{
    public class Shop : StateInfo
    {
        #region members

        #endregion

        #region constructors

        public Shop()
        {
            
        }

        #endregion

        #region properties

        public int ShopID { get; set; }



        #endregion

    }
}


