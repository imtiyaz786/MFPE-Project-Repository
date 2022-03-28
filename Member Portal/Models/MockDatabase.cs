using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemberPortal.Models
{
    
    public class MockDatabase
    {

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionID { get; set; }

        [Required(ErrorMessage = "Member ID can't be empty!")]
        [Range(1,3,ErrorMessage ="Entered Member ID is not valid!")]
        
        public int MemberID { get; set; }


        [Required(ErrorMessage = "Policy ID can't be empty!")]
        [Range(1,3, ErrorMessage = "Entered Policy ID is not valid!")]
        public int PolicyID { get; set; }


        [Required(ErrorMessage = "Claim ID can't be empty!")]
        [Range(1,3, ErrorMessage = "Entered Claim ID is not valid!")]
        public int ClaimID { get; set; }

        [Required(ErrorMessage = "Benefit ID can't be empty!")]
        [Range(1,2, ErrorMessage = "Entered Benefit ID is not valid!")]
        public int BenefitID { get; set; }

        [Required(ErrorMessage = "Hospital ID can't be empty!")]
        [DisplayName("HospitalID")]
        [Range(1,2, ErrorMessage = "Entered Hospital ID is not valid!")]
        public int HospitalId { get; set; }

        [Required(ErrorMessage ="Claim amount can't be empty!")]
        public double ClaimAmount { get; set; }

        [Required(ErrorMessage ="Benefit name can't be empty ")]
        public string BenefitName { get; set; }        
               
    }
}
