using System;
using System.Globalization;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class output : Form
    {
        static double[][] basis;
        static double[] B;
        static double[] F;
        static int constraints, variables;
        static int Fs;
        static bool isEnd = false;
        static string MinMax;
        static string[] header;

        public output(Double[,] InputConstraints, Double[] InputVariables, Double[] InputBJs, string InputMinMax, System.Windows.Forms.Form sender)
        {
            try
            {
                InitializeComponent();

                MinMax = InputMinMax;

                label1.Text = "Начальный базис";
                constraints = InputConstraints.GetLength(1);
                variables = InputVariables.Length;

                DataGridViewColumn[] Box = new DataGridViewColumn[variables];

                Fs = constraints + variables;
                if (MinMax.Equals("min"))
                    Fs += 1;

                basis = new double[constraints][];
                for (int i = 0; i < constraints; i++)
                    basis[i] = new double[Fs];

                B = InputBJs;
                F = new double[Fs];


                for (int x = 0; x < variables; x++)
                {
                    F[x] = InputVariables[x];
                }


                for (int s = 0; s < constraints; s++)
                {
                    for (int x = 0; x < variables; x++)
                        basis[s][x] = InputConstraints[s, x];
                }

                setUpTableau();
                printMatrix(basis, dataGridView1);
            }
            
            catch
            {
                sender.Size = new System.Drawing.Size(529, 478);
                this.Close();
            }

        }

        public static void printMatrix(double[][] matrix, DataGridView dataGridView1)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();


            dataGridView1.Columns.Add(" ", " "); // создание DGV
            for (int i = 0; i < variables; i++)
            {
                dataGridView1.Columns.Add("x" + (i + 1), "x" + (i + 1)); //Наименование колонок
            }

            for (int i = 0; i < variables; i++)
            {
                dataGridView1.Columns.Add("S" + (i+1), "S" + (i+1)); //Наименование колонок
            }

            if (MinMax.Equals("min"))
                dataGridView1.Columns.Add("P", "P");

            dataGridView1.Columns.Add("B", "B");

            int cellsCount = variables + 1;

            for (int i = 1; i <= constraints; i++)
                dataGridView1.Rows.Add("S" + i);
            

            for (int i = 0; i < matrix.Length ; i++)  //Заполнение матрицы
                for (int j = 0; j < matrix[0].Length; j++)
                    dataGridView1.Rows[i].Cells[j + 1].Value = matrix[i][j];


            for(int i = 0; i < B.Length; i++)
                dataGridView1.Rows[i].Cells[dataGridView1.ColumnCount - 1].Value = B[i];


            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = "F";

            for (int j = 0; j < F.Length; j++)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[j + 1].Value = F[j];

            dataGridView1.Columns[0].Width = 30;
        }

        public static void setUpTableau()
        {
            /*** making the tableau. ***/

            B[B.Length - 1] = 0;

            if (MinMax.Equals("min"))
                for (int i = 0; i < F.Length; i++)
                    if (F[i] != 0)
                        F[i] = F[i] * -1;

            if (MinMax.Equals("min"))
                F[F.Length - 1] = 1;

            for (int s = 0; s < constraints; s++)
                for (int x = variables; x < Fs - 1; x++)
                    if (x % variables == s)
                        basis[s][x] = 1;

            setUpTableauHeader();
        }

        public static void setUpTableauHeader()
        {
            header = new String[variables + constraints];

            for (int i = 0; i < variables; i++)
                header[i] = "x" + (i + 1);

            for (int i = 0; i < constraints; i++)
                header[variables + i] = "S" + (i + 1);
        }

        public static int findIdxOfMostNegative(double[] array)
        {
            int idxOfMostNegative = 0;

            for (int i = 1; i < array.Length; i++)
                idxOfMostNegative = (array[i] < array[idxOfMostNegative]) ? i : idxOfMostNegative;

            return idxOfMostNegative;
        }

        public static int findIdxOfLargest(double[] array)
        {
            int idxOfLargest = 0;

            for (int i = 1; i < array.Length; i++)
                idxOfLargest = (array[i] > array[idxOfLargest]) ? i : idxOfLargest;

            return idxOfLargest;
        }

        public static int getColPivot()
        {
            switch (MinMax)
            {
                case "min":
                    return findIdxOfMostNegative(F);
                case "max":
                    return findIdxOfLargest(F);
            }

            return 0;
        }

        public static int getRowPivot(int col_pivot)
        {
            int row_pivot = 0;

            for (int row = 0; row < basis.Length; row++)
            {
                double number = B[row] / basis[row][col_pivot];
                double smallest_number = B[row_pivot] / basis[row_pivot][col_pivot];

                row_pivot = (number < smallest_number) ? row : row_pivot;
            }

            return row_pivot;
        }

        public static void makePivotOne(int col_pivot, int row_pivot)
        {
            double denominator = basis[row_pivot][col_pivot];

            for (int col = 0; col < basis[0].Length; col++)
                basis[row_pivot][col] /= denominator;

            B[row_pivot] /= denominator;
        }

        public static void makePivotZeros(
                int col_pivot, int row_pivot)
        {
            double pivot_col_coefficient = 1;

            for (int row = 0; row < basis.Length; row++)
            {
                if (row == row_pivot) continue;
                pivot_col_coefficient = basis[row][col_pivot];

                for (int col = 0; col < basis[row].Length; col++)
                {
                    // basis[row][col] = basis[row][col] + basis[row][col_pivot] * -1 * basis[row_pivot][col];
                    basis[row][col] = basis[row][col] + (pivot_col_coefficient * basis[row_pivot][col] * -1);
                }
                B[row] = B[row] + (pivot_col_coefficient * B[row_pivot] * -1);
            }

            pivot_col_coefficient = F[col_pivot];
            B[B.Length - 1] = B[B.Length - 1] + (pivot_col_coefficient * B[row_pivot] * -1);

            for (int i = 0; i < F.Length; i++)
            {
                F[i] = F[i] + (pivot_col_coefficient * basis[row_pivot][i] * -1);
                // Console.WriteLine(F[i] + " + " + pivot_col_coefficient + " * " + -1 + " * " + basis[row_pivot][i]);
            }
        }

        public static String getResult()
        {
            String result = "";

            for (int col = 0; col < variables + constraints; col++)
            {
                bool thereIsOne = false;
                double BjValue = 0;
                for (int row = 0; row < basis.Length; row++)
                {
                    if (basis[row][col] != 0)
                    {
                        if (basis[row][col] == 1 && !thereIsOne)
                        {
                            thereIsOne = true;
                            BjValue = B[row];
                        }
                        else
                        {
                            BjValue = 0;
                            break;
                        }
                    }
                }
                result += header[col] + " = " + BjValue + "  ";
            }

            return result;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!isEnd)
            {
                label1.Text = "Конечный базис";

                button1.Visible = false;

                label2.Visible = true;
                label3.Visible = true;

                //ActiveForm.Size = new System.Drawing.Size(518, 355);

                switch (MinMax) //Проверка реузльтата
                {
                    case "min":
                        while (F[findIdxOfMostNegative(F)] < 0)
                        {
                            int col_pivot = getColPivot();
                            int row_pivot = getRowPivot(col_pivot);
                            makePivotOne(col_pivot, row_pivot);
                            makePivotZeros(col_pivot, row_pivot);

                            printMatrix(basis, dataGridView1);
                        }
                        break;
                    case "max":
                        while (F[findIdxOfLargest(F)] > 0)
                        {
                            int col_pivot = getColPivot();
                            int row_pivot = getRowPivot(col_pivot);
                            makePivotOne(col_pivot, row_pivot);
                            makePivotZeros(col_pivot, row_pivot);

                            printMatrix(basis, dataGridView1);
                        }
                        break;
                }

                label2.Text = "F = " + B[B.Length - 1];
                label3.Text = getResult();
            }
        }
    }
}
