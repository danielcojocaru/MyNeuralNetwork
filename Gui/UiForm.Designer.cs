﻿namespace Gui
{
    partial class UiForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGuess = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnStopTraining = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.lblGuess = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGuess
            // 
            this.btnGuess.Location = new System.Drawing.Point(323, 317);
            this.btnGuess.Name = "btnGuess";
            this.btnGuess.Size = new System.Drawing.Size(116, 39);
            this.btnGuess.TabIndex = 1;
            this.btnGuess.Text = "Guess";
            this.btnGuess.UseVisualStyleBackColor = true;
            this.btnGuess.Click += new System.EventHandler(this.btnGuess_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(28, 28);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(445, 317);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(116, 39);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnStopTraining
            // 
            this.btnStopTraining.Location = new System.Drawing.Point(445, 272);
            this.btnStopTraining.Name = "btnStopTraining";
            this.btnStopTraining.Size = new System.Drawing.Size(116, 39);
            this.btnStopTraining.TabIndex = 4;
            this.btnStopTraining.Text = "Stop Training";
            this.btnStopTraining.UseVisualStyleBackColor = true;
            this.btnStopTraining.Click += new System.EventHandler(this.btnStopTraining_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(323, 272);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(116, 39);
            this.btnTrain.TabIndex = 5;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // lblGuess
            // 
            this.lblGuess.AutoSize = true;
            this.lblGuess.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuess.Location = new System.Drawing.Point(381, 60);
            this.lblGuess.Name = "lblGuess";
            this.lblGuess.Size = new System.Drawing.Size(71, 24);
            this.lblGuess.TabIndex = 6;
            this.lblGuess.Text = "[guess]";
            // 
            // UiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 391);
            this.Controls.Add(this.lblGuess);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.btnStopTraining);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnGuess);
            this.Name = "UiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.UiForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGuess;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnStopTraining;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Label lblGuess;
    }
}

