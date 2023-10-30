using System;
using System.Collections;
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
    public partial class Form1 : Form
    {
        int tn;
        int max_tick;
        const int max_tick_set = 200;
        const int max_interval = 5;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化文件
            if (!File.Exists(Class1.path + "\\source.txt"))
            {
                File.Create(Class1.path + "\\source.txt");
                File.WriteAllText(Class1.path + "\\source.txt", "default");
            }
            if (!File.Exists(Class1.path + "\\slist.st"))
            {
                File.Create(Class1.path + "\\slist.st");
                File.WriteAllText(Class1.path + "\\slist.st", "");
            }

            //检查激活状态
            Class1.machine = Environment.MachineName;
            System.IO.FileStream fs;
            string rd;
            fs = File.Open(Class1.path + "\\settings.st", System.IO.FileMode.OpenOrCreate);
            fs.Close();
            rd = File.ReadAllText(Class1.path + "\\settings.st");
            if(Class1.Md5(Class1.machine) == rd)
            {
                Class1.ac_info = true;
            }
            else
            {
                Class1.ac_info = false;
            }

            //初始化窗口
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            lang_refresh();
            refresh();
            button6.Hide();
            button6.Enabled = false;
            label3.Text = "v " + Class1.version;
            max_tick = max_tick_set;
            richTextBox1.ReadOnly = true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string fname;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fname = openFileDialog1.FileName;
                if (fname.Substring(fname.Length - 3, 3) == ".st")
                {
                    File.WriteAllText(Class1.path + "\\slist.st", File.ReadAllText(fname));
                }
                else
                {
                    File.WriteAllText(Class1.path + "\\source.txt", File.ReadAllText(fname));
                }
            }
            refresh();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //刷新前检查是否正在运行
            if(button6.Enabled == true)
            {
                return;
            }
            else
            {
                refresh();
            }
        }

        void refresh()
        {
            int temp = 0;
            string str = string.Empty;

            Class1.num = 0;
            Class1.al.Clear();

            //读取列表
            str = string.Empty;
            StreamReader ar = new StreamReader(Class1.path + "\\source.txt");
            while (!ar.EndOfStream)
            {
                str = ar.ReadLine();
                Class1.al.Add(str);
            }
            ar.Close();


            //读取配置
            str = string.Empty;
            StreamReader sr = new StreamReader(Class1.path + "\\slist.st");
            while (!sr.EndOfStream)
            {
                switch(temp % 2)
                {
                    case 0:
                        //读取条目名
                        str = sr.ReadLine();
                        Class1.sl_name.Add(str);
                        break;

                    case 1:
                        //读取配置值
                        str = sr.ReadLine();
                        Class1.sl_num.Add(int.Parse(str));
                        if ((int)Class1.sl_num[Class1.sl_num.Count - 1] < 0)
                        {
                            Class1.sl_num[Class1.sl_num.Count - 1] = -(int)Class1.sl_num[Class1.sl_num.Count];
                            Class1.sl_type.Add(false);
                        }
                        else
                        {
                            Class1.sl_type.Add(true);
                        }
                        break;
                }
                temp++;
            }
            sr.Close();

            Class1.num = Class1.al.Count;
            Class1.line = 1;
            Class1.target_num = Int32.Parse(this.numericUpDown1.Value.ToString());
            label1.Text = Class1.num.ToString() + (Class1.lang == 0 ? " items": " 个条目");
            numericUpDown1.Maximum = Class1.num;

            button3.Enabled = true;
            numericUpDown1.Enabled = true;
            richTextBox1.Text = Class1.lang == 0 ? "Result(s):": "结果:";
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Class1.target_num = Int32.Parse(this.numericUpDown1.Value.ToString());
            tn = 0;
            timer1.Enabled = true;
            timer1.Interval = 200;
            timer1.Start();

            button6.Show();
            button6.Enabled = false;
            button3.Enabled = false;
            button6.Text = Class1.lang == 0 ? "Accelerating..." : "加速中...";
        }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if(Int32.Parse(this.numericUpDown1.Value.ToString())<1)
            {
                MessageBox.Show(Class1.lang == 0 ? "The target number could not below 1.": "抽取数量不能小于1。");
                numericUpDown1.Value = 1;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if(Class1.al.Count == 1)
            {
                richTextBox1.Text = richTextBox1.Text + "\n" + "[" + Class1.line + "]\t" + Class1.al[0].ToString();
                label1.Text = Class1.al[0].ToString();
                timer1.Stop();
                MessageBox.Show(Class1.lang == 0 ? "The picking is over." : "源数据已经全部抽取。");
                button3.Enabled = false;
                numericUpDown1.Enabled = false;
                button6.Hide();
                button6.Enabled = false;
                checkb();
                return;
            }

            if(timer1.Interval <= max_interval)
            {
                button6.Text = Class1.lang == 0 ? "Stop!" : "停止!";
                button6.Enabled = true;
                button3.Enabled = true;
            }

            int index_n = Class1.ran.Next(0, Class1.al.Count);
            label1.Text = Class1.al[index_n].ToString();
            tn++;

            if (tn >= max_tick)
            {
                //抽取！
                if (Class1.sl_name.Contains(Class1.al[index_n]) == true
                    && (int)Class1.sl_num[Class1.sl_name.IndexOf(Class1.al[index_n])] > Class1.line)
                {
                    timer1.Interval = 1;
                }
                else
                {
                    richTextBox1.Text = richTextBox1.Text + "\n" + "[" + Class1.line + "]\t" + Class1.al[index_n].ToString();
                    Class1.al.RemoveAt(index_n);
                    Class1.num--;
                    numericUpDown1.Maximum--;
                    Class1.line++;
                    Class1.target_num--;
                    if (Class1.target_num == 0)
                    {
                        timer1.Stop();
                        button6.Hide();
                        button6.Enabled = false;
                        checkb();
                    }
                }
            }

            if(timer1.Interval > max_interval)
            {
                timer1.Interval -= 2;
                if(timer1.Interval > 60)
                {
                    timer1.Interval -= 10;
                }
            }
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.richTextBox1.SelectionStart = this.richTextBox1.TextLength;
            this.richTextBox1.ScrollToCaret();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            max_tick = 0;
        }

        private void CheckBox1_Click(object sender, EventArgs e)
        {
            checkb();
        }

        void checkb()
        {
            switch(checkBox1.Checked)
            {
                case true:
                    max_tick = max_tick_set;
                    break;

                case false:
                    max_tick = int.MaxValue;
                    break;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();
            string filename = saveFileDialog1.FileName;
            if (dr == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(filename))
            {
                StreamWriter sw = new StreamWriter(filename, true, Encoding.UTF8);
                sw.Write(richTextBox1.Text);
                sw.Close();
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if(Class1.lang == 1)
            {
                Class1.lang = 0;
            }
            else
            {
                Class1.lang = 1;
            }

            lang_refresh();
        }

        void lang_refresh()
        {
            string s;
            s = richTextBox1.Text;
            switch(Class1.lang)
            {
                case 0:
                    button1.Text = "Open";
                    button2.Text = "Save";
                    button4.Text = "Refresh";
                    label2.Text = "Target number";
                    button3.Text = "Pick!";
                    button6.Text = "Stop!";
                    checkBox1.Text = "Auto stop";
                    label1.Text = label1.Text.Replace("个条目", "items");
                    richTextBox1.Text = "Result(s):" + s.Remove(0, 3);
                    saveFileDialog1.FileName = "Results";
                    break;

                case 1:
                    button1.Text = "打开";
                    button2.Text = "保存";
                    button4.Text = "刷新";
                    label2.Text = "目标数量";
                    button3.Text = "抽取!";
                    button6.Text = "停止!";
                    checkBox1.Text = "自动停止";
                    label1.Text = label1.Text.Replace("items", "个条目");
                    richTextBox1.Text = "结果:" + s.Remove(0, 10);
                    saveFileDialog1.FileName = "结果";
                    break;
            }
            ti_refresh();
            button2.Enabled = Class1.ac_info;
        }

        private void Label3_Click(object sender, EventArgs e)
        {
            Form ab = new AboutBox1();
            ab.ShowDialog();
        }

        void ti_refresh()
        {
            string s = string.Empty;
            switch (Class1.lang)
            {
                case 0:
                    s = "[This is an inactive copy. Please active it.]";
                    break;

                case 1:
                    s = "[此程序副本未激活，请立即激活。]";
                    break;
            }

            switch (Class1.ac_info)
            {
                case true:
                    this.Text = "Let's Pick ";
                    break;

                case false:
                    this.Text = "Let's Pick " + s;
                    break;
            }
        }
    }
}
