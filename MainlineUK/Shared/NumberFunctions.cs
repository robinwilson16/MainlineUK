using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainlineUK.Shared
{
    public class NumberFunctions
    {
        public static int CurrencyToInt(string currency)
        {
            int currencyNum = 0;

            if(currency != null)
            {
                currency = currency.Replace("£", "");
                currency = currency.Replace(",", "");

                //Added double part in case pence are returned
                currencyNum = Convert.ToInt32(Convert.ToDouble(currency));
            }

            return currencyNum;
        }
    }
}
