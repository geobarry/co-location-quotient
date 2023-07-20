Imports DotSpatial.Data
Imports DotSpatial.Modeling
Imports DotSpatial.Symbology
Imports DotSpatial.Topology
Imports DotSpatial.Controls
Public Class frmMap
  Dim doResize As Boolean = False
#Region "Resize Events"

  Private Sub frmMap_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ' control controls
    cmbObsOrSim.SelectedIndex = 0

  End Sub
  Private Sub mapForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
    If Not doResize Then Exit Sub
    ' get size of client window
    Dim cSize As Size = frmCLQ.getMDIClientSize
    ' see if this is in the focus window (right) or in the bottom left
    If frmCLQ.currentBigFormID = frmCLQ.primaryFormID.mapForm Then
      ' right
      frmCLQ.hSplitPos = cSize.Width - Me.Width
    Else
      ' bottom left
      frmCLQ.hSplitPos = Me.Width
      If frmCLQ.controlFormVisible Then frmCLQ.vSplitPos = cSize.Height - Me.Height
    End If
    ' rearrange windows
    frmCLQ.arrangeWindows()
  End Sub
  Private Sub mapForm_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeBegin
    doResize = True
  End Sub
  Private Sub mapForm_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
    doResize = False
  End Sub
#End Region
#Region "Labels & Colors"
  Public Sub drawLabels()
    ' let's try labeling the points
    Dim TargetLayer As IMapLayer = frmControl.popPtLayer
    Dim DS As IDataSet = targetLayer.DataSet
    Dim FS As IFeatureSet = DS
    Dim FL As IFeatureLayer = TargetLayer
    Dim DT As DataTable = FS.DataTable
    Dim LG As Legend = legendMain
    Dim columnName As String = frmControl.cmbFields.SelectedItem
    columnName = columnName.Substring(0, columnName.LastIndexOf("[") - 1)
    columnName = "[" & columnName & "]"
    mapMain.AddLabels(FL, columnName, New Font("Arial", 12), System.Drawing.Color.Black)
    ' wish the following would work:
    'Dim X As ILabelSymbolizer = New LabelSymbolizer
    'X.OffsetX = 0
    'X.FontFamily = "Arial"
    'X.FontColor = Color.Black
    'X.Orientation = ContentAlignment.MiddleRight
    '  mapMain.AddLabels(FL, columnName, columnName & "=" & columnName, X, 2.0)
    ' try another way
    'Dim featLabelLyr As New MapLabelLayer
    'FL.ShowLabels = True
    'featLabelLyr.Symbology.Categories(0).Expression = columnName
    'featLabelLyr.Symbolizer.Orientation = ContentAlignment.TopCenter
    'mapMain.Refresh()
    FL.LabelLayer.Symbolizer.OffsetX = 6
    FL.LabelLayer.Symbolizer.Alignment = StringAlignment.Far
    FL.LabelLayer.Symbolizer.FontSize = 12
    FL.LabelLayer.Symbolizer.Orientation = ContentAlignment.MiddleRight
    mapMain.Refresh()
  End Sub
  Public Sub hideLabels()
    mapMain.ClearLabels(frmControl.popPtLayer)
  End Sub
#End Region
  Private Sub chkLabels_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLabels.CheckedChanged
    If frmControl.cmbFields.SelectedIndex > -1 Then
      If chkLabels.Checked Then drawLabels() Else hideLabels()
    End If
  End Sub
  Public Sub showSimClasses()
    If frmControl.popPtLayer Is Nothing Then Exit Sub
    Dim DS As IDataSet = frmControl.popPtLayer.DataSet
    Dim FS As IFeatureSet = DS
    Dim DT As DataTable = FS.DataTable
    If frmControl.cmbFields.SelectedIndex < 0 Then Exit Sub
    Dim columnName As String = frmControl.cmbFields.SelectedItem
    columnName = columnName.Substring(0, columnName.LastIndexOf("[") - 1)
    Dim simClassID() As Integer
    If cmbObsOrSim.SelectedIndex = 1 Then
      simClassID = frmControl.pCLQ.lastSimCats
    Else
      simClassID = frmControl.pCLQ.obsCats
    End If
    ' change values in class column
    'Dim dummy As Boolean = True

    'For i = 0 To DT.Rows.Count - 1
    '  DT.Rows(i).Item(columnName) = simClassID(i)
    'Next
    ' force map to update
    chkLabels.Checked = Not (chkLabels.Checked)
    chkLabels.Checked = Not (chkLabels.Checked)
  End Sub

  Private Sub cmbObsOrSim_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbObsOrSim.SelectedIndexChanged
    showSimClasses()
  End Sub

  Private Sub btnMapSwitch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMapSwitch.Click
    frmCLQ.switchMapAndTable()
  End Sub

#Region "Zoom & Pan Options"
  Private Sub setZoomMode()
    If radZoomIn.Checked Then
      mapMain.ActivateMapFunction(New MapFunctionClickZoom(mapMain))
    ElseIf radZoomOut.Checked Then
      mapMain.ActivateMapFunction(New MapFunctionZoomOut(mapMain))
    ElseIf radPan.Checked Then
      mapMain.ActivateMapFunction(New MapFunctionPan(mapMain))
    End If
  End Sub
  Private Sub radZoomIn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radZoomIn.CheckedChanged
    setZoomMode()
  End Sub
  Private Sub radPan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radPan.CheckedChanged
    setZoomMode()
  End Sub

  Private Sub radZoomOut_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radZoomOut.CheckedChanged
    setZoomMode()
  End Sub

#End Region
End Class