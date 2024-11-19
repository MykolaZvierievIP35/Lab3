namespace Lab3Apps
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnFill;
        private System.Windows.Forms.ListBox lstResults;
        private System.Windows.Forms.Label lblResults;
        
        private System.Windows.Forms.Panel pnlGraphContainer;
        private System.Windows.Forms.Button btnVizualise;
        private System.Windows.Forms.PictureBox pictureBoxGraph;
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            this.lblKey = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.lblData = new System.Windows.Forms.Label();
            this.txtData = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnFill = new System.Windows.Forms.Button();
            this.lstResults = new System.Windows.Forms.ListBox();
            this.lblResults = new System.Windows.Forms.Label();
            
            this.pnlGraphContainer = new System.Windows.Forms.Panel();
            this.pictureBoxGraph = new System.Windows.Forms.PictureBox();
            this.btnVizualise = new System.Windows.Forms.Button();
            this.pnlGraphContainer.Controls.Add(this.pictureBoxGraph);
            
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 1000);
            this.Text = "Red-Black Tree Database";
            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblKey, txtKey, lblData, txtData,
                btnAdd, btnSearch, btnEdit, btnDelete,
                btnClear, btnRefresh, btnFill, lblResults, lstResults, pnlGraphContainer, btnVizualise
            });
            
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(20, 20);
            this.lblKey.Name = "lblKey";
            this.lblKey.Text = "Key:";
            
            this.txtKey.Location = new System.Drawing.Point(100, 20);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(200, 27);
            
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(20, 60);
            this.lblData.Name = "lblData";
            this.lblData.Text = "Data:";
            
            this.txtData.Location = new System.Drawing.Point(100, 60);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(200, 27);
            
            this.btnAdd.Location = new System.Drawing.Point(320, 20);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            
            this.btnSearch.Location = new System.Drawing.Point(320, 60);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 30);
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            
            this.btnEdit.Location = new System.Drawing.Point(320, 100);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 30);
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            
            this.btnDelete.Location = new System.Drawing.Point(320, 140);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            
            this.btnClear.Location = new System.Drawing.Point(320, 180);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 30);
            this.btnClear.Text = "Clear DB";
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            
            this.btnRefresh.Location = new System.Drawing.Point(320, 220);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            
            this.btnFill.Location = new System.Drawing.Point(320, 260);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(100, 30);
            this.btnFill.Text = "Fill";
            this.btnFill.Click += new System.EventHandler(this.BtnFill_Click);
            
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(20, 2600);
            this.lblResults.Name = "lblResults";
            this.lblResults.Text = "Results:";
            
            this.lstResults.Location = new System.Drawing.Point(20, 340);
            this.lstResults.Name = "lstResults";
            this.lstResults.Size = new System.Drawing.Size(400, 250);
            this.lstResults.DoubleClick += new System.EventHandler(this.LstResults_DoubleClick);
            
            this.btnVizualise.Location = new System.Drawing.Point(320, 300);
            this.btnVizualise.Name = "btnVizualise";
            this.btnVizualise.Size = new System.Drawing.Size(100, 30);
            this.btnVizualise.Text = "Visualise";
            this.btnVizualise.Click += new System.EventHandler(this.BtnVisualize_Click);
            
            this.pnlGraphContainer.Location = new System.Drawing.Point(470, 300);
            this.pnlGraphContainer.Name = "pnlGraphContainer";
            this.pnlGraphContainer.Size = new System.Drawing.Size(900, 600);
            this.pnlGraphContainer.AutoScroll = true;
            this.pnlGraphContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            
            this.pictureBoxGraph.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxGraph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        }
    }
}
