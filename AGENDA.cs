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
        public int codigo { get; set; }

        [StringLength(50)]
        public string cliente { get; set; }

        [StringLength(50)]
        public string descicao { get; set; }

        public DateTime? hora_inicio { get; set; }

        public DateTime hora_fim { get; set; }

        [Column(TypeName = "date")]
        public DateTime? data { get; set; }

        public int? codcliente { get; set; }

        public int? codbarbeiro { get; set; }

        [StringLength(50)]
        public string nome_barbeiro { get; set; }

        public virtual AGENDA AGENDA1 { get; set; }

        public virtual AGENDA AGENDA2 { get; set; }

        public virtual CLIENTES CLIENTES { get; set; }

        public virtual BARBEIROS BARBEIROS { get; set; }
    }
}
