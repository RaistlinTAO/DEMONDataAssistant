// ######################################################################################################################
// #  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the  #
// #  following conditions are met:                                                                                     #
// #    1、Redistributions of source code must retain the above copyright notice, this list of conditions and the       #
// #       following disclaimer.                                                                                        #
// #    2、Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the    #
// #       following disclaimer in the documentation and/or other materials provided with the distribution.             #
// #    3、Neither the name of the D.E.M.O.N, K9998(Wei Tao) nor the names of its contributors may be used to endorse   #
// #       or promote products derived from this software without specific prior written permission.                    #
// #                                                                                                                    #
// #       Project Name:                                                                                                #
// #       Module  Name:                                                                                                #
// #       Part of:                                                                                                     #
// #       Date:                                                                                                        #
// #       Version:                                                                                                     #
// #                                                                                                                    #
// #                                           Copyright © 2011 ORG: D.E.M.O.N K9998(Wei Tao) All Rights Reserved      #
// ######################################################################################################################
namespace DEMONDataAssistant.Service
{
    #region

    using System;
    using System.Runtime.InteropServices;

    #endregion

    public static class ClsService
    {
        private const int SC_MANAGER_CREATE_SERVICE = 0x0002;
        private const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        //int SERVICE_DEMAND_START = 0x00000003; 
        private const int SERVICE_ERROR_NORMAL = 0x00000001;
        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int SERVICE_QUERY_CONFIG = 0x0001;
        private const int SERVICE_CHANGE_CONFIG = 0x0002;
        private const int SERVICE_QUERY_STATUS = 0x0004;
        private const int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
        private const int SERVICE_START = 0x0010;
        private const int SERVICE_STOP = 0x0020;
        private const int SERVICE_PAUSE_CONTINUE = 0x0040;
        private const int SERVICE_INTERROGATE = 0x0080;
        private const int SERVICE_USER_DEFINED_CONTROL = 0x0100;

        private const int SERVICE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED |
                                                SERVICE_QUERY_CONFIG |
                                                SERVICE_CHANGE_CONFIG |
                                                SERVICE_QUERY_STATUS |
                                                SERVICE_ENUMERATE_DEPENDENTS |
                                                SERVICE_START |
                                                SERVICE_STOP |
                                                SERVICE_PAUSE_CONTINUE |
                                                SERVICE_INTERROGATE |
                                                SERVICE_USER_DEFINED_CONTROL);

        private const int SERVICE_AUTO_START = 0x00000002;
        private const int SERVICE_DEMAND_START = 0x00000003;
        private const int GENERIC_WRITE = 0x40000000;
        private const int DELETE = 0x10000;

        [DllImport("advapi32.dll")]
        internal static extern IntPtr OpenSCManager(string lpMachineName, string lpSCDB, int scParameter);

        [DllImport("Advapi32.dll")]
        internal static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
                                                    int dwDesiredAccess, int dwServiceType, int dwStartType,
                                                    int dwErrorControl, string lpPathName,
                                                    string lpLoadOrderGroup, int lpdwTagId, string lpDependencies,
                                                    string lpServiceStartName, string lpPassword);

        [DllImport("advapi32.dll")]
        internal static extern void CloseServiceHandle(IntPtr SCHANDLE);

        [DllImport("advapi32.dll")]
        internal static extern int StartService(IntPtr SVHANDLE, int dwNumServiceArgs, string lpServiceArgVectors);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern IntPtr OpenService(IntPtr SCHANDLE, string lpSvcName, int dwNumServiceArgs);

        [DllImport("advapi32.dll")]
        internal static extern int DeleteService(IntPtr SVHANDLE);

        [DllImport("kernel32.dll")]
        internal static extern int GetLastError();

        public static bool InstallService(string svcPath, string svcName, string svcDispName)
        {
            IntPtr sc_handle = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
            if (sc_handle.ToInt32() != 0)
            {
                IntPtr sv_handle = CreateService(sc_handle, svcName, svcDispName, SERVICE_ALL_ACCESS,
                                                 SERVICE_WIN32_OWN_PROCESS, SERVICE_DEMAND_START, SERVICE_ERROR_NORMAL,
                                                 svcPath, null, 0, null, null, null);
                if (sv_handle.ToInt32() == 0)
                {
                    CloseServiceHandle(sc_handle);
                    return false;
                }
                //试尝启动服务
                /*
                int i = StartService(sv_handle, 0, null);
                if (i == 0)
                {
                    return false;
                }
                 */
                CloseServiceHandle(sc_handle);
                return true;
            }
            return false;
        }

        public static bool UnInstallService(string svcName)
        {
            IntPtr sc_hndl = OpenSCManager(null, null, GENERIC_WRITE);
            if (sc_hndl.ToInt32() == 0)
            {
                return false;
            }

            IntPtr svc_hndl = OpenService(sc_hndl, svcName, DELETE);
            if (svc_hndl.ToInt32() != 0)
            {
                int i = DeleteService(svc_hndl);
                if (i != 0)
                {
                    CloseServiceHandle(sc_hndl);
                    return true;
                }
                CloseServiceHandle(sc_hndl);
                return false;
            }
            return false;
        }
    }
}