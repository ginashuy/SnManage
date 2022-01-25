<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SearchDate.aspx.cs" Inherits="WebSnManage.SearchDate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!DOCTYPE html>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>ExcelDemo</title>
        <style>
            
        </style>
</head>
<body>
    <form id="form1" runat="server">    
        <div > 
            <asp:Button ID="SearchBtn" runat="server" Text="查詢" OnClick="SearchBtn_Click" />
            <asp:GridView ID="SearchGrV" runat="server"></asp:GridView>
        </div>
    </form>
</body>
</html>
</asp:Content>
