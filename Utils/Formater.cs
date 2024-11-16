using System.Globalization;

namespace MyLaptopWebsite.Utils
{
    public static class Formater
    {
        public static string ToCurrency(decimal amount)
        {

            CultureInfo culture = new CultureInfo("vi-VN");
            culture.NumberFormat.CurrencyDecimalDigits = 0;

            string formattedValue = amount.ToString("C0", culture);

            return formattedValue.Remove(
                formattedValue.Length - 2, 2
            );
        }
    }
}
