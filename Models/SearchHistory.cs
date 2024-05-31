using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Tasken2.Models
{
    public class SearchHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? KeyWord { get; set; }
        public string? Ip { get; set; }
        public string? RoomFilter { get; set; }

        public string? PriceFilter { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}