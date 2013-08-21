<%@ Page Title="" Language="C#" MasterPageFile="~/m/auth/Shared/Main.Master" AutoEventWireup="true" CodeBehind="SongProperties.aspx.cs" Inherits="DasKlub.Web.m.auth.SongProperties" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 


    <asp:Label ID="lblStatus" runat="server"></asp:Label>
  
    <div>
        <table>
            

            <tr>
                <td>
                    :</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Artist:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlArtist1_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Song:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtistSongs1" runat="server" AutoPostBack="True" 
                                      onselectedindexchanged="ddlArtistSongs1_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    iTunes Link:
                </td>
                <td>     
                    <asp:TextBox ID="txtiTunesLink" runat="server" Width="577px" Height="64px" 
                                 TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                    Amazon Link:
                </td>
                <td>
                    <asp:TextBox ID="txtAmazonLink" runat="server" Width="577px" Height="64px" 
                                 TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Set Song Property" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
 
</asp:Content>