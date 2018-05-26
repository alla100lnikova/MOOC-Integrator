namespace Searcher
{
    using System.Data.Entity;

    public partial class MOOC : DbContext
    {
        public MOOC()
            : base("name=MOOC")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<ВремяНачала> ВремяНачала { get; set; }
        public virtual DbSet<Группа_ВремяНачала> Группа_ВремяНачала { get; set; }
        public virtual DbSet<Группа_ПредметнаяОбласть> Группа_ПредметнаяОбласть { get; set; }
        public virtual DbSet<Описание_MOOC> Описание_MOOC { get; set; }
        public virtual DbSet<ПредметнаяОбласть> ПредметнаяОбласть { get; set; }
        public virtual DbSet<Провайдер> Провайдер { get; set; }
        public virtual DbSet<Институт> Институт { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.PassHash)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ВремяНачала>()
                .HasMany(e => e.Описание_MOOC)
                .WithOptional(e => e.ВремяНачала1)
                .HasForeignKey(e => e.ВремяНачала);

            modelBuilder.Entity<Группа_ВремяНачала>()
                .HasMany(e => e.ВремяНачала)
                .WithOptional(e => e.Группа_ВремяНачала)
                .HasForeignKey(e => e.Группа);

            modelBuilder.Entity<Группа_ПредметнаяОбласть>()
                .HasMany(e => e.ПредметнаяОбласть)
                .WithOptional(e => e.Группа_ПредметнаяОбласть)
                .HasForeignKey(e => e.Группа);

            modelBuilder.Entity<ПредметнаяОбласть>()
                .HasMany(e => e.Описание_MOOC)
                .WithOptional(e => e.ПредметнаяОбласть1)
                .HasForeignKey(e => e.ПредметнаяОбласть);

            modelBuilder.Entity<Провайдер>()
                .HasMany(e => e.Описание_MOOC)
                .WithRequired(e => e.Провайдер1)
                .HasForeignKey(e => e.Провайдер)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Институт>()
                .Property(e => e.Аббревиатура)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
