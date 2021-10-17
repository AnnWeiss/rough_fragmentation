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
        private List<Pixel> pointsList = new List<Pixel>();
        private HashSet<Pair> lines = new HashSet<Pair>();
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            CreateBitmapAtRuntime();
            //GeneratePoints();
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
            int maxXvalue = bmp.Size.Width;
            int maxYvalue = bmp.Size.Height;
            int pointsCount = Convert.ToInt32(textBox1.Text);
            if (pointsCount <= 0 || pointsCount > 1000)
            {
                MessageBox.Show("Введите число от 1 до 1000");
            }
            int pointsIterator = 0;
            pointsList.Clear();
            while (pointsIterator < pointsCount)
            {
                int x = rnd.Next(0, maxXvalue);
                int y = rnd.Next(0, maxYvalue);
                pointsList.Add(new Pixel(x, y, RandCol())); //используется конструктор
                pointsIterator++;
            }
        }

        public void DrawRandomPoints()
        {
            Graphics flagGraphics = Graphics.FromImage(bmp);
            foreach (Pixel p in pointsList)
            {
                flagGraphics.FillEllipse(Brushes.Red, p.X - 2, p.Y - 2, 2, 2);
            }
        }

        public Color RandCol()
        {
            Color outCol = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            return outCol;
        }

        public void CalcDistance()
        {
            if (pointsList.Count == 0)
            {
                MessageBox.Show("Сгенерируйте кол-во точек от 1 до 1000!");
            }
            int indexUpperPixel = -1;
            int indexLeftPixel = -1;
            int[] leftPix = new int[bmp.Height];//массив левых пикселей
            int[] currentPix = new int[bmp.Height];//массив текущих пикселей
            //текущий записываем в предыдущий

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    double minDist = 0;
                    int pointIterator = 0; //индекс для перебора всех точек
                    int indexNearestPoint = 0; //индекс точки для которой есть мин.дистанция
                    foreach (Pixel p in pointsList)
                    {
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
                    Pixel currentPixel = pointsList[indexNearestPoint];
                    bmp.SetPixel(i, j, currentPixel.GetColor());

                    //заполняем связи
                    if (i == 0)
                    {
                        leftPix[j] = indexNearestPoint; //при первой итерации записываем элементы в левый массив
                    }
                    if (i > 0)
                    {
                        indexLeftPixel = leftPix[j];
                        currentPix[j] = indexNearestPoint; //при второй заполняем массив текущих пикселей
                        if (indexNearestPoint != indexLeftPixel && indexLeftPixel >= 0)
                        {
                            Pair line = new Pair(indexNearestPoint, indexLeftPixel);
                            lines.Add(line);
                        }
                    }
                    if (indexNearestPoint != indexUpperPixel && indexUpperPixel >= 0)
                    {
                        Pair line = new Pair(indexNearestPoint, indexUpperPixel);
                        lines.Add(line);
                        
                    }
                    indexUpperPixel = indexNearestPoint; //ставим первый индекс для верхней точки
                }
                indexUpperPixel = -1; //обнуляем счетчик верхних индексов
                if (i > 0)
                {
                    Array.Clear(leftPix, 0, bmp.Height);
                    Array.Copy(currentPix, leftPix, currentPix.Length);
                    Array.Clear(currentPix, 0, bmp.Height);
                }
            }
        }
        public void DrawLines()
        {
            Pen blackPen = new Pen(Color.Black, 1);
            using (var graphics = Graphics.FromImage(bmp))
            {
                foreach (Pair p in lines)
                {
                    graphics.DrawLine(blackPen, pointsList[p.IndexPoint1].X, pointsList[p.IndexPoint1].Y,
                                                pointsList[p.IndexPoint2].X, pointsList[p.IndexPoint2].Y);
                }
            }
            lines.Clear();
        }

       
        private void buttonGenPoints_Click(object sender, EventArgs e)
        {
            CreateBitmapAtRuntime();
            GeneratePoints();
            DrawRandomPoints();
            
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            CalcDistance();
            DrawLines();
            mainPictureBox.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
