namespace Searcher
{
    using System.ComponentModel.DataAnnotations;

    public partial class Описание_MOOC
    {
        public int id { get; set; }

        [Required]
        public string URL { get; set; }

        [Required]
        public string НазваниеКурса { get; set; }

        public int Провайдер { get; set; }

        public string Институт { get; set; }

        public int? ПредметнаяОбласть { get; set; }

        public int? ВремяНачала { get; set; }

        public bool? НаличиеСертификата { get; set; }

        public bool? Школа { get; set; }

        public bool? ВысшееОбразование { get; set; }

        public bool? ПовышениеКвалификации { get; set; }

        public virtual ВремяНачала ВремяНачала1 { get; set; }

        public virtual ПредметнаяОбласть ПредметнаяОбласть1 { get; set; }

        public virtual Провайдер Провайдер1 { get; set; }
    }
}
