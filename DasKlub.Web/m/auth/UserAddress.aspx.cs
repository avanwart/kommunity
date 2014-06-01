using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;

namespace DasKlub.Web.m.auth
{
    public partial class UserAddress : Page
    {
        private UserAccount ua;
        private Lib.BOL.UserAddress uadd;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // load grid
                var uadds = new UserAddresses();
                uadds.GetUserAddressesByStatus('U');

                uadds.Sort((p1, p2) => p2.CreateDate.CompareTo(p1.CreateDate));

                gvwUserAddresses.DataSource = uadds;
                gvwUserAddresses.DataBind();

                Array lstProvider = Enum.GetValues(typeof (SiteEnums.CountryCodeISO));

                foreach (SiteEnums.CountryCodeISO enProvider in lstProvider)
                {
                    ddlCountry.Items.Add(new ListItem(Utilities.GetEnumDescription(enProvider), enProvider.ToString()));
                }
            }
        }

        protected void gvwUserAddresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            // load address
            uadd =
                new Lib.BOL.UserAddress(Convert.ToInt32(gvwUserAddresses.SelectedDataKey.Value));

            LoadUserAddress(uadd.UserAccountID);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // update address 

            uadd = new Lib.BOL.UserAddress(Convert.ToInt32(hfUserAddressID.Value))
            {
                AddressLine1 = txtAddressLine1.Text,
                AddressLine2 = txtAddressLine2.Text,
                AddressLine3 = txtAddressLine3.Text,
                City = txtCity.Text,
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                PostalCode = txtPostalCode.Text,
                Region = txtRegion.Text,
                CountryISO = ddlCountry.SelectedValue,
                UserAccountID = Convert.ToInt32(txtUserID.Text)
            };

            if (!string.IsNullOrEmpty(ddlAddressStatus.SelectedValue))
            {
                uadd.AddressStatus = Convert.ToChar(ddlAddressStatus.SelectedValue);
            }

            if (uadd.UserAddressID == 0)
            {
                if (uadd.Create() > 0)
                {
                    litStatus.Text = "created: " + uadd.UserAddressID;
                }
                else
                {
                    litStatus.Text = "NOTHING CREATED";
                }
            }
            else if (uadd.Update())
            {
                litStatus.Text = "UPDATED: " + uadd.UserAddressID;
            }
            else
            {
                litStatus.Text = "FAILED TO UPDATE: " + uadd.UserAddressID;
            }
        }


        private void LoadUserAddress(int userAccountID)
        {
            ua = new UserAccount(userAccountID);

            if (uadd == null)
            {
                uadd = new Lib.BOL.UserAddress();
                uadd.GetUserAddress(ua.UserAccountID);
            }

            txtUserID.Text = ua.UserAccountID.ToString();
            hfUserAddressID.Value = uadd.UserAddressID.ToString();
            litUserAddressID.Text = uadd.UserAddressID.ToString();
            litFullAddress.Text = uadd.FormattedAddress;

            hlkUserLink.Text = ua.UserName;
            hlkUserLink.NavigateUrl = "/" + ua.UserName;
            hlkUserLink.Target = "_blank";

            txtAddressLine1.Text = uadd.AddressLine1;
            txtAddressLine2.Text = uadd.AddressLine2;
            txtAddressLine3.Text = uadd.AddressLine3;
            txtCity.Text = uadd.City;
            txtFirstName.Text = uadd.FirstName;
            txtLastName.Text = uadd.LastName;
            txtPostalCode.Text = uadd.PostalCode;
            txtRegion.Text = uadd.Region;
            if (!string.IsNullOrEmpty(uadd.CountryISO.Trim()))
            {
                ddlCountry.SelectedValue = uadd.CountryISO;
            }


            litChoice1.Text = "blank";
            litChoice2.Text = "blank";

            litChoice1.Text = uadd.Choice1;
            litChoice2.Text = uadd.Choice2;

            if (uadd.AddressStatus != char.MinValue)
            {
                ddlAddressStatus.SelectedValue = Convert.ToString(uadd.AddressStatus);
            }
            else
            {
                ddlAddressStatus.SelectedIndex = -1;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                ua = new UserAccount(txtSearch.Text);

                LoadUserAddress(ua.UserAccountID);
            }
        }
    }
}