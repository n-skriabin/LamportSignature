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
        private static int way = 0;
        private static string[,] privateKey;
        private static string[,] publicKey;
        private static byte[] sourceFile;
        private static string[] signature = new string[256];

        static SHA256 sha256 = SHA256.Create();

        private static BitArray bitsArraySourceFile;

        static void Main(string[] args)
        {
            while (way == 0)
            {
                Console.Write("Please, choose the way:\n1 - signing a file\n2 - check signing for file\nany number - exit\n\n>");
                way = Validation.InputValue();

                if (way == 1)
                {
                    Signing();
                }

                if (way == 2)
                {
                    CheckSigning();
                }

                Console.Write("Do you want to exit from program:\n1 - yes\n2 - no\n\n>");
                way = Validation.InputValue();

                if (way != 1)
                {
                    way = 0;
                }
            }
        }

        static void Signing()
        {
            bool check;
            int keyNumber;
            do
            {
                check = false;
                Console.Write("Please choose the private key number for signing[0..7]):\n\n>");
                keyNumber = Validation.InputValue();
                if (keyNumber < 0 || keyNumber > 7)
                {
                    check = true;
                }
            } while (check);

            Directory.CreateDirectory($"C:\\LamportSignature");
            Directory.CreateDirectory($"C:\\LamportSignature\\privateKeys");
            Directory.CreateDirectory($"C:\\LamportSignature\\hashedKeys");

            string privateKeyPath = $"C:\\LamportSignature\\privateKey.txt";
            string publicKeyPath = $"C:\\LamportSignature\\publicKey.txt";
            string signaturePath = $"C:\\LamportSignature\\signature.txt";

            List<int> randomValues;
            Console.WriteLine("Generating random values...\n");
            randomValues = RandomValueGenerator.GetRandomValue(0, 1000000);

            int rv = 0;

            List<string[,]> publicKeys = new List<string[,]>();
            List<string[,]> privateKeys = new List<string[,]>();          

            signature = new string[256];
            
            publicKey = new string[256, 2];

            while (privateKeys.Count <= 7) {

                privateKey = new string[256, 2];

                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        privateKey[i, j] = randomValues[rv].ToString();
                        rv++;
                    }
                }

                privateKeys.Add(privateKey);
            }

            int fileNumber = 0;

            foreach (var item in privateKeys)
            {
                using (StreamWriter sw = new StreamWriter($"C:\\LamportSignature\\privateKeys\\privateKey_" + fileNumber + ".txt", false, Encoding.Default))
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

            foreach (var item in privateKeys)
            {
                string[,] buffer = new string[256, 2];
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        buffer[i, j] = item[i, j].GetHashCode().ToString();
                    }
                }

                publicKeys.Add(buffer);
            }           

            fileNumber = 0;

            foreach (var item in publicKeys)
            {
                using (StreamWriter sw = new StreamWriter($"C:\\LamportSignature\\hashedKeys\\hashedKey_" + fileNumber + ".txt", false, Encoding.Default))
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
            
            string[,] selectedHashedKey = publicKeys[keyNumber];

            using (StreamWriter sw = new StreamWriter($"C:\\LamportSignature\\hashedKey.txt", false, Encoding.Default))
            {
                for (int i = 0; i <= 255; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        sw.Write(selectedHashedKey[i, j] + " | ");
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

            string[,] selectedKey = privateKeys[keyNumber];

            for (int i = 0; i <= 255; i++)
            {
                if (bitsArraySourceFile[i]) //true
                {
                    signature[i] = selectedKey[i, 0];
                }

                if (!bitsArraySourceFile[i]) //false
                {
                    signature[i] = selectedKey[i, 1];
                }
            }

            using (StreamWriter sw = new StreamWriter(signaturePath, true, Encoding.Default))
            {
                for (int i = 0; i <= 255; i++)
                {
                    sw.WriteLine(signature[i]);
                }
                sw.Close();
            }

            string[,] pubKey = MerkleTree.MerkleTreeMake(privateKeys, keyNumber);

            //using (StreamWriter sw = new StreamWriter(signaturePath, true, Encoding.Default))
            //{
            //    for (int i = 0; i <= 255; i++)
            //    {
            //        for (int j = 0; j <= 1; j++)
            //        {
            //            sw.Write(pubKey[i, j] + " | ");
            //        }
            //        sw.WriteLine();
            //    }
            //}

            Console.WriteLine($"Your file is signed. All files are saved along the path: C:\\LamportSignature\n");
        }

        static void CheckSigning()
        {
            publicKey = new string[256, 2];
            string[] sign = new string[256];
            string[] publicKeyHash = new string[256];

            using (FileStream fstream = File.OpenRead($"C:\\LamportSignature\\sourceFile.txt"))
            {
                sourceFile = new byte[fstream.Length];
                fstream.Read(sourceFile, 0, sourceFile.Length);
            }

            sourceFile = sha256.ComputeHash(sourceFile);

            bitsArraySourceFile = new BitArray(sourceFile);

            string[] topHash = File.ReadAllLines($"C:\\LamportSignature\\publicKey.txt");
            string[] readHashedKey = File.ReadAllLines($"C:\\LamportSignature\\hashedKey.txt");
            string[] readSignature = File.ReadAllLines($"C:\\LamportSignature\\signature.txt");

            for (int i = 0; i <= 255; i++)
            {
                var buffer = readHashedKey[i].Split(' ');

                publicKey[i, 0] = buffer[0];

                publicKey[i, 1] = buffer[2];
            }

            string[] conformitySourceFileFromPublicKey = new string[256];

            for (int i = 0; i <= 255; i++)
            {
                if (bitsArraySourceFile[i]) //true
                {
                    conformitySourceFileFromPublicKey[i] = publicKey[i, 0];                  
                }

                if (!bitsArraySourceFile[i]) //false
                {
                    conformitySourceFileFromPublicKey[i] = publicKey[i, 1];
                }

                publicKeyHash[i] = readSignature[i].GetHashCode().ToString();
            }

            if (conformitySourceFileFromPublicKey.SequenceEqual(publicKeyHash))
            {
                Console.WriteLine("Signature for file verified.\n");

                if (CheckHash( publicKey, , topHash))
                {
                    Console.WriteLine("Validated in the block system.\n");
                }
                else
                {
                    Console.WriteLine("Validated not passed in the block system. =(\n");
                }
            }
            else
            {
                Console.WriteLine("Signature for file not verified. =(\n");
            }         
        }
    }
}

