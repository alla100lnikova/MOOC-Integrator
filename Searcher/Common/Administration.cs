using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
