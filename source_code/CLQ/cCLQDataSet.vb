
Public Class cCoLocationEngine
  ' Requires mod2DTree, modUtils
  ' Before starting, set up input as follows:
  ' 1. Point location input must consist of an array of points (structSimplePoint)
  ' 2. Categories must be an array of objects of same length as points
  '    (strings or integers should work fine; not sure of other object types)
  ' 3. Requires specification of iNeighborhoodCalculator to calculate neighbor relations
  ' 4. Requires specification of iSimulator to designate Monte Carlo reclassification method
#Region "Variables"
  ' defined one time, when object is initialized
  Private pNbCalculator As iNeighborhoodCalculator
  Private pSimulator As iSimulator
  ' calculated one time, when object is initialized
  Private pPt() As twoDTree.sPoint
  Private pClassNames() As String
  Private pObsCatID() As Integer ' the category ID of the input points
  Private pSimCatID() As Integer ' the category IDs of the last simulation
  Private pCatLookup As List(Of Object) ' the original string or other value associated with each category ID
  Private pNumCats As Integer ' the number of categories in pCatLookup, for convenience
  ' calculated one time, after object is initialized
  Private pNeighbors() As List(Of sNeighborRel) ' A list of neighbor relations for each point
  Private pObsNNCT As cContingencyTable ' The neighborhood contingency table of the input points
  ' new contingency table added for each simulation
  Private pSimCT As New List(Of cContingencyTable) ' Neighborhood contingency tables for each simulation
  Private pJPopMet As New List(Of Double) ' joint population pattern metric
  ' also keep track of simulated chi-squared statistics
  Private pChiSQ As New List(Of Double)
  ' keep track of outline
  Dim outlineFeat As DotSpatial.Data.Feature = Nothing
  Dim outlineExt As DotSpatial.Data.Extent = Nothing
  ' keep track of simulation time
  Dim simTime As TimeSpan = TimeSpan.Zero
  Dim keepSimsGoing As Boolean = True
  ' datagridview cell styles
  Public headerStyle As New System.Windows.Forms.DataGridViewCellStyle
  Public rowCatStyle As New System.Windows.Forms.DataGridViewCellStyle
  Public colCatStyle As New System.Windows.Forms.DataGridViewCellStyle
  Public valStyle As New System.Windows.Forms.DataGridViewCellStyle

#End Region
#Region "Structures and Interfaces"
  Public Structure sNeighborRel
    Dim nbID As Integer
    Dim nbWeight As Double
  End Structure
  ' interface to calculate distance
  Public Interface iNeighborhoodCalculator
    Sub createIndex(ByVal ptSet() As twoDTree.sPoint)
    Sub createIndex(ByVal newIndex As twoDTree)
    Function getNeighborRelations(ByVal ptID As Integer) As List(Of sNeighborRel)
    Function getJointPopMetric() As Double
  End Interface
  ' interface to reclassify points
  Public Interface iSimulator
    '    Function Reclassify(ByVal inCatID() As Integer) As Integer()
    ReadOnly Property NullModelName As String ' for checking if a new null model has been defined
    ReadOnly Property StateString As String ' for checking if a new null model state has been defined (esp. CRL)
    Function expectedNNCT(ByVal obsNNCT As cContingencyTable) As cContingencyTable
    Sub runSim()
    ReadOnly Property simNNCT As cContingencyTable
    ReadOnly Property simJointPopMetric As Double
    ReadOnly Property lastSimClasses As Integer()
    ReadOnly Property lastSimPts As twoDTree.sPoint()
    'ReadOnly Property simJointPopMetric As Double
  End Interface
  Public Enum eNullModel
    CSR = 0
    ToroidalShift = 1
    RL = 2
    CRL_FixedCat = 3
    CRL_FixedBase = 4
    CRL_FixedNeighbor = 5
    CRL_FixedOther = 6
  End Enum
  Public Enum eStatType
    overall = 0
    obsCount = 1
    expCount = 2
    avgSimCount = 3
    CLQ = 4
    pValue = 5
  End Enum
#End Region
#Region "Public Methods"
  ' start here
  Public Sub New(ByVal pt() As twoDTree.sPoint,
                 ByVal catID() As Integer,
                 ByVal catName() As String,
                 ByVal numCats As Integer,
                 ByVal nullModel As eNullModel,
                 ByVal neighborhoodDef As iNeighborhoodCalculator,
                 tracker As Feedback.ProgressTracker,
                 Optional ByVal fixedCat() As Integer = Nothing,
                 Optional ByVal outlineFeature As DotSpatial.Data.Feature = Nothing,
                 Optional ByVal outlineExtent As DotSpatial.Data.Extent = Nothing)
    ' Creates index of points
    ' calculates co-location quotients
    ' assumes input catID() values range from 0 to numCats-1
    tracker.initializeTask("Initializing null model...")
    ' assign defined variables
    pNbCalculator = neighborhoodDef
    pPt = pt
    pObsCatID = catID
    outlineFeat = outlineFeature
    outlineExt = outlineExtent
    ' get category index
    'Data.Lookup.createIDLookup(cat, pObsCatID, pCatLookup)

    pNumCats = numCats
    pClassNames = catName
    ' calculate neighbor relations
    pNbCalculator.createIndex(pt) ' create index for fast calculation
    ReDim pNeighbors(UBound(pt)) ' set up array for neighbor relations of each point
    For i = 0 To pObsCatID.Length - 1
      pNeighbors(i) = pNbCalculator.getNeighborRelations(i) ' get neighbor relations for point i
    Next
    ' calculate contingency table from observed data
    pObsNNCT = clqUtils.calcNCT(pObsCatID, pClassNames, pNeighbors)

    ' create simulator
    Select Case nullModel
      Case Is = eNullModel.CRL_FixedBase
        pSimulator = New CRL_FixedBase_Simulator(pPt, pObsCatID, pClassNames, pNbCalculator, pNeighbors)
      Case Is = eNullModel.CRL_FixedCat
        pSimulator = New CRL_FixedCats_Simulator(fixedCat, pObsCatID, pClassNames, pNbCalculator, pNeighbors)
      Case Is = eNullModel.CRL_FixedNeighbor
        pSimulator = New CRL_FixedNeighbor_Simulator(pObsCatID, pClassNames, pNbCalculator, pNeighbors)
      Case Is = eNullModel.CRL_FixedOther
        pSimulator = New CRL_FixedOther_Simulator(pObsCatID, pClassNames, pNbCalculator, pNeighbors)
      Case Is = eNullModel.CSR
        ' get outline
        If outlineFeature Is Nothing Then
          Dim minX, maxX, minY, maxY As Double
          If outlineExtent Is Nothing Then
            minX = pt(0).x : minY = pt(0).y : maxX = minX : maxY = minY
            For Each p In pt
              If p.x < minX Then minX = p.x
              If p.y < minY Then minY = p.y
              If p.x > maxX Then maxX = p.x
              If p.y > maxY Then maxY = p.y
            Next
          Else
            minX = outlineExt.MinX
            minY = outlineExt.MinY
            maxX = outlineExt.MaxX
            maxY = outlineExt.MaxY
          End If
          Dim LL As New DotSpatial.Topology.Coordinate(minX, minY)
          Dim UL As New DotSpatial.Topology.Coordinate(minX, maxY)
          Dim UR As New DotSpatial.Topology.Coordinate(maxX, maxY)
          Dim LR As New DotSpatial.Topology.Coordinate(maxX, minY)
          outlineFeature = New DotSpatial.Data.Feature(DotSpatial.Topology.FeatureType.Polygon, {LL, UL, UR, LR, LL})
          outlineFeature.UpdateEnvelope()
        End If
        pSimulator = New CSR_Simulator(pObsCatID, pClassNames, pNbCalculator, pNeighbors, outlineFeature)
      Case Is = eNullModel.RL
        pSimulator = New RL_Simulator(pObsCatID, pClassNames, pNbCalculator, pNeighbors)
      Case Is = eNullModel.ToroidalShift
        If outlineExtent Is Nothing Then

        Else
          pSimulator = New ToroidalShift_Simulator(pObsCatID, pClassNames, pNbCalculator, pPt, outlineExtent)
        End If
    End Select
    tracker.finishTask("Initializing null model...")

  End Sub
  ' run simulations as necessary
  Public Sub runSims(ByVal numSims As Integer, Optional ByVal PT As Feedback.ProgressTracker = Nothing)
    ' run simulations, add to simulated contingency tables
    ' requires that neighborhoods (pNeighbors) already be defined
    If pNeighbors Is Nothing Then Exit Sub
    If pSimulator Is Nothing Then Exit Sub
    ' reset previous interrupt
    keepSimsGoing = True
    ' report start
    If Not PT Is Nothing Then
      PT.initializeTask("Running simulations...")
      PT.setTotal(numSims)
    End If
    Dim SW As Stopwatch = Stopwatch.StartNew
    ' initialize variables
    Dim xpNNCT As cContingencyTable = expectedNNCT()
    ' run simulations
    For i = 1 To numSims
      ' report progress
      If Not PT Is Nothing Then
        PT.setCompleted(i)
        Application.DoEvents()
      End If
      ' run simulation

      pSimulator.runSim()
      System.Threading.Thread.Sleep(0)
      Dim newCC As cContingencyTable = pSimulator.simNNCT
      Dim newChiSQ As Double = clqUtils.ChiSqStat(newCC, xpNNCT)
      Dim JPMet As Double = pSimulator.simJointPopMetric
      ' add to list of contingency tables, chi-squared statisitcs, 
      ' joint population pattern metrics
      pSimCT.Add(newCC)
      pChiSQ.Add(newChiSQ)
      pJPopMet.Add(JPMet)
      ' make sure user hasn't paused
      If Not keepSimsGoing Then Exit For
    Next
    ' report finish
    If Not PT Is Nothing Then
      PT.finishTask("Running simulations...")
    End If
    SW.Stop()
    simTime = simTime.Add(SW.Elapsed)
  End Sub
  Public Sub resetSims()
    pSimCT.Clear()
    pChiSQ.Clear()
    pJPopMet.Clear()
  End Sub
  Public Sub interruptSims()
    ' interrupts current simulation without breaking anything
    keepSimsGoing = False
  End Sub
  ' get results
  Public ReadOnly Property NullModel As String
    Get
      Return pSimulator.NullModelName
    End Get
  End Property
  Public ReadOnly Property simulationTime As TimeSpan
    Get
      Return simTime
    End Get
  End Property
  Public Function obsNNCT() As cContingencyTable
    Return pObsNNCT
  End Function
  Public Function SimulatedNNCT(ByVal simID As Integer) As cContingencyTable
    ' error checking
    If simID >= pSimCT.Count Then Return Nothing
    If simID < 0 Then Return Nothing
    ' return value
    Return pSimCT.Item(simID)
  End Function
  Public Function avgSimNNCT() As cContingencyTable
    ' returns the average value for each cell in the nearest neighbor contingency table

    ' set up variable for results
    Dim R As New cContingencyTable(pObsNNCT.classNames)
    ' set to zero, just in case
    For A = 0 To R.numClasses - 1
      For B = 0 To R.numClasses - 1
        R.Value(A, B) = 0
      Next
    Next
    ' loop through simulations
    For Each simNNCT In pSimCT
      ' add values
      For A = 0 To R.numClasses - 1
        For B = 0 To R.numClasses - 1
          R.Value(A, B) += simNNCT.Value(A, B)
        Next B
      Next A
    Next simNNCT
    ' get averages
    Dim numSim As Integer = pSimCT.Count
    For A = 0 To R.numClasses - 1
      For B = 0 To R.numClasses - 1
        R.Value(A, B) /= numSim
      Next
    Next
    ' return result
    Return R
  End Function
  Public Function expectedNNCT() As cContingencyTable
    Return pSimulator.expectedNNCT(pObsNNCT)
  End Function
  Public Function simChiSQ(ByVal simID As Integer) As Double
    Return pChiSQ.Item(simID)
  End Function
  Public Function simJointPopPatternMetric(ByVal simID As Integer) As Double
    Return pJPopMet.Item(simID)
  End Function
  Public Function simJPopMetArray() As Double()
    Dim R() As Double
    ReDim R(pJPopMet.Count - 1)
    pJPopMet.CopyTo(R)
    Return R
  End Function
  Public Function CLQ() As cContingencyTable
    ' calculates the CLQ as Observed/Expected

    ' error checking
    If pObsNNCT Is Nothing Then Return Nothing
    If pSimulator Is Nothing Then Return Nothing

    ' set up result
    Dim R As New cContingencyTable(pClassNames)

    ' get expected
    Dim expNNCT As cContingencyTable
    If numSimsCompleted > 0 Then expNNCT = avgSimNNCT() Else expNNCT = expectedNNCT()
    ' divide
    For A = 0 To pObsNNCT.numClasses - 1
      For B = 0 To pObsNNCT.numClasses - 1
        ' check for zero in denominator
        If expNNCT.Value(A, B) <= 0 Then
          R.Value(A, B) = -1
        Else ' divide observed by expected
          R.Value(A, B) = pObsNNCT.Value(A, B) / expNNCT.Value(A, B)
        End If
      Next
    Next
    ' return result
    Return R

  End Function
  Public Sub compareNNCTValuesToSimArrays(ByVal ofNNCT As cContingencyTable, _
                                 ByRef numLower As cContingencyTable, _
                                 ByRef numSame As cContingencyTable, _
                                 ByRef numHigher As cContingencyTable)
    ' calculates the number of simulations with lower, same and higher values than the observed value
    ' in each cell of the NNCT
    ' set up results
    numLower = New cContingencyTable(ofNNCT.classNames)
    numSame = New cContingencyTable(ofNNCT.classNames)
    numHigher = New cContingencyTable(ofNNCT.classNames)
    ' other variables
    Dim curSimArray() As Double
    Dim curLow, curSame, curHigh As Double
    ' loop through class pairs
    For C1 = 0 To ofNNCT.numClasses - 1
      For C2 = 0 To ofNNCT.numClasses - 1
        ' get array
        curSimArray = nnctArray(C1, C2)
        System.Array.Sort(curSimArray)
        ' get lower/upper bounds
        Data.Sorting.compareValueToArray(ofNNCT.Value(C1, C2), curSimArray, curLow, curSame, curHigh)
        ' place them in results
        numLower.Value(C1, C2) = curLow
        numSame.Value(C1, C2) = curSame
        numHigher.Value(C1, C2) = curHigh
      Next
    Next
  End Sub
  Public Overloads Function pValues() As cContingencyTable
    Return pValues(pObsNNCT)
  End Function
  Public Overloads Function pvalues(ByVal simID As Integer) As cContingencyTable
    Return pvalues(pSimCT.Item(simID))
  End Function
  Public Overloads Function pValues(ByVal ofNNCT As cContingencyTable) As cContingencyTable
    ' returns p-values based on simulated NNCTs
    Dim R As New cContingencyTable(ofNNCT.classNames)
    ' get bounding simulation ranks
    Dim numLower, numSame, numHigher As cContingencyTable
    compareNNCTValuesToSimArrays(ofNNCT, numLower, numSame, numHigher)
    ' get p-values
    For c1 = 0 To ofNNCT.numClasses - 1
      For c2 = 0 To ofNNCT.numClasses - 1
        R.Value(c1, c2) = pValue(numLower.Value(c1, c2), numSame.Value(c1, c2), numHigher.Value(c1, c2))
      Next
    Next
    Return R
  End Function
  Shared Function pValue(ByVal numLower As Integer, ByVal numSame As Integer, ByVal numHigher As Integer) As Double
    ' returns the two-tailed probability that an observed value
    ' comes from the simulated distribution
    Dim p As Double
    p = numSame + Math.Min(numLower, numHigher)
    p = p / (numLower + numSame + numHigher)
    p = p * 2
    If p > 1 Then p = 1
    Return p
  End Function
  Public ReadOnly Property lastSimCats() As Integer()
    Get
      If numSimsCompleted > 0 Then Return pSimCatID Else Return pObsCatID
    End Get
  End Property
  Public ReadOnly Property obsCats() As Integer()
    Get
      Return pObsCatID
    End Get
  End Property
  Public Function ChiSqStat(Optional ByVal includeDiagonal As Boolean = True) As Double
    ' returns the chi-squared statistic for the observed NNCT
    Return clqUtils.ChiSqStat(pObsNNCT, expectedNNCT, includeDiagonal)
  End Function
  Public Function ChiSqPValue() As Double
    ' returns the one-tailed p-value associated with the chi-squared statistic
    If pChiSQ.Count < 1 Then Return -1
    Dim nL, nS, nH
    Data.Numbers.compareValueToArray(ChiSqStat, pChiSQ.ToArray, nL, nS, nH)
    Return (nS + nH) / pChiSQ.Count
  End Function
  Public Function jointPopulationPValue() As Double
    ' returns the two-tailed p-value associated with the joint population pattern metric
    Dim simJPM() As Double = simJPopMetArray()
    If simJPM.Length = 0 Then Return -1
    Dim JPM As Double = jointPopulationPatternMetric()
    Dim nL, nS, nH
    Data.Numbers.compareValueToArray(JPM, simJPM, nL, nS, nH)
    Return (Math.Min(nL, nH) + nS) * 2 / pChiSQ.Count
  End Function
  Public Function jointPopulationPatternMetric() As Double
    pNbCalculator.createIndex(pPt)
    Return pNbCalculator.getJointPopMetric
  End Function
  Public Function overviewStatsAsText(numDecimalPlaces As Integer) As String
    Dim R As String
    R = "Chi-square" & vbTab & misc.formatting.numToText(ChiSqStat(True), numDecimalPlaces) & vbCrLf
    R &= "p-value" & vbTab & misc.formatting.numToText(ChiSqPValue, numDecimalPlaces) & vbCrLf
    '    R &= "Avg Nbr Distance" & misc.formatting.numToText(jointPopulationPatternMetric, numDecimalPlaces) & vbCrLf
    'Dim avgJPPM As Double = -1
    'If simJPopMetArray.Length > 0 Then avgJPPM = simJPopMetArray.Average
    'R &= "Simulated Average Neighbor Distance" & vbTab & misc.formatting.numToText(avgJPPM, numDecimalPlaces) & vbCrLf
    'R &= "Avg Nbr Dist p-value" & vbTab & misc.formatting.numToText(jointPopulationPValue(), numDecimalPlaces) & vbCrLf
    Return R
  End Function
  Public Sub showInRTB(RTB As RichTextBox, statType As eStatType, Optional colWid As Integer = 12, Optional numDecimals As Integer = 3, Optional sepChr As String = " ")
    ' show particular statistics in rich text box
    Dim CT As cContingencyTable
    ' get table values
    Select Case statType
      Case Is = eStatType.avgSimCount
        If numSimsCompleted > 0 Then CT = avgSimNNCT()
      Case Is = eStatType.CLQ
        CT = CLQ()
      Case Is = eStatType.obsCount
        If numSimsCompleted > 0 Then CT = obsNNCT()
      Case Is = eStatType.pValue
        If numSimsCompleted > 0 Then CT = pValues()
      Case Is = eStatType.expCount
        CT = expectedNNCT()
      Case Is = eStatType.overall
        RTB.Text = overviewStatsAsText(5)
    End Select
    ' show table values
    If Not CT Is Nothing Then
      RTB.Text = CT.valuesAsText(numDecimals,, sepChr)
    End If
  End Sub

  Public Sub showInDGV(ByVal DGV As DataGridView, _
                       Optional ByVal numDecimalPlaces As Integer = -1)
    ' shows ALL statistics in the DGV
    ' (1) summary stats
    ' (2) nearest neighbor counts
    ' (3) expected or sim avg nearest neighbor counts
    ' (4) CLQ
    ' (5) p-values
    DGV.SuspendLayout()
    ' clear dgv
    DGV.SelectAll()
    DGV.ClearSelection()

    ' size dgv
    ' columns
    Dim numCols As Integer = pNumCats + 1
    If numCols < 3 Then numCols = 3
    ' rows
    Dim numRows As Integer
    Dim summaryRows As Integer = 5
    Dim numContingencyTables As Integer
    numContingencyTables = 4
    ' dgv

    '    headerStyle.Font = New Font("Times New Roman", 9, System.Drawing.FontStyle.Bold)

    '    rowCatStyle.Font = New Font("Times New Roman", 9, System.Drawing.FontStyle.Italic)
    '   rowCatStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight

    ' colCatStyle.Font = New Font("Times New Roman", 9, System.Drawing.FontStyle.Italic)
    'colCatStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter

    'valStyle.Font = New Font("Times New Roman", 9, System.Drawing.FontStyle.Regular)
    'valStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter


    DGV.RowCount = numContingencyTables * (pNumCats + 1) + summaryRows
    DGV.ColumnCount = numCols
    DGV.ColumnHeadersVisible = False
    DGV.RowHeadersVisible = False
    DGV.AutoResizeColumns()
    'DGV.Columns(0).DefaultCellStyle = headerStyle
    ' show summary statistics
    DGV.Rows(0).Cells(0).Value = "Chi-square:"
    DGV.Rows(0).Cells(1).Value = misc.formatting.numToText(ChiSqStat(True), numDecimalPlaces)
    DGV.Rows(1).Cells(0).Value = "p-value"
    DGV.Rows(1).Cells(1).Value = misc.formatting.numToText(ChiSqPValue, numDecimalPlaces)
    DGV.Rows(2).Cells(0).Value = "Average Neighbor Distance:"
    DGV.Rows(2).Cells(1).Value = misc.formatting.numToText(jointPopulationPatternMetric, numDecimalPlaces)
    DGV.Rows(3).Cells(0).Value = "Simulated Average Neighbor Distance:"
    Dim avgJPPM As Double = -1
    If simJPopMetArray.Length > 0 Then avgJPPM = simJPopMetArray.Average
    DGV.Rows(3).Cells(1).Value = misc.formatting.numToText(avgJPPM, numDecimalPlaces)
    DGV.Rows(4).Cells(0).Value = "Avg Nbr Dist p-value:"
    DGV.Rows(4).Cells(1).Value = misc.formatting.numToText(jointPopulationPValue(), numDecimalPlaces)
    'For thisRow = 0 To 4
    '  DGV.Rows(thisRow).Cells(1).Style = valStyle
    'Next
    ' (2) nearest neighbor counts
    Dim R As Integer = 5
    'DGV.Rows(R).DefaultCellStyle = headerStyle
    'For i = 0 To pNumCats - 1
    '  DGV.Rows(R).Cells(i + 1).Style = colCatStyle
    '  DGV.Rows(R + i + 1).Cells(0).Style = rowCatStyle
    'Next
    'DGV.Rows(R).Cells(0).Style = headerStyle
    DGV.Rows(R).Cells(0).Value = "NEIGHBOR COUNTS"
    obsNNCT.showInDataGrid(DGV, R, pClassNames, numDecimalPlaces, rowCatStyle, valStyle)
    ' (3) expected or sim avg nearest neighbor counts
    R = 5 + pNumCats + 1
    'DGV.Rows(R).DefaultCellStyle = headerStyle
    'For i = 0 To pNumCats - 1
    '  DGV.Rows(R).Cells(i + 1).Style = colCatStyle
    '  DGV.Rows(R + i + 1).Cells(0).Style = rowCatStyle
    'Next
    'DGV.Rows(R).Cells(0).Style = headerStyle
    DGV.Rows(R).Cells(0).Value = "EXPECTED NEIGHBOR COUNTS"
    Dim expNNCT As cContingencyTable
    If numSimsCompleted = 0 Then expNNCT = expectedNNCT() Else expNNCT = avgSimNNCT()
    expNNCT.showInDataGrid(DGV, R, pClassNames, numDecimalPlaces, rowCatStyle, valStyle)
    ' (4) CLQ
    R = 5 + 2 * (pNumCats + 1)
    'DGV.Rows(R).DefaultCellStyle = headerStyle
    'For i = 0 To pNumCats - 1
    '  DGV.Rows(R).Cells(i + 1).Style = colCatStyle
    '  DGV.Rows(R + i + 1).Cells(0).Style = rowCatStyle
    'Next
    'DGV.Rows(R).Cells(0).Style = headerStyle
    DGV.Rows(R).Cells(0).Value = "CO-LOCATION QUOTIENT"
    CLQ.showInDataGrid(DGV, R, pClassNames, numDecimalPlaces, rowCatStyle, valStyle)
    ' (5) p-values
    R = 5 + 3 * (pNumCats + 1)
    'DGV.Rows(R).DefaultCellStyle = headerStyle
    'For i = 0 To pNumCats - 1
    '  DGV.Rows(R).Cells(i + 1).Style = colCatStyle
    '  DGV.Rows(R + i + 1).Cells(0).Style = rowCatStyle
    'Next
    'DGV.Rows(R).Cells(0).Style = headerStyle
    DGV.Rows(R).Cells(0).Value = "P-VALUES"
    pValues().showInDataGrid(DGV, R, pClassNames, numDecimalPlaces, rowCatStyle, valStyle)
    DGV.ResumeLayout()
  End Sub
  Public Function lastSim_asFeatureSet() As DotSpatial.Data.FeatureSet
    ' returns a featureset of the last simulated points, 
    ' that can be saved as a shapefile

    ' error checking
    If numSimsCompleted = 0 Then Return Nothing
    ' get from simulator
    Return clqUtils.SimToFeatureSet(pSimulator)
  End Function
  ' retrieve original points
  Public Function originalPoints() As twoDTree.sPoint()
    Return pPt
  End Function
#End Region
#Region "Sim Tabulation"
  Public Function nnctArray(ByVal C1 As Integer, ByVal C2 As Integer) As Double()
    ' fetches array of nearest neighbor counts for two classes
    Dim R() As Double
    ReDim R(pSimCT.Count - 1)
    For i = 0 To R.Length - 1
      R(i) = pSimCT(i).Value(C1, C2)
    Next
    Return R
  End Function
#End Region
  Private Sub calcNearestNeighbors(ByVal ptCatID() As Integer, _
                                      ByVal numCats As Integer, _
                                      ByVal Neighbors() As List(Of sNeighborRel), _
                                      ByVal DGV As DataGridView, _
                                      ByVal classNames() As Object)
    ' code is as close as possible exactly the same as calcContingencyTable, 
    ' but produces a list of the nearest neighbors of each point
    ' new variables:
    Dim cat() As String
    ReDim cat(ptCatID.Length - 1)
    Dim nnStr() As String
    ReDim nnStr(ptCatID.Length - 1)


    Dim R As New cContingencyTable(pClassNames)
    ' loop through points
    For ptID = 0 To ptCatID.Length - 1
      ' note category of current point
      Dim fromCat As Integer = ptCatID(ptID)

      ' for debugging:
      cat(ptID) = classNames(fromCat).ToString
      nnStr(ptID) = ""

      ' get list of neighbor relations
      Dim nbRelList As List(Of sNeighborRel) = pNeighbors(ptID)
      ' loop through neighbor relations
      For Each curNbRel In nbRelList
        ' get category of neighbor
        Dim toCat As Integer = ptCatID(curNbRel.nbID)
        ' add neighbor count or weight to contingency table
        R.Value(fromCat, toCat) = R.Value(fromCat, toCat) + curNbRel.nbWeight

        ' for debugging
        nnStr(ptID) = nnStr(ptID) & classNames(toCat).ToString

      Next curNbRel
    Next ptID

    ' put in DGV
    DGV.RowCount = cat.Length
    DGV.ColumnCount = 2
    DGV.RowHeadersVisible = True
    DGV.ColumnHeadersVisible = True
    DGV.RowHeadersWidth = 100
    DGV.Columns(0).HeaderText = "Category"
    DGV.Columns(1).HeaderText = "Neighbor(s)"
    For i = 0 To cat.Length - 1
      DGV.Rows(i).HeaderCell.Value = i
      DGV.Rows(i).Cells(0).Value = cat(i)
      DGV.Rows(i).Cells(1).Value = nnStr(i)
    Next
  End Sub
  Public ReadOnly Property numSimsCompleted As Integer
    Get
      Return pSimCT.Count
    End Get
  End Property
End Class
Public Class cContingencyTable
  ' stores and manages contingency table
  ' consisting of counts or weighted proportions of neighbors 
  ' of each class to each other class
  Dim pValue(,) As Double
  Dim pNumClasses As Integer
  Dim pClassNames() As String
  Public Sub New(ByVal classNames() As String)
    ' designate as square matrix based on number of classes
    pClassNames = classNames
    pNumClasses = classNames.Count
    ReDim pValue(numClasses - 1, numClasses - 1)
    ' set to zeroes, just in case!!!
    For i = 0 To numClasses - 1
      For j = 0 To numClasses - 1
        pValue(i, j) = 0
      Next
    Next
  End Sub
  Public Property Value(ByVal fromClass As Integer, ByVal toClass As Integer) As Double
    Get
      Return pValue(fromClass, toClass)
    End Get
    Set(ByVal value As Double)
      pValue(fromClass, toClass) = value
    End Set
  End Property
  Public ReadOnly Property numClasses
    Get
      Return pNumClasses
    End Get
  End Property
  Public ReadOnly Property classNames() As String()
    Get
      Return pClassNames
    End Get
  End Property
  Public Overloads Sub showInDataGrid(ByVal dgv As DataGridView, _
                                      ByVal startRow As Integer, _
                                      ByVal classNames() As String, _
                                      Optional ByVal numDecimalPlaces As Integer = -1, _
                                      Optional ByVal hdrStyle As System.Windows.Forms.DataGridViewCellStyle = Nothing, _
                                      Optional ByVal valStyle As System.Windows.Forms.DataGridViewCellStyle = Nothing)
    ' shows values beginning at the start row of the input grid
    ' headers
    dgv.SuspendLayout()
    For rowCol = 0 To pNumClasses - 1
      ' show row headers
      '      dgv.Rows(startRow + rowCol + 1).Cells(0).Style = hdrStyle
      dgv.Rows(startRow + rowCol + 1).Cells(0).Value = classNames(rowCol)
      ' show column headers
      '     dgv.Rows(startRow).Cells(rowCol + 1).Style = hdrStyle
      dgv.Rows(startRow).Cells(rowCol + 1).Value = classNames(rowCol)
    Next
    ' values
    For row = 0 To pNumClasses - 1
      For col = 0 To pNumClasses - 1
        '  dgv.Rows(startRow + row + 1).Cells(col + 1).Style = valStyle
        dgv.Rows(startRow + row + 1).Cells(col + 1).Value = misc.formatting.numToText(pValue(row, col), numDecimalPlaces)
      Next col
    Next row
    dgv.ResumeLayout()
  End Sub

  Public Overloads Sub showInDataGrid(ByVal DGV As DataGridView, _
                            Optional ByVal classNames() As Object = Nothing, _
                            Optional ByVal numDecimalPlaces As Integer = -1, _
                            Optional ByVal showVertically As Boolean = False, _
                            Optional ByVal appendColumn As Boolean = False, _
                            Optional ByVal valueLabel As String = "value")
    ' first resizes grid 
    ' if appendColumn is true, only adds a column (does not reset number of rows)

    ' error checking
    If DGV Is Nothing Then Exit Sub
    ' variable declaration
    Dim cN() As Object
    ' resize grid 
    If appendColumn Then ' just add a column
      DGV.ColumnCount = DGV.ColumnCount + 1
    Else ' resize entire grid
      ' create class names if not input
      If classNames Is Nothing Then
        ReDim cN(pNumClasses - 1)
        For i = 0 To pNumClasses - 1
          cN(i) = "Class " & i.ToString
        Next
      Else : cN = classNames
      End If
      ' size grid
      If showVertically Then ' one row for each pair of classes
        DGV.RowCount = pNumClasses ^ 2
        DGV.ColumnCount = 3
        DGV.ColumnHeadersVisible = True
        DGV.RowHeadersVisible = False
      Else ' one row for each class
        DGV.RowCount = pNumClasses
        DGV.ColumnCount = pNumClasses
        DGV.ColumnHeadersVisible = True
        DGV.RowHeadersVisible = True
      End If
    End If
    ' get ID of last column
    Dim lastColID As Integer = DGV.ColumnCount - 1
    ' populate row and column headings
    DGV.RowHeadersWidth = 100
    If showVertically Then ' class names in columns 0 & 1
      If Not appendColumn Then
        DGV.Columns(0).HeaderText = "From Class"
        DGV.Columns(1).HeaderText = "To Class"
        Dim rowNum As Integer = 0
        For A = 0 To pNumClasses - 1
          For B = 0 To pNumClasses - 1
            DGV.Rows(rowNum).Cells(0).Value = cN(A)
            DGV.Rows(rowNum).Cells(1).Value = cN(B)
            rowNum += 1
          Next
        Next
      End If

      DGV.Columns(lastColID).HeaderText = valueLabel

    Else ' class names in row and column headers
      For i = 0 To pNumClasses - 1
        DGV.Columns(i).HeaderText = cN(i).ToString
        DGV.Rows.Item(i).HeaderCell.Value = cN(i).ToString
      Next
    End If
    ' populate cells
    If showVertically Then ' by row
      Dim rowNum As Integer = 0
      For A = 0 To pNumClasses - 1
        For B = 0 To pNumClasses - 1
          If numDecimalPlaces = 0 Then
            DGV.Rows(rowNum).Cells(lastColID).Value = pValue(A, B)
          Else
            DGV.Rows(rowNum).Cells(lastColID).Value = pValue(A, B).ToString("F" & numDecimalPlaces.ToString)
          End If
          rowNum += 1
        Next
      Next
    Else ' by row and column
      For i = 0 To pNumClasses - 1
        For j = 0 To pNumClasses - 1
          If numDecimalPlaces = 0 Then
            DGV.Rows(i).Cells(j).Value = pValue(i, j)
          Else
            DGV.Rows(i).Cells(j).Value = pValue(i, j).ToString("F" & numDecimalPlaces.ToString)
          End If
        Next
      Next
    End If

  End Sub
  Public Function valueArray() As Double()
    ' returns an array of values by row
    ' sequence is 11 12 13 ... 1k 21 22 23 ... 2k ... kk
    Dim R() As Double
    ReDim R(pNumClasses ^ 2 - 1)
    Dim ID As Integer = 0
    For fromClass = 0 To pNumClasses - 1
      For toClass = 0 To pNumClasses - 1
        R(ID) = pValue(fromClass, toClass)
        ID += 1
      Next toClass
    Next fromClass
    Return R
  End Function
  Public Function Clone() As cContingencyTable
    ' creates a new object with the same values
    Dim myClone As New cContingencyTable(pClassNames)
    For i = 0 To pNumClasses - 1
      For j = 0 To pNumClasses - 1
        myClone.Value(i, j) = Me.Value(i, j)
      Next
    Next
    Return myClone
  End Function

  Public Function valuesAsText(Optional numDecimals As Integer = 3, Optional colWid As Integer = 12, Optional sepChr As String = " ") As String
    ' returns multiline text containing value table
    Dim R As String = misc.formatting.toFixedWidthString(" ", colWid, sepChr)
    ' get column headers
    For i = 0 To pNumClasses - 1
      If sepChr = vbTab Then
        R &= pClassNames(i) & vbTab
      Else
        R &= misc.formatting.toFixedWidthString(pClassNames(i), colWid)
      End If

    Next
    ' go through rows
    For i = 0 To pNumClasses - 1
      ' get row header
      Dim rowHead As String
      If sepChr = vbTab Then
        rowHead = pClassNames(i) & vbTab
      Else
        rowHead = misc.formatting.toFixedWidthString(pClassNames(i), colWid, sepChr)
      End If
      Dim oneLine As String = rowHead
      ' get row values
      For j = 0 To pNumClasses - 1
        Dim val As String
        If numDecimals >= 0 Then
          val = misc.formatting.numToText(Value(i, j), numDecimals)
        Else
          val = CStr(Value(i, j))
        End If
        oneLine = oneLine & misc.formatting.toFixedWidthString(val, colWid, sepChr)
      Next j
      R = R & vbCrLf & oneLine
    Next i
    Return R
  End Function
End Class
Public Class EuclideanNeighborCalculator
  ' defines a neighborhood as a weighted set of neighbor ranks
  ' i.e. neighbor weights of {0.7, 0.3} would indicate: 
  '  70% weight to nearest neighbor
  '  30% weight to 2nd nearest neighbor
  Implements cCoLocationEngine.iNeighborhoodCalculator
  Dim pIndex As twoDTree ' 2d tree index
  Dim kdTree As DotSpatial.Topology.KDTree.KdTree
  Dim pNbWt() As Double ' must add up to one
  Public Property numNeighbors As Integer
    Get
      Return pNbWt.Count
    End Get
    Set(ByVal neighborCount As Integer)
      ' default is equal weighting of each neighbor
      If neighborCount > 0 Then
        ReDim pNbWt(neighborCount - 1)
        For i = 0 To neighborCount - 1
          pNbWt(i) = 1 / neighborCount
        Next
      End If
    End Set
  End Property
  Public Property neighborWeightsTemplate() As Double()
    Get
      Return pNbWt
    End Get
    Set(ByVal newWts As Double())
      ' these should normally be positive and sum to one
      ' but I'm not gonna force the issue
      ' in case somebody wants to play around
      pNbWt = newWts
    End Set
  End Property
  Public Overloads Sub createIndex(ByVal ptSet() As twoDTree.sPoint) Implements cCoLocationEngine.iNeighborhoodCalculator.createIndex
    ' creates a 2-d tree index by adding points one at a time
    pIndex = New twoDTree
    For i = 0 To ptSet.Length - 1
      pIndex.addPoint(ptSet(i).x, ptSet(i).y, i, i, 0)
    Next
  End Sub
  Public Overloads Sub createIndex(ByVal newIndex As twoDTree) Implements cCoLocationEngine.iNeighborhoodCalculator.createIndex
    pIndex = newIndex
  End Sub
  Public Function getNeighborRelations(ByVal ptID As Integer) _
    As System.Collections.Generic.List(Of cCoLocationEngine.sNeighborRel) _
    Implements cCoLocationEngine.iNeighborhoodCalculator.getNeighborRelations
    ' returns a list of neighbors with weights
    ' using the current point index
    Dim R As New List(Of cCoLocationEngine.sNeighborRel)
    Dim nbList As List(Of Neighbor) = pIndex.nearestNeighborList(ptID, pNbWt.Count)
    ' nbList should always exist, even if it has no items
    If nbList.Count = 0 Then Return R ' get out now while you still can!!
    ' actual weight calculation is delegated to allow other objects to use
    R = neighborWeights(nbList, pNbWt)
    Return R
  End Function
  Public Function neighborWeights(ByVal allNbrs As List(Of Neighbor), _
                             ByVal nbrWts() As Double) _
                           As List(Of cCoLocationEngine.sNeighborRel)
    ' determines weights for a set of input neighbors
    ' input list should have more neighbors than required
    ' returns a list of neighbors with weights, corresponding to the
    ' nbrWts input which for example would say to weight the 1st neighbor 0.5, 2nd 0.3, etc.

    ' *** really should make explicit the requirement to 
    ' designate a neighbor weight list in the Interface specification
    ' or NEW sub

    Dim R As New List(Of cCoLocationEngine.sNeighborRel)
    ' sort list
    Dim sortedNeighborQuery = From nbr In allNbrs
                              Order By nbr.Distance

    allNbrs = sortedNeighborQuery.ToList()


    ' go sequentially through neighbors
    Dim equidistantNeighbors As New List(Of Neighbor)
    Dim firstWtIndex As Integer = 0
    Dim nextWtIndex As Integer = 0
    Do While firstWtIndex <= nbrWts.Count - 1
      ' clear list
      equidistantNeighbors.Clear()
      ' get all equidistant neighbors
      ' loop condition
      Dim keepGoing As Boolean = True
      Do While keepGoing
        ' add neighbor to list
        equidistantNeighbors.Add(allNbrs(nextWtIndex))
        nextWtIndex += 1
        ' check condition
        If nextWtIndex > allNbrs.Count - 1 Then
          keepGoing = False ' end of list condition
        ElseIf allNbrs(nextWtIndex).Distance <> allNbrs(firstWtIndex).Distance Then
          keepGoing = False
        End If
      Loop
      ' get sum of weights for next n neighbors
      ' where n is number of equidistant neighbors
      Dim lastWtIndex As Integer = nextWtIndex - 1
      If lastWtIndex > nbrWts.Count - 1 Then lastWtIndex = nbrWts.Count - 1
      Dim wtSum As Double = 0
      For tallyIndex = firstWtIndex To lastWtIndex
        wtSum += nbrWts(tallyIndex)
      Next
      ' divvy these up among the equidistant neighbors
      lastWtIndex = nextWtIndex - 1
      Dim wtPer As Double = wtSum / (lastWtIndex - firstWtIndex + 1)
      For i = firstWtIndex To lastWtIndex
        Dim REL As cCoLocationEngine.sNeighborRel
        REL.nbID = allNbrs(i).ID
        REL.nbWeight = wtPer
        R.Add(REL)
      Next
      ' then update first index
      firstWtIndex = nextWtIndex
    Loop
    ' return result
    Return R
  End Function
  Public Function getJointPopMetric() As Double Implements cCoLocationEngine.iNeighborhoodCalculator.getJointPopMetric
    ' metric is average weighted distance to neighbors
    Dim thisD As Double, totD As Double = 0
    Dim numPts As Integer
    Dim thisNB As List(Of Neighbor)
    ' error checking
    If pIndex Is Nothing Then Return Nothing
    numPts = pIndex.NodeList.Count
    If numPts = 0 Then Return Nothing
    ' loop through points
    For i = 0 To numPts - 1
      ' get neighbor relations
      thisNB = pIndex.nearestNeighborList(i, Me.numNeighbors)
      ' get weighted average of distance
      thisD = weightedAverageDistance(thisNB)
      ' add to total
      totD += thisD
    Next
    ' return average
    Return totD / numPts
  End Function
  Private Function weightedAverageDistance(ByVal nb As List(Of Neighbor)) As Double
    ' calculates the weighted average distance to neighbors
    Dim totWtD As Double = 0
    Dim numNb As Integer = Me.numNeighbors
    ' error checking
    If nb Is Nothing Then Return Nothing
    If nb.Count < numNb Then Return Nothing
    ' loop through neighbors to calculate sum of weight*distance
    For i = 0 To numNb - 1
      totWtD += nb.Item(i).Distance * pNbWt(i)
    Next
    ' calculate sum of weights
    Dim totWt As Double = pNbWt.Sum
    ' return weighted average
    Return totWtD / totWt
  End Function
End Class
#Region "Null Model Simulators"
Public Class RL_Simulator
  ' Reclassifies using random labeling
  ' in other words, shuffles the categories of all points
  Implements cCoLocationEngine.iSimulator
  Dim ObsCatID() As Integer
  Dim SimCatID() As Integer
  Dim ClassNames() As String
  Dim Neighbors() As List(Of cCoLocationEngine.sNeighborRel)
  Dim pNNCT As cContingencyTable
  Dim pPopMet As Double ' joint population pattern metric

  Public Sub New(ByVal inObsCatID() As Integer, _
                    ByVal inClassNames() As String, _
                        ByVal nbHood As cCoLocationEngine.iNeighborhoodCalculator, _
                        ByVal inNeighbors() As List(Of cCoLocationEngine.sNeighborRel))
    ObsCatID = inObsCatID
    ClassNames = inClassNames
    Neighbors = inNeighbors
    ' calculate population metric (average neighbor distance)
    pPopMet = nbHood.getJointPopMetric
  End Sub
  Public Function Reclassify(ByVal inCatID() As Integer) As Integer() ' Implements cCoLocationEngine.iSimulator.Reclassify
    ' this should be easy (famous last words!)
    ' make a copy!!!
    Dim cID() As Integer = inCatID.Clone
    ' get a random sequence
    Dim swapWithID() As Integer = Data.Sorting.randomOrder(cID.Length)
    ' create result array
    Dim R() As Integer
    ReDim R(cID.Length - 1)
    ' get category of randomly sequenced partner
    For i = 0 To cID.Length - 1
      R(i) = cID(swapWithID(i))
    Next
    Return R
  End Function
  Public ReadOnly Property NullModelName As String Implements cCoLocationEngine.iSimulator.NullModelName
    Get
      Return "RandomLabeling"
    End Get
  End Property
  Public ReadOnly Property StateString As String Implements cCoLocationEngine.iSimulator.StateString
    Get
      Return "Standard"
    End Get
  End Property

  Public Sub runSim() Implements cCoLocationEngine.iSimulator.runSim
    SimCatID = Reclassify(ObsCatID)
    pNNCT = clqUtils.calcNCT(SimCatID, ClassNames, Neighbors)
  End Sub

  Public Function expectedNNCT(ByVal obsNNCT As cContingencyTable) As cContingencyTable Implements cCoLocationEngine.iSimulator.expectedNNCT
    Dim R As New cContingencyTable(obsNNCT.classNames)
    ' designate no fixed classes
    Dim fixedCat() As Boolean
    ReDim fixedCat(R.numClasses - 1)
    For i = 0 To R.numClasses - 1
      fixedCat(i) = False
    Next
    For A = 0 To R.numClasses - 1
      For B = 0 To R.numClasses - 1
        R.Value(A, B) = clqUtils.expectedNNCount(A, B, obsNNCT, fixedCat)
      Next
    Next
    Return R
  End Function

  Public ReadOnly Property simNNCT As cContingencyTable Implements cCoLocationEngine.iSimulator.simNNCT
    Get
      Return pNNCT
    End Get
  End Property

  Public ReadOnly Property simJointPopMetric As Double Implements cCoLocationEngine.iSimulator.simJointPopMetric
    Get
      Return pPopMet
    End Get
  End Property

  Public ReadOnly Property lastSimClasses As Integer() Implements cCoLocationEngine.iSimulator.lastSimClasses
    Get
      Return SimCatID
    End Get
  End Property

  Public ReadOnly Property lastSimPts As twoDTree.sPoint() Implements cCoLocationEngine.iSimulator.lastSimPts
    Get
      Return Nothing
    End Get
  End Property
End Class
Public Class CRL_FixedCats_Simulator
  ' Reclassifies using constrained random labeling
  ' requires specification of fixed categories upon initialization
  Implements cCoLocationEngine.iSimulator
  Dim pFixedCat As New HashSet(Of Integer)
  Dim SimCatID() As Integer
  Dim ObsCatID() As Integer
  Dim ClassNames() As String
  Dim Neighbors() As List(Of cCoLocationEngine.sNeighborRel)
  Dim pNNCT As cContingencyTable
  Dim pPopMet As Double ' joint population pattern metric
  Public Sub New(ByVal fixedCat() As Integer, _
                 ByVal inObsCatID() As Integer, _
                    ByVal inClassNames() As String, _
                    ByVal nbHood As cCoLocationEngine.iNeighborhoodCalculator, _
                    ByVal inNeighbors() As List(Of cCoLocationEngine.sNeighborRel))
    ObsCatID = inObsCatID
    ClassNames = inClassNames
    Neighbors = inNeighbors
    ' add fixed categories to hash set
    For Each ID As Integer In fixedCat
      pFixedCat.Add(ID)
    Next
    ' calculate population pattern metric
    pPopMet = nbHood.getJointPopMetric
  End Sub
  Public Sub New(ByVal fixedCat As List(Of Integer), _
                 ByVal inObsCatID() As Integer, _
                    ByVal inClassNames() As String, _
                    ByVal nbHood As cCoLocationEngine.iNeighborhoodCalculator, _
                    ByVal inNeighbors() As List(Of cCoLocationEngine.sNeighborRel))
    ObsCatID = inObsCatID
    ClassNames = inClassNames
    Neighbors = inNeighbors
    ' add fixed categories to hash set
    For Each ID As Integer In fixedCat
      pFixedCat.Add(ID)
    Next
    ' calculate population pattern metric
    pPopMet = nbHood.getJointPopMetric
  End Sub
  Public Function Reclassify(ByVal CatID() As Integer) As Integer() ' Implements cCoLocationEngine.iSimulator.Reclassify
    ' implements constrained random labeling
    ' keeping fixed categories constant (defined by pFixedCat)

    ' make a copy
    Dim inCatID() As Integer = CatID.Clone

    ' create a subset of elements that are to be swapped around
    Dim listOfPtIDsThatCanBeSwapped As New List(Of Integer)
    For i = 0 To inCatID.Length - 1
      If Not pFixedCat.Contains(inCatID(i)) Then listOfPtIDsThatCanBeSwapped.Add(i)
    Next
    Dim ptIDsThatCanBeSwapped() As Integer = listOfPtIDsThatCanBeSwapped.ToArray
    ' get swap list
    Dim swapID() As Integer
    ReDim swapID(ptIDsThatCanBeSwapped.Count - 1)
    swapID = Data.Sorting.randomOrder(swapID.Length)
    ' set up result with original categories
    Dim outCatID() As Integer = inCatID.Clone
    Dim curPtID As Integer
    Dim swapPtID As Integer
    ' swap around those pts that can be swapped
    For i = 0 To ptIDsThatCanBeSwapped.Length - 1
      curPtID = ptIDsThatCanBeSwapped(i)
      swapPtID = ptIDsThatCanBeSwapped(swapID(i))
      outCatID(curPtID) = inCatID(swapPtID)
    Next
    Return outCatID
  End Function
  Public ReadOnly Property NullModelName As String Implements cCoLocationEngine.iSimulator.NullModelName
    Get
      Return "ConstrainedRandomLabeling"
    End Get
  End Property
  Public ReadOnly Property StateString As String Implements cCoLocationEngine.iSimulator.StateString
    Get
      ' create string designating fixed categories
      If pFixedCat.Count = 0 Then Return "nonefixed"
      Dim SS As String = pFixedCat(0).ToString
      For i = 1 To pFixedCat.Count - 1
        SS &= "_" & pFixedCat(i).ToString
      Next
      Return SS
    End Get
  End Property
  Public Sub runSim() Implements cCoLocationEngine.iSimulator.runSim
    SimCatID = Reclassify(ObsCatID)
    pNNCT = clqUtils.calcNCT(SimCatID, ClassNames, Neighbors)
  End Sub
  Public Function expectedNNCT(ByVal obsNNCT As cContingencyTable) As cContingencyTable Implements cCoLocationEngine.iSimulator.expectedNNCT
    Dim R As New cContingencyTable(obsNNCT.classNames)
    ' designate no fixed classes
    Dim fixedCat() As Boolean
    ReDim fixedCat(R.numClasses - 1)
    For i = 0 To R.numClasses - 1
      fixedCat(i) = False
    Next
    For i = 0 To pFixedCat.Count - 1
      fixedCat(pFixedCat(i)) = True
    Next
    ' calculate expectations
    For A = 0 To R.numClasses - 1
      For B = 0 To R.numClasses - 1
        R.Value(A, B) = clqUtils.expectedNNCount(A, B, obsNNCT, fixedCat)
      Next
    Next
    Return R
  End Function
  Public ReadOnly Property simNNCT As cContingencyTable Implements cCoLocationEngine.iSimulator.simNNCT
    Get
      Return pNNCT
    End Get
  End Property
  Public ReadOnly Property simJointPopMetric As Double Implements cCoLocationEngine.iSimulator.simJointPopMetric
    Get
      Return pPopMet
    End Get
  End Property
  Public ReadOnly Property lastSimClasses As Integer() Implements cCoLocationEngine.iSimulator.lastSimClasses
    Get
      Return SimCatID
    End Get
  End Property

  Public ReadOnly Property lastSimPts As twoDTree.sPoint() Implements cCoLocationEngine.iSimulator.lastSimPts
    Get
      Return Nothing
    End Get
  End Property
End Class
Public Class CRL_FixedBase_Simulator
  ' Reclassifies using constrained random labeling
  ' The base category is always fixed
  Implements cCoLocationEngine.iSimulator
  Dim SimCatID() As Integer
  Dim ObsCatID() As Integer
  Dim ClassNames() As String
  Dim Neighbors() As List(Of cCoLocationEngine.sNeighborRel)
  Dim pNNCT As cContingencyTable
  Dim pPt() As twoDTree.sPoint
  Dim pPopMet As Double ' joint population metric
  Public Sub New(ByVal pt() As twoDTree.sPoint, _
                 ByVal inObsCatID() As Integer, _
                    ByVal inClassNames() As String, _
                    ByVal nbHood As cCoLocationEngine.iNeighborhoodCalculator, _
                    ByVal inNeighbors() As List(Of cCoLocationEngine.sNeighborRel))
    ObsCatID = inObsCatID
    ClassNames = inClassNames
    Neighbors = inNeighbors
    ' calculate population metric (average neighbor distance)
    pPopMet = nbHood.getJointPopMetric
  End Sub
  Public Function Reclassify(ByVal CatID() As Integer, ByVal fixedCat As Integer) As Integer() ' Implements cCoLocationEngine.iSimulator.Reclassify
    ' implements constrained random labeling
    ' keeping fixed categories constant (defined by pFixedCat)

    ' make a copy
    Dim inCatID() As Integer = CatID.Clone

    ' create a subset of elements that are to be swapped around
    Dim listOfPtIDsThatCanBeSwapped As New List(Of Integer)
    For i = 0 To inCatID.Length - 1
      If Not fixedCat = inCatID(i) Then listOfPtIDsThatCanBeSwapped.Add(i)
    Next
    Dim ptIDsThatCanBeSwapped() As Integer = listOfPtIDsThatCanBeSwapped.ToArray
    ' get swap list
    Dim swapID() As Integer
    ReDim swapID(ptIDsThatCanBeSwapped.Count - 1)
    swapID = Data.Sorting.randomOrder(swapID.Length)
    ' set up result with original categories
    Dim outCatID() As Integer = inCatID.Clone
    Dim curPtID As Integer
    Dim swapPtID As Integer
    ' swap around those pts that can be swapped
    For i = 0 To ptIDsThatCanBeSwapped.Length - 1
      curPtID = ptIDsThatCanBeSwapped(i)
      swapPtID = ptIDsThatCanBeSwapped(swapID(i))
      outCatID(curPtID) = inCatID(swapPtID)
    Next
    Return outCatID
  End Function
  Public ReadOnly Property NullModelName As String Implements cCoLocationEngine.iSimulator.NullModelName
    Get
      Return "RRL (row)"
    End Get
  End Property
  Public ReadOnly Property StateString As String Implements cCoLocationEngine.iSimulator.StateString
    Get
      ' create string designating base category fixed
      Dim SS As String = "BaseFixed"
      Return SS
    End Get
  End Property
  Public Sub runSim() Implements cCoLocationEngine.iSimulator.runSim
    ' create contingency table for each row, then combine them
    Dim rowCC() As cContingencyTable
    Dim numCats As Integer = ClassNames.Count
    ReDim rowCC(numCats - 1)
    ' loop through rows to get contingency tables for each row
    For rowID = 0 To numCats - 1
      SimCatID = Reclassify(ObsCatID, rowID)
      rowCC(rowID) = clqUtils.calcNCT(SimCatID, ClassNames, Neighbors)
    Next
    ' combine them together
    Dim R As New cContingencyTable(ClassNames)
    For rowID = 0 To numCats - 1
      For colID = 0 To numCats - 1
        R.Value(rowID, colID) = rowCC(rowID).Value(rowID, colID)
      Next
    Next
    pNNCT = R
  End Sub
  Public Function expectedNNCT(ByVal obsNNCT As cContingencyTable) As cContingencyTable Implements cCoLocationEngine.iSimulator.expectedNNCT
    Dim R As New cContingencyTable(obsNNCT.classNames)
    ' designate no fixed classes
    Dim fixedCat() As Boolean
    ReDim fixedCat(R.numClasses - 1)
    For i = 0 To R.numClasses - 1
      fixedCat(i) = False
    Next
    ' loop through base classes
    For A = 0 To R.numClasses - 1
      ' free last base class
      If A > 0 Then fixedCat(A - 1) = False
      ' constrain current base class
      fixedCat(A) = True
      ' loop through neighbor classes and calculate expectations
      For B = 0 To R.numClasses - 1
        R.Value(A, B) = clqUtils.expectedNNCount(A, B, obsNNCT, fixedCat)
      Next
    Next
    Return R
  End Function
  Public ReadOnly Property simNNCT As cContingencyTable Implements cCoLocationEngine.iSimulator.simNNCT
    Get
      Return pNNCT
    End Get
  End Property
  Public ReadOnly Property simJointPopMetric As Double Implements cCoLocationEngine.iSimulator.simJointPopMetric
    Get
      Return pPopMet
    End Get
  End Property
  Public ReadOnly Property lastSimClasses As Integer() Implements cCoLocationEngine.iSimulator.lastSimClasses
    Get
      ' note this only simulates for the first cat fixed
      Return SimCatID
    End Get
  End Property

  Public ReadOnly Property lastSimPts As twoDTree.sPoint() Implements cCoLocationEngine.iSimulator.lastSimPts
    Get
      Return Nothing
    End Get
  End Property
End Class
Public Class CRL_FixedNeighbor_Simulator
  ' Reclassifies using constrained random labeling
  ' The base category is always fixed
  Implements cCoLocationEngine.iSimulator
  Dim SimCatID() As Integer
  Dim ObsCatID() As Integer
  Dim ClassNames() As String
  Dim Neighbors() As List(Of cCoLocationEngine.sNeighborRel)
  Dim pNNCT As cContingencyTable
  Dim pPopMet As Double ' joint population pattern metric
  Public Sub New(ByVal inObsCatID() As Integer, _
                    ByVal inClassNames() As String, _
                    ByVal nbHood As cCoLocationEngine.iNeighborhoodCalculator, _
                    ByVal inNeighbors() As List(Of cCoLocationEngine.sNeighborRel))
    ObsCatID = inObsCatID
    ClassNames = inClassNames
    Neighbors = inNeighbors
    pPopMet = nbHood.getJointPopMetric
  End Sub
  Public Function Reclassify(ByVal CatID() As Integer, ByVal fixedCat As Integer) As Integer() ' Implements cCoLocationEngine.iSimulator.Reclassify
    ' implements constrained random labeling
    ' keeping fixed categories constant (defined by pFixedCat)

    ' make a copy
    Dim inCatID() As Integer = CatID.Clone

    ' create a subset of elements that are to be swapped around
    Dim listOfPtIDsThatCanBeSwapped As New List(Of Integer)
    For i = 0 To inCatID.Length - 1
      If Not fixedCat = inCatID(i) Then listOfPtIDsThatCanBeSwapped.Add(i)
    Next
    Dim ptIDsThatCanBeSwapped() As Integer = listOfPtIDsThatCanBeSwapped.ToArray
    ' get swap list
    Dim swapID() As Integer
    ReDim swapID(ptIDsThatCanBeSwapped.Count - 1)
    swapID = Data.Sorting.randomOrder(swapID.Length)
    ' set up result with original categories
    Dim outCatID() As Integer = inCatID.Clone
    Dim curPtID As Integer
    Dim swapPtID As Integer
    ' swap around those pts that can be swapped
    For i = 0 To ptIDsThatCanBeSwapped.Length - 1
      curPtID = ptIDsThatCanBeSwapped(i)
      swapPtID = ptIDsThatCanBeSwapped(swapID(i))
      outCatID(curPtID) = inCatID(swapPtID)
    Next
    Return outCatID
  End Function
  Public ReadOnly Property NullModelName As String Implements cCoLocationEngine.iSimulator.NullModelName
    Get
      Return "RRL (col)"
    End Get
  End Property
  Public ReadOnly Property StateString As String Implements cCoLocationEngine.iSimulator.StateString
    Get
      ' create string designating base category fixed
      Dim SS As String = "NeighborFixed"
      Return SS
    End Get
  End Property
  Public Sub runSim() Implements cCoLocationEngine.iSimulator.runSim
    ' create contingency table for each column, then combine them
    Dim numCats As Integer = ClassNames.Count
    Dim colCC() As cContingencyTable
    ReDim colCC(numCats - 1)
    ' loop through columns to get contingency tables for each column
    For colID = 0 To numCats - 1
      SimCatID = Reclassify(ObsCatID, colID)
      colCC(colID) = clqUtils.calcNCT(SimCatID, ClassNames, Neighbors)
    Next
    ' combine them together
    Dim R As New cContingencyTable(ClassNames)
    For rowID = 0 To numCats - 1
      For colID = 0 To numCats - 1
        R.Value(rowID, colID) = colCC(colID).Value(rowID, colID)
      Next
    Next
    pNNCT = R
  End Sub
  Public Function expectedNNCT(ByVal obsNNCT As cContingencyTable) As cContingencyTable Implements cCoLocationEngine.iSimulator.expectedNNCT
    Dim R As New cContingencyTable(obsNNCT.classNames)
    ' designate no fixed classes
    Dim fixedCat() As Boolean
    ReDim fixedCat(R.numClasses - 1)
    For i = 0 To R.numClasses - 1
      fixedCat(i) = False
    Next
    ' loop through neighbor classes
    For B = 0 To R.numClasses - 1
      ' free last neighbor
      If B > 0 Then fixedCat(B - 1) = False
      ' constrain thy current neighbor
      fixedCat(B) = True
      ' loop through base classes and calculate expectations
      For A = 0 To R.numClasses - 1
        R.Value(A, B) = clqUtils.expectedNNCount(A, B, obsNNCT, fixedCat)
      Next
    Next
    Return R
  End Function
  Public ReadOnly Property simNNCT As cContingencyTable Implements cCoLocationEngine.iSimulator.simNNCT
    Get
      Return pNNCT
    End Get
  End Property
  Public ReadOnly Property simJointPopMetric As Double Implements cCoLocationEngine.iSimulator.simJointPopMetric
    Get
      Return pPopMet
    End Get
  End Property
  Public ReadOnly Property lastSimClasses As Integer() Implements cCoLocationEngine.iSimulator.lastSimClasses
    Get
      ' note: this is only for reshuffling all but the first class
      Return SimCatID
    End Get
  End Property

  Public ReadOnly Property lastSimPts As twoDTree.sPoint() Implements cCoLocationEngine.iSimulator.lastSimPts
    Get
      Return Nothing
    End Get
  End Property
End Class
Public Class CRL_FixedOther_Simulator
  ' Reclassifies using constrained random labeling
  ' All categories except the base and neighbor category are always fixed
  Implements cCoLocationEngine.iSimulator
  Dim SimCatID() As Integer
  Dim ObsCatID() As Integer
  Dim ClassNames() As String
  Dim Neighbors() As List(Of cCoLocationEngine.sNeighborRel)
  Dim pNNCT As cContingencyTable
  Dim pPopMet As Double ' joint population pattern metric
  Public Sub New(ByVal inObsCatID() As Integer, _
                    ByVal inClassNames() As String, _
                    ByVal nbHood As cCoLocationEngine.iNeighborhoodCalculator, _
                    ByVal inNeighbors() As List(Of cCoLocationEngine.sNeighborRel))
    ObsCatID = inObsCatID
    ClassNames = inClassNames
    Neighbors = inNeighbors
    pPopMet = nbHood.getJointPopMetric
  End Sub
  Public Function Reclassify(ByVal CatID() As Integer, ByVal freeCat1 As Integer, ByVal freeCat2 As Integer) As Integer() ' Implements cCoLocationEngine.iSimulator.Reclassify
    ' implements constrained random labeling
    ' keeping fixed categories constant (defined by pFixedCat)

    ' make a copy
    Dim inCatID() As Integer = CatID.Clone

    ' create a subset of elements that are to be swapped around
    Dim listOfPtIDsThatCanBeSwapped As New List(Of Integer)
    For i = 0 To inCatID.Length - 1
      If inCatID(i) = freeCat1 Or inCatID(i) = freeCat2 Then
        listOfPtIDsThatCanBeSwapped.Add(i)
      End If
    Next
    Dim ptIDsThatCanBeSwapped() As Integer = listOfPtIDsThatCanBeSwapped.ToArray
    ' get swap list
    Dim swapID() As Integer
    ReDim swapID(ptIDsThatCanBeSwapped.Count - 1)
    swapID = Data.Sorting.randomOrder(swapID.Length)
    ' set up result with original categories
    Dim outCatID() As Integer = inCatID.Clone
    Dim curPtID As Integer
    Dim swapPtID As Integer
    ' swap around those pts that can be swapped
    For i = 0 To ptIDsThatCanBeSwapped.Length - 1
      curPtID = ptIDsThatCanBeSwapped(i)
      swapPtID = ptIDsThatCanBeSwapped(swapID(i))
      outCatID(curPtID) = inCatID(swapPtID)
    Next
    Return outCatID
  End Function
  Public ReadOnly Property NullModelName As String Implements cCoLocationEngine.iSimulator.NullModelName
    Get
      Return "ConstrainedRandomLabeling"
    End Get
  End Property
  Public ReadOnly Property StateString As String Implements cCoLocationEngine.iSimulator.StateString
    Get
      ' create string designating base category fixed
      Dim SS As String = "OtherFixed"
      Return SS
    End Get
  End Property
  Public Sub runSim() Implements cCoLocationEngine.iSimulator.runSim
    ' create contingency table for each cell, then combine them
    Dim numCats As Integer = ClassNames.Count
    Dim cellCC(,) As cContingencyTable
    ReDim cellCC(numCats - 1, numCats - 1)
    ' loop through rows & columns to get contingency tables for each column
    For rowID = 0 To numCats - 1
      For colID = 0 To numCats - 1
        SimCatID = Reclassify(ObsCatID, rowID, colID)
        cellCC(rowID, colID) = clqUtils.calcNCT(SimCatID, ClassNames, Neighbors)
      Next colID
    Next rowID
    ' combine them together
    Dim R As New cContingencyTable(ClassNames)
    For rowID = 0 To numCats - 1
      For colID = 0 To numCats - 1
        R.Value(rowID, colID) = cellCC(rowID, colID).Value(rowID, colID)
      Next
    Next
    pNNCT = R
  End Sub
  Public Function expectedNNCT(ByVal obsNNCT As cContingencyTable) As cContingencyTable Implements cCoLocationEngine.iSimulator.expectedNNCT
    ' this is implemented the easy way
    ' slightly inefficient, but since we are just adding extra boolean assignments,
    ' I doubt the inefficiency will be noticable
    Dim R As New cContingencyTable(obsNNCT.classNames)
    Dim fixedCat() As Boolean
    ReDim fixedCat(R.numClasses - 1)
    ' loop through base classe
    For A = 0 To R.numClasses - 1
      ' loop through neighbor class
      For B = 0 To R.numClasses - 1
        ' designate all fixed classes
        For i = 0 To R.numClasses - 1
          fixedCat(i) = True
        Next
        ' free up A and B
        fixedCat(A) = False
        fixedCat(B) = False
        ' calculate expectation
        R.Value(A, B) = clqUtils.expectedNNCount(A, B, obsNNCT, fixedCat)
      Next
    Next
    Return R
  End Function
  Public ReadOnly Property simNNCT As cContingencyTable Implements cCoLocationEngine.iSimulator.simNNCT
    Get
      Return pNNCT
    End Get
  End Property
  Public ReadOnly Property simJointPopMetric As Double Implements cCoLocationEngine.iSimulator.simJointPopMetric
    Get
      Return pPopMet
    End Get
  End Property
  Public ReadOnly Property lastSimClasses As Integer() Implements cCoLocationEngine.iSimulator.lastSimClasses
    Get
      ' there is no single set of classes, since simulation is performed
      ' separately for each from class
      Return Nothing
    End Get
  End Property

  Public ReadOnly Property lastSimPts As twoDTree.sPoint() Implements cCoLocationEngine.iSimulator.lastSimPts
    Get
      Return Nothing
    End Get
  End Property
End Class
Public Class CSR_Simulator
  Implements cCoLocationEngine.iSimulator
  Dim simPt() As twoDTree.sPoint
  Dim ObsCatID() As Integer
  Dim ClassNames() As String
  Dim Neighbors() As List(Of cCoLocationEngine.sNeighborRel)
  Dim pNNCT As cContingencyTable
  Private pOutline As DotSpatial.Data.Feature
  Private pNbHood As cCoLocationEngine.iNeighborhoodCalculator
  Private pPopMet As Double
  ' implements complete spatial randomness within the defined outline
  ' uses simple "try-and-discard" method to place points within this outline
  Public Sub New(ByVal inObsCatID() As Integer, _
                    ByVal inClassNames() As String, _
                    ByVal nbHood As cCoLocationEngine.iNeighborhoodCalculator, _
                    ByVal inNeighbors() As List(Of cCoLocationEngine.sNeighborRel), _
                    ByVal Outline As DotSpatial.Data.Feature)
    ObsCatID = inObsCatID
    ClassNames = inClassNames
    Neighbors = inNeighbors
    pNbHood = nbHood
    pOutline = Outline
  End Sub

  Public ReadOnly Property NullModelName As String Implements cCoLocationEngine.iSimulator.NullModelName
    Get
      Return "Complete Spatial Randomness"
    End Get
  End Property

  Public Sub runSim() Implements cCoLocationEngine.iSimulator.runSim
    ' Important: must set outline before running any simulations

    ' create new point locations within polygon

    Dim outlineEnv As DotSpatial.Topology.Envelope = pOutline.Envelope
    Dim X As New DotSpatial.Data.Extent(outlineEnv)
    Dim PtIndex As New twoDTree
    Dim newX, newY As Double, newC As DotSpatial.Topology.Coordinate
    Randomize()
    ReDim simPt(ObsCatID.Length - 1)
    For i = 0 To ObsCatID.Length - 1
      Dim insidePoly As Boolean = False
      Do While insidePoly = False
        newX = X.MinX + Rnd() * (X.MaxX - X.MinX)
        newY = X.MinY + Rnd() * (X.MaxY - X.MinY)
        newC = New DotSpatial.Topology.Coordinate(newX, newY)
        If Spatial.ShapefileUtils.pointInPoly(newC, pOutline) Then insidePoly = True
      Loop
      PtIndex.addPoint(newX, newY)
      simPt(i).x = newX
      simPt(i).y = newY
    Next
    ' determine new neighbor relations
    Dim numNeighbors As Integer = Neighbors(0).Count
    Dim sampleNbRelList As List(Of cCoLocationEngine.sNeighborRel) = Neighbors(0)
    For i = 0 To ObsCatID.Length - 1
      Dim nbList As List(Of Neighbor) = PtIndex.nearestNeighborList(i, numNeighbors)
      Dim nbRels As New List(Of cCoLocationEngine.sNeighborRel)
      Dim newNbRelList As New List(Of cCoLocationEngine.sNeighborRel)
      For j = 0 To sampleNbRelList.Count - 1
        Dim nbRel As cCoLocationEngine.sNeighborRel
        nbRel.nbID = nbList(j).ID
        nbRel.nbWeight = sampleNbRelList(j).nbWeight
        newNbRelList.Add(nbRel)
      Next
      Neighbors(i) = newNbRelList
    Next
    ' calculate contingency table
    pNNCT = clqUtils.calcNCT(ObsCatID, ClassNames, Neighbors)
    ' calculate joint population pattern metric
    pNbHood.createIndex(PtIndex)
    pPopMet = pNbHood.getJointPopMetric
  End Sub
  Public ReadOnly Property StateString As String Implements cCoLocationEngine.iSimulator.StateString
    Get
      Return "Standard"
    End Get
  End Property
  Public Function expectedNNCT(ByVal obsNNCT As cContingencyTable) As cContingencyTable Implements cCoLocationEngine.iSimulator.expectedNNCT
    Dim R As New cContingencyTable(obsNNCT.classNames)
    ' should be same as random labeling
    ' designate no fixed classes
    Dim fixedCat() As Boolean
    ReDim fixedCat(R.numClasses - 1)
    For i = 0 To R.numClasses - 1
      fixedCat(i) = False
    Next
    For A = 0 To R.numClasses - 1
      For B = 0 To R.numClasses - 1
        R.Value(A, B) = clqUtils.expectedNNCount(A, B, obsNNCT, fixedCat)
      Next
    Next
    Return R
  End Function
  Public ReadOnly Property simNNCT As cContingencyTable Implements cCoLocationEngine.iSimulator.simNNCT
    Get
      Return pNNCT
    End Get
  End Property
  Public ReadOnly Property simJointPopMetric As Double Implements cCoLocationEngine.iSimulator.simJointPopMetric
    Get
      Return pPopMet
    End Get
  End Property
  Public ReadOnly Property lastSimClasses As Integer() Implements cCoLocationEngine.iSimulator.lastSimClasses
    Get
      ' classes don't change, locations do
      Return ObsCatID
    End Get
  End Property
  Public ReadOnly Property lastSimPts As twoDTree.sPoint() Implements cCoLocationEngine.iSimulator.lastSimPts
    Get
      Return simPt
    End Get
  End Property
End Class
Public Class ToroidalShift_Simulator
  Implements cCoLocationEngine.iSimulator
  Dim ObsCatID() As Integer
  Dim simPt() As twoDTree.sPoint
  Dim ClassNames() As String
  Dim nbrCalc As EuclideanNeighborCalculator
  Private origPt() As twoDTree.sPoint
  Private pOutline As DotSpatial.Data.Extent
  Dim pNNCT As cContingencyTable
  ' subpopulation indices
  '  Private subIndex() As toroidalIndex
  'Private anyID() As Integer ' the ID of a point in each category
  Private pPopMet As Double ' joint pattern population metric
  ' This could be made more efficient by creating separate indices for each mark category,
  ' so the index wouldn't have to be updated every iteration
  Public Sub New(ByVal inObsCatID() As Integer, _
                    ByVal uniqueClassNames() As String, _
                    ByVal neighborCalculator As EuclideanNeighborCalculator, _
                    ByVal originalPoints As twoDTree.sPoint(), _
                 ByVal Outline As DotSpatial.Data.Extent)
    ObsCatID = inObsCatID
    ClassNames = uniqueClassNames
    nbrCalc = neighborCalculator
    origPt = originalPoints
    pOutline = Outline
    Dim outlineBox As twoDTree.sBox
    outlineBox.Bottom = pOutline.MinY
    outlineBox.Top = pOutline.MaxY
    outlineBox.Left = pOutline.MinX
    outlineBox.Right = pOutline.MaxX
    ' create subpopulations index objects
    'ReDim subIndex(ClassNames.Count - 1)
    'For i = 0 To ClassNames.Count - 1
    '  subIndex(i) = New toroidalIndex(outlineBox)
    'Next
    ' populate subpopulation indices
    ' (a) create lists to contain points in each class
    'Dim subPtLists() As List(Of twoDTree.sPoint)
    'ReDim subPtLists(ClassNames.Count - 1)
    'For i = 0 To ClassNames.Count - 1
    '  subPtLists(i) = New List(Of twoDTree.sPoint)
    'Next
    '' (b) populate these lists with pt coordinates
    'For i = 0 To ObsCatID.Count - 1
    '  Dim p As twoDTree.sPoint = originalPoints(i)
    '  Dim p2 As twoDTree.sPoint
    '  p2.x = p.x
    '  p2.y = p.y
    '  subPtLists(ObsCatID(i)).Add(p2)
    'Next
    ' (c) use these points to initialize the indices
    'For i = 0 To ClassNames.Count - 1
    '  subIndex(i).addPoints(subPtLists(i).ToArray)
    'Next
    ' create anyID list
    'ReDim anyID(ClassNames.Count - 1)
    'For i = 0 To ObsCatID.Count - 1
    '  anyID(ObsCatID(i)) = i
    'Next
  End Sub
  Public Sub runSim() Implements cCoLocationEngine.iSimulator.runSim
    ' shifts all subpopulations except the base population by a random amount
    ' up to one half the dimensions of the study area outline
    ' timing
    Dim overallWatch As Stopwatch
    Dim overallTime As TimeSpan
    overallWatch = Stopwatch.StartNew

    Dim numCats As Integer = ClassNames.Count
    Dim numPts As Integer = origPt.Count
    ' determine random shifts
    Dim xShift() As Double = getRandomShifts(False)
    Dim yShift() As Double = getRandomShifts(True)
    Dim shiftVec() As twoDTree.sPoint
    ReDim shiftVec(xShift.Length - 1)
    For i = 0 To shiftVec.Length - 1
      shiftVec(i) = twoDTree.getPoint(xShift(i), yShift(i))
    Next
    ' get width, height of outline
    Dim outlineWidth As Double = pOutline.MaxX - pOutline.MinX
    Dim outlineHeight As Double = pOutline.MaxY - pOutline.MinY
    ' set up lists of neighbor relations
    Dim nbList() As List(Of List(Of cCoLocationEngine.sNeighborRel))
    ReDim nbList(numCats - 1)
    For i = 0 To numCats - 1
      nbList(i) = New List(Of List(Of cCoLocationEngine.sNeighborRel))
    Next
    ' set up joint population metrics
    'Dim jPMet() As Double
    'ReDim jPMet(numCats - 1)
    ' loop through classes
    For k = 0 To numCats - 1
      ' reset points
      ReDim simPt(ObsCatID.Length - 1)
      ' loop through points
      For i = 0 To ObsCatID.Length - 1
        ' determine if class is the same
        If ObsCatID(i) = k Then
          ' if so, just get point coordinates
          simPt(i).x = origPt(i).x
          simPt(i).y = origPt(i).y
        Else
          ' otherwise, apply the shift
          simPt(i).x = origPt(i).x + xShift(ObsCatID(i))
          simPt(i).y = origPt(i).y + yShift(ObsCatID(i))
          ' check bounds, adjust toroidally
          If simPt(i).x < pOutline.MinX Then simPt(i).x = simPt(i).x + outlineWidth
          If simPt(i).x > pOutline.MaxX Then simPt(i).x = simPt(i).x - outlineWidth
          If simPt(i).y < pOutline.MinY Then simPt(i).y = simPt(i).y + outlineHeight
          If simPt(i).y > pOutline.MaxY Then simPt(i).y = simPt(i).y - outlineHeight
        End If
      Next i
      ' create index
      Dim thisIndex As New EuclideanNeighborCalculator()
      thisIndex.numNeighbors = nbrCalc.numNeighbors
      thisIndex.neighborWeightsTemplate = nbrCalc.neighborWeightsTemplate
      thisIndex.createIndex(simPt)
      ' get nearest neighbor relations
      For i = 0 To numPts - 1
        ' only get neighbor relations if point is of cat K
        If ObsCatID(i) = k Then
          nbList(k).Add(thisIndex.getNeighborRelations(i))
        Else ' otherwise, add a dummy relation
          nbList(k).Add(New List(Of cCoLocationEngine.sNeighborRel))
        End If
        Application.DoEvents()
      Next
      ' get joint populatin metric
      'jPMet(k) = thisIndex.getJointPopMetric
    Next k
    ' combine neighbor relations
    Dim realNbrs() As List(Of cCoLocationEngine.sNeighborRel)
    ReDim realNbrs(numPts - 1)
    For i = 0 To numPts - 1
      realNbrs(i) = nbList(ObsCatID(i)).Item(i)
    Next
    ' get nearest neighbor counts
    Dim R As cContingencyTable = clqUtils.calcNCT(ObsCatID, ClassNames, realNbrs)
    ' get average joint population pattern metric
    'pPopMet = jPMet.Average
    ' timing
    overallTime = overallTime.Add(overallWatch.Elapsed)
    overallWatch.Stop()


    ' return result
    pNNCT = R
  End Sub
  'Public Function runSim_OLD() As cContingencyTable
  '  ' shifts points randomly within bounds of box
  '  ' keeps "from" class locations the same
  '  ' This is all achieved by actually shifting the test point only,
  '  ' so the original indices can be maintained
  '  ' (creating new indices each time is very slow!!!)
  '  ' *** FIXES NEEDED ***
  '  ' (1) Figure out why this doesn't work as expected (see test data) [is this just #2?]
  '  ' (2) Modify so it measures distances within the study area only
  '  '     (in other words, shift points toroidally but measure distances
  '  '      non-toroidally)
  '  '     To do this, we need to create a function in the 2dtree to find
  '  '      the nearest neighbors within a constrained rectangle [done]


  '  ' timing
  '  Dim overallWatch As Stopwatch
  '  Dim overallTime As TimeSpan
  '  overallWatch = Stopwatch.StartNew

  '  Dim numCats As Integer = ClassNames.Count
  '  Dim numPts As Integer = origPt.Count
  '  ' determine random shifts
  '  Dim xShift() As Double = getRandomShifts(False)
  '  Dim yShift() As Double = getRandomShifts(True)
  '  Dim shiftVec() As twoDTree.sPoint
  '  ReDim shiftVec(xShift.Length - 1)
  '  For i = 0 To shiftVec.Length - 1
  '    shiftVec(i) = twoDTree.getPoint(xShift(i), yShift(i))
  '  Next
  '  ' loop through points to find nearest neighbors after shifting
  '  Dim neighborLists() As List(Of cCoLocationEngine.sNeighborRel)
  '  ReDim neighborLists(numPts - 1)
  '  For ptID = 0 To numPts - 1
  '    ' get point information
  '    Dim curClass As Integer = ObsCatID(ptID)
  '    Dim ptCopy As twoDTree.sPoint
  '    ' set up lists of neighbors for each category
  '    Dim otherClassNeighbors() As List(Of Neighbor)
  '    ReDim otherClassNeighbors(numCats - 1)
  '    ' loop through categores
  '    For otherClass = 0 To numCats - 1
  '      ' get point coordinates
  '      ptCopy.x = origPt(ptID).x
  '      ptCopy.y = origPt(ptID).y
  '      ' shift position if point class is different from class being examined
  '      If otherClass <> curClass Then
  '        ptCopy.x += xShift(curClass)
  '        ptCopy.y += yShift(curClass)
  '      End If
  '      Dim shiftAsClass As New twoDTree.cPoint(xShift(curClass), yShift(curClass))
  '      ' get nearest neighbors within the other class
  '      ' >> if the other class is the same as the class of the current individual, then the current individual
  '      '    will show up in the results so we have to remove it
  '      ' >> retrieve the nearest neighbor within the original rectangle bounds, shifted with the current point
  '      '    (because it is as if we are shifting the entire population of hte other class)

  '      If otherClass = curClass Then
  '        otherClassNeighbors(otherClass) = _
  '          subIndex(otherClass).nearestNodeIDs(ptCopy, nbrCalc.numNeighbors + 1, , False).ToList
  '        otherClassNeighbors(otherClass).RemoveAt(0)
  '      Else
  '        otherClassNeighbors(otherClass) = _
  '          subIndex(otherClass).nearestNodeIDs(ptCopy, nbrCalc.numNeighbors, , False, shiftAsClass).ToList
  '      End If
  '      ' replace IDs in neighbor list to an ID from the master list of any point of the other class
  '      For Each nb As Neighbor In otherClassNeighbors(otherClass)
  '        nb.ID = anyID(otherClass)
  '      Next
  '    Next otherClass
  '    ' merge lists
  '    Dim combinedNbList As List(Of Neighbor) = combineNbrLists(otherClassNeighbors)
  '    ' get nearest neighbors with weights based on template
  '    neighborLists(ptID) = nbrCalc.neighborWeights(combinedNbList, nbrCalc.neighborWeightsTemplate)
  '  Next ptID
  '  ' create contingency table from neighbor lists
  '  Dim R As cContingencyTable = clqUtils.calcNCT(ObsCatID, ClassNames, neighborLists)


  '  ' timing
  '  overallTime = overallTime.Add(overallWatch.Elapsed)
  '  overallWatch.Stop()
  '  Dim timeIndexTotal As TimeSpan
  '  Dim timeDown As TimeSpan
  '  Dim timeUp As TimeSpan
  '  Dim timeSatisfy As TimeSpan
  '  Dim timeAddNb As TimeSpan
  '  Dim timePrelim As TimeSpan
  '  Dim timeSort As TimeSpan
  '  Dim timeTrimNb As TimeSpan
  '  Dim timeCP1, timeCP2, timeCP3 As TimeSpan
  '  For i = 0 To subIndex.Count - 1
  '    timeIndexTotal = timeIndexTotal.Add(subIndex(i).realTree.totalTime)
  '    timePrelim = timePrelim.Add(subIndex(i).realTree.timePreliminaries)
  '    timeDown = timeDown.Add(subIndex(i).realTree.timeNeedToMoveDown)
  '    timeUp = timeUp.Add(subIndex(i).realTree.timeNeedToMoveUp)
  '    timeSatisfy = timeSatisfy.Add(subIndex(i).realTree.timeToSatisfy)
  '    timeAddNb = timeAddNb.Add(subIndex(i).realTree.timeToAddNeighbor)
  '    timeTrimNb = timeTrimNb.Add(subIndex(i).realTree.timeToTrimNb)
  '    timeSort = timeSort.Add(subIndex(i).realTree.timeSort)
  '    timeCP1 = timeCP1.Add(subIndex(i).realTree.cp1Time)
  '    timeCP2 = timeCP2.Add(subIndex(i).realTree.cp2Time)
  '    timeCP3 = timeCP3.Add(subIndex(i).realTree.cp3Time)
  '  Next

  '  ' return contingency table as result
  '  Return R
  'End Function

  Private Function getRandomShifts(Optional ByVal vertical As Boolean = False) As Double()
    ' gets random shifts in the horizontal or vertical dimension
    ' where shift is up to +/- 1/2 the range of the study area in that dimension
    Dim numCat As Integer = ClassNames.Count
    Dim Shift(numCat - 1) As Double
    Dim Range As Double
    If vertical Then _
      Range = pOutline.MaxY - pOutline.MinY _
      Else Range = pOutline.MaxX - pOutline.MinX
    Randomize()
    For i = 0 To numCat - 1
      Shift(i) = Rnd() * Range - Range / 2
    Next i
    Return Shift
  End Function
  Public ReadOnly Property NullModelName As String Implements cCoLocationEngine.iSimulator.NullModelName
    Get
      Return "Toroidal Shift"
    End Get
  End Property

  'Public Function runSim_old() As cContingencyTable
  '  ' Important: must set outline before running any simulations
  '  ' create new point locations within outline extent rectangle
  '  ' A category is not shifted
  '  ' every other category is
  '  Dim numCats As Integer = ClassNames.Count
  '  Dim X As DotSpatial.Data.Extent = pOutline
  '  Dim numPts As Integer = ObsCatID.Length
  '  Randomize()
  '  ' get shifts for each mark category
  '  Dim xShift() As Double
  '  Dim yShift() As Double
  '  ReDim xShift(numCats - 1)
  '  ReDim yShift(numCats - 1)
  '  For i = 0 To numCats - 1
  '    xShift(i) = Rnd() * X.Width
  '    yShift(i) = Rnd() * X.Height
  '  Next
  '  ' get different counts for each category
  '  Dim catR() As cContingencyTable
  '  ReDim catR(numCats - 1)
  '  ' loop through categories
  '  For thisCat = 0 To numCats - 1
  '    ' shift point locations
  '    Dim newX(), newY() As Double
  '    ReDim newX(numPts - 1)
  '    ReDim newY(numPts - 1)
  '    For i = 0 To numPts - 1
  '      Dim insideOutline As Boolean = False
  '      Dim cat As Integer = ObsCatID(i)
  '      If cat <> thisCat Then ' shift points in other categories only
  '        newX(i) = origPt(i).X + xShift(cat)
  '        If newX(i) > pOutline.MaxX Then newX(i) -= pOutline.Width
  '        newY(i) = origPt(i).Y + yShift(cat)
  '        If newY(i) > pOutline.MaxY Then newY(i) -= pOutline.Height
  '      End If
  '    Next
  '    ' rebuild point index
  '    Dim PtIndex As New twoDTree
  '    For i = 0 To numPts - 1
  '      PtIndex.addPoint(newX(i), newY(i))
  '    Next
  '    ' determine new neighbor relations
  '    Dim numNeighbors As Integer = nbrCalc.numNeighbors

  '    For i = 0 To ObsCatID.Length - 1
  '      Dim nbList As List(Of Neighbor) = PtIndex.nearestNeighborList(i, numNeighbors)
  '      Dim nbRels As New List(Of cCoLocationEngine.sNeighborRel)
  '      Dim newNbRelList As New List(Of cCoLocationEngine.sNeighborRel)
  '      ' get neighbor weights, taking into account possible ties
  '      Dim actualNumNeighbors As Integer = nbList.Count
  '      Dim nbWeight As New List(Of Double)

  '      If actualNumNeighbors = numNeighbors Then
  '        For Each Rel In sampleNbRelList
  '          nbWeight.Add(Rel.nbWeight)
  '        Next
  '      Else ' for now, just split the weight of the furthest neighbor
  '        For Rel = 0 To sampleNbRelList.Count - 2
  '          nbWeight.Add(sampleNbRelList.Item(Rel).nbWeight)
  '        Next
  '        For rel = 1 To actualNumNeighbors - numNeighbors + 1
  '          nbWeight.Add(sampleNbRelList.Item(numNeighbors - 1).nbWeight / (actualNumNeighbors - numNeighbors + 1))
  '        Next
  '      End If
  '      For j = 0 To nbList.Count - 1
  '        Dim nbRel As cCoLocationEngine.sNeighborRel
  '        nbRel.nbID = nbList(j).ID
  '        nbRel.nbWeight = nbWeight.Item(j)
  '        newNbRelList.Add(nbRel)
  '      Next
  '      Neighbors(i) = newNbRelList
  '    Next
  '    ' calculate contingency table
  '    catR(thisCat) = cCoLocationEngine.calcContingencyTable(ObsCatID, ClassNames, Neighbors)
  '  Next
  '  ' merge contingency tables
  '  Dim R As New cContingencyTable(ClassNames)
  '  For i = 0 To numCats - 1
  '    For j = 0 To numCats - 1
  '      R.Value(i, j) = catR(i).Value(i, j)
  '    Next
  '  Next
  '  ' return result
  '  Return R
  'End Function

  Public ReadOnly Property StateString As String Implements cCoLocationEngine.iSimulator.StateString
    Get
      Return "Standard"
    End Get
  End Property
  Public Function expectedNNCT(ByVal obsNNCT As cContingencyTable) As cContingencyTable Implements cCoLocationEngine.iSimulator.expectedNNCT
    Dim R As New cContingencyTable(obsNNCT.classNames)
    ' I don't think it is possible to determine this analytically
    ' Instead, it is better to use average of empirical simulations
    For A = 0 To R.numClasses - 1
      For B = 0 To R.numClasses - 1
        R.Value(A, B) = -1
      Next
    Next
    Return R
  End Function
  Private Function combineNbrLists(ByVal nbList() As List(Of Neighbor)) As List(Of Neighbor)
    ' combines multiple lists of neighbors
    Dim R As New List(Of Neighbor)
    For Each L In nbList
      For Each nb In L
        R.Add(nb)
      Next
    Next
    Return R
  End Function
  Public Shared Function topNbrs(ByVal allNbrs As List(Of Neighbor), _
                           ByVal nbrWts() As Double) _
                         As List(Of cCoLocationEngine.sNeighborRel)
    ' input list should have more neighbors than required
    ' returns a list of neighbors with weights, corresponding to the
    ' nbrWts input which for example would say to weight the 1st neighbor 0.5, 2nd 0.3, etc.

    ' *** really should make explicit the requirement to 
    ' designate a neighbor weight list in the Interface specification
    ' or NEW sub

    Dim R As New List(Of cCoLocationEngine.sNeighborRel)
    ' sort list
    Dim sortedNeighborQuery = From nbr In allNbrs
                              Order By nbr.Distance

    allNbrs = sortedNeighborQuery.ToList()


    ' go sequentially through neighbors
    Dim equidistantNeighbors As New List(Of Neighbor)
    Dim firstWtIndex As Integer = 0
    Dim nextWtIndex As Integer = 0
    Do While firstWtIndex <= nbrWts.Count - 1
      ' clear list
      equidistantNeighbors.Clear()
      ' get all equidistant neighbors
      ' loop condition
      Dim keepGoing As Boolean = True
      Do While keepGoing
        ' add neighbor to list
        equidistantNeighbors.Add(allNbrs(nextWtIndex))
        nextWtIndex += 1
        ' check condition
        If nextWtIndex > allNbrs.Count - 1 Then
          keepGoing = False ' end of list condition
        ElseIf allNbrs(nextWtIndex).Distance <> allNbrs(firstWtIndex).Distance Then
          keepGoing = False
        End If
      Loop
      ' get sum of weights for next n neighbors
      ' where n is number of equidistant neighbors
      Dim lastWtIndex As Integer = nextWtIndex - 1
      If lastWtIndex > nbrWts.Count - 1 Then lastWtIndex = nbrWts.Count - 1
      Dim wtSum As Double = 0
      For tallyIndex = firstWtIndex To lastWtIndex
        wtSum += nbrWts(tallyIndex)
      Next
      ' divvy these up among the equidistant neighbors
      lastWtIndex = nextWtIndex - 1
      Dim wtPer As Double = wtSum / (lastWtIndex - firstWtIndex + 1)
      For i = firstWtIndex To lastWtIndex
        Dim REL As cCoLocationEngine.sNeighborRel
        REL.nbID = allNbrs(i).ID
        REL.nbWeight = wtPer
        R.Add(REL)
      Next
      ' then update first index
      firstWtIndex = nextWtIndex
    Loop
    ' return result
    Return R
  End Function
  Public ReadOnly Property simNNCT As cContingencyTable Implements cCoLocationEngine.iSimulator.simNNCT
    Get
      Return pNNCT
    End Get
  End Property

  Public ReadOnly Property simJointPopMetric As Double Implements cCoLocationEngine.iSimulator.simJointPopMetric
    Get
      Return pPopMet
    End Get
  End Property

  Public ReadOnly Property lastSimClasses As Integer() Implements cCoLocationEngine.iSimulator.lastSimClasses
    Get
      ' classes don't change, locations do
      Return ObsCatID
    End Get
  End Property

  Public ReadOnly Property lastSimPts As twoDTree.sPoint() Implements cCoLocationEngine.iSimulator.lastSimPts
    Get
      Return simPt
    End Get
  End Property
End Class

#End Region
Public Class clqUtils

  Shared Function calcNCT(ByVal ptCatID() As Integer, _
                                      ByVal classNames() As String, _
                                      ByVal Neighbors() As List(Of cCoLocationEngine.sNeighborRel)) _
                                      As cContingencyTable
    ' sums up neighbor relations to produce a neighbor contingency table
    ' input arrays ptCat and nbRel should be of same rank
    ' ptCat() should contain category IDs, coded from 0 to numCats-1

    Dim R As New cContingencyTable(classNames)
    ' loop through points
    For ptID = 0 To ptCatID.Length - 1
      ' note category of current point
      Dim fromCat As Integer = ptCatID(ptID)
      ' get list of neighbor relations
      Dim nbRelList As List(Of cCoLocationEngine.sNeighborRel) = Neighbors(ptID)
      ' loop through neighbor relations
      For Each curNbRel In nbRelList
        ' get category of neighbor
        Dim toCat As Integer = ptCatID(curNbRel.nbID)
        ' add neighbor count or weight to contingency table
        R.Value(fromCat, toCat) = R.Value(fromCat, toCat) + curNbRel.nbWeight
      Next curNbRel
    Next ptID
    Return R
  End Function
  'Public Sub testCalcContingencyTable()
  '  ' INPUT:
  '  ' ID cat nbIDs nbWTs
  '  ' 0  0   1     1
  '  ' 1  0   2     1
  '  ' 2  1   3     1
  '  ' 3  2   4,1   0.5, 0.5
  '  ' 4  2   3     1

  '  ' RESULT:
  '  ' cat/  0   1   2
  '  ' 0     1   1   0
  '  ' 1     0   0   1
  '  ' 2     0.5 0   1.5

  '  ' set up global variables
  '  ReDim pNeighbors(4)

  '  ' set up data
  '  Dim ptCat() As Integer = {0, 0, 1, 2, 2}
  '  Dim X As sNeighborRel
  '  Dim xList As New List(Of sNeighborRel)
  '  X.nbID = 1
  '  X.nbWeight = 1
  '  xList.Add(X)
  '  pNeighbors(0) = xList
  '  xList = New List(Of sNeighborRel)
  '  X.nbID = 2
  '  X.nbWeight = 1
  '  xList.Add(X)
  '  pNeighbors(1) = xList
  '  xList = New List(Of sNeighborRel)
  '  X.nbID = 3
  '  X.nbWeight = 1
  '  xList.Add(X)
  '  pNeighbors(2) = xList
  '  xList = New List(Of sNeighborRel)
  '  X.nbID = 4
  '  X.nbWeight = 0.5
  '  xList.Add(X)
  '  X.nbID = 1
  '  X.nbWeight = 0.5
  '  xList.Add(X)
  '  pNeighbors(3) = xList
  '  xList = New List(Of sNeighborRel)
  '  X.nbID = 3
  '  X.nbWeight = 1
  '  xList.Add(X)
  '  pNeighbors(4) = xList
  '  ' get contingency table
  '  Dim CC As cContingencyTable
  '  CC = calcContingencyTable(ptCat, {"A", "B", "C"}, pNeighbors)
  '  ' report results
  '  Dim msg As String
  '  For i = 0 To CC.numClasses - 1
  '    msg = ""
  '    For j = 0 To CC.numClasses - 1
  '      msg &= CC.Value(i, j).ToString & vbTab
  '    Next j
  '    Debug.Print(msg)
  '  Next i
  'End Sub
  ' Aggregate Categories
  Shared Function aggregateClasses(ByVal obsCatVal() As Object, ByVal classDictionary As Dictionary(Of Object, Object)) As Object()
    ' returns array of category values given category aggregation defined by dictionary
    Dim R As Object()
    ' error checking
    If obsCatVal.Length = 0 Then Return Nothing
    ' Dim missingDictionaryEntry As Boolean = False
    ReDim R(obsCatVal.Length - 1)
    ' loop
    For i = 0 To obsCatVal.Length - 1
      classDictionary.TryGetValue(obsCatVal(i), R(i)) ' should capture the boolean result and return error if there is a missing dictionary entry
    Next
    ' return result
    Return R
  End Function
  Shared Sub testExpNNCount()
    ' four classes

    ' set up NNCT
    Dim obsNNCT As New cContingencyTable({"A", "B", "C", "D"})
    obsNNCT.Value(0, 0) = 2
    obsNNCT.Value(0, 1) = 0
    obsNNCT.Value(0, 2) = 2
    obsNNCT.Value(0, 3) = 5
    obsNNCT.Value(1, 0) = 3
    obsNNCT.Value(1, 1) = 0
    obsNNCT.Value(1, 2) = 3
    obsNNCT.Value(1, 3) = 3
    obsNNCT.Value(2, 0) = 1
    obsNNCT.Value(2, 1) = 4
    obsNNCT.Value(2, 2) = 2
    obsNNCT.Value(2, 3) = 2
    obsNNCT.Value(3, 0) = 6
    obsNNCT.Value(3, 1) = 1
    obsNNCT.Value(3, 2) = 1
    obsNNCT.Value(3, 3) = 1
    ' get total counts
    Dim N() As Integer
    ReDim N(3)
    For i = 0 To 3
      N(i) = 0
      For j = 0 To 3
        N(i) += obsNNCT.Value(i, j)
      Next
    Next
    ' designate fixed cats
    Dim catFixed(3) As Boolean
    catFixed(0) = False
    catFixed(1) = True
    catFixed(2) = True
    catFixed(3) = False
    ' designate baseCat, neighborCat and fixed Cats
    Dim baseCat As Integer = 0
    Dim neighborCat As Integer = 3
    ' get expected counts
    Dim expNNCT As New cContingencyTable({"A", "B", "C", "D"})
    For i = 0 To 3
      For j = 0 To 3
        expNNCT.Value(i, j) = expectedNNCount(i, j, obsNNCT, catFixed)
      Next
    Next
  End Sub
  Shared Function expectedNNCount(ByVal baseCat As Integer, _
                                  ByVal neighborCat As Integer, _
                                  ByVal obsNNCT As cContingencyTable, _
                                  ByVal catFixed() As Boolean) As Double
    ' calculates the expected count of neighborCat individuals in the set of 
    ' baseCat individuals' neighbors
    ' given the constraint that all individuals where catFixed is True are immovable
    ' based on equations in Kronenfeld & Leslie (in progress, April 2012)
    ' checked for accuracy 4/29/2012

    ' GET CATEGORY COUNTS
    Dim numCats As Integer = obsNNCT.numClasses
    Dim catCounts() As Integer
    ReDim catCounts(numCats - 1)
    For i = 0 To numCats - 1
      catCounts(i) = 0
      For j = 0 To numCats - 1
        catCounts(i) += obsNNCT.Value(i, j)
      Next
    Next

    ' consistency checks
    If baseCat < 0 Then Return -1
    If baseCat > numCats Then Return -1
    If neighborCat < 0 Then Return -1
    If neighborCat > numCats Then Return -1
    If catCounts Is Nothing Then Return -1
    If UBound(catCounts) <> numCats - 1 Then Return -1
    If catFixed Is Nothing Then Return -1
    If UBound(catFixed) <> numCats - 1 Then Return -1

    ' DETERMINE CONSTRAINED CLASSES
    Dim baseConstrained, nbConstrained As Boolean
    baseConstrained = catFixed(baseCat)
    nbConstrained = catFixed(neighborCat)
    ' FIRST CASE
    ' if both classes are fixed, return the observed value (eq. 4)
    If baseConstrained And nbConstrained Then
      Return obsNNCT.Value(baseCat, neighborCat)
    End If
    ' GET COMMON VALUES
    ' total number of individuals
    Dim N As Integer = 0
    For i = 0 To numCats - 1
      N += catCounts(i)
    Next
    Dim NBase As Long = catCounts(baseCat)
    Dim NNeighbor As Long = catCounts(neighborCat)
    If baseCat = neighborCat Then NNeighbor -= 1 ' subtract 1 if it is the same as baseCat
    Dim NFree As Long = 0 ' number of free (unconstrained) individuals
    Dim C_Base_Free As Long = 0 ' count of base class individuals with neighbors in a constrained class
    Dim C_Free_Nb As Long = 0 ' count of individuals in a constrained class whose nearest neighbor is the Neighbor class
    For i = 0 To numCats - 1
      If Not catFixed(i) Then
        NFree += catCounts(i)
        C_Base_Free += obsNNCT.Value(baseCat, i)
        C_Free_Nb += obsNNCT.Value(i, neighborCat)
      End If
    Next
    Dim CFree_Free As Long = 0 ' number of free individuals with free individuals as their nearest neighbor
    For i = 0 To numCats - 1
      For j = 0 To numCats - 1
        If Not catFixed(i) And Not catFixed(j) Then CFree_Free += obsNNCT.Value(i, j)
      Next
    Next
    ' SORT THROUGH CASES OF FIXED CLASSES
    If Not baseConstrained And Not nbConstrained Then
      Return (NBase * CFree_Free / NFree) * NNeighbor / (NFree - 1)
    End If
    If baseConstrained And Not nbConstrained Then
      Return C_Base_Free * NNeighbor / NFree
    End If
    If Not baseConstrained And nbConstrained Then
      Return C_Free_Nb * NBase / (NFree)
    End If
    ' shouldn't happen, but just in case nothing has been returned yet
    Return -1

  End Function
  Shared Function getCLQ_OLD(ByVal fromNNCT As cContingencyTable) As cContingencyTable
    ' calculates the co-location quotient from the nearest neighbor contingency table
    ' using the formula in Leslie & Kronenfeld (2011)
    Dim nC As Integer = fromNNCT.numClasses ' number of classes
    Dim R As New cContingencyTable(fromNNCT.classNames) ' result
    ' get counts
    Dim N, C() As Double
    N = 0
    ReDim C(nC)
    For i = 0 To nC - 1
      C(i) = 0
      For j = 0 To nC - 1
        C(i) += fromNNCT.Value(i, j)
        N += fromNNCT.Value(i, j)
      Next j
    Next i
    ' calculate CLQs
    Dim NprimeB, numerator, denominator As Double
    For i = 0 To nC - 1
      For j = 0 To nC - 1
        ' denominator count is minus 1 for A = B
        If i = j Then
          NprimeB = C(j) - 1
        Else
          NprimeB = C(j)
        End If
        denominator = NprimeB / (N - 1)
        If denominator = 0 Then
          R.Value(i, j) = -1
        Else
          numerator = fromNNCT.Value(i, j) / C(i)
          R.Value(i, j) = numerator / denominator
        End If
      Next j
    Next i
    Return R
  End Function
  Shared Function ChiSqStat(ByVal obsNNCT As cContingencyTable, _
                            ByVal expNNCT As cContingencyTable, _
                            Optional ByVal includeDiagonals As Boolean = True) _
                            As Double
    ' returns the chi-squared statistic, as the sum of (Obs-exp)^2/exp

    ' error checking
    If obsNNCT.numClasses <> expNNCT.numClasses Then Return -1
    Dim R As Double = 0
    Dim O, E As Double
    ' loop through pairs of classes
    For A = 0 To obsNNCT.numClasses - 1
      For B = 0 To obsNNCT.numClasses - 1
        ' watch for diagonals
        Dim includeThis As Boolean = True
        If A = B Then includeThis = includeDiagonals
        If includeThis Then
          ' calculate (obs-exp)^2/exp
          O = obsNNCT.Value(A, B)
          E = expNNCT.Value(A, B)
          R += ((O - E) ^ 2) / E
        End If
      Next
    Next
    ' return result
    Return R
  End Function
  Shared Function compareNNCTs(ByVal clq1 As cCoLocationEngine, ByVal clq2 As cCoLocationEngine) As cContingencyTable
    ' performs a T-test on two sets of simulations
    ' and returns an array of two-tailed significance values
    ' inputs must have the same number of classes

    ' test inputs
    Dim numClasses As Integer = clq1.obsNNCT.numClasses
    If numClasses <> clq2.obsNNCT.numClasses Then Return Nothing
    ' get sim counts
    Dim N1 As Integer = clq1.numSimsCompleted
    Dim N2 As Integer = clq2.numSimsCompleted
    ' set up statistical calculator, result table
    Dim Stat As New StatisticsCalculator
    Stat.Open()
    Dim R As New cContingencyTable(clq1.obsNNCT.classNames)
    ' loop through rows and columns
    For row = 0 To numClasses - 1
      For col = 0 To numClasses - 1
        ' get arrays from each clq
        Dim V1() As Double = clq1.nnctArray(row, col)
        Dim v2() As Double = clq2.nnctArray(row, col)
        ' calculate statistic
        Dim t As Double = StatisticsCalculator.tStat_separate(V1, v2)
        Dim tSig As Double = Stat.Tsig_separate_twoTail(t, V1.Length, v2.Length)
        ' record in result
        R.Value(row, col) = tSig
      Next col
    Next row
    ' close statistical calculator
    Stat.Close()
    ' return result
    Return R
  End Function
  Shared Function getSigCount(ByVal ctList As List(Of cContingencyTable), _
                              Optional ByVal sigP As Double = 0.05) As cContingencyTable
    ' From input of simulated p-values,
    ' Returns number that are equal or less than significance value
    Dim repCT As cContingencyTable = ctList(0)
    Dim R As New cContingencyTable(repCT.classNames)
    Dim numClasses As Integer = repCT.numClasses
    ' loop through everything
    For Each CT In ctList
      For row = 0 To numClasses - 1
        For col = 0 To numClasses - 1
          If CT.Value(row, col) <= sigP Then R.Value(row, col) = R.Value(row, col) + 1
        Next
      Next
    Next
    Return R
  End Function
  Shared Function getCTAvgStDev(ByVal ctList As List(Of cContingencyTable)) _
    As List(Of cContingencyTable)
    ' calculates the average and st. dev. of each cell in contingency table
    ' returns a list of two contingency tables (first is avg, 2nd is stdev)
    Dim R As New List(Of cContingencyTable)
    Dim repCT As cContingencyTable = ctList.Item(0)
    Dim ctAvg As New cContingencyTable(repCT.classNames)
    Dim ctStDev As New cContingencyTable(repCT.classNames)
    ' get number in list
    Dim listCount As Integer = ctList.Count
    ' loop once to get sums
    For Each CT In ctList
      For row = 0 To CT.numClasses - 1
        For col = 0 To CT.numClasses - 1
          ctAvg.Value(row, col) = ctAvg.Value(row, col) + CT.Value(row, col)
        Next
      Next
    Next
    ' divide to get averages
    For row = 0 To ctAvg.numClasses - 1
      For col = 0 To ctAvg.numClasses - 1
        ctAvg.Value(row, col) = ctAvg.Value(row, col) / listCount
      Next col
    Next row
    ' loop again to get sum sq diff from mean
    For Each CT In ctList
      For row = 0 To CT.numClasses - 1
        For col = 0 To CT.numClasses - 1
          ctStDev.Value(row, col) = ctStDev.Value(row, col) + (CT.Value(row, col) - ctAvg.Value(row, col)) ^ 2
        Next col
      Next row
    Next CT
    ' divide for st dev
    For row = 0 To ctStDev.numClasses - 1
      For col = 0 To ctStDev.numClasses - 1
        ctStDev.Value(row, col) = ctStDev.Value(row, col) / listCount
        ctStDev.Value(row, col) = ctStDev.Value(row, col) ^ 0.5
      Next col
    Next row
    ' put in result and return to sender
    R.Add(ctAvg)
    R.Add(ctStDev)
    Return R
  End Function
  Shared Function SimToFeatureSet(ByVal simmer As cCoLocationEngine.iSimulator) As DotSpatial.Data.FeatureSet
    ' get points, marks
    Dim pts() As twoDTree.sPoint = simmer.lastSimPts
    Dim classes() As Integer = simmer.lastSimClasses
    ' create shapefile
    Dim SF As New DotSpatial.Data.FeatureSet(DotSpatial.Topology.FeatureType.Point)
    ' create column for marks
    Dim intVar As Double = 1
    Dim intType As Type = intVar.GetType
    SF.DataTable.Columns.Add("Mark", intVar.GetType)
    ' create features
    Dim F As DotSpatial.Data.Feature
    Dim C As DotSpatial.Topology.Coordinate
    For i = 0 To pts.Length - 1
      C = New DotSpatial.Topology.Coordinate(pts(i).x, pts(i).y)
      F = New DotSpatial.Data.Feature(C)
      SF.AddFeature(F)
      SF.DataTable.Rows(i).Item("Mark") = classes(i)
    Next
    ' return resut
    Return SF
  End Function
End Class
