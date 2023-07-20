<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTable
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.panelTableTop = New System.Windows.Forms.Panel()
    Me.cmbTableFormat = New System.Windows.Forms.ComboBox()
    Me.lblFormat = New System.Windows.Forms.Label()
    Me.numDecimalPlaces = New System.Windows.Forms.NumericUpDown()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.btnTableSwitch = New System.Windows.Forms.Button()
    Me.cmbStatType = New System.Windows.Forms.ComboBox()
    Me.panelTableBottom = New System.Windows.Forms.Panel()
    Me.rtbResults = New System.Windows.Forms.RichTextBox()
    Me.panelTableTop.SuspendLayout()
    CType(Me.numDecimalPlaces, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'panelTableTop
    '
    Me.panelTableTop.Controls.Add(Me.cmbTableFormat)
    Me.panelTableTop.Controls.Add(Me.lblFormat)
    Me.panelTableTop.Controls.Add(Me.numDecimalPlaces)
    Me.panelTableTop.Controls.Add(Me.Label1)
    Me.panelTableTop.Controls.Add(Me.btnTableSwitch)
    Me.panelTableTop.Controls.Add(Me.cmbStatType)
    Me.panelTableTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.panelTableTop.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.panelTableTop.Location = New System.Drawing.Point(0, 0)
    Me.panelTableTop.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.panelTableTop.Name = "panelTableTop"
    Me.panelTableTop.Size = New System.Drawing.Size(1315, 34)
    Me.panelTableTop.TabIndex = 0
    '
    'cmbTableFormat
    '
    Me.cmbTableFormat.Dock = System.Windows.Forms.DockStyle.Left
    Me.cmbTableFormat.FormattingEnabled = True
    Me.cmbTableFormat.Location = New System.Drawing.Point(434, 0)
    Me.cmbTableFormat.Name = "cmbTableFormat"
    Me.cmbTableFormat.Size = New System.Drawing.Size(132, 32)
    Me.cmbTableFormat.TabIndex = 18
    Me.cmbTableFormat.Text = "(format)"
    '
    'lblFormat
    '
    Me.lblFormat.AutoSize = True
    Me.lblFormat.Dock = System.Windows.Forms.DockStyle.Left
    Me.lblFormat.Location = New System.Drawing.Point(361, 0)
    Me.lblFormat.Name = "lblFormat"
    Me.lblFormat.Size = New System.Drawing.Size(73, 24)
    Me.lblFormat.TabIndex = 17
    Me.lblFormat.Text = "Format:"
    '
    'numDecimalPlaces
    '
    Me.numDecimalPlaces.Dock = System.Windows.Forms.DockStyle.Left
    Me.numDecimalPlaces.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.numDecimalPlaces.Location = New System.Drawing.Point(311, 0)
    Me.numDecimalPlaces.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
    Me.numDecimalPlaces.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
    Me.numDecimalPlaces.Name = "numDecimalPlaces"
    Me.numDecimalPlaces.Size = New System.Drawing.Size(50, 32)
    Me.numDecimalPlaces.TabIndex = 15
    Me.numDecimalPlaces.Value = New Decimal(New Integer() {3, 0, 0, 0})
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Dock = System.Windows.Forms.DockStyle.Left
    Me.Label1.Location = New System.Drawing.Point(179, 0)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(132, 24)
    Me.Label1.TabIndex = 14
    Me.Label1.Text = "Decimal Places:"
    Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'btnTableSwitch
    '
    Me.btnTableSwitch.Dock = System.Windows.Forms.DockStyle.Right
    Me.btnTableSwitch.FlatAppearance.BorderSize = 0
    Me.btnTableSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnTableSwitch.Font = New System.Drawing.Font("Wingdings 3", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
    Me.btnTableSwitch.Image = Global.CLQ.My.Resources.Resources.upRightTriangle
    Me.btnTableSwitch.Location = New System.Drawing.Point(1261, 0)
    Me.btnTableSwitch.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
    Me.btnTableSwitch.Name = "btnTableSwitch"
    Me.btnTableSwitch.Size = New System.Drawing.Size(54, 34)
    Me.btnTableSwitch.TabIndex = 6
    Me.btnTableSwitch.UseVisualStyleBackColor = True
    '
    'cmbStatType
    '
    Me.cmbStatType.Dock = System.Windows.Forms.DockStyle.Left
    Me.cmbStatType.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmbStatType.FormattingEnabled = True
    Me.cmbStatType.Location = New System.Drawing.Point(0, 0)
    Me.cmbStatType.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.cmbStatType.Name = "cmbStatType"
    Me.cmbStatType.Size = New System.Drawing.Size(179, 33)
    Me.cmbStatType.TabIndex = 4
    Me.cmbStatType.Text = "(stat type)"
    '
    'panelTableBottom
    '
    Me.panelTableBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelTableBottom.Location = New System.Drawing.Point(0, 508)
    Me.panelTableBottom.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.panelTableBottom.Name = "panelTableBottom"
    Me.panelTableBottom.Size = New System.Drawing.Size(1315, 38)
    Me.panelTableBottom.TabIndex = 1
    Me.panelTableBottom.Visible = False
    '
    'rtbResults
    '
    Me.rtbResults.Dock = System.Windows.Forms.DockStyle.Fill
    Me.rtbResults.Font = New System.Drawing.Font("Consolas", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.rtbResults.Location = New System.Drawing.Point(0, 34)
    Me.rtbResults.Name = "rtbResults"
    Me.rtbResults.Size = New System.Drawing.Size(1315, 474)
    Me.rtbResults.TabIndex = 3
    Me.rtbResults.Text = ""
    Me.rtbResults.WordWrap = False
    '
    'frmTable
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1315, 546)
    Me.ControlBox = False
    Me.Controls.Add(Me.rtbResults)
    Me.Controls.Add(Me.panelTableBottom)
    Me.Controls.Add(Me.panelTableTop)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
    Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
    Me.Name = "frmTable"
    Me.Text = "Results"
    Me.panelTableTop.ResumeLayout(False)
    Me.panelTableTop.PerformLayout()
    CType(Me.numDecimalPlaces, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents panelTableTop As System.Windows.Forms.Panel
  Friend WithEvents panelTableBottom As System.Windows.Forms.Panel
  Friend WithEvents cmbStatType As System.Windows.Forms.ComboBox
  Friend WithEvents btnTableSwitch As System.Windows.Forms.Button
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents numDecimalPlaces As System.Windows.Forms.NumericUpDown
  Friend WithEvents rtbResults As RichTextBox
  Friend WithEvents cmbTableFormat As ComboBox
  Friend WithEvents lblFormat As Label
End Class
