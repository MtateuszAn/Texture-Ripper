using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Texture_Ripper
{
    internal static class ResoultDisplayHandler
    {
        public static Form1 form1;
        public static PictureBox pictureBox;
        private static Panel panel;
        private static Point _startPoint;         // Początkowy punkt zaznaczenia
        private static Selection _transformedSelection = null; // Zmienna do przechowania referencji do zaznaczenia
        private static Side _foundSide = Side.none;
        private static Side _foundSide2 = Side.none;

        public static float scaleFactor = 1;
        private static int width;
        private static int height;

        public static void Inicialize(PictureBox newPB)
        {
            pictureBox = newPB;
            if(pictureBox.Parent is Panel parent)
                panel = parent;

            width = pictureBox.Width;
            height = pictureBox.Height;

            pictureBox.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
            pictureBox.MouseMove += new MouseEventHandler(pictureBox_MouseTransformSelection);
            pictureBox.MouseMove += new MouseEventHandler(pictureBox_MouseHover);
            pictureBox.MouseUp += new MouseEventHandler(pictureBox_MouseUp);

            pictureBox.MouseDown += new MouseEventHandler(pictureBox_MouseDown_Drag);
            pictureBox.MouseMove += new MouseEventHandler(pictureBox_MouseMove_Drag);
            pictureBox.MouseUp += new MouseEventHandler(pictureBox_MouseUp_Drag);

            panel.MouseDown += new MouseEventHandler(pictureBox_MouseDown_Clear);
            panel.MouseDown += new MouseEventHandler(pictureBox_MouseDown_Drag);
            panel.MouseMove += new MouseEventHandler(pictureBox_MouseMove_Drag_Parent);
            panel.MouseUp += new MouseEventHandler(pictureBox_MouseUp_Drag);

            pictureBox.Paint += new PaintEventHandler(pictureBox_Paint);

            pictureBox.MouseEnter += (s, e) => pictureBox.Focus(); // Fokus na PictureBox

            pictureBox.MouseLeave += (s, e) => SourceDisplayHandler.Reset();
        }


        private static void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            bool empty=true;
            ResPoints();
            //foreach (TabPage tabPage in sorceTabControl.TabPages)
            for (int j = form1.sorceTabControl.TabPages.Count - 1; j >= 0; j--)
            {
                var tabPage = form1.sorceTabControl.TabPages[j];
                if (tabPage is SourceTabPage sourceTabPage && sourceTabPage.selections.Count > 0)
                {
                    empty = false;
                    for (int i = sourceTabPage.selections.Count - 1; i >= 0; i--)
                    {
                        Selection sel = sourceTabPage.selections[i];
                        sel.DrawImage(e);

                        LU.X = Math.Min(LU.X, sel.position.X);
                        LU.Y = Math.Min(LU.Y, sel.position.Y);

                        RD.X = Math.Max(RD.X, sel.position.X + sel.selectedImage.Width);
                        RD.Y = Math.Max(RD.Y, sel.position.Y + sel.selectedImage.Height);
                        
                    }
                }
            }

            if (empty)
            {
                LU.X = 0;
                LU.Y = 0;

                RD.X = 0;
                RD.Y = 0;
            }
            EvaluatePB();
        }


        #region SelectionImages




        private static void pictureBox_MouseHover(object sender, MouseEventArgs e)
        {
            if (_transformedSelection != null)
                return;

            _foundSide2 = Side.none; // Resetowanie znalezionej strony

            foreach (TabPage tabPage in form1.sorceTabControl.TabPages)
            {
                if (_foundSide2 != Side.none)
                    break;

                if (tabPage is SourceTabPage sourceTabPage && sourceTabPage.selections.Count > 0)
                {
                    foreach (Selection selection in sourceTabPage.selections)
                    {
                        _foundSide2 = selection.PointInImage(e.X, e.Y);
                        if (_foundSide2 != Side.none)
                        {
                            // Zmiana kursora na podstawie znalezionej strony
                            switch (_foundSide2)
                            {
                                case Side.up:
                                case Side.down:
                                    Cursor.Current = Cursors.SizeNS; // Kursor do zmiany rozmiaru w pionie
                                    break;
                                case Side.left:
                                case Side.right:
                                    Cursor.Current = Cursors.SizeWE; // Kursor do zmiany rozmiaru w poziomie
                                    break;
                                case Side.upLeft:
                                case Side.downRight:
                                    Cursor.Current = Cursors.SizeNWSE; // Kursor do zmiany rozmiaru po skosie
                                    break;
                                case Side.upRight:
                                case Side.downLeft:
                                    Cursor.Current = Cursors.SizeNESW; // Kursor do zmiany rozmiaru po skosie (przeciwna przekątna)
                                    break;
                                default:
                                    Cursor.Current = Cursors.Default; // Domyślny kursor
                                    break;
                            }
                            break;
                        }
                    }
                }
            }

            // Jeśli nie znaleziono żadnej strony, ustaw domyślny kursor
            if (_foundSide2 == Side.none)
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private static void pictureBox_MouseDown_Clear(object sender, MouseEventArgs e)
        {
            form1.ClearSelect();
        }

        private static void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            form1.ClearSelect();
            if (e.Button == MouseButtons.Left)
            {
                _startPoint = e.Location;
                foreach (TabPage tabPage in form1.sorceTabControl.TabPages)
                {
                    if (_foundSide != Side.none)
                        break;

                    if (tabPage is SourceTabPage sourceTabPage && sourceTabPage.selections.Count > 0)
                    {
                        foreach (Selection selection in sourceTabPage.selections)
                        {
                            _foundSide = selection.PointInImage(e.X, e.Y);
                            if (_foundSide != Side.none)
                            {
                                _transformedSelection = selection;
                                form1.SetSelect(selection);

                                break;
                            }
                        }
                    }
                }
            }
            if (form1.sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
            {
                selectedTabPage.pictureBox.Invalidate();
            }
        }

        private static void pictureBox_MouseTransformSelection(object sender, MouseEventArgs e)
        {
            if (_transformedSelection != null)
            {
                switch (_foundSide)
                {
                    case Side.midle:
                        _transformedSelection.MoveImage(e.X - _startPoint.X, e.Y - _startPoint.Y);
                        break;
                    default:
                        _transformedSelection.ResizeImage(_foundSide, e.X - _startPoint.X, e.Y - _startPoint.Y);
                        break;
                }
                _startPoint = e.Location;
            }
            pictureBox.Invalidate();
        }

        private static void pictureBox_MouseUp(object sender, MouseEventArgs e)
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
            if (_foundSide > Side.midle && _transformedSelection != null)
                _transformedSelection.InvalidateImage();

            _foundSide = Side.none;
            _transformedSelection = null;
        }
        #endregion

        #region PictureBoxTransform

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

                //minimalna wartosc pozycji
                x = Math.Max(x, -(pictureBox.Width - pictureBox.Parent.Width) - pictureBox.Parent.Width / 2);
                y = Math.Max(y, -(pictureBox.Height - pictureBox.Parent.Height) - pictureBox.Parent.Height / 2);

                //maksymalna wartosc pozycji
                x = Math.Min(pictureBox.Parent.Width/2, x);
                y = Math.Min(pictureBox.Parent.Height/2, y);

                pictureBox.Location = new Point(x, y);
            }
        }

        private static void pictureBox_MouseMove_Drag_Parent(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // Oblicz przesunięcie
                int dx = e.X - dragStartPoint.X;
                int dy = e.Y - dragStartPoint.Y;


                // Przesuń PictureBox
                int x = pictureBox.Location.X + dx;
                int y = pictureBox.Location.Y + dy;

                //minimalna wartosc pozycji
                x = Math.Max(x, -(pictureBox.Width - pictureBox.Parent.Width) - pictureBox.Parent.Width / 2);
                y = Math.Max(y, -(pictureBox.Height - pictureBox.Parent.Height) - pictureBox.Parent.Height / 2);

                //maksymalna wartosc pozycji
                x = Math.Min(pictureBox.Parent.Width / 2, x);
                y = Math.Min(pictureBox.Parent.Height / 2, y);

                pictureBox.Location = new Point(x, y);
                dragStartPoint = e.Location;
            }
        }

        private static void pictureBox_MouseUp_Drag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _isDragging = false; // Zatrzymaj przesuwanie
            }
        }

        public static void pictureBox2_MouseWheel(object sender, MouseEventArgs e)
        {

            // Aktualna pozycja kursora myszy względem obrazu
            float mouseXRelativeToImage = (e.X - pictureBox.Location.X) / scaleFactor;
            float mouseYRelativeToImage = (e.Y - pictureBox.Location.Y) / scaleFactor;

            // Stara wartość skali
            float oldScaleFactor = scaleFactor;

            // Zmiana skali w zależności od kierunku scrollowania
            if (e.Delta > 0) // Przybliżenie
            {
                scaleFactor *= 1.1f; // Zwiększenie współczynnika skalowania o 10%
            }
            else if (e.Delta < 0) // Oddalenie
            {
                scaleFactor *= 0.9f; // Zmniejszenie współczynnika skalowania o 10%
            }

            // Zaktualizowanie rozmiaru PictureBox na podstawie nowej skali
            pictureBox.Width = (int)(width * scaleFactor);
            pictureBox.Height = (int)(height * scaleFactor);

            // Nowa pozycja obrazu, aby uwzględnić przybliżenie/oddalenie względem kursora

            int x = (int)(e.X - mouseXRelativeToImage * scaleFactor);
            int y = (int)(e.Y - mouseYRelativeToImage * scaleFactor);

            x = Math.Max(x, -(pictureBox.Width - pictureBox.Parent.Width));
            y = Math.Max(y, -(pictureBox.Height - pictureBox.Parent.Height));

            x = Math.Min(0, x);
            y = Math.Min(0, y);

            pictureBox.Location = new Point(x, y);
        }

        #endregion

        #region PictureBoxBounds

        static Point LU = new Point(0,0);
        static Point RD = new Point(0,0);

        public static void ResPoints()
        {
            LU.X = int.MaxValue;
            LU.Y = int.MaxValue;
            RD.X = int.MinValue;
            RD.Y = int.MinValue;
        }

        public static void EvaluatePB()
        {
            
            ResizeRD();
            //ResizeLU();
            ValidatePBPosition();
        }

        

        private static void ValidatePBPosition()
        {
            int x = pictureBox.Location.X;
            int y = pictureBox.Location.Y;

            //minimalna wartosc pozycji
            x = Math.Max(x, -(pictureBox.Width - pictureBox.Parent.Width) - pictureBox.Parent.Width / 2);
            y = Math.Max(y, -(pictureBox.Height - pictureBox.Parent.Height) - pictureBox.Parent.Height / 2);

            //maksymalna wartosc pozycji
            x = Math.Min(pictureBox.Parent.Width / 2, x);
            y = Math.Min(pictureBox.Parent.Height / 2, y);

            pictureBox.Location = new Point(x, y);
        }

        public static void ResizeRD()
        {
            int margin = 0;
            // Określenie minimalnego rozmiaru PictureBox
            int minWidth = 1; // Minimalna szerokość
            int minHeight = 1; // Minimalna wysokość

            // Obliczenie wymaganych rozmiarów na podstawie RD
            int requiredWidth = RD.X;
            int requiredHeight = RD.Y;

            // Ustawienie nowych wymiarów PictureBox
            pictureBox.Width = Math.Max(requiredWidth + margin, minWidth);
            pictureBox.Height = Math.Max(requiredHeight + margin, minHeight);
        }
        private static void ResizeLU()
        {
            Point newLocation = pictureBox.Location;

            newLocation.X += LU.X; 
            newLocation.Y += LU.Y; 
            

            pictureBox.Location = newLocation;

            foreach (SourceTabPage tabPage in form1.sorceTabControl.TabPages)
            {
                foreach (Selection sel in tabPage.selections)
                {
                    if(sel!= Selection.selected)
                        sel.MoveImage(-LU.X,-LU.Y);
                }
            }

        }

        #endregion
    }

}
