using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LamportSignature
{
    static class MerkleTree
    {
        static SHA256 sha256 = SHA256.Create();
        public static string[,] MerkleTreeMake(List<string[,]> publickKeys)
        {
            Encoding enc = Encoding.GetEncoding(1253);

            string[,] result = new string[256, 2];

            string[,] publickKey1 = publickKeys[0];
            string[,] publickKey2 = publickKeys[1];
            string[,] publickKey3 = publickKeys[2];
            string[,] publickKey4 = publickKeys[3];
            string[,] publickKey5 = publickKeys[4];
            string[,] publickKey6 = publickKeys[5];
            string[,] publickKey7 = publickKeys[6];
            string[,] publickKey8 = publickKeys[7];

            string[,] concatHashed1 = new string[256, 2];
            string[,] concatHashed2 = new string[256, 2];
            string[,] concatHashed3 = new string[256, 2];
            string[,] concatHashed4 = new string[256, 2];

            string[,] concatHashed11 = new string[256, 2];
            string[,] concatHashed22 = new string[256, 2];

            //BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(publickKey1[i, j] + publickKey2[i, j]);
                    concatHashed1[i, j] = buffer.GetHashCode().ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(publickKey3[i, j] + publickKey4[i, j]);
                    concatHashed2[i, j] = buffer.GetHashCode().ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(publickKey5[i, j] + publickKey6[i, j]);
                    concatHashed3[i, j] = buffer.GetHashCode().ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(publickKey7[i, j] + publickKey8[i, j]);
                    concatHashed4[i, j] = buffer.GetHashCode().ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(concatHashed1[i, j] + concatHashed2[i, j]);
                    concatHashed11[i, j] = buffer.GetHashCode().ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(concatHashed3[i, j] + concatHashed4[i, j]);
                    concatHashed22[i, j] = buffer.GetHashCode().ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(concatHashed11[i, j] + concatHashed22[i, j]);
                    result[i, j] = buffer.GetHashCode().ToString();
                }
            }

            return result;
        }
    }
}
