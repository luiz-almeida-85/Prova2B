using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Prova2B.Models
{
    public class Dados
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Texto:  ")]
        public string txt { get; set; }

    }
}