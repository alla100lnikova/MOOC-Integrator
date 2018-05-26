namespace Searcher
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Группа_ВремяНачала
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Группа_ВремяНачала()
        {
            ВремяНачала = new HashSet<ВремяНачала>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string Название { get; set; }

        public int? Значение { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ВремяНачала> ВремяНачала { get; set; }
    }
}
