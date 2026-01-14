using OpenTK;
using OpenTK.Graphics.OpenGL;

class Mandelbrot
{
    public static void Render(int width, int height, int maxIterations)
    {
        GL.Begin(PrimitiveType.Points);

        for (int px = 0; px < width; px++)
        {
            for (int py = 0; py < height; py++)
            {
                var x0 = (px / (double)width) * 3.47 - 2.47;
                var y0 = (py / (double)height) * 2.24 - 1.12;

                double x = 0.0;
                double y = 0.0;

                int iteration = 0;

                while (x * x + y * y <= 2*2 && iteration < maxIterations)
                {
                    double xtemp = x * x - y * y + x0;
                    y = 2 * x * y + y0;
                    x = xtemp;
                    iteration++;
                }

                var color = iteration / (double)maxIterations;
                GL.Color3(color, color, color);
                
                GL.Vertex2((px / (double)width) * 2 - 1, (py / (double)height) * 2 - 1);
            }
        }

        GL.End();
    }
}
