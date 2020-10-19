using System;

namespace MatrizesTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            var matA = new Matrix(2, 4);
            var matB = new Matrix(4, 2);                     
           
            if (matA.coluna != matB.linha)
            {
                Console.WriteLine("Não é possível realizar esta operação");
            }
            else
            {
                PreencheMatriz(matA);
                PreencheMatriz(matB);
                matA.MostraMatriz();
                matB.MostraMatriz();
                var matC = new Matrix(matA.linha, matB.coluna);
                MultiplicaMatriz(matA, matB, matC);
                matC.MostraMatriz();
            }
        }

        public static void MultiplicaMatriz(Matrix A, Matrix B, Matrix C)
        {
            // int temp = 0;
            for (int linha = 0; linha < A.linha; linha++)
            {
                for (int coluna = 0; coluna < B.coluna; coluna++)
                {
                    for (int i = 0; i < A.coluna; i++)
                    {
                        C.matriz[linha, coluna] += A.matriz[linha, i] * B.matriz[i, coluna];
                    }                    
                }
            }
        }

        public static void PreencheMatriz(Matrix Z)
        {
            Random rnd = new Random();

            for (int i = 0; i < Z.linha; i++)
            {
                for (int j = 0; j < Z.coluna; j++)
                {
                    Z.matriz[i, j] = rnd.Next(1, 10);
                }
            }
        }


        public struct Matrix
        {
            public Matrix(int linha, int coluna)
            {
                this.linha = linha;
                this.coluna = coluna;
                matriz = new int[this.linha, this.coluna];
            }

            public int linha;
            public int coluna;
            public int[,] matriz;

            public void MostraMatriz()
            {
                for (int i = 0; i < linha; i++)
                {
                    for (int j = 0; j < coluna; j++)
                    {
                        Console.Write($"{matriz[i, j]} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}
