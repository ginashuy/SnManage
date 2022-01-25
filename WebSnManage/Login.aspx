<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebSnManage.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login</title>
        <style>
        html, body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        ul li {
            list-style: none;
        }

        .background {
            background-color: #f3f3f3;
            width: 100%;
            height: 100vh;
            display: -webkit-flex;
            display: flex;
            -webkit-align-items: center;
            align-items: center;
            -webkit-justify-content: center;
            justify-content: center;
        }

        .login-wrapper {
            background-color: #fefefe;
            width: 320px;
            box-shadow: 0 0 8px rgba(0,0,0, .25);
            position: relative;
        }

            .login-wrapper img {
                width: 100%;
                padding: 15px 20px;
                box-sizing: border-box;
                margin: 10px 0 0;
            }

            .login-wrapper:before, .login-wrapper:after {
                content: '';
                position: absolute;
                display: block;
                width: 100%;
                height: 3px;
            }

            .login-wrapper:after {
                background-color: #D7006D;
                bottom: 0;
            }

        .input-wrapper {
            display: block;
            margin: 0 20px 20px;
        }

            .input-wrapper span {
                display: block;
                font-size: 14px;
                font-weight: 500;
                color: #007883;
                margin-bottom: 3px;
            }

            .input-wrapper input {
                box-sizing: border-box;
                width: 100%;
                font-size: 16px;
                padding: 5px;
                border: 1px solid #cacaca;
                border-radius: 3px;
            }

        .btn-wrapper {
            text-align: center;
            margin: 30px 0;
        }

        .login-btn {
            cursor: pointer;
            font-size: 18px;
            border: none;
            padding: 10px 25px;
            color: #fefefe;
            background-color: #0099A8;
            border-radius: 100px;
        }

            .login-btn:hover {
                background-color: #007883;
            }

        .warning-text {
            font-size: 14px;
            color: red;
            padding: 0 20px;
        }
    </style>
</head>
<body>
    <div class="background">
        <div class="login-wrapper">
            <img src="./images/LoginLogo.png" alt="logo">
            <form id="form1" runat="server">
                <label class="input-wrapper">
                    <span>Account</span>
                    <asp:TextBox ID="LoginId" runat="server" type="text" />
                </label>
                <label class="input-wrapper">
                    <span>Password</span>
                    <asp:TextBox ID="LoginPassword" type="password" runat="server" />
                </label>

                <label id="LoginMessage" runat="server" class="warning-text" visible="false"></label>

                <div class="btn-wrapper">
                    <asp:Button ID="btnSummit" class="login-btn" runat="server" Text="Login" OnClick="btnSummit_Click" />
                </div>
            </form>
        </div>
    </div>
</body>
</html>
