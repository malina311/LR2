using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Matrix<T> where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        private T[,] _data;

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        // Конструктор для инициализации матрицы
        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _data = new T[rows, columns];
        }

        // Индексатор для доступа к элементам матрицы
        public T this[int i, int j]
        {
            get => _data[i, j];
            set => _data[i, j] = value;
        }

        // сложение
        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            if (a.Rows != b.Rows || a.Columns != b.Columns)
                throw new ArgumentException("Размеры матриц должны быть одинаковыми для сложения.");

            var result = new Matrix<T>(a.Rows, a.Columns);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i, j] = (dynamic)a[i, j] + b[i, j];
                }
            }
            return result;
        }

        // умножение
        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            if (a.Columns != b.Rows)
                throw new ArgumentException("Количество столбцов первой матрицы должно быть равно количеству строк второй матрицы.");

            var result = new Matrix<T>(a.Rows, b.Columns);
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Columns; j++)
                {
                    result[i, j] = default(T);
                    for (int k = 0; k < a.Columns; k++)
                    {
                        result[i, j] = (dynamic)result[i, j] + (dynamic)a[i, k] * b[k, j];
                    }
                }
            }
            return result;
        }

        // Генерация матрицы с использованием делегата
        public static Matrix<T> Generate(int rows, int columns, Func<int, int, T> generator)
        {
            var matrix = new Matrix<T>(rows, columns);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = generator(i, j);
                }
            }
            return matrix;
        }

        // Преобразование матрицы в строку
        public override string ToString()
        {
            var rows = new List<string>();
            for (int i = 0; i < Rows; i++)
            {
                var row = string.Join(", ", Enumerable.Range(0, Columns).Select(j => _data[i, j].ToString()));
                rows.Add(row);
            }
            return string.Join(Environment.NewLine, rows);
        }

        // Преобразование матрицы в CSV формат
        public string ToCsv()
        {
            return string.Join(Environment.NewLine, Enumerable.Range(0, Rows).Select(i => string.Join(",", Enumerable.Range(0, Columns).Select(j => _data[i, j].ToString()))));
        }
    }
}