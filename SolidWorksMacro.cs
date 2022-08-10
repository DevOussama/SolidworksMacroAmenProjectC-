


using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.sldworks;
using System.Runtime.InteropServices;
using Microsoft.Win32;


namespace SolidWorksMacro
{
    public class SolidWorksMacro : SwAddin
    {
        SldWorks swApp;
        int SessionCookie;
        #region Solidworks connectin
        public bool ConnectToSW(object ThisSW, int Cookie)
        {
            swApp = ThisSW as SldWorks;
            swApp.SetAddinCallbackInfo2(0, this, Cookie);
            SessionCookie = Cookie;
            swApp.FileNewNotify2 += SwApp_FileNewNotify2;

            // this where we build UI
            return true;

        }

        private int SwApp_FileNewNotify2(object NewDoc, int DocType, string TemplateName)
        {
            swApp.SendMsgToUser("New File Created by Oussama");
            return 0;
        }

        public bool DisconnectFromSW()
        {
            // this is where we destroy the UI
            GC.Collect();
            swApp = null;

            return true;
        }
        #endregion

        #region com register-unregister function

        [ComRegisterFunction]
        private static void RegisterAssembly(Type t)
        {
            string Path = String.Format(@"SOFTWARE\Solidworks\AddIns\{0:b}", t);
            RegistryKey Key = Registry.LocalMachine.CreateSubKey(Path);
            // startup int
            Key.SetValue(null, 1);
            Key.SetValue("Title", "AddinSample");
            Key.SetValue("Description", "This is a description");


        }

        [ComUnregisterFunction]
        private static void UnregisteryAssembly(Type t)
        {
            string Path = String.Format(@"SOFTWARE\Solidworks\AddIns\{0:b}", t);
            Registry.LocalMachine.DeleteSubKey(Path);

        }
        #endregion

    }
}