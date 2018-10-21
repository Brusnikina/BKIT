using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> list = new List<string>();
        private void reading_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "текстовые файлы|*.txt";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                
                string text = File.ReadAllText(fd.FileName, Encoding.GetEncoding(1251));                                
                char[] separators = new char[] { ' ', '.', ',', '!', '?', '/', '\t', '\n' };
                string[] textArray = text.Split(separators);
                foreach (string strTemp in textArray)
                {                   
                    string str = strTemp.Trim();                   
                    if (!list.Contains(str)) list.Add(str);
                }

                timer.Stop();
                this.Time_of_loading.Text = timer.Elapsed.ToString();

                MessageBox.Show("Чтение файла завершено.");
            }
            else
            {
                MessageBox.Show("Необходимо выбрать файл!");
            }
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Search_Click(object sender, EventArgs e)
        {            
            string word = this.Field_for_input.Text.Trim();            
            if (!string.IsNullOrWhiteSpace(word) && list.Count > 0)
            {                
                string wordUpper = word.ToUpper();                
                List<string> tempList = new List<string>();

                Stopwatch timer = new Stopwatch();
                timer.Start();

                foreach (string str in list)
                {
                    if (str.ToUpper().Contains(wordUpper))
                    {
                        tempList.Add(str);
                    }
                }

                timer.Stop();
                this.Time_of_searching.Text = timer.Elapsed.ToString();

                this.Result_list.BeginUpdate();                
                this.Result_list.Items.Clear();                 
                foreach (string str in tempList)
                {
                    this.Result_list.Items.Add(str);
                }
                this.Result_list.EndUpdate();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать файл и ввести слово для поиска");
            }
        }
    }
}
