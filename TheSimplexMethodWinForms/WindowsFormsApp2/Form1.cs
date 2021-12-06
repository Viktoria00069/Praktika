using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            String fString = "1x1 + 2x2 + 3x3"; // Текстовая константа
            Regex regex = new Regex(@"[a-z]+[0-9]", RegexOptions.IgnoreCase); // Оператор приведениея выражения к маленькому регистру.

            regex.Replace(fString, "a"); //Замена иксов на 'a'

            Console.WriteLine(regex.Replace(fString, "")); //Убиранеие всех пробелов в функции
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // ActiveForm.Size = new System.Drawing.Size(339, 289);

            try // Вызов 2-й формы
            {
                inputForm form = new inputForm(Convert.ToInt32(textBox1.Text));
                form.Show();
            }

            catch (System.FormatException)
            {
                ActiveForm.Size = new System.Drawing.Size(339, 289);
            }

        }
    }
}
