<%@ Page Title="" Language="C#" MasterPageFile="~/m/auth/Shared/Main.Master" AutoEventWireup="true" CodeBehind="ArtistProperties.aspx.cs" Inherits="DasKlub.Web.Web.m.auth.ArtistProperties" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <asp:GridView AutoGenerateColumns="False" DataKeyNames="artistID" ID="gvwAllArtists"
                  runat="server" BackColor="White"
                  BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                  CellPadding="3" ForeColor="Black"
                  GridLines="Vertical" onselectedindexchanged="gvwAllArtists_SelectedIndexChanged" 
        >
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" HeaderText="" />
              
            <asp:HyperLinkField DataTextField="urlto"
                                HeaderText="Product URL" DataNavigateUrlFields="urlto"
                                Target="_blank" />

            <asp:BoundField DataField="artistID" HeaderText="Artist ID" />
            <asp:BoundField DataField="name" HeaderText="name" />
            <asp:BoundField DataField="createDate" HeaderText="Create Date" DataFormatString="{0:u}" />
            
        </Columns>
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#808080" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#383838" />
    </asp:GridView>

    <br />
    Page Size:
    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed">
        <asp:ListItem Text="5" Value="5" />
        <asp:ListItem Text="10" Value="10" />
        <asp:ListItem Text="15" Value="15" />
        <asp:ListItem Text="25" Value="25" />
        <asp:ListItem Text="50" Value="50" />
    </asp:DropDownList>
    <br />
    <asp:Repeater ID="rptPager" runat="server">
        <ItemTemplate>
            <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                            Enabled='<%# Eval("Enabled") %>' OnClick="Page_Changed"></asp:LinkButton>
        </ItemTemplate>
    </asp:Repeater>


    <br />
    <table>
        <tr>
            <td>
                Artist ID:
            </td>
            <td>
                <asp:Literal ID="litArtistID" runat="server"></asp:Literal>
                <asp:HiddenField ID="hfArtistID" runat="server" />
            </td>
        </tr>

        <tr>
            <td>Artist Photo:</td>
            <td>
                <asp:Image runat="server" ID="imgArtistPhoto" />
                <br />
                <asp:DropDownList ID="ddlBGColor" runat="server">
                    <asp:ListItem>Black</asp:ListItem>
                    <asp:ListItem>White</asp:ListItem>
                            
                </asp:DropDownList>
                <br />
    
                <asp:FileUpload ID="fupArtistPhoto" runat="server" />
            </td>
        </tr>



        <tr>
            <td>Is Hidden:</td>
            <td>
                <asp:CheckBox ID="chkIsHidden" runat="server" />
            </td>
        </tr>


        <tr>
            <td>Artist Name:</td>
            <td>
                <asp:TextBox ID="txtArtistName" runat="server"></asp:TextBox>
            </td>
        </tr>



    
        <tr>
            <td>Artist Alt Name 
                <br />
                (used mostly for non-URL safe characters):</td>
            <td>
                <asp:TextBox ID="txtArtistAltName" runat="server"></asp:TextBox>
            </td>
        </tr>







        <tr>
            <td>
                Artist   Description:
            </td>
            <td>
    

    
                <asp:TextBox 
   

                    ID="txtArtistDescription" TextMode="MultiLine" runat="server" 
                    Height="328px" Width="306px"></asp:TextBox>
                <br />
                You have <input readonly type="text" name="countdown" size="3" value="600"> characters left. 

     
            </td>
        </tr>



     





        <tr>
            <td>
                Artist Meta Description:
            </td>
            <td>
    
                <asp:TextBox 
 
                    ID="txtArtistMetaDescription" TextMode="MultiLine" runat="server" 
                    Height="128px" Width="306px"></asp:TextBox>
                <br />
                You have <input readonly type="text" name="countdown2" size="3" value="155"> characters left. 
    
    
            </td>
        </tr>














        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="Set" 
                            Height="40px" onclick="btnSubmit_Click" Width="125px" 
                    />
            </td>
        </tr>

    </table>


</asp:Content>