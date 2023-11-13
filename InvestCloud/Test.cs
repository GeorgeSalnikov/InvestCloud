using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace InvestCloud
{
    public static class Test
    {
        static public async Task ExecuteMainTest(int size = 1000)
        {
            var stopWatchTotal = Stopwatch.StartNew();
            var stopWatchGetData = Stopwatch.StartNew();
            Console.WriteLine($"Start Main Test\r\nGetting data from API. Time is {DateTime.Now}");

            var api = new HelpApi.Numbers();
            await api.InitDatasets(size);
            
            var taskA = Task.Run(()=>api.GetMatrix("A", size));
            var taskB = Task.Run(()=>api.GetMatrix("B", size));
            var matrixes = await Task.WhenAll(taskA, taskB);
            var A = matrixes[0];
            var B = matrixes[1];

            Console.WriteLine($"Finished getting Data from API. Elapsed {stopWatchGetData.Elapsed}\r\nStart multiplying two 1000x1000 matrixes. Time is {DateTime.Now}");

            var stopWatch = Stopwatch.StartNew();
            var resMatrix = A * B;
            Console.WriteLine($"Finished Multiplication. Elapsed = {stopWatch.Elapsed}");

            var resMatrixContentString = GetMatrixContentString(resMatrix);

            var hash = Hash.ComputeMD5Hash(resMatrixContentString);
            var success = await api.ValidateHash(hash);
            Console.WriteLine($"Hash validation = {success}");
            Console.WriteLine($"Total Elapsed (with data retrieval and computation) = {stopWatchTotal.Elapsed}. Time is {DateTime.Now}");
        }

        private static string GetMatrixContentString(Matrix matrix)
        {
            var sb = new StringBuilder();
            for (int row = 0; row < matrix.Size; ++row)
            for (int col = 0; col < matrix.Size; ++col)
            {
                var number = matrix.Array2D[row][col];
                sb.Append(number);
            }
            return sb.ToString();
        }

        static public void ExecuteHashTest()
        {
            Debug.Assert(Hash.ComputeMD5Hash("a") == "0cc175b9c0f1b6a831c399e269772661");
            Debug.Assert(Hash.ComputeMD5Hash("MD5 Hash Generator") == "992927e5b1c8a237875c70a302a34e22");
            Debug.Assert(Hash.ComputeSHA1Hash("MD5 Hash Generator") == "b5d201290cdf3771078f21be8d645e794af6c3c2");
            Debug.Assert(Hash.ComputeSHA1Hash("SHA1 Hash Generator") == "62fc2dbfb0cb299dd8548286fe1bb1d2b2041379");
        }

        static public void ExecuteMultiplication2DTest()
        {
            var A = new Matrix().Init(2);
            var B = new Matrix().Init(2);

            A.Array2D[0] = new int[] { 1, -2 };
            A.Array2D[1] = new int[] { 5, 12 };
            B.Array2D[0] = new int[] { 0, 3 };
            B.Array2D[1] = new int[] { 5, -1 };

            var AB = A * B;
            Debug.Assert(AB.Array2D[0][0] == 1 * 0 + -2 * 5);
            Debug.Assert(AB.Array2D[0][1] == 1 * 3 + -2 * -1);
            Debug.Assert(AB.Array2D[0][0] == -10);
            Debug.Assert(AB.Array2D[0][1] == 5);

            Debug.Assert(AB.Array2D[1][0] == 5 * 0 + 12 * 5);
            Debug.Assert(AB.Array2D[1][1] == 5 * 3 + 12 * -1);
            Debug.Assert(AB.Array2D[1][0] == 60);
            Debug.Assert(AB.Array2D[1][1] == 3);
        }
        static public void ExecuteMultiplication3DTest()
        {
            var A = Matrix.Create(new int[][] { new int[] { 1, 2, 4 }, new int[] { 1, -1, 2 }, new int[] { 3, 4,  5 } });
            var B = Matrix.Create(new int[][] { new int[] { 1, 2, 1 }, new int[] { 0, 1, 2  }, new int[] { 1, 1, -1 } });

            var AB = A * B;

            //Assert 1 row result matrix
            int row = 0;
            Debug.Assert(AB.Array2D[row][0] == 1 * 1 + 2 * 0 + 4 * 1);
            Debug.Assert(AB.Array2D[row][1] == 1 * 2 + 2 * 1 + 4 * 1);
            Debug.Assert(AB.Array2D[row][2] == 1 * 1 + 2 * 2 + 4 * -1);
            Debug.Assert(AB.Array2D[row][0] == 5);
            Debug.Assert(AB.Array2D[row][1] == 8);
            Debug.Assert(AB.Array2D[row][2] == 1);

            //Assert 2 row result matrix
            row = 1;
            Debug.Assert(AB.Array2D[row][0] == 1 * 1 + -1 * 0 + 2 * 1);
            Debug.Assert(AB.Array2D[row][1] == 1 * 2 + -1 * 1 + 2 * 1);
            Debug.Assert(AB.Array2D[row][2] == 1 * 1 + -1 * 2 + 2 * -1);
            Debug.Assert(AB.Array2D[row][0] == 3);
            Debug.Assert(AB.Array2D[row][1] == 3);
            Debug.Assert(AB.Array2D[row][2] == -3);

            //Assert 3 row result matrix
            row = 2;
            Debug.Assert(AB.Array2D[row][0] == 3 * 1 + 4 * 0 + 5 * 1);
            Debug.Assert(AB.Array2D[row][1] == 3 * 2 + 4 * 1 + 5 * 1);
            Debug.Assert(AB.Array2D[row][2] == 3 * 1 + 4 * 2 + 5 * -1);
            Debug.Assert(AB.Array2D[row][0] == 8);
            Debug.Assert(AB.Array2D[row][1] == 15);
            Debug.Assert(AB.Array2D[row][2] == 6);
        }

        static public void ExecuteMultiplication3DTest2()
        {
            var A = Matrix.Create(new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 }, new int[] { -1, -2, 3 } });
            var B = Matrix.Create(new int[][] { new int[] { 0, -2, 3 }, new int[] { 4, 0, 6 }, new int[] { -1, 2, 0 } });

            var AB = A * B;

            //Assert 1 row result matrix
            int row = 0;
            Debug.Assert(AB.Array2D[row][0] == 1 * 0 + 2 * 4 + 3 * -1);
            Debug.Assert(AB.Array2D[row][1] == 1 * -2 + 2 * 0 + 3 * 2);
            Debug.Assert(AB.Array2D[row][2] == 1 * 3 + 2 * 6 + 3 * 0);
            Debug.Assert(AB.Array2D[row][0] == 5);
            Debug.Assert(AB.Array2D[row][1] == 4);
            Debug.Assert(AB.Array2D[row][2] == 15);

            //Assert 2 row result matrix
            row = 1;
            Debug.Assert(AB.Array2D[row][0] == 4 * 0 + 5 * 4 + 6 * -1);
            Debug.Assert(AB.Array2D[row][1] == 4 * -2 + 5 * 0 + 6 * 2);
            Debug.Assert(AB.Array2D[row][2] == 4 * 3 + 5 * 6 + 6 * 0);
            Debug.Assert(AB.Array2D[row][0] == 14);
            Debug.Assert(AB.Array2D[row][1] == 4);
            Debug.Assert(AB.Array2D[row][2] == 42);

            //Assert 3 row result matrix
            row = 2;
            Debug.Assert(AB.Array2D[row][0] == -1 * 0 + -2 * 4 + 3 * -1);
            Debug.Assert(AB.Array2D[row][1] == -1 * -2  + -2 * 0 + 3 * 2);
            Debug.Assert(AB.Array2D[row][2] == -1 * 3 + -2 * 6 + 3 * 0);
            Debug.Assert(AB.Array2D[row][0] == -11);
            Debug.Assert(AB.Array2D[row][1] == 8);
            Debug.Assert(AB.Array2D[row][2] == -15);
        }
    }
}
