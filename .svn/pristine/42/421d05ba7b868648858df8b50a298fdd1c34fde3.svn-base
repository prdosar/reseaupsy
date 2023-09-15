using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ReseauPsy.Models
{
    public class CardNumberTypeRegex
    {
        public string CardTypeName { get; set; }
        public Regex CardRegex { get; set; }

        public string GetCardTypeFromCardNumber(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", "");

            // lien ou trouver info pour la construction des patern "regex"
            // https://stackoverflow.com/questions/72768/how-do-you-detect-credit-card-type-based-on-number

            List<CardNumberTypeRegex> lstRegex = new List<CardNumberTypeRegex>
            {
                new CardNumberTypeRegex { CardTypeName = "VISA", CardRegex = new Regex(@"^4[0-9]{6,}$") }, // Visa
                new CardNumberTypeRegex { CardTypeName = "MASTERCARD", CardRegex = new Regex(@"^(5[1-5][0-9]{5,}|222[1-9][0-9]{3,}|22[3-9][0-9]{4,}|2[3-6][0-9]{5,}|27[01][0-9]{4,}|2720[0-9]{3,})$") }, // MasterCard
                new CardNumberTypeRegex { CardTypeName = "AMEX", CardRegex = new Regex(@"^3[47][0-9]{5,}$") }, // American Express
                new CardNumberTypeRegex { CardTypeName = "DINERS", CardRegex = new Regex(@"^3(?:0[0-5]|[68][0-9])[0-9]{4,}$") }, // Diners Club
                new CardNumberTypeRegex { CardTypeName = "DISCOVER", CardRegex = new Regex(@"^6(?:011|5[0-9]{2})[0-9]{3,}$") }, // Discover
                new CardNumberTypeRegex { CardTypeName = "JCB", CardRegex = new Regex(@"^(?:2131|1800|35[0-9]{3})[0-9]{3,}$") }, // JCB
            };

            // Récupère le type de la carte
            string cardType = "NotValid";
            Match match;
            int lstRegexCount = lstRegex.Count();
            CardNumberTypeRegex cardNumberTypeRegex;
            for (int cptRegex = 0; cptRegex < lstRegexCount; cptRegex++)
            {
                cardNumberTypeRegex = lstRegex[cptRegex];
                match = cardNumberTypeRegex.CardRegex.Match(cardNumber);
                if (match.Success)
                {
                    // Carte valide
                    cardType = cardNumberTypeRegex.CardTypeName;
                    cptRegex = lstRegexCount++; // Trouver, sort de la boucle
                }
            }
            return cardType;
        }
    }
}