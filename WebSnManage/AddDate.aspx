<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddDate.aspx.cs" Inherits="WebSnManage.AddDate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>ExcelDemo</title>
        <style>
            
        </style>
</head>
<body>
    <div >           
        <asp:FileUpload ID="FileUpload1" runat="server" /></br></br>
        <asp:Button ID="BtnImport" runat="server" Text="匯入" OnClick="BtnImport_Click" />
    </div>
</body>
</html>
</asp:Content>
