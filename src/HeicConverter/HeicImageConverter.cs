using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace HeicConverter
{
    /// <summary>
    /// HEIC / HEIF 转 PNG / JPG 的核心逻辑（与界面无关）
    /// </summary>
    public static class HeicImageConverter
    {
        /// <summary>支持的输入扩展名</summary>
        public static readonly string[] SupportedExtensions = { ".heic", ".heif" };

        /// <summary>
        /// 判断文件是否为支持的输入格式
        /// </summary>
        public static bool IsSupportedFile(string path)
        {
            var ext = Path.GetExtension(path);
            return SupportedExtensions.Any(e => string.Equals(e, ext, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 在背景线程转换单个文件，避免卡住 UI 线程
        /// </summary>
        /// <returns>实际输出的文件路径</returns>
        public static Task<string> ConvertAsync(string inputPath, ConvertOptions options, CancellationToken cancellationToken)
        {
            return Task.Run(() => Convert(inputPath, options, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// 同步转换单个文件
        /// </summary>
        /// <returns>实际输出的文件路径</returns>
        public static string Convert(string inputPath, ConvertOptions options, CancellationToken cancellationToken)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(options.OutputDirectory)) throw new ArgumentException("未指定输出资料夹。", nameof(options));

            cancellationToken.ThrowIfCancellationRequested();

            using (var image = new MagickImage(inputPath))
            {
                cancellationToken.ThrowIfCancellationRequested();

                // 依方向信息把图片转正，之后移除 EXIF 也不会转错方向
                image.AutoOrient();

                // 只保留 ICC 色彩描述档（iPhone 多为 Display P3，去掉会偏色），
                // 其余 EXIF / XMP / IPTC（含 GPS 位置）全部移除
                var colorProfile = image.GetColorProfile();
                image.Strip();
                if (colorProfile != null)
                {
                    image.SetProfile(colorProfile);
                }

                MagickFormat format;
                string extension;

                if (options.Format == OutputFormat.Png)
                {
                    // PNG 无损：保留原始位深（10 位 HEIC 会输出 16 位 PNG），不做任何压缩损失
                    format = MagickFormat.Png;
                    extension = ".png";
                }
                else
                {
                    format = MagickFormat.Jpeg;
                    extension = ".jpg";

                    // JPG 不支持透明，透明部分以白色填满
                    if (image.HasAlpha)
                    {
                        image.BackgroundColor = MagickColors.White;
                        image.Alpha(AlphaOption.Remove);
                    }

                    // JPG 只支持 8 位
                    image.Depth = 8;
                    image.Quality = (uint)Math.Max(1, Math.Min(100, options.JpegQuality));

                    // 4:4:4 不做色彩次取样，尽量减少色彩损失
                    image.Settings.SetDefine(MagickFormat.Jpeg, "sampling-factor", "4:4:4");
                }

                cancellationToken.ThrowIfCancellationRequested();

                Directory.CreateDirectory(options.OutputDirectory);
                var outputPath = GetUniqueOutputPath(options.OutputDirectory, Path.GetFileNameWithoutExtension(inputPath), extension);

                // 先写到临时文件再改名，避免写到一半失败时留下损坏的输出文件
                var tempPath = outputPath + ".tmp";
                try
                {
                    image.Write(tempPath, format);
                    File.Move(tempPath, outputPath);
                }
                catch
                {
                    TryDelete(tempPath);
                    throw;
                }

                return outputPath;
            }
        }

        /// <summary>
        /// 取得不会覆盖现有文件的输出路径，例如 IMG_0001.png 已存在时改用 IMG_0001 (1).png
        /// </summary>
        public static string GetUniqueOutputPath(string directory, string baseName, string extension)
        {
            var path = Path.Combine(directory, baseName + extension);
            var index = 1;
            while (File.Exists(path))
            {
                path = Path.Combine(directory, string.Format("{0} ({1}){2}", baseName, index, extension));
                index++;
            }

            return path;
        }

        /// <summary>
        /// 从文件与资料夹（含子资料夹）中找出所有支持的文件，无权限的资料夹会略过
        /// </summary>
        public static List<string> CollectFiles(IEnumerable<string> paths)
        {
            var result = new List<string>();
            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    if (IsSupportedFile(path)) result.Add(path);
                }
                else if (Directory.Exists(path))
                {
                    CollectFromDirectory(path, result);
                }
            }

            return result;
        }

        private static void CollectFromDirectory(string root, List<string> result)
        {
            var pending = new Stack<string>();
            pending.Push(root);

            while (pending.Count > 0)
            {
                var dir = pending.Pop();
                try
                {
                    result.AddRange(Directory.EnumerateFiles(dir).Where(IsSupportedFile).OrderBy(f => f, StringComparer.OrdinalIgnoreCase));
                    foreach (var sub in Directory.EnumerateDirectories(dir))
                    {
                        pending.Push(sub);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // 无权限的资料夹直接略过
                }
                catch (IOException)
                {
                    // 资料夹在列举途中被删除等情况，略过
                }
            }
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }
}
