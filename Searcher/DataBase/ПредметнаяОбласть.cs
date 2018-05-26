namespace Searcher
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ПредметнаяОбласть
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ПредметнаяОбласть()
        {
            Описание_MOOC = new HashSet<Описание_MOOC>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string Название { get; set; }

        public int? Группа { get; set; }

        public virtual Группа_ПредметнаяОбласть Группа_ПредметнаяОбласть { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Описание_MOOC> Описание_MOOC { get; set; }
    }
}
