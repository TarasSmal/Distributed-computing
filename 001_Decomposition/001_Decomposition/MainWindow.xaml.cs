using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;


namespace _001_Decomposition
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static int N { get; set; }
		public static int MatrixMin { get; set; }
		public static int MatrixMax { get; set; }
		public static int VectorMax { get; set; }
		public static int VectorMin { get; set; }



		[DllImport("user32.dll", EntryPoint = "FindWindowEx")]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

		public MainWindow()
		{
			InitializeComponent();
		}

		private void button_calc_Click(object sender, RoutedEventArgs e)
		{
			var date = DateTime.Now;
			var LogFile = "log-" + date.ToString("yyyy-MM-dd-HH-mm") + ".txt";
			var f = File.Create(LogFile);
			f.Close();

			///Ініціалізація вхідних даних
			Lab.Data.Matrix A = new Lab.Data.Matrix(N, MatrixMax, MatrixMin);
			Lab.Data.Matrix B2 = new Lab.Data.Matrix(N, MatrixMax, MatrixMin);
			Lab.Data.Matrix A2 = new Lab.Data.Matrix(N, MatrixMax, MatrixMin);
			Lab.Data.Vector b1 = new Lab.Data.Vector(N, VectorMax, VectorMin);
			Lab.Data.Matrix C2 = new Lab.Data.Matrix(N);
			Lab.Data.Matrix A1 = new Lab.Data.Matrix(N, MatrixMax, MatrixMin);
			Lab.Data.Vector c1 = new Lab.Data.Vector(N, VectorMax, VectorMin);
			Lab.Data.Vector b = new Lab.Data.Vector(N);

			///Логування вхідних даних
			A.WriteToFile(LogFile, "Matrix A");
			A1.WriteToFile(LogFile, "Matrix A1");
			A2.WriteToFile(LogFile, "Matrix A2");
			B2.WriteToFile(LogFile, "Matrix B2");
			b1.WriteToFile(LogFile, "Vector b1");
			c1.WriteToFile(LogFile, "Vector c1");
			b.WriteToFile(LogFile, "vactor b");
			C2.WriteToFile(LogFile, "Matrix C2");


			///Перші обчислення 
			var y1 = A * b;
			y1.WriteToFile(LogFile, "Vector y1");

			var Y3 = A2 * B2;
			Y3.WriteToFile(LogFile, "matrix Y3 poslidovno");

			var b1c1 = b1 + (20 * c1);
			b1c1.WriteToFile(LogFile, "sub b1 c1");

			///Другий блок обчислень
			Y3 = Y3 - C2;
			Y3.WriteToFile(LogFile, "End of Y3 calc");

			var y2 = A1 * b1c1;
			y2.WriteToFile(LogFile, "End of y2 calc");

			///Другий блок обчислень
			var Y3qr = Y3 * Y3;
			Y3qr.WriteToFile(LogFile, "Y3^2");

			///перший доданок
			var Y3y2 = Y3qr * y2;
			Y3y2.WriteToFile(LogFile, "(Y3^2)*y2");

			///Другий доданок
			var Y3y1 = Y3 * y1;
			Y3y1.WriteToFile(LogFile, "Y3*y1");

			var y2y2 = y2 * y2;
			using (StreamWriter str = new StreamWriter(LogFile, true))
			{
				str.WriteLine("{0}", y2y2);
				str.WriteLine("///////////////////////////////////////////////////////////////////");
				str.Close();
			}

			var Y3New = y2y2 * Y3;
			Y3New.WriteToFile(LogFile, "Y3* y2`y2");

			///3 доданок
			var Y3Vector = Y3 * y1;
			Y3Vector.WriteToFile(LogFile, "y2`y2Y3y1");

			var firsObj = Y3y2 + Y3y1 + Y3Vector;
			firsObj.WriteToFile(LogFile, "Значення в дужках");

			var X = Y3qr * firsObj;
			X.WriteToFile(LogFile, "Finish Result ");


			MessageBox.Show("The End", "27 Yes");
			string res = "";
			for (int i = 0; i < X.n; i++)
			{
				res += X.vector[i].ToString()+" ";
			}

			Result.Text = res;
		}
		private void button_enter_data_Click(object sender, RoutedEventArgs e)
		{
			Window1 w1 = new Window1();
			w1.Show();
		}

		private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{

		}
	}
}
