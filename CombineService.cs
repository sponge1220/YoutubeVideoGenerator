using System.Diagnostics;

namespace YoutubeVideoGenerator;

public class CombineService
{
    public static void CombineMusicAndImagesUsingFFmpeg(string sourceFolder, string finalOutputFile)
    {
        var fileIndex = 1;
        var tempFiles = new List<string>();
        var fileListPath = Path.Combine(sourceFolder, "filelist.txt");

        while (true)
        {
            var musicFile = Path.Combine(sourceFolder, $"m{fileIndex:00}.mp3");
            var imageFile = Path.Combine(sourceFolder, $"p{fileIndex:00}.jpg");

            if (!File.Exists(musicFile) || !File.Exists(imageFile))
                break;

            var tempOutputFile = Path.Combine(sourceFolder, $"temp{fileIndex:00}.mp4");
            tempFiles.Add($"file '{tempOutputFile}'");

            var ffmpegCommand =
                $"-loop 1 -i \"{imageFile}\" -i \"{musicFile}\" -c:v libx264 -tune stillimage -s 1920x1080 -r 30 -c:a aac -b:a 192k -pix_fmt yuv420p -shortest \"{tempOutputFile}\"";
            if (!ExecuteFFmpegCommand(ffmpegCommand))
            {
                Console.WriteLine("FFmpeg 命令執行失敗，終止處理。");
                return;
            }

            fileIndex++;
        }

        if (tempFiles.Count <= 0) return;
        File.WriteAllLines(fileListPath, tempFiles);

        var ffmpegConcatCommand = $"-f concat -safe 0 -i \"{fileListPath}\" -c copy \"{finalOutputFile}\"";
        if (!ExecuteFFmpegCommand(ffmpegConcatCommand))
        {
            Console.WriteLine("FFmpeg 合併命令執行失敗。");
            return;
        }

        // 清理臨時文件
        foreach (var tempFile in tempFiles)
        {
            File.Delete(tempFile.Replace("file '", "").Replace("'", ""));
        }

        File.Delete(fileListPath);
    }

    private static bool ExecuteFFmpegCommand(string command)
    {
        try
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
            process?.WaitForExit();

            if (process is { ExitCode: 0 }) return true;
            Console.WriteLine($"FFmpeg 命令執行錯誤: {process?.StandardError.ReadToEnd()}");
            return false;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"執行 FFmpeg 命令時發生異常: {ex.Message}");
            return false;
        }
    }
}