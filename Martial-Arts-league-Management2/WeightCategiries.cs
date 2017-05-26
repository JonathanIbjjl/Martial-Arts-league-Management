using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contenders
{
   public class WeightCategiries
    {
        public enum WeightCatEnum
        {
            IBJJL=0,
            Nogi=1,
            Generic = 2
        }

        public static Dictionary<string, int> GetWeightCategory(WeightCatEnum w, bool IsChild)
        {
            switch (w)
            {
                case WeightCatEnum.IBJJL:
                    if (IsChild == true)
                        return IBJJL_Child;
                    else
                        return IBJJL_Adult;
                case WeightCatEnum.Nogi:
                    if (IsChild == true)
                        return Nogi_Child;
                    else
                        return Nogi_Adult;
                case WeightCatEnum.Generic:
                    if (IsChild == true)
                        return Generic_Child;
                    else
                        return Generic_Adult;
                default:
                    if (IsChild == true)
                        return IBJJL_Child;
                    else
                        return IBJJL_Adult;
            }

        }


        #region "Basic"

        // basic weight + child
        private static Dictionary<string, int> IBJJL_Child
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("עד 21", 1);
                Weights.Add("מעל 21", 2);
                Weights.Add("עד 24", 3);
                Weights.Add("עד 27", 4);
                Weights.Add("עד 30", 5);
                Weights.Add("מעל 30", 6);
                Weights.Add("עד 34", 7);
                Weights.Add("עד 38", 8);
                Weights.Add("מעל 38", 9);
                Weights.Add("עד 42", 10);
                Weights.Add("עד 46", 11);
                Weights.Add("עד 50", 12);
                Weights.Add("מעל 50", 13);
                Weights.Add("עד 55", 14);
                Weights.Add("מעל 55", 15);
                Weights.Add("עד 60", 16);
                Weights.Add("עד 65", 17);
                Weights.Add("מעל 65", 18);
                Weights.Add("עד 70", 19);
                Weights.Add("עד 75", 20);
                Weights.Add("עד 80", 21);
                Weights.Add("מעל 80", 22);
                return Weights;
            }
        }

        // basic weight + adult
        private static Dictionary<string, int> IBJJL_Adult
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();

                Weights.Add("עד 53.5", 14);
                Weights.Add("עד 58.5", 15);
                Weights.Add("עד 60", 16);
                Weights.Add("עד 64", 17);
                Weights.Add("עד 69", 18);
                Weights.Add("עד 70", 19);
                Weights.Add("עד 74", 20);
                Weights.Add("מעל 74", 21);
                Weights.Add("עד 76", 22);
                Weights.Add("עד 82", 23);
                Weights.Add("עד 88", 24);
                Weights.Add("עד 94", 25);
                Weights.Add("עד 100", 26);
                Weights.Add("מעל 100", 27);
                return Weights;
            }
        }

        #endregion

        #region "NoGI"

        // nogi weight + child
        private static Dictionary<string, int> Nogi_Child
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("18KG", 1);
                Weights.Add("21KG", 2);
                Weights.Add("24KG", 3);
                Weights.Add("27KG", 4);
                Weights.Add("30KG", 5);
                Weights.Add("34KG", 6);
                Weights.Add("38KG", 7);
                Weights.Add("42KG", 8);
                Weights.Add("44.3KG", 9);
                Weights.Add("46KG", 10);
                Weights.Add("48.3KG", 11);
                Weights.Add("50KG", 12);
                Weights.Add("52.5KG", 13);
                Weights.Add("53.5KG", 14);
                Weights.Add("55KG", 15);
                Weights.Add("56.5KG", 16);
                Weights.Add("58.5KG", 17);
                Weights.Add("60.5KG", 18);
                Weights.Add("60KG", 19);
                Weights.Add("64KG", 20);
                Weights.Add("65KG", 21);
                Weights.Add("69KG", 22);
                Weights.Add("70KG", 23);
                Weights.Add("74KG", 24);
                Weights.Add("79.3KG", 25);
                Weights.Add("84.3KG", 26);
                Weights.Add("89.3KG", 27);
                Weights.Add("Maximum", 28);
                return Weights;
            }
        }

        // nogi weight + adult
        private static Dictionary<string, int> Nogi_Adult
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("57.5KG", 17);
                Weights.Add("58.5KG", 18);
                Weights.Add("64KG", 19);
                Weights.Add("69KG", 20);
                Weights.Add("70KG", 21);
                Weights.Add("74KG", 22);
                Weights.Add("76KG", 23);
                Weights.Add("79.3KG", 24);
                Weights.Add("82.3KG", 25);
                Weights.Add("88.3KG", 26);
                Weights.Add("94.3KG", 27);
                Weights.Add("100.5KG", 28);
                Weights.Add("Maximum", 29);
                return Weights;
            }
        }

        #endregion


        #region "Generic"

        // generic weight + child
        private static Dictionary<string, int> Generic_Child
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("18-20", 1);
                Weights.Add("21-23", 2);
                Weights.Add("24-26", 3);
                Weights.Add("27-29", 4);
                Weights.Add("30-32", 5);
                Weights.Add("33-35", 6);
                Weights.Add("36-38", 7);
                Weights.Add("39-41", 8);
                Weights.Add("42-44", 9);
                Weights.Add("45-47", 10);
                Weights.Add("48-50", 11);
                Weights.Add("51-53", 12);
                Weights.Add("54-56", 13);
                Weights.Add("57-59", 14);
                Weights.Add("60-62", 15);
                Weights.Add("63-65", 16);
                Weights.Add("66-68", 17);
                Weights.Add("69-71", 18);
                Weights.Add("72-74", 19);
                Weights.Add("75-77", 20);
                Weights.Add("78-80", 21);
                Weights.Add("81-83", 22);
                Weights.Add("84-86", 23);
                Weights.Add("87-89", 24);
                Weights.Add("90-92", 25);
                return Weights;
            }
        }

        // generic weight + adult
        private static Dictionary<string, int> Generic_Adult
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("42-44", 9);
                Weights.Add("45-47", 10);
                Weights.Add("48-50", 11);
                Weights.Add("51-53", 12);
                Weights.Add("54-56", 13);
                Weights.Add("57-59", 14);
                Weights.Add("60-62", 15);
                Weights.Add("63-65", 16);
                Weights.Add("66-68", 17);
                Weights.Add("69-71", 18);
                Weights.Add("72-74", 19);
                Weights.Add("75-77", 20);
                Weights.Add("78-80", 21);
                Weights.Add("81-83", 22);
                Weights.Add("84-86", 23);
                Weights.Add("87-89", 24);
                Weights.Add("90-92", 25);
                Weights.Add("93-95", 26);
                Weights.Add("96-98", 27);
                Weights.Add("99-101", 28);
                Weights.Add("102-104", 29);
                Weights.Add("105-107", 30);
                Weights.Add("108-110", 31);
                Weights.Add("111-113", 32);
                Weights.Add("114-116", 33);
                Weights.Add("117-119", 34);
                Weights.Add("120-122", 35);
                return Weights;
            }
        }

        #endregion

    }
}
