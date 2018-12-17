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
        public static int Distance(string str1Par, string str2Par)
        {
            if ((str1Par == null) || (str2Par == null)) return -1;
            int str1Len = str1Par.Length;
            int str2Len = str2Par.Length;

            if ((str1Len == 0) && (str2Len == 0)) return 0;
            if (str1Len == 0) return str2Len;
            if (str2Len == 0) return str1Len;

            string str1 = str1Par.ToUpper();
            string str2 = str2Par.ToUpper();
            int[,] matrix = new int[str1Len + 1, str2Len + 1];

            //Инициализация нулевой строки и нулевого столбца матрицы
            for (int i = 0; i <= str1Len; i++) matrix[i, 0] = i;
            for (int j = 0; j <= str2Len; j++) matrix[0, j] = j;
        
            for (int i = 1; i <= str1Len; i++)
            {
                for (int j = 1; j <= str2Len; j++)
                {
                    //Эквивалентность символов
                    int symbEqual = ((str1.Substring(i - 1, 1) == str2.Substring(j - 1, 1)) ? 0 : 1);
                    //Возможные операции
                    int ins = matrix[i, j - 1] + 1;
                    int del = matrix[i - 1, j] + 1;
                    int subst = matrix[i - 1, j - 1] + symbEqual;

                    //Элемент матрицы вычисляется как минимальный из трех случаев
                    matrix[i, j] = Math.Min(Math.Min(ins, del), subst);

                    //Дополнение Дамерау
                    if ((i > 1) && (j > 1) && (str1.Substring(i - 1, 1) == str2.Substring(j - 2, 1)) &&
                        (str1.Substring(i - 2, 1) == str2.Substring(j - 1, 1)))
                    {
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + symbEqual);
                    }
                }
            }
            //Нижний правый элемент матрицы
            return matrix[str1Len, str2Len];
        }
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
                this.count_of_words.Text = list.Count.ToString();

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
                    if (Distance(wordUpper, str.ToUpper()) <= int.Parse(text_dist.Text)) tempList.Add(str);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label_dist_Click(object sender, EventArgs e)
        {

        }

        private void explicit_search_Click(object sender, EventArgs e)
        {            
            string word = this.Field_for_input.Text.Trim();
           
            if (!string.IsNullOrWhiteSpace(word) && list.Count > 0)
            {                
                string wordUpper = word.ToUpper();
               
                List<string> tempList = new List<string>();

                Stopwatch t = new Stopwatch();
                t.Start();

                foreach (string str in list)
                {
                    if (str.ToUpper().Contains(wordUpper))
                    {
                        tempList.Add(str);
                    }
                }

                t.Stop();
                this.exp_search_time.Text = t.Elapsed.ToString();

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
