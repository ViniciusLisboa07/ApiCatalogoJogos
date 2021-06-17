using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.InputModel
{
    public class JogoInputModel
    {
        [Required]
        [StringLength(100,MinimumLength = 3, ErrorMessage ="O nome do jogo deve ter no minímo três caracteres!")]
        public string Nome { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da produtora deve ter no minímo três caracteres!")]
        public string Produtora { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "O preço deve ser no mínimo um real e no máximo mil reais!")]
        public double Preco { get; set; }
    }
}
