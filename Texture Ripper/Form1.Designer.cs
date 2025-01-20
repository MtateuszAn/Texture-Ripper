using System;
using System.Windows.Forms;

namespace Texture_Ripper
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.resultPictureBox = new System.Windows.Forms.PictureBox();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sorceTabControl = new System.Windows.Forms.TabControl();
            this.btnRemoveTab = new System.Windows.Forms.Button();
            this.selectionDataGridView = new System.Windows.Forms.DataGridView();
            this.rotate1 = new System.Windows.Forms.Button();
            this.rotate2 = new System.Windows.Forms.Button();
            this.mirror = new System.Windows.Forms.Button();
            this.mirrorHoriz = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectionDataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // resultPictureBox
            // 
            this.resultPictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.resultPictureBox.Location = new System.Drawing.Point(0, 0);
            this.resultPictureBox.Name = "resultPictureBox";
            this.resultPictureBox.Size = new System.Drawing.Size(200, 200);
            this.resultPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.resultPictureBox.TabIndex = 0;
            this.resultPictureBox.TabStop = false;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFile.Location = new System.Drawing.Point(1289, 9);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenFile.TabIndex = 2;
            this.buttonOpenFile.Text = "Open img";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "png";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "\"Pliki obrazów|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff;*.webp|Wszystkie pliki|*.*";
            // 
            // sorceTabControl
            // 
            this.sorceTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sorceTabControl.Location = new System.Drawing.Point(0, 0);
            this.sorceTabControl.Name = "sorceTabControl";
            this.sorceTabControl.SelectedIndex = 0;
            this.sorceTabControl.Size = new System.Drawing.Size(572, 655);
            this.sorceTabControl.TabIndex = 7;
            // 
            // btnRemoveTab
            // 
            this.btnRemoveTab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveTab.Location = new System.Drawing.Point(1370, 9);
            this.btnRemoveTab.Name = "btnRemoveTab";
            this.btnRemoveTab.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTab.TabIndex = 8;
            this.btnRemoveTab.Text = "Close img";
            this.btnRemoveTab.UseVisualStyleBackColor = true;
            this.btnRemoveTab.Click += new System.EventHandler(this.btnRemoveTab_Click);
            // 
            // selectionDataGridView
            // 
            this.selectionDataGridView.AllowUserToAddRows = false;
            this.selectionDataGridView.AllowUserToDeleteRows = false;
            this.selectionDataGridView.AllowUserToResizeColumns = false;
            this.selectionDataGridView.AllowUserToResizeRows = false;
            this.selectionDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.selectionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.selectionDataGridView.Dock = System.Windows.Forms.DockStyle.Right;
            this.selectionDataGridView.Location = new System.Drawing.Point(1215, 38);
            this.selectionDataGridView.Name = "selectionDataGridView";
            this.selectionDataGridView.ReadOnly = true;
            this.selectionDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.selectionDataGridView.Size = new System.Drawing.Size(242, 655);
            this.selectionDataGridView.TabIndex = 10;
            // 
            // rotate1
            // 
            this.rotate1.BackgroundImage = global::Texture_Ripper.Properties.Resources.rotateL;
            this.rotate1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rotate1.Location = new System.Drawing.Point(12, 5);
            this.rotate1.Name = "rotate1";
            this.rotate1.Size = new System.Drawing.Size(30, 30);
            this.rotate1.TabIndex = 11;
            this.rotate1.UseVisualStyleBackColor = true;
            this.rotate1.Click += new System.EventHandler(this.rotateL_Click);
            // 
            // rotate2
            // 
            this.rotate2.BackgroundImage = global::Texture_Ripper.Properties.Resources.rotateR;
            this.rotate2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rotate2.Location = new System.Drawing.Point(48, 5);
            this.rotate2.Name = "rotate2";
            this.rotate2.Size = new System.Drawing.Size(30, 30);
            this.rotate2.TabIndex = 12;
            this.rotate2.UseVisualStyleBackColor = true;
            this.rotate2.Click += new System.EventHandler(this.rotateR_Click);
            // 
            // mirror
            // 
            this.mirror.BackgroundImage = global::Texture_Ripper.Properties.Resources.flipH;
            this.mirror.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mirror.Location = new System.Drawing.Point(84, 5);
            this.mirror.Name = "mirror";
            this.mirror.Size = new System.Drawing.Size(30, 30);
            this.mirror.TabIndex = 13;
            this.mirror.UseVisualStyleBackColor = true;
            this.mirror.Click += new System.EventHandler(this.mirror_Click);
            // 
            // mirrorHoriz
            // 
            this.mirrorHoriz.BackgroundImage = global::Texture_Ripper.Properties.Resources.flipV;
            this.mirrorHoriz.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mirrorHoriz.Location = new System.Drawing.Point(120, 5);
            this.mirrorHoriz.Name = "mirrorHoriz";
            this.mirrorHoriz.Size = new System.Drawing.Size(30, 30);
            this.mirrorHoriz.TabIndex = 14;
            this.mirrorHoriz.UseVisualStyleBackColor = true;
            this.mirrorHoriz.Click += new System.EventHandler(this.mirrorHoriz_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.resultPictureBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(639, 655);
            this.panel1.TabIndex = 15;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.saveButton.Location = new System.Drawing.Point(564, 9);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 16;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRemoveTab);
            this.panel2.Controls.Add(this.saveButton);
            this.panel2.Controls.Add(this.buttonOpenFile);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1457, 38);
            this.panel2.TabIndex = 17;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 38);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.sorceTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(1215, 655);
            this.splitContainer1.SplitterDistance = 639;
            this.splitContainer1.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1457, 693);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.mirrorHoriz);
            this.Controls.Add(this.mirror);
            this.Controls.Add(this.rotate2);
            this.Controls.Add(this.rotate1);
            this.Controls.Add(this.selectionDataGridView);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Texture Repper";
            //this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.resultPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectionDataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox resultPictureBox;
        public System.Windows.Forms.Button buttonOpenFile;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.TabControl sorceTabControl;
        public System.Windows.Forms.Button btnRemoveTab;
        public System.Windows.Forms.DataGridView selectionDataGridView;
        private System.Windows.Forms.Button rotate1;
        private System.Windows.Forms.Button rotate2;
        private System.Windows.Forms.Button mirror;
        private System.Windows.Forms.Button mirrorHoriz;
        private System.Windows.Forms.Panel panel1;
        private SaveFileDialog saveFileDialog1;
        private Button saveButton;
        private Panel panel2;
        private SplitContainer splitContainer1;
    }
}

