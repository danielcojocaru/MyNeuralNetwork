using Auxiliar.Worker;
using Auxiliar.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    public partial class UiForm : Form
    {
        public UiWorker W { get; set; }
        public int Len { get; set; }

        public UiForm()
        {
            InitializeComponent();
            InitializeFields();
            InitializeWorker();
            InitializePaint();
        }


        private void UiForm_Load(object sender, EventArgs e)
        {
        }

        private void InitializeFields()
        {
            Len = DataWorker._len;
        }

        private void InitializeWorker()
        {
            W = new UiWorker();
            W.Create(this);
            //W.Train();
        }

        private Pen Pen;
        private bool Moving;
        private int X;
        private int Y;

        private void InitializePaint()
        {
            Pen = new Pen(Color.Black, 3);
            Pen.StartCap = Pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            ClearImage();
        }

        private void ClearImage()
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height, pictureBox.CreateGraphics());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearImage();
        }

        private void btnGuess_Click(object sender, EventArgs e)
        {
            Guess();
        }

        private void Guess()
        {
            byte[] imgAsByte = GetImgFromPictureBox();

            //DataWorker w = new DataWorker();
            //w.WriteToFile(imgAsByte, "testImg");

            int guessed = W.Guess(imgAsByte);
            lblGuess.Text = guessed.ToString();
        }

        private byte[] GetImgFromPictureBox()
        {
            //Bitmap MyBitmap = ScaleImage(pictureBox.Image, 28, 28);
            //this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox.Scale(new SizeF(0.1F, 0.1F));
            Bitmap MyBitmap = (Bitmap)pictureBox.Image;

            byte[] imgAsByte = new byte[Len * Len];
            int index = -1;
            for (int i = 0; i < Len; i++)
            {
                for (int j = 0; j < Len; j++)
                {
                    index++;
                    Color color = MyBitmap.GetPixel(j, i);

                    bool isWhite = color.Name.Equals("0") || color.Name.Equals("ffffffff");
                    if (!isWhite)
                    {
                        imgAsByte[index] = 1;
                    }
                }
            }

            return imgAsByte;
        }

        static public Bitmap ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            Bitmap bmp = new Bitmap(newImage);
            return bmp;
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Moving = true;
            X = e.X;
            Y = e.Y;
            //pictureBox.Cursor = Cursors.Cross;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Moving = false;
            X = -1;
            Y = -1;
            //pictureBox.Cursor = Cursors.Default;

            Guess();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Moving && X != -1 && Y != -1)
            {
                //Graphics.DrawLine(Pen, new Point(X, Y), e.Location);
                //X = e.X;
                //Y = e.Y;

                Image image = pictureBox.Image;
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.DrawLine(Pen, new Point(X, Y), e.Location);
                }
                pictureBox.Image = image;
                X = e.X;
                Y = e.Y;

                Guess();
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            W.Train();
        }

        private void btnStopTraining_Click(object sender, EventArgs e)
        {
            W.StopTraining();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TestReport report = W.Test();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            bool isNumber = int.TryParse(txtbObjIndex.Text, out int index);
            if (isNumber)
            {
                byte[] img = W.GetRandomImage(index);
                ApplyByteImg(img);
            }
            else
            {
                MessageBox.Show("In the txtObjIndex is not a number!");
            }
        }

        private void ApplyByteImg(byte[] img)
        {
            Bitmap bitmap = (Bitmap)pictureBox.Image;


            for (int i = 0; i < pictureBox.Image.Size.Height; i++)
            {
                for (int j = 0; j < pictureBox.Image.Size.Width; j++)
                {
                    Color color = img[i * pictureBox.Image.Size.Width + j] == 0 ? Color.White : Color.Black;
                    bitmap.SetPixel(j, i, color);
                }
            }
            pictureBox.Image = bitmap;
        }

        private void btnToTxtFile_Click(object sender, EventArgs e)
        {
            byte[] imgAsByte = GetImgFromPictureBox();

            DataWorker w = new DataWorker();
            w.WriteToFile(imgAsByte, "testImg");

            try
            {
                Process myProcess = new Process();
                Process.Start("notepad++.exe", @"C:\Useful\NN\txt\testImg.txt");
            }
            catch (Exception)
            {
                MessageBox.Show("Could not open file");
            }
        }
    }
}
