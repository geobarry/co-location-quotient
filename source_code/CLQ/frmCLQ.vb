Public Class frmCLQ
  'Co-Location Quotient - A Measure of Spatial Association Between Point Subpopulations
  'Copyright (C) 20112  Barry J. Kronenfeld

  'This program is free software: you can redistribute it and/or modify
  'it under the terms of the GNU General Public License as published by
  'the Free Software Foundation, either version 3 of the License, or
  '(at your option) any later version.

  'This program is distributed in the hope that it will be useful,
  'but WITHOUT ANY WARRANTY; without even the implied warranty of
  'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  'GNU General Public License for more details.

  'A copy of the GNU General Public License
  'is available at <http://www.gnu.org/licenses/>.
  Friend controlFormVisible As Boolean = True
  Friend bigFormVisible As Boolean = True
  Friend vSplitPos As Integer
  Friend hSplitPos As Integer
  Friend Enum primaryFormID
    controlForm = 0
    tableForm = 1
    mapForm = 2
  End Enum
  Friend currentBigFormID As primaryFormID = primaryFormID.controlForm
  Friend mapInMainWindow As Boolean = True
  ' form shortcuts
  Public tableForm As frmTable = frmTable
  Public controlForm As frmControl = frmControl
  Public mapForm As frmMap = frmMap
  Public Function otherFormVisible() As Boolean
    ' determines if the form below the control form is visible
    Return otherForm.Visible
  End Function
  Public Function bigForm() As Form
    ' returns a reference to the form to the right of the control form
    If currentBigFormID = primaryFormID.mapForm Then Return mapForm Else Return tableForm
  End Function
  Public Function otherForm() As Form
    ' returns a reference to the form below the control form
    If currentBigFormID = primaryFormID.mapForm Then Return tableForm Else Return mapForm

  End Function
#Region "Program Setup"
  Private Sub frmCLQ_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ' run any testing and debugging
    initTest()
    ' set up forms
    frmTable.MdiParent = Me
    frmMap.MdiParent = Me
    frmControl.MdiParent = Me
    frmTable.FormBorderStyle = Windows.Forms.FormBorderStyle.None
    frmMap.FormBorderStyle = Windows.Forms.FormBorderStyle.None
    frmControl.FormBorderStyle = Windows.Forms.FormBorderStyle.None
    frmTable.Show()
    '    frmMap.Show()
    frmControl.Show()
    ' determine starting window split positions
    Dim cSize As Size = getMDIClientSize()
    'frmControl.recordPanelHeights()
    vSplitPos = 377
    hSplitPos = controlForm.panelMaxWidth
    arrangeWindows()
    '    switchMapAndTable()
    ' make map form invisible
    frmMap.Visible = False
    frmTable.btnTableSwitch.Visible = False
    '   arrangeWindowsOnStartup()

  End Sub
#End Region
#Region "Window Management"
  Private Sub arrangeWindowsOnStartup()
    Dim vertPos, hzPos As Integer
    vertPos = Me.getMDIClientSize.Height ' makes map form invisible
    hzPos = Me.getMDIClientSize.Width / 3
    frmControl.Location = New System.Drawing.Point(0, 0)
    frmControl.Width = hzPos
    frmControl.Height = vertPos
    frmMap.Location = New System.Drawing.Point(0, vertPos)
    frmMap.Width = hzPos
    frmMap.Height = vertPos
    frmTable.Location = New System.Drawing.Point(hzPos, 0)
    frmTable.Width = Me.getMDIClientSize.Width - hzPos
    frmTable.Height = Me.getMDIClientSize.Height
  End Sub
  Friend Sub arrangeWindows()
    ' determine focus window
    Dim bigForm, otherForm As Form
    If currentBigFormID = primaryFormID.mapForm Then
      bigForm = frmMap
      otherForm = frmTable
    Else
      bigForm = frmTable
      otherForm = frmMap
    End If
    ' arrange forms
    Dim cSize As Size = getMDIClientSize()
    ' control form
    frmControl.Location = New System.Drawing.Point(0, 0)
    frmControl.Width = hSplitPos
    If otherFormVisible() Then
      frmControl.Height = vSplitPos
    Else
      frmControl.Height = cSize.Height
    End If
    ' stretch the panels to the width of the flow layout control
    For Each ctl As Control In frmControl.flowSteps.Controls
      ctl.Width = frmControl.flowSteps.Width - ctl.Margin.Left - ctl.Margin.Right
    Next
    ' other form on left side
    otherForm.Location = New System.Drawing.Point(0, frmControl.Height)
    otherForm.Width = frmControl.Width
    If controlFormVisible Then
      otherForm.Height = cSize.Height - frmControl.Height
    Else
      otherForm.Height = cSize.Height
    End If
    ' focus form on right side
    If otherFormVisible() = False And controlFormVisible = False Then
      bigForm.Location = New System.Drawing.Point(0, 0)
      bigForm.Width = cSize.Width
    Else
      bigForm.Location = New System.Drawing.Point(frmControl.Width + 2, 0)
      bigForm.Width = cSize.Width - frmControl.Width - 2
    End If
    bigForm.Height = cSize.Height
  End Sub
  Friend Function getMDIClientSize() As Size
    For Each ctl As Control In Me.Controls
      If TypeOf ctl Is MdiClient Then
        Return ctl.ClientSize
      End If
    Next
  End Function
  Public Sub switchMapAndTable()
    ' switches focus between map and table
    If currentBigFormID = primaryFormID.mapForm Then
      currentBigFormID = primaryFormID.tableForm
      'FocusWindowToolStripMenuItem.Text = "Show Map in Main Window"
      ' check if table form is invisible
      If frmTable.Visible = False Then
        frmTable.Visible = True
        frmMap.Visible = False
      End If
    Else
      currentBigFormID = primaryFormID.mapForm
      '  FocusWindowToolStripMenuItem.Text = "Show Table in Main Window"
      ' check if map form is invisible
      If frmMap.Visible = False Then
        frmMap.Visible = True
        frmTable.Visible = False
      End If
    End If
    arrangeWindows()
    ' change buttons
    If mapInMainWindow Then
      frmTable.btnTableSwitch.Image = My.Resources.downLeftTriangle
      frmMap.btnMapSwitch.Image = My.Resources.upRightTriangle
    Else
      frmTable.btnTableSwitch.Image = My.Resources.upRightTriangle
      frmMap.btnMapSwitch.Image = My.Resources.downLeftTriangle
    End If
    mapInMainWindow = Not mapInMainWindow
  End Sub
  ' code below is for menu items to close & switch windows around
  'Private Sub FocusWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FocusWindowToolStripMenuItem.Click
  '  switchMapAndTable()
  'End Sub
  'Private Sub ControlPanelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ControlPanelToolStripMenuItem.Click
  '  frmControl.Visible = Not frmControl.Visible
  '  controlFormVisible = Not controlFormVisible
  '  arrangeWindows()
  'End Sub
  'Private Sub TableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TableToolStripMenuItem.Click
  '  frmTable.Visible = Not frmTable.Visible
  '  otherFormVisible = Not otherFormVisible
  '  arrangeWindows()
  'End Sub
  'Private Sub MapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MapToolStripMenuItem.Click
  '  frmMap.Visible = Not frmMap.Visible
  '  otherFormVisible = Not otherFormVisible
  '  arrangeWindows()
  'End Sub
  Private Sub frmCLQ_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
    arrangeWindows()
  End Sub
#End Region
#Region "Testing and Debugging"
  Private Sub initTest()
    testTopNbrSelection()
  End Sub
  Private Sub testTopNbrSelection()
    Dim nbrs As New List(Of Neighbor)
    Dim wts() As Double = {0.5, 0.3, 0.2}
    nbrs.Add(New Neighbor(2, 5))
    nbrs.Add(New Neighbor(5, 6))
    nbrs.Add(New Neighbor(0, 3))
    nbrs.Add(New Neighbor(3, 5))
    nbrs.Add(New Neighbor(4, 4))
    nbrs.Add(New Neighbor(1, 2))
    
    Dim R As List(Of cCoLocationEngine.sNeighborRel) = ToroidalShift_Simulator.topNbrs(nbrs, wts)
    For Each E In R
      Console.WriteLine(E.nbID.ToString & ": " & E.nbWeight.ToString)
    Next
  End Sub
  Private Sub testToroidalShift()
    ' create dataset
    Dim x(), y() As Double
    ReDim x(5) : ReDim y(5)
    ' points 1 & 2 are nearest neighbors
    x(0) = 0.01
    y(0) = 0.25
    x(1) = 0.99
    y(1) = 0.25
    ' points 3 & 4 are nearest neighbors
    x(2) = 0.01
    y(2) = 0.01
    x(3) = 0.99
    y(3) = 0.99
    ' points 5 & 6 are normal
    x(4) = 0.4
    y(4) = 0.4
    x(5) = 0.5
    y(5) = 0.5
    ' convert to sPoints
    Dim p() As twoDTree.sPoint
    ReDim p(x.Length - 1)
    For i = 0 To x.Length - 1
      p(i).x = x(i)
      p(i).y = y(i)
    Next

    ' create bounding box
    Dim bBox As twoDTree.sBox
    bBox.Left = 0
    bBox.Bottom = 0
    bBox.Right = 1
    bBox.Top = 1
    ' create index
    Dim toroidIndex As New toroidalIndex(bBox)
    toroidIndex.addPoints(p)
    ' get list of nearest neighbors
    Dim nbLList As New List(Of List(Of Neighbor)) ' = toroidIndex.nearestNeighborListList(1) [function has been removed]

    ' report information from list
    For i = 0 To nbLList.Count - 1
      Dim nbList As List(Of Neighbor) = nbLList(i)
      Console.Write(i.ToString & ": ")
      For Each nb In nbList
        Console.Write(nb.ID.ToString & ", ")
      Next
      Console.Write(vbCrLf)
    Next
  End Sub
  Private Sub testContingencyTable()
    ' requires bypass of cCoLocationEngine initialization, which is no longer implemented
    ' because it passed the test
    'Dim C As New cCoLocationEngine(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, True)
    '    C.testCalcContingencyTable()
  End Sub
  Private Sub testUniqueValuesFunction()
    ' test unique values function
    Dim x() As String = {"a", "b", "a", "c", "a"}
    Dim y As List(Of Object) = Data.Lookup.uniqueValueList(x)
    For Each a In y
      Debug.Print(a.ToString)
    Next
  End Sub
  Private Sub testMannWhitneyU()
    Dim S As New StatisticsCalculator
    Dim U As Integer = S.test()
    Dim Z As Double = S.MannWhitneyZScore({1, 7, 8, 9, 10, 11}, {2, 3, 4, 5, 6, 12})
    S.Open()
    Dim p As Double = S.Zsig_oneTail(Z)
    S.Close()
    Console.Write(U)
  End Sub
#End Region


  Private Sub menuAbout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles menuAbout.Click
    frmLicense.ShowDialog()
  End Sub
End Class
