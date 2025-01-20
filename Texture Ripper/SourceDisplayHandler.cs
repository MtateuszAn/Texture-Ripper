using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Texture_Ripper
{


    internal static class SourceDisplayHandler
    {
        public static Form1 form1;
        private static PictureBox pictureBox;
        private static SourceTabPage selectedTabPage;

        private static bool _newSelection = false;   // Flaga czy dodajemy nowy obszar

        private static Rectangle _selectionRectangle;  // Prostokąt zaznaczenia
        private static Point _startPoint;         // Początkowy punkt zaznaczenia

        private static int _selectedPointI = 0;  // Zmienna do przechowania znalezionego punktu
        private static Selection _transformedSelection = null; // Zmienna do przechowania referencji do zaznaczenia
        public static void Reset() => _transformedSelection = null;



        static MouseEventHandler _eventPictureBox_MouseDown = new MouseEventHandler(SourceDisplayHandler.pictureBox_MouseDown);
        static MouseEventHandler _eventPictureBox_MouseDown_Drag = new MouseEventHandler(SourceDisplayHandler.pictureBox_MouseDown_Drag);
        static MouseEventHandler _eventPictureBox_MouseMove = new MouseEventHandler(SourceDisplayHandler.pictureBox_MouseMove);
        static MouseEventHandler _eventPictureBox_MouseMove_Drag = new MouseEventHandler(SourceDisplayHandler.pictureBox_MouseMove_Drag);
        static MouseEventHandler _eventPictureBox_MouseUp = new MouseEventHandler(SourceDisplayHandler.pictureBox_MouseUp);
        static MouseEventHandler _eventPictureBox_MouseUp_Drag = new MouseEventHandler(SourceDisplayHandler.pictureBox_MouseUp_Drag);
        static MouseEventHandler _eventPictureBox2_MouseWheel = new MouseEventHandler(SourceDisplayHandler.pictureBox2_MouseWheel);
        static PaintEventHandler _eventPictureBox_Paint = new PaintEventHandler(SourceDisplayHandler.pictureBox_Paint);
        public static void PictureBoxChange(PictureBox newPB)
        {
            // Usunięcie zdarzeń z poprzedniego PictureBox
            if (pictureBox != null)
            {
                pictureBox.MouseDown  -= _eventPictureBox_MouseDown;
                pictureBox.MouseDown  -= _eventPictureBox_MouseDown_Drag;
                pictureBox.MouseMove  -= _eventPictureBox_MouseMove;
                pictureBox.MouseMove  -= _eventPictureBox_MouseMove_Drag;
                pictureBox.MouseUp    -= _eventPictureBox_MouseUp;
                pictureBox.MouseUp    -= _eventPictureBox_MouseUp_Drag;
                pictureBox.MouseWheel -= _eventPictureBox2_MouseWheel;
                pictureBox.Paint      -= _eventPictureBox_Paint;
            }

            // Przypisanie nowego PictureBox
            pictureBox = newPB;

            // Przypisanie zdarzeń do nowego PictureBox
            pictureBox.MouseDown  += _eventPictureBox_MouseDown;
            pictureBox.MouseDown  += _eventPictureBox_MouseDown_Drag;
            pictureBox.MouseMove  += _eventPictureBox_MouseMove;
            pictureBox.MouseMove  += _eventPictureBox_MouseMove_Drag;
            pictureBox.MouseUp    += _eventPictureBox_MouseUp;
            pictureBox.MouseUp    += _eventPictureBox_MouseUp_Drag;
            pictureBox.MouseWheel += _eventPictureBox2_MouseWheel;
            pictureBox.Paint      += _eventPictureBox_Paint;

            //Zapewnienie porprawnej konfiguracji pictureBox
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.MouseEnter += (s, args) => pictureBox.Focus();

            // Ustawienie rozmiarów PictureBox zgodnie ze skalą
            if (form1.sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
            {
                SourceDisplayHandler.selectedTabPage = selectedTabPage;
                pictureBox.Width = (int)(SourceDisplayHandler.selectedTabPage.image.Width * SourceDisplayHandler.selectedTabPage.scaleFactor);
                pictureBox.Height = (int)(SourceDisplayHandler.selectedTabPage.image.Height * SourceDisplayHandler.selectedTabPage.scaleFactor);
            }
        }

        public static void pictureBox_Paint(object sender, PaintEventArgs e)
        {

            if (_newSelection)
            {
                // Rysowanie prostokąta zaznaczenia
                e.Graphics.DrawRectangle(Pens.Red, _selectionRectangle);
            }

            if (selectedTabPage.selections.Count > 0)
            {
                foreach (Selection selection in selectedTabPage.selections)
                {
                    if (selection.visible == true)
                    {
                        selection.DrawSelection(e, selectedTabPage.scaleFactor);
                        selection.DrawPoint(e, form1.pointIcon, selectedTabPage.scaleFactor);
                    }
                }

            }
        }

        #region PicBoxDrag
        private static bool _isDragging = false;   // Flaga czy poruszamy obszarem
        private static Point dragStartPoint = Point.Empty;
        private static void pictureBox_MouseDown_Drag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _isDragging = true;
                dragStartPoint = e.Location; // Zapamiętaj punkt początkowy
            }
        }

        private static void pictureBox_MouseMove_Drag(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // Oblicz przesunięcie
                int dx = e.X - dragStartPoint.X;
                int dy = e.Y - dragStartPoint.Y;


                // Przesuń PictureBox
                int x = pictureBox.Location.X + dx;
                int y = pictureBox.Location.Y + dy;

                x = Math.Max(x, -(pictureBox.Width-pictureBox.Parent.Width));
                y = Math.Max(y, -(pictureBox.Height - pictureBox.Parent.Height));

                x = Math.Min(0, x);
                y = Math.Min(0, y);

                pictureBox.Location = new Point(x, y);
            }
        }

        private static void pictureBox_MouseUp_Drag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _isDragging = false; // Zatrzymaj przesuwanie
            }
        }
        #endregion


        public static void pictureBox2_MouseWheel(object sender, MouseEventArgs e)
        {

            // Aktualna pozycja kursora myszy względem obrazu
            float mouseXRelativeToImage = (e.X - pictureBox.Location.X) / selectedTabPage.scaleFactor;
            float mouseYRelativeToImage = (e.Y - pictureBox.Location.Y) / selectedTabPage.scaleFactor;

            // Stara wartość skali
            float oldScaleFactor = selectedTabPage.scaleFactor;

            // Zmiana skali w zależności od kierunku scrollowania
            if (e.Delta > 0) // Przybliżenie
            {
                selectedTabPage.scaleFactor *= 1.1f; // Zwiększenie współczynnika skalowania o 10%
            }
            else if (e.Delta < 0) // Oddalenie
            {
                selectedTabPage.scaleFactor *= 0.9f; // Zmniejszenie współczynnika skalowania o 10%
            }

            // Zaktualizowanie rozmiaru PictureBox na podstawie nowej skali
            pictureBox.Width = (int)(selectedTabPage.image.Width * selectedTabPage.scaleFactor);
            pictureBox.Height = (int)(selectedTabPage.image.Height * selectedTabPage.scaleFactor);

            // Nowa pozycja obrazu, aby uwzględnić przybliżenie/oddalenie względem kursora

            int x = (int)(e.X - mouseXRelativeToImage * selectedTabPage.scaleFactor);
            int y = (int)(e.Y - mouseYRelativeToImage * selectedTabPage.scaleFactor);

            x = Math.Max(x, -(pictureBox.Width - pictureBox.Parent.Width));
            y = Math.Max(y, -(pictureBox.Height - pictureBox.Parent.Height));

            x = Math.Min(0, x);
            y = Math.Min(0, y);

            pictureBox.Location = new Point(x, y);
        }


        // Zdarzenie przycisku myszy - początek zaznaczenia
        public static void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox!=null)
            {
                if (e.Button == MouseButtons.Left && pictureBox.Image != null)
                {
                    _startPoint = e.Location; // Początkowy punkt zaznaczenia

                    Point pos = new Point((int)(e.Location.X / selectedTabPage.scaleFactor), (int)(e.Location.Y / selectedTabPage.scaleFactor));
                    CheckForPointNearMouse(pos);

                    if (_transformedSelection == null)
                    {
                        _newSelection = true;
                        _selectionRectangle = new Rectangle(e.X, e.Y, 0, 0); // Inicjalizacja prostokąta
                    }
                    else
                    {
                        form1.SetSelect(_transformedSelection);
                        Cursor.Current = Cursors.Hand;
                    }

                }
            }
        }

        // Zdarzenie poruszania myszą - aktualizacja zaznaczenia
        public static void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (_newSelection)
                {
                    // Aktualizacja prostokąta zaznaczenia
                    _selectionRectangle = new Rectangle(
                        Math.Min(_startPoint.X, e.X),
                        Math.Min(_startPoint.Y, e.Y),
                        Math.Abs(_startPoint.X - e.X),
                        Math.Abs(_startPoint.Y - e.Y));

                    pictureBox.Invalidate(); // Odświeżenie kontrolki, aby narysować prostokąt
                }
                else if (_transformedSelection != null)
                {
                    int x = e.Location.X;
                    int y = e.Location.Y;

                    // Ograniczenie wartości x i y
                    x = Math.Max(1, Math.Min(x, pictureBox.Width - 1));
                    y = Math.Max(1, Math.Min(y, pictureBox.Height - 1));

                    EditSelectedPoint((int)(x / selectedTabPage.scaleFactor), (int)(y / selectedTabPage.scaleFactor));
                    pictureBox.Invalidate(); // Odświeżenie kontrolki, aby narysować prostokąt
                }
            }
            else
            {
                Point pos = new Point((int)(e.Location.X / selectedTabPage.scaleFactor), (int)(e.Location.Y / selectedTabPage.scaleFactor));
                CheckForPointNearMouse(pos);

                if (_transformedSelection != null)
                {
                    Cursor.Current = Cursors.Hand;
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                }

            }

            
        }

        // Zdarzenie puszczenia przycisku myszy - zakończenie zaznaczenia
        public static void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            SourceTabPage sourceTabPage;
            if (form1.sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
            {
                sourceTabPage = selectedTabPage;
            }
            else
            {
                return;
            }
            // koniec dodawania nowego obszaru
            if (_newSelection)
            {
                _newSelection = false;

                if (_selectionRectangle.Width > 1 && _selectionRectangle.Height > 1)
                {
                    // Wycięcie zaznaczonego obszaru i utworzenie nowego obrazu
                    Bitmap sourceBitmap = new Bitmap(sourceTabPage.image);
                    Bitmap selectedBitmap = new Bitmap(_selectionRectangle.Width, _selectionRectangle.Height);

                    using (Graphics g = Graphics.FromImage(selectedBitmap))
                    {
                        // Skopiowanie zaznaczonego obszaru do nowego obrazu
                        g.DrawImage(sourceBitmap, new Rectangle(0, 0, selectedBitmap.Width, selectedBitmap.Height),
                            _selectionRectangle, GraphicsUnit.Pixel);
                    }
                    int newWidth = (int)(selectedBitmap.Width / SourceDisplayHandler.selectedTabPage.scaleFactor);
                    int newHeight = (int)(selectedBitmap.Height / SourceDisplayHandler.selectedTabPage.scaleFactor);

                    selectedBitmap = new Bitmap(selectedBitmap, newWidth, newHeight);
                    //nowy obiekt obszaru zaznaczenia
                    // Oblicz współrzędne ograniczone przez rozmiar PictureBox
                    int rawStartX = Math.Max(1, Math.Min(_startPoint.X, pictureBox.Width - 1));
                    int rawStartY = Math.Max(1, Math.Min(_startPoint.Y, pictureBox.Height - 1));

                    int rawEndX = Math.Max(1, Math.Min(e.Location.X, pictureBox.Width - 1));
                    int rawEndY = Math.Max(1, Math.Min(e.Location.Y, pictureBox.Height - 1));

                    // Przeskalowanie współrzędnych
                    int startX = (int)(rawStartX / SourceDisplayHandler.selectedTabPage.scaleFactor);
                    int startY = (int)(rawStartY / SourceDisplayHandler.selectedTabPage.scaleFactor);

                    int endX = (int)(rawEndX / SourceDisplayHandler.selectedTabPage.scaleFactor);
                    int endY = (int)(rawEndY / SourceDisplayHandler.selectedTabPage.scaleFactor);

                    // Tworzenie punktów zaznaczenia
                    Point topLeft = new Point(startX, startY);
                    Point topRight = new Point(endX, startY);
                    Point bottomRight = new Point(endX, endY);
                    Point bottomLeft = new Point(startX, endY);

                    Point position = form1.FindFreeSpace(new Point(0,0),selectedBitmap.Width, selectedBitmap.Height);

                    // Utworzenie nowego obiektu Selection
                    Selection newSel = new Selection(topLeft, topRight, bottomRight, bottomLeft, position, selectedBitmap, sourceTabPage);

                    sourceTabPage.selections.Add(newSel);
                    form1.UpdateSelectionDataGridView(selectedTabPage);
                    form1.SetSelect(newSel);
                    newSel.InvalidateImage();
                }
            }// koniec modyfikowania punktu obszaru
            else if (_transformedSelection != null)
            {
                _transformedSelection.InvalidateImage();
            }


            Reset();
            form1.resultPictureBox.Invalidate();
        }

        public static void CheckForPointNearMouse(Point e)
        {
            SourceTabPage sourceTabPage;
            if (form1.sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
            {
                sourceTabPage = selectedTabPage;
            }
            else
            {
                return;
            }

            foreach (Selection selection in sourceTabPage.selections)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (selection.visible == false)
                        break;
                    PointF point = selection.points[i];
                    Point mouse = e;

                    // Oblicz odległość między punktem myszy a punktem
                    double distance = Math.Sqrt(Math.Pow(mouse.X - point.X, 2) + Math.Pow(mouse.Y - point.Y, 2));

                    // Sprawdzenie, czy myszka jest w pobliżu punktu (<= 10 pikseli)
                    if (distance <= 10)
                    {
                        // Zapisz referencję do znalezionego punktu
                        _selectedPointI = i;
                        _transformedSelection = selection;
                        return;  // Kończymy działanie funkcji po znalezieniu pierwszego punktu
                    }
                }
            }

            _transformedSelection = null;
            return;
        }

        public static void EditSelectedPoint(int x, int y)
        {
            if (_transformedSelection != null) // Sprawdzenie, czy punkt został wybrany
            {
                // Edytowanie pozycji punktu
                Point newPoint = new Point(x, y);

                // Aktualizacja punktu w oryginalnym obiekcie Selection
                _transformedSelection.MovePoint(_selectedPointI, x, y);
            }
        }

    }
}
