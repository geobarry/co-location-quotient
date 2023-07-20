<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmControl
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
    Me.lblProgress = New System.Windows.Forms.Label()
    Me.flowSteps = New System.Windows.Forms.FlowLayoutPanel()
    Me.panelLoadData = New System.Windows.Forms.Panel()
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.lblStudyArea = New System.Windows.Forms.Label()
    Me.btnLoadStudyArea = New System.Windows.Forms.Button()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.btnLoadPopulationShapefile = New System.Windows.Forms.Button()
    Me.cmbFields = New System.Windows.Forms.ComboBox()
    Me.lblNumPts = New System.Windows.Forms.Label()
    Me.btnBatchProcess = New System.Windows.Forms.Button()
    Me.btnLoadAuxiliaryData = New System.Windows.Forms.Button()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.btnLoadDataToggle = New System.Windows.Forms.Button()
    Me.pnlGroups = New System.Windows.Forms.Panel()
    Me.tabClasses = New System.Windows.Forms.TabControl()
    Me.tabObsClasses = New System.Windows.Forms.TabPage()
    Me.dgvClasses = New System.Windows.Forms.DataGridView()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.tabAggClasses = New System.Windows.Forms.TabPage()
    Me.dgvAggClasses = New System.Windows.Forms.DataGridView()
    Me.btnGroupToggle = New System.Windows.Forms.Button()
    Me.pnlNeighborhood = New System.Windows.Forms.Panel()
    Me.cmbDecayFunction = New System.Windows.Forms.ComboBox()
    Me.lblNeighborhoodType = New System.Windows.Forms.Label()
    Me.lblRadiusUnits = New System.Windows.Forms.Label()
    Me.cmbNeighborhoodType = New System.Windows.Forms.ComboBox()
    Me.txtRadius = New System.Windows.Forms.TextBox()
    Me.lblDecayFunction = New System.Windows.Forms.Label()
    Me.btnNeighborhoodToggle = New System.Windows.Forms.Button()
    Me.lblRadius = New System.Windows.Forms.Label()
    Me.pnlNullModel = New System.Windows.Forms.Panel()
    Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
    Me.radioCSR = New System.Windows.Forms.RadioButton()
    Me.radioToroidalShift = New System.Windows.Forms.RadioButton()
    Me.radioRL = New System.Windows.Forms.RadioButton()
    Me.radioCRL = New System.Windows.Forms.RadioButton()
    Me.panelRestrictionOptions = New System.Windows.Forms.FlowLayoutPanel()
    Me.Label6 = New System.Windows.Forms.Label()
    Me.radFixedBase = New System.Windows.Forms.RadioButton()
    Me.radFixedNeighbor = New System.Windows.Forms.RadioButton()
    Me.radDesignateFixed = New System.Windows.Forms.RadioButton()
    Me.listFixed = New System.Windows.Forms.ListBox()
    Me.radFixedOther = New System.Windows.Forms.RadioButton()
    Me.btnStatParamToggle = New System.Windows.Forms.Button()
    Me.pnlExecution = New System.Windows.Forms.Panel()
    Me.btnGetStdDev = New System.Windows.Forms.Button()
    Me.btnStopSim = New System.Windows.Forms.Button()
    Me.btnSaveSim = New System.Windows.Forms.Button()
    Me.btnCompareNullModels = New System.Windows.Forms.Button()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.btnResetSims = New System.Windows.Forms.Button()
    Me.lblSimCompleted = New System.Windows.Forms.Label()
    Me.udNumSims = New System.Windows.Forms.NumericUpDown()
    Me.btnRunMany = New System.Windows.Forms.Button()
    Me.btnRunOnce = New System.Windows.Forms.Button()
    Me.btnExecution = New System.Windows.Forms.Button()
    Me.PanelResults = New System.Windows.Forms.Panel()
    Me.grpDisplayStat = New System.Windows.Forms.GroupBox()
    Me.radDisplayNNCT = New System.Windows.Forms.RadioButton()
    Me.radDisplayCLQ = New System.Windows.Forms.RadioButton()
    Me.grpSource = New System.Windows.Forms.GroupBox()
    Me.cmbSimNum = New System.Windows.Forms.ComboBox()
    Me.radSrcSimulation = New System.Windows.Forms.RadioButton()
    Me.radSrcData = New System.Windows.Forms.RadioButton()
    Me.btnResultsToggle = New System.Windows.Forms.Button()
    Me.pnlComputeCoLocation = New System.Windows.Forms.Panel()
    Me.btnCalculateCLQ = New System.Windows.Forms.Button()
    Me.btnCalculationToggle = New System.Windows.Forms.Button()
    Me.flowSteps.SuspendLayout()
    Me.panelLoadData.SuspendLayout()
    Me.TableLayoutPanel1.SuspendLayout()
    Me.pnlGroups.SuspendLayout()
    Me.tabClasses.SuspendLayout()
    Me.tabObsClasses.SuspendLayout()
    CType(Me.dgvClasses, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.tabAggClasses.SuspendLayout()
    CType(Me.dgvAggClasses, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.pnlNeighborhood.SuspendLayout()
    Me.pnlNullModel.SuspendLayout()
    Me.FlowLayoutPanel1.SuspendLayout()
    Me.panelRestrictionOptions.SuspendLayout()
    Me.pnlExecution.SuspendLayout()
    CType(Me.udNumSims, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.PanelResults.SuspendLayout()
    Me.grpDisplayStat.SuspendLayout()
    Me.grpSource.SuspendLayout()
    Me.pnlComputeCoLocation.SuspendLayout()
    Me.SuspendLayout()
    '
    'lblProgress
    '
    Me.lblProgress.BackColor = System.Drawing.Color.Ivory
    Me.lblProgress.Dock = System.Windows.Forms.DockStyle.Bottom
    Me.lblProgress.Location = New System.Drawing.Point(0, 717)
    Me.lblProgress.Name = "lblProgress"
    Me.lblProgress.Size = New System.Drawing.Size(1081, 69)
    Me.lblProgress.TabIndex = 0
    Me.lblProgress.Text = "- idle -"
    '
    'flowSteps
    '
    Me.flowSteps.BackColor = System.Drawing.Color.Transparent
    Me.flowSteps.Controls.Add(Me.panelLoadData)
    Me.flowSteps.Controls.Add(Me.pnlGroups)
    Me.flowSteps.Controls.Add(Me.pnlNeighborhood)
    Me.flowSteps.Controls.Add(Me.pnlNullModel)
    Me.flowSteps.Controls.Add(Me.pnlExecution)
    Me.flowSteps.Controls.Add(Me.PanelResults)
    Me.flowSteps.Controls.Add(Me.pnlComputeCoLocation)
    Me.flowSteps.Dock = System.Windows.Forms.DockStyle.Fill
    Me.flowSteps.Location = New System.Drawing.Point(0, 0)
    Me.flowSteps.Margin = New System.Windows.Forms.Padding(0)
    Me.flowSteps.Name = "flowSteps"
    Me.flowSteps.Size = New System.Drawing.Size(1081, 717)
    Me.flowSteps.TabIndex = 1
    '
    'panelLoadData
    '
    Me.panelLoadData.BackColor = System.Drawing.Color.Wheat
    Me.panelLoadData.Controls.Add(Me.TableLayoutPanel1)
    Me.panelLoadData.Controls.Add(Me.btnBatchProcess)
    Me.panelLoadData.Controls.Add(Me.btnLoadAuxiliaryData)
    Me.panelLoadData.Controls.Add(Me.Label3)
    Me.panelLoadData.Controls.Add(Me.btnLoadDataToggle)
    Me.panelLoadData.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.panelLoadData.Location = New System.Drawing.Point(0, 0)
    Me.panelLoadData.Margin = New System.Windows.Forms.Padding(0)
    Me.panelLoadData.Name = "panelLoadData"
    Me.panelLoadData.Size = New System.Drawing.Size(373, 213)
    Me.panelLoadData.TabIndex = 0
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.AutoSize = True
    Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.TableLayoutPanel1.ColumnCount = 2
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
    Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.lblStudyArea, 0, 3)
    Me.TableLayoutPanel1.Controls.Add(Me.btnLoadStudyArea, 1, 3)
    Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 2)
    Me.TableLayoutPanel1.Controls.Add(Me.btnLoadPopulationShapefile, 1, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.cmbFields, 1, 2)
    Me.TableLayoutPanel1.Controls.Add(Me.lblNumPts, 1, 1)
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(4, 44)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 4
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(375, 174)
    Me.TableLayoutPanel1.TabIndex = 13
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label1.Location = New System.Drawing.Point(4, 0)
    Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(173, 28)
    Me.Label1.TabIndex = 2
    Me.Label1.Text = "Population Data:"
    '
    'lblStudyArea
    '
    Me.lblStudyArea.AutoSize = True
    Me.lblStudyArea.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblStudyArea.Location = New System.Drawing.Point(3, 110)
    Me.lblStudyArea.Name = "lblStudyArea"
    Me.TableLayoutPanel1.SetRowSpan(Me.lblStudyArea, 2)
    Me.lblStudyArea.Size = New System.Drawing.Size(218, 56)
    Me.lblStudyArea.TabIndex = 12
    Me.lblStudyArea.Text = "Study Area (optional)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(CSR or TS):"
    '
    'btnLoadStudyArea
    '
    Me.btnLoadStudyArea.AutoSize = True
    Me.btnLoadStudyArea.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.btnLoadStudyArea.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnLoadStudyArea.Location = New System.Drawing.Point(227, 113)
    Me.btnLoadStudyArea.Name = "btnLoadStudyArea"
    Me.btnLoadStudyArea.Size = New System.Drawing.Size(71, 38)
    Me.btnLoadStudyArea.TabIndex = 2
    Me.btnLoadStudyArea.Text = "Load"
    Me.btnLoadStudyArea.UseVisualStyleBackColor = True
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label2.Location = New System.Drawing.Point(4, 66)
    Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(156, 28)
    Me.Label2.TabIndex = 3
    Me.Label2.Text = "Category Field:"
    '
    'btnLoadPopulationShapefile
    '
    Me.btnLoadPopulationShapefile.AutoSize = True
    Me.btnLoadPopulationShapefile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.btnLoadPopulationShapefile.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnLoadPopulationShapefile.Location = New System.Drawing.Point(228, 4)
    Me.btnLoadPopulationShapefile.Margin = New System.Windows.Forms.Padding(4)
    Me.btnLoadPopulationShapefile.Name = "btnLoadPopulationShapefile"
    Me.btnLoadPopulationShapefile.Size = New System.Drawing.Size(71, 38)
    Me.btnLoadPopulationShapefile.TabIndex = 0
    Me.btnLoadPopulationShapefile.Text = "Load"
    Me.btnLoadPopulationShapefile.UseVisualStyleBackColor = True
    '
    'cmbFields
    '
    Me.cmbFields.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmbFields.FormattingEnabled = True
    Me.cmbFields.ItemHeight = 28
    Me.cmbFields.Location = New System.Drawing.Point(228, 70)
    Me.cmbFields.Margin = New System.Windows.Forms.Padding(4)
    Me.cmbFields.Name = "cmbFields"
    Me.cmbFields.Size = New System.Drawing.Size(143, 36)
    Me.cmbFields.TabIndex = 1
    '
    'lblNumPts
    '
    Me.lblNumPts.AutoSize = True
    Me.lblNumPts.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNumPts.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.lblNumPts.Location = New System.Drawing.Point(228, 46)
    Me.lblNumPts.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
    Me.lblNumPts.Name = "lblNumPts"
    Me.lblNumPts.Size = New System.Drawing.Size(115, 20)
    Me.lblNumPts.TabIndex = 10
    Me.lblNumPts.Text = "0 pts loaded"
    Me.lblNumPts.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'btnBatchProcess
    '
    Me.btnBatchProcess.Location = New System.Drawing.Point(231, 405)
    Me.btnBatchProcess.Name = "btnBatchProcess"
    Me.btnBatchProcess.Size = New System.Drawing.Size(97, 60)
    Me.btnBatchProcess.TabIndex = 11
    Me.btnBatchProcess.Text = "Batch Process (private)"
    Me.btnBatchProcess.UseVisualStyleBackColor = True
    Me.btnBatchProcess.Visible = False
    '
    'btnLoadAuxiliaryData
    '
    Me.btnLoadAuxiliaryData.Location = New System.Drawing.Point(206, 369)
    Me.btnLoadAuxiliaryData.Margin = New System.Windows.Forms.Padding(4)
    Me.btnLoadAuxiliaryData.Name = "btnLoadAuxiliaryData"
    Me.btnLoadAuxiliaryData.Size = New System.Drawing.Size(122, 29)
    Me.btnLoadAuxiliaryData.TabIndex = 7
    Me.btnLoadAuxiliaryData.Text = "Load Shapefile"
    Me.btnLoadAuxiliaryData.UseVisualStyleBackColor = True
    Me.btnLoadAuxiliaryData.Visible = False
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(71, 374)
    Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(138, 26)
    Me.Label3.TabIndex = 6
    Me.Label3.Text = "Auxiliary Data:"
    Me.Label3.Visible = False
    '
    'btnLoadDataToggle
    '
    Me.btnLoadDataToggle.AutoSize = True
    Me.btnLoadDataToggle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.btnLoadDataToggle.Dock = System.Windows.Forms.DockStyle.Top
    Me.btnLoadDataToggle.FlatAppearance.BorderSize = 0
    Me.btnLoadDataToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnLoadDataToggle.Font = New System.Drawing.Font("Palatino Linotype", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnLoadDataToggle.Location = New System.Drawing.Point(0, 0)
    Me.btnLoadDataToggle.Margin = New System.Windows.Forms.Padding(4)
    Me.btnLoadDataToggle.Name = "btnLoadDataToggle"
    Me.btnLoadDataToggle.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
    Me.btnLoadDataToggle.Size = New System.Drawing.Size(373, 42)
    Me.btnLoadDataToggle.TabIndex = 0
    Me.btnLoadDataToggle.Text = "Data"
    Me.btnLoadDataToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.btnLoadDataToggle.UseVisualStyleBackColor = True
    '
    'pnlGroups
    '
    Me.pnlGroups.BackColor = System.Drawing.Color.LightYellow
    Me.pnlGroups.Controls.Add(Me.tabClasses)
    Me.pnlGroups.Controls.Add(Me.btnGroupToggle)
    Me.pnlGroups.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlGroups.Location = New System.Drawing.Point(373, 0)
    Me.pnlGroups.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlGroups.Name = "pnlGroups"
    Me.pnlGroups.Size = New System.Drawing.Size(520, 130)
    Me.pnlGroups.TabIndex = 4
    '
    'tabClasses
    '
    Me.tabClasses.Controls.Add(Me.tabObsClasses)
    Me.tabClasses.Controls.Add(Me.tabAggClasses)
    Me.tabClasses.Location = New System.Drawing.Point(0, 46)
    Me.tabClasses.Margin = New System.Windows.Forms.Padding(4)
    Me.tabClasses.Name = "tabClasses"
    Me.tabClasses.SelectedIndex = 0
    Me.tabClasses.Size = New System.Drawing.Size(497, 214)
    Me.tabClasses.TabIndex = 11
    '
    'tabObsClasses
    '
    Me.tabObsClasses.Controls.Add(Me.dgvClasses)
    Me.tabObsClasses.Controls.Add(Me.Label4)
    Me.tabObsClasses.Location = New System.Drawing.Point(4, 33)
    Me.tabObsClasses.Margin = New System.Windows.Forms.Padding(4)
    Me.tabObsClasses.Name = "tabObsClasses"
    Me.tabObsClasses.Padding = New System.Windows.Forms.Padding(4)
    Me.tabObsClasses.Size = New System.Drawing.Size(489, 177)
    Me.tabObsClasses.TabIndex = 0
    Me.tabObsClasses.Text = "Observed Classes"
    Me.tabObsClasses.UseVisualStyleBackColor = True
    '
    'dgvClasses
    '
    Me.dgvClasses.AllowUserToAddRows = False
    Me.dgvClasses.AllowUserToDeleteRows = False
    Me.dgvClasses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
    Me.dgvClasses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
    Me.dgvClasses.Dock = System.Windows.Forms.DockStyle.Fill
    Me.dgvClasses.Location = New System.Drawing.Point(4, 30)
    Me.dgvClasses.Margin = New System.Windows.Forms.Padding(4)
    Me.dgvClasses.Name = "dgvClasses"
    Me.dgvClasses.RowHeadersWidth = 72
    Me.dgvClasses.RowTemplate.Height = 24
    Me.dgvClasses.Size = New System.Drawing.Size(481, 143)
    Me.dgvClasses.TabIndex = 10
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
    Me.Label4.Location = New System.Drawing.Point(4, 4)
    Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(384, 26)
    Me.Label4.TabIndex = 9
    Me.Label4.Text = "To aggregate classes, edit right-hand column:"
    '
    'tabAggClasses
    '
    Me.tabAggClasses.Controls.Add(Me.dgvAggClasses)
    Me.tabAggClasses.Location = New System.Drawing.Point(4, 33)
    Me.tabAggClasses.Margin = New System.Windows.Forms.Padding(4)
    Me.tabAggClasses.Name = "tabAggClasses"
    Me.tabAggClasses.Padding = New System.Windows.Forms.Padding(4)
    Me.tabAggClasses.Size = New System.Drawing.Size(489, 177)
    Me.tabAggClasses.TabIndex = 1
    Me.tabAggClasses.Text = "Aggregated Classes"
    Me.tabAggClasses.UseVisualStyleBackColor = True
    '
    'dgvAggClasses
    '
    Me.dgvAggClasses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
    Me.dgvAggClasses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
    Me.dgvAggClasses.Dock = System.Windows.Forms.DockStyle.Fill
    Me.dgvAggClasses.Location = New System.Drawing.Point(4, 4)
    Me.dgvAggClasses.Margin = New System.Windows.Forms.Padding(4)
    Me.dgvAggClasses.Name = "dgvAggClasses"
    Me.dgvAggClasses.RowHeadersWidth = 72
    Me.dgvAggClasses.RowTemplate.Height = 24
    Me.dgvAggClasses.Size = New System.Drawing.Size(481, 169)
    Me.dgvAggClasses.TabIndex = 0
    '
    'btnGroupToggle
    '
    Me.btnGroupToggle.Dock = System.Windows.Forms.DockStyle.Top
    Me.btnGroupToggle.FlatAppearance.BorderSize = 0
    Me.btnGroupToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnGroupToggle.Font = New System.Drawing.Font("Palatino Linotype", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnGroupToggle.Location = New System.Drawing.Point(0, 0)
    Me.btnGroupToggle.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.btnGroupToggle.Name = "btnGroupToggle"
    Me.btnGroupToggle.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
    Me.btnGroupToggle.Size = New System.Drawing.Size(520, 30)
    Me.btnGroupToggle.TabIndex = 1
    Me.btnGroupToggle.Text = "Data Aggregation"
    Me.btnGroupToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.btnGroupToggle.UseVisualStyleBackColor = True
    '
    'pnlNeighborhood
    '
    Me.pnlNeighborhood.BackColor = System.Drawing.Color.LightSteelBlue
    Me.pnlNeighborhood.Controls.Add(Me.cmbDecayFunction)
    Me.pnlNeighborhood.Controls.Add(Me.lblNeighborhoodType)
    Me.pnlNeighborhood.Controls.Add(Me.lblRadiusUnits)
    Me.pnlNeighborhood.Controls.Add(Me.cmbNeighborhoodType)
    Me.pnlNeighborhood.Controls.Add(Me.txtRadius)
    Me.pnlNeighborhood.Controls.Add(Me.lblDecayFunction)
    Me.pnlNeighborhood.Controls.Add(Me.btnNeighborhoodToggle)
    Me.pnlNeighborhood.Controls.Add(Me.lblRadius)
    Me.pnlNeighborhood.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlNeighborhood.Location = New System.Drawing.Point(893, 0)
    Me.pnlNeighborhood.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlNeighborhood.Name = "pnlNeighborhood"
    Me.pnlNeighborhood.Size = New System.Drawing.Size(184, 152)
    Me.pnlNeighborhood.TabIndex = 5
    '
    'cmbDecayFunction
    '
    Me.cmbDecayFunction.FormattingEnabled = True
    Me.cmbDecayFunction.Items.AddRange(New Object() {"no decay", "linear", "Gaussian"})
    Me.cmbDecayFunction.Location = New System.Drawing.Point(155, 92)
    Me.cmbDecayFunction.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.cmbDecayFunction.Name = "cmbDecayFunction"
    Me.cmbDecayFunction.Size = New System.Drawing.Size(137, 32)
    Me.cmbDecayFunction.TabIndex = 4
    Me.cmbDecayFunction.Text = "(select)"
    Me.cmbDecayFunction.Visible = False
    '
    'lblNeighborhoodType
    '
    Me.lblNeighborhoodType.AutoSize = True
    Me.lblNeighborhoodType.Location = New System.Drawing.Point(18, 128)
    Me.lblNeighborhoodType.Name = "lblNeighborhoodType"
    Me.lblNeighborhoodType.Size = New System.Drawing.Size(177, 26)
    Me.lblNeighborhoodType.TabIndex = 6
    Me.lblNeighborhoodType.Text = "NeighborhoodType"
    Me.lblNeighborhoodType.Visible = False
    '
    'lblRadiusUnits
    '
    Me.lblRadiusUnits.AutoSize = True
    Me.lblRadiusUnits.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblRadiusUnits.Location = New System.Drawing.Point(255, 33)
    Me.lblRadiusUnits.Name = "lblRadiusUnits"
    Me.lblRadiusUnits.Size = New System.Drawing.Size(63, 28)
    Me.lblRadiusUnits.TabIndex = 3
    Me.lblRadiusUnits.Text = "Units"
    '
    'cmbNeighborhoodType
    '
    Me.cmbNeighborhoodType.FormattingEnabled = True
    Me.cmbNeighborhoodType.Items.AddRange(New Object() {"Neighbors", "Distance"})
    Me.cmbNeighborhoodType.Location = New System.Drawing.Point(155, 125)
    Me.cmbNeighborhoodType.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.cmbNeighborhoodType.Name = "cmbNeighborhoodType"
    Me.cmbNeighborhoodType.Size = New System.Drawing.Size(137, 32)
    Me.cmbNeighborhoodType.TabIndex = 5
    Me.cmbNeighborhoodType.Text = "(select)"
    Me.cmbNeighborhoodType.Visible = False
    '
    'txtRadius
    '
    Me.txtRadius.Location = New System.Drawing.Point(185, 32)
    Me.txtRadius.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.txtRadius.Name = "txtRadius"
    Me.txtRadius.Size = New System.Drawing.Size(63, 32)
    Me.txtRadius.TabIndex = 2
    Me.txtRadius.Text = "1"
    Me.txtRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    '
    'lblDecayFunction
    '
    Me.lblDecayFunction.AutoSize = True
    Me.lblDecayFunction.Location = New System.Drawing.Point(18, 95)
    Me.lblDecayFunction.Name = "lblDecayFunction"
    Me.lblDecayFunction.Size = New System.Drawing.Size(147, 26)
    Me.lblDecayFunction.TabIndex = 1
    Me.lblDecayFunction.Text = "Decay Function:"
    Me.lblDecayFunction.Visible = False
    '
    'btnNeighborhoodToggle
    '
    Me.btnNeighborhoodToggle.Dock = System.Windows.Forms.DockStyle.Top
    Me.btnNeighborhoodToggle.FlatAppearance.BorderSize = 0
    Me.btnNeighborhoodToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnNeighborhoodToggle.Font = New System.Drawing.Font("Palatino Linotype", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnNeighborhoodToggle.Location = New System.Drawing.Point(0, 0)
    Me.btnNeighborhoodToggle.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.btnNeighborhoodToggle.Name = "btnNeighborhoodToggle"
    Me.btnNeighborhoodToggle.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
    Me.btnNeighborhoodToggle.Size = New System.Drawing.Size(184, 30)
    Me.btnNeighborhoodToggle.TabIndex = 2
    Me.btnNeighborhoodToggle.Text = "Neighborhood Definition"
    Me.btnNeighborhoodToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.btnNeighborhoodToggle.UseVisualStyleBackColor = True
    '
    'lblRadius
    '
    Me.lblRadius.AutoSize = True
    Me.lblRadius.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblRadius.Location = New System.Drawing.Point(25, 33)
    Me.lblRadius.Name = "lblRadius"
    Me.lblRadius.Size = New System.Drawing.Size(88, 28)
    Me.lblRadius.TabIndex = 0
    Me.lblRadius.Text = "Radius: "
    '
    'pnlNullModel
    '
    Me.pnlNullModel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.pnlNullModel.BackColor = System.Drawing.Color.DarkKhaki
    Me.pnlNullModel.Controls.Add(Me.FlowLayoutPanel1)
    Me.pnlNullModel.Controls.Add(Me.radFixedOther)
    Me.pnlNullModel.Controls.Add(Me.btnStatParamToggle)
    Me.pnlNullModel.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlNullModel.Location = New System.Drawing.Point(0, 213)
    Me.pnlNullModel.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlNullModel.Name = "pnlNullModel"
    Me.pnlNullModel.Size = New System.Drawing.Size(341, 204)
    Me.pnlNullModel.TabIndex = 1
    '
    'FlowLayoutPanel1
    '
    Me.FlowLayoutPanel1.AutoSize = True
    Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.FlowLayoutPanel1.Controls.Add(Me.radioCSR)
    Me.FlowLayoutPanel1.Controls.Add(Me.radioToroidalShift)
    Me.FlowLayoutPanel1.Controls.Add(Me.radioRL)
    Me.FlowLayoutPanel1.Controls.Add(Me.radioCRL)
    Me.FlowLayoutPanel1.Controls.Add(Me.panelRestrictionOptions)
    Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
    Me.FlowLayoutPanel1.Location = New System.Drawing.Point(11, 37)
    Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
    Me.FlowLayoutPanel1.Size = New System.Drawing.Size(375, 408)
    Me.FlowLayoutPanel1.TabIndex = 11
    '
    'radioCSR
    '
    Me.radioCSR.AutoSize = True
    Me.radioCSR.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.radioCSR.Location = New System.Drawing.Point(4, 4)
    Me.radioCSR.Margin = New System.Windows.Forms.Padding(4)
    Me.radioCSR.Name = "radioCSR"
    Me.radioCSR.Size = New System.Drawing.Size(293, 32)
    Me.radioCSR.TabIndex = 8
    Me.radioCSR.TabStop = True
    Me.radioCSR.Text = "Random Positioning (CSR)"
    Me.radioCSR.UseVisualStyleBackColor = True
    '
    'radioToroidalShift
    '
    Me.radioToroidalShift.AutoSize = True
    Me.radioToroidalShift.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.radioToroidalShift.Location = New System.Drawing.Point(3, 43)
    Me.radioToroidalShift.Name = "radioToroidalShift"
    Me.radioToroidalShift.Size = New System.Drawing.Size(210, 32)
    Me.radioToroidalShift.TabIndex = 10
    Me.radioToroidalShift.TabStop = True
    Me.radioToroidalShift.Text = "Toroidal Shift (TS)"
    Me.radioToroidalShift.UseVisualStyleBackColor = True
    '
    'radioRL
    '
    Me.radioRL.AutoSize = True
    Me.radioRL.Checked = True
    Me.radioRL.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.radioRL.Location = New System.Drawing.Point(4, 82)
    Me.radioRL.Margin = New System.Windows.Forms.Padding(4)
    Me.radioRL.Name = "radioRL"
    Me.radioRL.Size = New System.Drawing.Size(252, 32)
    Me.radioRL.TabIndex = 5
    Me.radioRL.TabStop = True
    Me.radioRL.Text = "Random Labeling (RL)"
    Me.radioRL.UseVisualStyleBackColor = True
    '
    'radioCRL
    '
    Me.radioCRL.AutoSize = True
    Me.radioCRL.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.radioCRL.Location = New System.Drawing.Point(4, 122)
    Me.radioCRL.Margin = New System.Windows.Forms.Padding(4)
    Me.radioCRL.Name = "radioCRL"
    Me.radioCRL.Size = New System.Drawing.Size(367, 32)
    Me.radioCRL.TabIndex = 6
    Me.radioCRL.TabStop = True
    Me.radioCRL.Text = "Restricted Random Labeling (RRL)"
    Me.radioCRL.UseVisualStyleBackColor = True
    '
    'panelRestrictionOptions
    '
    Me.panelRestrictionOptions.AutoSize = True
    Me.panelRestrictionOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
    Me.panelRestrictionOptions.Controls.Add(Me.Label6)
    Me.panelRestrictionOptions.Controls.Add(Me.radFixedBase)
    Me.panelRestrictionOptions.Controls.Add(Me.radFixedNeighbor)
    Me.panelRestrictionOptions.Controls.Add(Me.radDesignateFixed)
    Me.panelRestrictionOptions.Controls.Add(Me.listFixed)
    Me.panelRestrictionOptions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
    Me.panelRestrictionOptions.Location = New System.Drawing.Point(25, 161)
    Me.panelRestrictionOptions.Margin = New System.Windows.Forms.Padding(25, 3, 3, 3)
    Me.panelRestrictionOptions.Name = "panelRestrictionOptions"
    Me.panelRestrictionOptions.Size = New System.Drawing.Size(220, 244)
    Me.panelRestrictionOptions.TabIndex = 11
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label6.Location = New System.Drawing.Point(3, 0)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(170, 28)
    Me.Label6.TabIndex = 0
    Me.Label6.Text = "Restricted Classes:"
    '
    'radFixedBase
    '
    Me.radFixedBase.AutoSize = True
    Me.radFixedBase.Checked = True
    Me.radFixedBase.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.radFixedBase.Location = New System.Drawing.Point(4, 32)
    Me.radFixedBase.Margin = New System.Windows.Forms.Padding(4)
    Me.radFixedBase.Name = "radFixedBase"
    Me.radFixedBase.Size = New System.Drawing.Size(124, 32)
    Me.radFixedBase.TabIndex = 0
    Me.radFixedBase.TabStop = True
    Me.radFixedBase.Text = "row class"
    Me.radFixedBase.UseVisualStyleBackColor = True
    '
    'radFixedNeighbor
    '
    Me.radFixedNeighbor.AutoSize = True
    Me.radFixedNeighbor.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.radFixedNeighbor.Location = New System.Drawing.Point(4, 72)
    Me.radFixedNeighbor.Margin = New System.Windows.Forms.Padding(4)
    Me.radFixedNeighbor.Name = "radFixedNeighbor"
    Me.radFixedNeighbor.Size = New System.Drawing.Size(161, 32)
    Me.radFixedNeighbor.TabIndex = 1
    Me.radFixedNeighbor.Text = "column class"
    Me.radFixedNeighbor.UseVisualStyleBackColor = True
    '
    'radDesignateFixed
    '
    Me.radDesignateFixed.AutoSize = True
    Me.radDesignateFixed.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.radDesignateFixed.Location = New System.Drawing.Point(4, 112)
    Me.radDesignateFixed.Margin = New System.Windows.Forms.Padding(4)
    Me.radDesignateFixed.Name = "radDesignateFixed"
    Me.radDesignateFixed.Size = New System.Drawing.Size(212, 32)
    Me.radDesignateFixed.TabIndex = 2
    Me.radDesignateFixed.Text = "designated classes"
    Me.radDesignateFixed.UseVisualStyleBackColor = True
    '
    'listFixed
    '
    Me.listFixed.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.listFixed.FormattingEnabled = True
    Me.listFixed.ItemHeight = 28
    Me.listFixed.Location = New System.Drawing.Point(4, 152)
    Me.listFixed.Margin = New System.Windows.Forms.Padding(4)
    Me.listFixed.Name = "listFixed"
    Me.listFixed.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
    Me.listFixed.Size = New System.Drawing.Size(120, 88)
    Me.listFixed.TabIndex = 3
    '
    'radFixedOther
    '
    Me.radFixedOther.AutoSize = True
    Me.radFixedOther.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.radFixedOther.Location = New System.Drawing.Point(11, 405)
    Me.radFixedOther.Margin = New System.Windows.Forms.Padding(4)
    Me.radFixedOther.Name = "radFixedOther"
    Me.radFixedOther.Size = New System.Drawing.Size(182, 32)
    Me.radFixedOther.TabIndex = 3
    Me.radFixedOther.TabStop = True
    Me.radFixedOther.Text = "All but A and B"
    Me.radFixedOther.UseVisualStyleBackColor = True
    Me.radFixedOther.Visible = False
    '
    'btnStatParamToggle
    '
    Me.btnStatParamToggle.Dock = System.Windows.Forms.DockStyle.Top
    Me.btnStatParamToggle.FlatAppearance.BorderSize = 0
    Me.btnStatParamToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnStatParamToggle.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnStatParamToggle.Location = New System.Drawing.Point(0, 0)
    Me.btnStatParamToggle.Margin = New System.Windows.Forms.Padding(4)
    Me.btnStatParamToggle.Name = "btnStatParamToggle"
    Me.btnStatParamToggle.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
    Me.btnStatParamToggle.Size = New System.Drawing.Size(341, 30)
    Me.btnStatParamToggle.TabIndex = 2
    Me.btnStatParamToggle.Text = "Null Model Selection"
    Me.btnStatParamToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.btnStatParamToggle.UseVisualStyleBackColor = True
    '
    'pnlExecution
    '
    Me.pnlExecution.BackColor = System.Drawing.Color.PapayaWhip
    Me.pnlExecution.Controls.Add(Me.btnGetStdDev)
    Me.pnlExecution.Controls.Add(Me.btnStopSim)
    Me.pnlExecution.Controls.Add(Me.btnSaveSim)
    Me.pnlExecution.Controls.Add(Me.btnCompareNullModels)
    Me.pnlExecution.Controls.Add(Me.Label5)
    Me.pnlExecution.Controls.Add(Me.btnResetSims)
    Me.pnlExecution.Controls.Add(Me.lblSimCompleted)
    Me.pnlExecution.Controls.Add(Me.udNumSims)
    Me.pnlExecution.Controls.Add(Me.btnRunMany)
    Me.pnlExecution.Controls.Add(Me.btnRunOnce)
    Me.pnlExecution.Controls.Add(Me.btnExecution)
    Me.pnlExecution.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlExecution.Location = New System.Drawing.Point(341, 213)
    Me.pnlExecution.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlExecution.Name = "pnlExecution"
    Me.pnlExecution.Size = New System.Drawing.Size(353, 308)
    Me.pnlExecution.TabIndex = 3
    '
    'btnGetStdDev
    '
    Me.btnGetStdDev.Location = New System.Drawing.Point(25, 123)
    Me.btnGetStdDev.Name = "btnGetStdDev"
    Me.btnGetStdDev.Size = New System.Drawing.Size(247, 29)
    Me.btnGetStdDev.TabIndex = 12
    Me.btnGetStdDev.Text = "Repeat 100x and get st. dev. of CLQ"
    Me.btnGetStdDev.UseVisualStyleBackColor = True
    Me.btnGetStdDev.Visible = False
    '
    'btnStopSim
    '
    Me.btnStopSim.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnStopSim.Location = New System.Drawing.Point(93, 63)
    Me.btnStopSim.Name = "btnStopSim"
    Me.btnStopSim.Size = New System.Drawing.Size(55, 29)
    Me.btnStopSim.TabIndex = 2
    Me.btnStopSim.Text = "Stop"
    Me.btnStopSim.UseVisualStyleBackColor = True
    '
    'btnSaveSim
    '
    Me.btnSaveSim.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnSaveSim.Location = New System.Drawing.Point(25, 185)
    Me.btnSaveSim.Name = "btnSaveSim"
    Me.btnSaveSim.Size = New System.Drawing.Size(101, 34)
    Me.btnSaveSim.TabIndex = 11
    Me.btnSaveSim.Text = "Save Last Sim"
    Me.btnSaveSim.UseVisualStyleBackColor = True
    Me.btnSaveSim.Visible = False
    '
    'btnCompareNullModels
    '
    Me.btnCompareNullModels.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnCompareNullModels.Location = New System.Drawing.Point(158, 185)
    Me.btnCompareNullModels.Name = "btnCompareNullModels"
    Me.btnCompareNullModels.Size = New System.Drawing.Size(162, 37)
    Me.btnCompareNullModels.TabIndex = 10
    Me.btnCompareNullModels.Text = "Compare Null Models (temp)"
    Me.btnCompareNullModels.UseVisualStyleBackColor = True
    Me.btnCompareNullModels.Visible = False
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Label5.Location = New System.Drawing.Point(25, 32)
    Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(132, 28)
    Me.Label5.TabIndex = 6
    Me.Label5.Text = "Simulations:"
    '
    'btnResetSims
    '
    Me.btnResetSims.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnResetSims.Location = New System.Drawing.Point(157, 63)
    Me.btnResetSims.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.btnResetSims.Name = "btnResetSims"
    Me.btnResetSims.Size = New System.Drawing.Size(55, 29)
    Me.btnResetSims.TabIndex = 3
    Me.btnResetSims.Text = "Reset"
    Me.btnResetSims.UseVisualStyleBackColor = True
    '
    'lblSimCompleted
    '
    Me.lblSimCompleted.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblSimCompleted.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
    Me.lblSimCompleted.Location = New System.Drawing.Point(90, 99)
    Me.lblSimCompleted.Name = "lblSimCompleted"
    Me.lblSimCompleted.Size = New System.Drawing.Size(123, 21)
    Me.lblSimCompleted.TabIndex = 4
    Me.lblSimCompleted.Text = "0 sims completed"
    Me.lblSimCompleted.TextAlign = System.Drawing.ContentAlignment.TopRight
    '
    'udNumSims
    '
    Me.udNumSims.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.udNumSims.Location = New System.Drawing.Point(124, 31)
    Me.udNumSims.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.udNumSims.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
    Me.udNumSims.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
    Me.udNumSims.Name = "udNumSims"
    Me.udNumSims.Size = New System.Drawing.Size(89, 36)
    Me.udNumSims.TabIndex = 0
    Me.udNumSims.ThousandsSeparator = True
    Me.udNumSims.Value = New Decimal(New Integer() {1000, 0, 0, 0})
    '
    'btnRunMany
    '
    Me.btnRunMany.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnRunMany.Location = New System.Drawing.Point(29, 63)
    Me.btnRunMany.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.btnRunMany.Name = "btnRunMany"
    Me.btnRunMany.Size = New System.Drawing.Size(55, 29)
    Me.btnRunMany.TabIndex = 1
    Me.btnRunMany.Text = "Run"
    Me.btnRunMany.UseVisualStyleBackColor = True
    '
    'btnRunOnce
    '
    Me.btnRunOnce.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnRunOnce.Location = New System.Drawing.Point(200, 227)
    Me.btnRunOnce.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.btnRunOnce.Name = "btnRunOnce"
    Me.btnRunOnce.Size = New System.Drawing.Size(111, 30)
    Me.btnRunOnce.TabIndex = 1
    Me.btnRunOnce.Text = "Once"
    Me.btnRunOnce.UseVisualStyleBackColor = True
    Me.btnRunOnce.Visible = False
    '
    'btnExecution
    '
    Me.btnExecution.Dock = System.Windows.Forms.DockStyle.Top
    Me.btnExecution.FlatAppearance.BorderSize = 0
    Me.btnExecution.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnExecution.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnExecution.Location = New System.Drawing.Point(0, 0)
    Me.btnExecution.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.btnExecution.Name = "btnExecution"
    Me.btnExecution.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
    Me.btnExecution.Size = New System.Drawing.Size(353, 30)
    Me.btnExecution.TabIndex = 4
    Me.btnExecution.Text = "Simulation Control"
    Me.btnExecution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.btnExecution.UseVisualStyleBackColor = True
    '
    'PanelResults
    '
    Me.PanelResults.BackColor = System.Drawing.Color.LightBlue
    Me.PanelResults.Controls.Add(Me.grpDisplayStat)
    Me.PanelResults.Controls.Add(Me.grpSource)
    Me.PanelResults.Controls.Add(Me.btnResultsToggle)
    Me.PanelResults.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.PanelResults.Location = New System.Drawing.Point(694, 213)
    Me.PanelResults.Margin = New System.Windows.Forms.Padding(0)
    Me.PanelResults.Name = "PanelResults"
    Me.PanelResults.Size = New System.Drawing.Size(297, 40)
    Me.PanelResults.TabIndex = 2
    '
    'grpDisplayStat
    '
    Me.grpDisplayStat.Controls.Add(Me.radDisplayNNCT)
    Me.grpDisplayStat.Controls.Add(Me.radDisplayCLQ)
    Me.grpDisplayStat.Location = New System.Drawing.Point(3, 129)
    Me.grpDisplayStat.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.grpDisplayStat.Name = "grpDisplayStat"
    Me.grpDisplayStat.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.grpDisplayStat.Size = New System.Drawing.Size(270, 75)
    Me.grpDisplayStat.TabIndex = 2
    Me.grpDisplayStat.TabStop = False
    Me.grpDisplayStat.Text = "Display in Table:"
    '
    'radDisplayNNCT
    '
    Me.radDisplayNNCT.AutoSize = True
    Me.radDisplayNNCT.Location = New System.Drawing.Point(11, 40)
    Me.radDisplayNNCT.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.radDisplayNNCT.Name = "radDisplayNNCT"
    Me.radDisplayNNCT.Size = New System.Drawing.Size(344, 30)
    Me.radDisplayNNCT.TabIndex = 1
    Me.radDisplayNNCT.Text = "Nearest Neighbor Contingency Table"
    Me.radDisplayNNCT.UseVisualStyleBackColor = True
    '
    'radDisplayCLQ
    '
    Me.radDisplayCLQ.AutoSize = True
    Me.radDisplayCLQ.Checked = True
    Me.radDisplayCLQ.Location = New System.Drawing.Point(11, 19)
    Me.radDisplayCLQ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.radDisplayCLQ.Name = "radDisplayCLQ"
    Me.radDisplayCLQ.Size = New System.Drawing.Size(213, 30)
    Me.radDisplayCLQ.TabIndex = 0
    Me.radDisplayCLQ.TabStop = True
    Me.radDisplayCLQ.Text = "Co-Location Quotient"
    Me.radDisplayCLQ.UseVisualStyleBackColor = True
    '
    'grpSource
    '
    Me.grpSource.Controls.Add(Me.cmbSimNum)
    Me.grpSource.Controls.Add(Me.radSrcSimulation)
    Me.grpSource.Controls.Add(Me.radSrcData)
    Me.grpSource.Location = New System.Drawing.Point(3, 41)
    Me.grpSource.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.grpSource.Name = "grpSource"
    Me.grpSource.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.grpSource.Size = New System.Drawing.Size(270, 79)
    Me.grpSource.TabIndex = 1
    Me.grpSource.TabStop = False
    Me.grpSource.Text = "Source:"
    '
    'cmbSimNum
    '
    Me.cmbSimNum.FormattingEnabled = True
    Me.cmbSimNum.Location = New System.Drawing.Point(94, 44)
    Me.cmbSimNum.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.cmbSimNum.Name = "cmbSimNum"
    Me.cmbSimNum.Size = New System.Drawing.Size(70, 32)
    Me.cmbSimNum.TabIndex = 2
    '
    'radSrcSimulation
    '
    Me.radSrcSimulation.AutoSize = True
    Me.radSrcSimulation.Location = New System.Drawing.Point(11, 45)
    Me.radSrcSimulation.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.radSrcSimulation.Name = "radSrcSimulation"
    Me.radSrcSimulation.Size = New System.Drawing.Size(126, 30)
    Me.radSrcSimulation.TabIndex = 1
    Me.radSrcSimulation.Text = "Simulation"
    Me.radSrcSimulation.UseVisualStyleBackColor = True
    '
    'radSrcData
    '
    Me.radSrcData.AutoSize = True
    Me.radSrcData.Checked = True
    Me.radSrcData.Location = New System.Drawing.Point(11, 22)
    Me.radSrcData.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.radSrcData.Name = "radSrcData"
    Me.radSrcData.Size = New System.Drawing.Size(76, 30)
    Me.radSrcData.TabIndex = 0
    Me.radSrcData.TabStop = True
    Me.radSrcData.Text = "Data"
    Me.radSrcData.UseVisualStyleBackColor = True
    '
    'btnResultsToggle
    '
    Me.btnResultsToggle.Dock = System.Windows.Forms.DockStyle.Top
    Me.btnResultsToggle.FlatAppearance.BorderSize = 0
    Me.btnResultsToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnResultsToggle.Font = New System.Drawing.Font("Palatino Linotype", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnResultsToggle.Location = New System.Drawing.Point(0, 0)
    Me.btnResultsToggle.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.btnResultsToggle.Name = "btnResultsToggle"
    Me.btnResultsToggle.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
    Me.btnResultsToggle.Size = New System.Drawing.Size(297, 30)
    Me.btnResultsToggle.TabIndex = 6
    Me.btnResultsToggle.Text = "Results"
    Me.btnResultsToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.btnResultsToggle.UseVisualStyleBackColor = True
    '
    'pnlComputeCoLocation
    '
    Me.pnlComputeCoLocation.BackColor = System.Drawing.Color.BurlyWood
    Me.pnlComputeCoLocation.Controls.Add(Me.btnCalculateCLQ)
    Me.pnlComputeCoLocation.Controls.Add(Me.btnCalculationToggle)
    Me.pnlComputeCoLocation.Font = New System.Drawing.Font("Palatino Linotype", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.pnlComputeCoLocation.Location = New System.Drawing.Point(0, 521)
    Me.pnlComputeCoLocation.Margin = New System.Windows.Forms.Padding(0)
    Me.pnlComputeCoLocation.Name = "pnlComputeCoLocation"
    Me.pnlComputeCoLocation.Size = New System.Drawing.Size(487, 89)
    Me.pnlComputeCoLocation.TabIndex = 9
    '
    'btnCalculateCLQ
    '
    Me.btnCalculateCLQ.Location = New System.Drawing.Point(0, 39)
    Me.btnCalculateCLQ.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
    Me.btnCalculateCLQ.Name = "btnCalculateCLQ"
    Me.btnCalculateCLQ.Size = New System.Drawing.Size(248, 29)
    Me.btnCalculateCLQ.TabIndex = 11
    Me.btnCalculateCLQ.Text = "Compute Co-Location Statistics"
    Me.btnCalculateCLQ.UseVisualStyleBackColor = True
    '
    'btnCalculationToggle
    '
    Me.btnCalculationToggle.Dock = System.Windows.Forms.DockStyle.Top
    Me.btnCalculationToggle.FlatAppearance.BorderSize = 0
    Me.btnCalculationToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
    Me.btnCalculationToggle.Font = New System.Drawing.Font("Palatino Linotype", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.btnCalculationToggle.Location = New System.Drawing.Point(0, 0)
    Me.btnCalculationToggle.Margin = New System.Windows.Forms.Padding(0)
    Me.btnCalculationToggle.Name = "btnCalculationToggle"
    Me.btnCalculationToggle.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
    Me.btnCalculationToggle.Size = New System.Drawing.Size(487, 30)
    Me.btnCalculationToggle.TabIndex = 5
    Me.btnCalculationToggle.Text = "Calculation"
    Me.btnCalculationToggle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.btnCalculationToggle.UseVisualStyleBackColor = True
    '
    'frmControl
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 23.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(1081, 786)
    Me.ControlBox = False
    Me.Controls.Add(Me.flowSteps)
    Me.Controls.Add(Me.lblProgress)
    Me.Font = New System.Drawing.Font("Calibri Light", 8.25!)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
    Me.Margin = New System.Windows.Forms.Padding(4)
    Me.Name = "frmControl"
    Me.Text = "Control Panel"
    Me.flowSteps.ResumeLayout(False)
    Me.panelLoadData.ResumeLayout(False)
    Me.panelLoadData.PerformLayout()
    Me.TableLayoutPanel1.ResumeLayout(False)
    Me.TableLayoutPanel1.PerformLayout()
    Me.pnlGroups.ResumeLayout(False)
    Me.tabClasses.ResumeLayout(False)
    Me.tabObsClasses.ResumeLayout(False)
    Me.tabObsClasses.PerformLayout()
    CType(Me.dgvClasses, System.ComponentModel.ISupportInitialize).EndInit()
    Me.tabAggClasses.ResumeLayout(False)
    CType(Me.dgvAggClasses, System.ComponentModel.ISupportInitialize).EndInit()
    Me.pnlNeighborhood.ResumeLayout(False)
    Me.pnlNeighborhood.PerformLayout()
    Me.pnlNullModel.ResumeLayout(False)
    Me.pnlNullModel.PerformLayout()
    Me.FlowLayoutPanel1.ResumeLayout(False)
    Me.FlowLayoutPanel1.PerformLayout()
    Me.panelRestrictionOptions.ResumeLayout(False)
    Me.panelRestrictionOptions.PerformLayout()
    Me.pnlExecution.ResumeLayout(False)
    Me.pnlExecution.PerformLayout()
    CType(Me.udNumSims, System.ComponentModel.ISupportInitialize).EndInit()
    Me.PanelResults.ResumeLayout(False)
    Me.grpDisplayStat.ResumeLayout(False)
    Me.grpDisplayStat.PerformLayout()
    Me.grpSource.ResumeLayout(False)
    Me.grpSource.PerformLayout()
    Me.pnlComputeCoLocation.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents lblProgress As System.Windows.Forms.Label
  Friend WithEvents flowSteps As System.Windows.Forms.FlowLayoutPanel
  Friend WithEvents panelLoadData As System.Windows.Forms.Panel
  Friend WithEvents tabClasses As System.Windows.Forms.TabControl
  Friend WithEvents tabObsClasses As System.Windows.Forms.TabPage
  Friend WithEvents dgvClasses As System.Windows.Forms.DataGridView
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents tabAggClasses As System.Windows.Forms.TabPage
  Friend WithEvents dgvAggClasses As System.Windows.Forms.DataGridView
  Friend WithEvents lblNumPts As System.Windows.Forms.Label
  Friend WithEvents btnLoadAuxiliaryData As System.Windows.Forms.Button
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents cmbFields As System.Windows.Forms.ComboBox
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents btnLoadPopulationShapefile As System.Windows.Forms.Button
  Friend WithEvents btnLoadDataToggle As System.Windows.Forms.Button
  Friend WithEvents pnlNullModel As System.Windows.Forms.Panel
  Friend WithEvents radioCSR As System.Windows.Forms.RadioButton
  Friend WithEvents listFixed As System.Windows.Forms.ListBox
  Friend WithEvents radioCRL As System.Windows.Forms.RadioButton
  Friend WithEvents radioRL As System.Windows.Forms.RadioButton
  Friend WithEvents btnStatParamToggle As System.Windows.Forms.Button
  Friend WithEvents PanelResults As System.Windows.Forms.Panel
  Friend WithEvents btnResultsToggle As System.Windows.Forms.Button
  Friend WithEvents grpSource As System.Windows.Forms.GroupBox
  Friend WithEvents cmbSimNum As System.Windows.Forms.ComboBox
  Friend WithEvents radSrcSimulation As System.Windows.Forms.RadioButton
  Friend WithEvents radSrcData As System.Windows.Forms.RadioButton
  Friend WithEvents grpDisplayStat As System.Windows.Forms.GroupBox
  Friend WithEvents radDisplayNNCT As System.Windows.Forms.RadioButton
  Friend WithEvents radDisplayCLQ As System.Windows.Forms.RadioButton
  Friend WithEvents pnlExecution As System.Windows.Forms.Panel
  Friend WithEvents btnExecution As System.Windows.Forms.Button
  Friend WithEvents pnlGroups As System.Windows.Forms.Panel
  Friend WithEvents btnGroupToggle As System.Windows.Forms.Button
  Friend WithEvents pnlNeighborhood As System.Windows.Forms.Panel
  Friend WithEvents btnNeighborhoodToggle As System.Windows.Forms.Button
  Friend WithEvents cmbDecayFunction As System.Windows.Forms.ComboBox
  Friend WithEvents lblRadiusUnits As System.Windows.Forms.Label
  Friend WithEvents txtRadius As System.Windows.Forms.TextBox
  Friend WithEvents lblDecayFunction As System.Windows.Forms.Label
  Friend WithEvents lblRadius As System.Windows.Forms.Label
  Friend WithEvents cmbNeighborhoodType As System.Windows.Forms.ComboBox
  Friend WithEvents lblNeighborhoodType As System.Windows.Forms.Label
  Friend WithEvents udNumSims As System.Windows.Forms.NumericUpDown
  Friend WithEvents btnRunMany As System.Windows.Forms.Button
  Friend WithEvents btnRunOnce As System.Windows.Forms.Button
  Friend WithEvents lblSimCompleted As System.Windows.Forms.Label
  Friend WithEvents btnResetSims As System.Windows.Forms.Button
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents radDesignateFixed As System.Windows.Forms.RadioButton
  Friend WithEvents radFixedNeighbor As System.Windows.Forms.RadioButton
  Friend WithEvents radFixedBase As System.Windows.Forms.RadioButton
  Friend WithEvents radFixedOther As System.Windows.Forms.RadioButton
  Friend WithEvents btnBatchProcess As System.Windows.Forms.Button
  Friend WithEvents btnLoadStudyArea As System.Windows.Forms.Button
  Friend WithEvents lblStudyArea As System.Windows.Forms.Label
  Friend WithEvents btnCompareNullModels As System.Windows.Forms.Button
  Friend WithEvents radioToroidalShift As System.Windows.Forms.RadioButton
  Friend WithEvents pnlComputeCoLocation As System.Windows.Forms.Panel
  Friend WithEvents btnCalculateCLQ As System.Windows.Forms.Button
  Friend WithEvents btnCalculationToggle As System.Windows.Forms.Button
  Friend WithEvents btnSaveSim As System.Windows.Forms.Button
  Friend WithEvents btnStopSim As System.Windows.Forms.Button
  Friend WithEvents btnGetStdDev As System.Windows.Forms.Button
  Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
  Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
  Friend WithEvents panelRestrictionOptions As FlowLayoutPanel
  Friend WithEvents Label6 As Label
End Class
