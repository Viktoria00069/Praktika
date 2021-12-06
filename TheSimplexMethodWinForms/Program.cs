using System;

namespace ConsoleApp1
{
    
    
    class Program
    {
        static double[][] basis;
        static double[] B;
        static double[] F;
        static int constraints, variables;
        static int Bs, Fs;

        static void Main(string[] args)
        {
            inputData();

            /*** making the tableau. ***/
            setUpTableau();

            // output.
            printMatrix(basis);

            /*** calculations ***/

            // get the pivots
            while (F[findIdxOfMostNegative(F)] < 0)
            {
                int col_pivot = getColPivot();
                int row_pivot = getRowPivot(col_pivot);
                makePivotOne(col_pivot, row_pivot);
                makePivotZeros(col_pivot, row_pivot);

                // output.
                printMatrix(basis);
            }
            Console.WriteLine("\nAnswer:");
            Console.WriteLine("F = " + B[B.Length - 1]);
        }

        public static void inputData()
        {
            Console.Write("Enter the number of constraints: ");
            constraints = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the number of variables: ");
            variables = Convert.ToInt32(Console.ReadLine());

            Bs = constraints + 1;
            Fs = constraints + variables + 1;

            basis = new double[constraints][];
            for (int i = 0; i < constraints; i++)
                basis[i] = new double[Fs];

            B = new double[Bs];
            F = new double[Fs];

            Console.WriteLine("Enter the coefficients of the objective function:");
            for (int x = 0; x < variables; x++)
            {
                Console.Write("x" + (x + 1) + " = ");
                F[x] = Convert.ToDouble(Console.ReadLine());
            }

            Console.WriteLine("Enter the values of the system of constraints:");
            for (int s = 0; s < constraints; s++)
            {
                for (int x = 0; x < variables; x++)
                {
                    Console.Write("S" + (s + 1) + " x" + (x + 1) + " = ");
                    basis[s][x] = Convert.ToDouble(Console.ReadLine());
                }
                Console.Write("B" + (s + 1) + " = ");
                B[s] = Convert.ToDouble(Console.ReadLine());
            }
        }

        public static void setUpTableau()
        {
            /*** making the tableau. ***/

            B[B.Length - 1] = 0;

            for (int i = 0; i < F.Length; i++)
            {
                if (F[i] != 0)
                {
                    F[i] = F[i] * -1;
                }
            }
            F[F.Length - 1] = 1;

            for (int s = 0; s < constraints; s++)
            {
                for (int x = variables; x < Fs - 1; x++)
                {
                    if (x % variables == s)
                    {
                        basis[s][x] = 1;
                    }
                }
            }
        }

        public static int getColPivot()
        {
            return findIdxOfMostNegative(F);
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

        public static int findIdxOfMostNegative(double[] array)
        {
            int idxOfMostNegative = 0;

            for (int i = 1; i < array.Length; i++)
                idxOfMostNegative = (array[i] < array[idxOfMostNegative]) ? i : idxOfMostNegative;

            return idxOfMostNegative;
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

        public static void printMatrix(double[][] matrix)
        {
            Console.Write("    ");
            for (int i = 0; i < variables; i++)
            {
                Console.Write("x" + (i + 1) + "    ");
            }
            for (int i = 0; i < constraints; i++)
            {
                Console.Write("S" + (i + 1) + "    ");
            }
            Console.Write("P     ");
            Console.WriteLine("B");

            for (int s = 0; s < matrix.Length; s++)
            {
                Console.Write("S" + (s + 1) + " ");
                for (int x = 0; x < matrix[s].Length; x++)
                {
                    Console.Write("[" + matrix[s][x] + "] ");
                }
                Console.WriteLine("[" + B[s] + "]");
            }

            Console.Write("F  ");
            printArray(F);

            Console.WriteLine("[" + B[B.Length - 1] + "]");

        }

        public static void printArray(double[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write("[" + array[i] + "] ");
            }
        }
    }
}
