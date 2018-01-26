using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Data
{
    [Serializable]
	public class Vector
	{
		public double[] vector { get; set; }
		public int n { get; set; }

		#region Constructor 
		public Vector(int n, bool Randomize)
		{
			this.n = n;
			vector = new double[n];
			Random rnd = new Random();

			if (Randomize)
				for (int i = 0; i < n; i++)
				{
					vector[i] = rnd.Next();
				}
		}

		public Vector(Int32 n)
		{
			this.n = n;
			vector = new double[n];
			for (int i = 0; i < n ; i++)
				vector[i] = 21 / Math.Pow(i+1, 4);
		}

		public Vector(int n, int max)
		{
			this.n = n;
			vector = new double[n];
			Random rnd = new Random();

			for (int i = 0; i < n; i++)
			{
				vector[i] = rnd.Next(max);
			}

		}

		public Vector(int n, int max, int min)
		{
			this.n = n;
			vector = new double[n];
			Random rnd = new Random();

			for (int i = 0; i < n; i++)
			{
				vector[i] = rnd.Next(min, max);
			}

		}

		#endregion

		#region Combine Methods
		public void WriteToFile(string fileName, string Notes)
		{
			using (StreamWriter str = new StreamWriter(fileName, true))
			{
				str.WriteLine(Notes);
				for (int i = 0; i < n; i++)
				{
					str.Write("[{0}] ", vector[i]);

				}
				str.WriteLine();
				str.WriteLine("//////////////////////////////////////////////////////////////////////////////////////////");
				str.Close();
			}
		}

		#endregion

		#region Operators
		public static Double operator *(Vector value1, Vector value2)
		{
			double res = 0;
			for (int i = 0; i < value1.n; i++)
			{
				res += value1.vector[i] * value2.vector[i];
			}

			return res;
		}

		public static Vector operator *(double value1, Vector value2)
		{
			Vector res = new Vector(value2.n, false);
			for (int i = 0; i < value2.n; i++)
			{
				res.vector[i] = value1 * value2.vector[i];
			}

			return res;
		}

		public static Vector operator +(Vector value1, Vector value2)
		{
			var res =new Vector(value1.n, false);
			for (int i = 0; i < value1.n; i++)
			{
				res.vector[i] = value1.vector[i] + value2.vector[i];
			}
			return res;
		}

		public static Vector operator -(Vector value1, Vector value2)
		{
			var res = new Vector(value1.n, false);

			for (int i = 0; i < value1.n; i++)
			{
				res.vector[i] = value1.vector[i] - value2.vector[i];
			}

			return res;
		}
		#endregion
	}
}
