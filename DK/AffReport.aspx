<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AffReport.aspx.cs" Inherits="DasKlub.AffReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <style>
            body {
                background-color: #000;
                color: #FFF;
            }

            a { color: Aqua; }
    
        </style>
    </head>
    <body>
        <form id="form1" runat="server">
            <a href="http://dasklub.com">back to Das Klub</a>
            <div>
                <asp:Literal runat="server" ID="litUserName"></asp:Literal> REPORT
                <br />
                <asp:GridView ID="gvwReport" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="createDate" HeaderText="Created"  DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="lastactivitydate" HeaderText="Last Active"  DataFormatString="{0:yyyy-MM-dd}" />
                        <asp:BoundField DataField="username" HeaderText="User Name" />
                    </Columns>
                </asp:GridView>
            </div>
        </form>
    </body>
</html>