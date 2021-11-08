
namespace MineSweeper2Pt8hgxr.View
{
    partial class NewGameDialogBox
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
            this.label1 = new System.Windows.Forms.Label();
            this.boardSizeChooser = new System.Windows.Forms.DomainUpDown();
            this.dialogOKButton = new System.Windows.Forms.Button();
            this.dialogBoxCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Game size";
            // 
            // boardSizeChooser
            // 
            this.boardSizeChooser.Items.Add("Small");
            this.boardSizeChooser.Items.Add("Medium");
            this.boardSizeChooser.Items.Add("Large");
            this.boardSizeChooser.Location = new System.Drawing.Point(120, 33);
            this.boardSizeChooser.Name = "boardSizeChooser";
            this.boardSizeChooser.Size = new System.Drawing.Size(97, 27);
            this.boardSizeChooser.TabIndex = 2;
            this.boardSizeChooser.Text = "Medium";
            // 
            // dialogOKButton
            // 
            this.dialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.dialogOKButton.Location = new System.Drawing.Point(23, 78);
            this.dialogOKButton.Name = "dialogOKButton";
            this.dialogOKButton.Size = new System.Drawing.Size(91, 29);
            this.dialogOKButton.TabIndex = 3;
            this.dialogOKButton.Text = "OK";
            this.dialogOKButton.UseVisualStyleBackColor = true;
            this.dialogOKButton.Click += new System.EventHandler(this.dialogOKButton_Click);
            // 
            // dialogBoxCancelButton
            // 
            this.dialogBoxCancelButton.Location = new System.Drawing.Point(120, 78);
            this.dialogBoxCancelButton.Name = "dialogBoxCancelButton";
            this.dialogBoxCancelButton.Size = new System.Drawing.Size(97, 29);
            this.dialogBoxCancelButton.TabIndex = 4;
            this.dialogBoxCancelButton.Text = "Cancel";
            this.dialogBoxCancelButton.UseVisualStyleBackColor = true;
            this.dialogBoxCancelButton.Click += new System.EventHandler(this.dialogBoxCancelButton_Click);
            // 
            // NewGameDialogBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 121);
            this.Controls.Add(this.dialogBoxCancelButton);
            this.Controls.Add(this.dialogOKButton);
            this.Controls.Add(this.boardSizeChooser);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NewGameDialogBox";
            this.Text = "New Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DomainUpDown boardSizeChooser;
        private System.Windows.Forms.Button dialogOKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button dialogBoxCancelButton;
    }
}