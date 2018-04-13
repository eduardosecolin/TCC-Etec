﻿using System.ComponentModel.DataAnnotations;
using System;

namespace BarberSystem.Dados {
    public partial class CONTAS_RECEBER {

        [Key]
        public int? codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string descricao { get; set; }
        public DateTime? data_vencto { get; set; }
        public DateTime? data_pagto { get; set; }
        public double? vl_unitario { get; set; }
        public double? vl_total { get; set; }

    }
}