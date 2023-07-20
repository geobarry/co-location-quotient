<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMap
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
    Me.panelMapTop = New System.Windows.Forms.Panel()
    Me.cmbObsOrSim = New System.Windows.Forms.ComboBox()
    Me.chkLabels = New System.Windows.Forms.CheckBox()
    Me.panelMapBottom = New System.Windows.Forms.Panel()
    Me.panelMapLeft = New System.Windows.Forms.Panel()
    Me.panelMapRight = New System.Windows.Forms.Panel()
    Me.radZoomOut = New System.Windows.Forms.RadioButton()
    Me.mapMain = New DotSpatial.Controls.Map()
    Me.legendMain = New DotSpatial.Controls.Legend()
    Me.radZoomIn = New System.Windows.Forms.RadioButton()
    Me.btnMapSwitch = New System.Windows.Forms.Button()
    Me.radPan = New System.Windows.Forms.RadioButton()
    Me.panelMapTop.SuspendLayout()
    Me.panelMapLeft.SuspendLayout()
    Me.SuspendLayout()
    '
    'panelMapTop
    '
    Me.panelMapTop.BackColor = System.Drawing.SystemColors.ControlLight
    Me.panelMapTop.Controls.Add(Me.radPan)
    Me.panelMapTop.Controls.Add(Me.radZoomOut)
    Me.panelMapTop.Controls.Add(Me.radZoomIn)
    Me.panelMapTop.Controls.Add(Me.btnMapSwitch)
    Me.panelMapTop.Controls.Add(Me.cmbObsOrSim)
    Me.panelMapTop.Controls.Add(Me.chkLabels)
    Me.panelMapTop.Dock = System.Windows.Forms.DockStyle.Top
    Me.panelMapTop.Location = New System.Drawing.Point(0, 0)
    Me.panelMapTop.Name = "panelMapTop"
    Me.panelMapTop.Size = New System.Drawing.Size(578, 34)
    Me.panelMapTop.TabIndex = 0
    '
    'cmbObsOrSim
    '
    Me.cmbObsOrSim.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmbObsOrSim.FormattingEnabled = True
    Me.cmbObsOrSim.Items.AddRange(New Object() {"Observed Values", "Last Simulation"})
    Me.cmbObsOrSim.Location = New System.Drawing.Point(131, 3)
    Me.cmbObsOrSim.Margin = New System.Windows.Forms.Padding(2)
    Me.cmbObsOrSim.Name = "cmbObsOrSim"
    Me.cmbObsOrSim.Size = New System.Drawing.Size(143, 24)
    Me.cmbObsOrSim.TabIndex = 1
    Me.cmbObsOrSim.Text = "(select label source)"
    Me.cmbObsOrSim.Visible = False
    '
    'chkLabels
    '
    Me.chkLabels.AutoSize = True
    Me.chkLabels.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkLabels.Location = New System.Drawing.Point(0, 5)
    Me.chkLabels.Margin = New System.Windows.Forms.Padding(2)
    Me.chkLabels.Name = "chkLabels"
    Me.chkLabels.Size = New System.Drawing.Size(127, 21)
    Me.chkLabels.TabIndex = 0
    Me.chkLabels.Text = "Label points by:"
    Me.chkLabels.UseVisualStyleBackColor = True
    Me.chkLabels.Visible = False
    '
    'panelMapBottom
    '
    Me.panelMapBottom.BackColor = System.Drawing.SystemColors.ControlLight
    Me.panelMapBottom.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.panelMapBottom.Location = New System.Drawing.Point(0, 401)
    Me.panelMapBottom.Name = "panelMapBottom"
    Me.panelMapBottom.Size = New System.Drawing.Size(578, 29)
    Me.panelMapBottom.TabIndex = 1
    '
    'panelMapLeft
    '
    Me.panelMapLeft.BackColor = System.Drawing.SystemColors.Info
    Me.panelMapLeft.Controls.Add(Me.legendMain)
    Me.panelMapLeft.Dock = System.Windows.Forms.DockStyle.Left
    Me.panelMapLeft.Location = New System.Drawing.Point(0, 34)
    Me.panelMapLeft.Name = "panelMapLeft"
    Me.panelMapLeft.Size = New System.Drawing.Size(173, 367)
    Me.panelMapLeft.TabIndex = 2
    '
    'panelMapRight
    '
    Me.panelMapRight.BackColor = System.Drawing.SystemColors.Info
    Me.panelMapRight.Dock = System.Windows.Forms.DockStyle.Right
    Me.panelMapRight.Location = New System.Drawing.Point(539, 34)
    Me.panelMapRight.Name = "panelMapRight"
    Me.panelMapRight.Size = New System.Drawing.Size(39, 367)
    Me.panelMapRight.TabIndex = 3
    '
    'radZoomOut
    '
    Me.radZoomOut.Appearance = System.Windows.Forms.Appearance.Button
    Me.radZoomOut.BackgroundImage = Global.CLQ.My.Resources.Resources._1334456289_buttons_21
    Me.radZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
    Me.radZoomOut.Dock = System.Windows.Forms.DockStyle.Left
    Me.radZoomOut.FlatAppearance.BorderSize = 0
    Me.radZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.radZoomOut.Location = New System.Drawing.Point(32, 0)
    Me.radZoomOut.Margin = New System.Windows.Forms.Padding(0)
    Me.radZoomOut.Name = "radZoomOut"
    Me.radZoomOut.Size = New System.Drawing.Size(37, 34)
    Me.radZoomOut.TabIndex = 6
    Me.radZoomOut.TabStop = True
    Me.radZoomOut.UseVisualStyleBackColor = True
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
    Me.mapMain.Legend = Me.legendMain
    Me.mapMain.Location = New System.Drawing.Point(173, 34)
    Me.mapMain.Name = "mapMain"
    Me.mapMain.ProgressHandler = Nothing
    Me.mapMain.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt
    Me.mapMain.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt
    Me.mapMain.RedrawLayersWhileResizing = False
    Me.mapMain.SelectionEnabled = True
    Me.mapMain.Size = New System.Drawing.Size(366, 367)
    Me.mapMain.TabIndex = 4
    '
    'legendMain
    '
    Me.legendMain.BackColor = System.Drawing.SystemColors.Info
    Me.legendMain.ControlRectangle = New System.Drawing.Rectangle(0, 0, 173, 367)
    Me.legendMain.Dock = System.Windows.Forms.DockStyle.Fill
    Me.legendMain.DocumentRectangle = New System.Drawing.Rectangle(0, 0, 119, 141)
    Me.legendMain.HorizontalScrollEnabled = True
    Me.legendMain.Indentation = 30
    Me.legendMain.IsInitialized = False
    Me.legendMain.Location = New System.Drawing.Point(0, 0)
    Me.legendMain.MinimumSize = New System.Drawing.Size(5, 5)
    Me.legendMain.Name = "legendMain"
    Me.legendMain.ProgressHandler = Nothing
    Me.legendMain.ResetOnResize = False
    Me.legendMain.SelectionFontColor = System.Drawing.Color.Black
    Me.legendMain.SelectionHighlight = System.Drawing.Color.FromArgb(CType(CType(215, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(252, Byte), Integer))
    Me.legendMain.Size = New System.Drawing.Size(173, 367)
    Me.legendMain.TabIndex = 0
    Me.legendMain.Text = "Legend1"
    Me.legendMain.VerticalScrollEnabled = True
    '
    'radZoomIn
    '
    Me.radZoomIn.Appearance = System.Windows.Forms.Appearance.Button
    Me.radZoomIn.BackgroundImage = Global.CLQ.My.Resources.Resources._1334456369_buttons_19
    Me.radZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
    Me.radZoomIn.Dock = System.Windows.Forms.DockStyle.Left
    Me.radZoomIn.FlatAppearance.BorderSize = 0
    Me.radZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.radZoomIn.Location = New System.Drawing.Point(0, 0)
    Me.radZoomIn.Margin = New System.Windows.Forms.Padding(0)
    Me.radZoomIn.Name = "radZoomIn"
    Me.radZoomIn.Size = New System.Drawing.Size(32, 34)
    Me.radZoomIn.TabIndex = 3
    Me.radZoomIn.TabStop = True
    Me.radZoomIn.UseVisualStyleBackColor = True
    '
    'btnMapSwitch
    '
    Me.btnMapSwitch.Dock = System.Windows.Forms.DockStyle.Right
    Me.btnMapSwitch.FlatAppearance.BorderSize = 0
    Me.btnMapSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnMapSwitch.Image = Global.CLQ.My.Resources.Resources.downLeftTriangle
    Me.btnMapSwitch.Location = New System.Drawing.Point(543, 0)
    Me.btnMapSwitch.Name = "btnMapSwitch"
    Me.btnMapSwitch.Size = New System.Drawing.Size(35, 34)
    Me.btnMapSwitch.TabIndex = 2
    Me.btnMapSwitch.UseVisualStyleBackColor = True
    '
    'radPan
    '
    Me.radPan.Appearance = System.Windows.Forms.Appearance.Button
    Me.radPan.BackgroundImage = Global.CLQ.My.Resources.Resources.Pan
    Me.radPan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
    Me.radPan.Dock = System.Windows.Forms.DockStyle.Left
    Me.radPan.FlatAppearance.BorderSize = 0
    Me.radPan.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.radPan.Location = New System.Drawing.Point(69, 0)
    Me.radPan.Name = "radPan"
    Me.radPan.Size = New System.Drawing.Size(32, 34)
    Me.radPan.TabIndex = 7
    Me.radPan.TabStop = True
    Me.radPan.UseVisualStyleBackColor = True
    '
    'frmMap
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(578, 430)
    Me.ControlBox = False
    Me.Controls.Add(Me.mapMain)
    Me.Controls.Add(Me.panelMapRight)
    Me.Controls.Add(Me.panelMapLeft)
    Me.Controls.Add(Me.panelMapBottom)
    Me.Controls.Add(Me.panelMapTop)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
    Me.Name = "frmMap"
    Me.Text = "Map"
    Me.panelMapTop.ResumeLayout(False)
    Me.panelMapTop.PerformLayout()
    Me.panelMapLeft.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents panelMapTop As System.Windows.Forms.Panel
  Friend WithEvents panelMapBottom As System.Windows.Forms.Panel
  Friend WithEvents panelMapLeft As System.Windows.Forms.Panel
  Friend WithEvents panelMapRight As System.Windows.Forms.Panel
  Friend WithEvents mapMain As DotSpatial.Controls.Map
  Friend WithEvents legendMain As DotSpatial.Controls.Legend
  Friend WithEvents chkLabels As System.Windows.Forms.CheckBox
  Friend WithEvents cmbObsOrSim As System.Windows.Forms.ComboBox
  Friend WithEvents btnMapSwitch As System.Windows.Forms.Button
  Friend WithEvents radZoomIn As System.Windows.Forms.RadioButton
  Friend WithEvents radZoomOut As System.Windows.Forms.RadioButton
  Friend WithEvents radPan As System.Windows.Forms.RadioButton
  '  Friend WithEvents SpatialToolStrip1 As DotSpatial.Controls.SpatialToolStrip
End Class
