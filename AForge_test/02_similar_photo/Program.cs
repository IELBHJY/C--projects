using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace _02_similar_photo
{
    class SimilarPhoto
    {
        public Image SourceImg;
        public SimilarPhoto(string filePath)
        {
            SourceImg=Image.FromFile(filePath);
        }
        public SimilarPhoto(Stream stream)
        {
            SourceImg= Image.FromStream(stream);
        }
        public String GetHash()
        {
            Image image = ReduceSize();
            Byte[] grayValues = ReduceColor(image);
            Byte average = CalcAverage(grayValues);
            String reslut = ComputeBits(grayValues, average);
            return reslut;
        }
        //Step 1 : Reduce size to 8*8
        public Image ReduceSize(int width= 8, int height= 8)
        {
            Image image = SourceImg.GetThumbnailImage(width, height, () => { return false;}, IntPtr.Zero);
            return image;
        }
        //Step 2 : Reduce Color
        public Byte[] ReduceColor(Image image)
        {
            Bitmap bitMap = new Bitmap(image);
            Byte[] grayValues = new Byte[image.Width* image.Height];
 
            for(int x= 0; x<image.Width; x++)
                for (int y= 0; y < image.Height; y++)
                {
                    Color color = bitMap.GetPixel(x, y);
                    byte grayValue = (byte)((color.R * 30 + color.G * 59 + color.B * 11) / 100);
                    grayValues[x* image.Width + y] = grayValue;
                }
            return grayValues;
        }
        //Step 3 : Average the colors
        public Byte CalcAverage(byte[] values)
        {
            int sum= 0;
            for (int i = 0; i < values.Length; i++)
                sum+= (int)values[i];
            return Convert.ToByte(sum/ values.Length);
        }
        //Step 4 : Compute the bits
        public String ComputeBits(byte[] values, byte averageValue)
        {
            char[] result = new char[values.Length];
            for (int i= 0; i < values.Length; i++)
            {
                if (values[i]< averageValue)
                    result[i] = '0';
                else
                    result[i]= '1';
            }
            return new String(result);
        }
        //Compare hash
        public static Int32 CalcSimilarDegree(string a,string b)
        {
            if (a.Length!= b.Length)
                throw new ArgumentException();
            int count = 0;
            for (int i= 0; i < a.Length; i++)
            {
                if (a[i]!= b[i])
                    count++;
            }
            return count;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            SimilarPhoto SP = new SimilarPhoto(@"E:\复杂机电\单目标定图片\000000.bmp");
            Image image1_8_8 = SP.ReduceSize(8, 8);
            Byte[] ReduceColor1 = SP.ReduceColor(image1_8_8);
            Byte average1 = SP.CalcAverage(ReduceColor1);
            string str1 = SP.ComputeBits(ReduceColor1, average1);


            SimilarPhoto SP1 = new SimilarPhoto(@"E:\复杂机电\单目标定图片\000005.bmp");
            Image image2_8_8 = SP1.ReduceSize(8, 8);
            Byte[] ReduceColor2 = SP1.ReduceColor(image2_8_8);
            Byte average2 = SP1.CalcAverage(ReduceColor2);
            string str2 = SP1.ComputeBits(ReduceColor2, average2);
            int num_count = SimilarPhoto.CalcSimilarDegree(str1, str2);
            Console.WriteLine(str1);
            Console.WriteLine(str2);
            Console.WriteLine(num_count);
            Console.ReadKey();
        }
    }
}
