Imports DotSpatial.Data
Imports DotSpatial.Topology
Imports DotSpatial.Controls
Public Class frmControl
#Region "Variables"
  ' automated parameters
  Dim doResize As Boolean = False

  Dim panelExpanded As New Dictionary(Of Panel, Boolean)
  Public panelMaxWidth As Integer
  Dim PanelBuffer As Integer = 15

  ' data layers
  Public popPtLayer As IMapLayer
  Public studyAreaLayer As MapPolygonLayer
  Public outlineFeature As IFeature
  Public outlineExtent As Extent
  Public pCLQ As cCoLocationEngine
  ' current panel
  Dim curPnl As Panel
  ' classification
  'Public classID() As Integer ' class of each observation
  '  Public className() As Object ' list of classes by ID, with associated names
  '  Public classIDLookup As IDictionary ' allows lookup of class 
  Public aggClasses() As Object
  '  Dim aggClassList As List(Of Object) ' to look up aggregate class name from ID
  Public AggIDofOrigClass As IDictionary(Of Object, Integer) ' to look up aggregate class ID from original class
  Public AggIDofAggClass As IDictionary(Of Object, Integer) ' to look up aggregate class ID from aggregate class name
  ' simulation parameters
  Dim numNeighbors As Integer = 1
  Dim ProgTracker As New Feedback.ProgressTracker
  ' layout options for experimental and release versions of software
  Private expandPanelToFillUpSpace As Boolean = False
  Private showOnePanelAtATime As Boolean = False
  Private startCollapsed As Boolean = False
  Private showAllControls As Boolean = False
  Private unifyColors As Boolean = True
  ' null model
  Dim lastSelectedNullModel As cCoLocationEngine.eNullModel
  ' managing display
  Dim showResults As Boolean = False
#End Region
  Public Function flowWidth() As Integer
    Return flowSteps.Width
  End Function
#Region "Initialization"
  Private Sub controlForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    ' expand data table
    dgvClasses.Width = Me.ClientSize.Width - 2 * dgvClasses.Location.X
    ' set up controls
    cmbNeighborhoodType.SelectedIndex = 0
    cmbDecayFunction.SelectedIndex = 0
    panelRestrictionOptions.Visible = False
    listFixed.Visible = False

    ' hide panels we don't need
    ' pnlNeighborhood.Visible = False
    PanelResults.Visible = False
    'pnlGroups.Visible = False
    pnlComputeCoLocation.Visible = False
    ' hide map window

    ' pnlNullModel.Visible = False

    ' collapse all panels
    setUpPanelDictionaries()
    panelMaxWidth = getMaxPanelWidth()
    resizePanels()

    ProgTracker.setLabel(lblProgress)
  End Sub
  Public Sub New()

    ' This call is required by the designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    '  recordPanelHeights()
  End Sub
#End Region
#Region "Resize Events"
  Private Sub controlForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
    ' resize data table
    '    tabClasses.Width = Me.ClientSize.Width - 2 * tabClasses.Location.X
    ' see if we're supposed to resize other forms
    If Not doResize Then Exit Sub
    ' get size of container window
    Dim cSize As Size = frmCLQ.getMDIClientSize
    ' reset splitter locations
    frmCLQ.hSplitPos = Me.Width
    If frmCLQ.otherFormVisible Then frmCLQ.vSplitPos = Me.Height
    frmCLQ.arrangeWindows()
    ' resize panels
    resizePanels()
  End Sub
  Private Sub controlForm_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeBegin
    doResize = True
  End Sub
  Private Sub controlForm_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
    doResize = False
  End Sub
#End Region
#Region "PanelManagement"
  Private Function getMaxToggleBtnWd() As Integer
    ' measures the width of the strings in all the toggle buttons
    Dim g As Graphics = Me.CreateGraphics
    Dim R As Integer = 0
    For Each curPanel As Panel In flowSteps.Controls
      For Each ctl In curPanel.Controls
        If isToggleBtn(ctl, curPanel) Then
          Dim ctlBtn As Button = ctl
          Dim sF As SizeF = g.MeasureString(ctlBtn.Text, ctlBtn.Font)
          R = Math.Max(R, Math.Ceiling(sF.Width))
        End If
      Next
    Next
    g.Dispose()
    Return R
  End Function
  Private Sub setUpPanelDictionaries()
    For Each curPanel As Panel In flowSteps.Controls
      panelExpanded(curPanel) = False
    Next
  End Sub
  Private Function isToggleBtn(ctl As Control, pnl As Panel) As Boolean
    ' returns true if the input control is the toggle button for the input panel
    If ctl.Top = 0 Then Return True Else Return False
  End Function
  Private Function getPanelSize(curPanel As Panel, Optional assumeToggleOn As Boolean = False) As Size
    ' returns the bottom of the lowest visible control
    ' or else the height of the toggle button if toggleOff is True
    Dim R As Size = New Size(0, 0)
    If panelExpanded.Count > 0 Then
      For i = 0 To curPanel.Controls.Count - 1
        Dim curCtl As Control = curPanel.Controls(i)
        If assumeToggleOn Or panelExpanded(curPanel) Then
          If curCtl.Visible Then
            R.Height = Math.Max(R.Height, curCtl.Top + curCtl.Height)
            R.Width = Math.Max(R.Width, curCtl.Left + curCtl.Width)
          End If
        Else
          If isToggleBtn(curCtl, curPanel) Then
            R.Height = curCtl.Height
          End If
        End If
      Next
    End If
    Return R
  End Function
  Private Function getMaxPanelWidth() As Integer
    Dim maxToggleBtnWd As Integer = getMaxToggleBtnWd()
    Dim maxCtlWd As Integer = 0
    For Each pnl As Panel In flowSteps.Controls
      Dim pnlSz As Size = getPanelSize(pnl, True)
      If pnlSz.Width > maxCtlWd Then maxCtlWd = pnlSz.Width
    Next
    Return Math.Max(maxToggleBtnWd, maxCtlWd)
  End Function

  Private Sub resizePanels()
    Dim maxW As Integer = 0
    For Each pnl As Panel In flowSteps.Controls
      If pnl.Visible Then
        Dim curSz As Size = getPanelSize(pnl)
        pnl.Height = curSz.Height
        maxW = Math.Max(maxW, curSz.Width)
      End If ' panel is visible
    Next pnl
    '    If maxW > 0 Then panelMaxWidth = maxW Else panelMaxWidth = getMaxToggleBtnWd()

  End Sub
  Private Sub togglePanel(ByVal btn As Button)
    ' get panel
    curPnl = btn.Parent
    ' collapse if expanded
    If panelExpanded(curPnl) Then
      panelExpanded(curPnl) = False
    Else
      panelExpanded(curPnl) = True
    End If

    If curPnl.Height > btn.Height Then
      curPnl.Height = btn.Height
    Else
      ' resize
      resizePanels()
    End If
  End Sub
  Private Sub btnCalculationToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculationToggle.Click
    togglePanel(btnCalculationToggle)
  End Sub
  Private Sub btnNeighborhoodToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNeighborhoodToggle.Click
    togglePanel(btnNeighborhoodToggle)
  End Sub
  Private Sub btnGroupToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGroupToggle.Click
    togglePanel(btnGroupToggle)
  End Sub
  Private Sub btnResultsToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResultsToggle.Click
    togglePanel(btnResultsToggle)
  End Sub
  Private Sub btnExecution_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecution.Click
    togglePanel(btnExecution)
  End Sub
  Private Sub btnLoadDataToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadDataToggle.Click
    togglePanel(btnLoadDataToggle)
  End Sub
  Private Sub btnStatParamToggle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStatParamToggle.Click
    togglePanel(btnStatParamToggle)
  End Sub
#End Region
#Region "Population Data, Field Selection and Study Area"
  ' load user-specified shapefile
  Private Sub loadPopulationSF(Optional ByVal fileName As String = "")
    ' loads a shapefile and sets up the corresponding interface
    ' show progress
    lblProgress.Text = "loading shapefile..."
    lblProgress.Visible = True
    Dim selFile As String
    If fileName = "" Then
      ' get file from user
      Dim dlgOpen As New OpenFileDialog
      dlgOpen.Filter = "Shapefiles (*.shp)|*.shp"
      Dim dlgResult As DialogResult
      dlgResult = dlgOpen.ShowDialog()
      If dlgResult = Windows.Forms.DialogResult.Cancel Then Exit Sub
      selFile = dlgOpen.FileName
    Else
      selFile = fileName
    End If
    ' create a data layer
    Dim FS As New DotSpatial.Data.FeatureSet()
    FS = FeatureSet.Open(selFile)
    ' make sure it is a point shapefile
    If Not FS.FeatureType = FeatureType.Point Then
      MsgBox("Selected shapefile is not a point shapefile. Please select a point shapefile.")
      Exit Sub
    End If

    ' unload any previous map target layer
    If showResults Then
      If Not popPtLayer Is Nothing Then
        frmMap.mapMain.Layers.Remove(popPtLayer)
      End If
    End If
    ' create data layer
    popPtLayer = New MapPointLayer(FS)

    ' load it into the map and record for posterity
    If showResults Then
      popPtLayer = frmMap.mapMain.AddLayer(selFile)
    End If
    ' determine number of points
    lblNumPts.Text = FS.NumRows.ToString & " pts loaded"
    ' clear fixed class list
    listFixed.Items.Clear()
    ' set number of simulations to zero
    lblSimCompleted.Text = "0 sims completed"

    ' populate fields
    cmbFields.Items.Clear()
    cmbFields.Text = "(select field)"
    For Each DC As DataColumn In FS.DataTable.Columns
      If DC.DataType.IsEquivalentTo(0.GetType) Then cmbFields.Items.Add(DC.ColumnName & " [int]")
      If DC.DataType.IsEquivalentTo("abc".GetType) Then cmbFields.Items.Add(DC.ColumnName & " [str]")
    Next
    ' reset results panel
    lblProgress.Text = "- idle -"
    '    lblProgress.Visible = False
  End Sub
  Private Sub loadStudyAreaSF(Optional ByVal fileName As String = "")
    ' loads a shapefile and sets up the corresponding interface
    ' show results panel
    lblProgress.Text = "loading study region..."
    lblProgress.Visible = True
    Dim selFile As String
    If fileName = "" Then
      ' get file from user
      Dim dlgOpen As New OpenFileDialog
      dlgOpen.Filter = "Shapefiles (*.shp)|*.shp"
      Dim dlgResult As DialogResult
      dlgResult = dlgOpen.ShowDialog()
      If dlgResult = Windows.Forms.DialogResult.Cancel Then Exit Sub
      selFile = dlgOpen.FileName
    Else
      selFile = fileName
    End If
    ' create a data layer
    Dim FS As New DotSpatial.Data.FeatureSet()
    FS = FeatureSet.OpenFile(selFile)
    ' make sure it is a polygon shapefile with exactly one polygon
    If Not FS.FeatureType = FeatureType.Polygon Then
      MsgBox("Selected shapefile is not a polygon shapefile. Please select a polygon shapefile.")
      Exit Sub
    ElseIf FS.NumRows > 1 Then
      MsgBox("Study area shapefile contains more than one polygon. Please create a shapefile with just a single polygon to define the study area.")
      Exit Sub
    End If
    ' load it into the map and record for posterity
    If Not studyAreaLayer Is Nothing Then frmMap.mapMain.Layers.Remove(studyAreaLayer)
    studyAreaLayer = New MapPolygonLayer(FS)
    studyAreaLayer.Symbolizer.SetFillColor(Color.Transparent)
    studyAreaLayer.Symbolizer.SetOutline(Color.Black, 2)
    frmMap.mapMain.Layers.Add(studyAreaLayer)
    frmMap.mapMain.ZoomToMaxExtent()
    outlineFeature = studyAreaLayer.DataSet.GetFeature(0)
    outlineExtent = studyAreaLayer.Extent
    ' reset results panel
    lblProgress.Text = "- idle -"
    '    lblProgress.Visible = False
  End Sub
  Private Sub btnLoadStudyArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadStudyArea.Click
    loadStudyAreaSF()
  End Sub
  Private Sub btnLoadShapefile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadPopulationShapefile.Click
    loadPopulationSF()
  End Sub
  Private Sub btnLoadAuxiliaryData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadAuxiliaryData.Click
    ' get file from user
    Dim dlgOpen As New OpenFileDialog
    dlgOpen.Filter = "Shapefiles (*.shp)|*.shp"
    Dim dlgResult As DialogResult
    dlgResult = dlgOpen.ShowDialog()
    If dlgResult = Windows.Forms.DialogResult.Cancel Then Exit Sub
    Dim selFile As String = dlgOpen.FileName
    ' create a data layer
    Dim FS As New DotSpatial.Data.FeatureSet()
    FS = FS.OpenFile(selFile)
    ' load it into the map; no need to record for posterity
    frmMap.mapMain.AddLayer(selFile)
    ' move main target layer to top
    popPtLayer.LockDispose()
    frmMap.mapMain.Layers.Remove(popPtLayer)
    frmMap.mapMain.Layers.Insert(frmMap.mapMain.Layers.Count, popPtLayer)
    popPtLayer.UnlockDispose()

  End Sub
  Private Sub cmbFields_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFields.DropDownClosed
    If cmbFields.SelectedIndex < 0 Then Exit Sub
    ' progress
    Dim P As New Feedback.ProgressTracker
    lblProgress.Visible = True
    P.setLabel(lblProgress)
    P.initializeTask("tabulating classes...")
    For i = 0 To dgvClasses.ColumnCount - 1
      dgvClasses.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
    Next
    ' determine categories and counts
    Dim catList As List(Of Object)
    Dim countList As List(Of Integer)
    Dim DS As IDataSet = popPtLayer.DataSet
    Dim FS As IFeatureSet = DS
    Dim DT As DataTable = FS.DataTable
    Dim columnName As String = cmbFields.SelectedItem
    columnName = columnName.Substring(0, columnName.LastIndexOf("[") - 1)
    Dim Values() As Object = Data.Lookup.columnArray(DT, columnName)
    Data.Lookup.calcUniqueValueCounts(Values, catList, countList)
    ' populate categories table
    dgvClasses.RowCount = catList.Count
    dgvClasses.ColumnCount = 4
    dgvClasses.ColumnHeadersVisible = True
    dgvClasses.Columns(1).HeaderText = "Original Class"
    dgvClasses.Columns(2).HeaderText = "Count"
    dgvClasses.Columns(3).HeaderText = "Aggregate Class"
    dgvClasses.RowHeadersVisible = False
    For i = 0 To catList.Count - 1
      dgvClasses.Rows(i).Cells(1).Value = catList.Item(i) ' original category
      dgvClasses.Rows(i).Cells(2).Value = countList.Item(i) ' count in original category
      dgvClasses.Rows(i).Cells(3).Value = catList.Item(i) ' category to aggregate to
    Next
    ' sort and number
    dgvClasses.Sort(dgvClasses.Columns(2), System.ComponentModel.ListSortDirection.Descending)
    For i = 0 To catList.Count - 1
      dgvClasses.Rows(i).Cells(0).Value = i + 1
    Next
    ' run this mother
    If showResults Then initializeCLQ()
    If showResults Then frmTable.showStats()
    ' show finished
    P.finishTask()
    lblProgress.Text = "- idle -"
    For i = 0 To dgvClasses.ColumnCount - 1
      dgvClasses.Columns(i).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
    Next
    '    lblProgress.Visible = False

  End Sub
  Private Sub cmbFields_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFields.SelectedIndexChanged
    If cmbFields.SelectedIndex > -1 Then
      If frmMap.chkLabels.Checked Then frmMap.drawLabels() Else frmMap.hideLabels()
    End If
  End Sub
#End Region
#Region "Class Definition and Aggregation"
  Private Sub createClassLists()
    ' aggregates classes based on automated/user input of dgvClasses
    ' populates three lookups
    ' aggClasses(object) to look up aggregate class name from ID
    ' AggIDofOrigClass As IDictionary to look up aggregate class ID from original class
    ' Dim AggIDofAggClass As IDictionary to look up aggregate class ID from aggregate class name

    ' get list of classes
    Dim aggClassList As New List(Of Object)
    Dim curAggClass As Object
    For i = 0 To dgvClasses.RowCount - 1
      curAggClass = dgvClasses.Rows(i).Cells(3).Value
      If Not aggClassList.Contains(curAggClass) Then aggClassList.Add(curAggClass)
    Next
    '' handle blanks
    'For i = 0 To aggClassList.Count - 1
    '  If aggClassList(i) = "" Then
    '    Dim newName As String = "blank"
    '    Dim tryNum As Integer = 0
    '    Do While aggClassList.Contains(newName)
    '      tryNum += 1
    '      newName = "blank" & tryNum.ToString
    '    Loop
    '    aggClassList(i) = newName
    '  End If
    'Next
    ' convert to array
    aggClasses = aggClassList.ToArray
    Array.Sort(aggClasses)
    ' create dictionary to look up ID from aggregate class name
    ' this could be made more efficient for strings & integers
    AggIDofAggClass = New Dictionary(Of Object, Integer)
    For i = 0 To UBound(aggClasses)
      AggIDofAggClass.Add(aggClasses(i), i)
    Next

    ' create dictionary to look up ID from original class name
    AggIDofOrigClass = New Dictionary(Of Object, Integer)
    Dim classID As Integer, origClass As Object, aggClass As Object
    ' go through rows of data grid view
    For i = 0 To dgvClasses.RowCount - 1
      origClass = dgvClasses.Rows(i).Cells(1).Value
      aggClass = dgvClasses.Rows(i).Cells(3).Value
      classID = AggIDofAggClass.Item(aggClass)
      AggIDofOrigClass.Add(origClass, classID)
    Next

  End Sub
  'Private Function getAggClassDictionary() As Dictionary(Of Object, Object)
  '  ' creates a dictionary for easy lookup of
  '  ' aggregated class for each original class
  '  Dim R As New Dictionary(Of Object, Object)
  '  For i = 0 To dgvClasses.RowCount - 1
  '    With dgvClasses.Rows(i)
  '      R.Add(.Cells(1).Value, .Cells(3).Value)
  '    End With
  '  Next
  '  Return R
  'End Function
  'Private Function getAggClasses() As List(Of Object)
  '  ' retrieves a list of aggregate classes
  '  ' from the table where the user can specify the aggregation
  '  Dim R As New List(Of Object)
  '  Dim curAggClass As Object
  '  For i = 0 To dgvClasses.RowCount - 1
  '    curAggClass = dgvClasses.Rows(i).Cells(3).Value
  '    If Not R.Contains(curAggClass) Then R.Add(curAggClass)
  '  Next
  '  Return R
  'End Function

  Private Sub dgvObsClasses_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvClasses.Leave
    ' get list of unique agg classes
    initializeCLQ()
    'createClassLists()
    ' get counts
    Dim aggClassCount() As Integer
    ReDim aggClassCount(aggClasses.Length - 1)
    Dim curAggClass As Object
    Dim curAggClassIndex As Integer
    For i = 0 To dgvClasses.RowCount - 1
      curAggClass = dgvClasses.Rows(i).Cells(3).Value
      curAggClassIndex = AggIDofAggClass.Item(curAggClass)
      aggClassCount(curAggClassIndex) += dgvClasses.Rows(i).Cells(2).Value
    Next
    ' populate aggClassTable
    ' populate categories table
    dgvAggClasses.RowCount = aggClasses.Length
    dgvAggClasses.ColumnCount = 3
    dgvAggClasses.ColumnHeadersVisible = True
    dgvAggClasses.Columns(1).HeaderText = "Aggregate Class"
    dgvAggClasses.Columns(2).HeaderText = "Count"
    dgvAggClasses.RowHeadersVisible = False
    For i = 0 To dgvAggClasses.RowCount - 1
      dgvAggClasses.Rows(i).Cells(1).Value = aggClasses(i)
      dgvAggClasses.Rows(i).Cells(2).Value = aggClassCount(i)
    Next
    ' sort and number
    dgvAggClasses.Sort(dgvAggClasses.Columns(2), System.ComponentModel.ListSortDirection.Descending)
    For i = 0 To dgvAggClasses.RowCount - 1
      dgvAggClasses.Rows(i).Cells(0).Value = i + 1
    Next
  End Sub
#End Region
#Region "Neighborhood Definition"
  Private Sub cmbNeighborhoodType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNeighborhoodType.SelectedIndexChanged
    Select Case cmbNeighborhoodType.SelectedIndex
      Case Is = 0
        lblRadiusUnits.Text = "Neighbors"
      Case Is = 1
        lblRadiusUnits.Text = "Units"
    End Select
  End Sub
  Private Sub txtRadius_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRadius.Validated
    ' see if number of neighbors has changed
    If Val(txtRadius.Text) <> numNeighbors Then
      initializeCLQ()
    End If

  End Sub
  Private Sub txtRadius_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtRadius.Validating
    Dim doCancel As Boolean = False
    If Not IsNumeric(txtRadius.Text) Then
      doCancel = True
    Else
      Dim N As Double = Val(txtRadius.Text)
      If N <= 0 Then doCancel = True
      If cmbNeighborhoodType.SelectedIndex = 0 Then
        If N < 0.5 Then doCancel = True
        If N > 25 Then doCancel = True
        txtRadius.Text = Math.Round(N)
      End If
    End If
    If doCancel Then txtRadius.Text = "1"
    ' re-run the CLQ
    initializeCLQ()
    frmTable.showStats()
  End Sub
#End Region
#Region "CLQ Calculation"
  Private Sub initializeCLQ()
    ' error checking
    If cmbFields.SelectedItem Is Nothing Then Exit Sub
    If popPtLayer Is Nothing Then Exit Sub
    ' creates the CLQ calculator object
    createClassLists()
    Dim pt() As twoDTree.sPoint
    Dim ptCatID() As Integer
    Dim nbCalc As cCoLocationEngine.iNeighborhoodCalculator = New EuclideanNeighborCalculator
    If cmbNeighborhoodType.SelectedIndex = 0 Then ' get number of neighbors
      Dim nnNbCalc As EuclideanNeighborCalculator = nbCalc
      Dim numNeighbors As Integer = CInt(Val(txtRadius.Text))
      If numNeighbors < 1 Then numNeighbors = 1
      nnNbCalc.numNeighbors = numNeighbors
    End If
    ' get the points
    If popPtLayer Is Nothing Then Exit Sub
    Dim ptFS As IFeatureSet = popPtLayer.DataSet
    ReDim pt(ptFS.NumRows - 1)
    For i = 0 To ptFS.NumRows - 1
      pt(i).x = ptFS.GetFeature(i).Coordinates(0).X
      pt(i).y = ptFS.GetFeature(i).Coordinates(0).Y
    Next
    ' get original categories for each point
    Dim columnName As String = cmbFields.SelectedItem
    If columnName Is Nothing Then Exit Sub
    columnName = columnName.Substring(0, columnName.LastIndexOf("[") - 1)
    Dim DT As DataTable = ptFS.DataTable
    Dim V() As Object = Data.Lookup.columnArray(DT, columnName)
    ' aggregate as specified in aggregation dictionary
    ReDim ptCatID(UBound(V))
    For i = 0 To UBound(V)
      ptCatID(i) = AggIDofOrigClass(V(i))
    Next
    ' get classes as integers; lookup tables
    '   Data.Lookup.getIntegerIDswithLookup(V, classID, className, classIDLookup)
    ' get classes as strings
    Dim classNames(aggClasses.Count - 1) As String
    ' convert to string
    For i = 0 To aggClasses.Count - 1
      classNames(i) = aggClasses(i).ToString
    Next
    'aggClasses.CopyTo(classNames, 0)
    ' determine null model type
    Dim nullModel As cCoLocationEngine.eNullModel = getSelectedNullModel()
    ' get parameters for specific null models
    Dim fixedCat() As Integer
    If radDesignateFixed.Checked Then
      ' get fixed classes
      Dim FC As New List(Of Integer)
      For Each listItem As String In listFixed.SelectedItems
        FC.Add(AggIDofAggClass.Item(listItem))
      Next
      ' make sure that at least two classes are not fixed
      If FC.Count > listFixed.Items.Count - 2 Then
        MsgBox("At least two classes must be allowed to be randomized.")
        Exit Sub
      End If
      fixedCat = FC.ToArray
    End If
    ' get outline extent for toroidal shift
    If nullModel = cCoLocationEngine.eNullModel.ToroidalShift Or nullModel = cCoLocationEngine.eNullModel.CSR Then
      If outlineExtent Is Nothing Then
        Dim dlgTorus As New dlgTorusDefinition(ptFS)
        Dim dlgResult As DialogResult = dlgTorus.ShowDialog()
        If dlgResult = DialogResult.Cancel Then
          Exit Sub
        Else
          outlineExtent = dlgTorus.getExtent()
        End If
      End If
    End If
    ' create CLQ dataset

    pCLQ = New cCoLocationEngine(pt, ptCatID, classNames,
                           aggClasses.Length, nullModel, nbCalc, ProgTracker,
                           fixedCat, outlineFeature, outlineExtent)
    If showResults Then
      ' show contingency table
      '    pCLQ.obsNNCT.showInDataGrid(frmTable.dgvResults, className)
      frmTable.cmbStatType.SelectedIndex = 0
      frmTable.showStats()
    End If
    ' reset the number of sims
    lblSimCompleted.Text = pCLQ.numSimsCompleted.ToString & " sims completed"
  End Sub
  Private Sub btnCalculateCLQ_Click(ByVal sender As System.Object,
                                    ByVal e As System.EventArgs) _
                                  Handles btnCalculateCLQ.Click
    ' calculate and show results of CLQ calculation
    ' error checking
    If popPtLayer Is Nothing Then
      MsgBox("Please load population data.")
      Exit Sub
    End If
    If cmbFields.SelectedItem Is Nothing Then
      MsgBox("Please select Category Field.")
      Exit Sub
    End If


    ' force show
    Dim previouslyShow As Boolean = showResults
    showResults = True
    ' run calculations
    initializeCLQ()
    frmTable.showStats()
    ' return to previous
    showResults = previouslyShow
  End Sub
#End Region
#Region "Batch Processing"
  Private Sub batchProcessNoSims(ByVal dataFolder As String,
                                 ByVal catField As String,
                                 ByVal numCats As Integer)
    ' NOT SURE WHAT THIS DOES!
    ' loop through files in folder
    Dim di As New System.IO.DirectoryInfo(dataFolder)
    Dim fi As System.IO.FileInfo() = di.GetFiles("*.shp")

    Dim lCLQ_RL As New List(Of cContingencyTable)

    Dim T As cContingencyTable

    ' show progress
    ProgTracker.initializeTask("Working through dataset files...")
    ProgTracker.setTotal(UBound(fi) + 1)
    For fileNum = 0 To UBound(fi)
      ' load file
      loadPopulationSF(fi(fileNum).FullName)
      ' set category
      cmbFields.SelectedIndex = 0
      cmbFields_DropDownClosed(Me, Nothing)
      dgvObsClasses_Leave(Me, Nothing)
      ' RANDOM LABELING
      ' set null model to RL
      radioRL.Checked = True
      ' calculate CLQ, p-values, chi-sq, overall p
      T = pCLQ.CLQ
      lCLQ_RL.Add(T.Clone)
      ' report progress
      ProgTracker.setCompleted(fileNum + 1)
      Application.DoEvents()
    Next fileNum
    ' convert to data tables
    Dim tbRLCLQ As DataTable = ctListToDataTable(lCLQ_RL, "CLQ")


    ' save tables
    saveDataTableToCSV(tbRLCLQ, dataFolder & "\RL_CLQ.csv")

    ' report finish
    ProgTracker.finishTask()
  End Sub
  Private Sub batchProcess(ByVal dataFolder As String,
                           ByVal catField As String,
                           ByVal numCats As Integer,
                           Optional ByVal numSims As Integer = 1000)
    ' MAIN SUB FOR BATCH PROCESSING
    ' Loops through files in folder, and calculates CLQ & p-values for each file
    ' using several null models
    frmCLQ.pnlStatus.Visible = True


    Dim di As New System.IO.DirectoryInfo(dataFolder)
    Dim fi As System.IO.FileInfo() = di.GetFiles("*.shp")

    Dim lCLQ_RL As New List(Of cContingencyTable)
    Dim lP_RL As New List(Of cContingencyTable)
    Dim lChiSq_RL As New List(Of Double)
    Dim lOverallP_RL As New List(Of Double)

    Dim lCLQ_CRLA As New List(Of cContingencyTable)
    Dim lP_CRLA As New List(Of cContingencyTable)
    Dim lChiSq_CRLA As New List(Of Double)
    Dim lOverallP_CRLA As New List(Of Double)

    Dim lP_CRLB As New List(Of cContingencyTable)
    Dim lCLQ_CRLB As New List(Of cContingencyTable)
    Dim lChiSq_CRLB As New List(Of Double)
    Dim lOverallP_CRLB As New List(Of Double)

    Dim lP_CRLother As New List(Of cContingencyTable)
    Dim lCLQ_CRLother As New List(Of cContingencyTable)
    Dim lChiSq_CRLother As New List(Of Double)
    Dim lOverallP_CRLother As New List(Of Double)

    Dim lp_TS As New List(Of cContingencyTable)
    Dim lCLQ_TS As New List(Of cContingencyTable)
    Dim lChiSq_TS As New List(Of Double)
    Dim lOverallP_TS As New List(Of Double)

    Dim T As cContingencyTable

    ' show progress
    ProgTracker.initializeTask("Working through dataset files...")
    ProgTracker.setTotal(UBound(fi) + 1)
    showResults = False
    For fileNum = 0 To UBound(fi)
      ' load file
      loadPopulationSF(fi(fileNum).FullName)
      ' set category
      cmbFields.SelectedIndex = 0
      cmbFields_DropDownClosed(Me, Nothing)
      dgvObsClasses_Leave(Me, Nothing)
      ' RANDOM LABELING
      ' set null model to RL
      radioRL.Checked = True
      ' run simulations
      initializeCLQ()
      runSims(numSims)
      ' calculate CLQ, p-values, chi-sq, overall p
      T = pCLQ.CLQ
      lCLQ_RL.Add(T.Clone)
      T = pCLQ.obsNNCT
      T = pCLQ.pValues
      lP_RL.Add(T.Clone)
      lChiSq_RL.Add(pCLQ.ChiSqStat)
      lOverallP_RL.Add(pCLQ.ChiSqPValue)
      ' CONSTRAINED RANDOM LABELING (A)
      ' set null model to cRLa
      radioCRL.Checked = True
      radFixedBase.Checked = True
      ' run simulations
      initializeCLQ()
      runSims(numSims)
      ' calculate CLQ, p-values, chi-sq, overall p
      T = pCLQ.CLQ
      lCLQ_CRLA.Add(T.Clone)
      T = pCLQ.obsNNCT
      T = pCLQ.pValues
      lP_CRLA.Add(T.Clone)
      lChiSq_CRLA.Add(pCLQ.ChiSqStat)
      lOverallP_CRLA.Add(pCLQ.ChiSqPValue)
      ' CONSTRAINED RANDOM LABELING (b)
      ' set null model to cRLb
      radioCRL.Checked = True
      radFixedNeighbor.Checked = True
      ' run simulations
      initializeCLQ()
      runSims(numSims)
      ' calculate CLQ, p-values, chi-sq, overall p
      T = pCLQ.CLQ
      lCLQ_CRLB.Add(T.Clone)
      T = pCLQ.obsNNCT
      T = pCLQ.pValues
      lP_CRLB.Add(T.Clone)
      lChiSq_CRLB.Add(pCLQ.ChiSqStat)
      lOverallP_CRLB.Add(pCLQ.ChiSqPValue)
      ' CONSTRAINED RANDOM LABELING (other)
      ' set null model to cRLother
      radioCRL.Checked = True
      radFixedOther.Checked = True
      ' run simulations
      initializeCLQ()
      runSims(numSims)
      ' calculate CLQ, p-values, chi-sq, overall p
      T = pCLQ.CLQ
      lCLQ_CRLother.Add(T.Clone)
      T = pCLQ.obsNNCT
      T = pCLQ.pValues
      lP_CRLother.Add(T.Clone)
      lChiSq_CRLother.Add(pCLQ.ChiSqStat)
      lOverallP_CRLother.Add(pCLQ.ChiSqPValue)
      ' TOROIDAL SHIFT
      ' set null model to toroidal shift
      radioToroidalShift.Checked = True
      ' run simulations
      initializeCLQ()
      runSims(numSims)
      ' calculate CLQ, p-values, chi-sq, overall p
      T = pCLQ.CLQ
      lCLQ_TS.Add(T.Clone)
      T = pCLQ.obsNNCT
      T = pCLQ.pValues
      lp_TS.Add(T.Clone)
      lChiSq_TS.Add(pCLQ.ChiSqStat)
      lOverallP_TS.Add(pCLQ.ChiSqPValue)
      ' report progress
      ProgTracker.setCompleted(fileNum + 1)
      Application.DoEvents()
    Next fileNum
    ' convert to data tables
    Dim tbRLp As DataTable = ctListToDataTable(lP_RL, "p")
    Dim tbRLCLQ As DataTable = ctListToDataTable(lCLQ_RL, "CLQ")

    Dim tbCRLAp As DataTable = ctListToDataTable(lP_CRLA, "p")
    Dim tbCRLACLQ As DataTable = ctListToDataTable(lCLQ_CRLA, "CLQ")

    Dim tbCRLBp As DataTable = ctListToDataTable(lP_CRLB, "p")
    Dim tbCRLBCLQ As DataTable = ctListToDataTable(lCLQ_CRLB, "CLQ")

    Dim tbCRLotherP As DataTable = ctListToDataTable(lP_CRLB, "p")
    Dim tbCRLotherCLQ As DataTable = ctListToDataTable(lCLQ_CRLB, "CLQ")

    Dim tbTSp As DataTable = ctListToDataTable(lp_TS, "p")
    Dim tbTSCLQ As DataTable = ctListToDataTable(lCLQ_TS, "CLQ")

    Dim tbOverall As DataTable
    Dim valList() As List(Of Double)
    ReDim valList(9)
    Dim nameList As New List(Of String)
    valList(0) = lChiSq_RL : nameList.Add("ChiSq_RL")
    valList(1) = lChiSq_CRLA : nameList.Add("ChiSq_CRLA")
    valList(2) = lChiSq_CRLB : nameList.Add("ChiSq_CRLB")
    valList(3) = lChiSq_CRLother : nameList.Add("ChiSq_CRLother")
    valList(4) = lChiSq_TS : nameList.Add("ChiSq_TS")

    valList(5) = lOverallP_RL : nameList.Add("OverallP_RL")
    valList(6) = lOverallP_CRLA : nameList.Add("OverallP_CRLA")
    valList(7) = lOverallP_CRLB : nameList.Add("OverallP_CRLB")
    valList(8) = lOverallP_CRLother : nameList.Add("OverallP_CRLother")
    valList(9) = lOverallP_TS : nameList.Add("OverallP_TS")

    tbOverall = valListListToDataTable(valList, nameList)
    ' create summary table
    Dim sumTableList As New List(Of cContingencyTable)
    Dim sumTableNameList As New List(Of String)

    Dim RL_temp As List(Of cContingencyTable) = clqUtils.getCTAvgStDev(lCLQ_RL)
    Dim RL_CLQ_Avg As cContingencyTable = RL_temp(0)
    Dim RL_CLQ_StDev As cContingencyTable = RL_temp(1)
    Dim RL_numSig As cContingencyTable = clqUtils.getSigCount(lP_RL)

    sumTableList.Add(RL_CLQ_Avg)
    sumTableNameList.Add("RL_CLQ_Avg")
    sumTableList.Add(RL_CLQ_StDev)
    sumTableNameList.Add("RL_CLQ_StDev")
    sumTableList.Add(RL_numSig)
    sumTableNameList.Add("RL_NumSig")

    Dim CRLA_temp As List(Of cContingencyTable) = clqUtils.getCTAvgStDev(lCLQ_CRLA)
    Dim CRLA_CLQ_Avg As cContingencyTable = CRLA_temp(0)
    Dim CRLA_CLQ_StDev As cContingencyTable = CRLA_temp(1)
    Dim CRLA_numSig As cContingencyTable = clqUtils.getSigCount(lP_CRLA)

    sumTableList.Add(CRLA_CLQ_Avg)
    sumTableNameList.Add("CRLA_CLQ_Avg")
    sumTableList.Add(CRLA_CLQ_StDev)
    sumTableNameList.Add("CRLA_CLQ_StDev")
    sumTableList.Add(CRLA_numSig)
    sumTableNameList.Add("CRLA_NumSig")

    Dim CRLB_temp As List(Of cContingencyTable) = clqUtils.getCTAvgStDev(lCLQ_CRLB)
    Dim CRLB_CLQ_Avg As cContingencyTable = CRLB_temp(0)
    Dim CRLB_CLQ_StDev As cContingencyTable = CRLB_temp(1)
    Dim CRLB_numSig As cContingencyTable = clqUtils.getSigCount(lP_CRLB)

    sumTableList.Add(CRLB_CLQ_Avg)
    sumTableNameList.Add("CRLB_CLQ_Avg")
    sumTableList.Add(CRLB_CLQ_StDev)
    sumTableNameList.Add("CRLB_CLQ_StDev")
    sumTableList.Add(CRLB_numSig)
    sumTableNameList.Add("CRLB_NumSig")

    Dim TS_temp As List(Of cContingencyTable) = clqUtils.getCTAvgStDev(lCLQ_TS)
    Dim TS_CLQ_Avg As cContingencyTable = TS_temp(0)
    Dim TS_CLQ_StDev As cContingencyTable = TS_temp(1)
    Dim TS_numSig As cContingencyTable = clqUtils.getSigCount(lp_TS)

    sumTableList.Add(TS_CLQ_Avg)
    sumTableNameList.Add("TS_CLQ_Avg")
    sumTableList.Add(TS_CLQ_StDev)
    sumTableNameList.Add("TS_CLQ_StDev")
    sumTableList.Add(TS_numSig)
    sumTableNameList.Add("TS_NumSig")

    Dim summaryTable As DataTable = ctListToDataTable(sumTableList, sumTableNameList)

    ' save tables
    dataFolder = System.IO.Path.GetDirectoryName(dataFolder)
    saveDataTableToCSV(tbRLCLQ, dataFolder & "\RL_CLQ.csv")
    saveDataTableToCSV(tbRLp, dataFolder & "\RL_p.csv")
    saveDataTableToCSV(tbCRLACLQ, dataFolder & "\CRLA_CLQ.csv")
    saveDataTableToCSV(tbCRLAp, dataFolder & "\CRLA_p.csv")
    saveDataTableToCSV(tbCRLBCLQ, dataFolder & "\CRLB_CLQ.csv")
    saveDataTableToCSV(tbCRLBp, dataFolder & "\CRLB_p.csv")
    saveDataTableToCSV(tbCRLotherCLQ, dataFolder & "\CRLother_CLQ.csv")
    saveDataTableToCSV(tbCRLotherP, dataFolder & "\CRLother_p.csv")
    saveDataTableToCSV(tbTSCLQ, dataFolder & "\TS_CLQ.csv")
    saveDataTableToCSV(tbTSp, dataFolder & "\TS_p.csv")
    saveDataTableToCSV(tbOverall, dataFolder & "\overall.csv")
    saveDataTableToCSV(summaryTable, dataFolder & "\Summary.csv")

    ' report finish
    ProgTracker.finishTask()
  End Sub
  Private Sub saveDataTablesToCSV(ByVal dTList As List(Of DataTable), ByVal fileName As String)
    ' saves multiple datatables to a single file!
    Dim SW As New System.IO.StreamWriter(fileName)
    ' loop through list
    For Each dT In dTList
      Dim lineText As String = ""
      ' write header text
      For i = 0 To dT.Columns.Count - 2
        lineText &= dT.Columns(i).ColumnName & ", "
      Next
      lineText &= dT.Columns(dT.Columns.Count - 1).ColumnName
      SW.WriteLine(lineText)
      ' write values
      For rownum = 0 To dT.Rows.Count - 1
        lineText = ""
        For colNum = 0 To dT.Columns.Count - 2
          lineText &= dT.Rows(rownum).Item(colNum).ToString & ", "
        Next colNum
        lineText &= dT.Rows(rownum).Item(dT.Columns.Count - 1).ToString
        SW.WriteLine(lineText)
      Next rownum
    Next dT
    ' close file
    SW.Close()
  End Sub
  Private Sub saveDataTableToCSV(ByVal dT As DataTable, ByVal fileName As String)
    ' saves the datatable to the file!
    Dim SW As New System.IO.StreamWriter(fileName)

    Dim lineText As String = ""
    ' write header text
    For i = 0 To dT.Columns.Count - 2
      lineText &= dT.Columns(i).ColumnName & ", "
    Next
    lineText &= dT.Columns(dT.Columns.Count - 1).ColumnName
    SW.WriteLine(lineText)
    ' write values
    For rownum = 0 To dT.Rows.Count - 1
      lineText = ""
      For colNum = 0 To dT.Columns.Count - 2
        lineText &= dT.Rows(rownum).Item(colNum).ToString & ", "
      Next
      lineText &= dT.Rows(rownum).Item(dT.Columns.Count - 1).ToString
      SW.WriteLine(lineText)
    Next
    ' close file
    SW.Close()
  End Sub
  Private Function valListListToDataTable(ByVal vListArray As List(Of Double)(),
                                          Optional ByVal nameList As List(Of String) = Nothing) As DataTable
    ' combines value lists into a single data table
    ' get number of lists
    Dim numLists As Integer = vListArray.Length
    Dim numRows As Integer = vListArray(0).Count
    ' handle name list
    If nameList Is Nothing Then
      nameList = New List(Of String)
      For i = 1 To numLists
        nameList.Add("Var" & i.ToString)
      Next i
    End If
    ' set up result table
    Dim R As New DataTable
    Dim dummyDbl As Double = 3.1415
    Dim dblType As System.Type = dummyDbl.GetType
    For Each individualName In nameList
      R.Columns.Add(individualName, dblType)
    Next
    ' add rows to results table
    Dim valArray() As Double
    ReDim valArray(numLists - 1)
    Dim newRow As DataRow
    For rowNum = 0 To numRows - 1
      For listNum = 0 To numLists - 1
        valArray(listNum) = vListArray(listNum).Item(rowNum)
      Next listNum
      newRow = R.NewRow()
      For colNum = 0 To UBound(valArray)
        newRow.Item(colNum) = valArray(colNum)
      Next
      R.Rows.Add(newRow)
    Next rowNum

    ' return result
    Return R
  End Function
  Private Overloads Function ctListToDataTable(ByVal ctList As List(Of cContingencyTable),
                                               ByVal nameList As List(Of String)) As DataTable
    ' creates a datatable object containing the data from the contingency tables
    ' as subtables in sequence
    ' assumes that all inputs have the same number of classes
    Dim R As New DataTable
    Dim dummyDbl As Double = 3.1415
    Dim dblType As System.Type = dummyDbl.GetType
    Dim dummyStr As String = "3.1415"
    Dim strType As System.Type = dummyStr.GetType
    Dim colName As String
    ' get/set dimensions
    Dim nClasses As Integer = ctList(0).numClasses
    Dim classNames As String() = ctList(0).classNames
    Dim nCols As Integer = nClasses + 2
    Dim nRows As Integer = ctList.Count * nClasses
    ' define columns
    colName = "Table"
    R.Columns.Add(colName, strType)
    colName = "FromCat"
    R.Columns.Add(colName, strType)
    For i = 0 To nClasses - 1
      colName = classNames(i)
      R.Columns.Add(colName, dblType)
    Next
    ' loop through tables
    For ctNum = 0 To ctList.Count - 1
      Dim CT As cContingencyTable = ctList.Item(ctNum)
      Dim ctName As String = nameList.Item(ctNum)
      ' loop through rows
      For row = 0 To nClasses - 1
        ' get data row
        Dim DR As DataRow = R.NewRow
        ' get data table, from class
        DR.Item(0) = ctName
        DR.Item(1) = classNames(row)
        ' get values to To class
        For col = 0 To nClasses - 1
          DR.Item(2 + col) = CT.Value(row, col)
        Next col
        ' add to result
        R.Rows.Add(DR)
      Next row
    Next ctNum
    ' return data table
    Return R
  End Function
  Private Overloads Function ctListToDataTable(ByVal ctList As List(Of cContingencyTable),
                                     Optional ByVal prefix As String = "") As DataTable
    ' creates a datatable object containing the data from the contingency tables
    ' columns are the classes (e.g. "AB", "AD", "DB")
    ' rows are the values
    Dim R As New DataTable
    Dim dummyDbl As Double = 3.1415
    Dim dblType As System.Type = dummyDbl.GetType
    Dim colName As String
    ' get/set dimensions
    Dim nClasses As Integer = ctList(0).numClasses
    Dim nCols As Integer = nClasses ^ 2
    Dim nRows As Integer = ctList.Count
    For fromClass = 1 To nClasses
      For toClass = 1 To nClasses
        colName = prefix & "F" & fromClass.ToString & "T" & toClass.ToString
        R.Columns.Add(colName, dblType)
      Next toClass
    Next fromClass
    ' add rows
    Dim rowVal() As Double
    For Each T In ctList
      rowVal = T.valueArray
      Dim newRow As DataRow = R.NewRow
      For i = 0 To UBound(rowVal)
        newRow.Item(i) = rowVal(i)
      Next
      R.Rows.Add(newRow)
    Next T
    Return R
  End Function
  Private Sub btnBatchProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBatchProcess.Click
    ' get file name
    Dim dlgOpenFolder As New FolderBrowserDialog
    Dim dlgResult As DialogResult
    dlgResult = dlgOpenFolder.ShowDialog
    If dlgResult = DialogResult.OK Then
      Dim pathName As String = dlgOpenFolder.SelectedPath
      '      batchProcessNoSims(pathName, "Mark", 5)
      batchProcess(pathName, "Mark", 3, 100)
    End If
  End Sub
#End Region
#Region "Null Model"
  Private Function getSelectedNullModel() As cCoLocationEngine.eNullModel
    Dim nullModel As cCoLocationEngine.eNullModel
    If radioCSR.Checked Then nullModel = cCoLocationEngine.eNullModel.CSR
    If radioToroidalShift.Checked Then nullModel = cCoLocationEngine.eNullModel.ToroidalShift
    If radioRL.Checked Then nullModel = cCoLocationEngine.eNullModel.RL
    If radioCRL.Checked Then
      If radFixedBase.Checked Then nullModel = cCoLocationEngine.eNullModel.CRL_FixedBase
      If radFixedNeighbor.Checked Then nullModel = cCoLocationEngine.eNullModel.CRL_FixedNeighbor
      If radFixedOther.Checked Then nullModel = cCoLocationEngine.eNullModel.CRL_FixedOther
      If radDesignateFixed.Checked Then nullModel = cCoLocationEngine.eNullModel.CRL_FixedCat
    End If
    Return nullModel
  End Function
  Private Sub showNullModelControls()
    If radioCRL.Checked = False Then
      listFixed.Visible = False
      panelRestrictionOptions.Visible = False
    Else
      panelRestrictionOptions.Visible = True
      If radDesignateFixed.Checked Then
        listFixed.Visible = True
      End If
    End If
    resizePanels()
  End Sub
  Private Sub changeNullModel()
    ' adjust control visibility
    showNullModelControls()
    ' check to see if selection changed
    Dim newNullModel As cCoLocationEngine.eNullModel = getSelectedNullModel()
    If newNullModel <> lastSelectedNullModel Then
      lastSelectedNullModel = newNullModel
      initializeCLQ()
      resetSims()
    End If
  End Sub
  Private Sub panelNullModel_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles pnlNullModel.Enter
    If aggClasses Is Nothing Then Exit Sub
    ' populate randomized list
    If listFixed.Items.Count = 0 Then
      ' get categories from dgv
      Dim CList As List(Of Object) = aggClasses.ToList
      ' add to listbox
      For Each C In CList
        listFixed.Items.Add(C)
      Next
    End If
  End Sub
  Private Sub radDesignateFixed_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radDesignateFixed.CheckedChanged
    listFixed.Visible = radDesignateFixed.Checked
    changeNullModel()
  End Sub
  Private Sub radioCRL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioCRL.CheckedChanged
    changeNullModel()
  End Sub
  Private Sub radioCSR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioCSR.CheckedChanged
    changeNullModel()
  End Sub
  Private Sub radioToroidalShift_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioToroidalShift.CheckedChanged
    changeNullModel()
  End Sub
  Private Sub radioRL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioRL.CheckedChanged
    changeNullModel()
  End Sub
  Private Sub radFixedBase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radFixedBase.CheckedChanged
    changeNullModel()
  End Sub
  Private Sub radFixedNeighbor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radFixedNeighbor.CheckedChanged
    changeNullModel()
  End Sub
  Private Sub radFixedOther_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radFixedOther.CheckedChanged
    changeNullModel()
  End Sub
  Private Sub listFixed_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listFixed.SelectedIndexChanged
    changeNullModel()
  End Sub


#End Region
#Region "Simulation"
  Private Sub btnRunOnce_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunOnce.Click
    runSims(1)
  End Sub
  Private Sub btnRunMany_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunMany.Click
    ' if user clicks this button, force results to show and also show time
    ' error checking
    If popPtLayer Is Nothing Then
      MsgBox("Please load population data.")
      Exit Sub
    End If
    If cmbFields.SelectedItem Is Nothing Then
      MsgBox("Please select Category Field.")
      Exit Sub
    End If
    ' progress
    ProgTracker.initializeTask("Testing null model...")

    ' see if we need to initialize
    If pCLQ Is Nothing Then
      initializeCLQ()
    Else
      Dim newNullModel As cCoLocationEngine.eNullModel = getSelectedNullModel()
      If newNullModel <> lastSelectedNullModel Then
        resetSims()
        initializeCLQ()
      End If
    End If
    ' always show results after clicking Run Many
    Dim previouslyShow As Boolean = showResults
    showResults = True
    ' run simulations
    runSims(udNumSims.Value)

    showResults = previouslyShow
    ' report progress
    ProgTracker.initializeTask("Calculating statistics...")


    ProgTracker.finishTask("Calculating statistics...")
    ' report finish
    ProgTracker.finishTask("Testing Null Model")
    lblProgress.Text = "- idle -"
  End Sub
  Private Sub runSims(ByVal numSims As Integer)
    ' see if we have a new null model
    changeNullModel()
    ' reclassifier
    If pCLQ Is Nothing Then Exit Sub
    ' progress
    '    If showResults Then frmCLQ.pnlStatus.Visible = True
    ' run
    If showResults Then
      pCLQ.runSims(numSims, ProgTracker)
    Else
      pCLQ.runSims(numSims)
    End If
    ' If showResults Then frmCLQ.pnlStatus.Visible = False
    If showResults Then
      ' show results
      frmMap.showSimClasses()
      ' update number of simulations
      lblSimCompleted.Text = pCLQ.numSimsCompleted.ToString & " sims completed"
      frmTable.updateNumSims()
      frmTable.showStats()
    End If
  End Sub
  Private Sub resetSims()
    If Not pCLQ Is Nothing Then
      pCLQ.resetSims()
      If Not frmTable Is Nothing Then frmTable.updateNumSims()
      lblSimCompleted.Text = "Simulations completed: " &
        pCLQ.numSimsCompleted.ToString
    End If

  End Sub
  Private Sub btnResetSims_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetSims.Click
    resetSims()
  End Sub
  Private Sub btnCompareNullModels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompareNullModels.Click
    compareNullModels(udNumSims.Value)
  End Sub
  Private Sub btnStopSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStopSim.Click
    ' pauses simulation
    ' error checking
    If Not pCLQ Is Nothing Then
      ' pause simulation
      pCLQ.interruptSims()
    End If
  End Sub
  ' change point classes to simulated classes for visualization
  Private Sub btnSaveSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveSim.Click
    ' saves last simulation as a shapefile
    ' error checking
    If pCLQ Is Nothing Then Exit Sub
    If pCLQ.numSimsCompleted = 0 Then Exit Sub
    ' get file from user
    Dim dlgSave As New SaveFileDialog
    dlgSave.Title = "Choose file to save last simulation:"
    dlgSave.Filter = "Shapefiles (*.shp)|*.shp"
    Dim dlgRes As DialogResult = dlgSave.ShowDialog
    If dlgRes = DialogResult.OK Then
      pCLQ.lastSim_asFeatureSet.SaveAs(dlgSave.FileName, True)
    End If
  End Sub
#End Region
#Region "Miscellaneous"
  Public Sub compareNullModels(Optional ByVal numSims As Integer = 1000)
    ' compares nearest neighbor count distributions 
    ' from two null models for same data
    Dim CLQ1, CLQ2 As cCoLocationEngine
    ' model #1 information
    Dim model1name As String = "Fixed base RL"
    radioCRL.Checked = True
    radFixedBase.Checked = True
    initializeCLQ()
    CLQ1 = pCLQ
    ' reset object
    pCLQ = Nothing
    ' model #2 information
    Dim model2name As String = "Toroidal Shift"
    radioToroidalShift.Checked = True
    initializeCLQ()
    initializeCLQ()
    CLQ2 = pCLQ
    ' get output file name from user
    Dim outFileName As String
    Dim dlgSave As New SaveFileDialog
    dlgSave.Title = "Designate output file:"
    dlgSave.Filter = "Comma-delimited text files (*.csv)|*.csv"
    Dim dlgRes As DialogResult = dlgSave.ShowDialog
    If dlgRes = DialogResult.Cancel Then Exit Sub
    outFileName = dlgSave.FileName
    ' display feedback on status to user
    frmCLQ.pnlStatus.Visible = True
    ' create and capture null model 1 simulations
    ProgTracker.initializeTask(model1name)
    CLQ1.runSims(numSims, ProgTracker)
    ProgTracker.finishTask(model1name)
    ' create and capture null model 2 simulations
    ProgTracker.initializeTask(model2name)
    CLQ2.runSims(numSims, ProgTracker)
    ProgTracker.finishTask(model2name)

    ' compare results using T test
    Dim p As cContingencyTable = clqUtils.compareNNCTs(CLQ1, CLQ2)
    ' save results
    Using outFile As New System.IO.StreamWriter(outFileName, System.IO.FileMode.CreateNew)
      outFile.Write("Observed NNCT:" & vbCrLf)
      outFile.Write(CLQ1.obsNNCT.valuesAsText & vbCrLf)
      outFile.Write(model1name & " Avg NNCT:" & vbCrLf)
      outFile.Write(CLQ1.avgSimNNCT.valuesAsText & vbCrLf)
      outFile.Write(model2name & " Avg NNCT:" & vbCrLf)
      outFile.Write(CLQ2.avgSimNNCT.valuesAsText & vbCrLf)
      outFile.Write("Comparison (Mann-Whitney 2-tailed significance values)" & vbCrLf)
      outFile.Write(p.valuesAsText & vbCrLf)
      ' stop displaying feedback
      frmCLQ.pnlStatus.Visible = False
    End Using
  End Sub
  Private Sub btnGetStdDev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetStdDev.Click
    ' runs the given number of simulations 100x
    ' and gets the st. dev. of CLQ values for each cell in table
    Dim resultList As New List(Of cContingencyTable)
    For i = 1 To 100
      runSims(udNumSims.Value)
      resultList.Add(pCLQ.CLQ)
      ' report progress
      lblProgress.Text = "finished " & Str(i) & " of 100"
      Application.DoEvents()
    Next
    ' get standard deviations
    Dim avg_stdev As List(Of cContingencyTable)
    avg_stdev = clqUtils.getCTAvgStDev(resultList)
    ' copy to clipboard
    Dim cliptxt As String = avg_stdev(0).valuesAsText
    cliptxt &= vbCrLf & vbCrLf
    cliptxt &= avg_stdev(1).valuesAsText
    Clipboard.SetText(cliptxt)
    ' report
    MsgBox("Results copied to clipboard.")
  End Sub

  Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

  End Sub
#End Region




End Class