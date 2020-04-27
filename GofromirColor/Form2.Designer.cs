namespace GofromirColor
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.whiteBlackDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extenderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extenderProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yellowProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.redProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rubinProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radominProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orangeProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pinkProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.violetProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.blueProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.greenProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.blackProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.whiteProcDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.viscosityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.col1DataSet = new GofromirColor.col1DataSet();
            this.colorsTableAdapter = new GofromirColor.col1DataSetTableAdapters.ColorsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.col1DataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.colorDataGridViewTextBoxColumn,
            this.whiteBlackDataGridViewTextBoxColumn,
            this.extenderDataGridViewTextBoxColumn,
            this.extenderProcDataGridViewTextBoxColumn,
            this.yellowProcDataGridViewTextBoxColumn,
            this.redProcDataGridViewTextBoxColumn,
            this.rubinProcDataGridViewTextBoxColumn,
            this.radominProcDataGridViewTextBoxColumn,
            this.orangeProcDataGridViewTextBoxColumn,
            this.pinkProcDataGridViewTextBoxColumn,
            this.violetProcDataGridViewTextBoxColumn,
            this.blueProcDataGridViewTextBoxColumn,
            this.greenProcDataGridViewTextBoxColumn,
            this.blackProcDataGridViewTextBoxColumn,
            this.whiteProcDataGridViewTextBoxColumn,
            this.waterDataGridViewTextBoxColumn,
            this.viscosityDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.colorsBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(27, 62);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(761, 150);
            this.dataGridView1.TabIndex = 0;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // colorDataGridViewTextBoxColumn
            // 
            this.colorDataGridViewTextBoxColumn.DataPropertyName = "Color";
            this.colorDataGridViewTextBoxColumn.HeaderText = "Color";
            this.colorDataGridViewTextBoxColumn.Name = "colorDataGridViewTextBoxColumn";
            // 
            // whiteBlackDataGridViewTextBoxColumn
            // 
            this.whiteBlackDataGridViewTextBoxColumn.DataPropertyName = "WhiteBlack";
            this.whiteBlackDataGridViewTextBoxColumn.HeaderText = "По белому/По бурому";
            this.whiteBlackDataGridViewTextBoxColumn.Name = "whiteBlackDataGridViewTextBoxColumn";
            // 
            // extenderDataGridViewTextBoxColumn
            // 
            this.extenderDataGridViewTextBoxColumn.DataPropertyName = "Extender";
            this.extenderDataGridViewTextBoxColumn.HeaderText = "Extender";
            this.extenderDataGridViewTextBoxColumn.Name = "extenderDataGridViewTextBoxColumn";
            // 
            // extenderProcDataGridViewTextBoxColumn
            // 
            this.extenderProcDataGridViewTextBoxColumn.DataPropertyName = "ExtenderProc";
            this.extenderProcDataGridViewTextBoxColumn.HeaderText = "ExtenderProc";
            this.extenderProcDataGridViewTextBoxColumn.Name = "extenderProcDataGridViewTextBoxColumn";
            // 
            // yellowProcDataGridViewTextBoxColumn
            // 
            this.yellowProcDataGridViewTextBoxColumn.DataPropertyName = "YellowProc";
            this.yellowProcDataGridViewTextBoxColumn.HeaderText = "YellowProc";
            this.yellowProcDataGridViewTextBoxColumn.Name = "yellowProcDataGridViewTextBoxColumn";
            // 
            // redProcDataGridViewTextBoxColumn
            // 
            this.redProcDataGridViewTextBoxColumn.DataPropertyName = "RedProc";
            this.redProcDataGridViewTextBoxColumn.HeaderText = "RedProc";
            this.redProcDataGridViewTextBoxColumn.Name = "redProcDataGridViewTextBoxColumn";
            // 
            // rubinProcDataGridViewTextBoxColumn
            // 
            this.rubinProcDataGridViewTextBoxColumn.DataPropertyName = "RubinProc";
            this.rubinProcDataGridViewTextBoxColumn.HeaderText = "RubinProc";
            this.rubinProcDataGridViewTextBoxColumn.Name = "rubinProcDataGridViewTextBoxColumn";
            // 
            // radominProcDataGridViewTextBoxColumn
            // 
            this.radominProcDataGridViewTextBoxColumn.DataPropertyName = "RadominProc";
            this.radominProcDataGridViewTextBoxColumn.HeaderText = "RadominProc";
            this.radominProcDataGridViewTextBoxColumn.Name = "radominProcDataGridViewTextBoxColumn";
            // 
            // orangeProcDataGridViewTextBoxColumn
            // 
            this.orangeProcDataGridViewTextBoxColumn.DataPropertyName = "OrangeProc";
            this.orangeProcDataGridViewTextBoxColumn.HeaderText = "OrangeProc";
            this.orangeProcDataGridViewTextBoxColumn.Name = "orangeProcDataGridViewTextBoxColumn";
            // 
            // pinkProcDataGridViewTextBoxColumn
            // 
            this.pinkProcDataGridViewTextBoxColumn.DataPropertyName = "PinkProc";
            this.pinkProcDataGridViewTextBoxColumn.HeaderText = "PinkProc";
            this.pinkProcDataGridViewTextBoxColumn.Name = "pinkProcDataGridViewTextBoxColumn";
            // 
            // violetProcDataGridViewTextBoxColumn
            // 
            this.violetProcDataGridViewTextBoxColumn.DataPropertyName = "VioletProc";
            this.violetProcDataGridViewTextBoxColumn.HeaderText = "VioletProc";
            this.violetProcDataGridViewTextBoxColumn.Name = "violetProcDataGridViewTextBoxColumn";
            // 
            // blueProcDataGridViewTextBoxColumn
            // 
            this.blueProcDataGridViewTextBoxColumn.DataPropertyName = "BlueProc";
            this.blueProcDataGridViewTextBoxColumn.HeaderText = "BlueProc";
            this.blueProcDataGridViewTextBoxColumn.Name = "blueProcDataGridViewTextBoxColumn";
            // 
            // greenProcDataGridViewTextBoxColumn
            // 
            this.greenProcDataGridViewTextBoxColumn.DataPropertyName = "GreenProc";
            this.greenProcDataGridViewTextBoxColumn.HeaderText = "GreenProc";
            this.greenProcDataGridViewTextBoxColumn.Name = "greenProcDataGridViewTextBoxColumn";
            // 
            // blackProcDataGridViewTextBoxColumn
            // 
            this.blackProcDataGridViewTextBoxColumn.DataPropertyName = "BlackProc";
            this.blackProcDataGridViewTextBoxColumn.HeaderText = "BlackProc";
            this.blackProcDataGridViewTextBoxColumn.Name = "blackProcDataGridViewTextBoxColumn";
            // 
            // whiteProcDataGridViewTextBoxColumn
            // 
            this.whiteProcDataGridViewTextBoxColumn.DataPropertyName = "WhiteProc";
            this.whiteProcDataGridViewTextBoxColumn.HeaderText = "WhiteProc";
            this.whiteProcDataGridViewTextBoxColumn.Name = "whiteProcDataGridViewTextBoxColumn";
            // 
            // waterDataGridViewTextBoxColumn
            // 
            this.waterDataGridViewTextBoxColumn.DataPropertyName = "Water";
            this.waterDataGridViewTextBoxColumn.HeaderText = "Water";
            this.waterDataGridViewTextBoxColumn.Name = "waterDataGridViewTextBoxColumn";
            // 
            // viscosityDataGridViewTextBoxColumn
            // 
            this.viscosityDataGridViewTextBoxColumn.DataPropertyName = "Viscosity";
            this.viscosityDataGridViewTextBoxColumn.HeaderText = "Viscosity";
            this.viscosityDataGridViewTextBoxColumn.Name = "viscosityDataGridViewTextBoxColumn";
            // 
            // colorsBindingSource
            // 
            this.colorsBindingSource.DataMember = "Colors";
            this.colorsBindingSource.DataSource = this.col1DataSet;
            // 
            // col1DataSet
            // 
            this.col1DataSet.DataSetName = "col1DataSet";
            this.col1DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // colorsTableAdapter
            // 
            this.colorsTableAdapter.ClearBeforeFill = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colorsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.col1DataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private col1DataSet col1DataSet;
        private System.Windows.Forms.BindingSource colorsBindingSource;
        private col1DataSetTableAdapters.ColorsTableAdapter colorsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn whiteBlackDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn extenderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn extenderProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yellowProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn redProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rubinProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn radominProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orangeProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pinkProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn violetProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn blueProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn greenProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn blackProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn whiteProcDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waterDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn viscosityDataGridViewTextBoxColumn;
    }
}