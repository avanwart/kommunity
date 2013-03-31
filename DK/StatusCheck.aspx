<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StatusCheck.aspx.cs" Inherits="DasKlub.StatusCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
    </head>
    <body>
        <form id="form1" runat="server">

            <div>
                <table>

                    <tr>
                        <td>
                            google.com ping
                        </td>
                        <td>
                            <asp:Label ID="lblPing" runat="server"></asp:Label>
                        </td>
                    </tr>




                    <tr>
                        <td>
                            DB:
                        </td>
                        <td>
                            <asp:Label ID="lblDB" runat="server"></asp:Label>
                        </td>
                    </tr>


    
    
    
    
    
                    <tr>
                        <td>
                            Email:
                        </td>
                        <td>
                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            Log Error:
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server"></asp:Label>
                        </td>
                    </tr>
       

                    <tr>
                        <td>
                            Region:
                        </td>
                        <td>
                            <asp:Label ID="lblRegion" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            Country:
                        </td>
                        <td>
                            <asp:Label ID="lblCountry" runat="server"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            Current IP:
                        </td>
                        <td>
                            <asp:Label ID="lblIP" runat="server"></asp:Label>
                        </td>
                    </tr>


                    <tr>
                        <td>
                            Browser:
                        </td>
                        <td>
                            <asp:Label ID="lblBrowser" runat="server"></asp:Label>
                        </td>
                    </tr>


                    <tr>
                        <td>
                            Is Mobile:
                        </td>
                        <td>
                            <asp:Label ID="lblIsMobile" runat="server"></asp:Label>
                        </td>
                    </tr>


                    <tr>
                        <td>
                            Server IP:
                        </td>
                        <td>
                            <asp:Label ID="lblServerIP" runat="server"></asp:Label>
                        </td>
                    </tr>

                </table>
                <asp:Button ID="btnEmail" Text="Send E-Mail" runat="server" 
                            onclick="btnEmail_Click" />
                <br />
                <asp:Literal ID="litCache" runat="server"></asp:Literal>
            </div>
        </form>
    </body>
</html>