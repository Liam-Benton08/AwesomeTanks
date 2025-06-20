namespace AwesomeTanks
{
    partial class LevelDoneScreen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.moneyEarnedLabel = new System.Windows.Forms.Label();
            this.levelOverLabel = new System.Windows.Forms.Label();
            this.continueButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // moneyEarnedLabel
            // 
            this.moneyEarnedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moneyEarnedLabel.ForeColor = System.Drawing.Color.Gold;
            this.moneyEarnedLabel.Location = new System.Drawing.Point(50, 182);
            this.moneyEarnedLabel.Name = "moneyEarnedLabel";
            this.moneyEarnedLabel.Size = new System.Drawing.Size(500, 50);
            this.moneyEarnedLabel.TabIndex = 0;
            this.moneyEarnedLabel.Text = "You Earned :1000";
            this.moneyEarnedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // levelOverLabel
            // 
            this.levelOverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.levelOverLabel.ForeColor = System.Drawing.Color.White;
            this.levelOverLabel.Location = new System.Drawing.Point(50, 34);
            this.levelOverLabel.Name = "levelOverLabel";
            this.levelOverLabel.Size = new System.Drawing.Size(500, 77);
            this.levelOverLabel.TabIndex = 1;
            this.levelOverLabel.Text = "Level Complete";
            this.levelOverLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // continueButton
            // 
            this.continueButton.BackColor = System.Drawing.Color.Transparent;
            this.continueButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.continueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.continueButton.ForeColor = System.Drawing.Color.White;
            this.continueButton.Location = new System.Drawing.Point(160, 316);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(238, 102);
            this.continueButton.TabIndex = 2;
            this.continueButton.Text = "Continue";
            this.continueButton.UseVisualStyleBackColor = false;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // LevelDoneScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.levelOverLabel);
            this.Controls.Add(this.moneyEarnedLabel);
            this.Name = "LevelDoneScreen";
            this.Size = new System.Drawing.Size(600, 600);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label moneyEarnedLabel;
        private System.Windows.Forms.Label levelOverLabel;
        private System.Windows.Forms.Button continueButton;
    }
}
