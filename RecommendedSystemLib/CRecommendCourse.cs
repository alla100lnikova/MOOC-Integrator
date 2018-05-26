using Searcher;

namespace RecommendedSystemLib
{
    public class CRecommendCourse : Course
    {
        private CName m_NameCharacters;
        private CCharacters m_CourseCharacters;

        public CRecommendCourse(string name, string url, string provider, string subject, int subvalue, string startTime, 
            int timevalue, string university, bool isSertificate, bool isSchool, bool isUniversity, bool isQualification)
            : base(name, url, provider, subject, startTime,  university, isSertificate, isSchool, isUniversity, isQualification, subvalue, timevalue)
        {
            m_NameCharacters = new CName(m_Name);
            m_CourseCharacters = new CCharacters(m_SubjectValue, m_StartTimeValue, m_IsSertificate, m_IsSchool, m_IsUniversity, m_IsQualification);
        }

        public CRecommendCourse(Course course)
            : base(course.Name, course.URL, course.Provider, course.Subject, course.StartTime, course.University, 
                  course.IsSertificate, course.IsSchool, course.IsUniversity, course.IsQualification, course.SubjectValue, course.StartTimeValue)
        {
            m_NameCharacters = new CName(m_Name);
            m_CourseCharacters = new CCharacters(m_SubjectValue, m_StartTimeValue, m_IsSertificate, m_IsSchool, m_IsUniversity, m_IsQualification);
        }

        public CName NameCharacters
        {
            get { return m_NameCharacters; }
        }

        public CCharacters CourseCharacters
        {
            get { return m_CourseCharacters; }
        }
    }
}
