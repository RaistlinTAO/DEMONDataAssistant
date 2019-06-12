#region SOURCE INFORMATION

// COPYRIGHT LICENCE
// 
//  Copyright (c) 2011, D.E.M.O.N Organization
//  All rights reserved.
// 
//  Redistribution and use in source and binary forms, with or without modification,
//  are permitted provided that the following conditions are met:
// 
//      * Redistributions of source code must retain the above copyright notice,
//      this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright notice,
//      this list of conditions and the following disclaimer in the documentation
//      and/or other materials provided with the distribution.
//      * Neither D.E.M.O.N Organization nor its contributors
//      may be used to endorse or promote products derived from this
//      software without specific prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
//  FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
//  CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
//  OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//  THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
// 
// 
// CODE DESCRIPTION
// 
//        Created by Raistlin.K @ D.E.M.O.N at  19:22  07/11/2011 .
//        E-Mail:                         DemonVK@Gmail.com
// 
//        Project Name:                   DEMONDataAssistant
//        Module  Name:                   clsMySQL.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               4:07 18/12/2011

#endregion

namespace DEMONDataAssistant
{
    #region

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Management;
    using System.Reflection;
    using System.Security.Permissions;
    using DEMONDataAssistant.INI;
    using SevenZip;

//using NetFwTypeLib;

    #endregion

    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class clsMySQL : IDisposable
    {
        internal string _iPath;

        public clsMySQL(string iPath)
        {
            _iPath = iPath;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public bool InstallMySql()
        {
            return MakeInstall();
        }


        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public bool RunMySql()
        {
            try
            {
                var p = new Process
                            {
                                StartInfo =
                                    {
                                        WindowStyle = ProcessWindowStyle.Hidden,
                                        Arguments = "--defaults-file=\"" + _iPath + "bin\\my.ini\"",
                                        CreateNoWindow = true,
                                        FileName = _iPath + "bin\\mysqld.exe"
                                    }
                            };

                //p.StartInfo.UseShellExecute = True;
                //var strArgument = " /c dir c: > c:\11.txt"; //启动参数
                try
                {
                    p.Start();
                }
                finally
                {
                    p.Close();
                }
                //(_iPath + "bin\\mysqld.exe", "--defaults-file=\"" + _iPath + "bin\\my.ini\"");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            //bin\mysqld --defaults-file=bin\my.ini --standalone --console
        }

        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public bool StopMySql()
        {
            try
            {
                var p = new Process
                            {
                                StartInfo =
                                    {
                                        WindowStyle = ProcessWindowStyle.Hidden,
                                        Arguments = "--port 9998 -u root shutdown",
                                        CreateNoWindow = true,
                                        UseShellExecute = false,
                                        FileName = _iPath + "bin\\mysqladmin.exe",
                                        RedirectStandardInput = true,
                                        RedirectStandardOutput = true
                                    }
                            };

                //p.StartInfo.UseShellExecute = True;
                //var strArgument = " /c dir c: > c:\11.txt"; //启动参数
                //p.StartInfo.RedirectStandardError = true;
                p.Start();
                //p.StandardInput.WriteLine("7ujm6yhn5tgb");

                //7ujm6yhn5tgb

                //(_iPath + "bin\\mysqld.exe", "--defaults-file=\"" + _iPath + "bin\\my.ini\"");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        internal bool MakeInstall()
        {
            try
            {
                ExtractFile("DEMONDataAssistant.MySQL.SHARE.7z", _iPath);
                ExtractFile("DEMONDataAssistant.MySQL.DATA.7z", _iPath);
                ExtractFile(
                    Distinguish64or32System() == "32"
                        ? "DEMONDataAssistant.MySQL.X86.7z"
                        : "DEMONDataAssistant.MySQL.X64.7z", _iPath);
                Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("DEMONDataAssistant.MySQL.my.ini");
                var sr = new StreamReader(sm);
                var sw = new StreamWriter(_iPath + "bin\\my.ini");
                sw.Write(sr.ReadToEnd());
                sw.Flush();
                sw.Close();
                sr.Close();
                var iINIControl = new ClsIni(_iPath + "bin\\my.ini");
                iINIControl.IniWriteValue("mysqld", "basedir", (_iPath).Replace("\\", "/"));
                iINIControl.IniWriteValue("mysqld", "datadir", (_iPath).Replace("\\", "/") + "data/");

                //添加防火墙例外

                //NetFwAddApps("DEMON TAOBAO MASTER", _iPath + "bin\\mysqld.exe");

                //安装服务
                //"\"d:\\my share\\myservice.exe\""
                //D:\xampp\mysql\bin\mysqld.exe --defaults-file=D:\xampp\mysql\bin\my.ini mysql
                //ClsService.InstallService("\"" + _iPath + "bin\\mysqld.exe\" --defaults-file=\"" + _iPath + "bin\\my.ini\"", "DEMONDataBase", "DEMON Studio Database");
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///   解压缩7Z
        /// </summary>
        /// <param name = "iStream"></param>
        /// <param name = "iPath"></param>
        internal void ExtractFile(string iStream, string iPath)
        {
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream(iStream);
           
            var ext = new SevenZipExtractor(sm);

            for (int i = 0; i < ext.FilesCount; ++i)
            {
                ext.ExtractFiles(iPath, i);
            }

            //ext.Dispose();
            sm.Close();
        }

        /// <summary>
        ///   判定操作系统版本
        /// </summary>
        /// <returns></returns>
        internal static string Distinguish64or32System()
        {
            try
            {
                string addressWidth = String.Empty;
                var mConnOption = new ConnectionOptions();
                var mMs = new ManagementScope("\\\\localhost", mConnOption);
                var mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                var mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementBaseObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }
                return addressWidth;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "32";
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _iPath = null;
            }
            // free native resources
        }
    }
}