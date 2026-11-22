using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

namespace LocalizationConsoleApp
{
    // A single-file ResourceManager implementation.
    // In a larger project, these dictionaries can be moved to .resx files.
    sealed class DictionaryResourceManager : ResourceManager
    {
        private readonly Dictionary<string, Dictionary<string, string>> _resources =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["en"] = new Dictionary<string, string>
                {
                    ["Title"] = "Multilingual Localization Demo",
                    ["ChooseLanguage"] = "Choose language (fa/en): ",
                    ["NamePrompt"] = "Enter your name: ",
                    ["Welcome"] = "Hello, {0}!",
                    ["DateLabel"] = "Current date:",
                    ["NumberLabel"] = "Sample number:",
                    ["CurrencyLabel"] = "Sample currency:",
                    ["Goodbye"] = "Localization demo finished."
                },
                ["fa"] = new Dictionary<string, string>
                {
                    ["Title"] = "نمونه برنامه چندزبانه",
                    ["ChooseLanguage"] = "زبان را انتخاب کنید (fa/en): ",
                    ["NamePrompt"] = "نام خود را وارد کنید: ",
                    ["Welcome"] = "سلام {0}!",
                    ["DateLabel"] = "تاریخ امروز:",
                    ["NumberLabel"] = "عدد نمونه:",
                    ["CurrencyLabel"] = "مبلغ نمونه:",
                    ["Goodbye"] = "اجرای نمونه چندزبانه پایان یافت."
                }
            };

        public override string GetString(string name, CultureInfo culture)
        {
            string language = culture?.TwoLetterISOLanguageName ?? "en";

            if (_resources.TryGetValue(language, out var selected) &&
                selected.TryGetValue(name, out string value))
                return value;

            if (_resources["en"].TryGetValue(name, out string fallback))
                return fallback;

            return name;
        }
    }

    internal class Program
    {
        private static readonly ResourceManager Resources =
            new DictionaryResourceManager();

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write(Resources.GetString("ChooseLanguage",
                CultureInfo.GetCultureInfo("en-US")));

            string language = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();

            CultureInfo culture;
            if (language == "fa")
                culture = CultureInfo.GetCultureInfo("fa-IR");
            else
                culture = CultureInfo.GetCultureInfo("en-US");

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            ShowLocalizedInformation(culture);
        }

        private static void ShowLocalizedInformation(CultureInfo culture)
        {
            Console.WriteLine();
            Console.WriteLine(Resources.GetString("Title", culture));
            Console.Write(Resources.GetString("NamePrompt", culture));

            string name = Console.ReadLine() ?? "";
            Console.WriteLine(Resources.GetString("Welcome", culture), name);

            DateTime today = DateTime.Now;
            double sampleNumber = 1234567.89;
            decimal sampleCurrency = 987654.32m;

            Console.WriteLine(
                $"{Resources.GetString("DateLabel", culture)} {today.ToString("D", culture)}");

            Console.WriteLine(
                $"{Resources.GetString("NumberLabel", culture)} {sampleNumber.ToString("N2", culture)}");

            Console.WriteLine(
                $"{Resources.GetString("CurrencyLabel", culture)} {sampleCurrency.ToString("C", culture)}");

            Console.WriteLine();
            Console.WriteLine(Resources.GetString("Goodbye", culture));
        }
    }
}
