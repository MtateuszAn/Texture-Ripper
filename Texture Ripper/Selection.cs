using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using OpenCvSharp;
using Point = System.Drawing.Point;
using PointF = System.Drawing.PointF;


namespace Texture_Ripper
{
    public class Selection
    {
        public Mat homographyMatrix;

        public static Selection selected;

        //punkty okresliajace zaznaczony obszar
        public Point[] points = new Point[4];
        //public bool selected=false;
        public bool visible=true;
        Pen selectionPen = Pens.Red;

        //pozycja skala obrazu wynikowego danego obszaru
        public Point position = new Point(0,0);
        //obraz wynikowy danego obszaru
        public Bitmap selectedImage;

        private SourceTabPage tabPage;

        public Selection(Point p1, Point p2, Point p3, Point p4, Point pos, Image img, SourceTabPage orginalTabPage)
        {
            tabPage = orginalTabPage;
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            points[3] = p4;

            position = pos;

            selectedImage = (Bitmap)img;

            //homMat = new Homografia();
            //homMat.Update(points,selectedImage.Height,selectedImage.Width);

            Point2f[] srcPoints = {
            new Point2f(0, 0),
            new Point2f(selectedImage.Width, 0),
            new Point2f(selectedImage.Width, selectedImage.Height),
            new Point2f(0, selectedImage.Height)
            };

            Point2f[] dstPoints = {
            new Point2f(p1.X, p1.Y),
            new Point2f(p2.X, p2.Y),
            new Point2f(p3.X, p3.Y),
            new Point2f(p4.X, p4.Y)
            };

            

            var srcArray = InputArray.Create(srcPoints);
            var dstArray = InputArray.Create(dstPoints);

            homographyMatrix = Cv2.FindHomography(srcArray, dstArray);
        }

        public void DrawSelection(PaintEventArgs e, float sF)
        {
            if (selected == this)
            {
                selectionPen = Pens.Red;
            }
            else
            {
                selectionPen = Pens.Blue;
            }

            //Border
            e.Graphics.DrawLine(selectionPen,
                new Point((int)(points[0].X * sF), (int)(points[0].Y * sF)),
                new Point((int)(points[1].X * sF), (int)(points[1].Y * sF))
            );

            e.Graphics.DrawLine(selectionPen,
                new Point((int)(points[1].X * sF), (int)(points[1].Y * sF)),
                new Point((int)(points[2].X * sF), (int)(points[2].Y * sF))
            );

            e.Graphics.DrawLine(selectionPen,
                new Point((int)(points[2].X * sF), (int)(points[2].Y * sF)),
                new Point((int)(points[3].X * sF), (int)(points[3].Y * sF))
            );

            e.Graphics.DrawLine(selectionPen,
                new Point((int)(points[3].X * sF), (int)(points[3].Y * sF)),
                new Point((int)(points[0].X * sF), (int)(points[0].Y * sF))
            );

            //Grid
            Point[] grid = Grid();
            for (int i = 0; i < 11; i+=2)
            {
                Point scaledGrid0 = new Point((int)(grid[i].X * sF), (int)(grid[i].Y * sF));
                Point scaledGrid1 = new Point((int)(grid[i+1].X * sF), (int)(grid[i+1].Y * sF));

                e.Graphics.DrawLine(selectionPen, scaledGrid0, scaledGrid1);
            }
        }
        private Point[] Grid()
        {
            Point[] grid = new Point[12];

            // Obliczenie punktu 1/4 odległości między p1 a p2
            grid[0] = TransformPoint(new Point(selectedImage.Width / 4, 0));
            grid[1] = TransformPoint(new Point(selectedImage.Width / 4, selectedImage.Height));
            grid[2] = TransformPoint(new Point(selectedImage.Width / 2, 0));
            grid[3] = TransformPoint(new Point(selectedImage.Width / 2, selectedImage.Height));
            grid[4] = TransformPoint(new Point(3 * selectedImage.Width / 4, 0));
            grid[5] = TransformPoint(new Point(3 * selectedImage.Width / 4, selectedImage.Height));

            grid[6] = TransformPoint(new Point(0, selectedImage.Height/4));
            grid[7] = TransformPoint(new Point(selectedImage.Width, selectedImage.Height/4));
            grid[8] = TransformPoint(new Point(0, 2*selectedImage.Height / 4));
            grid[9] = TransformPoint(new Point(selectedImage.Width, 2*selectedImage.Height / 4));
            grid[10] = TransformPoint(new Point(0, 3*selectedImage.Height / 4));
            grid[11] = TransformPoint(new Point(selectedImage.Width, 3*selectedImage.Height / 4));


            return grid;
        }

        public PointF TransformPointF(Point point)
        {
            Point2f[] srcPoints = { new Point2f(point.X,point.Y) };

            Point2f[] targetPoint = OpenCvSharp.Cv2.PerspectiveTransform(srcPoints, homographyMatrix);

            return new PointF(targetPoint[0].X, targetPoint[0].Y);
        }
        public Point TransformPoint(Point point)
        {
            Point2f[] srcPoints = { new Point2f(point.X, point.Y) };

            Point2f[] targetPoint = OpenCvSharp.Cv2.PerspectiveTransform(srcPoints, homographyMatrix);

            return new Point((int)targetPoint[0].X, (int)targetPoint[0].Y);
        }

        public void DrawPoint(PaintEventArgs e, Icon icon, float sF)
        {
            int x = icon.Size.Width / 2;
            int y = icon.Size.Height / 2;
            foreach (Point p in points)
            {
                e.Graphics.DrawIcon(icon,(int)(p.X *sF) -x, (int)(p.Y * sF) - y);
            }
        }

        public void MovePoint(int i, int x, int y)
        {
            points[i] = new Point(x, y);

            FindHomography();
        }
        private void FindHomography()
        {
            Point2f[] srcPoints = {
            new Point2f(0, 0),
            new Point2f(selectedImage.Width, 0),
            new Point2f(selectedImage.Width, selectedImage.Height),
            new Point2f(0, selectedImage.Height)
            };

            Point2f[] dstPoints = {
            new Point2f(points[0].X, points[0].Y),
            new Point2f(points[1].X, points[1].Y),
            new Point2f(points[2].X, points[2].Y),
            new Point2f(points[3].X, points[3].Y)
            };

            var srcArray = InputArray.Create(srcPoints);
            var dstArray = InputArray.Create(dstPoints);

            homographyMatrix = Cv2.FindHomography(srcArray, dstArray);
        }
        public void InvalidateImage() // Dodac wspolbieznasc?!
        {
            Bitmap sourceImg = (Bitmap)tabPage.image;
            FindHomography();
            Bitmap bitmap = new Bitmap(selectedImage);

            for (int i = 0; i < selectedImage.Width; i++)
            {
                for (int j = 0; j < selectedImage.Height; j++)
                {
                    //punkt na obrazie źródłowym
                    PointF sourcePoint = TransformPointF(new Point(i, j));
                    Color color = Color.Black;

                    if (sourcePoint.X >= 0 && sourcePoint.X < sourceImg.Width - 1 &&
                        sourcePoint.Y >= 0 && sourcePoint.Y < sourceImg.Height - 1)
                    {
                        // Pobierz wartości x i y
                        float x = sourcePoint.X;
                        float y = sourcePoint.Y;

                        // Znajdź najbliższe integerowe współrzędne pikseli
                        int x1 = (int)Math.Floor(x);
                        int y1 = (int)Math.Floor(y);
                        int x2 = x1 + 1;
                        int y2 = y1 + 1;

                        // Oblicz frakcje dla interpolacji
                        float dx = x - x1;
                        float dy = y - y1;

                        // Pobierz kolory sąsiednich pikseli
                        Color c11 = sourceImg.GetPixel(x1, y1); // Piksel (x1, y1)
                        Color c12 = sourceImg.GetPixel(x1, y2); // Piksel (x1, y2)
                        Color c21 = sourceImg.GetPixel(x2, y1); // Piksel (x2, y1)
                        Color c22 = sourceImg.GetPixel(x2, y2); // Piksel (x2, y2)

                        // Zinterpoluj kolory
                        byte r = (byte)(
                            (1 - dx) * (1 - dy) * c11.R +
                            (1 - dx) * dy * c12.R +
                            dx * (1 - dy) * c21.R +
                            dx * dy * c22.R
                        );
                        byte g = (byte)(
                            (1 - dx) * (1 - dy) * c11.G +
                            (1 - dx) * dy * c12.G +
                            dx * (1 - dy) * c21.G +
                            dx * dy * c22.G
                        );
                        byte b = (byte)(
                            (1 - dx) * (1 - dy) * c11.B +
                            (1 - dx) * dy * c12.B +
                            dx * (1 - dy) * c21.B +
                            dx * dy * c22.B
                        );

                        byte a = (byte)(
                            (1 - dx) * (1 - dy) * c11.A +
                            (1 - dx) * dy * c12.A +
                            dx * (1 - dy) * c21.A +
                            dx * dy * c22.A
                        );

                        // Utwórz nowy kolor z zinterpolowanych wartości
                        color = Color.FromArgb(a, r, g, b);
                    }

                    // Zapisz piksel w nowym obrazie
                    bitmap.SetPixel(i, j, color);
                }
            }

            selectedImage = bitmap;
        }

        public void DrawImage(PaintEventArgs e)
        {
            if (selected == this)
            {
                // rysowanie obrazu
                e.Graphics.DrawImage(selectedImage, position);

                // obliczanie prostokąta dla ramki
                Rectangle borderRect = new Rectangle(
                    position.X,
                    position.Y,
                    selectedImage.Width,
                    selectedImage.Height
                );

                // rysowanie ramki
                using (Pen redPen = new Pen(Color.Red, 2)) 
                {
                    e.Graphics.DrawRectangle(redPen, borderRect);
                }
            }
            else
            {
                // rysowanie obrazu bez ramki
                e.Graphics.DrawImage(selectedImage, position);
            }
        }


        private void SetPX(int x)
        {
            position.X = Math.Max(0, x);
        }
        private void SetPY(int x)
        {
            position.Y = Math.Max(0, x);
        }

        public void MoveImage(int x,int y)
        {
            SetPX(position.X + x);
            SetPY(position.Y + y);
        }

        public Side PointInImage(int x, int y)
        {
            int Tolerance = 6;

            if (selectedImage == null) return Side.none;

            // Sprawdzamy, czy punkt znajduje się wewnątrz obrazu
            if (x + Tolerance > position.X &&
                y + Tolerance > position.Y &&
                x - Tolerance < position.X + selectedImage.Width &&
                y - Tolerance < position.Y + selectedImage.Height)
            {
                // Sprawdzamy rogi obrazu z tolerancją
                if (x >= position.X - Tolerance && x <= position.X + Tolerance &&
                    y >= position.Y - Tolerance && y <= position.Y + Tolerance)  // Top-left corner
                    return Side.upLeft;
                if (x >= position.X + selectedImage.Width - Tolerance && x <= position.X + selectedImage.Width + Tolerance &&
                    y >= position.Y - Tolerance && y <= position.Y + Tolerance)  // Top-right corner
                    return Side.upRight;
                if (x >= position.X - Tolerance && x <= position.X + Tolerance &&
                    y >= position.Y + selectedImage.Height - Tolerance && y <= position.Y + selectedImage.Height + Tolerance)  // Bottom-left corner
                    return Side.downLeft;
                if (x >= position.X + selectedImage.Width - Tolerance && x <= position.X + selectedImage.Width + Tolerance &&
                    y >= position.Y + selectedImage.Height - Tolerance && y <= position.Y + selectedImage.Height + Tolerance)  // Bottom-right corner
                    return Side.downRight;

                // Sprawdzamy krawędzie obrazu z tolerancją
                if (x >= position.X - Tolerance && x <= position.X + Tolerance)  // Left edge
                    return Side.left;
                if (x >= position.X + selectedImage.Width - Tolerance && x <= position.X + selectedImage.Width + Tolerance)  // Right edge
                    return Side.right;
                if (y >= position.Y - Tolerance && y <= position.Y + Tolerance)  // Top edge
                    return Side.up;
                if (y >= position.Y + selectedImage.Height - Tolerance && y <= position.Y + selectedImage.Height + Tolerance)  // Bottom edge
                    return Side.down;


                // Jeżeli punkt znajduje się w środku obrazu
                return Side.midle;
            }

            // Jeśli punkt znajduje się poza obrazem
            return Side.none;
        }

        public void ResizeImage(Side side, int x, int y)
        {
            int newWidth = selectedImage.Width;
            int newHeight = selectedImage.Height;

            switch (side)
            {
                case Side.up: // Zmiana rozmiaru w górę
                    newHeight -= y;  // Zmniejszamy wysokość obrazu
                    if(newHeight>10)
                        SetPY(position.Y + y); // Przemieszczamy obraz w górę
                    break;

                case Side.down: // Zmiana rozmiaru w dół
                    newHeight += y;  // Zwiększamy wysokość obrazu
                    break;

                case Side.left: // Zmiana rozmiaru w lewo
                    newWidth -= x;   // Zmniejszamy szerokość obrazu
                    if (newWidth > 10)
                        SetPX(position.X + x); // Przemieszczamy obraz w lewo
                    break;

                case Side.right: // Zmiana rozmiaru w prawo
                    newWidth += x;   // Zwiększamy szerokość obrazu
                    break;

                case Side.upLeft: // Zmiana rozmiaru w lewy górny róg
                    newWidth -= x;   // Zmniejszamy szerokość obrazu
                    newHeight -= y;  // Zmniejszamy wysokość obrazu
                    if (newWidth > 10)
                        SetPX(position.X + x); // Przemieszczamy obraz w lewo
                    if (newHeight > 10)
                        SetPY(position.Y + y); // Przemieszczamy obraz w górę
                    break;

                case Side.upRight: // Zmiana rozmiaru w prawy górny róg
                    newWidth += x;   // Zwiększamy szerokość obrazu
                    newHeight -= y;  // Zmniejszamy wysokość obrazu
                    if (newHeight > 10)
                        SetPY(position.Y + y); // Przemieszczamy obraz w górę
                    break;

                case Side.downLeft: // Zmiana rozmiaru w lewy dolny róg
                    newWidth -= x;   // Zmniejszamy szerokość obrazu
                    newHeight += y;  // Zwiększamy wysokość obrazu
                    if (newWidth > 10)
                        SetPX(position.X + x); // Przemieszczamy obraz w lewo
                    break;

                case Side.downRight: // Zmiana rozmiaru w prawy dolny róg
                    newWidth += x;   // Zwiększamy szerokość obrazu
                    newHeight += y;  // Zwiększamy wysokość obrazu
                    break;

                case Side.midle: // Zmiana rozmiaru w środku
                    newWidth += x;
                    newHeight += y;
                    break;

                case Side.none:  // Brak zmiany
                default:
                    break;
            }

            // Tworzymy nowy obraz o zmienionych wymiarach
            if (newHeight > 10 && newWidth > 10)
            {
                Bitmap resizedBitmap = new Bitmap(selectedImage, newWidth, newHeight);

                // Zastępujemy oryginalny obraz
                selectedImage = resizedBitmap;
            }
            
        }

        public void selectionToImage(Image image)
        {

        }

        internal void MirrorFlipVert()
        {
            Point p1 = points[0];
            Point p2 = points[2];
            points[0] = points[1];
            points[2] = points[3];
            points[1] = p1;
            points[3] = p2;
            InvalidateImage();
        }
        internal void MirrorFlipHori()
        {
            Point p1 = points[0];
            Point p2 = points[1];
            points[0] = points[3];
            points[1] = points[2];
            points[3] = p1;
            points[2] = p2;
            InvalidateImage();
        }

        internal void FlipRight()
        {
            Point p1 = points[1];
            Point p2 = points[0];
            points[0] = points[3];
            points[1] = p2;
            points[3] = points[2];
            points[2] = p1;
            

            Bitmap resizedBitmap = new Bitmap(selectedImage, selectedImage.Height, selectedImage.Width);

            // Zastępujemy oryginalny obraz
            selectedImage = resizedBitmap;

            InvalidateImage();
        }

        internal void FlipLeft()
        {
            Point p1 = points[0];

            points[0] = points[1];
            points[1] = points[2];
            points[2] = points[3];
            points[3] = p1;

            Bitmap resizedBitmap = new Bitmap(selectedImage, selectedImage.Height, selectedImage.Width);

            // Zastępujemy oryginalny obraz
            selectedImage = resizedBitmap;

            InvalidateImage();
        }
    }
}

public enum Side
{
    none=0,
    midle=1,
    up = 2,
    down = 3,
    right = 4,
    left = 5,
    upLeft = 6,
    upRight = 7,
    downLeft = 8,
    downRight = 9
}