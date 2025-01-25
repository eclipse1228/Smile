namespace smiletracker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            recordButton = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Location = new Point(0, 0);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(800, 450);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            // 
            // recordButton
            // 
            recordButton.Dock = DockStyle.Bottom;
            recordButton.Location = new Point(0, 427);
            recordButton.Name = "recordButton";
            recordButton.Size = new Size(800, 23);
            recordButton.TabIndex = 1;
            recordButton.Text = "record";
            recordButton.UseVisualStyleBackColor = true;
            recordButton.Click += recordButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(recordButton);
            Controls.Add(pictureBox);
            Name = "Form1";
            Text = "SmileTracker";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ResumeLayout(false);
        }


        #endregion
        
        private PictureBox pictureBox;
        private Button recordButton;
    }
}
