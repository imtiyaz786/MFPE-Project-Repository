using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemberPortal.ViewModel
{
    public class MemberLogin
    {
        [DataType(DataType.Text)]
        [RegularExpression(@"[a-zA-z,-]{3,}$", ErrorMessage = "Please follow the [a-zA-z,-] pattern and Enter minimum 3 characters.")]
        [Required(ErrorMessage = "Username can't be empty!")]
        public string Username { get; set; }        


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can't be empty!")]
        [StringLength(100, ErrorMessage = "Must be between 5 and 25 characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
