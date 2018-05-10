using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamportSignature
{
    static class Validation
    {
        public static int InputValue()
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
