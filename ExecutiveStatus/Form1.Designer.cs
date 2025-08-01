namespace ExecutiveStatus
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
            IbIName = new Label();
            cmbReason = new ComboBox();
            IbIStatus = new Label();
            rdoPresent = new RadioButton();
            rdoAbsent = new RadioButton();
            IbIReason = new Label();
            IbIMemo = new Label();
            txtMemo = new TextBox();
            btnSave = new Button();
            txtName = new TextBox();
            listViewStatus = new ListView();
            이름 = new ColumnHeader();
            상태 = new ColumnHeader();
            사유 = new ColumnHeader();
            메모 = new ColumnHeader();
            btnEdit = new Button();
            btnDelete = new Button();
            SuspendLayout();
            // 
            // IbIName
            // 
            IbIName.AutoSize = true;
            IbIName.Location = new Point(0, 0);
            IbIName.Name = "IbIName";
            IbIName.Size = new Size(34, 15);
            IbIName.TabIndex = 0;
            IbIName.Text = "이름:";
            // 
            // cmbReason
            // 
            cmbReason.Location = new Point(40, 64);
            cmbReason.Name = "cmbReason";
            cmbReason.Size = new Size(100, 23);
            cmbReason.TabIndex = 1;
            // 
            // IbIStatus
            // 
            IbIStatus.AutoSize = true;
            IbIStatus.Location = new Point(0, 36);
            IbIStatus.Name = "IbIStatus";
            IbIStatus.Size = new Size(34, 15);
            IbIStatus.TabIndex = 2;
            IbIStatus.Text = "상태:";
            // 
            // rdoPresent
            // 
            rdoPresent.AutoSize = true;
            rdoPresent.Location = new Point(40, 34);
            rdoPresent.Name = "rdoPresent";
            rdoPresent.Size = new Size(49, 19);
            rdoPresent.TabIndex = 3;
            rdoPresent.TabStop = true;
            rdoPresent.Text = "재실";
            rdoPresent.UseVisualStyleBackColor = true;
            rdoPresent.CheckedChanged += rdoPresent_CheckedChanged;
            // 
            // rdoAbsent
            // 
            rdoAbsent.AutoSize = true;
            rdoAbsent.Location = new Point(95, 34);
            rdoAbsent.Name = "rdoAbsent";
            rdoAbsent.Size = new Size(49, 19);
            rdoAbsent.TabIndex = 4;
            rdoAbsent.TabStop = true;
            rdoAbsent.Text = "부재";
            rdoAbsent.UseVisualStyleBackColor = true;
            // 
            // IbIReason
            // 
            IbIReason.AutoSize = true;
            IbIReason.Location = new Point(0, 70);
            IbIReason.Name = "IbIReason";
            IbIReason.Size = new Size(34, 15);
            IbIReason.TabIndex = 5;
            IbIReason.Text = "사유:";
            // 
            // IbIMemo
            // 
            IbIMemo.AutoSize = true;
            IbIMemo.Location = new Point(0, 106);
            IbIMemo.Name = "IbIMemo";
            IbIMemo.Size = new Size(34, 15);
            IbIMemo.TabIndex = 7;
            IbIMemo.Text = "메모:";
            // 
            // txtMemo
            // 
            txtMemo.Location = new Point(40, 103);
            txtMemo.Name = "txtMemo";
            txtMemo.Size = new Size(100, 23);
            txtMemo.TabIndex = 8;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(0, 132);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 9;
            btnSave.Text = "저장";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtName
            // 
            txtName.Location = new Point(40, 0);
            txtName.Name = "txtName";
            txtName.Size = new Size(100, 23);
            txtName.TabIndex = 10;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // listViewStatus
            // 
            listViewStatus.Columns.AddRange(new ColumnHeader[] { 이름, 상태, 사유, 메모 });
            listViewStatus.FullRowSelect = true;
            listViewStatus.GridLines = true;
            listViewStatus.Location = new Point(0, 170);
            listViewStatus.Name = "listViewStatus";
            listViewStatus.Size = new Size(245, 200);
            listViewStatus.TabIndex = 0;
            listViewStatus.UseCompatibleStateImageBehavior = false;
            listViewStatus.View = View.Details;
            listViewStatus.SelectedIndexChanged += listViewStatus_SelectedIndexChanged;
            // 
            // 이름
            // 
            이름.Text = "이름";
            // 
            // 상태
            // 
            상태.Text = "상태";
            // 
            // 사유
            // 
            사유.Text = "사유";
            // 
            // 메모
            // 
            메모.Text = "메모";
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(81, 132);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(75, 23);
            btnEdit.TabIndex = 11;
            btnEdit.Text = "수정";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(162, 132);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "삭제";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(listViewStatus);
            Controls.Add(txtName);
            Controls.Add(btnSave);
            Controls.Add(txtMemo);
            Controls.Add(IbIMemo);
            Controls.Add(IbIReason);
            Controls.Add(rdoAbsent);
            Controls.Add(rdoPresent);
            Controls.Add(IbIStatus);
            Controls.Add(cmbReason);
            Controls.Add(IbIName);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label IbIName;
        private ComboBox cmbReason;
        private Label IbIStatus;
        private RadioButton rdoPresent;
        private RadioButton rdoAbsent;
        private Label IbIReason;
        private Label IbIMemo;
        private TextBox txtMemo;
        private Button btnSave;
        private TextBox txtName;
        private ListView listViewStatus;
        private ColumnHeader 이름;
        private ColumnHeader 상태;
        private ColumnHeader 사유;
        private ColumnHeader 메모;
        private Button btnEdit;
        private Button btnDelete;
    }
}
