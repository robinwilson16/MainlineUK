using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainlineUK.Models
{
    public class Photo
    {
        public int PhotoID { get; set; }

        [Required]
        [StringLength(255)]
        public string PhotoPath { get; set; }

        [Required]
        public DateTime DateImported { get; set; }

        public StocklistImport StocklistImport { get; set; }
    }
}
