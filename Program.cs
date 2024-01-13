using YoutubeVideoGenerator;

Console.WriteLine("dir:");
var dir = Console.ReadLine();

if (!string.IsNullOrEmpty(dir))
{
    var combineService = new CombineService();
    var musicFile = Path.Combine(dir, "m01.mp3");
    var imageFile = Path.Combine(dir, "p01.jpg");
    var ffmpegCommand = $"-loop 1 -i \"{imageFile}\" -i \"{musicFile}\" -c:v libx264 -tune stillimage -c:a aac -b:a 192k -pix_fmt yuv420p -shortest \"{dir}\"/output.mp4";
    
    CombineService.ExecuteFFmpegCommand(ffmpegCommand);
}
else
{
    Console.WriteLine("the path provided is invalid。");
}


