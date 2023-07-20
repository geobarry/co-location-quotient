<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpatialIndexTestWindow
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
    Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.udNumPts = New System.Windows.Forms.NumericUpDown()
    Me.radSelectPoint = New System.Windows.Forms.RadioButton()
    Me.radAddPoint = New System.Windows.Forms.RadioButton()
    Me.lblInfo = New System.Windows.Forms.Label()
    Me.mapMain = New DotSpatial.Controls.Map()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.lblPtsSearched = New System.Windows.Forms.Label()
    Me.lblRootChecked = New System.Windows.Forms.Label()
    Me.btnAddRandom = New System.Windows.Forms.Button()
    CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SplitContainer1.Panel1.SuspendLayout()
    Me.SplitContainer1.Panel2.SuspendLayout()
    Me.SplitContainer1.SuspendLayout()
    CType(Me.udNumPts, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'SplitContainer1
    '
    Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
    Me.SplitContainer1.Name = "SplitContainer1"
    '
    'SplitContainer1.Panel1
    '
    Me.SplitContainer1.Panel1.Controls.Add(Me.btnAddRandom)
    Me.SplitContainer1.Panel1.Controls.Add(Me.lblRootChecked)
    Me.SplitContainer1.Panel1.Controls.Add(Me.lblPtsSearched)
    Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
    Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
    Me.SplitContainer1.Panel1.Controls.Add(Me.udNumPts)
    Me.SplitContainer1.Panel1.Controls.Add(Me.radSelectPoint)
    Me.SplitContainer1.Panel1.Controls.Add(Me.radAddPoint)
    Me.SplitContainer1.Panel1.Controls.Add(Me.lblInfo)
    '
    'SplitContainer1.Panel2
    '
    Me.SplitContainer1.Panel2.Controls.Add(Me.mapMain)
    Me.SplitContainer1.Size = New System.Drawing.Size(686, 512)
    Me.SplitContainer1.SplitterDistance = 228
    Me.SplitContainer1.TabIndex = 0
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(39, 78)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(62, 17)
    Me.Label1.TabIndex = 5
    Me.Label1.Text = "# points:"
    '
    'udNumPts
    '
    Me.udNumPts.Location = New System.Drawing.Point(107, 76)
    Me.udNumPts.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
    Me.udNumPts.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
    Me.udNumPts.Name = "udNumPts"
    Me.udNumPts.Size = New System.Drawing.Size(51, 22)
    Me.udNumPts.TabIndex = 4
    Me.udNumPts.Value = New Decimal(New Integer() {1, 0, 0, 0})
    '
    'radSelectPoint
    '
    Me.radSelectPoint.AutoSize = True
    Me.radSelectPoint.Location = New System.Drawing.Point(20, 54)
    Me.radSelectPoint.Name = "radSelectPoint"
    Me.radSelectPoint.Size = New System.Drawing.Size(104, 21)
    Me.radSelectPoint.TabIndex = 2
    Me.radSelectPoint.Text = "Select Point"
    Me.radSelectPoint.UseVisualStyleBackColor = True
    '
    'radAddPoint
    '
    Me.radAddPoint.AutoSize = True
    Me.radAddPoint.Checked = True
    Me.radAddPoint.Location = New System.Drawing.Point(20, 27)
    Me.radAddPoint.Name = "radAddPoint"
    Me.radAddPoint.Size = New System.Drawing.Size(90, 21)
    Me.radAddPoint.TabIndex = 1
    Me.radAddPoint.TabStop = True
    Me.radAddPoint.Text = "Add Point"
    Me.radAddPoint.UseVisualStyleBackColor = True
    '
    'lblInfo
    '
    Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
    Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.lblInfo.Location = New System.Drawing.Point(0, 405)
    Me.lblInfo.Name = "lblInfo"
    Me.lblInfo.Size = New System.Drawing.Size(228, 107)
    Me.lblInfo.TabIndex = 0
    Me.lblInfo.Text = "Idle"
    '
    'mapMain
    '
    Me.mapMain.AllowDrop = True
    Me.mapMain.BackColor = System.Drawing.Color.White
    Me.mapMain.CollectAfterDraw = False
    Me.mapMain.CollisionDetection = False
    Me.mapMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.mapMain.ExtendBuffer = False
    Me.mapMain.FunctionMode = DotSpatial.Controls.FunctionMode.None
    Me.mapMain.IsBusy = False
    Me.mapMain.Legend = Nothing
    Me.mapMain.Location = New System.Drawing.Point(0, 0)
    Me.mapMain.Name = "mapMain"
    Me.mapMain.ProgressHandler = Nothing
    Me.mapMain.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt
    Me.mapMain.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt
    Me.mapMain.RedrawLayersWhileResizing = False
    Me.mapMain.SelectionEnabled = True
    Me.mapMain.Size = New System.Drawing.Size(454, 512)
    Me.mapMain.TabIndex = 0
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(20, 105)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(114, 17)
    Me.Label2.TabIndex = 6
    Me.Label2.Text = "Points searched:"
    '
    'lblPtsSearched
    '
    Me.lblPtsSearched.AutoSize = True
    Me.lblPtsSearched.Location = New System.Drawing.Point(143, 105)
    Me.lblPtsSearched.Name = "lblPtsSearched"
    Me.lblPtsSearched.Size = New System.Drawing.Size(28, 17)
    Me.lblPtsSearched.TabIndex = 7
    Me.lblPtsSearched.Text = "n/a"
    '
    'lblRootChecked
    '
    Me.lblRootChecked.AutoSize = True
    Me.lblRootChecked.Location = New System.Drawing.Point(20, 135)
    Me.lblRootChecked.Name = "lblRootChecked"
    Me.lblRootChecked.Size = New System.Drawing.Size(28, 17)
    Me.lblRootChecked.TabIndex = 8
    Me.lblRootChecked.Text = "n/a"
    '
    'btnAddRandom
    '
    Me.btnAddRandom.Location = New System.Drawing.Point(23, 177)
    Me.btnAddRandom.Name = "btnAddRandom"
    Me.btnAddRandom.Size = New System.Drawing.Size(100, 26)
    Me.btnAddRandom.TabIndex = 9
    Me.btnAddRandom.Text = "Add Random Points"
    Me.btnAddRandom.UseVisualStyleBackColor = True
    '
    'SpatialIndexTestWindow
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(686, 512)
    Me.Controls.Add(Me.SplitContainer1)
    Me.Name = "SpatialIndexTestWindow"
    Me.Text = "SpatialIndexTestWindow"
    Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
    Me.SplitContainer1.Panel1.ResumeLayout(False)
    Me.SplitContainer1.Panel1.PerformLayout()
    Me.SplitContainer1.Panel2.ResumeLayout(False)
    CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
    Me.SplitContainer1.ResumeLayout(False)
    CType(Me.udNumPts, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
  Friend WithEvents mapMain As DotSpatial.Controls.Map
  Friend WithEvents lblInfo As System.Windows.Forms.Label
  Friend WithEvents radSelectPoint As System.Windows.Forms.RadioButton
  Friend WithEvents radAddPoint As System.Windows.Forms.RadioButton
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents udNumPts As System.Windows.Forms.NumericUpDown
  Friend WithEvents lblPtsSearched As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents lblRootChecked As System.Windows.Forms.Label
  Friend WithEvents btnAddRandom As System.Windows.Forms.Button
End Class
