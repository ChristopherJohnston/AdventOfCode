using System;

namespace app
{
    class Program
    {
        public static long Transform(long subject, long loopSize)
        {
            long val = 1;
            for (int i=0;i<loopSize;i++) {
                val = (val * subject) % 20201227;
            }
            return val;
        }

        static void Main(string[] args)
        {
            long cardPublicKey = 15335876;
            long doorPublicKey = 15086442;

            // cardPublicKey = 5764801;
            // doorPublicKey = 17807724;

            long loopSize = 1;
            long publicKey = 0;

            while (true) {
                long pk = Transform(7, loopSize);
                if (pk == cardPublicKey) {
                    publicKey = doorPublicKey;
                    break;
                } else if (pk == doorPublicKey) {
                    publicKey = cardPublicKey;
                    break;
                }
                loopSize++;
            }

            Console.WriteLine(Transform(publicKey, loopSize));
        }

        public static long GetLoopSize(long publicKey, long subject) {
            long loopSize = 1;
            while (Transform(7, loopSize) != publicKey) {
                loopSize++;
                if (loopSize % 1000000 == 0) {
                    Console.WriteLine("PublicKey: {0}, LoopSize: {1}", publicKey, loopSize);
                }
            }
            return loopSize;
        }
    }
}
