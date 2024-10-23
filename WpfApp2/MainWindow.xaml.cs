using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Matrix<int> _matrixA;
        private Matrix<int> _matrixB;
        private Matrix<int> _matrixC;
        public MainWindow()
        {
            InitializeComponent();
        }
        //"Сгенерировать матрицы"
        private void GenerateMatricesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MatrixSizeTextBox.Text, out int size) || size <= 0)
            {
                MessageBox.Show("введите корректный размер матрицы.");
                return;
            }

            _matrixA = Matrix<int>.Generate(size, size, (i, j) => new Random().Next(1, 100));
            _matrixB = Matrix<int>.Generate(size, size, (i, j) => new Random().Next(1, 100));

            // Отображение матриц A и B на форме
            GenerateMatrixGrid(MatrixAGrid, _matrixA);
            GenerateMatrixGrid(MatrixBGrid, _matrixB);
        }

        // генерация матриц
        private void GenerateMatrixGrid(Grid grid, Matrix<int> matrix)
        {
            grid.Children.Clear(); 
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            // Добавление строк и столбцов в сетку
            for (int i = 0; i < matrix.Rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < matrix.Columns; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Заполнение сетки текстовыми полями с элементами матрицы
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    var textBox = new TextBox
                    {
                        Text = matrix[i, j].ToString(),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Width = double.NaN,
                        Height = double.NaN,
                        FontSize = 16
                    };
                    int row = i; // Сохраняем текущие значения i и j
                    int col = j;
                    textBox.TextChanged += (s, args) =>
                    {
                        if (int.TryParse(textBox.Text, out int value))
                        {
                            // Проверка на выход за границы массива
                            if (row < matrix.Rows && col < matrix.Columns)
                            {
                                matrix[row, col] = value;
                            }
                            else
                            {
                                MessageBox.Show("Индекс выходит за границы массива.");
                            }
                        }
                    };
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                    grid.Children.Add(textBox);
                }
            }
        }

        //"Рассчитать"
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixA == null || _matrixB == null)
            {
                MessageBox.Show("сгенерируйте матрицы сначала.");
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (AddRadioButton.IsChecked == true)
            {
                _matrixC = _matrixA + _matrixB;
            }
            else if (MultiplyRadioButton.IsChecked == true)
            {
                _matrixC = _matrixA * _matrixB;
            }

            stopwatch.Stop();

            // Вывод результирующей матрицы и времени
            MessageBox.Show($"Результирующая матрица:\n{_matrixC}\n\nВремя выполнения: {stopwatch.ElapsedMilliseconds} мс");
        }

    //"Сохранить"
    private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_matrixC == null)
            {
                MessageBox.Show("Пожалуйста, выполните расчет результата сначала.");
                return;
            }

            // Открытие диалогового окна для сохранения файла
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                DefaultExt = "csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, _matrixC.ToCsv());
                MessageBox.Show("Матрица успешно сохранена.");
            }
        }
        // прикольная смена значков
        private void OperationRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (AddRadioButton.IsChecked == true)
            {
                OperationTextBlock.Text = "+";
            }
            else if (MultiplyRadioButton.IsChecked == true)
            {
                OperationTextBlock.Text = "×";
            }
        }

    }
}