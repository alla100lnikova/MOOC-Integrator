namespace Searcher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ВремяНачала
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ВремяНачала()
        {
            Описание_MOOC = new HashSet<Описание_MOOC>();
        }

        public int id { get; set; }

        [StringLength(20)]
        public string Название { get; set; }

        public int? Группа { get; set; }

        public virtual Группа_ВремяНачала Группа_ВремяНачала { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Описание_MOOC> Описание_MOOC { get; set; }
    }
}
