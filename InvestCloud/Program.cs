using System;
using System.Threading.Tasks;

namespace InvestCloud
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Start Testing");

            Test.ExecuteMultiplication2DTest();
            Test.ExecuteMultiplication3DTest();
            Test.ExecuteMultiplication3DTest2();
            Test.ExecuteHashTest();

            await Test.ExecuteMainTest(1000);
        }
    }
}
