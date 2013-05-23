<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnSubscribe.aspx.cs" Inherits="DasKlub.Web.Web.UnSubscribe" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Unsubscribed</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
                <h2>Enter email address to unsubscribe</h2>
                <asp:literal runat="server" ID="litResult"></asp:literal>
                <table>
                    <tr>
                        <td>Email:</td>
                        <td>  <asp:TextBox ID="txtEmail" runat="server" Width="293px"></asp:TextBox></td>
                    </tr>

                </table>
      
                <asp:Button ID="btnSubmit" runat="server" Text="Unsubscribe" OnClick="btnSubmit_Click" />
     
            </div>
        </form>
    </body>
</html>