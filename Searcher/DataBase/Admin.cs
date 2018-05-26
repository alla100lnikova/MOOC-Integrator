namespace Searcher
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Admin")]
    public partial class Admin
    {
        public int id { get; set; }

        [StringLength(50)]
        public string Login { get; set; }

        [StringLength(40)]
        public string PassHash { get; set; }

        [StringLength(50)]
        public string Salt { get; set; }
    }
}
