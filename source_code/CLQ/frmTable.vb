Public Class frmTable
  Dim doResize As Boolean = False
  Dim inMainWindow As Boolean = False
  ' state variables for experimental vs. deployment versions
  Dim showTableSrc As Boolean = False
  Public Enum eTableFormat
    Fixed_Width = 0
    Tab_Delimited = 1
  End Enum
#Region "Resize Events"
  Private Sub frmTable_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ' set up controls
    panelTableTop.Height = numDecimalPlaces.Height
    Dim statTypes As List(Of String) = misc.enumUtils.enumNames(GetType(cCoLocationEngine.eStatType))
    For Each statType In statTypes
      cmbStatType.Items.Add(statType)
    Next
    cmbStatType.SelectedIndex = 0
    Dim formats As List(Of String) = misc.enumUtils.enumNames(GetType(eTableFormat))
    For Each tableFormat In formats
      cmbTableFormat.Items.Add(tableFormat)
    Next
    cmbTableFormat.SelectedIndex = 0
  End Sub
  Private Sub tableForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
    If Not doResize Then Exit Sub
    ' get size of client window
    Dim cSize As Size = frmCLQ.getMDIClientSize
    ' see if this is in the focus window (right) or in the bottom left
    If frmCLQ.currentBigFormID = frmCLQ.primaryFormID.mapForm Then
      ' bottom left
      frmCLQ.hSplitPos = Me.Width
      If frmCLQ.controlFormVisible Then frmCLQ.vSplitPos = cSize.Height - Me.Height

    Else
      ' right
      frmCLQ.hSplitPos = cSize.Width - Me.Width
    End If
    ' rearrange windows
    frmCLQ.arrangeWindows()
  End Sub
  Private Sub tableForm_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeBegin
    doResize = True
  End Sub
  Private Sub tableForm_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
    doResize = False
  End Sub
#End Region
  Public Sub showStats()
    ' error checking
    If frmControl.pCLQ Is Nothing Then Exit Sub
    Dim Engine As cCoLocationEngine = frmControl.pCLQ
    ' get number of decimal places
    Dim numDP As Integer = numDecimalPlaces.Value
    ' show!
    '    Engine.showInDGV(dgvResults, numDP)
    Dim statType As cCoLocationEngine.eStatType = [Enum].Parse(GetType(cCoLocationEngine.eStatType), cmbStatType.SelectedItem)
    Dim tableFormat As eTableFormat = [Enum].Parse(GetType(eTableFormat), CStr(cmbTableFormat.SelectedItem).Replace(" ", "_"))
    Dim sepChr As String = " "
    Select Case tableFormat
      Case Is = eTableFormat.Fixed_Width
        sepChr = " "
      Case Is = eTableFormat.Tab_Delimited
        sepChr = vbTab
    End Select
    Engine.showInRTB(rtbResults, statType, 10, numDP, sepChr)
  End Sub

  Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    ' updates the stats displayed in the data table
    ' based on the combo box selections
    showStats()
  End Sub
  Public Sub updateNumSims()
    ' updates the limits of the numericUpDown control specifying the sim ID
    Dim N As Integer = frmControl.pCLQ.numSimsCompleted
    If N = 0 Then rtbResults.Text = ""
  End Sub
  Private Sub cmbStatType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbStatType.SelectedIndexChanged
    showStats()
  End Sub

  Private Sub btnTableSwitch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTableSwitch.Click
    frmCLQ.switchMapAndTable()
  End Sub

  Private Sub numDecimalPlaces_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles numDecimalPlaces.ValueChanged
    showStats()
  End Sub


  Private Sub cmbFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTableFormat.SelectedIndexChanged
    showStats()
  End Sub


End Class