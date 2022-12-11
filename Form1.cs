using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blanch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
            //label1.BackColor = Color.Black;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void panelEx4_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    class PanelEx : Panel
    {
        string path = "";
        PictureBox box = new PictureBox();

        public PanelEx()
        {
            AllowDrop = true;
            box.Parent = this;
            box.Dock = DockStyle.Fill;
            box.Click += Box_Click;
            box.MouseMove += Box_MouseMove;
        }

        public string GetPath()
        {
            return path;
        }

        public void SetPath(string path0)
        {
            path = path0;

            box.SizeMode = PictureBoxSizeMode.StretchImage;
            if (File.Exists(path))
            {
                Icon appIcon = Icon.ExtractAssociatedIcon(path0);
                box.Image = appIcon.ToBitmap();
            }
            else if (Directory.Exists(path0))
            {
                box.Image = GetFolderImage();
            }
        }

        private void Box_MouseMove(object sender, MouseEventArgs e)
        {
            Form1 f = (Form1)FindForm();
            //f.label1.ForeColor = Color.White;
            //f.label1.Text = path;
        }

        private void Box_Click(object sender, EventArgs e)
        {
            try
            {
                Icon appIcon = Icon.ExtractAssociatedIcon(path);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                MessageBox.Show("ファイルが存在しません");
            }
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                path = files[0];

                box.SizeMode = PictureBoxSizeMode.StretchImage;
                if (File.Exists(path))
                {
                    Icon appIcon = Icon.ExtractAssociatedIcon(path);
                    box.Image = appIcon.ToBitmap();
                }
                else if (Directory.Exists(path))
                {
                    box.Image = GetFolderImage();
                }
            }
        }

        Image GetFolderImage()
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hSuccess = SHGetFileInfo("", 0, ref shinfo, (uint)System.Runtime.InteropServices.Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
            if (hSuccess != IntPtr.Zero)
            {
                Icon appIcon = Icon.FromHandle(shinfo.hIcon);
                return appIcon.ToBitmap();
            }
            return null;
        }

        // SHGetFileInfo関数
        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        // SHGetFileInfo関数で使用する構造体
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        // SHGetFileInfo関数で使用するフラグ
        private const uint SHGFI_ICON = 0x100; // アイコン・リソースの取得
        private const uint SHGFI_LARGEICON = 0x0; // 大きいアイコン
        private const uint SHGFI_SMALLICON = 0x1; // 小さいアイコン
    }
}
