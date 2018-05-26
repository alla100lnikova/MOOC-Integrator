namespace Searcher
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Институт")]
    public partial class Институт
    {
        [StringLength(10)]
        public string Аббревиатура { get; set; }

        public string ПолноеНазвание { get; set; }
    }
}