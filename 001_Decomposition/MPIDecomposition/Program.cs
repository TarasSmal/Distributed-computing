using System;
using Lab.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace MPIDecomposition
{
    class Program
    {
        static void Main(string[] args)
        {
            var LogFile = "log-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".txt";

            var Size = 1;

            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                    switch (comm.Rank)
                    {
                    ///Перший процесс 
                        case 0:
                        Console.WriteLine($"Start: - {comm.Rank}");

                        ///Ініціалізація початкових даних
                        Vector b = new  Vector(Size);
                        Matrix A = new  Matrix(Size, 2, 0);

                        ///Обрахування вектора y1
                        var y1 = A * b;
                        // y1.WriteToFile(LogFile, "Vector y1");
                        Console.WriteLine($"Barrier Rank{comm.Rank}/ - {DateTime.Now}");
                        comm.Barrier();
                        int x = 0x5a;
                        x=0b1010101010101;
                        Console.WriteLine($"Barrier Rank{comm.Rank}/- {DateTime.Now}");

                        comm.Barrier();

                        var Y3to1 = comm.Receive<Matrix>(1, 0);
                        Console.WriteLine($"Receive from 1 to 0");
                        Console.WriteLine($"Barrier Rank{comm.Rank}/ - {DateTime.Now}");
                        comm.Send<Vector>(y1, 3, 0);

                        comm.Barrier();

                        ///Другий доданок
                        var Y3y1 = Y3to1 * y1;
                        Console.WriteLine($"Y3y1 well - {Y3y1.vector[0]}");
                        comm.Send<Vector>(Y3y1, 3, 0);

                        break;            
                        
                         //////////////////////////////////////////////////////////     
                        ///Другий процесс
                        ///////////////////////////////////////////////////////////
                        case 1:
                        Console.WriteLine($"Start: - {comm.Rank}");

                        ///Ініціалізаці початкових даних
                        Matrix B2 = new Matrix(Size, 2, 0);
                        Matrix A2 = new Matrix(Size, 2, 0);

                        //Початок обрахунку Y3
                        var Y3 = A2 * B2;
                        Console.WriteLine($"BarrierRank{comm.Rank}/ - {DateTime.Now}");
                        comm.Barrier();

                        //Отримання пересланих даних від 3 процесса
                        var C2r = comm.Receive<Matrix>(2, 0);
                        Console.WriteLine($"Receive from 2 to 1");

                        ///Закінчення обрахунку Y3
                        Y3 = Y3 - C2r;
                        var Y3qr = Y3 * Y3;
                        Console.WriteLine($"Barrier Rank{comm.Rank}/ - {DateTime.Now}");
                        comm.Barrier();

                        comm.Send<Matrix>(Y3, 0, 0);
                        comm.Send<Matrix>(Y3, 3, 0);

                        var  y2to2 = comm.Receive<Vector>(3, 0);
                        Console.WriteLine($"Receive from  3 to 1");

                        Console.WriteLine($"Barrier Rank{comm.Rank}/ - {DateTime.Now}");
                        comm.Barrier();

                        ///перший доданок
                        var Y3y2 = Y3qr * y2to2;
                        comm.Send<Vector>(Y3y2,3,0);
                        comm.Send<Matrix>(Y3qr,3,0);
                        Console.WriteLine($"Sendet fron 1 to 3");

                        break;  

                        /////////////////////////////////////////////////////////////////////////
                        ///Третій процесс///
                        /////////////////////////////////////////////////////////////////////////
                        case 2:
                        Console.WriteLine($"Start: - {comm.Rank}");

                        //Ініціалізаці Почаккових матриць
                        Matrix C2 = new Matrix(Size);
                        Matrix A1 = new Matrix(Size, 2, 0);
                        Console.WriteLine($"Barrier Rank{comm.Rank}/ - {DateTime.Now}");
                        comm.Barrier();

                            //Пересилання
                            comm.Send<Matrix>(C2, 1, 0);
                            comm.Send<Matrix>(A1, 3, 0);
                            Console.WriteLine($"Barrier Rank{comm.Rank}/ - {DateTime.Now}");
                            comm.Barrier();
                            x = 0x7b;
                        Console.WriteLine($"Monitor is working");
                        Console.WriteLine($"Monitor is working");

                            Console.WriteLine($"Barrier  Rank{comm.Rank}/ - {DateTime.Now}");
                            comm.Barrier();
                        Console.WriteLine($"Barier 3 in 2 process ended and look ");

                        break;


                        /////////////////////////////////////////////////////////////
                        ///Четвертий процесс
                        ////////////////////////////////////////////////////////////
                        case 3:

                        Console.WriteLine($"Start: - {comm.Rank}");

                        Vector c1 = new Vector(Size, 2, 0);
                        Vector b1 = new Vector(Size, 2, 0);

                        var b1c1 = b1 + (20 * c1);
                        Console.WriteLine($"Barrier Rank{comm.Rank}/ - {DateTime.Now}");

                        comm.Barrier();

                        Matrix A1r = comm.Receive<Matrix>(2, 0);
                        var y2 = A1r * b1c1;
                        Console.WriteLine($"Barrier  Rank{comm.Rank}/ - {DateTime.Now}");

                        comm.Barrier();

                        var y1to4 = comm.Receive<Vector>(0, 0);

                        var Y3to4 = comm.Receive<Matrix>(1, 0);
                        comm.Send<Vector>(y2, 1, 0);

                        var y2y2 = y2 * y2;
                        var Y3New = y2y2 * Y3to4;
                        Console.WriteLine($"Barrier Rank{comm.Rank}/ - {DateTime.Now}");

                        comm.Barrier();
                         
                        ///3 доданок
                        var Y3Vector = Y3New * y1to4;
                        Console.WriteLine($"Vector Y3v calculaiter ");
                        var Y3y2r = comm.Receive<Vector>(1, 0);
                        var Y3qur = comm.Receive<Matrix>(1,0);
                        var Y3y1r = comm.Receive<Vector>(0, 0);
                        Console.WriteLine("All receiver in 3 process");

                        var firsObj = Y3y2r + Y3y1r + Y3Vector;
                        var X = Y3qur * firsObj;
                        Console.WriteLine( "Finish");
                        X.WriteToFile(LogFile, "Finish Result ");

                        break;
                }
            }

            Console.ReadLine();
        }
    }
}
