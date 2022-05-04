using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageCreator;

class Program
{
    static void Main(string[] args)
    {
        var playerImageData = "" +
            ". . . . f f f f f . . . . . . .\n" +
            ". . . f 0 0 0 0 0 f . . . . . .\n" +
            ". . f d d d d 0 0 0 f . . . . .\n" +
            ". c d 1 d d 1 d 0 0 f f . . . .\n" +
            ". c d 1 d d 1 d 0 0 d d f . . .\n" +
            "c d 0 0 d d d d 0 0 b d c . . .\n" +
            "c d d d d c d d 0 0 b d c . f f\n" +
            "c c c c c d d d 0 0 f c . f 0 f\n" +
            ". f d d d d d 0 0 f f . . f 0 f\n" +
            ". . f f f f f 0 0 0 0 f . f 0 f\n" +
            ". . . . f 0 0 0 0 0 0 0 f f 0 f\n" +
            ". . . f 0 f f 0 f 0 0 0 0 f f .\n" +
            ". . . f 0 f f 0 f 0 0 0 0 f . .\n" +
            ". . . f d b f d b f f 0 f . . .\n" +
            ". . . f d d c d d b b d f . . .\n" +
            ". . . . f f f f f f f f f . . .";

        var sharkImageData = "" +
            ". . . . . . . . . . . . . . c c f f f f . . . . . . . . . . . . . . . \n" +
            ". . . . . . . . . . . . . c d d b b b f . . . . . . . . . . . . . . . \n" +
            ". . . . . . f f f f f f c d d b b b f . . . . . . . . . . . . . . . . \n" +
            ". . . . f f b b b b b b b b b b b b b c f f f . . . . . . . c c c c c \n" +
            ". . f f b b b b b b b b c b c b b b b b c c c f f . . . . c d b b b c \n" +
            ". f b b b b b b b b b b c b b c b b b b c c c c c f f f c d d b b c . \n" +
            "f b c b b b b b b b b b b c b c b b b b c c c c c c c c b d b b f . . \n" +
            "f b b b b b b b 2 2 2 b b c b b b b b c c c c c c c c c c b b c f . . \n" +
            "f f b b 1 1 1 1 2 2 2 b b c b b b b c c c c c c c b c f f b c c f . . \n" +
            ". f f 1 1 1 1 1 1 1 1 1 b b b b c c c c c c b b b c c . . f b b c f . \n" +
            ". . . c c c c c c c 1 1 1 b d b b b f d d b c c c . . . . . f f b b f \n" +
            ". . . . . . . c c c c c c f b d b b b f c c . . . . . . . . . . f f f \n" +
            ". . . . . . . . . . . . . . f f f f f f . . . . . . . . . . . . . . . ";

        var bananaImageData = "" +
            ". . . . . 5 f \n" +
            ". . . 5 5 5 5 \n" +
            ". . 5 5 5 5 . \n" +
            ". 5 5 e 5 . . \n" +
            "5 5 5 5 5 . . \n" +
            "5 5 5 5 . . . \n" +
            "5 5 5 . . . . \n" +
            "5 e 5 . . . . \n" +
            "5 5 5 . . . . \n" +
            ". 5 5 5 . . . \n" +
            ". 5 5 e 5 . . \n" +
            ". . 5 5 5 . . ";

        CreatePlayer("monkey1.png", playerImageData, eyes: Color.Red, body: Color.Blue);
        CreatePlayer("monkey2.png", playerImageData, eyes: Color.Blue, body: Color.Brown);
        CreatePlayer("monkey3.png", playerImageData, eyes: Color.DarkBlue, body: Color.DarkKhaki);
        CreatePlayer("monkey4.png", playerImageData, eyes: Color.Blue, body: Color.Yellow);
        CreatePlayer("monkey5.png", playerImageData, eyes: Color.Black, body: Color.Violet);
        CreatePlayer("monkey6.png", playerImageData, eyes: Color.Blue, body: Color.DarkBlue);

        CreateImage("shark.png", sharkImageData);
        CreateImage("banana.png", bananaImageData);
    }

    private static void CreatePlayer(string imageName, string data, Color eyes, Color body)
    {
        var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var rows = lines.Length;
        var columns = lines[0].Length / 2;
        using var bitmap = new Bitmap(columns, rows, PixelFormat.Format32bppArgb);
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var c1 = lines[y][x * 2];
                if (c1 == '.')
                {
                    continue;
                }
                if (c1 == '0')
                {
                    bitmap.SetPixel(x, y, body);
                }
                else if (c1 == '1')
                {
                    bitmap.SetPixel(x, y, eyes);
                }
                else
                {
                    bitmap.SetPixel(x, y, DefineColor(c1));
                }
            }
        }
        bitmap.Save(imageName, ImageFormat.Png);
    }

    private static void CreateImage(string imageName, string data)
    {
        var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var rows = lines.Length;
        var columns = lines[0].Length / 2;
        using var bitmap = new Bitmap(columns, rows, PixelFormat.Format32bppArgb);
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                var c1 = lines[y][x * 2];
                if (c1 == '.')
                {
                    continue;
                }
                bitmap.SetPixel(x, y, DefineColor(c1));
            }
        }
        bitmap.Save(imageName, ImageFormat.Png);
    }

    private static Color DefineColor(char c)
    {
        return c switch
        {
            '1' => Color.White,
            '2' => Color.FromArgb(255, 33, 33), // Red
            '3' => Color.FromArgb(255, 147, 196), // Pink
            '4' => Color.FromArgb(255, 129, 53),
            '5' => Color.FromArgb(255, 246, 9), // Yellow
            '6' => Color.FromArgb(36, 156, 163),
            '7' => Color.FromArgb(120, 220, 82), // Green
            '8' => Color.FromArgb(0, 63, 173), // Blue
            '9' => Color.FromArgb(135, 242, 255), // Light blue
            'a' => Color.FromArgb(142, 46, 196), // Violet
            'b' => Color.FromArgb(164, 131, 159),
            'c' => Color.FromArgb(92, 64, 108),
            'd' => Color.FromArgb(229, 205, 196),
            'e' => Color.FromArgb(145, 70, 61),
            'f' => Color.Black,
            _ => Color.FromArgb(255, 0, 0, 0)
        };
    }
}
