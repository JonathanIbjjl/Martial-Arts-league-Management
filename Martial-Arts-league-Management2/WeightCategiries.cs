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
            Generic = 2,
            MMA = 3,
            Boxing = 4
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
                case WeightCatEnum.MMA:
                    if (IsChild == true)
                        return MMA_Child;
                    else
                        return MMA_Adult;
                case WeightCatEnum.Boxing:
                    if (IsChild == true)
                        return Boxing_Child;
                    else
                        return Boxing_Adult;
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

        #region "MMA"

        // MMA weight + child
        private static Dictionary<string, int> MMA_Child
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("Strawweight 115 lb (52.2 kg)", 1);
                Weights.Add("Flyweight 125 lb (56.7 kg)", 2);
                Weights.Add("Bantamweight 135 lb (61.2 kg)", 3);
                Weights.Add("Featherweight 145 lb (65.8 kg)", 4);
                Weights.Add("Lightweight 155 lb (70.3 kg)", 5);
                Weights.Add("Welterweight 170 lb (77.1 kg)", 6);
                Weights.Add("Middleweight 185 lb (83.9 kg)", 7);
                Weights.Add("Light Heavyweight 205 lb (93.0 kg)", 8);
                Weights.Add("Heavyweight 265 lb (120.2 kg)", 9);
                Weights.Add("Maximum", 10);
                return Weights;
            }
        }

        // MMA weight + adult
        private static Dictionary<string, int> MMA_Adult
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("Strawweight 115 lb (52.2 kg)", 1);
                Weights.Add("Flyweight 125 lb (56.7 kg)", 2);
                Weights.Add("Bantamweight 135 lb (61.2 kg)", 3);
                Weights.Add("Featherweight 145 lb (65.8 kg)", 4);
                Weights.Add("Lightweight 155 lb (70.3 kg)", 5);
                Weights.Add("Welterweight 170 lb (77.1 kg)", 6);
                Weights.Add("Middleweight 185 lb (83.9 kg)", 7);
                Weights.Add("Light Heavyweight 205 lb (93.0 kg)", 8);
                Weights.Add("Heavyweight 265 lb (120.2 kg)", 9);
                Weights.Add("Maximum", 10);
                return Weights;
            }
        }

        #endregion

        #region "Boxing"

        // Boxing weight + child
        private static Dictionary<string, int> Boxing_Child
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("Light heavyweight 80 kg (176.4 lb; 12.6 st)", 13);
                Weights.Add("Middleweight 74 kg (163.1 lb; 11.7 st)", 12);
                Weights.Add("Light middleweight 70 kg (154.3 lb; 11.0 st)", 11);
                Weights.Add("Welterweight 66 kg (145.5 lb; 10.4 st)", 10);
                Weights.Add("Light welterweight 63 kg (138.9 lb; 9.9 st)", 9);
                Weights.Add("Lightweight 60 kg (132.3 lb; 9.4 st)", 8);
                Weights.Add("Featherweight 57 kg (125.7 lb; 9.0 st)", 7);
                Weights.Add("Bantamweight 54 kg (119.0 lb; 8.5 st)", 6);
                Weights.Add("Light bantamweight 52 kg (114.6 lb; 8.2 st)", 5);
                Weights.Add("Flyweight 50 kg (110.2 lb; 7.9 st)", 4);
                Weights.Add("Light flyweight 48 kg (105.8 lb; 7.6 st)", 3);
                Weights.Add("Pinweight 46 kg (101.4 lb; 7.2 st)", 2);
                return Weights;
            }
        }

        // Boxing weight + adult
        private static Dictionary<string, int> Boxing_Adult
        {
            get
            {
                Dictionary<string, int> Weights = new Dictionary<string, int>();
                Weights.Add("Super heavyweight Unlimited", 13);
                Weights.Add("Heavyweight 91 kg (200.6 lb; 14.3 st)", 12);
                Weights.Add("Light heavyweight 81 kg (178.6 lb; 12.8 st)", 11);
                Weights.Add("Middleweight 75 kg (165.3 lb; 11.8 st)", 10);
                Weights.Add("Light middleweight 69 kg (152.1 lb; 10.9 st)", 9);
                Weights.Add("Welterweight 69 kg (152.1 lb; 10.9 st)", 8);
                Weights.Add("Light welterweight 64 kg (141.1 lb; 10.1 st)", 7);
                Weights.Add("Lightweight 60 kg (132.3 lb; 9.4 st)", 6);
                Weights.Add("Featherweight 57 kg (125.7 lb; 9.0 st)", 5);
                Weights.Add("Bantamweight 56 kg (123.5 lb; 8.8 st)", 4);
                Weights.Add("Flyweight 52 kg (114.6 lb; 8.2 st)", 3);
                Weights.Add("Light flyweight 49 kg (108.0 lb; 7.7 st)", 2);
                return Weights;
            }
        }

        #endregion

    }
}
