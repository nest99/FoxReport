<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InitWeek.aspx.cs" Inherits="FoxReport.InitWeek" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="ButtonInitWeek" runat="server" Text="初始化周数据表" OnClick="ButtonInitWeek_Click" />
    
        <br />
        <asp:Label ID="LabelMsg" runat="server" Text=""></asp:Label>
    
    </div>
    </form>
</body>
</html>
