<%@ Page Title="主页" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Test._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
<script type="text/jscript">
    $(function () {
        
        alert(qq);
    });
</script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   <asp:TextBox
       ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Label ID="L1" runat="server" Text="e0064b6952d699725b62ff81dcce84821c5e23442057fafaa737d2247ddcf885"></asp:Label><br />
    <asp:Button ID="Button2" runat="server" Text="加密" onclick="Button2_Click" /><asp:TextBox
        ID="TextBox2" runat="server"></asp:TextBox>
    <asp:Label ID="lb" runat="server" Text="Label"></asp:Label>
    <asp:Button ID="Button1" runat="server" Text="test" onclick="Button1_Click" />

 
</asp:Content>

