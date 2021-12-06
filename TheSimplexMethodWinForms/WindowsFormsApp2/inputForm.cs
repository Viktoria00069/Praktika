using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class inputForm : Form
    {
        public inputForm(int variablesNumber)
        {
            InitializeComponent(); // Создание DGV и добавление названия колонок
            DataGridViewColumn[] Box = new DataGridViewColumn[variablesNumber];
            for (int i = 0; i < variablesNumber; i++)
            {
                this.dataGridView1.Columns.Add("x" + (i + 1), "x" + (i + 1));
            }
        }

        private void button1_Click(object sender, EventArgs e) //Кнопка начала расчета
         {
            try
            {
                Double[,] constraints = new Double[dataGridView1.RowCount, dataGridView1.ColumnCount]; // Cоздание вещественноого двойного массива размером с DGV
                Double[] BJs = new Double[dataGridView2.RowCount]; //Создание вещественного масива по кол-ву строк в DGV
                for (int i = 0; i < dataGridView1.RowCount; i++) 
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        constraints[i, j] = Convert.ToDouble(this.dataGridView1.Rows[i].Cells[j].Value);
                    }
                }
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    BJs[i] = Convert.ToDouble(this.dataGridView2.Rows[i].Cells[0].Value); //Заполнение DGV со свободными элементами
                }



                Regex template = new Regex(@"[a-z]+[0-9]", RegexOptions.IgnoreCase); // оператор приведения всего к прописному регистру

                String[] fString = template.Replace(textBox1.Text, "").Split('+');


                Double[] variables = new Double[fString.Length];
                for (int i = 0; i < fString.Length; i++)
                {
                    variables[i] = Convert.ToDouble(fString[i].Trim()); //Обрезание троки до числа
                }

                string minmax = Convert.ToString(listBox1.SelectedItem);

                output form = new output(constraints, variables, BJs, minmax, ActiveForm); // Открытие 3-й формы
                form.Show();
            }
            
            catch
            {
                //ActiveForm.Size = new System.Drawing.Size(622, 367);
            }
        }

        private void inputForm_Load(object sender, EventArgs e)
        {

        }
    }
}
