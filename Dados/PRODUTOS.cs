using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberSystem.Dados {

    [Table("dbo.PRODUTOS")]
    public partial class PRODUTOS {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public PRODUTOS(){
            ESTOQUE = new HashSet<ESTOQUE>();
        }
       
        [Key]
        public int codigo { get; set; }
        
        [StringLength(50)]
        public string descricao { get; set; }

        public double? vl_unitario { get; set; }

        public int? codfornecedor { get; set; }

        [StringLength(50)]
        public string nome_fornecedor { get; set; }

        public virtual FORNECEDORES FORNECEDORES { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ESTOQUE> ESTOQUE { get; set; }

    }
}