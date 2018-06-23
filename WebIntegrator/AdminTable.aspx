<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminTable.aspx.cs" Inherits="WebIntegrator.AdminTable" %>

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
        <form runat="server">
            <asp:Button ID="btnNew" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" OnClick="btnNew_Click" Text="Добавить новый курс"  />
            <asp:Button ID="btnExit" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" OnClick="btnExit_Click" Text="Выйти из системы" />
            <asp:Label ID="lbAdm" runat="server" Font-Size="16pt" Text="Чтобы получить доступ к данной странице, нужно обладать правами администратора" Visible="False"></asp:Label>
            &nbsp;<asp:Button ID="btnAutoEdit" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" OnClick="btnAutoEdit_Click" Text="AutoEdit" />
            <br />
            <br />
            <asp:GridView ID="AdminTableView" CssClass="table table-hover" runat="server" AllowPaging="True" AutoGenerateColumns="False" Font-Size="9pt" OnPageIndexChanging="AdminTableView_PageIndexChanging" OnRowCommand="AdminTableView_RowCommand" PageSize="40" OnSelectedIndexChanged="AdminTableView_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="id" />
                    <asp:BoundField DataField="MyURL" HeaderText="URL" NullDisplayText=" " >
                    <ControlStyle Font-Size="0pt" />
                    <HeaderStyle Font-Size="0pt" />
                    <ItemStyle Font-Size="0pt" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Name" HeaderText="Название курса" />
                    <asp:BoundField DataField="MyProvider" HeaderText="Провайдер" />
                    <asp:BoundField DataField="MyUniversity" HeaderText="Институт" />
                    <asp:BoundField DataField="MySubject" HeaderText="Предметная область" />
                    <asp:BoundField DataField="MyTime" HeaderText="Время начала" />
                    <asp:BoundField DataField="Sertificate" HeaderText="Сертификат" >
                    <HeaderStyle Font-Size="10pt" />
                    </asp:BoundField>
                    <asp:BoundField DataField="School" HeaderText="Школа" >
                    <HeaderStyle Font-Size="10pt" />
                    </asp:BoundField>   
                    <asp:BoundField DataField="Student" HeaderText="Высшее образование" >
                    <HeaderStyle Font-Size="10pt" />
                    </asp:BoundField>             
                    <asp:BoundField DataField="Qualification" HeaderText="Повышение квалификации" >
                    <HeaderStyle Font-Size="10pt" />
                    </asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                                 <EditItemTemplate>
                                     <asp:LinkButton ID="ULabel" runat="server" Text='Изменить' > </asp:LinkButton>
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="ULabel1" runat="server" CommandName = "Upd" CommandArgument = '<%# DataBinder.Eval(Container, "RowIndex") %>' Text='Изменить'> </asp:LinkButton>
                                 </ItemTemplate>
                             </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                 <EditItemTemplate>
                                     <asp:LinkButton ID="DLabel" runat="server" Text='Удалить' > </asp:LinkButton>
                                 </EditItemTemplate>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="DLabel" runat="server" CommandName ="Del" CommandArgument = '<%# DataBinder.Eval(Container, "RowIndex") %>' Text='Удалить'> </asp:LinkButton>
                                 </ItemTemplate>
                             </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
                <PagerStyle HorizontalAlign="Center" />
                <RowStyle HorizontalAlign="Center" />
            </asp:GridView>
    
             <asp:Panel ID="Panel1" runat="server" CssClass="jumbotron yobject-marked" Visible="false" Width="687px" >
                 <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="URL" Width="150px" CssClass="col-sm-2 col-form-label"></asp:Label>
                        <asp:TextBox ID="tbURL" runat="server" CssClass="form-control" ></asp:TextBox>
                 </div>
                <div class="form-group">
                    <asp:Label ID="Label2" runat="server" Text="Название курса" CssClass="col-sm-2 col-form-label"></asp:Label>
                    <asp:TextBox ID="tbName" runat="server" CssClass="form-control" ></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label3" runat="server" Text="Провайдер" CssClass="col-sm-2 col-form-label"></asp:Label>
                    <asp:DropDownList ID="cmbProvider" runat="server" CssClass="form-control"  >
                    </asp:DropDownList>
                </div>
                <div class="form-group">  
                    <asp:Label ID="Label4" runat="server" Text="Институт" CssClass="col-sm-2 col-form-label"></asp:Label>
                    <asp:TextBox ID="tbUniversity" runat="server" CssClass="form-control" ></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label5" runat="server" Text="Предметная область" CssClass="col-sm-2 col-form-label"></asp:Label>
                    <asp:DropDownList ID="cmbSubject" runat="server" CssClass="form-control" >
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Label ID="Label6" runat="server" Text="Время начала" CssClass="col-sm-2 col-form-label"></asp:Label>
                    <asp:DropDownList ID="cmbTime" runat="server" CssClass="form-control" >
                    </asp:DropDownList>
                </div>
                    <asp:CheckBoxList ID="chbBool" runat="server" RepeatDirection="Horizontal" Height="16px" Width="567px" CssClass="btn-group-toggle">
                        <asp:ListItem>Сертификат</asp:ListItem>
                        <asp:ListItem>Школа</asp:ListItem>
                        <asp:ListItem>Высшее образование</asp:ListItem>
                        <asp:ListItem>Повышение квалификации</asp:ListItem>
                    </asp:CheckBoxList>
                <br />
                    <asp:Label ID="lbResult" runat="server" Font-Bold="True" ForeColor="Red" Text="lbResult" Visible="False"></asp:Label>
                <br />
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" OnClick="btnSave_Click" Text="Сохранить" Width="172px" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary my-2 my-sm-0" OnClick="btnCancel_Click" Text="Отмена" Width="172px" />
            </asp:Panel>
    </form>
    </div>
    <script src="Scripts/jquery-3.3.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>