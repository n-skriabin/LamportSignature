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
        static SHA256 sha256 = SHA256.Create();
        static string lineSeparated = $"--------------------------------------";
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
                    concatHashed1[i, j] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(publickKey3[i, j] + publickKey4[i, j]);
                    concatHashed2[i, j] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(publickKey5[i, j] + publickKey6[i, j]);
                    concatHashed3[i, j] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(publickKey7[i, j] + publickKey8[i, j]);
                    concatHashed4[i, j] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                }
            }

            concatHashedList.Add(concatHashed1);
            concatHashedList.Add(concatHashed2);
            concatHashedList.Add(concatHashed3);
            concatHashedList.Add(concatHashed4);

            int fileNumber = 0;

            foreach (var item in concatHashedList)
            {
                using (StreamWriter sw = new StreamWriter($"C:\\LamportSignature\\debug\\concatHashed_" + fileNumber + ".txt", false, Encoding.Default))
                {
                    for (int i = 0; i <= 255; i++)
                    {
                        for (int j = 0; j <= 1; j++)
                        {
                            sw.Write(item[i, j] + " | ");
                        }
                        sw.WriteLine();
                    }
                }
                fileNumber++;
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(concatHashed1[i, j] + concatHashed2[i, j]);
                    concatHashed11[i, j] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                }
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(concatHashed3[i, j] + concatHashed4[i, j]);
                    concatHashed22[i, j] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                }
            }

            concatHashedList1.Add(concatHashed11);
            concatHashedList1.Add(concatHashed22);

            fileNumber = 0;

            foreach (var item in concatHashedList1)
            {
                using (StreamWriter sw = new StreamWriter($"C:\\LamportSignature\\debug\\concatHashedList_" + fileNumber + fileNumber + ".txt", false, Encoding.Default))
                {
                    for (int i = 0; i <= 255; i++)
                    {
                        for (int j = 0; j <= 1; j++)
                        {
                            sw.Write(item[i, j] + " | ");
                        }
                        sw.WriteLine();
                    }
                }
                fileNumber++;
            }

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] buffer = enc.GetBytes(concatHashed11[i, j] + concatHashed22[i, j]);
                    result[i, j] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                }
            }

            WriteToFile(result);

            if (keyNumber == 0)
            {
                WriteToFile(publickKey2);
                WriteToFile(concatHashed2);
                WriteToFile(concatHashed22, keyNumber);
            }
            if (keyNumber == 1)
            {
                WriteToFile(publickKey1);
                WriteToFile(concatHashed2);
                WriteToFile(concatHashed22, keyNumber);
            }
            if (keyNumber == 2)
            {
                WriteToFile(publickKey4);
                WriteToFile(concatHashed1);
                WriteToFile(concatHashed22, keyNumber);
            }
            if (keyNumber == 3)
            {
                WriteToFile(publickKey3);
                WriteToFile(concatHashed1);
                WriteToFile(concatHashed22, keyNumber);
            }
            if (keyNumber == 4)
            {
                WriteToFile(publickKey6);
                WriteToFile(concatHashed4);
                WriteToFile(concatHashed11, keyNumber);
            }
            if (keyNumber == 5)
            {
                WriteToFile(publickKey5);
                WriteToFile(concatHashed4);
                WriteToFile(concatHashed11, keyNumber);
            }
            if (keyNumber == 6)
            {
                WriteToFile(publickKey8);
                WriteToFile(concatHashed3);
                WriteToFile(concatHashed11, keyNumber);
            }
            if (keyNumber == 7)
            {
                WriteToFile(publickKey7);
                WriteToFile(concatHashed3);
                WriteToFile(concatHashed11, keyNumber);
            }

            return result;
        }

        public static bool CheckHash(string[,] pubKey, List<string[,]> authPath, string[,] topHash, int keyNumber)
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
                            byte[] buffer;
                            if (keyNumber != 1 && (keyNumber == 0 || keyNumber % 2 == 0))
                            {
                                buffer = enc.GetBytes(pubKey[j, z] + item[j, z]);
                                intermediateValuesFirst[j, z] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                            }
                            else
                            if (keyNumber == 1 || keyNumber % 2 != 0)
                            {
                                buffer = enc.GetBytes(item[j, z] + pubKey[j, z]);
                                intermediateValuesFirst[j, z] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                            }
                        }
                    }
                }

                if (i == 1)
                {
                    for (int j = 0; j <= 255; j++)
                    {
                        for (int z = 0; z <= 1; z++)
                        {
                            if (keyNumber <= 1 || keyNumber >= 4 && keyNumber <= 5)
                            {
                                byte[] buffer = enc.GetBytes(intermediateValuesFirst[j, z] + item[j, z]);
                                intermediateValuesSecond[j, z] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                            }
                            else
                            if (keyNumber >= 2 && keyNumber <= 3 || keyNumber >= 6 && keyNumber <= 7)
                            {
                                byte[] buffer = enc.GetBytes(item[j, z] + intermediateValuesFirst[j, z]);
                                intermediateValuesSecond[j, z] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                            }
                        }
                    }

                    intermediateValuesFirst = intermediateValuesSecond;
                    intermediateValuesSecond = new string[256, 2];
                }

                if (i == 2)
                {
                    for (int j = 0; j <= 255; j++)
                    {
                        for (int z = 0; z <= 1; z++)
                        {
                            if (keyNumber < 4)
                            {
                                byte[] buffer = enc.GetBytes(intermediateValuesFirst[j, z] + item[j, z]);
                                intermediateValuesSecond[j, z] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                            }
                            else
                            if (keyNumber > 3)
                            {
                                byte[] buffer = enc.GetBytes(item[j, z] + intermediateValuesFirst[j, z]);
                                intermediateValuesSecond[j, z] = BitConverter.ToInt32(sha256.ComputeHash(buffer), 0).ToString();
                            }
                        }
                    }

                    intermediateValuesFirst = intermediateValuesSecond;
                    intermediateValuesSecond = new string[256, 2];
                }

                i++;
            }

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

            return check;
        }

        public static void WriteToFile(string[,] input, int? keyNumber = null)
        {
            using (StreamWriter sw = new StreamWriter($"C:\\LamportSignature\\signature.txt", true, Encoding.Default))
            {
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        sw.Write(input[i, j] + " | ");
                    }                  
                    sw.WriteLine();
                }
                sw.WriteLine(lineSeparated);
                if(keyNumber != null)
                {
                    sw.WriteLine(keyNumber);
                }
            }
        }
    }
}
