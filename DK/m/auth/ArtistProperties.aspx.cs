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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.Operational;
using BootBaronLib.Values;
using Image = System.Drawing.Image;

namespace DasKlub.Web.m.auth
{
    public partial class ArtistProperties : Page
    {
        #region variables

        private Artist art;
        private ArtistProperty artprop;
        private Artists arts;
        private MembershipUser mu;

        #endregion

        #region events

        protected void Page_Load(object sender, EventArgs e)
        {
            mu = Membership.GetUser();

            if (!IsPostBack)
            {
                ClearControls();
                GetArtistsPageWise(1);
                SetControls();
            }
        }


        protected void gvwAllArtists_SelectedIndexChanged(object sender, EventArgs e)
        {
            hfArtistID.Value = gvwAllArtists.SelectedDataKey.Value.ToString();

            LoadArtistDetail(Convert.ToInt32(hfArtistID.Value));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfArtistID.Value))
            {
                art = new Artist();
            }
            else
            {
                art = new Artist(Convert.ToInt32(hfArtistID.Value));
            }

            art.IsHidden = chkIsHidden.Checked;

            art.AltName = txtArtistAltName.Text;
            art.Name = txtArtistName.Text;


            if (art.ArtistID == 0)
            {
                if (art.Create() > 0)
                {
                    MasterPageHelper.SetMainMasterPageMessageText(Page, "Artist created", true);
                }
                else
                {
                    MasterPageHelper.SetMainMasterPageMessageText(Page, "Artist not created", false);
                    return;
                }
            }
            else
            {
                if (art.Update())
                {
                    MasterPageHelper.SetMainMasterPageMessageText(Page, "Artist updated", true);
                }
                else
                {
                    MasterPageHelper.SetMainMasterPageMessageText(Page, "Artist updated", false);
                    return;
                }
            }


            artprop = new ArtistProperty();
            artprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.LD.ToString());
            artprop.PropertyContent = txtArtistDescription.Text;

            if (artprop.ArtistPropertyID == 0)
            {
                artprop.Create();
            }
            else
            {
                artprop.Update();
            }

            artprop = new ArtistProperty();
            artprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.MD.ToString());
            artprop.PropertyContent = txtArtistMetaDescription.Text;


            if (artprop.ArtistPropertyID == 0)
            {
                artprop.Create();
            }
            else
            {
                artprop.Update();
            }


            if (fupArtistPhoto.HasFile)
            {
                artprop = new ArtistProperty();
                artprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.PH.ToString());
                artprop.PropertyContent = imgArtistPhoto.ImageUrl;

                var b = new Bitmap(fupArtistPhoto.FileContent);
                string saveS = string.Empty;
                Image imgPhoto = null;
                Color theBGColor = Color.Black;
                string fileRoot = ArtistProperty.Artistimageprefix;
                Guid fileGuid = Guid.NewGuid();

                if (ddlBGColor.SelectedValue.ToLower() == "black")
                {
                    theBGColor = Color.Black;
                }
                else if (ddlBGColor.SelectedValue.ToLower() == "white")
                {
                    theBGColor = Color.White;
                }

                // delete main image if exists
                if (!string.IsNullOrEmpty(artprop.PropertyContent))
                {
                    // delete the existing file
                    try
                    {
                        File.Delete(Server.MapPath(artprop.PropertyContent));
                    }
                    catch (Exception ex)
                    {
                        Utilities.LogError(ex);
                    }
                }

                // 300 x 300  
                imgPhoto = b;
                imgPhoto = ImageResize.FixedSize(imgPhoto, 300, 300, theBGColor);
                saveS = fileRoot + artprop.ArtistID.ToString() + "/" + fileGuid.ToString() + "_main.jpg";

                string artistFolder = ArtistProperty.Artistimageprefix + artprop.ArtistID.ToString();

                if (!Directory.Exists(artistFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(Server.MapPath(artistFolder));
                    }
                    catch (Exception ex)
                    {
                        Utilities.LogError(ex);
                    }
                }

                imgPhoto.Save(Server.MapPath(saveS), ImageFormat.Jpeg);
                artprop.PropertyContent = saveS;


                if (artprop.ArtistPropertyID == 0)
                {
                    artprop.Create();
                }
                else
                {
                    artprop.Update();
                }


                ///////////////////////


                artprop = new ArtistProperty();
                artprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.TH.ToString());


                // delete main image if exists
                if (!string.IsNullOrEmpty(artprop.PropertyContent))
                {
                    // delete the existing file
                    try
                    {
                        File.Delete(Server.MapPath(artprop.PropertyContent));
                    }
                    catch (Exception ex)
                    {
                        Utilities.LogError(ex);
                    }
                }


                imgPhoto = b;
                imgPhoto = ImageResize.FixedSize(imgPhoto, 75, 75, theBGColor);
                saveS = fileRoot + artprop.ArtistID.ToString() + "/" + fileGuid.ToString() + "_thumb.jpg";

                artprop.PropertyContent = saveS;

                if (!Directory.Exists(artistFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(Server.MapPath(artistFolder));
                    }
                    catch (Exception ex)
                    {
                        Utilities.LogError(ex);
                    }
                }

                imgPhoto.Save(Server.MapPath(saveS), ImageFormat.Jpeg);
                artprop.PropertyContent = saveS;


                if (artprop.ArtistPropertyID == 0)
                {
                    artprop.Create();
                }
                else
                {
                    artprop.Update();
                }
            }


            LoadArtistDetail(art.ArtistID);
        }


        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);

            gvwAllArtists.SelectedIndex = -1;

            GetArtistsPageWise(pageIndex);
        }

        protected void PageSize_Changed(object sender, EventArgs e)
        {
            GetArtistsPageWise(1);
        }

        #endregion

        #region methods

        private void LoadArtistDetail(int artistID)
        {
            art = new Artist(artistID);

            txtArtistAltName.Text = art.AltName;
            txtArtistName.Text = art.Name;
            chkIsHidden.Checked = art.IsHidden;

            artprop = new ArtistProperty();
            artprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.LD.ToString());
            txtArtistDescription.Text = artprop.PropertyContent;


            artprop = new ArtistProperty();
            artprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.MD.ToString());
            txtArtistMetaDescription.Text = artprop.PropertyContent;


            artprop = new ArtistProperty();
            artprop.GetArtistPropertyForTypeArtist(art.ArtistID, SiteEnums.ArtistPropertyType.PH.ToString());
            imgArtistPhoto.ImageUrl = artprop.PropertyContent;
        }


        private void SetControls()
        {
            txtArtistDescription.Attributes.Add("onkeyup",
                                                "limitText(this.form." + txtArtistDescription.ClientID +
                                                ",this.form.countdown,600);");

            txtArtistDescription.Attributes.Add("onkeydown",
                                                "limitText(this.form." + txtArtistDescription.ClientID +
                                                ",this.form.countdown,600);");

            ////

            txtArtistMetaDescription.Attributes.Add("onkeyup",
                                                    "limitText(this.form." + txtArtistMetaDescription.ClientID +
                                                    ",this.form.countdown2,155);");

            txtArtistMetaDescription.Attributes.Add("onkeydown",
                                                    "limitText(this.form." + txtArtistMetaDescription.ClientID +
                                                    ",this.form.countdown2,155);");
        }

        private void ClearControls()
        {
            txtArtistAltName.Text = string.Empty;
            txtArtistDescription.Text = string.Empty;
            txtArtistMetaDescription.Text = string.Empty;
            txtArtistName.Text = string.Empty;
        }


        private void GetArtistsPageWise(int pageIndex)
        {
            arts = new Artists();

            int recordCount = arts.GetArtistsPageWise(pageIndex, Convert.ToInt32(ddlPageSize.SelectedValue));

            gvwAllArtists.DataSource = arts;
            gvwAllArtists.DataBind();

            PopulatePager(recordCount, pageIndex);
        }

        private void PopulatePager(int recordCount, int currentPage)
        {
            var dblPageCount = (double) (recordCount/decimal.Parse(ddlPageSize.SelectedValue));
            var pageCount = (int) Math.Ceiling(dblPageCount);
            var pages = new List<ListItem>();
            if (pageCount > 0)
            {
                pages.Add(new ListItem("First", "1", currentPage > 1));
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                pages.Add(new ListItem("Last", pageCount.ToString(), currentPage < pageCount));
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        #endregion
    }
}