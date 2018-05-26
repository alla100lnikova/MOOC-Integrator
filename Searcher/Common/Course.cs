namespace Searcher
{
    public class Course
    {
        protected string m_Name;
        protected string m_URL;
        protected string m_University;
        protected string m_Subject;
        protected string m_StartTime;
        protected string m_Provider;
        protected bool m_IsSertificate;
        protected bool m_IsSchool;
        protected bool m_IsUniversity;
        protected bool m_IsQualification;
        protected int m_StartTimeValue;
        protected int m_SubjectValue;

        /// <summary>
        /// Адрес курса
        /// </summary>
        public string URL
        {
            get { return m_URL; }
            set { m_URL = value; }
        }
        /// <summary>
        /// Название курса
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        /// <summary>
        /// Университет, создавший курс
        /// </summary>
        public string University
        {
            get { return m_University; }
            set { m_University = value; }
        }
        /// <summary>
        /// Предметная область курса
        /// </summary>
        public string Subject
        {
            get { return m_Subject; }
            set { m_Subject = value; }
        }
        /// <summary>
        /// Время начала
        /// </summary>
        public string StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }
        /// <summary>
        /// Значение для предметной области 
        /// </summary>
        public int SubjectValue
        {
            get { return m_SubjectValue; }
            set { m_SubjectValue = value; }
        }
        /// <summary>
        /// Значение времени начала
        /// </summary>
        public int StartTimeValue
        {
            get { return m_StartTimeValue; }
            set { m_StartTimeValue = value; }
        }
        /// <summary>
        /// Провайдер, предоставляющий курс
        /// </summary>
        public string Provider
        {
            get { return m_Provider; }
            set { m_Provider = value; }
        }
        /// <summary>
        /// Наличие сертификата
        /// </summary>
        public bool IsSertificate
        {
            get { return m_IsSertificate; }
            set { m_IsSertificate = value; }
        }
        /// <summary>
        /// Подходит для школьного уровня?
        /// </summary>
        public bool IsSchool
        {
            get { return m_IsSchool; }
            set { m_IsSchool = value; }
        }
        /// <summary>
        /// Уровень высшего образования?
        /// </summary>
        public bool IsUniversity
        {
            get { return m_IsUniversity; }
            set { m_IsUniversity = value; }
        }
        /// <summary>
        /// Курс для повышения квалификации?
        /// </summary>
        public bool IsQualification
        {
            get { return m_IsQualification; }
            set { m_IsQualification = value; }
        }
        /// <summary>
        /// Конструктор для класса "Курс"
        /// </summary>
        /// <param name="name">Название курса</param>
        /// <param name="url">Адрес курса</param>
        /// <param name="provider">Провайдер, предоставляющий курс</param>
        /// <param name="subject">Предетная область</param>
        /// <param name="startTime">Время начала</param>
        /// <param name="university">Институт, создавший курс</param>
        /// <param name="isSertificate">Наличие сертификата</param>
        /// <param name="isSchool">Подходит для школьного уровня?</param>
        /// <param name="isUniversity">Подходит для уровня высшего образования?</param>
        /// <param name="isQualification">Курс для повышения кввалификации?</param>
        public Course(string name, string url, string provider, string subject, string startTime, string university, bool isSertificate, bool isSchool,
            bool isUniversity, bool isQualification, int subvalue = 0, int timevalue = 0)
        {
            m_Name = name;
            m_URL = url;
            m_Provider = provider;
            m_Subject = subject;
            m_StartTime = startTime;
            m_IsSertificate = isSertificate;
            m_IsSchool = isSchool;
            m_IsUniversity = isUniversity;
            m_IsQualification = isQualification;
            m_University = university;
            m_StartTimeValue = timevalue;
            m_SubjectValue = subvalue;
        }

        public Course()
        {

        }
    }
}
