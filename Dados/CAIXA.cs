using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace BarberSystem.Dados {

    [Table("dbo.CAIXA")]
    public class CAIXA {
     
     [Key]
     public int codigo { get; set; }

     public double? entrada { get; set; }

     public double? saida { get; set; }

     public decimal? vl_total { get; set; }

      public void entradaCaixa(decimal? valor){
            try {
                if (this.vl_total == null) {
                    this.vl_total = 0;
                    this.vl_total += valor;
                }
                else {
                    this.vl_total += valor;
                }
            }catch(Exception){
                throw;
            }
      }

      public void saidaCaixa(decimal? valor){
            try {
                if (this.vl_total == null) {
                    this.vl_total = 0;
                    this.vl_total -= valor;
                }
                else {
                    this.vl_total -= valor;
                }
            }catch(Exception){
                throw;
            }
      }

    }
}