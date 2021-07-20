using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerLead
{
    public class DealerLeadUser
    {
        [Key]
        public int UserId { get; set; }

        public Guid AzureAdId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }
    }
    /*        CREATE TABLE DealerLeadUser
(
    UserId INT PRIMARY KEY IDENTITY,
    AzureADId UNIQUEIDENTIFIER NOT NULL,
    CreateDate DATETIME NOT NULL DEFAULT(getdate()),
	CONSTRAINT UNIQUE_AzureADId UNIQUE(AzureADId)
)
*/
}
