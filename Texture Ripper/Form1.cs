using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texture_Ripper.Properties;

namespace Texture_Ripper
{
    public partial class Form1 : Form
    {
        public Icon pointIcon;

        public Form1()
        {
            InitializeComponent();

            SourceDisplayHandler.form1 = this;
            ResoultDisplayHandler.form1 = this;

            ResoultDisplayHandler.Inicialize(resultPictureBox);

            sorceTabControl.SelectedIndexChanged += SorceTabControl_SelectedIndexChanged;

            InitializeSelectionDataGridView();

            pointIcon = Resources.point;

            saveFileDialog1.Filter = "Pliki PNG|*.png|Pliki JPEG|*.jpg|Wszystkie pliki|*.*";
            saveFileDialog1.DefaultExt = "png";
        }


        #region TabControll
        //Otwarcie obrazu i dodanie nowej zakladki
        void openImage()
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string title = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                Image image = Image.FromFile(openFileDialog1.FileName);

                // Tworzenie nowej zakładki z obrazem
                SourceTabPage myTabPage = new SourceTabPage(title, image);

                // Dodanie zakładki do TabControl
                sorceTabControl.TabPages.Add(myTabPage);

                // Opcjonalnie możesz ustawić aktywną zakładkę na nowo dodaną
                sorceTabControl.SelectedTab = myTabPage;

                var pictureBox = myTabPage.pictureBox;

                SorceTabControl_SelectedIndexChanged(null,null);
               
            }
        }
        //usuwanie otwartej zakladki
        private void btnRemoveTab_Click(object sender, EventArgs e)
        {
            // Sprawdzenie, czy istnieje wybrana zakładka
            if (sorceTabControl.SelectedTab != null)
            {
                // Usunięcie wybranej zakładki
                selectionDataGridView.Rows.Clear();
                resultPictureBox.Invalidate();

                sorceTabControl.TabPages.Remove(sorceTabControl.SelectedTab);
            }
        }

        private void SorceTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
            {
                var pictureBox = selectedTabPage.pictureBox;

                SourceDisplayHandler.PictureBoxChange(pictureBox);

                UpdateSelectionDataGridView(selectedTabPage);
            }
        }

        #endregion

        #region SelectionList

        public void ClearSelect()
        {
            selectionDataGridView.ClearSelection();
            Selection.selected = null;
        }

        public void SetSelect(Selection selection)
        {
            if (sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
            {
                selectionDataGridView.ClearSelection();
                Selection.selected =selection;
                selectedTabPage.pictureBox.Invalidate();

                int rowIndex = selectedTabPage.selections.IndexOf(selection);

                if (rowIndex < 0)
                    return;

                selectionDataGridView.Rows[rowIndex].Selected = true;
            }
        }
        void SetSelect(int i)
        {
            if (sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
            {
                selectionDataGridView.ClearSelection();
                Selection.selected = selectedTabPage.selections[i];
                selectedTabPage.pictureBox.Invalidate();

                int rowIndex = selectedTabPage.selections.IndexOf(Selection.selected);

                if (rowIndex < 0)
                    return;

                selectionDataGridView.Rows[rowIndex].Selected = true;
            }
        }

        private void InitializeSelectionDataGridView()
        {
            // Kolumna z nazwą
            var nameColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Selection Name",
                Name = "SelectionName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            };
            selectionDataGridView.Columns.Add(nameColumn);
            //kolumna z przyciskiem
            var buttonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Name = "Visibility",
                Width = 10
            };
            selectionDataGridView.Columns.Add(buttonColumn);
            // Kolumna z przyciskiem
            buttonColumn = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Name = "Delete",
                Width = 10
            };
            selectionDataGridView.Columns.Add(buttonColumn);

            selectionDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            selectionDataGridView.ColumnHeadersVisible = false;
            selectionDataGridView.MultiSelect = false;
            selectionDataGridView.RowHeadersVisible = false;

            // Obsługa kliknięcia w przycisk
            selectionDataGridView.CellClick += SelectionDataGridView_CellClick;

            // Obsługa rysowania ikon
            selectionDataGridView.CellPainting += SelectionDataGridView_CellPainting;
        }
        private void SelectionDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == selectionDataGridView.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Zatrzymaj domyślne malowanie komórki
                e.Handled = true;

                // Wyczyść tło komórki
                e.PaintBackground(e.ClipBounds, true);

                // Wczytaj ikonę z zasobów
                Image icon = Resources.Delete;

                // Oblicz położenie ikony, aby była wyśrodkowana
                int iconX = e.CellBounds.Left + (e.CellBounds.Width - icon.Width) / 2;
                int iconY = e.CellBounds.Top + (e.CellBounds.Height - icon.Height) / 2;

                // Narysuj ikonę
                e.Graphics.DrawImage(icon, new Point(iconX, iconY));
            }

            if (e.ColumnIndex == selectionDataGridView.Columns["Visibility"].Index && e.RowIndex >= 0 && sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
            {
                // Zatrzymaj domyślne malowanie komórki
                e.Handled = true;

                // Wyczyść tło komórki
                e.PaintBackground(e.ClipBounds, true);

                int selectionIndex = e.RowIndex;
                Image icon;
                // Wczytaj ikonę z zasobów
                if (selectedTabPage.selections[selectionIndex].visible)
                {
                    icon = Properties.Resources.Visibility;
                }
                else
                {
                    icon = Properties.Resources.NoVisibility;
                }
                

                // Oblicz położenie ikony, aby była wyśrodkowana
                int iconX = e.CellBounds.Left + (e.CellBounds.Width - icon.Width) / 2;
                int iconY = e.CellBounds.Top + (e.CellBounds.Height - icon.Height) / 2;

                // Narysuj ikonę
                e.Graphics.DrawImage(icon, new Point(iconX, iconY));
            }
        }
        public void UpdateSelectionDataGridView(SourceTabPage selectedTabPage)
        {
            selectionDataGridView.Rows.Clear();

            for (int i = 0; i < selectedTabPage.selections.Count; i++)
            {
                string selectionName = $"Selection {i + 1}";
                selectionDataGridView.Rows.Add(selectionName);
            }
            if (Selection.selected!=null) 
            {
                int rowIndex = selectedTabPage.selections.IndexOf(Selection.selected);

                selectionDataGridView.ClearSelection();
                if (rowIndex < 0)
                    return;
                selectionDataGridView.Rows[rowIndex].Selected = true;
            }
        }
        private void SelectionDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Pobierz indeks zaznaczonego elementu
            int selectionIndex = e.RowIndex;

            // Sprawdź, czy kliknięto przycisk
            if (e.ColumnIndex == selectionDataGridView.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                // Wykonaj akcję usuniecia
                if (sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
                {
                    selectedTabPage.selections.RemoveAt(selectionIndex);
                    UpdateSelectionDataGridView(selectedTabPage); // Odśwież listę
                    selectedTabPage.pictureBox.Invalidate();
                }
            }else if (e.ColumnIndex == selectionDataGridView.Columns["Visibility"].Index && e.RowIndex >= 0)
            {
                // Wykonaj akcję zmiany widzialnosci
                if (sorceTabControl.SelectedTab is SourceTabPage selectedTabPage)
                {

                    selectedTabPage.selections[selectionIndex].visible=!selectedTabPage.selections[selectionIndex].visible;
                    UpdateSelectionDataGridView(selectedTabPage); // Odśwież listę
                    selectedTabPage.pictureBox.Invalidate();
                }
            }
            else
            {
                SetSelect(selectionIndex);
            }


            resultPictureBox.Invalidate();
        }

        #endregion

        #region buttons
        private void button1_Click(object sender, EventArgs e)
        {
            openImage();
        }

        private void mirror_Click(object sender, EventArgs e)
        {
            if (Selection.selected != null)
            {
                Selection.selected.MirrorFlipVert();
                resultPictureBox.Invalidate();
            }
        }

        private void mirrorHoriz_Click(object sender, EventArgs e)
        {
            if (Selection.selected != null)
            {
                Selection.selected.MirrorFlipHori();
                resultPictureBox.Invalidate();
            }
        }

        private void rotateR_Click(object sender, EventArgs e)
        {
            if (Selection.selected != null)
            {
                Selection.selected.FlipRight();
                resultPictureBox.Invalidate();
            }
        }

        private void rotateL_Click(object sender, EventArgs e)
        {
            if (Selection.selected != null)
            {
                Selection.selected.FlipLeft();
                resultPictureBox.Invalidate();
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            // Wyświetlenie dialogu zapisu
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                try
                {
                    // Pobranie ścieżki do zapisu
                    string filePath = saveFileDialog1.FileName;

                    // Utworzenie bitmapy o wymiarach PictureBox
                    Bitmap bmp = new Bitmap(ResoultDisplayHandler.pictureBox.ClientSize.Width, ResoultDisplayHandler.pictureBox.ClientSize.Height);

                    // Rysowanie zawartości PictureBox na bitmapie
                    ResoultDisplayHandler.pictureBox.DrawToBitmap(bmp, ResoultDisplayHandler.pictureBox.ClientRectangle);

                    // Zapis bitmapy do pliku
                    bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                    // Zwalnianie zasobów
                    bmp.Dispose();

                    // komunikat o sukcesie
                    MessageBox.Show("Obraz zapisano pomyślnie.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Obsługa błędu zapisu
                    MessageBox.Show($"Błąd podczas zapisywania obrazu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        public Point FindFreeSpace(Point start, int w, int h)
        {
            List<Rectangle> occupiedRectangles = new List<Rectangle>();

            // Zbieranie zajętych prostokątów
            foreach (SourceTabPage tapPage in sorceTabControl.TabPages)
            {
                foreach (Selection sel in tapPage.selections)
                {
                    Point posB = sel.position;
                    int wB = sel.selectedImage.Width;
                    int hB = sel.selectedImage.Height;
                    occupiedRectangles.Add(new Rectangle(posB.X, posB.Y, wB, hB));
                }
            }

            if (occupiedRectangles.Count == 0)
            {
                return new Point(0, 0);
            }

            // Obliczenie maksymalnego obszaru zajmowanego przez prostokąty
            int maxX = occupiedRectangles.Max(r => r.Right);
            int maxY = occupiedRectangles.Max(r => r.Bottom);

            // Lista potencjalnych miejsc
            List<Point> candidates = new List<Point> { start };

            // Dodajemy wszystkie rogi zajętych prostokątów jako potencjalne miejsca
            foreach (var rect in occupiedRectangles)
            {
                // Punkty na prawej krawędzi
                for (int y = rect.Top; y <= rect.Bottom; y += 1)
                {
                    candidates.Add(new Point(rect.Right, y));
                }

                // Punkty na dolnej krawędzi
                for (int x = rect.Left; x <= rect.Right; x += 1)
                {
                    candidates.Add(new Point(x, rect.Bottom));
                }
            }

            // Sortowanie według dwóch kryteriów:
            // 1. punkty powiększające zajmowany obszar najmniej
            // 2. Odległość od (0, 0)
            candidates = candidates
                .OrderBy(p => ((Math.Max(maxX, p.X + w) - maxX) + (Math.Max(maxY, p.Y + h) - maxY))) // Łączny wzrost obszaru
                .ThenBy(p => p.X + p.Y) // Odległość od (0, 0)
                .ToList();

            // Szukamy pierwszego wolnego miejsca
            foreach (var candidate in candidates)
            {
                Rectangle newRect = new Rectangle(candidate.X, candidate.Y, w, h);

                if (CanPlaceRectangle(newRect, occupiedRectangles))
                {
                    return candidate;
                }
            }

            // Jeśli nic nie znajdziemy, dodajemy nowy prostokąt poza aktualnym obszarem
            return new Point(maxX, maxY); // Umieszczamy na końcu aktualnego obszaru
        }

        // Funkcja sprawdzająca kolizje
        private bool CanPlaceRectangle(Rectangle newRect, List<Rectangle> occupiedRectangles)
        {
            foreach (var rect in occupiedRectangles)
            {
                if (newRect.IntersectsWith(rect))
                {
                    return false;
                }
            }
            return true;
        }

    }

}
