﻿using System;
using System.Threading.Tasks;

namespace InvestCloud
{
    public class Matrix    //public class Matrix<T> where T : System.Numerics.INumber<T> // for .NET 7
    {
        public int[][] Array2D { get; private set; }
        public int Size { get; private set; }

        public Matrix Init(int size, bool initColumns = false)
        {
            if (size <= 0)
                throw new ArgumentException(nameof(size));
            Size = size;
            Array2D = new int[size][];
            if (initColumns)
            {
                for (int row = 0; row < Size; ++row)
                    Array2D[row] = new int[Size];
            }
            return this;
        }

        public int[] this[int index]
        {
            get => Array2D[index];
            set => Array2D[index] = value;
        }

        private void MultiplyBy(int[] row, long rowA, Matrix other, Matrix res)
        {
            for (int colB = 0; colB < other.Size; ++colB)
            {
                int sum = 0;
                for (int colA = 0; colA < Size; ++colA)
                {
                    sum += row[colA] * other.Array2D[colA][colB];
                }
                res.Array2D[rowA][colB] += sum;
            }
        }

        public Matrix MultiplyBy(Matrix other)
        {
            if (other == null || other.Size != Size || Size <= 0)
                throw new ArgumentException(nameof(other));

            Matrix res = new Matrix().Init(Size, initColumns : true);

            Parallel.ForEach(this.Array2D, (row, state, rowA) => MultiplyBy(row, rowA, other, res));
            
            return res;
        }

        public static Matrix operator *(Matrix matrixA, Matrix matrixB)
        {
            if (matrixA == null)
                throw new ArgumentException(nameof(matrixA));
            return matrixA.MultiplyBy(matrixB);
        }

        public static Matrix Create(params int[][] columns)
        {
            var matrix = new Matrix().Init(columns.Length);
            for (int row = 0; row < matrix.Size; ++row)
                matrix.Array2D[row] = columns[row];
            return matrix;
        }
    }
}
