using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Configuration;
using System.Web;


    public class Impersonalizacion
    {
        private const int LOGON32_PROVIDER_DEFAULT = 0;
        private const int LOGON32_LOGON_INTERACTIVE = 2;
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        [DllImport("advapi32.dll", EntryPoint = "DuplicateToken", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr ExistingTokenHandle, int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);
        public static WindowsImpersonationContext WinLogOn(string strUsuario, string strClave, string strDominio)
        {
            IntPtr tokenDuplicate = new IntPtr(0);
            IntPtr tokenHandle = new IntPtr(0);
            if (LogonUser(strUsuario, strDominio, strClave, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref tokenHandle))
                if (DuplicateToken(tokenHandle, 2, ref tokenDuplicate) != 0)
                    return (new WindowsIdentity(tokenDuplicate)).Impersonate();
            return null;
        }

        public static WindowsImpersonationContext WinLogOn()
        {
            string strUsuario = ConfigurationManager.AppSettings["Identificacion"].Split(Convert.ToChar(","))[0];
            string strClave = ConfigurationManager.AppSettings["Identificacion"].Split(Convert.ToChar(","))[1];
            string strDominio = ConfigurationManager.AppSettings["Identificacion"].Split(Convert.ToChar(","))[2];

            IntPtr tokenDuplicate = new IntPtr(0);
            IntPtr tokenHandle = new IntPtr(0);
            if (LogonUser(strUsuario, strDominio, strClave, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref tokenHandle))
                if (DuplicateToken(tokenHandle, 2, ref tokenDuplicate) != 0)
                    return (new WindowsIdentity(tokenDuplicate)).Impersonate();
            return null;
        }
    }
