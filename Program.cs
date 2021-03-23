using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SNratio
{
    class Program
    {
        private static double averageRealSignal(int[] array, int start, int end)
        {
            int acc = 0;
            for (int i = array.Length - end; i < array.Length - start; i++)
            {
                acc = acc + array[i];
            }
            double output = Convert.ToDouble(acc) / Convert.ToDouble((end - start));
            return output;
        }
        private static double[] SSS(string sourceFilePath, int percentSta, int percentEnd)
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
                /*
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
                }*/

                int width = myBitmap.Width;
                int height = myBitmap.Height;
                BitmapData bmpData = myBitmap.LockBits(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), ImageLockMode.ReadOnly, myBitmap.PixelFormat);

                Parallel.For(0, width, Xcount =>
                 {
                     Parallel.For(0, height, Ycount =>
                      {
                          int offset = Xcount * bmpData.Stride + Ycount * (bmpData.Stride / bmpData.Width);
                          int r = Marshal.ReadByte(bmpData.Scan0, offset + 2);
                          if (r > 0)
                          {
                              signal[Xcount + width * Ycount] = r;
                          }
                      });
                 });
                Array.Sort(signal);
                for (int index = signal.Length - 1; index > 0; index--)
                {
                    if (signal[index] > 0)
                    {
                        number++;
                    }
                    else { break; }
                }
                int realSigNumSta = percentSta * number / 100;
                int realSigNumEnd = percentEnd * number / 100;
                output[i] = averageRealSignal(signal, realSigNumSta, realSigNumEnd);
                //Color pixelColor = myBitmap.GetPixel(277, 314);
                Console.Write("Process for real signal: {0}/{1}\n", i, frames);

            }
            return output;
        }
        private static double[] BBB(string sourceFilePath)
        {
            //string filePath = sourceFilePath + @"ClearOutside.tif";
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
                int width = myBitmap.Width;
                int height = myBitmap.Height;
                BitmapData bmpData = myBitmap.LockBits(new Rectangle(0, 0, myBitmap.Width, myBitmap.Height), ImageLockMode.ReadOnly, myBitmap.PixelFormat);
                Parallel.For(0, width, Xcount =>
                 {
                     for (int Ycount = 0; Ycount < height; Ycount++)
                     {
                         int offset = Xcount * bmpData.Stride + Ycount * (bmpData.Stride / bmpData.Width);
                         int r = Marshal.ReadByte(bmpData.Scan0, offset + 2);
                         if (r > 0)
                         {
                             signal[Xcount + width * Ycount] = r;
                             number++;
                         }
                     }
                 });
                /*
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
                }*/
                Array.Sort(signal);
                double acc = 0;
                for (int j = 0; j < signal.Length; j++)
                {
                    if (signal[j] > 0)
                    {
                        acc = acc + signal[j];
                    }
                }
                output[i] = acc / number;
                //output[i] = averageRealSignal(signal, number);
                //Color pixelColor = myBitmap.GetPixel(277, 314);
                Console.Write("Process for background: {0}/{1}\n", i, frames);
            }
            return output;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter directory name: ");
            string inputName = Console.ReadLine();
            Console.WriteLine("How many percentage for signal starting: ");
            int percentSigSta = Int32.Parse(Console.ReadLine());
            Console.WriteLine("How many percentage for signal ending: ");
            int percentSigEnd = Int32.Parse(Console.ReadLine());
            string filePath = @".\" + inputName + @"\";
            string outputFileName = inputName + @"_" + percentSigSta.ToString() + @"_" + percentSigEnd.ToString() + @".txt";
            System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();//引用stopwatch物件
            clock.Reset();//碼表歸零
            clock.Start();//碼表開始計時
            double[] sigAvg = SSS(filePath, percentSigSta, percentSigEnd);
            double[] backAvg = BBB(filePath);
            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                for (int i = 0; i < sigAvg.Length; i++)
                {
                    //string content = sigAvg[i].ToString();
                    string content = sigAvg[i].ToString() + @" " + backAvg[i].ToString();
                    sw.WriteLine(content);
                }
            }
            //foreach (int x in bnswer) { Console.WriteLine(x); }
            clock.Stop();
            string result1 = clock.Elapsed.TotalMilliseconds.ToString();
            Console.WriteLine("Done!!!It takes {0} ms!", result1);
            Console.ReadLine();
        }
    }
}
