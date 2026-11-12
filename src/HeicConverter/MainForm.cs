using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeicConverter
{
    /// <summary>
    /// 主窗口：管理文件清单、转换选项与批次转换流程
    /// </summary>
    public partial class MainForm : Form
    {
        private const string StatusPending = "等待中";
        private const string StatusConverting = "轉換中…";
        private const string StatusCancelled = "已取消";

        /// <summary>已加入清单的文件（不分大小写），用来避免重复加入</summary>
        private readonly HashSet<string> _filePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly AppSettings _settings;
        private CancellationTokenSource _cts;
        private bool _isBusy;

        /// <summary>转换中按下关闭时设为 true，等转换停止后再关闭窗口</summary>
        private bool _closeRequested;

        public MainForm()
        {
            InitializeComponent();
            _settings = AppSettings.Load();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 套用上次的设定
            if (!string.IsNullOrEmpty(_settings.OutputDirectory) && Directory.Exists(_settings.OutputDirectory))
            {
                txtOutput.Text = _settings.OutputDirectory;
            }

            rbJpg.Checked = _settings.Format == OutputFormat.Jpg;
            rbPng.Checked = _settings.Format == OutputFormat.Png;
            trkQuality.Value = _settings.JpegQuality;

            UpdateFormatUi();
            UpdateButtons();
        }

        #region 加入 / 移除文件

        private async void BtnAddFiles_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "選擇 HEIC 檔案";
                dialog.Filter = "HEIC / HEIF 圖片 (*.heic;*.heif)|*.heic;*.heif|所有檔案 (*.*)|*.*";
                dialog.Multiselect = true;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    await AddPathsAsync(dialog.FileNames);
                }
            }
        }

        private async void BtnAddFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "選擇資料夾（包含子資料夾內的 HEIC 檔案）";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    await AddPathsAsync(new[] { dialog.SelectedPath });
                }
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedItems();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            lvFiles.Items.Clear();
            _filePaths.Clear();
            progressBar.Value = 0;
            lblStatus.Text = "請加入要轉換的 HEIC 檔案";
            UpdateButtons();
        }

        private void LvFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && !_isBusy)
            {
                RemoveSelectedItems();
            }
        }

        private void LvFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = !_isBusy && e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private async void LvFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (_isBusy) return;

            var paths = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (paths != null && paths.Length > 0)
            {
                await AddPathsAsync(paths);
            }
        }

        /// <summary>
        /// 加入文件或资料夹；扫描资料夹在背景执行，避免大资料夹卡住界面
        /// </summary>
        private async Task AddPathsAsync(IEnumerable<string> paths)
        {
            SetBusy(true);
            lblStatus.Text = "正在搜尋 HEIC 檔案…";

            try
            {
                var pathList = paths.ToList();
                var files = await Task.Run(() => HeicImageConverter.CollectFiles(pathList));
                if (IsDisposed) return;

                var added = 0;
                lvFiles.BeginUpdate();
                try
                {
                    foreach (var file in files)
                    {
                        if (!_filePaths.Add(file)) continue;

                        var item = new ListViewItem(Path.GetFileName(file));
                        item.SubItems.Add(Path.GetDirectoryName(file));
                        item.SubItems.Add(StatusPending);
                        item.Tag = file;
                        lvFiles.Items.Add(item);
                        added++;
                    }
                }
                finally
                {
                    lvFiles.EndUpdate();
                }

                lblStatus.Text = added > 0
                    ? string.Format("已加入 {0} 個檔案，共 {1} 個", added, lvFiles.Items.Count)
                    : "沒有找到新的 HEIC / HEIF 檔案";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "加入檔案失敗：" + ex.Message;
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void RemoveSelectedItems()
        {
            lvFiles.BeginUpdate();
            try
            {
                foreach (ListViewItem item in lvFiles.SelectedItems.Cast<ListViewItem>().ToList())
                {
                    _filePaths.Remove((string)item.Tag);
                    lvFiles.Items.Remove(item);
                }
            }
            finally
            {
                lvFiles.EndUpdate();
            }

            UpdateButtons();
        }

        #endregion

        #region 选项

        private void FormatOption_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFormatUi();
        }

        private void TrkQuality_ValueChanged(object sender, EventArgs e)
        {
            lblQualityValue.Text = trkQuality.Value.ToString();
            UpdateFormatUi();
        }

        private void UpdateFormatUi()
        {
            var isJpg = rbJpg.Checked;
            lblQuality.Enabled = isJpg && !_isBusy;
            trkQuality.Enabled = isJpg && !_isBusy;
            lblQualityValue.Enabled = isJpg && !_isBusy;

            if (!isJpg)
            {
                lblFormatNote.Text = "PNG 為無損格式，畫質與原圖完全相同，但檔案會比 HEIC 大很多。";
            }
            else if (trkQuality.Value >= 95)
            {
                lblFormatNote.Text = "JPG 為有損格式；品質 95 以上肉眼幾乎看不出差異（使用 4:4:4 色彩取樣）。";
            }
            else
            {
                lblFormatNote.Text = "JPG 為有損格式；品質越低檔案越小，但畫質損失越明顯。";
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "選擇輸出資料夾";
                dialog.ShowNewFolderButton = true;
                if (Directory.Exists(txtOutput.Text))
                {
                    dialog.SelectedPath = txtOutput.Text;
                }

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtOutput.Text = dialog.SelectedPath;
                    UpdateButtons();
                }
            }
        }

        private void BtnOpenOutput_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtOutput.Text)) return;

            try
            {
                Process.Start("explorer.exe", "\"" + txtOutput.Text + "\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "無法開啟資料夾：" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region 转换

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (lvFiles.Items.Count == 0)
            {
                MessageBox.Show(this, "請先加入要轉換的 HEIC 檔案。", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtOutput.Text) || !Directory.Exists(txtOutput.Text))
            {
                MessageBox.Show(this, "請先選擇輸出資料夾。", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                BtnBrowse_Click(sender, e);
                if (!Directory.Exists(txtOutput.Text)) return;
            }

            var options = new ConvertOptions
            {
                Format = rbJpg.Checked ? OutputFormat.Jpg : OutputFormat.Png,
                JpegQuality = trkQuality.Value,
                OutputDirectory = txtOutput.Text
            };

            SaveSettings(options);
            await ConvertAllAsync(options);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                btnCancel.Enabled = false;
                lblStatus.Text = "正在取消，會在目前這張完成後停止…";
            }
        }

        /// <summary>
        /// 逐一转换清单中的文件。转换在背景线程执行，await 之后回到 UI 线程更新控件，不需要 Invoke
        /// </summary>
        private async Task ConvertAllAsync(ConvertOptions options)
        {
            var items = lvFiles.Items.Cast<ListViewItem>().ToList();
            var succeeded = 0;
            var failed = 0;
            var cancelled = false;

            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            SetBusy(true);
            progressBar.Minimum = 0;
            progressBar.Maximum = items.Count;
            progressBar.Value = 0;

            foreach (var item in items)
            {
                item.SubItems[2].Text = StatusPending;
            }

            try
            {
                for (var i = 0; i < items.Count; i++)
                {
                    var item = items[i];

                    if (token.IsCancellationRequested)
                    {
                        cancelled = true;
                        MarkCancelled(items, i);
                        break;
                    }

                    var inputPath = (string)item.Tag;
                    item.SubItems[2].Text = StatusConverting;
                    item.EnsureVisible();
                    lblStatus.Text = string.Format("正在轉換 {0}/{1}：{2}", i + 1, items.Count, item.Text);

                    try
                    {
                        var outputPath = await HeicImageConverter.ConvertAsync(inputPath, options, token);
                        item.SubItems[2].Text = "完成 → " + Path.GetFileName(outputPath);
                        item.ForeColor = System.Drawing.SystemColors.WindowText;
                        succeeded++;
                    }
                    catch (OperationCanceledException)
                    {
                        cancelled = true;
                        MarkCancelled(items, i);
                        break;
                    }
                    catch (Exception ex)
                    {
                        // 单张失败不影响其他文件
                        item.SubItems[2].Text = "失敗：" + ex.Message;
                        item.ForeColor = System.Drawing.Color.Firebrick;
                        failed++;
                    }

                    progressBar.Value = i + 1;
                }
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
                SetBusy(false);
            }

            if (_closeRequested)
            {
                Close();
                return;
            }

            var summary = string.Format("完成 {0} 個，失敗 {1} 個", succeeded, failed);
            if (cancelled) summary = "已取消。" + summary;
            lblStatus.Text = summary;

            if (failed > 0)
            {
                MessageBox.Show(this, summary + "。失敗原因請看清單中的「狀態」欄。", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void MarkCancelled(IList<ListViewItem> items, int startIndex)
        {
            for (var j = startIndex; j < items.Count; j++)
            {
                items[j].SubItems[2].Text = StatusCancelled;
            }
        }

        #endregion

        #region 状态与设定

        /// <summary>
        /// 切换忙碌状态，忙碌时停用会影响清单或选项的控件
        /// </summary>
        private void SetBusy(bool busy)
        {
            if (IsDisposed) return;

            _isBusy = busy;
            UpdateButtons();
            UpdateFormatUi();
        }

        private void UpdateButtons()
        {
            var hasItems = lvFiles.Items.Count > 0;
            btnAddFiles.Enabled = !_isBusy;
            btnAddFolder.Enabled = !_isBusy;
            btnRemove.Enabled = !_isBusy && hasItems;
            btnClear.Enabled = !_isBusy && hasItems;
            rbPng.Enabled = !_isBusy;
            rbJpg.Enabled = !_isBusy;
            btnBrowse.Enabled = !_isBusy;
            btnStart.Enabled = !_isBusy && hasItems;
            btnCancel.Enabled = _isBusy && _cts != null && !_cts.IsCancellationRequested;
            btnOpenOutput.Enabled = Directory.Exists(txtOutput.Text);
        }

        private void SaveSettings(ConvertOptions options)
        {
            _settings.OutputDirectory = options.OutputDirectory;
            _settings.Format = options.Format;
            _settings.JpegQuality = options.JpegQuality;
            _settings.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isBusy || _cts == null) return;

            var result = MessageBox.Show(this, "轉換尚未完成，確定要結束嗎？", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // 先取消并等目前这张完成，转换停止后会自动关闭窗口，避免背景工作存取已释放的控件
            e.Cancel = true;
            if (result == DialogResult.Yes)
            {
                _closeRequested = true;
                _cts.Cancel();
                btnCancel.Enabled = false;
                lblStatus.Text = "正在停止，完成後會自動關閉…";
            }
        }

        #endregion
    }
}
