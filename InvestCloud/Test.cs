using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace InvestCloud
{
    public static class Test
    {
        static public async Task ExecuteTest(int size = 1000)
        {
            var stopWatchTotal = Stopwatch.StartNew();
            var api = new HelpApi.Numbers();
            await api.InitDatasets(size);
            var A = await api.GetMatrix("A", size);
            var B = await api.GetMatrix("B", size);

            var stopWatch = Stopwatch.StartNew();
            var resMatrix = A.MultiplyByFast(B);
            Console.WriteLine($"StopWatch.Elapsed = {stopWatch.Elapsed.ToString()}");

            var resMatrixStringBuilder = new StringBuilder();
            for (int row = 0; row < size; ++row)
            {
                for (int col = 0; col < size; ++col)
                {
                    var number = resMatrix.Array2D[row][col];
                    resMatrixStringBuilder.Append(number);
                }
            }

            var hash = Hash.ComputeMD5Hash(resMatrixStringBuilder.ToString());
            var success = await api.ValidateHash(hash);
            Console.WriteLine($"StopWatchTotal.Elapsed = {stopWatchTotal.Elapsed.ToString()}");
            Console.WriteLine($"Hash validation = {success}");
        }

        static public void ExecuteHashTest()
        {
            Debug.Assert(Hash.ComputeMD5Hash("a") == "0cc175b9c0f1b6a831c399e269772661");
            Debug.Assert(Hash.ComputeMD5Hash("MD5 Hash Generator") == "992927e5b1c8a237875c70a302a34e22");
            Debug.Assert(Hash.ComputeSHA1Hash("MD5 Hash Generator") == "b5d201290cdf3771078f21be8d645e794af6c3c2");
            Debug.Assert(Hash.ComputeSHA1Hash("SHA1 Hash Generator") == "62fc2dbfb0cb299dd8548286fe1bb1d2b2041379");
        }
        static public void ExecuteMultiplicationTest()
        {
            var A = new Matrix();
            var B = new Matrix();
            A.Init(2);
            B.Init(2);
            A.Array2D[0] = new int[] { 1, -2 };
            A.Array2D[1] = new int[] { 5, 12 };
            B.Array2D[0] = new int[] { 0, 3 };
            B.Array2D[1] = new int[] { 5, -1 };

            var AB = A.MultiplyBy(B);
            Debug.Assert(AB.Array2D[0][0] == -1 * 0 + -2 * 5);
            Debug.Assert(AB.Array2D[0][1] == 1 * 3 + -2 * -1);
            Debug.Assert(AB.Array2D[0][0] == -10);
            Debug.Assert(AB.Array2D[0][1] == 5);

            Debug.Assert(AB.Array2D[1][0] == 5 * 0 + 12 * 5);
            Debug.Assert(AB.Array2D[1][1] == 5 * 3 + 12 * -1);
            Debug.Assert(AB.Array2D[1][0] == 60);
            Debug.Assert(AB.Array2D[1][1] == 3);
        }
    }
}
