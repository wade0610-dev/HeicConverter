using System;
using System.IO;

namespace HeicConverter
{
    /// <summary>
    /// 简单的使用者设定（记住上次的输出资料夹、格式与品质），存放在 %AppData%\HeicConverter\settings.ini
    /// 读写失败时一律忽略，不影响主要功能
    /// </summary>
    internal sealed class AppSettings
    {
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HeicConverter", "settings.ini");

        public string OutputDirectory { get; set; } = string.Empty;

        public OutputFormat Format { get; set; } = OutputFormat.Png;

        public int JpegQuality { get; set; } = 100;

        public static AppSettings Load()
        {
            var settings = new AppSettings();
            try
            {
                if (!File.Exists(SettingsPath)) return settings;

                foreach (var line in File.ReadAllLines(SettingsPath))
                {
                    var index = line.IndexOf('=');
                    if (index <= 0) continue;

                    var key = line.Substring(0, index).Trim();
                    var value = line.Substring(index + 1).Trim();

                    switch (key)
                    {
                        case "OutputDirectory":
                            settings.OutputDirectory = value;
                            break;
                        case "Format":
                            OutputFormat format;
                            if (Enum.TryParse(value, out format)) settings.Format = format;
                            break;
                        case "JpegQuality":
                            int quality;
                            if (int.TryParse(value, out quality) && quality >= 1 && quality <= 100) settings.JpegQuality = quality;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                // 设定文件损坏或无法读取时使用预设值
            }

            return settings;
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
                File.WriteAllLines(SettingsPath, new[]
                {
                    "OutputDirectory=" + OutputDirectory,
                    "Format=" + Format,
                    "JpegQuality=" + JpegQuality
                });
            }
            catch (Exception)
            {
                // 无法写入设定时忽略
            }
        }
    }
}
