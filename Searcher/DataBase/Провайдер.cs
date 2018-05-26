namespace Searcher
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Провайдер
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Провайдер()
        {
            Описание_MOOC = new HashSet<Описание_MOOC>();
        }

        public int id { get; set; }

        [StringLength(30)]
        public string Название { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Описание_MOOC> Описание_MOOC { get; set; }
    }
}
