using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Blog.Domain.Entities
{
    [Table("Noticia")]
    public class Noticia
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        
        [MaxLength(255)] 
        public string Chapeu { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataPublicacao { get; set; }
        
        [MaxLength(255)] 
        public string Autor { get; set; } = string.Empty;
    }
}
