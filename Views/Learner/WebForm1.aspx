﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication6.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
 <title>Online Exam Page</title>
 <script type="text/javascript" src="count.js">
</script>
</head>
<body>
 <form id="form1" runat="server">
 <div>
 <asp:Label ID="Label1" runat="server" Font-Size="Medium" ForeColor="DarkRed" Style="z-index: 100;
 left: 64px; position: absolute; top: 19px" Text="Name : " Width="68px"></asp:Label>

 <asp:TextBox ID="txtName" runat="server" Style="z-index: 101; left: 146px; position: absolute;
 top: 18px"></asp:TextBox>
 <asp:Button ID="Button1" runat="server" Style="z-index: 102; left: 321px; position: absolute;
 top: 18px" Text="Start Exam" ToolTip="Enter Your Name" OnClick="Button1_Click" />
 <asp:TextBox ID="txtScore" runat="server" Style="z-index: 103; left: 681px; position: absolute;
 top: 276px" Visible="False" Width="63px">0</asp:TextBox>

 <asp:Panel ID="Panel1" runat="server" BackColor="#E0E0E0" BorderColor="#E0E0E0" Height="264px"
 Style="z-index: 104; left: 60px; position: absolute; top: 54px" Visible="False"
 Width="707px" ForeColor="#0000C0">
 <asp:Label ID="lblName" runat="server" Style="z-index: 100; left: 13px; position: absolute;
 top: 10px" Text="Name : " ForeColor="#0000C0" Width="387px"></asp:Label>
 &nbsp;
 <asp:Label ID="lblScore" runat="server" ForeColor="Green" Style="z-index: 102; left: 567px;
 position: absolute; top: 11px" Text="Score : " Width="136px"></asp:Label>
 <asp:Panel ID="Panel3" runat="server" Height="14px" Width="119px" style="left:427px; z-index: 106; position: absolute; top: 10px;">
 <span id="cd" style ="left:100px;"></span>
 </asp:Panel>
 <asp:Panel ID="Panel2" runat="server" Height="214px" Style="z-index: 103; left: 8px;
 position: absolute; top: 41px" Width="696px">
 <asp:Label ID="lblQuestion" runat="server" Style="z-index: 100; left: 3px; position: absolute;
 top: 7px" Text="Label" Width="682px"></asp:Label>
 <asp:RadioButtonList ID="RblOption" runat="server" Style="z-index: 102; left: 30px;
 position: absolute; top: 36px" Width="515px">
 </asp:RadioButtonList>
 <asp:Button ID="Button2" runat="server" Style="z-index: 106; left: 289px; position: absolute;
 top: 178px" Text="Next" ToolTip="Click Here to Take Next Question" OnClick="Button2_Click" />
 </asp:Panel>
 </asp:Panel>

 </div>
 </form>
</body>
</html>
