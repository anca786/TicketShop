
using System.ComponentModel.DataAnnotations;

namespace TicketShop.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu")]
        [StringLength(50,MinimumLength = 3, ErrorMessage = "Numele trebuie să aibă între 3 și 50 de caractere.")]
        public string Nume { get; set; }

        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Prenumele trebuie să aibă între 3 și 50 de caractere.")]
        public string Prenume { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Formatul email-ului este invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8 ,ErrorMessage = "Parola trebuie să aibă cel puțin {2} caractere.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Parola trebuie să conțină o majusculă, o cifră și un caracter special.")]
        public string Password { get; set; }

        [Display(Name = "Confirmă parola")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parolele nu coincid.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Formatul email-ului nu este valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Ține-mă minte")]
        public bool RememberMe { get; set; }
    }
}
