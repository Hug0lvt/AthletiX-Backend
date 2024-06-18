using System;
using System.Diagnostics;

namespace API.Services.Processing
{
    public static class ExtractThumbnail
    {
        public static void GetThumbnail(string inputFilePath, string outputFilePath, int timeInSeconds)
        {
            string ffmpegPath = "ffmpeg";

            var processStartInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-ss {timeInSeconds} -i \"{inputFilePath}\" -frames:v 1 \"{outputFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        Console.WriteLine("Error extracting thumbnail:");
                        Console.WriteLine(error);
                    }
                    else
                    {
                        Console.WriteLine($"Thumbnail extracted successfully to {outputFilePath}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while extracting the thumbnail:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
