using System.IO;

namespace Searcher
{
    public class Administration
    {
        public static void DeleteRecord(int courseID)
        {
            Описание_MOOC course = new Описание_MOOC();
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
                ctx.Описание_MOOC.Remove(course);
                ctx.SaveChanges();
            }
        }

        public static void ToLog(string Text)
        {
            //StreamWriter Writer = new StreamWriter("Log.txt", true, System.Text.Encoding.GetEncoding(1251));
            //Writer.WriteLine(Text);
            //Writer.Close();
        }
    }
}
