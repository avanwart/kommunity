<%@ Page Title="" Language="C#" MasterPageFile="~/m/auth/Shared/Main.Master" AutoEventWireup="true"
         CodeBehind="ArtistTD.aspx.cs" Inherits="DasKlub.Web.Web.m.auth.ArtistTD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:GridView AutoGenerateColumns="False" DataKeyNames="eventID" ID="gvwEvents" runat="server"
                      BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                      CellPadding="4" ForeColor="Black" GridLines="Vertical" AllowPaging="True" OnSelectedIndexChanged="gvwEvents_SelectedIndexChanged">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="eventID" HeaderText="Event ID" />
                <asp:BoundField DataField="name" HeaderText="name" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <%--   <asp:Button ID="btnDeleteEvent" Text="delete" runat="server" OnClick="btnDeleteEvent_Click"
                            OnClientClick="return confirmSubmit()" />--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCC99" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <RowStyle BackColor="#F7F7DE" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#FBFBF2" />
            <SortedAscendingHeaderStyle BackColor="#848384" />
            <SortedDescendingCellStyle BackColor="#EAEAD3" />
            <SortedDescendingHeaderStyle BackColor="#575357" />
        </asp:GridView>
        <table>
            <tr>
                <td>
                    Tour Date:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTourDate" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Artist 1:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist1" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Artist 2:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist2" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Artist 3:
                </td>
                <td>
                    <asp:DropDownList ID="ddlArtist3" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>