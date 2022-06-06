namespace RTCV_Plugin_Sym2VMD.UI
{

    partial class PluginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginForm));
            this.label5 = new System.Windows.Forms.Label();
            this.cbSymbolFormat = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.cbGenMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMDName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(12, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 200;
            this.label5.Text = "Symbol Map Format";
            // 
            // cbSymbolFormat
            // 
            this.cbSymbolFormat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbSymbolFormat.DisplayMember = "Value";
            this.cbSymbolFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSymbolFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSymbolFormat.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbSymbolFormat.ForeColor = System.Drawing.Color.White;
            this.cbSymbolFormat.FormattingEnabled = true;
            this.cbSymbolFormat.IntegralHeight = false;
            this.cbSymbolFormat.Location = new System.Drawing.Point(12, 31);
            this.cbSymbolFormat.MaxDropDownItems = 15;
            this.cbSymbolFormat.Name = "cbSymbolFormat";
            this.cbSymbolFormat.Size = new System.Drawing.Size(173, 21);
            this.cbSymbolFormat.TabIndex = 199;
            this.cbSymbolFormat.Tag = "color:normal";
            this.cbSymbolFormat.ValueMember = "Value";
            this.cbSymbolFormat.SelectedIndexChanged += new System.EventHandler(this.cbSymbolFormat_SelectedIndexChanged);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.BackColor = System.Drawing.Color.Gray;
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Location = new System.Drawing.Point(200, 159);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(73, 50);
            this.btnGenerate.TabIndex = 204;
            this.btnGenerate.TabStop = false;
            this.btnGenerate.Tag = "color:light1";
            this.btnGenerate.Text = "Generate VMDs";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cbGenMode
            // 
            this.cbGenMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.cbGenMode.DisplayMember = "Value";
            this.cbGenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGenMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbGenMode.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbGenMode.ForeColor = System.Drawing.Color.White;
            this.cbGenMode.FormattingEnabled = true;
            this.cbGenMode.IntegralHeight = false;
            this.cbGenMode.Location = new System.Drawing.Point(12, 87);
            this.cbGenMode.MaxDropDownItems = 15;
            this.cbGenMode.Name = "cbGenMode";
            this.cbGenMode.Size = new System.Drawing.Size(173, 21);
            this.cbGenMode.TabIndex = 199;
            this.cbGenMode.Tag = "color:normal";
            this.cbGenMode.ValueMember = "Value";
            this.cbGenMode.SelectedIndexChanged += new System.EventHandler(this.cbGenMode_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 200;
            this.label1.Text = "Generation Mode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 200;
            this.label2.Text = "Memory Domain Name";
            // 
            // tbMDName
            // 
            this.tbMDName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.tbMDName.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbMDName.ForeColor = System.Drawing.Color.White;
            this.tbMDName.Location = new System.Drawing.Point(12, 138);
            this.tbMDName.Name = "tbMDName";
            this.tbMDName.Size = new System.Drawing.Size(173, 22);
            this.tbMDName.TabIndex = 205;
            this.tbMDName.TextChanged += new System.EventHandler(this.tbMDName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 200;
            this.label3.Text = "Symbol Filter";
            // 
            // tbFilter
            // 
            this.tbFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.tbFilter.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbFilter.ForeColor = System.Drawing.Color.White;
            this.tbFilter.Location = new System.Drawing.Point(12, 188);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(173, 22);
            this.tbFilter.TabIndex = 205;
            this.tbFilter.TextChanged += new System.EventHandler(this.tbMDName_TextChanged);
            // 
            // PluginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(285, 221);
            this.Controls.Add(this.tbFilter);
            this.Controls.Add(this.tbMDName);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbGenMode);
            this.Controls.Add(this.cbSymbolFormat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PluginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "color:dark1";
            this.Text = "Plugin Form";
            this.Load += new System.EventHandler(this.PluginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.ComboBox cbSymbolFormat;
        public System.Windows.Forms.Button btnGenerate;
        public System.Windows.Forms.ComboBox cbGenMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMDName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFilter;
    }
}
