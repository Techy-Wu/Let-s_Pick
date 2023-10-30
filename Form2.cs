using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Lottery_Picking
{
    public partial class Key_Form : Form
    {
        public Key_Form()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.ToString().ToUpper();
            if (textBox1.Text.ToString() == Class1.Md5(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString())) 
            {
                MessageBox.Show("激活成功，请重启软件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                File.WriteAllText(Class1.path + "\\settings.st", Class1.Md5(Class1.machine));
                Class1.ac_info = true;
            }
            else
            {
                MessageBox.Show("激活码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
