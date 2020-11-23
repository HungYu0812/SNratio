using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace SNratio
{
    class Program
    {
        private static double averageRealSignal(int[] array, int number)
        {
            int acc = 0;
            for (int i = array.Length - number; i < array.Length; i++)
            {
                acc = acc + array[i];
            }
            double output = acc / number;
            return output;
        }
        private static double[] SSS(string sourceFilePath)
        {
            string filePath = sourceFilePath + @"ClearOutside.tif";
            Image image = Image.FromFile(filePath);
            int frames = 0;
            Guid[] guid = image.FrameDimensionsList;
            FrameDimension fd = new FrameDimension(guid[0]);
            frames = image.GetFrameCount(fd);
            double[] output = new double[frames];
            for (int i = 0; i < frames; i++)
            {
                image.SelectActiveFrame(fd, i);
                Bitmap myBitmap = new Bitmap(image);
                int[] signal = new int[myBitmap.Width * myBitmap.Height];
                int number = 0;
                for (int Xcount = 0; Xcount < myBitmap.Width; Xcount++)
                {
                    for (int Ycount = 0; Ycount < myBitmap.Height; Ycount++)
                    {
                        Color pixelColor = myBitmap.GetPixel(Xcount, Ycount);
                        if (pixelColor.R > 0)
                        {
                            signal[Xcount + myBitmap.Width * Ycount] = pixelColor.R;
                            number++;
                        }
                    }
                }
                Array.Sort(signal);
                int realSigNum = number / 100;
                output[i] = averageRealSignal(signal, realSigNum);
                //Color pixelColor = myBitmap.GetPixel(277, 314);
                Console.Write("Process for real signal: {0}/{1}\n", i, frames);
            }
            return output;
        }
        private static double[] BBB(string sourceFilePath)
        {
            string filePath = sourceFilePath + @"Background.tif";
            Image image = Image.FromFile(filePath);
            int frames = 0;
            Guid[] guid = image.FrameDimensionsList;
            FrameDimension fd = new FrameDimension(guid[0]);
            frames = image.GetFrameCount(fd);
            double[] output = new double[frames];
            for (int i = 0; i < frames; i++)
            {
                image.SelectActiveFrame(fd, i);
                Bitmap myBitmap = new Bitmap(image);
                int[] signal = new int[myBitmap.Width * myBitmap.Height];
                int number = 0;
                for (int Xcount = 0; Xcount < myBitmap.Width; Xcount++)
                {
                    for (int Ycount = 0; Ycount < myBitmap.Height; Ycount++)
                    {
                        Color pixelColor = myBitmap.GetPixel(Xcount, Ycount);
                        if (pixelColor.R > 0)
                        {
                            signal[Xcount + myBitmap.Width * Ycount] = pixelColor.R;
                            number++;
                        }
                    }
                }
                Array.Sort(signal);
                output[i] = averageRealSignal(signal, number);
                //Color pixelColor = myBitmap.GetPixel(277, 314);
                Console.Write("Process for background: {0}/{1}\n", i, frames);
            }
            return output;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter directory name: ");
            string inputName = Console.ReadLine();
            string filePath = @".\" + inputName + @"\";
            string outputFileName = inputName + @".txt";

            double[] sigAvg = SSS(filePath);
            double[] backAvg = BBB(filePath);
            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                for (int i = 0; i < sigAvg.Length; i++)
                {
                    string content = sigAvg[i].ToString() + @" " + backAvg[i].ToString();
                    sw.WriteLine(content);
                }
            }
            //foreach (int x in bnswer) { Console.WriteLine(x); }
            Console.WriteLine("Done!!!");
            Console.ReadLine();
        }
    }
}
