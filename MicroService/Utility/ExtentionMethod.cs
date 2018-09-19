using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Controllers
{



    public enum Language
    {
        /// <summary>
        /// English Language
        /// </summary>
        English,

        /// <summary>
        /// Persian Language
        /// </summary>
        Persian
    }

    public enum DigitGroup
    {
        /// <summary>
        /// Ones group
        /// </summary>
        Ones,

        /// <summary>
        /// Teens group
        /// </summary>
        Teens,

        /// <summary>
        /// Tens group
        /// </summary>
        Tens,

        /// <summary>
        /// Hundreds group
        /// </summary>
        Hundreds,

        /// <summary>
        /// Thousands group
        /// </summary>
        Thousands
    }

    public class NumberWord
    {
        /// <summary>
        /// Digit's group
        /// </summary>
        public DigitGroup Group { set; get; }

        /// <summary>
        /// Number to word language
        /// </summary>
        public Language Language { set; get; }

        /// <summary>
        /// Equivalent names
        /// </summary>
        public IList<string> Names { set; get; }
    }


    public static class ExtentionMethod
    {
        /// <summary>
        /// converts the all data rows of data table to dynamic object
        /// </summary>
        /// <returns>the equivalent dynamic object/returns>
        public static List<dynamic> toDynamic(this DataTable dataTable)
        {
            var dynamics = new List<dynamic>();

            foreach (DataRow dataRow in dataTable.Rows)
            {

                dynamic expandoObject = new ExpandoObject();
                IDictionary<string, object> myUnderlyingObject = expandoObject;

                foreach (DataColumn column in dataTable.Columns)
                {

                    myUnderlyingObject.Add(column.ColumnName, dataRow[column]); // Adding dynamically named property

                }

                dynamics.Add(expandoObject);
            }
            return dynamics;
        }

  
        public static List<dynamic> toDynamic(this DataTable dataTable, int tillColumn)
        {
            var dynamics = new List<dynamic>();

            foreach (DataRow dataRow in dataTable.Rows)
            {

                dynamic expandoObject = new ExpandoObject();
                IDictionary<string, object> myUnderlyingObject = expandoObject;


                for (int i = 0; i < tillColumn; i++)
                {
                    DataColumn column = dataTable.Columns[i];
                    myUnderlyingObject.Add(column.ColumnName, dataRow[column]); // Adding dynamically named property


                }
                //foreach (DataColumn column in dataTable.Columns)
                //{

                //    myUnderlyingObject.Add(column.ColumnName, dataRow[column]); // Adding dynamically named property

                //}

                dynamics.Add(expandoObject);
            }
            return dynamics;
        }

        public static double calculateSum(this DataTable dataTable, int columnIndex)
        {

            double dblSum = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {

                try
                {
                    dblSum += Convert.ToDouble(dataRow[columnIndex].ToString().Replace(",", ""));

                }
                catch (Exception)
                {

                }

            }
            return dblSum;
        }

        public static double calculateAvg(this DataTable dataTable, int columnIndex)
        {

            if (dataTable.Rows.Count == 0)
                return 0;

            double dblSum = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                try
                {
                    dblSum += Convert.ToDouble(dataRow[columnIndex].ToString().Replace(",", ""));

                }
                catch (Exception)
                {

                }
            }
            return dblSum / (double)dataTable.Rows.Count;
        }

        public static double calculateMax(this DataTable dataTable, int columnIndex)
        {
            if (dataTable.Rows.Count == 0)
                return 0;

            double dblMax = double.MinValue;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                double dblValue = 0;

                try
                {
                    dblValue = Convert.ToDouble(dataRow[columnIndex].ToString().Replace(",", ""));
                }
                catch (Exception)
                {

                }
                if (dblMax < dblValue)
                    dblMax = dblValue;
            }
            return dblMax;
        }

        public static double calculateMin(this DataTable dataTable, int columnIndex)
        {

            if (dataTable.Rows.Count == 0)
                return 0;

            double dblMin = double.MaxValue;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                double dblValue = 0;

                try
                {
                    dblValue = Convert.ToDouble(dataRow[columnIndex].ToString().Replace(",", ""));
                }
                catch (Exception)
                {

                }
                if (dblMin > dblValue)
                    dblMin = dblValue;
            }
            return dblMin;
        }
        public static dynamic toDynamic(this DataRow dataRow)
        {

            dynamic expandoObject = new ExpandoObject();
            IDictionary<string, object> myUnderlyingObject = expandoObject;

            foreach (DataColumn column in dataRow.Table.Columns)
            {

                myUnderlyingObject.Add(column.ColumnName, dataRow[column]); // Adding dynamically named property

            }


            return expandoObject;
        }

      
        /// <summary>
        /// converts the data row to dynamic object
        /// </summary>
        /// <returns>the equivalent dynamic object/returns>
        public static dynamic toDynamic(this DataRow dataRow, int tillColumn)
        {

            dynamic expandoObject = new ExpandoObject();
            IDictionary<string, object> myUnderlyingObject = expandoObject;

            for (int i = 0; i < tillColumn; i++)
            {
                DataColumn column = dataRow.Table.Columns[i];
                myUnderlyingObject.Add(column.ColumnName, dataRow[column]); // Adding dynamically named property


            }
            //foreach (DataColumn column in dataRow.Table.Columns)
            //{

            //    myUnderlyingObject.Add(column.ColumnName, dataRow[column]); // Adding dynamically named property

            //}


            return expandoObject;
        }

        /// <summary>
        /// returns a persian date with format yyyy/MM/dd
        /// </summary>
        /// <returns>the equivalent perisan date</returns>
        public static string toPersianDate(this DateTime dateTime)
        {
            var oPersianCalendar = new System.Globalization.PersianCalendar();

            return (oPersianCalendar.GetYear(dateTime).ToString("0000") + "/"
                + oPersianCalendar.GetMonth(dateTime).ToString("00") + "/"
                + oPersianCalendar.GetDayOfMonth(dateTime).ToString("00"));

        }
        /// <summary>
        /// returns a persian date time with format yyyy/MM/dd HH:mm
        /// </summary>
        /// <returns>the equivalent perisan date time</returns>
        public static string toPersianDateTime(this DateTime dateTime)
        {

            var strPersianDate = dateTime.toPersianDate();

            return (strPersianDate + " " + dateTime.ToString("HH:mm"));

        }



    }


    public static class HumanReadableInteger
    {
        #region Fields (4)

        private static readonly IDictionary<Language, string> And = new Dictionary<Language, string>
  {
   { Language.English, " " },
   { Language.Persian, " و " }
  };
        private static readonly IList<NumberWord> NumberWords = new List<NumberWord>
  {
   new NumberWord { Group= DigitGroup.Ones, Language= Language.English, Names=
    new List<string> { string.Empty, "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" }},
   new NumberWord { Group= DigitGroup.Ones, Language= Language.Persian, Names=
    new List<string> { string.Empty, "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" }},

   new NumberWord { Group= DigitGroup.Teens, Language= Language.English, Names=
    new List<string> { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" }},
   new NumberWord { Group= DigitGroup.Teens, Language= Language.Persian, Names=
    new List<string> { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" }},

   new NumberWord { Group= DigitGroup.Tens, Language= Language.English, Names=
    new List<string> { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" }},
   new NumberWord { Group= DigitGroup.Tens, Language= Language.Persian, Names=
    new List<string> { "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" }},

   new NumberWord { Group= DigitGroup.Hundreds, Language= Language.English, Names=
    new List<string> {string.Empty, "One Hundred", "Two Hundred", "Three Hundred", "Four Hundred",
     "Five Hundred", "Six Hundred", "Seven Hundred", "Eight Hundred", "Nine Hundred" }},
   new NumberWord { Group= DigitGroup.Hundreds, Language= Language.Persian, Names=
    new List<string> {string.Empty, "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد" , "نهصد" }},

   new NumberWord { Group= DigitGroup.Thousands, Language= Language.English, Names=
     new List<string> { string.Empty, " Thousand", " Million", " Billion"," Trillion", " Quadrillion", " Quintillion", " Sextillian",
   " Septillion", " Octillion", " Nonillion", " Decillion", " Undecillion", " Duodecillion", " Tredecillion",
   " Quattuordecillion", " Quindecillion", " Sexdecillion", " Septendecillion", " Octodecillion", " Novemdecillion",
   " Vigintillion", " Unvigintillion", " Duovigintillion", " 10^72", " 10^75", " 10^78", " 10^81", " 10^84", " 10^87",
   " Vigintinonillion", " 10^93", " 10^96", " Duotrigintillion", " Trestrigintillion" }},
   new NumberWord { Group= DigitGroup.Thousands, Language= Language.Persian, Names=
     new List<string> { string.Empty, " هزار", " میلیون", " میلیارد"," تریلیون", " Quadrillion", " Quintillion", " Sextillian",
   " Septillion", " Octillion", " Nonillion", " Decillion", " Undecillion", " Duodecillion", " Tredecillion",
   " Quattuordecillion", " Quindecillion", " Sexdecillion", " Septendecillion", " Octodecillion", " Novemdecillion",
   " Vigintillion", " Unvigintillion", " Duovigintillion", " 10^72", " 10^75", " 10^78", " 10^81", " 10^84", " 10^87",
   " Vigintinonillion", " 10^93", " 10^96", " Duotrigintillion", " Trestrigintillion" }},
  };
        private static readonly IDictionary<Language, string> Negative = new Dictionary<Language, string>
  {
   { Language.English, "Negative " },
   { Language.Persian, "منهای " }
  };
        private static readonly IDictionary<Language, string> Zero = new Dictionary<Language, string>
  {
   { Language.English, "Zero" },
   { Language.Persian, "صفر" }
  };

        #endregion Fields

        #region Methods (7)

        // Public Methods (5)

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string toTextNumber(this int number, Language language)
        {
            return toTextNumber((long)number, language);
        }


        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string toTextNumber(this uint number, Language language)
        {
            return toTextNumber((long)number, language);
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string toTextNumber(this byte number, Language language)
        {
            return toTextNumber((long)number, language);
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string toTextNumber(this decimal number, Language language)
        {
            return toTextNumber((long)number, language);
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string toTextNumber(this double number, Language language)
        {
            return toTextNumber((long)number, language);
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public static string toTextNumber(this long number, Language language)
        {
            if (number == 0)
            {
                return Zero[language];
            }

            if (number < 0)
            {
                return Negative[language] + toTextNumber(-number, language);
            }

            return wordify(number, language, string.Empty, 0);
        }
        // Private Methods (2)

        private static string getName(int idx, Language language, DigitGroup group)
        {
            return NumberWords.Where(x => x.Group == group && x.Language == language).First().Names[idx];
        }

        private static string wordify(long number, Language language, string leftDigitsText, int thousands)
        {
            if (number == 0)
            {
                return leftDigitsText;
            }

            var wordValue = leftDigitsText;
            if (wordValue.Length > 0)
            {
                wordValue += And[language];
            }

            if (number < 10)
            {
                wordValue += getName((int)number, language, DigitGroup.Ones);
            }
            else if (number < 20)
            {
                wordValue += getName((int)(number - 10), language, DigitGroup.Teens);
            }
            else if (number < 100)
            {
                wordValue += wordify(number % 10, language, getName((int)(number / 10 - 2), language, DigitGroup.Tens), 0);
            }
            else if (number < 1000)
            {
                wordValue += wordify(number % 100, language, getName((int)(number / 100), language, DigitGroup.Hundreds), 0);
            }
            else
            {
                wordValue += wordify(number % 1000, language, wordify(number / 1000, language, string.Empty, thousands + 1), 0);
            }

            if (number % 1000 == 0) return wordValue;
            return wordValue + getName(thousands, language, DigitGroup.Thousands);
        }

        #endregion Methods
    }
}
