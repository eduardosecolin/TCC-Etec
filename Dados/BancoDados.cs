namespace BarberSystem.Dados {
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BancoDados : DbContext {
        public BancoDados()
            : base("name=BancoDados") {
        }

        public virtual DbSet<AGENDA> AGENDA { get; set; }
        public virtual DbSet<BARBEIROS> BARBEIROS { get; set; }
        public virtual DbSet<CLIENTES> CLIENTES { get; set; }
        public virtual DbSet<USUARIOS> USUARIOS { get; set; }
        public virtual DbSet<CONTAS_PAGAR> CONTAS_PAGAR { get; set; }
        public virtual DbSet<CONTAS_RECEBER> CONTAS_RECEBER { get; set; }
        public virtual DbSet<FORNECEDORES> FORNECEDORES { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<AGENDA>()
                .Property(e => e.cliente)
                .IsUnicode(false);

            modelBuilder.Entity<AGENDA>()
                .Property(e => e.nome_barbeiro)
                .IsUnicode(false);

            modelBuilder.Entity<AGENDA>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .Property(e => e.bairro)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .Property(e => e.cep)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .Property(e => e.sexo)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<BARBEIROS>()
                .HasMany(e => e.AGENDA)
                .WithOptional(e => e.BARBEIROS)
                .HasForeignKey(e => e.codbarbeiro);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.nome)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.bairro)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.cep)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .Property(e => e.sexo)
                .IsUnicode(false);

            modelBuilder.Entity<CLIENTES>()
                .HasMany(e => e.AGENDA)
                .WithOptional(e => e.CLIENTES)
                .HasForeignKey(e => e.codcliente);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.nome_usuario)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.senha)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.bairro)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.estado)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.cpf)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIOS>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<CONTAS_PAGAR>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<CONTAS_RECEBER>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
              .Property(e => e.nome)
              .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.endereco)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.bairro)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.cidade)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.cep)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.telefone)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<FORNECEDORES>()
                .Property(e => e.estado)
                .IsUnicode(false);

        }
    }
}
