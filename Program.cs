using YoutubeVideoGenerator;

Console.WriteLine("dir:");
var dir = Console.ReadLine();

if (!string.IsNullOrEmpty(dir))
{
    var finalOutputFile = Path.Combine(dir, "final_output.mp4");

    CombineService.CombineMusicAndImagesUsingFFmpeg(dir, finalOutputFile);
}
else
{
    Console.WriteLine("the path provided is invalid。");
}


