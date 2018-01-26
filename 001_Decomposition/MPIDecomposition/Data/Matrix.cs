using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Lab.Data
{
    [Serializable]
	public class Matrix
	{
		#region Private Properties
		public double[,] matrix { get; set; }
		private int n { get; set; }

        //static Mutex mutexObj = new Mutex();
        bool existed;
        // получаем GIUD приложения
        string guid = Marshal.GetTypeLibGuidForAssembly(Assembly.GetExecutingAssembly()).ToString();

        #endregion

        #region Constructor 

        /// <summary>
        /// Ініціалізація і радомне заповнення квадратної матриці 
        /// </summary>
        /// <param name="n">Розмірність матриці</param>
        public Matrix(int n, bool Randomize)
		{
			this.n = n;
			matrix = new double[n, n];
			Random rnd = new Random();

			if (Randomize)
				for (int i = 0; i < n; i++)
				{
					for (int j = 0; j < n; j++)
					{
						matrix[i, j] = rnd.Next();
					}
				}

		}

		/// <summary>
		/// Ініціалізація і радомне заповнення квадратної матриці з заданим масимальним значенням
		/// </summary>
		/// <param name="N"></param>
		/// <param name="max">Максимальне значення</param>
		public Matrix(int n, int max)
		{
			this.n = n;
			matrix = new double[n, n];
			Random rnd = new Random();

			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					matrix[i, j] = rnd.Next(max);
				}
			}

		}

		/// <summary>
		/// Ініціалізація і радомне заповнення квадратної матриці з заданим масимальним і мінімальним значеннями
		/// </summary>
		/// <param name="n">Розмірність матриці</param>
		/// <param name="max">Максимальне значення</param>
		/// <param name="min">Мінімальне значення</param>
		public Matrix(int n, int max, int min)
		{
			this.n = n;
			matrix = new double[n, n];
			Random rnd = new Random();

			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					matrix[i, j] = rnd.Next(min, max);
				}
			}

		}

		public Matrix(int n)
		{
			this.n = n;
			matrix = new double[n, n];
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					matrix[i, j] = 21 / (Math.Pow(i+1, 2) + (2*(j+1)));
				}
			}
		}

        #endregion

        #region Combine Methods

        public void Transponate()
        {
            double tmp;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    tmp = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = tmp;
                }
            }
        }

        public void WriteToFile(string fileName, string Notes)
        {
            void Send()
            {
                using (StreamWriter str = new StreamWriter(fileName, true))
                {
                    str.WriteLine(Notes);
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            str.Write("[{0}] ", this.matrix[i, j]);
                        }
                        str.WriteLine();
                    }
                    str.WriteLine("//////////////////////////////////////////////////////////////////////////////////////////");
                    str.Close();
                }
            }
            Mutex mutexObj = new Mutex(true, guid, out existed);
            if (existed)
                Send();
            else
            {
                WaitSend();
            }
            void WaitSend()
            {
                try
                {
                    Thread.Sleep(3500);
                    Send();
                }
                catch
                {
                    WaitSend();
                }
            }
        }

        #endregion

        #region Operators

        public static Matrix operator *(Matrix value1, Matrix value2)
		{
			Matrix resusl = new Matrix(value1.n, false);

			for (int i = 0; i < value1.matrix.GetLength(0); i++)
			{
				for (int j = 0; j < value2.matrix.GetLength(1); j++)
				{
					for (int k = 0; k < value2.matrix.GetLength(0); k++)
					{
						resusl.matrix[i, j] += value1.matrix[i, k] * value2.matrix[k, j];
					}
				}
			}

			return resusl;
		}

		public static Matrix operator /(Matrix value1, Matrix value2)
		{
			Matrix resusl = new Matrix(value1.n, false);

			Parallel.For((0), value1.matrix.GetLength(0), (i) =>
			{
				for (int j = 0; j < value2.matrix.GetLength(1); j++)
				{
					for (int k = 0; k < value2.matrix.GetLength(0); k++)
					{
						resusl.matrix[i, j] += value1.matrix[i, k] * value2.matrix[k, j];
					}
				}
			});
			return resusl;

		}

		public static Vector operator *(Matrix value1, Vector value2)
		{
			Vector resault = new Vector(value1.n, false);
			for (int i = 0; i < value1.matrix.GetLength(0); i++)
			{
				for (int j = 0; j < value2.vector.GetLength(0); j++)
				{
					resault.vector[i] += value1.matrix[i, j] * value2.vector[j];
				}
			}

			return resault;
		}

		public static Vector operator *(Vector value1, Matrix value2)
		{
			var res = new Vector(value1.n);
			for (int i = 0; i < value2.matrix.GetLength(0); i++)
			{
				for (int j = 0; j < value1.vector.GetLength(0); j++)
				{
					res.vector[i] += value2.matrix[i, j] * value1.vector[j];
				}
			}
			return res;
		}

		public static Matrix operator -(Matrix value1, Matrix value2)
		{
			Matrix result = new Matrix(value1.n, false);
			for (int i = 0; i < value1.n; i++)
			{
				for (int j = 0; j < value1.n; j++)
				{
					result.matrix[i, j] = value1.matrix[i, j] - value2.matrix[i, j];
				}
			}

			return result;
		}

		public static Matrix operator *(double value1, Matrix value2)
		{
			for (int i = 0; i < value2.n; i++)
			{
				for (int j = 0; j < value2.n; j++)
				{
					value2.matrix[i, j] *= value1;
				}
			}

			return value2;
		}

        #endregion Operators
	}
}
