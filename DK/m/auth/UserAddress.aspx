<%@ Page Title="" Language="C#" MasterPageFile="~/m/auth/Shared/Main.Master" AutoEventWireup="true" CodeBehind="UserAddress.aspx.cs" Inherits="DasKlub.Web.Web.m.auth.UserAddress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Literal ID="litStatus" runat="server"></asp:Literal>
    <asp:GridView AutoGenerateColumns="False" DataKeyNames="userAddressID" ID="gvwUserAddresses"
                  runat="server"  BackColor="White"
                  BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                  CellPadding="4" ForeColor="Black"
                  GridLines="Vertical" 
                  onselectedindexchanged="gvwUserAddresses_SelectedIndexChanged">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:CommandField ShowSelectButton="True" />
            <asp:BoundField DataField="userAddressID" HeaderText="User Address ID" />
            <asp:BoundField DataField="createDate" HeaderText="Create Date"  DataFormatString="{0:u}"  />
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
            <td>Search:</td>
            <td>
                <asp:TextBox ID="txtSearch"  runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" onclick="btnSearch_Click" 
                    />
            </td>
        </tr>
    
        <tr>
            <td>User:</td>
            <td>
                <asp:HyperLink ID="hlkUserLink" runat="server"></asp:HyperLink>
            </td>
        </tr>



        
        <tr>
            <td>User ID:</td>
            <td>
                <asp:TextBox ID="txtUserID" runat="server"></asp:TextBox>
            </td>
        </tr>



        <tr>
            <td>Selected:</td>
            <td>
                <asp:Literal runat="server" id="litUserAddressID"></asp:Literal>
                <asp:HiddenField ID="hfUserAddressID" runat="server" />
            </td>
        </tr>


        <tr>
            <td>Full Address:</td>
            <td>
                <asp:Literal runat="server" ID="litFullAddress"></asp:Literal>
            </td>
        </tr>


        <tr>
            <td>Address Status:</td>
            <td>
                <asp:DropDownList ID="ddlAddressStatus" runat="server">
                    <asp:ListItem Value="U">SELECT</asp:ListItem>
                    <asp:ListItem Value="N">Nothing</asp:ListItem>
                    <asp:ListItem Value="K">Kit</asp:ListItem>
                    <asp:ListItem Value="T">T-Shirt</asp:ListItem>
                    <asp:ListItem Value="S">Sticker</asp:ListItem>
                    <asp:ListItem Value="B">Boot</asp:ListItem>
                    <asp:ListItem Value="P">Pick</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>










     
        <tr>
            <td style="width: 150px">
                First Name:
            </td>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server" Width="235px"></asp:TextBox>
            </td>
        </tr>



        <tr>
            <td>
                Last Name:
            </td>
            <td>
            
                <asp:TextBox ID="txtLastName" runat="server"  Width="235px"></asp:TextBox>
            </td>
        </tr>


        
        <tr>
            <td>
                Address Line 1:
            </td>
            <td>
            
                <asp:TextBox ID="txtAddressLine1" runat="server"  Width="235px"></asp:TextBox>
            </td>
        </tr>

 

 
        <tr>
            <td>
                Address Line 2:
            </td>
            <td>
            
                <asp:TextBox ID="txtAddressLine2" runat="server"  Width="235px"></asp:TextBox>
            </td>
        </tr>



         

 
        <tr>
            <td>
                Address Line 3:
            </td>
            <td>
           
                <asp:TextBox ID="txtAddressLine3" runat="server"  Width="235px"></asp:TextBox>
            </td>
        </tr>

        <tr>
            <td>
                City
            </td>
            <td>
                <asp:TextBox ID="txtCity" runat="server"  Width="235px"></asp:TextBox>
            </td>
        </tr>




        
        <tr>
            <td>
                Region/ State:
            </td>
            <td>
            
                <asp:TextBox ID="txtRegion" runat="server"  Width="235px"></asp:TextBox>
            </td>
        </tr>

        
        <tr>
            <td>
                Country:
            </td>
            <td>
                <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
            </td>
        </tr>


       




        <tr>
            <td>
                Postal Code:
            </td>
            <td>
                <asp:TextBox ID="txtPostalCode" runat="server"  Width="235px"></asp:TextBox>
            </td>
        </tr>





        
        <tr>
            <td>
                Choice 1:
            </td>
            <td>
        
                <asp:Literal ID="litChoice1" runat="server"></asp:Literal>

            </td>
        </tr>

         



         
        <tr>
            <td>
                Choice 2: 
            </td>
            <td>
            

                <asp:Literal ID="litChoice2" runat="server"></asp:Literal>

            </td>
        </tr>



 
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" />
    
            </td>
        </tr>

    </table>



</asp:Content>