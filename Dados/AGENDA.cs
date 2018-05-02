using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace BarberSystem.Dados{
    
    [Table("dbo.AGENDA")]
    public partial class AGENDA
    {
        [Key]
        public int codigo { get; set; }

        [StringLength(50)]
        public string cliente { get; set; }

        public DateTime? hora_inicio { get; set; }

        public DateTime? hora_fim { get; set; }

        [Column(TypeName = "date")]
        public DateTime? data { get; set; }

        public int? codcliente { get; set; }

        public int? codbarbeiro { get; set; }

        [StringLength(50)]
        public string nome_barbeiro { get; set; }

        [Required]
        [StringLength(50)]
        public string descricao { get; set; }
        
        public virtual CLIENTES CLIENTES { get; set; }

        public virtual BARBEIROS BARBEIROS { get; set; }

    }
}
