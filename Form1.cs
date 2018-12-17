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
                int maxDist;
                if (!int.TryParse(this.text_dist.Text.Trim(), out maxDist))
                {
                    MessageBox.Show("Необходимо указать максимальное расстояние");
                    return;
                }

                if (maxDist < 1 || maxDist > 5)
                {
                    MessageBox.Show("Максимальное расстояние должно быть в диапазоне от 1 до 5");
                    return;
                }

                int ThreadCount;
                if (!int.TryParse(this.count_of_stream.Text.Trim(), out ThreadCount))
                {
                    MessageBox.Show("Необходимо указать количество потоков");
                    return;
                }

                Stopwatch timer = new Stopwatch();
                timer.Start();
                
                //Результирующий список  
                List<ParallelSearchResult> Result = new List<ParallelSearchResult>();

                //Деление списка на фрагменты для параллельного запуска в потоках
                List<MinMax> arrayDivList = SubArrays.DivideSubArrays(0, list.Count, ThreadCount);
                int count = arrayDivList.Count;

                //Количество потоков соответствует количеству фрагментов массива
                Task<List<ParallelSearchResult>>[] tasks = new Task<List<ParallelSearchResult>>[count];

                //Запуск потоков
                for (int i = 0; i < count; i++)
                {
                    //Создание временного списка, чтобы потоки не работали параллельно с одной коллекцией
                    List<string> tempTaskList = list.GetRange(arrayDivList[i].Min, arrayDivList[i].Max - arrayDivList[i].Min);

                    tasks[i] = new Task<List<ParallelSearchResult>>(
                        //Метод, который будет выполняться в потоке
                        ArrayThreadTask,
                        //Параметры потока 
                        new ParallelSearchThreadParam()
                        {
                            tempList = tempTaskList,
                            maxDist = maxDist,
                            ThreadNum = i,
                            wordPattern = word
                        });

                    //Запуск потока
                    tasks[i].Start();
                }
                Task.WaitAll(tasks);

                timer.Stop();

                //Объединение результатов
                for (int i = 0; i < count; i++)
                {
                    Result.AddRange(tasks[i].Result);
                }                

                //timer.Stop();                

                //Время поиска
                this.Time_of_searching.Text = timer.Elapsed.ToString();

                //Вычисленное количество потоков
                this.label_count_of_steam.Text = count.ToString();

                //Обновление списка результатов
                this.Result_list.BeginUpdate();              
                this.Result_list.Items.Clear();                
                foreach (var x in Result)
                {
                    string temp = x.word + "(расстояние=" + x.dist.ToString() + " поток=" + x.ThreadNum.ToString() + ")";
                    this.Result_list.Items.Add(temp);
                }               
                this.Result_list.EndUpdate();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать файл и ввести слово для поиска");
            }
        }

        public static List<ParallelSearchResult> ArrayThreadTask(object paramObj)
        {
            ParallelSearchThreadParam param = (ParallelSearchThreadParam)paramObj;
           
            string wordUpper = param.wordPattern.Trim().ToUpper();

            //Результаты поиска в одном потоке
            List<ParallelSearchResult> Result = new List<ParallelSearchResult>();

            //Перебор всех слов во временном списке данного потока 
            foreach (string str in param.tempList)
            {
                //Вычисление расстояния Дамерау-Левенштейна
                int dist = EditDistance.Distance(str.ToUpper(), wordUpper);
                //Если расстояние меньше порогового, то слово добавляется в результат
                if (dist <= param.maxDist)
                {
                    ParallelSearchResult temp = new ParallelSearchResult()
                    {
                        word = str,
                        dist = dist,
                        ThreadNum = param.ThreadNum
                    };
                    Result.Add(temp);
                }
            }
            return Result;
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

        private void button_report_Click(object sender, EventArgs e)
        {            
            string TempReportFileName = "Report_" + DateTime.Now.ToString("dd_MM_yyyy_hhmmss");

            //Диалог сохранения файла отчета
            SaveFileDialog fd = new SaveFileDialog();
            fd.FileName = TempReportFileName;         
            fd.Filter = "Text files|*.txt|HTML Reports|*.html;";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                string ReportFileName = fd.FileName;
                if (Path.GetExtension(fd.FileName) == ".txt")
                {
                    StreamWriter sw = new StreamWriter(fd.FileName);
                    sw.WriteLine("Отчет: " + ReportFileName);
                    sw.WriteLine("Время чтения из файла: " + this.Time_of_loading.Text);
                    sw.WriteLine("Количество уникальных слов в файле: " + this.count_of_words.Text);
                    sw.WriteLine("Слово для поиска: " + this.Field_for_input.Text);
                    sw.WriteLine("Максимальное расстояние для нечеткого поиска: " + this.text_dist.Text);
                    sw.WriteLine("Время четкого поиска: " + this.exp_search_time.Text);
                    sw.WriteLine("Время нечеткого поиска: " + this.Time_of_searching.Text);
                    sw.WriteLine("Результаты поиска: ");
                    foreach (var x in this.Result_list.Items)
                    {
                        sw.WriteLine(x.ToString());
                    }
                    sw.Close();                    
                }
                else
                {
                    StringBuilder b = new StringBuilder();
                    b.AppendLine("<html>");

                    b.AppendLine("<head>");
                    b.AppendLine("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/>");
                    b.AppendLine("<title>" + "Отчет: " + ReportFileName + "</title>");
                    b.AppendLine("</head>");

                    b.AppendLine("<body>");

                    b.AppendLine("<h1>" + "Отчет: " + ReportFileName + "</h1>");
                    b.AppendLine("<table border='1'>");

                    b.AppendLine("<tr>");
                    b.AppendLine("<td>Время чтения из файла</td>");
                    b.AppendLine("<td>" + this.Time_of_loading.Text + "</td>");
                    b.AppendLine("</tr>");

                    b.AppendLine("<tr>");
                    b.AppendLine("<td>Количество уникальных слов в файле</td>");
                    b.AppendLine("<td>" + this.count_of_words.Text + "</td>");
                    b.AppendLine("</tr>");

                    b.AppendLine("<tr>");
                    b.AppendLine("<td>Слово для поиска</td>");
                    b.AppendLine("<td>" + this.Field_for_input.Text + "</td>");
                    b.AppendLine("</tr>");

                    b.AppendLine("<tr>");
                    b.AppendLine("<td>Максимальное расстояние для нечеткого поиска</td>");
                    b.AppendLine("<td>" + this.text_dist.Text + "</td>");
                    b.AppendLine("</tr>");

                    b.AppendLine("<tr>");
                    b.AppendLine("<td>Время четкого поиска</td>");
                    b.AppendLine("<td>" + this.exp_search_time.Text + "</td>");
                    b.AppendLine("</tr>");

                    b.AppendLine("<tr>");
                    b.AppendLine("<td>Время нечеткого поиска</td>");
                    b.AppendLine("<td>" + this.Time_of_searching.Text + "</td>");
                    b.AppendLine("</tr>");

                    b.AppendLine("<tr valign='top'>");
                    b.AppendLine("<td>Результаты поиска</td>");
                    b.AppendLine("<td>");
                    b.AppendLine("<ul>");

                    foreach (var x in this.Result_list.Items)
                    {
                        b.AppendLine("<li>" + x.ToString() + "</li>");
                    }

                    b.AppendLine("</ul>");
                    b.AppendLine("</td>");
                    b.AppendLine("</tr>");

                    b.AppendLine("</table>");

                    b.AppendLine("</body>");
                    b.AppendLine("</html>");

                    //Сохранение файла
                    File.AppendAllText(ReportFileName, b.ToString());
                }                               
                MessageBox.Show("Отчет успешно сформирован. Файл: " + ReportFileName);
            }
        }
    }
}
