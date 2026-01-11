using System.ComponentModel.DataAnnotations;

namespace TicketShop.Models.ViewModels
{
    public class UserSettingsViewModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Nume { get; set; }

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        public string Prenume { get; set; }

        public string? Email { get; set; }



        [DataType(DataType.Password)]
        [Display(Name = "Parola Actuală")]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Parola Nouă")]
        [MinLength(8, ErrorMessage = "Parola trebuie să aibă minim 8 caractere")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Parola trebuie să conțină o majusculă, o cifră și un caracter special.")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmă Parola Nouă")]
        [Compare("NewPassword", ErrorMessage = "Parolele nu coincid.")]
        public string? ConfirmNewPassword { get; set; }
    }
}