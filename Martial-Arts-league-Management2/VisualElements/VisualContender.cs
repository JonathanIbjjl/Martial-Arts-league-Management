using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MartialArts;
namespace Visual
{
    [Serializable]
 public   class VisualElements
    {
        [Serializable]
        protected struct BeltColors
        {
            public Color DarkColor;
            public Color MediumColor;
            public Color LightColor;
        }
        protected string FactorSign
        {
            get
            {
                return "✔";
            }
        }
        protected string NoFactorSign
        {
            get
            {
                return "";
            }
        }
        protected BeltColors GetBeltShades(Contenders.ContndersGeneral.BeltsEnum belt)
        {
            var Cshapes = new BeltColors();

            switch (belt)
            {
                case Contenders.ContndersGeneral.BeltsEnum.white:
                    Cshapes.DarkColor = Color.FromArgb(222, 222, 222);
                    Cshapes.MediumColor = Color.FromArgb(240, 240, 240);
                    Cshapes.LightColor = Color.FromArgb(255, 255, 255);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.yellow:
                    Cshapes.DarkColor = Color.FromArgb(204, 204, 0);
                    Cshapes.MediumColor = Color.FromArgb(255, 255, 0);
                    Cshapes.LightColor = Color.FromArgb(255, 255, 51);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.purpule:
                    Cshapes.LightColor = Color.FromArgb(51, 0, 51);
                    Cshapes.MediumColor = Color.FromArgb(51, 0, 102);
                    Cshapes.DarkColor = Color.FromArgb(76, 0, 153);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.orange:
                    Cshapes.DarkColor = Color.FromArgb(204, 102, 0);
                    Cshapes.MediumColor = Color.FromArgb(255, 128, 0);
                    Cshapes.LightColor = Color.FromArgb(255, 153, 51);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.green:
                    Cshapes.DarkColor = Color.FromArgb(0, 102, 0);
                    Cshapes.MediumColor = Color.FromArgb(0, 204, 0);
                    Cshapes.LightColor = Color.FromArgb(0, 255, 0);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.gray:
                    Cshapes.DarkColor = Color.FromArgb(64, 64, 64);
                    Cshapes.MediumColor = Color.FromArgb(96, 96, 96);
                    Cshapes.LightColor = Color.FromArgb(160, 160, 160);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.brown:
                    Cshapes.LightColor = Color.FromArgb(51, 25, 0);
                    Cshapes.MediumColor = Color.FromArgb(61, 35, 0);
                    Cshapes.DarkColor = Color.FromArgb(102, 51, 0);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.blue:
                    Cshapes.LightColor = Color.FromArgb(0, 0, 102);
                    Cshapes.MediumColor = Color.FromArgb(0, 0, 153);
                    Cshapes.DarkColor = Color.FromArgb(0, 0, 204);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.black:
                    Cshapes.LightColor = Color.FromArgb(0, 0, 0);
                    Cshapes.MediumColor = Color.FromArgb(10, 10, 10);
                    Cshapes.DarkColor = Color.FromArgb(20, 20, 20);
                    break;
                default:
                    Cshapes.DarkColor = Color.FromArgb(255, 255, 255);
                    Cshapes.MediumColor = Color.FromArgb(250, 235, 215);
                    Cshapes.LightColor = Color.FromArgb(255, 222, 173);
                    break;
            }

            return Cshapes;
        }
    }
}