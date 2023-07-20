Module CLQ
#Region "CLQ Calculation"

  Private Function getCLQ(ByVal NN As List(Of List(Of Neighbor)), _
                       ByVal catID() As Integer, _
                       ByVal numCats As Integer) As Double(,)
    ' returns the co-location for each category pair
    ' note: you must calculate the nearest neighbors first
    ' this function can be re-used for Monte Carlo simulation by providing different catIDs
    Dim R(,) As Double ' result
    Dim cCat() As Double ' count of points in each category
    Dim cAB(,) As Double ' count of As whose nearest neighbor is B
    Dim nA As Double ' count of As
    Dim nB As Double ' count of Bs
    Dim curCat As Integer ' category of current point
    Dim nbCat As Integer ' category of current point's neighbor
    Dim nbList As List(Of Neighbor)
    Dim curNeighbor As Neighbor
    Dim numNeighbors As Integer
    Dim i, j As Integer
    ' set up arrays
    ReDim R(numCats - 1, numCats - 1)
    ReDim cAB(numCats - 1, numCats - 1)
    ReDim cCat(numCats - 1)
    ' loop through the points in the shapefile
    For i = 0 To NN.Count - 1
      ' get category of current point
      curCat = catID(i)
      ' increment count of points in this category
      cCat(curCat) += 1
      ' loop through neighbors
      nbList = NN.Item(i)
      numNeighbors = nbList.Count
      For j = 0 To numNeighbors - 1
        ' get neighboring category
        curNeighbor = nbList.Item(j)
        nbCat = catID(curNeighbor.ID)
        ' increment count of As with B as its nearest neighbor
        If numNeighbors = 1 Then
          cAB(curCat, nbCat) += 1
        Else
          cAB(curCat, nbCat) += 1 / numNeighbors
        End If
      Next j
    Next i
    ' now loop through pairs of categories
    For i = 0 To numCats - 1
      For j = 0 To numCats - 1
        ' transfer values to variables with recognizable names
        nA = cCat(i)
        If i = j Then
          nB = cCat(j) - 1
        Else
          nB = cCat(j)
        End If
        ' calculate CLQ
        If nB = 0 Then
          R(i, j) = -1
        Else
          R(i, j) = (cAB(i, j) / nA) / (nB / (NN.Count - 1))
        End If
      Next
    Next
    ' return result
    Return R
  End Function
  'Private Sub calcCLQandMonteCarlo()
  '  ' get categores
  '  getCategories(catID, catList, Tracker)
  '  ' get nearest neighbor list
  '  NN = treeCLQ.nearestNeighborList(Tracker)
  '  ' calculate CLQ
  '  CLQ = getCLQ(NN, catID, catList.Count)
  '  ' prepare for Monte Carlo simulations
  '  Dim simNum, numSims As Integer
  '  Dim curSimCLQ(,) As Double
  '  Dim simCLQ(,,) As Double
  '  Dim cat1, cat2 As Integer
  '  If IsNumeric(txtNumSims.Text) Then
  '    ' get number of simulations
  '    numSims = Val(txtNumSims.Text)
  '    ReDim simCLQ(catList.Count - 1, catList.Count - 1, numSims - 1)
  '    ' show start of simulations
  '    Tracker.initializeTask("Performing simulations...")
  '    Tracker.setTotal(numSims)
  '    ' loop through simulations
  '    For simNum = 0 To numSims - 1
  '      ' randomize categories
  '      Data.Sorting.shuffleInteger(catID)
  '      ' calculate CLQ of simulated pattern
  '      curSimCLQ = getCLQ(NN, catID, catList.Count)
  '      ' copy
  '      For cat1 = 0 To catList.Count - 1
  '        For cat2 = 0 To catList.Count - 1
  '          simCLQ(cat1, cat2, simNum) = curSimCLQ(cat1, cat2)
  '        Next cat2
  '      Next cat1
  '      ' report progress
  '      If simNum Mod 10 = 9 Then
  '        Tracker.setCompleted(simNum + 1)
  '      End If
  '    Next simNum
  '    ' report finish of simulations, start of calculations
  '    Tracker.finishTask("performing simulations...")
  '    Tracker.initializeTask("calculating significance...")
  '    ' calculate means, SDs, sigs
  '    meanCLQ = getMeanSimCLQ(simCLQ)
  '    sdCLQ = getSDSimCLQ(simCLQ, meanCLQ)
  '    sigCLQ = getSigCLQ(CLQ, simCLQ)
  '    ' report finish
  '    Tracker.finishTask("calculating significance...")
  '  End If
  'End Sub
  Private Function getPVal(ByVal numHigher(,) As Integer, _
                           ByVal numLower(,) As Integer, _
                           ByVal numSims As Integer) As Double(,)
    ' returns the p-value
    ' calculated as 2x the minimum of the number of simulations
    ' with higher or lower values than the actual values
    Dim R(,) As Double
    ReDim R(UBound(numHigher), UBound(numLower))
    Dim cat1, cat2 As Integer
    Dim min As Double
    For cat1 = 0 To UBound(numHigher, 1)
      For cat2 = 0 To UBound(numHigher, 2)
        If numHigher(cat1, cat2) < numLower(cat1, cat2) Then
          min = numHigher(cat1, cat2)
        Else
          min = numLower(cat1, cat2)
        End If
        R(cat1, cat2) = 2 * min / numSims
      Next cat2
    Next cat1
    Return R
  End Function
  Private Function getSigCLQ(ByVal actualCLQ(,) As Double, _
                             ByVal simCLQ(,,) As Double) As Double(,)
    ' calculates 2-tailed significance
    ' as 2x least number of more extreme simulated values (higher or lower)
    ' divided by number of simulations
    Dim R(,) As Double
    Dim numLower(,) As Integer
    Dim numHigher(,) As Integer
    Dim numCats As Integer = UBound(simCLQ, 1) + 1
    Dim numSims As Integer = UBound(simCLQ, 3) + 1
    Dim row, col As Integer
    ReDim R(numCats - 1, numCats - 1)
    ' get count of sims higher and lower
    numHigher = getNumMoreExtreme(actualCLQ, simCLQ, True)
    numLower = getNumMoreExtreme(actualCLQ, simCLQ, False)
    ' calculate significance
    For row = 0 To numCats - 1
      For col = 0 To numCats - 1
        R(row, col) = 2 * MinInt(numHigher(row, col), numLower(row, col)) / numSims
      Next
    Next
    ' return result
    Return R
  End Function
  Private Function MinInt(ByVal int1 As Integer, ByVal int2 As Integer) As Integer
    If int1 < int2 Then Return int1 Else Return int2
  End Function
  Private Function getNumMoreExtreme(ByVal actualCLQ(,) As Double, _
                                     ByVal simclq(,,) As Double, _
                                     ByVal getNumHigher As Boolean) As Integer(,)
    ' gets the number of simulated CLQ values
    ' higher (or lower) than the actual CLQ values
    Dim cat1, cat2, simNum As Integer
    Dim R(,) As Integer
    Dim numCats As Integer = UBound(simclq, 1) + 1
    Dim numSims As Integer = UBound(simclq, 3) + 1
    Dim curCount As Integer
    ' prepare results array
    ReDim R(numCats - 1, numCats - 1)
    ' loop through category pairs
    For cat1 = 0 To numCats - 1
      For cat2 = 0 To numCats - 1
        ' initialize total to zero
        curCount = 0
        ' loop through simulations
        For simNum = 0 To numSims - 1
          If getNumHigher Then
            If simclq(cat1, cat2, simNum) > actualCLQ(cat1, cat2) Then curCount += 1
          Else
            If simclq(cat1, cat2, simNum) < actualCLQ(cat1, cat2) Then curCount += 1
          End If
        Next
        ' get mean
        R(cat1, cat2) = curCount
      Next cat2
    Next cat1
    ' return result
    Return R
  End Function
  Private Function getMeanSimCLQ(ByVal simCLQ(,,) As Double) As Double(,)
    ' gets the mean of simulated CLQ values
    Dim cat1, cat2, simNum As Integer
    Dim R(,) As Double
    Dim numCats As Integer = UBound(simCLQ, 1) + 1
    Dim numSims As Integer = UBound(simCLQ, 3) + 1
    Dim curTotal As Double
    ' prepare results array
    ReDim R(numCats - 1, numCats - 1)
    ' loop through category pairs
    For cat1 = 0 To numCats - 1
      For cat2 = 0 To numCats - 1
        ' initialize total to zero
        curTotal = 0
        ' loop through simulations
        For simNum = 0 To numSims - 1
          curTotal += simCLQ(cat1, cat2, simNum)
        Next
        ' get mean
        R(cat1, cat2) = curTotal / numSims
      Next cat2
    Next cat1
    ' return result
    Return R
  End Function
  Private Function getSDSimCLQ(ByVal simCLQ(,,) As Double, _
                               ByVal meanCLQ(,) As Double) As Double(,)
    ' gets the standard deviation of simulated CLQ values
    Dim cat1, cat2, simNum As Integer
    Dim R(,) As Double
    Dim numCats As Integer = UBound(simCLQ, 1) + 1
    Dim numSims As Integer = UBound(simCLQ, 3) + 1
    Dim curTotal As Double
    ' prepare results array
    ReDim R(numCats - 1, numCats - 1)
    ' loop through category pairs
    For cat1 = 0 To numCats - 1
      For cat2 = 0 To numCats - 1
        ' initialize total to zero
        curTotal = 0
        ' loop through simulations
        For simNum = 0 To numSims - 1
          curTotal += (simCLQ(cat1, cat2, simNum) - meanCLQ(cat1, cat2)) ^ 2
        Next
        ' get mean
        R(cat1, cat2) = curTotal / numSims
      Next cat2
    Next cat1
    ' return result
    Return R
  End Function
  'Private Sub getCategories(ByRef catID() As Integer, _
  '                          ByRef catList As List(Of String), _
  '                          Optional ByVal T As Feedback.ProgressTracker = Nothing)
  '  ' creates:
  '  ' catID() : an array of category IDs for each point in the 
  '  '           shapefile (stored globally)
  '  ' catList : a lookup table giving the string value associated with 
  '  '           each category;
  '  '           these are the original values in the shapefile table, 
  '  '           converted to string format
  '  If HNDclqPoint = -1 Then Exit Sub
  '  Dim i As Integer
  '  Dim catField As Integer
  '  Dim curCat As String, curID As Integer
  '  ' get shapefile
  '  Dim SF As Shapefile
  '  SF = mapCLQ.get_GetObject(HNDclqPoint)
  '  ' initiate progress report
  '  If Not T Is Nothing Then
  '    T.initializeTask("Creating category list...")
  '    T.setTotal(SF.NumShapes)
  '  End If
  '  ' set up variables
  '  ReDim catID(SF.NumShapes - 1)
  '  catList = New List(Of String)
  '  ' get field
  '  catField = cmbField.SelectedIndex
  '  ' loop through points
  '  For i = 0 To SF.NumShapes - 1
  '    ' get name of current category
  '    curCat = SF.CellValue(catField, i).ToString
  '    ' see if it is already in the list; if not, add it
  '    If Not catList.Contains(curCat) Then
  '      catList.Add(curCat)
  '    End If
  '    ' report progress
  '    If (i + 1) Mod 1000 = 0 Then
  '      T.setCompleted(i + 1)
  '    End If
  '  Next
  '  ' sort list
  '  catList.Sort()
  '  ' loop again
  '  For i = 0 To SF.NumShapes - 1
  '    ' get name of current category
  '    curCat = SF.CellValue(catField, i).ToString
  '    ' get ID of current category
  '    curID = catList.IndexOf(curCat)
  '    catID(i) = curID
  '  Next
  '  ' report finish
  '  If Not T Is Nothing Then
  '    T.finishTask()
  '  End If
  'End Sub
#End Region
End Module
