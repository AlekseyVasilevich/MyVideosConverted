using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace MyVideosConvert
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string rootDirectory = Directory.GetCurrentDirectory();

            //Console.WriteLine(rootDirectory);
            rootDirectory = RemoveBin(rootDirectory);
            Console.WriteLine(rootDirectory);

            // inputFileName - your filename
            string inputFileName = "myvideo.mp4";
            string outputFileName = "myvideoconverted.mp4";

            string inputPath = Path.Combine(rootDirectory, inputFileName);
            string outputPath = Path.Combine(rootDirectory, outputFileName);

            await ConvertVideo(inputPath, outputPath);

           // rootDirectory = RemoveBin(rootDirectory);

        }

        static async Task ConvertVideo(string inputPath, string outputPath)
        {
            try
            {
                FFmpeg.SetExecutablesPath(@"C:\ffmpeg\bin");
                var ffmpeg = FFmpeg.Conversions.New();
                //input
                ffmpeg.AddParameter($"-i {inputPath}");

                //specify and codec
                //ffmpeg.AddParameter("-c:v h264_nvenc");
                ffmpeg.AddParameter("-c:v libx264");
                //output
                ffmpeg.AddParameter($"{outputPath}");

                //progress
                ffmpeg.OnProgress += (sender, args) =>
                {
                    double progress = (double)args.Duration.Ticks / TimeSpan.FromSeconds(1).Ticks;
                    Console.WriteLine($"Progress: {progress}");
                };

                await ffmpeg.Start();
                Console.WriteLine("Conversion completed successfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static string RemoveBin(string path)
        {
            string[] directoriesToRemove = { "bin", "obj" };
            foreach(string dir in directoriesToRemove)
            {
                int index = path.LastIndexOf(dir);
                if(index != -1)
                {
                    path = path.Substring(0, index);
                }
            }
            return path;
        }

       
    }
}
