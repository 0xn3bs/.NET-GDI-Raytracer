using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Drawing = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Raytracer.Primitives;
using System.Collections.Concurrent;

namespace Raytracer
{
    public partial class Form1 : Form
    {
        private Drawing.Graphics Graphics;
        private Drawing.Bitmap Backbuffer;
        private Sphere Sphere;
        private Lights.PointLight Light;

        public Form1()
        {
            InitializeComponent();
            this.Paint += Form1_Paint;
            this.ClientSize = new Drawing.Size(800, 600);
            this.Resize += Form1_Resize;
            Graphics = this.CreateGraphics();
            Backbuffer = new Drawing.Bitmap(this.ClientRectangle.Width,
                this.ClientRectangle.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            var lightPos = new Vector3(0.0f, 1.5f, 0.1f);

            Scene.Lights.Add(new Lights.PointLight(new Vector3(-1f, 0.1f, 0.5f), Color.White, 1.0f));
            //Scene.Lights.Add(new Lights.Point(new Vector3(0.3f, 0.0f, 0.4f), Color.White, 1.0f));
            Scene.Lights.Add(new Lights.PointLight(new Vector3(2f, 0.0f, 0.1f), Color.White, 1.0f));
            Scene.Objects.Add(new Sphere(lightPos, 0.01f, Color.White, true));
            Sphere = new Sphere(new Vector3(0.0f, -0.3f, 1.0f), 0.25f, Color.White, false, true);
            Scene.Objects.Add(Sphere);
            Scene.Objects.Add(new Sphere(new Vector3(0.0f, 0.20f, 1.25f), 0.25f, Color.White, false, true));
            Scene.Objects.Add(new Sphere(new Vector3(0.0f, 0f, 0.0f), 0.25f, Color.Red));

            Scene.Objects.Add(new Sphere(new Vector3(0.7f, -0.3f, 1.0f), 0.10f, Color.Green));

            Scene.Objects.Add(new Raytracer.Primitives.Plane(Vector3.Up, -0.75f, Color.Gray));


            this.Text = "Ray Tracer - " + Scene.Samples + " Sample(s)";

            GenerateRays();
        }

        void Form1_Resize(object sender, EventArgs e)
        {
            Backbuffer = new Drawing.Bitmap(this.ClientRectangle.Width,
                                    this.ClientRectangle.Height,
                                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            Graphics = this.CreateGraphics();

            GenerateRays();

            this.Invalidate();
        }

        void GenerateRays()
        {
            Scene.Rays = new BlockingCollection<PixelRay>();

            int heightLowerBound = -Convert.ToInt32(Math.Floor(Backbuffer.Height / 2.0f));
            int heightUpperBound = Convert.ToInt32(Math.Ceiling(Backbuffer.Height / 2.0f));
            int widthLowerBound = -Convert.ToInt32(Math.Floor(Backbuffer.Width / 2.0f));
            int widthUpperBound = Convert.ToInt32(Math.Ceiling(Backbuffer.Width / 2.0f));

            var width = Backbuffer.Width;
            var height = Backbuffer.Height;

            Parallel.For(heightLowerBound, heightUpperBound, y =>
            {
                Parallel.For(widthLowerBound, widthUpperBound, x =>
                {
                    var direction = new Vector3(x, y, 390.96f) - Vector3.Zero;

                    var pixelX = x + -widthLowerBound;
                    var pixelY = (height - (y + -heightLowerBound)) - 1;

                    var ray = new PixelRay(new Point(pixelX, pixelY), Vector3.Zero, direction);

                    Scene.Rays.Add(ray);
                });
            });
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            GenerateRays();
            TraceRays();

            Graphics.DrawImage(Backbuffer, 0, 0);
        }

        private void TraceRays()
        {
            var pixelValues = new BlockingCollection<PixelValue>(Backbuffer.Width * Backbuffer.Height);

            Parallel.ForEach(Scene.Rays,
                ray =>
                {
                    var color = ray.GetColor();
                    pixelValues.Add(new PixelValue() { X = ray.PixelX, Y = ray.PixelY, Color = color });
                });

            var BitmapData = Backbuffer.LockBits(new System.Drawing.Rectangle(0, 0, Backbuffer.Width, Backbuffer.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    Backbuffer.PixelFormat);

            IntPtr ptr = BitmapData.Scan0;

            var rgbValues = new int[Backbuffer.Height, Backbuffer.Width];
            foreach (var pixelValue in pixelValues)
            {
                int val = 0;
                val |= short.MaxValue;
                val <<= 8;
                val |= pixelValue.Color.R;
                val <<= 8;
                val |= pixelValue.Color.G;
                val <<= 8;
                val |= pixelValue.Color.B;
                rgbValues[pixelValue.Y, pixelValue.X] = val;
            }

            var buffer = rgbValues.Cast<int>().ToArray();

            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, ptr, Backbuffer.Width * Backbuffer.Height);
            Backbuffer.UnlockBits(BitmapData);
        }

        public struct PixelValue
        {
            public int X, Y;
            public Color Color;
        }
    }
}
