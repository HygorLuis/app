using System;
using System.Text.RegularExpressions;

namespace QueixaAki.Helpers
{
    public static class ValidationHelper
    {
        public static string OnlyNumber(this string sentence)
        {
            if (sentence == null) return null;
            var digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(sentence, "");
        }

        public static bool ValidarCep(this string postCode)
        {
            return postCode != null && Regex.Match(postCode, @"^[0-9]{2}.?[0-9]{3}-[0-9]{3}$").Success;
        }

        public static bool ValidarEMail(this string mail)
        {
            if (mail == null) return false;
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(mail);
        }

        public static bool ValidarCPF(this string cpfCnpj)
        {
            if (string.IsNullOrEmpty(cpfCnpj)) return false;

            var cleanCpfCnpj = cpfCnpj.Clear();

            var d = new int[14];
            var v = new int[2];
            int j, i, sum;

            if (cleanCpfCnpj == string.Empty || new string(cleanCpfCnpj[0], cleanCpfCnpj.Length) == cleanCpfCnpj) return false;

            if (cleanCpfCnpj.Length == 11)
            {
                for (i = 0; i <= 10; i++) d[i] = Convert.ToInt32(cleanCpfCnpj.Substring(i, 1));

                for (i = 0; i <= 1; i++)
                {
                    sum = 0;
                    for (j = 0; j <= 8 + i; j++) sum += d[j] * (10 + i - j);

                    v[i] = (sum * 10) % 11;
                    if (v[i] == 10) v[i] = 0;
                }

                return (v[0] == d[9] & v[1] == d[10]);
            }

            if (cleanCpfCnpj.Length != 14) return false;

            const string sequence = "6543298765432";

            for (i = 0; i <= 13; i++) d[i] = Convert.ToInt32(cleanCpfCnpj.Substring(i, 1));
            {
                for (i = 0; i <= 1; i++)
                {
                    sum = 0;
                    for (j = 0; j <= 11 + i; j++)
                        sum += d[j] * Convert.ToInt32(sequence.Substring(j + 1 - i, 1));

                    v[i] = (sum * 10) % 11;
                    if (v[i] == 10) v[i] = 0;
                }
            }

            return (v[0] == d[12] & v[1] == d[13]);
        }

        public static string Clear(this string stringToClean)
        {
            return stringToClean == null ? null : Regex.Replace(stringToClean, @"[^\d]", string.Empty);
        }
    }
}
