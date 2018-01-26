using System;
using System.Windows;

namespace _001_Decomposition
{
	/// <summary>
	/// Логика взаимодействия для Window1.xaml
	/// </summary>
	public partial class Window1 :Window
	{
		public Window1()
		{
			InitializeComponent();
		}

		private void button_data_set_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.MatrixMin = Convert.ToInt32(MinMatrix.Text);
			MainWindow.MatrixMax = Convert.ToInt32(MaxMatrix.Text);
			MainWindow.N = Int32.Parse(Size.Text);
			MainWindow.VectorMin = Convert.ToInt32(VectorMin.Text);
			MainWindow.VectorMax = Convert.ToInt32(VectorMax.Text);

			this.Close();
		}
	}
}