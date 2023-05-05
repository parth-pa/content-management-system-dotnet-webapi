using System.ComponentModel.DataAnnotations;

namespace keyclock_Authentication.Model
{
    public class UserLogin
    {
        [Required]
        public string? userName { get; set; }
        [Required]

        public string? password { get; set; }
    }
}
