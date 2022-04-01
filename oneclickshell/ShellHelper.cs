using Microsoft.Win32;
using System.Collections;
using System.IO;
using System.Reflection;

namespace OneClickShell
{
    [System.ComponentModel.RunInstaller(true)]
    public class ShellHelper : System.Configuration.Install.Installer
    {
        // 以下两行用于获取本dll文件所在目录
        static readonly string DLLPATH = Assembly.GetExecutingAssembly().Location;
        static readonly string CURDIR = Path.GetDirectoryName(DLLPATH);
        readonly string EXE_FULLPATH = Path.Combine(CURDIR, "oneclick.exe");
        const string REG_ITEM_CP_NAME = "OneClickN";
        const string REG_ITEM_CP_PATH = "OneClickP";
        const string RK_TEXT_CP_NAME = "复制目标名称";
        const string RK_TEXT_CP_PATH = "复制完整路径";
        const string REG_SUBKEY_COMMAND = "command";
        const string REG_CMD_CP_NAME = " -basename \"%V\"";
        const string REG_CMD_CP_PATH = " -fullpath \"%V\"";
        readonly RegistryKey REG_MAIN_KEY = Registry.ClassesRoot;
        RegistryKey REGKeyTemp;
        const string REG_DEFAULT_KEYNAME = "";
        const string REG_DIRS_FG_PAR = @"Directory\shell";
        const string REG_DIRS_BG_PAR = @"Directory\Background\shell";
        const string REG_FILES_PAR = @"*\shell";
        readonly string REG_FILES_CP_NAME = Path.Combine(REG_FILES_PAR, REG_ITEM_CP_NAME);
        readonly string REG_FILES_CP_PATH = Path.Combine(REG_FILES_PAR, REG_ITEM_CP_PATH);
        readonly string REG_DIR_FG_CP_NAME = Path.Combine(REG_DIRS_FG_PAR, REG_ITEM_CP_NAME);
        readonly string REG_DIR_BG_CP_NAME = Path.Combine(REG_DIRS_BG_PAR, REG_ITEM_CP_NAME);
        readonly string REG_DIR_FG_CP_PATH = Path.Combine(REG_DIRS_FG_PAR, REG_ITEM_CP_PATH);
        readonly string REG_DIR_BG_CP_PATH = Path.Combine(REG_DIRS_BG_PAR, REG_ITEM_CP_PATH);

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            RegistryKey[] OneClickREGKeys = new RegistryKey[]
            {
                REG_MAIN_KEY.OpenSubKey(REG_FILES_PAR, true),
                REG_MAIN_KEY.OpenSubKey(REG_DIRS_FG_PAR, true),
                REG_MAIN_KEY.OpenSubKey(REG_DIRS_BG_PAR, true)
            };
            foreach (RegistryKey key in OneClickREGKeys)
            {
                if (key != null)
                {
                    key.DeleteSubKeyTree(REG_ITEM_CP_NAME, false);
                    key.DeleteSubKeyTree(REG_ITEM_CP_PATH, false);
                    key.Close();
                }
            }
            return; // 强迫症
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            RegistryKey[] OneClickREGKeys = new RegistryKey[]
            {
                REG_MAIN_KEY.CreateSubKey(REG_FILES_CP_NAME, true),
                REG_MAIN_KEY.CreateSubKey(REG_FILES_CP_PATH, true),
                REG_MAIN_KEY.CreateSubKey(REG_DIR_FG_CP_NAME, true),
                REG_MAIN_KEY.CreateSubKey(REG_DIR_BG_CP_NAME, true),
                REG_MAIN_KEY.CreateSubKey(REG_DIR_FG_CP_PATH, true),
                REG_MAIN_KEY.CreateSubKey(REG_DIR_BG_CP_PATH, true)
            };
            foreach (RegistryKey key in OneClickREGKeys)
            {
                if (key is null) continue;
                if (key.Name.EndsWith(REG_ITEM_CP_NAME))
                {
                    key.SetValue(REG_DEFAULT_KEYNAME, RK_TEXT_CP_NAME);
                    REGKeyTemp = key.CreateSubKey(REG_SUBKEY_COMMAND, true);
                    if (REGKeyTemp is null) { key.Close(); continue; }
                    REGKeyTemp.SetValue(REG_DEFAULT_KEYNAME, EXE_FULLPATH + REG_CMD_CP_NAME);
                    key.Close(); REGKeyTemp.Close();
                }
                else if (key.Name.EndsWith(REG_ITEM_CP_PATH))
                {
                    key.SetValue(REG_DEFAULT_KEYNAME, RK_TEXT_CP_PATH);
                    REGKeyTemp = key.CreateSubKey(REG_SUBKEY_COMMAND, true);
                    if (REGKeyTemp is null) { key.Close(); continue; }
                    REGKeyTemp.SetValue(REG_DEFAULT_KEYNAME, EXE_FULLPATH + REG_CMD_CP_PATH);
                    key.Close(); REGKeyTemp.Close();
                }
            }
            return; // 强迫症
        }
    }
}
