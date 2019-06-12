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
//        Created by Raistlin.K @ D.E.M.O.N at  1:10  18/12/2011 .
//        E-Mail:                         DemonVK@Gmail.com
// 
//        Project Name:                   SevenZip
//        Module  Name:                   SevenZipSfx.cs
//        Part Of:                        DEMONDataAssistant
//        Last Change Date:               1:12 18/12/2011

#endregion

namespace SevenZip
{
#if SFX
    using SfxSettings = Dictionary<string, string>;

    /// <summary>
    /// Sfx module choice enumeration
    /// </summary>
    public enum SfxModule
    {
        /// <summary>
        /// Default module (leave this if unsure)
        /// </summary>
        Default,
        /// <summary>
        /// The simple sfx module by Igor Pavlov with no adjustable parameters
        /// </summary>
        Simple,
        /// <summary>
        /// The installer sfx module by Igor Pavlov
        /// </summary>
        Installer,
        /// <summary>
        /// The extended installer sfx module by Oleg Scherbakov 
        /// </summary>
        Extended,
        /// <summary>
        /// The custom sfx module. First you must specify the module file name.
        /// </summary>
        Custom
    }

    /// <summary>
    /// The class for making 7-zip based self-extracting archives.
    /// </summary>
    public class SevenZipSfx
    {
        private static readonly Dictionary<SfxModule, List<string>> SfxSupportedModuleNames =
            new Dictionary<SfxModule, List<string>>(3)
            {
                {SfxModule.Default, new List<string>(1) {"7zxSD_All.sfx"}},
                {SfxModule.Simple, new List<string>(2) {"7z.sfx", "7zCon.sfx"}},
                {SfxModule.Installer, new List<string>(2) {"7zS.sfx", "7zSD.sfx"}},
                {
                    SfxModule.Extended,
                    new List<string>(4) {"7zxSD_All.sfx", "7zxSD_Deflate", "7zxSD_LZMA", "7zxSD_PPMd"}
                    }
            };

        private SfxModule _module = SfxModule.Default;
        private string _moduleFileName;
        private Dictionary<SfxModule, List<string>> _sfxCommands;

        /// <summary>
        /// Initializes a new instance of the SevenZipSfx class.
        /// </summary>
        public SevenZipSfx()
        {
            _module = SfxModule.Default;
            CommonInit();
        }

        /// <summary>
        /// Initializes a new instance of the SevenZipSfx class.
        /// </summary>
        /// <param name="module">The sfx module to use as a front-end.</param>
        public SevenZipSfx(SfxModule module)
        {
            if (module == SfxModule.Custom)
            {
                throw new ArgumentException("You must specify the custom module executable.", "module");
            }
            _module = module;
            CommonInit();
        }

        /// <summary>
        /// Initializes a new instance of the SevenZipSfx class.
        /// </summary>
        /// <param name="moduleFileName"></param>
        public SevenZipSfx(string moduleFileName)
        {
            _module = SfxModule.Custom;
            ModuleFileName = moduleFileName;
            CommonInit();
        }

        /// <summary>
        /// Gets the sfx module type.
        /// </summary>
        public SfxModule SfxModule
        {
            get
            {
                return _module;
            }
        }

        /// <summary>
        /// Gets or sets the custom sfx module file name
        /// </summary>
        public string ModuleFileName
        {
            get
            {
                return _moduleFileName;
            }

            set
            {
                if (!File.Exists(value))
                {
                    throw new ArgumentException("The specified file does not exist.");
                }
                _moduleFileName = value;
                _module = SfxModule.Custom;
                string sfxName = Path.GetFileName(value);
                foreach (SfxModule mod in SfxSupportedModuleNames.Keys)
                {
                    if (SfxSupportedModuleNames[mod].Contains(sfxName))
                    {
                        _module = mod;
                    }
                }
            }
        }

        private void CommonInit()
        {
            LoadCommandsFromResource("Configs");
        }

        private static string GetResourceString(string str)
        {
#if !WINCE
            return "SevenZip.sfx." + str;
#else
            return "SevenZipSharpMobile.sfx." + str;
#endif
        }

        /// <summary>
        /// Gets the sfx module enum by the list of supported modules
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static SfxModule GetModuleByName(string name)
        {
            if (name.IndexOf("7z.sfx", StringComparison.Ordinal) > -1)
            {
                return SfxModule.Simple;
            }
            if (name.IndexOf("7zS.sfx", StringComparison.Ordinal) > -1)
            {
                return SfxModule.Installer;
            }
            if (name.IndexOf("7zxSD_All.sfx", StringComparison.Ordinal) > -1)
            {
                return SfxModule.Extended;
            }
            throw new SevenZipSfxValidationException("The specified configuration is unsupported.");
        }

        /// <summary>
        /// Loads the commands for each supported sfx module configuration
        /// </summary>
        /// <param name="xmlDefinitions">The resource name for xml definitions</param>
        private void LoadCommandsFromResource(string xmlDefinitions)
        {
            using (Stream cfg = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                GetResourceString(xmlDefinitions + ".xml")))
            {
                if (cfg == null)
                {
                    throw new SevenZipSfxValidationException("The configuration \"" + xmlDefinitions +
                                                             "\" does not exist.");
                }
                using (Stream schm = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    GetResourceString(xmlDefinitions + ".xsd")))
                {
                    if (schm == null)
                    {
                        throw new SevenZipSfxValidationException("The configuration schema \"" + xmlDefinitions +
                                                                 "\" does not exist.");
                    }
                    var sc = new XmlSchemaSet();
                    using (XmlReader scr = XmlReader.Create(schm))
                    {
                        sc.Add(null, scr);
                        var settings = new XmlReaderSettings {ValidationType = ValidationType.Schema, Schemas = sc};
                        string validationErrors = "";
                        settings.ValidationEventHandler +=
                            ((s, t) =>
                            {
                                validationErrors += String.Format(CultureInfo.InvariantCulture, "[{0}]: {1}\n",
                                                                  t.Severity.ToString(), t.Message);
                            });
                        using (XmlReader rdr = XmlReader.Create(cfg, settings))
                        {
                            _sfxCommands = new Dictionary<SfxModule, List<string>>();
                            rdr.Read();
                            rdr.Read();
                            rdr.Read();
                            rdr.Read();
                            rdr.Read();
                            rdr.ReadStartElement("sfxConfigs");
                            rdr.Read();
                            do
                            {
                                SfxModule mod = GetModuleByName(rdr["modules"]);
                                rdr.ReadStartElement("config");
                                rdr.Read();
                                if (rdr.Name == "id")
                                {
                                    var cmds = new List<string>();
                                    _sfxCommands.Add(mod, cmds);
                                    do
                                    {
                                        cmds.Add(rdr["command"]);
                                        rdr.Read();
                                        rdr.Read();
                                    } while (rdr.Name == "id");
                                    rdr.ReadEndElement();
                                    rdr.Read();
                                }
                                else
                                {
                                    _sfxCommands.Add(mod, null);
                                }
                            } while (rdr.Name == "config");
                        }
                        if (!String.IsNullOrEmpty(validationErrors))
                        {
                            throw new SevenZipSfxValidationException(
                                "\n" + validationErrors.Substring(0, validationErrors.Length - 1));
                        }
                        _sfxCommands.Add(SfxModule.Default, _sfxCommands[SfxModule.Extended]);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the sfx scenario commands.
        /// </summary>
        /// <param name="settings">The sfx settings dictionary to validate.</param>
        private void ValidateSettings(SfxSettings settings)
        {
            if (_module == SfxModule.Custom)
            {
                return;
            }
            List<string> commands = _sfxCommands[_module];
            if (commands == null)
            {
                return;
            }
            var invalidCommands = new List<string>();
            foreach (string command in settings.Keys)
            {
                if (!commands.Contains(command))
                {
                    invalidCommands.Add(command);
                }
            }
            if (invalidCommands.Count > 0)
            {
                var invalidText = new StringBuilder("\nInvalid commands:\n");
                foreach (string str in invalidCommands)
                {
                    invalidText.Append(str);
                }
                throw new SevenZipSfxValidationException(invalidText.ToString());
            }
        }

        /// <summary>
        /// Gets the stream containing the sfx settings.
        /// </summary>
        /// <param name="settings">The sfx settings dictionary.</param>
        /// <returns></returns>
        private static Stream GetSettingsStream(SfxSettings settings)
        {
            var ms = new MemoryStream();
            byte[] buf = Encoding.UTF8.GetBytes(@";!@Install@!UTF-8!" + '\n');
            ms.Write(buf, 0, buf.Length);
            foreach (string command in settings.Keys)
            {
                buf =
                    Encoding.UTF8.GetBytes(String.Format(CultureInfo.InvariantCulture, "{0}=\"{1}\"\n", command,
                                                         settings[command]));
                ms.Write(buf, 0, buf.Length);
            }
            buf = Encoding.UTF8.GetBytes(@";!@InstallEnd@!");
            ms.Write(buf, 0, buf.Length);
            return ms;
        }

        private SfxSettings GetDefaultSettings()
        {
            switch (_module)
            {
                default:
                    return null;
                case SfxModule.Installer:
                    var settings = new Dictionary<string, string> {{"Title", "7-Zip self-extracting archive"}};
                    return settings;
                case SfxModule.Default:
                case SfxModule.Extended:
                    settings = new Dictionary<string, string>
                               {
                                   {"GUIMode", "0"},
                                   {"InstallPath", "."},
                                   {"GUIFlags", "128+8"},
                                   {"ExtractPathTitle", "7-Zip self-extracting archive"},
                                   {"ExtractPathText", "Specify the path where to extract the files:"}
                               };
                    return settings;
            }
        }

        /// <summary>
        /// Writes the whole to the other one.
        /// </summary>
        /// <param name="src">The source stream to read from.</param>
        /// <param name="dest">The destination stream to wrie to.</param>
        private static void WriteStream(Stream src, Stream dest)
        {
            src.Seek(0, SeekOrigin.Begin);
            var buf = new byte[32768];
            int bytesRead;
            while ((bytesRead = src.Read(buf, 0, buf.Length)) > 0)
            {
                dest.Write(buf, 0, bytesRead);
            }
        }

        /// <summary>
        /// Makes the self-extracting archive.
        /// </summary>
        /// <param name="archive">The archive stream.</param>
        /// <param name="sfxFileName">The name of the self-extracting executable.</param>
        public void MakeSfx(Stream archive, string sfxFileName)
        {
            using (Stream sfxStream = File.Create(sfxFileName))
            {
                MakeSfx(archive, GetDefaultSettings(), sfxStream);
            }
        }

        /// <summary>
        /// Makes the self-extracting archive.
        /// </summary>
        /// <param name="archive">The archive stream.</param>
        /// <param name="sfxStream">The stream to write the self-extracting executable to.</param>
        public void MakeSfx(Stream archive, Stream sfxStream)
        {
            MakeSfx(archive, GetDefaultSettings(), sfxStream);
        }

        /// <summary>
        /// Makes the self-extracting archive.
        /// </summary>
        /// <param name="archive">The archive stream.</param>
        /// <param name="settings">The sfx settings.</param>
        /// <param name="sfxFileName">The name of the self-extracting executable.</param>
        public void MakeSfx(Stream archive, SfxSettings settings, string sfxFileName)
        {
            using (Stream sfxStream = File.Create(sfxFileName))
            {
                MakeSfx(archive, settings, sfxStream);
            }
        }

        /// <summary>
        /// Makes the self-extracting archive.
        /// </summary>
        /// <param name="archive">The archive stream.</param>
        /// <param name="settings">The sfx settings.</param>
        /// <param name="sfxStream">The stream to write the self-extracting executable to.</param>
        public void MakeSfx(Stream archive, SfxSettings settings, Stream sfxStream)
        {
            if (!sfxStream.CanWrite)
            {
                throw new ArgumentException("The specified output stream can not write.", "sfxStream");
            }
            ValidateSettings(settings);
            using (Stream sfx = _module == SfxModule.Default
                                    ? Assembly.GetExecutingAssembly().GetManifestResourceStream(
                                            GetResourceString(SfxSupportedModuleNames[_module][0]))
                                    : new FileStream(_moduleFileName, FileMode.Open, FileAccess.Read,
                                                     FileShare.ReadWrite))
            {
                WriteStream(sfx, sfxStream);
            }
            if (_module == SfxModule.Custom || _sfxCommands[_module] != null)
            {
                using (Stream set = GetSettingsStream(settings))
                {
                    WriteStream(set, sfxStream);
                }
            }
            WriteStream(archive, sfxStream);
        }

        /// <summary>
        /// Makes the self-extracting archive.
        /// </summary>
        /// <param name="archiveFileName">The archive file name.</param>
        /// <param name="sfxFileName">The name of the self-extracting executable.</param>
        public void MakeSfx(string archiveFileName, string sfxFileName)
        {
            using (Stream sfxStream = File.Create(sfxFileName))
            {
                using (
                    Stream archive = new FileStream(archiveFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    )
                {
                    MakeSfx(archive, GetDefaultSettings(), sfxStream);
                }
            }
        }

        /// <summary>
        /// Makes the self-extracting archive.
        /// </summary>
        /// <param name="archiveFileName">The archive file name.</param>
        /// <param name="sfxStream">The stream to write the self-extracting executable to.</param>
        public void MakeSfx(string archiveFileName, Stream sfxStream)
        {
            using (Stream archive = new FileStream(archiveFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                )
            {
                MakeSfx(archive, GetDefaultSettings(), sfxStream);
            }
        }

        /// <summary>
        /// Makes the self-extracting archive.
        /// </summary>
        /// <param name="archiveFileName">The archive file name.</param>
        /// <param name="settings">The sfx settings.</param>
        /// <param name="sfxFileName">The name of the self-extracting executable.</param>
        public void MakeSfx(string archiveFileName, SfxSettings settings, string sfxFileName)
        {
            using (Stream sfxStream = File.Create(sfxFileName))
            {
                using (
                    Stream archive = new FileStream(archiveFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    )
                {
                    MakeSfx(archive, settings, sfxStream);
                }
            }
        }

        /// <summary>
        /// Makes the self-extracting archive.
        /// </summary>
        /// <param name="archiveFileName">The archive file name.</param>
        /// <param name="settings">The sfx settings.</param>
        /// <param name="sfxStream">The stream to write the self-extracting executable to.</param>
        public void MakeSfx(string archiveFileName, SfxSettings settings, Stream sfxStream)
        {
            using (Stream archive = new FileStream(archiveFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                )
            {
                MakeSfx(archive, settings, sfxStream);
            }
        }
    }
#endif
}