using System.Diagnostics;

namespace YoutubeVideoGenerator;

public class CombineService
{
    public static void ExecuteFFmpegCommand(string command)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = command,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using var process = Process.Start(startInfo);
        using var reader = process?.StandardError;
        var result = reader?.ReadToEnd();
        Console.WriteLine(result);
    }
}