<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
   
        <asp:FileUpload ID="FileUpload1" runat="server" Height="23px" style="margin-bottom: 2px" Width="292px" />
    
    </div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" style="margin-right: 0px; margin-top: 0px" Text="保存" Width="62px" />
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="读取" Width="62px" />
        <asp:Label ID="Label2" runat="server"></asp:Label>&nbsp<br />
        <br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="删除" Width="59px" />
        <br />
        <br />
        <asp:Image ID="Image1" runat="server" Height="230px" Width="288px" />
        <br />

    </form>

    </body>
</html>
