using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App1
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        private List<Pixel> pointsList;
        private List<Color> colors;
        Random r = new Random();

        public Form1()
        {
            pointsList = new List<Pixel>();
            colors = new List<Color>();

            Color firstCol = new Color();
            Color secondCol = new Color();
            Color thirdCol = new Color();
            Color fourthCol = new Color();
            Color fifthCol = new Color();
            Color sixthCol = new Color();
            Color seventhCol = new Color();
            Color eighthCol = new Color();
            firstCol = Color.FromArgb(255, 60, 167, 251);
            secondCol = Color.FromArgb(255, 201, 134, 243);
            thirdCol = Color.FromArgb(255, 23, 239, 209);
            fourthCol = Color.FromArgb(255, 73, 254, 246);
            fifthCol = Color.FromArgb(255, 251, 149, 225);
            sixthCol = Color.FromArgb(255, 179, 209, 245);
            seventhCol = Color.FromArgb(255, 243, 227, 217);
            eighthCol = Color.FromArgb(255, 87, 102, 235);
            colors.Add(firstCol);
            colors.Add(secondCol);
            colors.Add(thirdCol);
            colors.Add(fourthCol);
            colors.Add(fifthCol);
            colors.Add(sixthCol);
            colors.Add(seventhCol);
            colors.Add(eighthCol);

            InitializeComponent();
            CreateBitmapAtRuntime();
            GeneratePoints();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void CreateBitmapAtRuntime()
        {
            bmp = new Bitmap(mainPictureBox.Size.Width, mainPictureBox.Size.Height);
            Graphics flagGraphics = Graphics.FromImage(bmp);
            flagGraphics.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
            mainPictureBox.Image = bmp;
            mainPictureBox.Invalidate();
        }

        public void GeneratePoints()
        {
            Random rnd = new Random();
            int maxXvalue = bmp.Size.Width;
            int maxYvalue = bmp.Size.Height;
            int pointsCount = rnd.Next(200, 400);
            int pointsIterator = 0;
            pointsList.Clear();
            while(pointsIterator < pointsCount) {
                int x = rnd.Next(0, maxXvalue);
                int y = rnd.Next(0, maxYvalue);
                pointsList.Add(new Pixel(x, y, RandCol())); //используется конструктор
                pointsIterator++;
            }
            DrawRandomPoints();
        }

        public void DrawRandomPoints()
        {
            foreach (Pixel p in pointsList)
            {
                bmp.SetPixel(p.X, p.Y, p.GetColor());
            }
        }

        public Color RandCol()
        {
            Color outCol = colors[r.Next(0, colors.Count)];
            return outCol;
        }

        public void CalcDistance()
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    double minDist = 0;
                    int pointIterator = 0; //индекс для перебора всех точек
                    int indexNearestPoint = 0; //индекс точки для которой есть мин.дистанция
                    foreach (Pixel p in pointsList)
                    {
                        if (i == p.X && j == p.Y)
                        {
                            continue; //пропускаем места, где точки
                        }

                        double xVal = Math.Pow(p.X - i, 2);
                        double yVal = Math.Pow(p.Y - j, 2);
                        double dist = Math.Sqrt(xVal + yVal);
                        
                        if (pointIterator == 0)
                        {
                            minDist = dist; //ставим первый dist который получили
                            indexNearestPoint = pointIterator;
                        }
                        else
                        {
                            if (dist < minDist)
                            {
                                minDist = dist; //перебирая все точки, определяем минимальный dist для пикселя
                                indexNearestPoint = pointIterator;//когда нашли ближайшую точку к пикселю, запомнили ее индекс
                            }
                        }
                        pointIterator++;
                    }
                    Pixel pix = pointsList[indexNearestPoint];
                    bmp.SetPixel(i, j, pix.GetColor());
                }
            }
            mainPictureBox.Refresh();
        }
        private void buttonGenPoints_Click(object sender, EventArgs e)
        {
            CreateBitmapAtRuntime();
            GeneratePoints();
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            CalcDistance();
        }
    }
}
