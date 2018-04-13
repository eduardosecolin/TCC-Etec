using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberSystem.Dados {
    public partial class PRODUTOS {

        public PRODUTOS(){
          
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

    }
}