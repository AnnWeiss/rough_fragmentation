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
        private List<Color> colorsList;
        private List<Pair> lines;
        //хешсет. переоприделить equals and hash. 1,2 и 2,1 хранить одинаково
        Random r = new Random();


        public Form1()
        {
            pointsList = new List<Pixel>();
            colorsList = new List<Color>();
            lines = new List<Pair>();
            

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
            colorsList.Add(firstCol);
            colorsList.Add(secondCol);
            colorsList.Add(thirdCol);
            colorsList.Add(fourthCol);
            colorsList.Add(fifthCol);
            colorsList.Add(sixthCol);
            colorsList.Add(seventhCol);
            colorsList.Add(eighthCol);

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
            //int pointsCount = rnd.Next(200, 400);
            int pointsCount = 5;
            int pointsIterator = 0;
            pointsList.Clear();
            while (pointsIterator < pointsCount)
            {
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
            // Color outCol = colorsList[r.Next(0, colorsList.Count)];
            Color outCol = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));

            return outCol;
        }

        public void CalcDistance()
        {
            int indexUpperPixel = -1;
            int indexLeftPixel = -1;
            int[] leftPix = new int[bmp.Height];//массив левых пикселей
            int[] currentPix = new int[bmp.Height];//массив текущих пикселей
            //текущий записываем в предыдущий

            for (int i = 0; i < bmp.Width; i++) //проход по ширине
            {
                for (int j = 0; j < bmp.Height; j++) //проход по высоте
                {
                    double minDist = 0;
                    int pointIterator = 0; //индекс для перебора всех точек
                    int indexNearestPoint = 0; //индекс точки для которой есть мин.дистанция
                    foreach (Pixel p in pointsList)
                    {
                        /*if (i == p.X && j == p.Y)
                        {
                            continue; //пропускаем места, где точки
                        }*/

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
                    if (i > 0)
                    {
                        indexLeftPixel = leftPix[j];
                        currentPix[j] = indexNearestPoint; //при второй заполняем массив текущих пикселей
                    }
                    if (indexNearestPoint != indexLeftPixel && indexLeftPixel >= 0)
                    {
                        Pair line = new Pair(indexNearestPoint, indexLeftPixel);
                        if (!isContainLine(line))
                        {
                            lines.Add(line);
                        }
                    }
                    if (indexNearestPoint != indexUpperPixel && indexUpperPixel >= 0)
                    {
                        Pair line = new Pair(indexNearestPoint, indexUpperPixel);
                        if (!isContainLine(line))
                        {
                            lines.Add(line);
                        }
                    }
                    indexUpperPixel = indexNearestPoint; //ставим первый индекс для первой точки
                    leftPix[j] = indexNearestPoint; //при первой итерации записываем элементы в левый массив
                }
                indexUpperPixel = -1; //обнуляем счетчик верхних индексов
            }
            DrawLines();
            mainPictureBox.Refresh();
        }

        bool isContainLine(Pair line)
        {
            foreach (Pair p in lines)
            {
                if ((p.IndexPoint1 == line.IndexPoint1 && p.IndexPoint2 == line.IndexPoint2) ||
                   (p.IndexPoint2 == line.IndexPoint1 && p.IndexPoint1 == line.IndexPoint2))
                {
                    return true;
                }
            }
            return false;
        }
        public void DrawLines()
        {
            Pen blackPen = new Pen(Color.Black, 2);
            using (var graphics = Graphics.FromImage(bmp))
            {
                foreach (Pair p in lines)
                {
                    graphics.DrawLine(blackPen, pointsList[p.IndexPoint1].X, pointsList[p.IndexPoint1].Y,
                                                pointsList[p.IndexPoint2].X, pointsList[p.IndexPoint2].Y);
                }
            }
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
