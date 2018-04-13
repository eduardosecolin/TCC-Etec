using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BarberSystem.Dados {
    public class FORNECEDORES {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public FORNECEDORES(){
            PRODUTOS = new HashSet<PRODUTOS>();
        }
        

        [Key]
        public int? codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string nome { get; set; }

        [Required]
        [StringLength(50)]
        public string endereco { get; set; }

        [Required]
        [StringLength(30)]
        public string bairro { get; set; }

        public int? numero { get; set; }

        [Required]
        [StringLength(30)]
        public string cidade { get; set; }

        [Required]
        [StringLength(20)]
        public string cep { get; set; }

        [Required]
        [StringLength(20)]
        public string telefone { get; set; }

        [Required]
        [StringLength(15)]
        public string tipo { get; set; }

        [Required]
        [StringLength(2)]
        public string estado { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODUTOS> PRODUTOS { get; set; }
    }
}