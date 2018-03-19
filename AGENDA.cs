namespace BarberSystem
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AGENDA
    {
        [Key]
        internal int codigo { get; set; }

        [StringLength(50)]
        public string cliente { get; set; }

        public DateTime? hora_inicio { get; set; }

        public DateTime hora_fim { get; set; }

        [Column(TypeName = "date")]
        public DateTime? data { get; set; }

        internal int? codcliente { get; set; }

        internal int? codbarbeiro { get; set; }

        [StringLength(50)]
        public string nome_barbeiro { get; set; }

        [Required]
        [StringLength(50)]
        public string descricao { get; set; }

        public virtual AGENDA AGENDA1 { get; set; }

        public virtual AGENDA AGENDA2 { get; set; }

        public virtual CLIENTES CLIENTES { get; set; }

        public virtual BARBEIROS BARBEIROS { get; set; }

        public AGENDA(int? codcliente, string cliente, string descricao,DateTime? hora_inicio, DateTime hora_fim, DateTime? data, 
                      int? codbarbeiro, string nome_barbeiro) {
            this.cliente = cliente;
            this.hora_inicio = hora_inicio;
            this.hora_fim = hora_fim;
            this.data = data;
            this.codcliente = codcliente;
            this.codbarbeiro = codbarbeiro;
            this.nome_barbeiro = nome_barbeiro;
            this.descricao = descricao;
        }
    }
}
