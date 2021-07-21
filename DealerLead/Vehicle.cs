using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        public decimal MSRP { get; set; }

        public string StockNumber { get; set; }

        public string Color { get; set; }
        public DateTime? SellDate { get; set; }

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }

        public int ModelId { get; set; }
        public SupportedModel Model { get; set; }

        public int DealershipId { get; set; }
        public Dealership Dealership { get; set; }

    }
}
