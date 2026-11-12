namespace HeicConverter
{
    /// <summary>
    /// 输出格式
    /// </summary>
    public enum OutputFormat
    {
        /// <summary>PNG：无损</summary>
        Png,

        /// <summary>JPG：有损，可调整品质</summary>
        Jpg
    }

    /// <summary>
    /// 转换参数
    /// </summary>
    public sealed class ConvertOptions
    {
        /// <summary>输出格式</summary>
        public OutputFormat Format { get; set; } = OutputFormat.Png;

        /// <summary>JPG 品质（1～100），只在输出 JPG 时使用</summary>
        public int JpegQuality { get; set; } = 100;

        /// <summary>输出资料夹</summary>
        public string OutputDirectory { get; set; }
    }
}
