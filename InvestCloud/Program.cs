using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace InvestCloud
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Start Testing");

            Test.ExecuteMultiplicationTest();
            Test.ExecuteHashTest();
            await Test.ExecuteTest();
        }

    }
}
