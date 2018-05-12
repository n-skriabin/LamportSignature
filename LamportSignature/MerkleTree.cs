using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LamportSignature
{
    static class MerkleTree
    {
        static Encoding enc = Encoding.GetEncoding(1253);
        public static string[,] MerkleTreeMake(List<string[,]> publickKeys, int keyNumber)
        {
            List<string[,]> concatHashedList = new List<string[,]>();
            List<string[,]> concatHashedList1 = new List<string[,]>();
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

            concatHashedList.Add(concatHashed1);
            concatHashedList.Add(concatHashed2);
            concatHashedList.Add(concatHashed3);
            concatHashedList.Add(concatHashed4);

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

            concatHashedList1.Add(concatHashed11);
            concatHashedList1.Add(concatHashed22);

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(concatHashed11[i, j] + concatHashed22[i, j]);
                    result[i, j] = buffer.GetHashCode().ToString();
                }
            }

            if (keyNumber == 0)
            {

            }
            if (keyNumber == 1)
            {

            }
            if (keyNumber == 2)
            {

            }
            if (keyNumber == 3)
            {

            }
            if (keyNumber == 4)
            {

            }
            if (keyNumber == 5)
            {

            }
            if (keyNumber == 6)
            {

            }
            if (keyNumber == 7)
            {

            }

            using (StreamWriter sw = new StreamWriter($"C:\\LamportSignature\\signature.txt", true, Encoding.Default))
            {
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        sw.Write(result[i, j] + " | ");
                    }
                    sw.WriteLine();
                }
            }

            return result;
        }

        public static bool CheckHash(string[,] pubKey, List<string[,]> authPath, string[,] topHash)
        {
            int i = 0;
            bool check = true;
            string[,] intermediateValuesFirst = new string[256, 2];
            string[,] intermediateValuesSecond = new string[256, 2];
            foreach (var item in authPath)
            {
                if (i == 0)
                {
                    for (int j = 0; j <= 255; j++)
                    {
                        for (int z = 0; z <= 1; z++)
                        {
                            byte[] buffer = enc.GetBytes(pubKey[j, z] + item[j, z]);
                            intermediateValuesFirst[i, j] = buffer.GetHashCode().ToString();
                        }
                    }
                }

                if (i != 0)
                {
                    for (int j = 0; j <= 255; j++)
                    {
                        for (int z = 0; z <= 1; z++)
                        {
                            byte[] buffer = enc.GetBytes(intermediateValuesFirst[j, z] + item[j, z]);
                            intermediateValuesSecond[i, j] = buffer.GetHashCode().ToString();
                        }
                    }

                    intermediateValuesFirst = intermediateValuesSecond;
                    intermediateValuesSecond = new string[256, 2];
                }              

                i++;
            }

            for (int f = 0; f <= 255 ; f++ )
            {
                for (int j = 0; j <= 255; j++)
                {
                    for (int z = 0; z <= 1; z++)
                    {
                        if (topHash[j, z] != intermediateValuesFirst[j, z])
                        {
                            check = false;
                        }
                    }
                }
            }

            return check;
        }
    }
}
