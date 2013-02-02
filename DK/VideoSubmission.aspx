<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoSubmission.aspx.cs" Inherits="DasKlub.VideoSubmission" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+</title>
    <style type="text/css">
    
    body { background-color: #000;color: #FFF;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Literal ID="litResult" runat="server"></asp:Literal>
    </div>
    <asp:HyperLink ID="hlkBack" NavigateUrl="~/" runat="server"><--</asp:HyperLink>
    
    </form>
</body>
</html>
