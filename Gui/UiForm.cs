using Auxiliar.Other;
using Auxiliar.Worker;
using Auxiliar.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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

        private Pen PenSmall;
        private bool Moving;
        private int X;
        private int Y;

        private void InitializePaint()
        {
            PenSmall = new Pen(Color.Black, 10);
            PenSmall.StartCap = PenSmall.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            ClearImage();
        }

        private void ClearImage()
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height, pictureBox.CreateGraphics());
            pictureBoxBig.Image = new Bitmap(pictureBoxBig.Width, pictureBoxBig.Height, pictureBoxBig.CreateGraphics());
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

        public byte[] GetImgFromPictureBox()
        {
            //Bitmap MyBitmap = ScaleImage(pictureBox.Image, 28, 28);
            //this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox.Scale(new SizeF(0.1F, 0.1F));
            Bitmap myBitmap = (Bitmap)pictureBox.Image;

            //int h = myBitmap.Height;
            //int w = myBitmap.Width;


            byte[] imgAsByte = new byte[Len * Len];
            int index = -1;
            for (int i = 0; i < Len; i++)
            {
                for (int j = 0; j < Len; j++)
                {
                    index++;
                    Color color = myBitmap.GetPixel(j, i);

                    if (!color.IsWhite())
                    {
                        imgAsByte[index] = 1;
                    }
                }
            }

            return imgAsByte;
        }

        

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Moving = true;
            X = e.X;
            Y = e.Y;
            //pictureBoxBig.Cursor = Cursors.Arrow;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Moving = false;
            X = -1;
            Y = -1;
            //pictureBoxBig.Cursor = Cursors.Default;

            //Guess();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Moving && X != -1 && Y != -1)
            {
                //Graphics.DrawLine(Pen, new Point(X, Y), e.Location);
                //X = e.X;
                //Y = e.Y;

                Image image = pictureBoxBig.Image;
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.DrawLine(PenSmall, new Point(X, Y), e.Location);
                }
                pictureBoxBig.Image = image;
                pictureBox.Image = ResizeImage(image, pictureBox.Width, pictureBox.Height);
                MakeGrayPixelsBlack();
                X = e.X;
                Y = e.Y;

                Guess();
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
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

        private void btnToTxtFile_Click(object sender, EventArgs e) => W.ImgToTxtFile();

        private void MakeGrayPixelsBlack()
        {
            Bitmap myBitmap = (Bitmap)pictureBox.Image;

            for (int i = 0; i < Len; i++)
            {
                for (int j = 0; j < Len; j++)
                {
                    Color color = myBitmap.GetPixel(j, i);

                    if (!color.IsWhite())
                    {
                        myBitmap.SetPixel(j, i, Color.Black);
                    }
                }
            }

            pictureBox.Image = myBitmap;
        }

        private void btnToNpyFile_Click(object sender, EventArgs e) => W.ImgToNpyFile();

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int index = int.Parse(txtbObjIndex.Text);
                int count = W.SaveCurrentImgToData(index);
                lblCount.Text = count.ToString();
                ClearImage();
            }
            catch (Exception)
            {
                MessageBox.Show("Dude, that is not a number.");
            }
        }

        private void btnSaveDataToNpy_Click(object sender, EventArgs e) => W.SaveCreatedDataToNpy();

        private void btnMakeGrayBlack_Click(object sender, EventArgs e) => MakeGrayPixelsBlack();

        private void btnRemoveLastInserted_Click(object sender, EventArgs e)
        {
            int index = int.Parse(txtbObjIndex.Text);
            W.RemoveLastInserted(index);
        }

        private void btnCustomStuff_Click(object sender, EventArgs e)
        {
            W.DoCustomStuff();
        }
    }
}
