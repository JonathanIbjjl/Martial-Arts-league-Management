using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using System.Diagnostics;

namespace MartialArts
{
    class Emails
    {

        public static void OpenEmail(string address)
        {
            if (IsOutLookOpen() == false)
            {
                Helpers.DefaultMessegeBox("תוכנת האאוטלוק סגורה, אנא פתח אותה", "IBJJL", System.Windows.Forms.MessageBoxIcon.Stop);
                return;
            }
            try
            {
                Application oApp = new Application();
                _MailItem oMailItem = (MailItem)oApp.CreateItem(OlItemType.olMailItem);
                oMailItem.To = address;
                oMailItem.Subject = "הודעה מ IBJJL";
                oMailItem.Display(true);
            }
            catch
            {

            }
        }

        private static bool IsOutLookOpen()
        {
            int procCount = 0;
            Process[] processlist = Process.GetProcessesByName("OUTLOOK");
            foreach (Process theprocess in processlist)
            {
                procCount++;
            }
            if (procCount > 0)
            {
                //outlook is open
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
