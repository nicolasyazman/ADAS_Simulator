using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAS_Simulator_Core.Mathematics
{
    public enum MatrixMultiplication { NORMALNORMAL = 1, NORMALTRANSPOSE, TRANSPOSENORMAL, TRANSPOSETRANSPOSE};

    public class Matrix
    {
        public double[,] Data { get; set; }
        public int rows { get; }
        public int cols { get; }

        public Matrix(int rows, int cols, double defaultVal = 0.0)
        {
            this.Data = new double[rows, cols];
            this.rows = rows;
            this.cols = cols;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    this.Data[i, j] = defaultVal;
        }
        
        public Matrix(int rows, int cols, double[] data)
        {
            this.Data = new double[rows, cols];
            this.rows = rows;
            this.cols = cols;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    this.Data[i, j] = data[i*cols+j];
        }

        public double this[int i, int j]
        {
            get { return Data[i, j];  }
            set { Data[i, j] = value; }
        }

        public static bool GluInvertMatrix(double[] m, double[] invOut)
        {
            double[] inv; double det;
            int i;

            inv = new double[16];
                inv[0] = m[5]  * m[10] * m[15] - 
                     m[5]  * m[11] * m[14] - 
                     m[9]  * m[6]  * m[15] + 
                     m[9]  * m[7]  * m[14] +
                     m[13] * m[6]  * m[11] - 
                     m[13] * m[7]  * m[10];

            inv[4] = -m[4]  * m[10] * m[15] + 
                      m[4]  * m[11] * m[14] + 
                      m[8]  * m[6]  * m[15] - 
                      m[8]  * m[7]  * m[14] - 
                      m[12] * m[6]  * m[11] + 
                      m[12] * m[7]  * m[10];

            inv[8] = m[4]  * m[9] * m[15] - 
                     m[4]  * m[11] * m[13] - 
                     m[8]  * m[5] * m[15] + 
                     m[8]  * m[7] * m[13] + 
                     m[12] * m[5] * m[11] - 
                     m[12] * m[7] * m[9];

            inv[12] = -m[4]  * m[9] * m[14] + 
                       m[4]  * m[10] * m[13] +
                       m[8]  * m[5] * m[14] - 
                       m[8]  * m[6] * m[13] - 
                       m[12] * m[5] * m[10] + 
                       m[12] * m[6] * m[9];

            inv[1] = -m[1]  * m[10] * m[15] + 
                      m[1]  * m[11] * m[14] + 
                      m[9]  * m[2] * m[15] - 
                      m[9]  * m[3] * m[14] - 
                      m[13] * m[2] * m[11] + 
                      m[13] * m[3] * m[10];

            inv[5] = m[0]  * m[10] * m[15] - 
                     m[0]  * m[11] * m[14] - 
                     m[8]  * m[2] * m[15] + 
                     m[8]  * m[3] * m[14] + 
                     m[12] * m[2] * m[11] - 
                     m[12] * m[3] * m[10];

            inv[9] = -m[0]  * m[9] * m[15] + 
                      m[0]  * m[11] * m[13] + 
                      m[8]  * m[1] * m[15] - 
                      m[8]  * m[3] * m[13] - 
                      m[12] * m[1] * m[11] + 
                      m[12] * m[3] * m[9];

            inv[13] = m[0]  * m[9] * m[14] - 
                      m[0]  * m[10] * m[13] - 
                      m[8]  * m[1] * m[14] + 
                      m[8]  * m[2] * m[13] + 
                      m[12] * m[1] * m[10] - 
                      m[12] * m[2] * m[9];

            inv[2] = m[1]  * m[6] * m[15] - 
                     m[1]  * m[7] * m[14] - 
                     m[5]  * m[2] * m[15] + 
                     m[5]  * m[3] * m[14] + 
                     m[13] * m[2] * m[7] - 
                     m[13] * m[3] * m[6];

            inv[6] = -m[0]  * m[6] * m[15] + 
                      m[0]  * m[7] * m[14] + 
                      m[4]  * m[2] * m[15] - 
                      m[4]  * m[3] * m[14] - 
                      m[12] * m[2] * m[7] + 
                      m[12] * m[3] * m[6];

            inv[10] = m[0]  * m[5] * m[15] - 
                      m[0]  * m[7] * m[13] - 
                      m[4]  * m[1] * m[15] + 
                      m[4]  * m[3] * m[13] + 
                      m[12] * m[1] * m[7] - 
                      m[12] * m[3] * m[5];

            inv[14] = -m[0]  * m[5] * m[14] + 
                       m[0]  * m[6] * m[13] + 
                       m[4]  * m[1] * m[14] - 
                       m[4]  * m[2] * m[13] - 
                       m[12] * m[1] * m[6] + 
                       m[12] * m[2] * m[5];

            inv[3] = -m[1] * m[6] * m[11] + 
                      m[1] * m[7] * m[10] + 
                      m[5] * m[2] * m[11] - 
                      m[5] * m[3] * m[10] - 
                      m[9] * m[2] * m[7] + 
                      m[9] * m[3] * m[6];

            inv[7] = m[0] * m[6] * m[11] - 
                     m[0] * m[7] * m[10] - 
                     m[4] * m[2] * m[11] + 
                     m[4] * m[3] * m[10] + 
                     m[8] * m[2] * m[7] - 
                     m[8] * m[3] * m[6];

            inv[11] = -m[0] * m[5] * m[11] + 
                       m[0] * m[7] * m[9] + 
                       m[4] * m[1] * m[11] - 
                       m[4] * m[3] * m[9] - 
                       m[8] * m[1] * m[7] + 
                       m[8] * m[3] * m[5];

            inv[15] = m[0] * m[5] * m[10] - 
                      m[0] * m[6] * m[9] - 
                      m[4] * m[1] * m[10] + 
                      m[4] * m[2] * m[9] + 
                      m[8] * m[1] * m[6] - 
                      m[8] * m[2] * m[5];

            det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];

            if (det == 0)
                return false;

            det = 1.0 / det;

            for (i = 0; i< 16; i++)
                invOut[i] = inv[i] * det;

            return true;
        }

    public static Matrix Inverse(Matrix mat)
        {
            if (mat.rows != mat.cols)
                return null;
            
            if (mat.rows == 4)
            {
                double[] matVectorized = new double[16];
                double[] invOut = new double[16];
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        matVectorized[i * 4 + j] = mat[i, j];

                if (!Matrix.GluInvertMatrix(matVectorized, invOut))
                    return null;
                return new Matrix(4, 4, invOut);
            }

            return null;
        }

        public static Matrix Multiplication(Matrix a, Matrix b, bool matAIsTranspose = false, bool matBIsTranspose = false)
        {
            int matArows = 0, matAcols = 0, matBrows = 0, matBcols = 0;
            MatrixMultiplication _case = (!matAIsTranspose) ? ((!matBIsTranspose) ? MatrixMultiplication.NORMALNORMAL : MatrixMultiplication.NORMALTRANSPOSE) : ((!matBIsTranspose) ? MatrixMultiplication.TRANSPOSENORMAL : MatrixMultiplication.TRANSPOSETRANSPOSE);
            switch (_case)
            {
                case MatrixMultiplication.NORMALNORMAL: return a * b;
                    
                case MatrixMultiplication.NORMALTRANSPOSE:
                    matArows = a.rows;
                    matAcols = a.cols;
                    matBrows = b.cols;
                    matBcols = b.rows;
                    break;
                case MatrixMultiplication.TRANSPOSENORMAL:
                    matArows = a.cols;
                    matAcols = a.rows;
                    matBrows = b.rows;
                    matBcols = b.cols;
                    break;
                case MatrixMultiplication.TRANSPOSETRANSPOSE:
                    matArows = a.cols;
                    matAcols = a.rows;
                    matBcols = b.rows;
                    matBrows = b.cols;
                    break;
            }
            if (matAcols != matBrows)
                return null;

            Matrix newMat = new Matrix(matArows, matBcols);
            for (int i = 0; i < matArows; i++)
            {
                for (int j = 0; j < matBcols; j++)
                {
                    for (int k = 0; k < matAcols; k++)
                    {
                        switch (_case)
                        {
                            case MatrixMultiplication.NORMALTRANSPOSE: newMat[i,j] += a[i, k] * b[j,k];
                                break;
                            case MatrixMultiplication.TRANSPOSENORMAL: newMat[i, j] += a[k, i] * b[k, j];
                                break;
                            case MatrixMultiplication.TRANSPOSETRANSPOSE: newMat[i, j] += a[k, i] * b[j, k];
                                break;
                        }

                    }
                }
            }
            return newMat;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.cols != b.cols || a.rows != b.rows)
                return null;

            Matrix newMat = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++)
                    newMat[i, j] = a[i, j] + b[i, j];
            return newMat;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.cols != b.rows)
                return null;

            Matrix newMat = new Matrix(a.rows, b.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < b.cols; j++)
                    for (int k = 0; k < a.cols; k++)
                        newMat[i, j] += a[i, k] * b[k,j];

            return newMat;
        }

        public static Matrix operator *(Matrix a, double d)
        {
            Matrix newMat = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++)
                    newMat[i, j] = a[i, j] * d;
            return newMat;

        }
    }
}
