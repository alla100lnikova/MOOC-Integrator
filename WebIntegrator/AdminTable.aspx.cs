using System;
using System.Web.UI.WebControls;
using System.Data;
using Searcher;

namespace WebIntegrator
{
    public partial class AdminTable : System.Web.UI.Page
    {
        public static int courseID;
        public static bool IsUpdate;
        public DataTable NewTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            lbAdm.Visible = false;
            //if (IsAdmin.AdminEnter)
            {
                lbAdm.Visible = false;
                NewTable = new DataTable();
                DataColumn ID = new DataColumn("ID", System.Type.GetType("System.String"));
                DataColumn URL = new DataColumn("MyURL", System.Type.GetType("System.String"));
                DataColumn Name = new DataColumn("Name", System.Type.GetType("System.String"));
                DataColumn MyProvider = new DataColumn("MyProvider", System.Type.GetType("System.String"));
                DataColumn MyUniversity = new DataColumn("MyUniversity", System.Type.GetType("System.String"));
                DataColumn MySubject = new DataColumn("MySubject", System.Type.GetType("System.String"));
                DataColumn StartTime = new DataColumn("MyTime", System.Type.GetType("System.String"));
                DataColumn Sertificate = new DataColumn("Sertificate", System.Type.GetType("System.Boolean"));
                DataColumn School = new DataColumn("School", System.Type.GetType("System.Boolean"));
                DataColumn Student = new DataColumn("Student", System.Type.GetType("System.Boolean"));
                DataColumn Qualification = new DataColumn("Qualification", System.Type.GetType("System.Boolean"));
                NewTable.Columns.Add(ID);
                NewTable.Columns.Add(URL);
                NewTable.Columns.Add(Name);
                NewTable.Columns.Add(MyProvider);
                NewTable.Columns.Add(MyUniversity);
                NewTable.Columns.Add(MySubject);
                NewTable.Columns.Add(StartTime);
                NewTable.Columns.Add(Sertificate);
                NewTable.Columns.Add(School);
                NewTable.Columns.Add(Student);
                NewTable.Columns.Add(Qualification);

                using (var ctx = new MOOCEntities())
                {
                    foreach (var course in ctx.Описание_MOOC)
                    {
                        DataRow MyRow = null;
                        MyRow = NewTable.NewRow();
                        MyRow[0] = course.id;
                        MyRow[1] = course.URL;
                        MyRow[2] = course.НазваниеКурса;
                        MyRow[3] = course.Провайдер1.Название;

                        if (course.Институт == null) MyRow[4] = "";
                        else MyRow[4] = course.Институт;

                        if (course.ПредметнаяОбласть == null) MyRow[6] = "";
                        else MyRow[5] = course.ПредметнаяОбласть1.Группа_ПредметнаяОбласть.Название;

                        if (course.ВремяНачала == null) MyRow[6] = "";
                        else MyRow[6] = course.ВремяНачала1.Группа_ВремяНачала.Название;

                        if (course.НаличиеСертификата == null) MyRow[7] = false;
                        else MyRow[7] = course.НаличиеСертификата;

                        if (course.Школа == null) MyRow[8] = false;
                        else MyRow[8] = course.Школа;

                        if (course.ВысшееОбразование == null) MyRow[9] = false;
                        else MyRow[9] = course.ВысшееОбразование;

                        if (course.ПовышениеКвалификации == null) MyRow[10] = false;
                        else MyRow[10] = course.ПовышениеКвалификации;

                        NewTable.Rows.Add(MyRow);
                    }
                    AdminTableView.DataSource = NewTable;
                    AdminTableView.DataBind();

                    #region Заполнение комбобоксов
                    if (cmbProvider.Items.Count == 0)
                    {
                        foreach (var provider in ctx.Провайдер)
                        {
                            cmbProvider.Items.Add(provider.Название);
                        }
                        foreach (var time in ctx.ВремяНачала)
                        {
                            cmbTime.Items.Add(time.Название);
                        }
                        cmbTime.Items.Add("Нет данных о времени начала");
                        foreach (var subject in ctx.ПредметнаяОбласть)
                        {
                            cmbSubject.Items.Add(subject.Название);
                        }
                    }

                    #endregion
                }
            }
            //else
            //{
            //    lbAdm.Visible = true;
            //    btnExit.Visible = false;
            //    btnNew.Visible = false;
            //}
        }

        protected void AdminTableView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void AdminTableView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AdminTableView.PageIndex = e.NewPageIndex;
            AdminTableView.DataBind();
        }

        public void Save(Описание_MOOC course)
        {
            course.URL = tbURL.Text;
            course.НазваниеКурса = tbName.Text;
            if (tbUniversity.Text == "") course.Институт = null;
            else course.Институт = tbUniversity.Text;
            course.Провайдер = Convert.ToInt32(cmbProvider.SelectedIndex + 1);
            course.ПредметнаяОбласть = Convert.ToInt32(cmbSubject.SelectedIndex + 1);
            if (cmbTime.Text == "Нет данных о времени начала") course.ВремяНачала = null;
            else course.ВремяНачала = Convert.ToInt32(cmbTime.SelectedIndex + 1);
            course.НаличиеСертификата = chbBool.Items[0].Selected;
            course.Школа = chbBool.Items[1].Selected;
            course.ВысшееОбразование = chbBool.Items[2].Selected;
            course.ПовышениеКвалификации = chbBool.Items[3].Selected;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Описание_MOOC course = new Описание_MOOC();

            lbResult.Visible = false;

            if (tbURL.Text == "" || tbName.Text == "")
            {
                lbResult.Visible = true;
                lbResult.Text = "У курса должны быть название и URL!";
                return;
            }

            if (!IsUpdate)
            {
                using (var ctx = new MOOCEntities())
                {
                    foreach (var course1 in ctx.Описание_MOOC)
                    {

                        if (tbURL.Text == course1.URL)
                        {
                            lbResult.Visible = true;
                            lbResult.Text = "Курс с заданным URL уже существует!";
                            return;
                        }
                    }
                }
            }

            if (IsUpdate)
            {
                using (var ctx = new MOOCEntities())
                {
                    foreach (var mycourse in ctx.Описание_MOOC)
                    {
                        if (mycourse.id == courseID)
                        {
                            course = mycourse;
                            break;
                        }
                    }
                    ctx.Описание_MOOC.Attach(course);
                    Save(course);
                    try
                    {
                        ctx.SaveChanges();
                        Panel1.Visible = false;
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        AdminTableView.Visible = true;
                        Page_Load(sender, e);
                    }
                    catch
                    {
                        lbResult.Visible = true;
                        lbResult.Text = "Название института не должно превышать 30 символов";
                    }
                }
            }
            else
            {
                using (var ctx = new MOOCEntities())
                {
                    Save(course);
                    ctx.Описание_MOOC.Add(course);
                    try
                    {
                        ctx.SaveChanges();
                        tbName.Text = "";
                        tbUniversity.Text = "";
                        tbURL.Text = "";
                        for (int i = 0; i < 4; i++)
                        {
                            chbBool.Items[i].Selected = false;
                        }
                        cmbProvider.SelectedIndex = 0;
                        cmbSubject.SelectedIndex = 0;
                        cmbTime.SelectedIndex = 0;
                        lbResult.Visible = true;
                        lbResult.Text = "Курс добавлен.";
                    }
                    catch
                    {
                        lbResult.Visible = true;
                        lbResult.Text = "Название института не должно превышать 30 символов";
                    }
                }

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Panel1.Visible = false;
            btnNew.Visible = true;
            btnExit.Visible = true;
            AdminTableView.Visible = true;
            Page_Load(sender, e);
        }

        protected void AdminTableView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            lbResult.Visible = false;
            if (e.CommandName.CompareTo("Upd") == 0)
            {
                int ID = Convert.ToInt32(e.CommandArgument);
                btnSave.Text = "Сохранить";
                btnCancel.Text = "Отмена";
                IsUpdate = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                courseID = Convert.ToInt32(AdminTableView.Rows[ID].Cells[0].Text);
                Panel1.Visible = true;
                using (var ctx = new MOOCEntities())
                {
                    #region Сохранение текущего
                    tbURL.Text = AdminTableView.Rows[ID].Cells[1].Text;
                    tbName.Text = AdminTableView.Rows[ID].Cells[2].Text;
                    if (AdminTableView.Rows[ID].Cells[4].Text == "&nbsp;") tbUniversity.Text = "";
                    else tbUniversity.Text = AdminTableView.Rows[ID].Cells[4].Text;

                    foreach (var provider in ctx.Провайдер)
                    {
                        if (AdminTableView.Rows[ID].Cells[3].Text == provider.Название)
                        {
                            cmbProvider.SelectedIndex = provider.id - 1;
                            break;
                        }
                    }

                    foreach (var subject in ctx.ПредметнаяОбласть)
                    {
                        if (AdminTableView.Rows[ID].Cells[5].Text == subject.Название)
                        {
                            cmbSubject.SelectedIndex = subject.id - 1;
                            break;
                        }
                    }

                    foreach (var time in ctx.ВремяНачала)
                    {
                        if (AdminTableView.Rows[ID].Cells[6].Text == time.Название)
                        {
                            cmbTime.SelectedIndex = time.id - 1;
                            break;
                        }
                    }

                    chbBool.Items[0].Selected = Convert.ToBoolean(AdminTableView.Rows[ID].Cells[7].Text);
                    chbBool.Items[1].Selected = Convert.ToBoolean(AdminTableView.Rows[ID].Cells[8].Text);
                    chbBool.Items[2].Selected = Convert.ToBoolean(AdminTableView.Rows[ID].Cells[9].Text);
                    chbBool.Items[3].Selected = Convert.ToBoolean(AdminTableView.Rows[ID].Cells[10].Text);
                    #endregion
                }
                AdminTableView.Visible = false;
            }
            else
            {
                if (e.CommandName.CompareTo("Del") == 0)
                {
                    int ID = Convert.ToInt32(e.CommandArgument);
                    courseID = Convert.ToInt32(AdminTableView.Rows[ID].Cells[0].Text);
                    Administration.DeleteRecord(courseID);
                    Page_Load(sender, e);
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            btnSave.Text = "Добавить";
            btnCancel.Text = "Выход";
            lbResult.Visible = false;
            btnNew.Visible = false;
            btnExit.Visible = false;
            tbName.Text = "";
            tbUniversity.Text = "";
            tbURL.Text = "";
            for (int i = 0; i < 4; i++)
            {
                chbBool.Items[i].Selected = false;
            }
            cmbProvider.SelectedIndex = 0;
            cmbSubject.SelectedIndex = 0;
            cmbTime.SelectedIndex = 0;
            Panel1.Visible = true;
            AdminTableView.Visible = false;
            IsUpdate = false;
        }


        protected void btnExit_Click(object sender, EventArgs e)
        {
            //IsAdmin.AdminEnter = false;
            Response.Redirect("Admin.aspx", false);
        }

        protected void btnAutoEdit_Click(object sender, EventArgs e)
        {
            //AutoEditWrapper AEW = new AutoEditWrapper();
            //AEW.Start();
            //Parser parser
        }
    }
}