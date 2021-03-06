﻿using System;
using System.Globalization;
using System.IO;
using System.Text;
using libphonenumber;
namespace Marketplaats.Winforms.Helper
{
	public static class Extensions
	{
		public static void WriteStringUtf8(this Stream target, string value)
		{
			var encoded = Encoding.UTF8.GetBytes(value);
			target.Write(encoded, 0, encoded.Length);
		}

        /// <summary>
        /// 
        // Produces "+4989123456"
        // number.Format(PhoneNumberUtil.PhoneNumberFormat.E164);
        // Produces "089 123456"
        // number.Format(PhoneNumberUtil.PhoneNumberFormat.NATIONAL);
        // Produces "+49 89 123456"
        // number.Format(PhoneNumberUtil.PhoneNumberFormat.INTERNATIONAL);
        // Produces "011 49 89 123456", the number when it is dialed in the United States.
        // number.FormatOutOfCountryCallingNumber("US");
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static string ToSkypeFormat(this string strNumber)
	    {
            PhoneNumber number;
            try
            {
                number = PhoneNumberUtil.Instance.Parse(strNumber, "NL");
            }
            catch (NumberParseException)
            {
                throw;
            }

            if (number.IsValidNumber)
            {

                return number.Format(PhoneNumberUtil.PhoneNumberFormat.E164);

            }

            return strNumber;
	    }

        public static double ToDouble(this object value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception)
            {

                return 0D;
            }
        }

        public static decimal ToDecimal(this object value)
        {
            try
            {
                return Decimal.Parse(value.ToString(), NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,
                    new CultureInfo("nl-NL"));
            }
            catch (Exception)
            {

                return 0;
            }
        }


        

    }
}