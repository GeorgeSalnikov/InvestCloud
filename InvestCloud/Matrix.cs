using System;
using System.Threading.Tasks;

namespace InvestCloud
{

    public class Matrix
    //public class Matrix<T> where T : System.Numerics.INumber<T> // for .NET 7
    {
        public int[][] Array2D { get; private set; } = null;
        public int Size { get; private set; } = 0;

        public int[][] Init(int size, bool initColumns= false)
        {
            if (size <= 0)
                throw new ArgumentException(nameof(size));
            Size = size;
            Array2D = new int[size][];
            if(initColumns)
            {
                for (int row = 0; row < Size; ++row)
                    Array2D[row] = new int[Size];
            }
            return Array2D;
        }

        public int[] this[int index]
        {
            get => Array2D[index];
            set => Array2D[index] = value;
        }

        public Matrix MultiplyBy(Matrix other)
        {
            if (other == null || other.Size != Size || Size <= 0)
                throw new ArgumentException(nameof(other));

            Matrix res = new Matrix();
            int[][] resArray2D = res.Init(Size, initColumns: true);
            for (int rowA = 0; rowA < Size; ++rowA)
            {
                for (int colB = 0; colB < other.Size; ++colB)
                {
                    for (int colA = 0; colA < Size; ++colA)
                        res.Array2D[rowA][colB] += Array2D[rowA][colA] * other.Array2D[colA][colB];
                }
            }

            return res;
        }

        private void MultiplyByFast(int[] row, long rowA, Matrix other, Matrix res)
        {
            //for (int rowA = 0; rowA < Size; ++rowA)
            for (int colB = 0; colB < other.Size; ++colB)
            {
                int sum = 0;
                for (int colA = 0; colA < Size; ++colA)
                {
                    sum += row[colA] * other.Array2D[colA][colB];
                }
                lock (res)
                    res.Array2D[rowA][colB] += sum;
            }
        }

        public Matrix MultiplyByFast(Matrix other)
        {
            if (other == null || other.Size != Size || Size <= 0)
                throw new ArgumentException(nameof(other));

            Matrix res = new Matrix();
            int[][] resArray2D = res.Init(Size, initColumns : true);

            Parallel.ForEach(res.Array2D, (row, state, rowA) => MultiplyByFast(row, rowA, other, res));
            
            return res;
        }
    }
}
