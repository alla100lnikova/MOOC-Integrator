<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="WebIntegrator.AdminLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Content/bootstrap.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="container">
            <br />
        <form id="form1" runat="server">
        <div>
            <asp:Panel ID="Panel1" runat="server" CssClass="jumbotron yobject-marked" Width="687px" >
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="form-group">
                        <asp:Label ID="Label7" runat="server" Text="Логин" CssClass="col-sm-2 col-form-label"></asp:Label>
                        <asp:TextBox ID="tbLogin" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label8" runat="server" Text="Пароль" CssClass="col-sm-2 col-form-label"></asp:Label>
                        <asp:TextBox ID="tbPassword" runat="server" CssClass="form-control" TextMode="Password" ></asp:TextBox>
                    </div>
                    <asp:Label ID="lbResult" runat="server" Font-Bold="True" ForeColor="Red" Text="lbResult" Visible="False"></asp:Label>
                    <br />
                    <asp:Button ID="btnEnter" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" Text="Вход" style="margin-left: 0px" OnClick="btnEnter_Click" Width="174px" />
                    <div class="form-group">
                        <asp:Button ID="btnEnterSystem" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" OnClick="btnEnterSystem_Click" style="margin-left: 0px" Text="Войти в систему" />
                        <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" OnClick="btnRegister_Click" style="margin-left: 13px" Text="Зарегистрировать нового администратора" Font-Size="11pt" />
                        <br />
                        <asp:Button ID="btnRegisterAdmin" runat="server" CssClass="btn btn-secondary my-2 my-sm-0"  OnClick="btnRegisterAdmin_Click" style="margin-left: 0px" Text="Регистрация" Visible="false" Width="174px" />
                        <asp:Button ID="btnCansel" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" OnClick="btnCansel_Click" style="margin-left: 13px" Text="Отмена" Visible="false" />
                    </div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        </form>
    </div>
    <script src="Scripts/jquery-3.3.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
