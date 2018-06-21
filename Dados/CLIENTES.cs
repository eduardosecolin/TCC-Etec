using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace BarberSystem.Dados{
    
    [Table("dbo.CLIENTES")]
    public partial class CLIENTES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLIENTES()
        {
            AGENDA = new HashSet<AGENDA>();
        }

        [Key]
        public int codigo { get; set; }

        [StringLength(50)]
        public string nome { get; set; }

        [StringLength(50)]
        public string endereco { get; set; }

        [StringLength(30)]
        public string bairro { get; set; }

        [StringLength(30)]
        public string cidade { get; set; }

        [StringLength(2)]
        public string estado { get; set; }

        [Required]
        [StringLength(20)]
        public string cep { get; set; }

        public int? numero { get; set; }

        [Required]
        [StringLength(20)]
        public string telefone { get; set; }

        [Required]
        [StringLength(20)]
        public string celular { get; set; }

        [Required]
        [StringLength(15)]
        public string sexo { get; set; }

        [Required]
        [StringLength(15)]
        public string status_cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AGENDA> AGENDA { get; set; }
    }
}
