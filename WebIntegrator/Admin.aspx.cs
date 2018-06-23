using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Searcher;

namespace WebIntegrator
{
    public class IsAdmin
    {
        public static bool AdminEnter;
    }

    public partial class AdminLogin : System.Web.UI.Page
    {
        bool isLoginValid = false;
        bool isPassValid = false;

        public bool IsLoginValid(string log)
        {
            Regex r = new Regex(@"^[a-zA-Z0-9а-яА-Я\s_]{2,30}$");
            return r.IsMatch(log);
        }
        public bool IsPassValid(string Pass)
        {
            Regex r = new Regex(@"^[\S]{6,30}$");
            return r.IsMatch(Pass);
        }

        public static byte[] GenerateSALT()
        {
            byte[] data = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return data;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;

        }

        static string CalcHash(string path, string salt)
        {
            using (var cp = new SHA1CryptoServiceProvider()) // считаем хэш криптопровайдером
            {
                return BitConverter.ToString(cp.ComputeHash(GetBytes(path + salt))).Replace("-", ""); // конвертнем в строку и все - заменим на ничего
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            btnEnter.Visible = true;
            btnEnterSystem.Visible = false;
            btnRegister.Visible = false;
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            lbResult.Visible = false;
            using (var ctx = new MOOCEntities())
            {
                foreach (var course1 in ctx.Admin)
                {
                    if (tbLogin.Text == course1.Login && CalcHash(tbPassword.Text, course1.Salt) == course1.PassHash)
                    {
                        lbResult.Visible = true;
                        lbResult.Text = "Вы вошли. Что хотите сделать?";
                        btnEnter.Visible = false;
                        btnEnterSystem.Visible = true;
                        btnRegister.Visible = true;
                        return;
                    }
                }
            }
            lbResult.Visible = true;
            lbResult.Text = "Неверный логин или пароль";
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            tbLogin.Text = "";
            lbResult.Visible = false;
            btnEnter.Visible = false;
            tbPassword.Text = "";
            btnRegister.Visible = false;
            btnEnterSystem.Visible = false;
            btnCansel.Visible = true;
            btnRegisterAdmin.Visible = true;
        }

        protected void btnEnterSystem_Click(object sender, EventArgs e)
        {
            btnCansel.Visible = false;
            btnEnterSystem.Visible = false;
            btnRegister.Visible = false;
            btnRegisterAdmin.Visible = false;
            btnEnter.Visible = true;
            lbResult.Visible = false;
            IsAdmin.AdminEnter = true;
            Response.Redirect("AdminTable.aspx", true);
        }

        protected void btnRegisterAdmin_Click(object sender, EventArgs e)
        {
            lbResult.Visible = false;
            btnEnter.Visible = false;
            if (IsLoginValid(tbLogin.Text))
            {
                if (IsPassValid(tbPassword.Text))
                {
                    lbResult.Visible = false;
                    using (var ctx = new MOOCEntities())
                    {
                        foreach (var course1 in ctx.Admin)
                        {
                            if (tbLogin.Text == course1.Login)
                            {
                                lbResult.Visible = true;
                                lbResult.Text = "Такой пользователь уже существует";
                                return;
                            }
                        }
                    }

                    using (var ctx = new MOOCEntities())
                    {
                        Admin newadmin = new Admin();
                        newadmin.Login = tbLogin.Text;
                        string salt = GetString(GenerateSALT());
                        newadmin.PassHash = CalcHash(tbPassword.Text, salt);
                        newadmin.Salt = salt;

                        ctx.Admin.Add(newadmin);
                        ctx.SaveChanges();

                        lbResult.Visible = true;
                        lbResult.Text = "Регистрация успешна";
                        btnRegisterAdmin.Visible = false;
                        btnRegister.Visible = true;
                        btnEnterSystem.Visible = true;
                        btnCansel.Visible = false;
                        btnEnter.Visible = false;
                    }
                }
                else
                {
                    lbResult.Visible = true;
                    lbResult.Text = "Пароль должен быть длиннее 8 символов и короче 30 и не может содержать пробелы";
                }
            }
            else
            {
                lbResult.Visible = true;
                lbResult.Text = "Логин должен содержать только цифры и буквы и должен быть длиннее двух символов и короче 30 ";
            }
        }

        protected void btnCansel_Click(object sender, EventArgs e)
        {
            lbResult.Visible = true;
            lbResult.Text = "Что Вы хотите сделать?";
            btnEnter.Visible = false;
            btnRegisterAdmin.Visible = false;
            btnRegister.Visible = true;
            btnEnterSystem.Visible = true;
            btnCansel.Visible = false;
        }
    }
}