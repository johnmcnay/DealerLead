using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class SupportedModel
    {
        [Key]
        public int ModelId { get; set; }

        [Display(Name = "Model")]
        public string ModelName { get; set; }
        
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? ModifyDate { get; set; }

        public int MakeId { get; set; }
        public SupportedMake Make { get; set; }
    }
}
