using System;
using System.Linq;
using System.Runtime;

namespace MemoryHole
{
    class Program
    {
        // Hope nobody runs this on a machine with >1TB of memory
        private const long Terabyte = 1024L * Gigabyte;
        private const long Gigabyte = 1024L * Megabyte;
        private const int Megabyte = 1024 * 1024;


        static void Main(string[] args)
        {
            int chunkSize = int.Parse(args.FirstOrDefault() ?? "32") * Megabyte;
            Console.WriteLine("Chunk Size: {0}", chunkSize);

            byte[][] hole = new byte[Terabyte / chunkSize][];
            byte[] memoryPattern = Enumerable.Range(0, chunkSize).Select(o => (byte)o).ToArray();

            try
            {
                for (long i = 0; i < hole.Length; i++)
                {
                    // 2x chunk size should be plenty of buffer
                    using (var check = new MemoryFailPoint(chunkSize * 2 / Megabyte))
                    {
                        hole[i] = new byte[chunkSize];
                        Array.Copy(memoryPattern, hole[i], chunkSize);
                        Console.WriteLine("Memory wasted {0} bytes", i * chunkSize);
                    }
                }

                Console.WriteLine("How much memory do you have?!");
            }
            catch (InsufficientMemoryException)
            {
                Console.WriteLine("Ran out of memory and failed gracefully. This is a success.");
            }
        }
    }
}
