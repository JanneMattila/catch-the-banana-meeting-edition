using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageCreator
{
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
            Console.WriteLine(playerImageData);

            CreatePlayer("monkey1.png", playerImageData, eyes: Color.Red, body: Color.Blue);
            CreatePlayer("monkey2.png", playerImageData, eyes: Color.Blue, body: Color.Brown);
            CreatePlayer("monkey3.png", playerImageData, eyes: Color.DarkBlue, body: Color.DarkKhaki);
        }

        private static void CreatePlayer(string imageName, string data, Color eyes, Color body)
        {
            using var bitmap = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
            var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var rows = lines.Length;
            var columns = lines[0].Length;
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns / 2; x++)
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
}
