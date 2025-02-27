using BankAppWithAPI.Data;
using BankAppWithAPI.Models;
using BankAppWithAPI.Repositories.Interfaces;
using System.Numerics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankAppWithAPI.Repositories.Implementations
{
    public class LuhnNumberRepository(DataContext _context) : ILuhnNumberRepository
    {
        public async Task<string> GenerateUniqueCardNumber(PaymentSystem? paymentSystem)
        {
            string bin = ((int)paymentSystem!).ToString() + "301025";
            string cardNumber = "";
            bool isUnique;

            do
            {
                cardNumber = GenerateRandomCardNumber(bin);
                isUnique = !await _context.Cards.AnyAsync(a => a.CardNumber == cardNumber);
            }
            while (!isUnique);

            return cardNumber;
        }

        private string GenerateRandomCardNumber(string bin)
        {
            StringBuilder sb = new StringBuilder(bin);
            int length = 16;
            bool check = false;

            while (!check)
            {
                // Generating random numbers for the card until we reach the desired length minus 1 (check digit)
                Random random = new Random();
                while (sb.Length < length - 1)
                {
                    sb.Append(random.Next(0, 10));
                }

                // Adding a check digit
                sb.Append(CalculateCheckDigits(sb.ToString()));

                if (ValidateLuhnCheck(sb.ToString()))
                {
                    check = true;
                }
                else
                {
                    sb = new StringBuilder(bin);
                }
            }

            return sb.ToString();
        }

        public async Task<string> GenerateUniqueIBAN()
        {
            string countryCode = "PL";
            string bankCode = "30102521";
            string iban = "";
            bool isUnique;

            do
            {
                iban = GenerateIBAN(bankCode);
                isUnique = !await _context.BankAccounts.AnyAsync(a => a.IBAN == countryCode + iban);
            }
            while (!isUnique);

            return countryCode + iban;
        }

        public string GenerateIBAN(string bankCode)
        {
            bool check = false;
            string result = "";
            while (!check)
            {
                string accountNumber = GenerateRandomAccountNumber();
                // Initial IBAN with check digits 00
                string iban = "00" + bankCode + accountNumber;

                // Calculation of check digits
                int checkDigits = CalculateCheckDigits(iban, "iban");

                // Formatting check digits
                string formattedCheckDigits = checkDigits.ToString("D2");
                result = formattedCheckDigits + bankCode + accountNumber;

                if (ValidateLuhnCheck(result))
                    check = true;
            }

            // Return of final IBAN
            return result;
        }

        private int CalculateCheckDigits(string number, string method = "Card")
        {
            if(method == "iban")
            {
                // Move the country code and check digits to the end
                string rearrangedIBAN = number.Substring(4) + number.Substring(0, 4);
                // Conversion to BigInteger to execute module 97
                BigInteger ibanNumber = BigInteger.Parse(rearrangedIBAN);
                int remainder = (int)(ibanNumber % 97);
                
                // Calculation of check digits
                int checkDigits = 98 - remainder;
                return checkDigits;
            }
            else
            {
                int sum = 0;
                bool alternate = false;
                int n = 0;

                for (int i = number.Length - 1; i >= 0; i--)
                {
                    n = int.Parse(number[i].ToString());

                    if (alternate)
                    {
                        n *= 2;
                        if (n > 9)
                            n -= 9;
                    }

                    sum += n;
                    alternate = !alternate;
                }

                int checkDigit = (10 - sum % 10) % 10;
                return checkDigit;
            }

        }

        private string GenerateRandomAccountNumber()
        {
            var random1 = new Random();
            return random1.Next(10000000, 99999999).ToString("D8") + random1.Next(10000000, 99999999).ToString("D8");
        }

        private bool ValidateLuhnCheck(string number)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(number[i].ToString());

                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                        n -= 9;
                }

                sum += n;
                alternate = !alternate;
            }

            return sum % 10 == 0;
        }
    }
}
