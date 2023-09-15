using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Helper
{
    public static class VerifyIdentityEmail
    {
        /// <summary>
        /// Verifie si le courriel identity existe
        /// </summary>
        /// <param name="_context">Le database context</param>
        /// <param name="email">Le courriel a vérifier</param>
        /// <returns>False si le courriel existe, true si le courriel est disponible</returns>
        public static bool VerifyEmailAvailabillity(ReseauPsyEntities _context, string email)
        {
            var usedEmails = _context.AspNetUsers.Select(e => e.Email.Trim().ToLower()).ToList();

            if (usedEmails.Contains(email.Trim().ToLower()))
                return false;

            return true;
        }
    }
}
