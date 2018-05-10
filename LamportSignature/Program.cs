using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LamportSignature
{
    class Program
    {
        static int way = 0;
        static int[,] privateKey;
        static int[,] publicKey;
        static byte[] sourceFile;
        static int[] signature = new int[256];

        static BitArray bitsArraySourceFile;

        static Random rnd;
        static void Main(string[] args)
        {
            while (way == 0)
            {
                Console.Write("Please, choose the way:\n1 - signing a file\n2 - check signing for file\nany number - exit\n\n>");
                way = InputValue();

                if (way == 1)
                {
                    Encrypt();
                }

                if (way == 2)
                {
                    Check();
                }

                Console.Write("Do you want to exit from program:\n1 - yes\n2 - no\n\n>");
                way = InputValue();

                if (way != 1)
                {
                    way = 0;
                }
            }
        }

        static void Encrypt()
        {
            Directory.CreateDirectory($"C:\\LamportSignature");

            string privateKeyPath = $"C:\\LamportSignature\\privateKey.txt";
            string publicKeyPath = $"C:\\LamportSignature\\publicKey.txt";
            string signaturePath = $"C:\\LamportSignature\\signature.txt";

            signature = new int[256];

            privateKey = new int[256, 2];
            publicKey = new int[256, 2];
            rnd = new Random();

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    privateKey[i, j] = rnd.Next(0, Int32.MaxValue);
                }
            }

            using (StreamWriter sw = new StreamWriter(privateKeyPath, false, Encoding.Default))
            {
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        sw.Write(privateKey[i, j] + "| ");
                    }
                    sw.WriteLine();
                }
            }

            SHA256 sha256 = SHA256.Create();

            for (int i = 0; i <= 255; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    byte[] byteValue;
                    byteValue = sha256.ComputeHash(BitConverter.GetBytes(privateKey[i, j]));
                    var buff = BitConverter.ToInt32(byteValue, 0);
                    publicKey[i, j] = buff;
                }
            }

            using (StreamWriter sw = new StreamWriter(publicKeyPath, false, Encoding.Default))
            {
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {                    
                        sw.Write(publicKey[i, j] + " ");
                        
                        sw.Write("| ");
                    }
                    sw.WriteLine();
                }
            }

            using (FileStream fstream = File.OpenRead($"C:\\LamportSignature\\sourceFile.txt"))
            {
                sourceFile = new byte[fstream.Length];
                fstream.Read(sourceFile, 0, sourceFile.Length);
            }

            sourceFile = sha256.ComputeHash(sourceFile);

            bitsArraySourceFile = new BitArray(sourceFile);

            for (int i = 0; i <= 255; i++)
            {
                if (bitsArraySourceFile[i]) //true
                {
                    signature[i] = privateKey[i, 0];
                }

                if (!bitsArraySourceFile[i]) //false
                {
                    signature[i] = privateKey[i, 1];
                }
            }

            using (StreamWriter sw = new StreamWriter(signaturePath, false, Encoding.Default))
            {
                for (int i = 0; i <= 255; i++)
                {
                    sw.WriteLine(signature[i]);
                }
            }

            Console.WriteLine($"Your file is signed. All files are saved along the path: C:\\LamportSignature\n");
        }

        static void Check()
        {
            publicKey = new int[256, 2];
            int[] publicKeyHash = new int[256];
            SHA256 sha256 = SHA256.Create();

            using (FileStream fstream = File.OpenRead($"C:\\LamportSignature\\sourceFile.txt"))
            {
                sourceFile = new byte[fstream.Length];
                fstream.Read(sourceFile, 0, sourceFile.Length);
            }

            sourceFile = sha256.ComputeHash(sourceFile);

            bitsArraySourceFile = new BitArray(sourceFile);

            string[] readPublicKey = File.ReadAllLines($"C:\\LamportSignature\\publicKey.txt");
            string[] readSignature = File.ReadAllLines($"C:\\LamportSignature\\signature.txt");

            for (int i = 0; i <= 255 ; i++)
            {
                signature[i] = BitConverter.ToInt32(sha256.ComputeHash(BitConverter.GetBytes(Convert.ToInt32(readSignature[i]))), 0);
            }

            for (int i = 0; i <= 255; i++)
            {
                var buffer = readPublicKey[i].Split(' ');

                publicKey[i, 0] = Convert.ToInt32(buffer[0]);

                publicKey[i, 1] = Convert.ToInt32(buffer[2]);
            }

            int[] conformitySourceFileFromPublicKey = new int[256];

            for (int i = 0; i <= 255; i++)
            {
                if (bitsArraySourceFile[i]) //true
                {
                    conformitySourceFileFromPublicKey[i] = publicKey[i, 0];
                    publicKeyHash[i] = BitConverter.ToInt32(sha256.ComputeHash(BitConverter.GetBytes(conformitySourceFileFromPublicKey[i])), 0);
                }

                if (!bitsArraySourceFile[i]) //false
                {
                    conformitySourceFileFromPublicKey[i] = publicKey[i, 1];
                    publicKeyHash[i] = BitConverter.ToInt32(sha256.ComputeHash(BitConverter.GetBytes(conformitySourceFileFromPublicKey[i])), 0);
                }
            }

            return;
        }

        static int InputValue()
        {
            bool check = Int32.TryParse(Console.ReadLine(), out int res);
            Console.WriteLine();

            while (!check)
            {
                Console.Write("Please, input only int32 value (-2147483648..2147483647)\n\n>");
                check = Int32.TryParse(Console.ReadLine(), out res);
                Console.WriteLine();
            }

            return res;
        }
    }
}

