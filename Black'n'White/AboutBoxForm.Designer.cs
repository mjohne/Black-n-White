﻿namespace Black_n_White
{
  partial class AboutBoxForm
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBoxForm));
      this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.labelProductName = new System.Windows.Forms.Label();
      this.labelVersion = new System.Windows.Forms.Label();
      this.labelCopyright = new System.Windows.Forms.Label();
      this.labelCompanyName = new System.Windows.Forms.Label();
      this.textBoxDescription = new System.Windows.Forms.TextBox();
      this.okButton = new System.Windows.Forms.Button();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.tableLayoutPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel
      // 
      this.tableLayoutPanel.AccessibleDescription = "Gruppiert die einzelnen GUI-Elemente";
      this.tableLayoutPanel.AccessibleName = "Panel";
      this.tableLayoutPanel.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
      this.tableLayoutPanel.ColumnCount = 2;
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.45638F));
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.54362F));
      this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
      this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
      this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
      this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
      this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
      this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 4);
      this.tableLayoutPanel.Controls.Add(this.okButton, 1, 5);
      this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
      this.tableLayoutPanel.Name = "tableLayoutPanel";
      this.tableLayoutPanel.RowCount = 6;
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.37349F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.07229F));
      this.tableLayoutPanel.Size = new System.Drawing.Size(356, 177);
      this.tableLayoutPanel.TabIndex = 0;
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.AccessibleDescription = "Das ist das Symbol der Anwendung";
      this.logoPictureBox.AccessibleName = "Icon";
      this.logoPictureBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
      this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
      this.logoPictureBox.Location = new System.Drawing.Point(3, 3);
      this.logoPictureBox.Name = "logoPictureBox";
      this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 6);
      this.logoPictureBox.Size = new System.Drawing.Size(59, 171);
      this.logoPictureBox.TabIndex = 12;
      this.logoPictureBox.TabStop = false;
      this.toolTip.SetToolTip(this.logoPictureBox, "Das ist das Symbol der Anwendung");
      // 
      // labelProductName
      // 
      this.labelProductName.AccessibleDescription = "Zeigt den Produktname an";
      this.labelProductName.AccessibleName = "Produktname";
      this.labelProductName.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
      this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelProductName.Location = new System.Drawing.Point(71, 0);
      this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelProductName.Name = "labelProductName";
      this.labelProductName.Size = new System.Drawing.Size(282, 17);
      this.labelProductName.TabIndex = 0;
      this.labelProductName.Text = "Produktname";
      this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTip.SetToolTip(this.labelProductName, "Zeigt den Produktname an");
      // 
      // labelVersion
      // 
      this.labelVersion.AccessibleDescription = "Zeigt die Versionnummer an";
      this.labelVersion.AccessibleName = "Version";
      this.labelVersion.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
      this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelVersion.Location = new System.Drawing.Point(71, 17);
      this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelVersion.Name = "labelVersion";
      this.labelVersion.Size = new System.Drawing.Size(282, 17);
      this.labelVersion.TabIndex = 1;
      this.labelVersion.Text = "Version";
      this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTip.SetToolTip(this.labelVersion, "Zeigt die Versionnummer an");
      // 
      // labelCopyright
      // 
      this.labelCopyright.AccessibleDescription = "Zeigt den Copyright-Vermerk an";
      this.labelCopyright.AccessibleName = "Copyright";
      this.labelCopyright.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
      this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCopyright.Location = new System.Drawing.Point(71, 34);
      this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelCopyright.Name = "labelCopyright";
      this.labelCopyright.Size = new System.Drawing.Size(282, 17);
      this.labelCopyright.TabIndex = 2;
      this.labelCopyright.Text = "Copyright";
      this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTip.SetToolTip(this.labelCopyright, "Zeigt den Copyright-Vermerk an");
      // 
      // labelCompanyName
      // 
      this.labelCompanyName.AccessibleDescription = "Zeigt den Firmenname an";
      this.labelCompanyName.AccessibleName = "Firmenname";
      this.labelCompanyName.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
      this.labelCompanyName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCompanyName.Location = new System.Drawing.Point(71, 51);
      this.labelCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelCompanyName.Name = "labelCompanyName";
      this.labelCompanyName.Size = new System.Drawing.Size(282, 17);
      this.labelCompanyName.TabIndex = 3;
      this.labelCompanyName.Text = "Firmenname";
      this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.toolTip.SetToolTip(this.labelCompanyName, "Zeigt den Firmenname an");
      // 
      // textBoxDescription
      // 
      this.textBoxDescription.AccessibleDescription = "Zeigt die Beschreibung an";
      this.textBoxDescription.AccessibleName = "Beschreibung";
      this.textBoxDescription.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
      this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBoxDescription.Location = new System.Drawing.Point(71, 71);
      this.textBoxDescription.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
      this.textBoxDescription.Multiline = true;
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.ReadOnly = true;
      this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.textBoxDescription.Size = new System.Drawing.Size(282, 69);
      this.textBoxDescription.TabIndex = 4;
      this.textBoxDescription.TabStop = false;
      this.textBoxDescription.Text = "Beschreibung";
      this.toolTip.SetToolTip(this.textBoxDescription, "Zeigt die Beschreibung an");
      // 
      // okButton
      // 
      this.okButton.AccessibleDescription = "Schließt den Dialog";
      this.okButton.AccessibleName = "Okay";
      this.okButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(278, 151);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "&OK";
      this.toolTip.SetToolTip(this.okButton, "Okay");
      // 
      // AboutBoxForm
      // 
      this.AcceptButton = this.okButton;
      this.AccessibleDescription = "Zeigt einige grundlegende Informationen zum Programm an";
      this.AccessibleName = "About-Dialog";
      this.AccessibleRole = System.Windows.Forms.AccessibleRole.Dialog;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(374, 195);
      this.Controls.Add(this.tableLayoutPanel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBoxForm";
      this.Padding = new System.Windows.Forms.Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "AboutBoxForm";
      this.Load += new System.EventHandler(this.AboutBoxForm_Load);
      this.tableLayoutPanel.ResumeLayout(false);
      this.tableLayoutPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    private System.Windows.Forms.Label labelProductName;
    private System.Windows.Forms.Label labelVersion;
    private System.Windows.Forms.Label labelCopyright;
    private System.Windows.Forms.Label labelCompanyName;
    private System.Windows.Forms.TextBox textBoxDescription;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.PictureBox logoPictureBox;
    private System.Windows.Forms.ToolTip toolTip;
  }
}
