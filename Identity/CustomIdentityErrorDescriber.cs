using Microsoft.AspNetCore.Identity;

namespace _420_15D_FX_H26_TP1.Identity
{
    public class CustomIdentityErrorDescriber:IdentityErrorDescriber
    {


        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"Le nom d'utilisateur '{userName}' est déjà utilisé."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = $"L'adresse courriel '{email}' est déjà utilisée."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"Le mot de passe doit contenir au moins {length} caractères."
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Le mot de passe doit contenir au moins un chiffre."
            };
        }

     

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "Le mot de passe doit contenir au moins une lettre majuscule."
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "Le mot de passe doit contenir au moins une lettre minuscule."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "Le mot de passe doit contenir au moins un caractère spécial."
            };
        }








    }
}
