using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace Disker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 添加引用 System.Management
            ManagementObjectSearcher mos = new ManagementObjectSearcher(
                "select name from win32_logicaldisk" // No SQL, Don't add ';'
            );
            foreach (ManagementObject mymo in mos.Get())
            {
                comboBox1.Items.Add(mymo["Name"].ToString());
            }
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1_Paint(null, null);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // 绘图准备
                DriveInfo dInfo = new DriveInfo(comboBox1.Text);
                long allSize = dInfo.TotalSize;
                long useSize = dInfo.TotalFreeSpace;
                Graphics g = panel1.CreateGraphics();
                // https://blog.csdn.net/ScapeD/article/details/85173982
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);
                Pen titlePen = new Pen(Color.Gray);
                Brush useBrush = new SolidBrush(Color.FromArgb(38, 160, 218));
                Brush surplusBrush = new SolidBrush(Color.Gray);
                Brush titleBrush = new SolidBrush(Color.Gray);
                Brush writeBrush = new SolidBrush(Color.White);
                Font titleFont = new Font("YaHei Consolas Hybrid", 16);
                Font textFont = new Font("YaHei Consolas Hybrid", 9);
                // 绘图
                float blueAngle = (float)(360 * (allSize - useSize) / allSize / 1.0);
                g.FillPie(surplusBrush, 50, 20, 150, 150, 0, 360);
                g.FillPie(useBrush, 50, 20, 150, 150, 180, blueAngle);
                g.FillPie(writeBrush, 50 + 25, 20 + 25, 100, 100, 0, 360);
                textBox1.Text = ((float)(1.0 * useSize / allSize)).ToString("P1");
            }
            catch (Exception)
            {

            }
        }
    }
}
