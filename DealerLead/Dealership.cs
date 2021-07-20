using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class Dealership
    {
        [Key]
        public int DealershipId { get; set; }

        public string DealershipName { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string City { get; set; }

        [Column("StateID")]
        public int StateId { get; set; }

        public string Zipcode { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }

        public int CreatingUserId { get; set; }


    }
}
