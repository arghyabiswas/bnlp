<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Google.aspx.cs" Inherits="BNLP2008.Google" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtInput" runat="server" TextMode="MultiLine" Width="99.4%" Height="100"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="Translate" OnClick="Button1_Click" />
        <b>
            <asp:Literal ID="litBengali" runat="server"></asp:Literal></b>
    </div>
    </form>
</body>
</html>
