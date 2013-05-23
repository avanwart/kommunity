<%@ Page Title="" Language="C#" MasterPageFile="~/m/auth/Shared/Main.Master" AutoEventWireup="true"
         CodeBehind="ModifyVenue.aspx.cs" Inherits="DasKlub.Web.Web.m.auth.ModifyVenue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div>


        <asp:HiddenField ID="hfVenueID" runat="server" />
        <table>


    

            <tr>
                <td>
    
                    Venue:</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlVenues" AutoPostBack="true" 
                                      onselectedindexchanged="ddlVenues_SelectedIndexChanged"></asp:DropDownList>
                </td>
            </tr>
    


            <tr> 
                <td>
                    Venue Name:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtVenueName" runat="server"></asp:TextBox>
                </td>
            </tr>
    
    
            <tr>
                <td>
                    Address Line 1:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtAddressLine1" runat="server"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    Address Line 2:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtAddressLine2" runat="server"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    City:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtCity" runat="server"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    Region:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtRegion" runat="server"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td>
                    Postal Code:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtPostalCode" runat="server"></asp:TextBox>
                </td>
            </tr>
    



    
            <tr>
                <td>
                    Country ISO:
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountryISO" runat="server">
                        <asp:ListItem>AR</asp:ListItem>
                        <asp:ListItem>US</asp:ListItem>
                        <asp:ListItem>NL</asp:ListItem>
                        <asp:ListItem>UK</asp:ListItem>
                        <asp:ListItem>CA</asp:ListItem>
                        <asp:ListItem>DE</asp:ListItem>
                        <asp:ListItem>ES</asp:ListItem>
                        <asp:ListItem>PL</asp:ListItem>
                        <asp:ListItem>RU</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
    
    

    
            <tr>
                <td>
                    Phone Number:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtPhoneNumber" runat="server"></asp:TextBox>
                </td>
            </tr>
    



        
            <tr>
                <td>
                    Venue Type:
                </td>
                <td>
                    <asp:DropDownList ID="ddlVenueType" runat="server">
                        <asp:ListItem Value="C">Club</asp:ListItem>
                        <asp:ListItem Value="V">Concert Venue</asp:ListItem>
                        <asp:ListItem Value="F">Festival</asp:ListItem>
                        <asp:ListItem Value="S">Shop</asp:ListItem>
                        <asp:ListItem Value="W">Wifi</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
    
    





    
            <tr>
                <td>
                    Latitude:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtLatitude" runat="server"></asp:TextBox>
                </td>
            </tr>
    




            <tr>
                <td>
                    Longitude:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtLongitude" runat="server"></asp:TextBox>
                </td>
            </tr>
    


    
            <tr>
                <td>
                    Venue URL:
                </td>
                <td>
                    <asp:TextBox width="200px" ID="txtVenueURL" runat="server"></asp:TextBox>
                </td>
            </tr>
    


    
    
            <tr>
                <td>
                    Is Enabled:
                </td>
                <td>
                    <asp:CheckBox ID="chkEnabled" runat="server" />
                </td>
            </tr>




        
    
            <tr>
                <td>
                    Details:
                </td>
                <td>
                    <asp:TextBox width="200px" Height="200px" ID="txtDescription" TextMode="MultiLine" runat="server"></asp:TextBox>
                </td>
            </tr>







            <tr>
    
                <td></td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" />
                </td>
    
            </tr>

    
    
        </table>
    </div>
</asp:Content>