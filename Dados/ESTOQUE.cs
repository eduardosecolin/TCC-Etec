using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberSystem.Dados {
    [Table("dbo.ESTOQUE")]
    public class ESTOQUE {

     [Key]
     public int codigo { get; set; }

     [StringLength(50)]
     public string produto { get; set; }

     public int? quantidade { get; set; }

     [Required]
     public double? vl_produto { get; set; }

     public int? codproduto { get; set; }

     public double? vl_total { get; set; }

     public virtual PRODUTOS PRODUTOS { get; set; }

     // Entrada no estoque
     public void entradaEstoque(int qtd){
            if (this.quantidade == null) {
                this.quantidade = 0;
                this.quantidade += qtd;
            }
            else {
                this.quantidade += qtd;
            }
     }

     // Saida no estoque
     public void saidaEstoque(int qtd){
            if (this.quantidade == null) {
                this.quantidade = 0;
                this.quantidade -= qtd;
            }
            else {
                this.quantidade -= qtd;
            }
     }

     // calcular valor total
     public double? calculaTotal(){
            if (this.quantidade == null) {
                return this.vl_produto;
            }
            else {
                return this.vl_produto * this.quantidade;
            }
     }

    }
}