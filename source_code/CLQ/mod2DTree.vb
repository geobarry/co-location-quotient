' Imports BKUtils
Public Class twoDTree
  ' timing
  Public totalWatch As Stopwatch
  Public totalTime As TimeSpan
  Public cp1Time As TimeSpan
  Public cp2Time As TimeSpan
  Public cp3Time As TimeSpan
  Public timeToSatisfy As TimeSpan
  Public timeNeedToMoveUp As TimeSpan
  Public timeNeedToMoveDown As TimeSpan
  Public timeToAddNeighbor As TimeSpan
  Public timeToTrimNb As TimeSpan
  Public watchTrimNb As Stopwatch
  Public shareWatch As Stopwatch
  Public watchSatisfy As Stopwatch
  Public watchTimUp, watchTimeDown As Stopwatch
  Public watchAddNb As Stopwatch
  Public watchPreliminaries As Stopwatch
  Public timePreliminaries As TimeSpan
  Public watchSort As Stopwatch
  Public timeSort As TimeSpan
  Public numNodesChecked As New List(Of Integer)
  Public curQuery As Integer = -1
#Region "Notes"
  ' notes
  ' all nodes in tree are also stored in an array list
  ' the TreeIndex of each node is not static, and changes
  ' any time a node is removed from the array list
#End Region
#Region "Structures, Constants and Enums"
  Public Enum eDimension
    x = 0
    y = 1
  End Enum
  Public Enum eSlot
    left = -1
    middle = 0
    right = 1
    indeterminate = 999
  End Enum
  Public Structure sNodeInfo
    Public X As Double
    Public Y As Double
    Public RightChild As Int32
    Public LeftChild As Int32
    Public MiddleChild As Int32
    Public Parent As Int32
    Public Slot As eSlot
    Public Dimension As eDimension
    Public BoxAroundDescendants As sBox
    Public TreeIndex As Int32
    Public UserIndex As Int32
    Public ShapeIndex As Int32
    Public PointIndex As Int32
    Public tag1 As Boolean
    Public tag2 As Boolean
  End Structure
  ' geometry structures
  Public Structure sPoint
    Public x As Double
    Public y As Double
  End Structure
  Public Structure sBox
    Public Top As Double
    Public Bottom As Double
    Public Left As Double
    Public Right As Double
  End Structure
  Public Structure sCircle
    Public Center As sPoint
    Public radius As Double
  End Structure
#End Region
#Region "Classes"
  ' geometry region classes
  Public Interface IRegion
    Function containedInBox(ByVal Box As sBox) As Boolean
    Function overlapsBox(ByVal box As sBox) As Boolean
    Function containsPoint(ByVal Pt As sPoint) As Boolean
    Function boundingBox() As sBox
  End Interface
  Public Class cPoint
    Public myPoint As sPoint
    Public Sub New(ByVal newX As Double, ByVal newY As Double)
      myPoint.x = newX
      myPoint.y = newY
    End Sub
    Public ReadOnly Property X As Double
      Get
        Return myPoint.x
      End Get
    End Property
    Public ReadOnly Property Y As Double
      Get
        Return myPoint.y
      End Get
    End Property
  End Class
  Public Class cRectangle
    Implements IRegion

    Public myBox As sBox
    Public Sub New(ByVal Left As Double, ByVal Right As Double, _
                   ByVal Bottom As Double, ByVal Top As Double)
      myBox.Left = Left
      myBox.Right = Right
      myBox.Bottom = Bottom
      myBox.Top = Top
    End Sub
    Public Sub New(ByVal newBox As sBox)
      myBox = newBox
    End Sub
    Public Function containedInBox(ByVal Box As sBox) As Boolean Implements IRegion.containedInBox
      Return twoDTree.BoxContainsBox(Box, myBox)
    End Function
    Public Function overlapsBox(ByVal box As sBox) As Boolean Implements IRegion.overlapsBox
      Return BoxesOverlap(box, myBox)
    End Function
    Public Function containsPoint(ByVal Pt As sPoint) As Boolean Implements IRegion.containsPoint
      Return pointInBox(Pt, myBox)
    End Function

    Public Function boundingBox() As sBox Implements IRegion.boundingBox
      Return myBox
    End Function
  End Class
  Public Class cCircle
    Implements IRegion

    Public myCircle As sCircle
    Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Rad As Double)
      myCircle.Center = getPoint(X, Y)
      myCircle.radius = Rad
    End Sub
    Public Sub New(ByVal centerPoint As cPoint, ByVal Rad As Double)
      myCircle.Center = centerPoint.myPoint
      myCircle.radius = Rad
    End Sub
    Public Function containedInBox(ByVal Box As sBox) As Boolean Implements IRegion.containedInBox
      Return BoxContainsCircle(Box, myCircle)
    End Function
    Public Function overlapsBox(ByVal box As sBox) As Boolean Implements IRegion.overlapsBox
      Return BoxOverlapsCircle(box, myCircle)
    End Function
    Public Function containsPoint(ByVal Pt As sPoint) As Boolean Implements IRegion.containsPoint
      Return pointInCircle(Pt, myCircle)
    End Function

    Public Function boundingBox() As sBox Implements IRegion.boundingBox
      Dim R As sBox
      Dim O As sPoint = myCircle.Center
      Dim D As Double = myCircle.radius
      R.Left = O.x - D
      R.Right = O.x + D
      R.Bottom = O.y - D
      R.Top = O.y + D
      Return R
    End Function
  End Class
  ' query parameters
  Public Class sQueryParam
    ' If we have a compulsory region but no search limit:
    '    - Include all points in compulsory region
    '    - Include more points if targetCount is higher

    ' If we have a search limit but no compulsory region:
    '    - Include up to targetCount
    '    - Only include points in search limit region

    ' If we have a search limit AND a compulsory region:
    '    - Include all points in compulsory region
    '    - include more points if targetCount is higher
    '    ' Only include points in search limit region

    ' (1) Compulsory region trumps count limits and search limit
    ' (2) Search limit trumps count limits
    ' (3) If counts are specified, then Center MUST be specified
    '     (For example, if minCount is 3, this means the nearest 3
    '      points will be returned.
    '      But this only makes sense if there is a Center point to
    '      measure distance from.
    Public Center As cPoint
    Public compulsoryRegion As IRegion
    Public searchLimit As IRegion
    Public targetCount As Integer
    Public Sub New(Optional ByVal centerPt As cPoint = Nothing, _
                   Optional ByVal target_Count As Integer = -1, _
                   Optional ByVal newCompulsoryRegion As IRegion = Nothing, _
                   Optional ByVal newSearchLimit As IRegion = Nothing)
      targetCount = target_Count
      Center = centerPt
      compulsoryRegion = newCompulsoryRegion
      searchLimit = newSearchLimit
    End Sub
  End Class
#End Region
#Region "Class Variables"
  Public NodeList As New List(Of sNodeInfo)
  Public RootID As Integer = -1
#End Region
#Region "Adding Points"
  Public Function addPoint(ByVal X As Double, ByVal Y As Double, _
                          Optional ByVal userIndex As Int32 = -1, _
                          Optional ByVal shpIndex As Int32 = -1, _
                          Optional ByVal ptIndex As Int32 = -1, _
                          Optional ByVal tag1 As Boolean = -1, _
                          Optional ByVal tag2 As Boolean = -1) _
                          As Integer
    ' places a new node into the tree at the correct point
    ' returns the ID of the new node
    Dim slotParentID As Integer = -1
    Dim Slot As eSlot
    Dim newNodeID As Integer
    ' set up node for tree
    Dim newNode As sNodeInfo
    newNode.X = X
    newNode.Y = Y
    newNode.UserIndex = userIndex
    newNode.ShapeIndex = shpIndex
    newNode.PointIndex = ptIndex
    newNode.tag1 = tag1
    newNode.tag2 = tag2
    newNode.LeftChild = -1
    newNode.RightChild = -1
    newNode.MiddleChild = -1
    newNode.Parent = -1
    ' find slot for point
    findSlot(X, Y, slotParentID, Slot)
    newNode.TreeIndex = NodeList.Count
    ' get place in list
    newNodeID = NodeList.Count
    ' place node into slot
    If slotParentID = -1 Then
      RootID = 0
      newNode.BoxAroundDescendants.Left = Double.NegativeInfinity
      newNode.BoxAroundDescendants.Right = Double.PositiveInfinity
      newNode.BoxAroundDescendants.Bottom = Double.NegativeInfinity
      newNode.BoxAroundDescendants.Top = Double.PositiveInfinity
      newNode.Slot = eSlot.indeterminate
    End If
    ' add node to list
    NodeList.Add(newNode)
    ' establish links
    If slotParentID <> -1 Then
      addChildInSlot(slotParentID, newNodeID, Slot)
    End If

    ' return new node ID
    Return newNodeID
  End Function
  Private Sub getFurthestNodeInList(ByVal resultList As List(Of Neighbor), _
                                    ByRef nodeInf As sNodeInfo, _
                                    ByRef d As Double)
    Dim lastIndex As Integer = resultList.Count - 1
    Dim nodeID As Integer = resultList.Item(lastIndex).ID
    nodeInf = NodeList(nodeID)
    ' sort index first
    '  resultList.Sort(AddressOf Neighbor.compareNeighbors)
    d = resultList.Item(lastIndex).Distance
  End Sub
#End Region
#Region "Queries"
  ' This is the main function on which the others rely
  Public Function nearestNodeIDs(ByRef Q As sQueryParam) _
                                As List(Of Neighbor)
    ' timing
    totalWatch = Stopwatch.StartNew
    watchPreliminaries = Stopwatch.StartNew
    curQuery += 1
    numNodesChecked.Add(0)

    ' returns a sorted list of neighbor objects
    ' representing nodes in the tree whose points
    ' are nearest to the input point
    ' Notes:
    ' (1) Number of nodes in result will exceed maxCount if there are ties

    ' check validity
    ' must have either a compulsory region or a target count
    If Q.targetCount < 1 Then
      If Q.compulsoryRegion Is Nothing Then
        Return Nothing
      End If
    End If
    ' must have a center if there is a target count
    If Q.targetCount > 0 Then
      If Q.Center Is Nothing Then
        Return Nothing
      End If
    End If
    ' declare variables
    Dim R As New List(Of Neighbor)
    Dim Slot As eSlot
    Dim curNodeID, startNodeID As Integer
    Dim pt As sPoint = Q.Center.myPoint
    ' traverse to the parent of the leaf node where the new point would be placed
    findSlot(pt.x, pt.y, startNodeID, Slot)
    ' make sure there is a result
    If startNodeID < 0 Then
      Return R
    End If
    ' if node is middle node, find highest coincident node
    If NodeList(startNodeID).Slot = eSlot.middle Then
      startNodeID = oldestCoincidentParent(startNodeID)
    End If
    ' add to sorted list
    curNodeID = startNodeID
    ' timing
    watchPreliminaries.Stop()
    timePreliminaries = timePreliminaries.Add(watchPreliminaries.Elapsed)
    watchSatisfy = Stopwatch.StartNew

    Dim nodeSatisfies As Boolean = NodeSatisfiesQuery(curNodeID, R, Q)

    watchSatisfy.Stop()
    timeToSatisfy = timeToSatisfy.Add(watchSatisfy.Elapsed)

    If nodeSatisfies Then
      addNeighbor(R, curNodeID, Q)
    End If

    ' timing
    totalWatch.Stop()
    cp1Time = cp1Time.Add(totalWatch.Elapsed)
    totalWatch.Start()

    ' work down
    curNodeID = startNodeID
    If NodeList(curNodeID).LeftChild > -1 Then workDown(R, NodeList(curNodeID).LeftChild, Q)
    curNodeID = startNodeID
    If NodeList(curNodeID).RightChild > -1 Then workDown(R, NodeList(curNodeID).RightChild, Q)
    '    workDown(x, y, R, curNodeID, minCount)

    ' timing
    totalWatch.Stop()
    cp2Time = cp2Time.Add(totalWatch.Elapsed)
    totalWatch.Start()


    ' work up
    curNodeID = startNodeID
    workUp(R, curNodeID, Q)
    ' sort list

    ' timing
    totalWatch.Stop()
    cp3Time = cp3Time.Add(totalWatch.Elapsed)
    totalWatch.Start()


    ' timing
    watchSort = Stopwatch.StartNew

    R.Sort(AddressOf Neighbor.compareNeighbors)

    watchSort.Stop()
    timeSort = timeSort.Add(watchSort.Elapsed)

    ' timing
    totalTime = totalTime.Add(totalWatch.Elapsed)
    totalWatch.Stop()
    ' return result
    Return R
  End Function
  Public Function nearestNodeID(ByVal P As sPoint) As Integer
    Dim neighborList As List(Of Neighbor)
    Dim Q As New sQueryParam(New cPoint(P.x, P.y), 1)
    neighborList = nearestNodeIDs(Q)
    If neighborList.Count > 0 Then
      Return neighborList.Item(0).ID
    Else
      Return -1
    End If
  End Function
  Public Function nearestNeighborList(ByVal ptID As Integer, _
                                      Optional ByVal numNeighbors As Integer = 1,
                                      Optional ByVal pt As Feedback.ProgressTracker = Nothing) _
                                      As List(Of Neighbor)
    Dim nodeInf As sNodeInfo = nodeInformation(ptID)
    Dim P As New cPoint(nodeInf.X, nodeInf.Y)
    Dim Q As New sQueryParam(P, numNeighbors + 1)
    Dim neighborList As List(Of Neighbor) = nearestNodeIDs(Q)
    Dim removeIndex As Integer = indexOf(neighborList, ptID)
    neighborList.RemoveAt(removeIndex)
    Return neighborList
  End Function
  Public Function nearestNeighborListList(Optional ByVal numNeighbors As Integer = 1, _
                                          Optional ByVal PT As Feedback.ProgressTracker = Nothing) _
                                      As List(Of List(Of Neighbor))
    ' returns a list of nearest neighbor IDs for each node 
    Dim R As New List(Of List(Of Neighbor))
    Dim i As Integer
    Dim nodeInf As sNodeInfo
    Dim neighborList As List(Of Neighbor)
    ' report start
    If Not PT Is Nothing Then
      PT.initializeTask("Finding nearest neighbors...")
      PT.setTotal(NodeList.Count)
    End If
    ' loop through nodes
    For i = 0 To NodeList.Count - 1
      ' get node
      nodeInf = nodeInformation(i)
      ' get neighbor list
      Dim P As New cPoint(nodeInf.X, nodeInf.Y)
      Dim Q As New sQueryParam(P, numNeighbors + 1)
      neighborList = nearestNodeIDs(Q)
      ' remove original node
      Dim removeIndex As Integer
      removeIndex = indexOf(neighborList, i)
      neighborList.RemoveAt(removeIndex)
      R.Add(neighborList)
      ' report progress
      If Not PT Is Nothing Then
        If (i + 1) Mod 1000 = 0 Then
          PT.setCompleted(i + 1)
        End If
      End If
    Next
    ' report finish
    If Not PT Is Nothing Then
      PT.finishTask()
    End If
    ' return result
    Return R
  End Function
#End Region
#Region "Query Base Functions"
  Private Function NodeSatisfiesQuery(ByRef nodeToCheck As Integer, _
                                      ByRef resultList As List(Of Neighbor), _
                                      ByRef Q As sQueryParam) _
                                    As Boolean



    ' checks if the input node should be added to the result list
    ' does not add the point...
    ' This should be done with:
    '  >>>>    addNeighbor(resultList, nodeToCheck, d, Q)
    If nodeToCheck = -1 Then Return False

    ' timing
    numNodesChecked(curQuery) += 1
    If nodeToCheck = 0 Then
      SpatialIndexTestWindow.lblRootChecked.Text = "Root checked"
    End If

    ' common variables
    Dim nodePt As sPoint = nodePoint(nodeToCheck)
    Dim d As Double = Double.PositiveInfinity
    If Not Q.Center Is Nothing Then d = Distance(nodeToCheck, Q.Center)
    ' COMPULSORY REGION
    If Not Q.compulsoryRegion Is Nothing Then
      If Q.compulsoryRegion.containsPoint(nodePt) Then Return True
    End If
    ' SEARCH LIMIT
    If Not Q.searchLimit Is Nothing Then
      If Not Q.searchLimit.containsPoint(nodePt) Then Return False
    End If
    ' TARGET COUNT
    ' if target count has not been met, definitely add
    If resultList.Count < Q.targetCount Then Return True
    ' otherwise, add only if this node is closer than furthest node
    ' if so, get furthest distance among results list
    Dim furthestD As Double
    If resultList.Count = 0 Then Return True
    furthestD = resultList.Last.Distance
    ' if I'm as close or closer, add me in!
    If d <= furthestD Then Return True
    ' Otherwise, we're out!    
    Return False
  End Function
  Private Function NeedToMoveUp(ByRef curNodeID As Integer, _
                                ByRef resultList As List(Of Neighbor), _
                                ByRef Q As sQueryParam) As Boolean
    ' Don't need to move up if we're sure all results are 
    ' descendants of the current node

    ' If the input node is null (ID = -1) or root we can't move up
    If curNodeID = -1 Then Return False
    Dim curNode As sNodeInfo
    If curNode.Parent = -1 Then Return False
    ' If the current node is a middle child, then we have to move up
    If NodeList(curNodeID).Slot = eSlot.middle Then Return True

    ' REGULAR CASES
    ' Get variables
    curNode = NodeList(curNodeID)
    Dim ControlArea As sBox = curNode.BoxAroundDescendants     ' box within which all nodes must be current node's descendents
    ' don't need to move up if current node has no parents

    ' COMPULSORY REGION
    ' If we don't control this, we have to move up
    If Not Q.compulsoryRegion Is Nothing Then
      If Not Q.compulsoryRegion.containedInBox(ControlArea) Then Return True
    End If

    ' SEARCH LIMIT
    ' If we do control this, there is no need to move up
    If Not Q.searchLimit Is Nothing Then
      If Q.searchLimit.containedInBox(ControlArea) Then Return False
    End If

    ' COUNTS
    ' only do this if we have a center
    If Q.Center Is Nothing Or Q.targetCount <= 0 Then
      ' compulsory region is the only criterion
      Return False
    Else
      ' If we haven't reached the minimum count, 
      ' then we have to move up
      If resultList.Count < Q.targetCount Then Return True
      ' If we don't control all locations closer than the furthest result so far,
      ' then we have to move up
      Dim furthestNode As sNodeInfo
      Dim furthestNodeDistance As Double
      getFurthestNodeInList(resultList, furthestNode, furthestNodeDistance)
      ' get circle we need to control
      Dim needToControlCircle As New cCircle(Q.Center, furthestNodeDistance)
      ' get rectangle around circle
      Dim needToControlBox As sBox = boxAroundCircle(needToControlCircle.myCircle)
      ' clip box by search limit
      If Not Q.searchLimit Is Nothing Then
        needToControlBox = boxIntersection(Q.searchLimit.boundingBox, needToControlBox)
      End If
      ' see if we control what we need to
      If BoxContainsBox(ControlArea, needToControlBox) Then
        ' we control the futhest circle
        Return False
      Else
        ' we don't control the furthest circle
        Return True
      End If
    End If
  End Function
  Private Function NeedToMoveDown(ByRef curNodeID As Integer, _
                                  ByRef resultList As List(Of Neighbor), _
                                  ByRef Q As sQueryParam) As Boolean
    ' Don't need to move down if we're sure
    ' no query results can be descendants of the current node
    If curNodeID = -1 Then Return False
    ' Get variables
    Dim curNode As sNodeInfo, d As Double
    curNode = NodeList(curNodeID)
    Dim ControlArea As sBox = curNode.BoxAroundDescendants     ' box within which all nodes must be current node's descendents
    Dim furthestNode As sNodeInfo
    getFurthestNodeInList(resultList, furthestNode, d)
    ' COMPULSORY REGION
    ' Need to move down if control area overlaps with the compulsory region
    If Not Q.compulsoryRegion Is Nothing Then
      If Q.compulsoryRegion.overlapsBox(ControlArea) Then Return True
    End If
    ' SEARCH LIMIT
    ' Don't need to move down if control area does not overlap the search limit
    If Not Q.searchLimit Is Nothing Then
      If Not Q.searchLimit.overlapsBox(ControlArea) Then Return False
    End If
    ' Min & Max counts are only applicable if a center point is defined
    If Q.Center Is Nothing Then
      ' Counts don't matter; don't need to move down because there is no
      ' overlap between control area and compulsory region
      Return False
    Else
      ' If we have no compulsory region and still need more results, must move down
      If Q.compulsoryRegion Is Nothing And resultList.Count < Q.targetCount Then Return True
      ' otherwise, we need to move down only if we overlap 
      ' maxCircle through furthest result so far
      Dim maxD As Double = Distance(furthestNode, Q.Center.X, Q.Center.Y)
      Dim maxCircle As New cCircle(Q.Center, maxD)
      If BoxOverlapsCircle(ControlArea, maxCircle.myCircle) Then
        Return True
      Else
        Return False
      End If
    End If
  End Function
  Private Sub workUp(ByRef resultList As List(Of Neighbor), _
                     ByRef curNodeID As Integer, _
                     ByRef Q As sQueryParam)
    ' If necessary, 
    ' Moves to the Parent of the current node 
    ' Checks the Parent
    ' Works down the other side
    ' Works up again

    ' timing
    watchTimUp = Stopwatch.StartNew

    Dim needUp As Boolean = NeedToMoveUp(curNodeID, resultList, Q)

    watchTimUp.Stop()
    timeNeedToMoveUp = timeNeedToMoveUp.Add(watchTimUp.Elapsed)

    If needUp Then
      Dim otherChildSlot As eSlot
      Dim otherChildID As Integer
      ' move to parent, get sibling
      otherChildSlot = otherSlot(NodeList(curNodeID).Slot)
      curNodeID = NodeList(curNodeID).Parent ' move to parent
      otherChildID = ChildID(curNodeID, otherChildSlot)
      ' check the parent, add to result list
      ' check parent

      ' timing
      watchSatisfy = Stopwatch.StartNew

      Dim nodeSat As Boolean = NodeSatisfiesQuery(curNodeID, resultList, Q)

      watchSatisfy.Stop()
      timeToSatisfy = timeToSatisfy.Add(watchSatisfy.Elapsed)

      If nodeSat Then
        addNeighbor(resultList, curNodeID, Q)
      End If
      ' work down the other side
      workDown(resultList, otherChildID, Q)
      ' work up again
      workUp(resultList, curNodeID, Q)
    End If
  End Sub
  Private Sub workDown(ByRef resultList As List(Of Neighbor), _
                              ByRef curNodeID As Integer, _
                              ByRef Q As sQueryParam)

    ' checks the current node and all its descendants
    ' for any node that is closer to the target
    Try
      ' check the current node

      ' timing
      watchSatisfy = Stopwatch.StartNew

      Dim nodeSat As Boolean = NodeSatisfiesQuery(curNodeID, resultList, Q)

      watchSatisfy.Stop()
      timeToSatisfy = timeToSatisfy.Add(watchSatisfy.Elapsed)

      If nodeSat Then
        addNeighbor(resultList, curNodeID, Q)
      End If
      ' see if we need to work down again
      watchTimeDown = Stopwatch.StartNew

      Dim needDown As Boolean = NeedToMoveDown(curNodeID, resultList, Q)

      watchTimeDown.Stop()
      timeNeedToMoveDown = timeNeedToMoveDown.Add(watchTimeDown.Elapsed)

      If needDown Then
        ' work down each child
        workDown(resultList, ChildID(curNodeID, eSlot.left), Q)
        workDown(resultList, ChildID(curNodeID, eSlot.right), Q)
      End If
    Catch EX As Exception
      Debug.Print(EX.Message)
    End Try
  End Sub
  Private Sub addNeighbor(ByRef resultList As List(Of Neighbor), _
                            ByVal nodeID As Integer, _
                            ByRef Q As sQueryParam)
    ' timing
    watchAddNb = Stopwatch.StartNew

    ' adds neighbor to the list, including all coincident neighbors
    ' and then trims to size, if necessary
    Dim D As Double
    ' get distance
    If Q.Center Is Nothing Then
      D = 0
    Else
      D = Distance(nodeID, Q.Center)
    End If
    ' add neighbor
    resultList.Add(New Neighbor(nodeID, D))
    ' add all coincident descendants
    Do While NodeList(nodeID).MiddleChild <> -1
      nodeID = NodeList(nodeID).MiddleChild
      resultList.Add(New Neighbor(nodeID, D))
    Loop
    ' sort list
    resultList.Sort(AddressOf Neighbor.compareNeighbors)

    ' timing
    watchAddNb.Stop()
    timeToAddNeighbor = timeToAddNeighbor.Add(watchAddNb.Elapsed)

    ' check if need to trim
    If Q.targetCount < 1 Then Exit Sub
    If resultList.Count <= Q.targetCount Then Exit Sub
    Dim curEntry As Integer
    Dim lastValidDistance, curEntryDistance As Double
    ' initialize

    ' timing
    watchTrimNb = Stopwatch.StartNew

    curEntry = resultList.Count - 1 ' start at end of list
    curEntryDistance = resultList.Item(curEntry).Distance
    lastValidDistance = resultList.Item(Q.targetCount - 1).Distance
    Dim numCompulsory As Integer = 0
    ' loop until two distances are the same
    Do While curEntryDistance <> lastValidDistance
      If Q.compulsoryRegion Is Nothing Then
        resultList.RemoveAt(curEntry)
      Else
        Dim curPoint As sPoint = nodePoint(resultList.Item(curEntry).ID)
        If Q.compulsoryRegion.containsPoint(curPoint) Then
          ' update lastValidDistance
          numCompulsory += 1
          Dim lastValidEntry As Integer = Q.targetCount - 1 - numCompulsory
          If lastValidEntry < 0 Then
            lastValidDistance = -1
          Else
            lastValidDistance = resultList.Item(lastValidEntry).Distance
          End If
        Else
          resultList.RemoveAt(curEntry)
        End If
      End If
      curEntry -= 1
      If curEntry < 0 Then
        curEntryDistance = -1
      Else
        curEntryDistance = resultList.Item(curEntry).Distance
      End If
    Loop

    ' timing
    watchTrimNb.Stop()
    timeToTrimNb = timeToTrimNb.Add(watchTrimNb.Elapsed)

  End Sub
#End Region

#Region "Finding and managing open slots"
  Private Sub findSlot(ByVal x As Double, ByVal y As Double, _
                      ByRef ParentID As Integer, _
                      Optional ByRef Slot As eSlot = eSlot.indeterminate)
    ' determines where to put a new point into the tree
    ' and returns the parent node
    ' and the slot (left or right child) of the parent node
    ' in the two ByRef variables
    Dim curNodeInSlotID As Integer
    Dim ParentInfo As sNodeInfo
    ' initialize to root node
    curNodeInSlotID = RootID
    ParentID = -1
    ' loop until slot is available
    Do While Not (curNodeInSlotID = -1)
      ParentID = curNodeInSlotID
      ParentInfo = NodeList(ParentID)
      Slot = slotForChild(ParentInfo, x, y)
      curNodeInSlotID = ChildID(ParentInfo, Slot)
    Loop
  End Sub
  Public Sub addChildInSlot(ByVal ParentID As Integer, _
                            ByVal childID As Integer, _
                            ByVal Slot As eSlot)
    ' Adds the child into the given slot,
    ' updating the dimension, slot and bounding box of the child node
    ' and creating forward and backward links
    ' update dimension of childnode
    Dim parentInfo, childInfo As sNodeInfo
    parentInfo = NodeList(ParentID)
    childInfo = NodeList(childID)
    If parentInfo.Dimension = eDimension.x Then
      childInfo.Dimension = eDimension.y
    Else
      childInfo.Dimension = eDimension.x
    End If
    ' update slot of childnode
    childInfo.Slot = Slot
    ' set bounding box of child to bounding box of parent
    childInfo.BoxAroundDescendants = parentInfo.BoxAroundDescendants
    ' adjust bounding box
    If Slot = eSlot.middle Then ' middle slot
      childInfo.BoxAroundDescendants.Left = parentInfo.X
      childInfo.BoxAroundDescendants.Right = parentInfo.X
      childInfo.BoxAroundDescendants.Top = parentInfo.Y
      childInfo.BoxAroundDescendants.Bottom = parentInfo.Y
    Else ' not middle slot
      If parentInfo.Dimension = eDimension.x Then
        If Slot = eSlot.left Then
          childInfo.BoxAroundDescendants.Right = parentInfo.X
        ElseIf Slot = eSlot.right Then
          childInfo.BoxAroundDescendants.Left = parentInfo.X
        End If
      Else
        If Slot = eSlot.left Then
          childInfo.BoxAroundDescendants.Top = parentInfo.Y
        ElseIf Slot = eSlot.right Then
          childInfo.BoxAroundDescendants.Bottom = parentInfo.Y
        End If
      End If ' parentInfo.Dimension
    End If ' middle slot or not
    ' create links
    If Slot = eSlot.left Then
      parentInfo.LeftChild = childID
    ElseIf Slot = eSlot.right Then
      parentInfo.RightChild = childID
    ElseIf Slot = eSlot.middle Then
      parentInfo.MiddleChild = childID
    End If
    childInfo.Parent = ParentID
    ' put back in array
    NodeList(ParentID) = parentInfo
    NodeList(childID) = childInfo
  End Sub
  Public Function slotForChild(ByVal parentInfo As sNodeInfo, _
                               ByVal x As Double, ByVal y As Double) As eSlot
    ' finds the correct slot for the child
    ' based on the dimension on which values are compared for the current node
    If parentInfo.Dimension Mod 2 = 0 Then
      Select Case x
        Case Is < parentInfo.X
          Return eSlot.left
        Case Is > parentInfo.X
          Return eSlot.right
        Case Is = parentInfo.X
          ' arbitrarily place in right slot 
          ' if value on this dimension is same
          ' but value on other dimensionn is not
          If y <> parentInfo.Y Then
            Return eSlot.right
          End If
          ' *** end debugging
          Return eSlot.middle
      End Select
    Else
      Select Case y
        Case Is < parentInfo.Y
          Return eSlot.left
        Case Is > parentInfo.Y
          Return eSlot.right
        Case Is = parentInfo.Y
          ' arbitrarily place in right slot 
          ' if value on this dimension is same
          ' but value on other dimensionn is not
          If x <> parentInfo.X Then
            Return eSlot.right
          End If
          ' otherwise, return middle slot
          Return eSlot.middle
      End Select
    End If
  End Function
  Public Overloads Function ChildID(ByVal parentInfo As sNodeInfo, ByVal slot As eSlot) As Integer
    ' returns the left or right child, as specified by the Slot variable
    If slot = eSlot.left Then Return parentInfo.LeftChild
    If slot = eSlot.right Then Return parentInfo.RightChild
    If slot = eSlot.middle Then Return parentInfo.MiddleChild
    ' handle case of indeterminate slot (pick any child)
    If slot = eSlot.indeterminate Then
      If parentInfo.LeftChild >= 0 Then
        Return parentInfo.LeftChild
      ElseIf parentInfo.RightChild >= 0 Then
        Return parentInfo.RightChild
      ElseIf parentInfo.MiddleChild >= 0 Then
        Return parentInfo.MiddleChild
      End If
    End If
    Return -1
  End Function
  Public Overloads Function ChildID(ByVal ParentID As Integer, ByVal Slot As eSlot) As Integer
    ' returns the left or right child, as specified by the Slot variable
    Dim ParentInfo As sNodeInfo
    ParentInfo = NodeList(ParentID)
    Return ChildID(ParentInfo, Slot)
  End Function
#End Region
#Region "Distance Functions"
  Public Overloads Shared Function Distance(ByVal X1 As Double, ByVal Y1 As Double, ByVal X2 As Double, ByVal Y2 As Double) As Double
    ' returns the Euclidean distance between two points
    Dim R As Double
    R = (X1 - X2) ^ 2
    R += (Y1 - Y2) ^ 2
    R = R ^ 0.5
    Return R
  End Function
  Public Overloads Function Distance(ByVal NodeID1 As Integer, ByVal NodeID2 As Integer) As Double
    Dim R As Double
    Dim N1, N2 As sNodeInfo
    N1 = NodeList.Item(NodeID1)
    N2 = NodeList.Item(NodeID2)
    R = ((N1.X - N2.X) ^ 2 + (N1.Y - N2.Y) ^ 2) ^ 0.5
    Return R
  End Function
  Public Overloads Function Distance(ByVal NodeID As Integer, ByVal X As Double, ByVal Y As Double) As Double
    Try
      Dim N As sNodeInfo
      N = NodeList.Item(NodeID)
      Dim R As Double
      R = ((N.X - X) ^ 2 + (N.Y - Y) ^ 2) ^ 0.5
      Return R
    Catch ex As Exception
      Debug.Print(ex.Message)
    End Try

  End Function
  Public Overloads Shared Function Distance(ByVal Node As sNodeInfo, ByVal X As Double, ByVal Y As Double) As Double
    Dim R As Double
    R = ((Node.X - X) ^ 2 + (Node.Y - Y) ^ 2) ^ 0.5
    Return R
  End Function
  Public Overloads Function distance(ByVal nodeID As Integer, ByVal Pt As cPoint) As Double
    Return distance(nodeID, Pt.X, Pt.Y)
  End Function
  Public Overloads Shared Function distance(ByVal pt1 As cPoint, ByVal pt2 As cPoint) As Double
    Return distance(pt1.X, pt1.Y, pt2.X, pt2.Y)
  End Function
  Public Overloads Function distance(ByVal Pt As cPoint, ByVal nodeID As Integer) As Double
    Return distance(nodeID, Pt.X, Pt.Y)
  End Function
#End Region
#Region "Geometry Checks"
  Public Shared Function BoxContainsBox(ByVal inBox As sBox, ByVal otherBox As sBox) As Boolean
    ' returns true of the input box completely contains the other box
    If otherBox.Right > inBox.Right Then Return False
    If otherBox.Left < inBox.Left Then Return False
    If otherBox.Top > inBox.Top Then Return False
    If otherBox.Bottom < inBox.Bottom Then Return False
    Return True
  End Function
  Public Shared Function CircleContainsBox(ByVal circle As sCircle, _
                                           ByVal box As sBox) As Boolean
    ' hmm... this is going to be difficult
    ' guess we have to check each point individually

    ' get box corners
    Dim boxCorners As New List(Of sPoint)
    boxCorners.Add(getPoint(box.Left, box.Bottom))
    boxCorners.Add(getPoint(box.Left, box.Top))
    boxCorners.Add(getPoint(box.Right, box.Top))
    boxCorners.Add(getPoint(box.Right, box.Bottom))
    ' check each corner
    For Each boxCorner In boxCorners
      ' if it's out of the circle, return false
      If Not pointInCircle(boxCorner, circle) Then Return False
    Next
    ' if we're still here, we're in!
    Return True
  End Function
  Public Shared Function CircleOverlapsBox(ByVal circle As sCircle, _
                                           ByVal box As sBox) As Boolean
    ' hmm... this is going to be difficult
    ' guess we have to check each point individually

    ' get box corners
    Dim boxCorners As New List(Of sPoint)
    boxCorners.Add(getPoint(box.Left, box.Bottom))
    boxCorners.Add(getPoint(box.Left, box.Top))
    boxCorners.Add(getPoint(box.Right, box.Top))
    boxCorners.Add(getPoint(box.Right, box.Bottom))
    ' check each corner
    For Each boxCorner In boxCorners
      ' if it's out of the circle, return false
      If pointInCircle(boxCorner, circle) Then Return False
    Next
    ' if we're still here, we're in!
    Return True
  End Function

  Public Shared Function BoxContainsCircle(ByVal inBox As sBox, _
                                  ByVal circle As sCircle) As Boolean
    ' Returns true if the entire circle is contained in the box
    If circle.Center.x + circle.radius >= inBox.Right Then Return False
    If circle.Center.x - circle.radius <= inBox.Left Then Return False
    If circle.Center.y + circle.radius >= inBox.Top Then Return False
    If circle.Center.y - circle.radius <= inBox.Bottom Then Return False
    Return True
  End Function
  Public Shared Function BoxesOverlap(ByVal Box1 As sBox, ByVal Box2 As sBox) As Boolean
    ' returns true if any part of the other box overlaps the input box
    ' (must overlap in both horizontal and vertical dimensions)

    ' check horizontal:
    If segmentsOverlap(Box1.Left, Box1.Right, Box2.Left, Box2.Right) Then
      ' check vertical
      If segmentsOverlap(Box1.Bottom, Box1.Top, Box2.Bottom, Box2.Top) Then
        ' if they both overlap:
        Return True
      End If
    End If
    ' otherwise
    Return False
  End Function
  Public Shared Function BoxOverlapsCircle(ByVal inBox As sBox, _
                              ByVal Circle As sCircle) As Boolean
    ' Returns true if any part of the circle overlaps the box
    If inBox.Left > Circle.Center.x + Circle.radius Then Return False
    If inBox.Right < Circle.Center.x - Circle.radius Then Return False
    If inBox.Top < Circle.Center.y - Circle.radius Then Return False
    If inBox.Bottom > Circle.Center.y + Circle.radius Then Return False
    Return True
  End Function
  Public Shared Function pointInCircle(ByVal pt As sPoint, ByVal Circle As sCircle) As Boolean
    ' returns true if point is inside the circle or on the boundary
    Dim D As Double = Distance(pt.x, pt.y, Circle.Center.x, Circle.Center.y)
    If D <= Circle.radius Then Return True Else Return False
  End Function
  Public Shared Function pointInBox(ByVal pt As sPoint, ByVal Box As sBox) As Boolean
    ' returns true if point is in box or on boundary
    Dim ptBox As sBox
    ptBox.Left = pt.x
    ptBox.Right = pt.x
    ptBox.Bottom = pt.y
    ptBox.Top = pt.y
    Return BoxContainsBox(Box, ptBox)
  End Function
  Public Shared Function segmentsOverlap(ByVal seg1start As Double, _
                                     ByVal seg1end As Double, _
                                     ByVal seg2start As Double, _
                                     ByVal seg2end As Double) As Boolean
    ' returns true if two segments overlap
    ' false otherwise (false if they just touch)
    ' input endpoints must be higher than startpoints
    Dim R As Boolean
    Select Case seg2end
      Case Is < seg1end
        If seg2end > seg1start Then Return True Else Return False
      Case Is = seg1end
        If seg2start = seg2end Or seg1start = seg1end Then
          Return False
        Else
          Return True
        End If
      Case Is > seg1end
        If seg1end > seg2start Then Return True Else Return False
    End Select
  End Function
  Public Function infiniteBox() As sBox
    ' creates a box with sides at infinity
    Dim outBox As sBox
    outBox.Top = Double.PositiveInfinity
    outBox.Bottom = Double.NegativeInfinity
    outBox.Left = Double.NegativeInfinity
    outBox.Right = Double.PositiveInfinity
    Return outBox
  End Function
#End Region
#Region "Geometric Structure Creation Utilities"
  Shared Function getPoint(ByVal x As Double, ByVal y As Double) As sPoint
    Dim R As sPoint
    R.x = x
    R.y = y
    Return R
  End Function
  Shared Function getCircle(ByVal x As Double, ByVal y As Double, ByVal radius As Double) As sCircle
    Dim R As sCircle

    Return R
  End Function
  Shared Function getBox(ByVal left As Double, ByVal right As Double, ByVal bottom As Double, ByVal top As Double) As sBox
    Dim R As sBox
    R.Left = left
    R.Right = right
    R.Bottom = bottom
    R.Top = top
    Return R
  End Function
  Shared Function boxAroundCircle(ByVal Circle As sCircle) As sBox
    ' shortcuts
    Dim X As Double = Circle.Center.x, Y As Double = Circle.Center.y, R As Double = Circle.radius
    Dim RR As sBox
    RR.Left = X - R
    RR.Right = X + R
    RR.Bottom = Y - R
    RR.Top = Y + R
    Return RR
  End Function
  Shared Function boxIntersection(ByVal box1 As sBox, ByVal box2 As sBox) As sBox
    ' returns the intersection of the input boxes
    Dim R As sBox
    R.Left = Math.Max(box1.Left, box2.Left)
    R.Right = Math.Min(box1.Right, box2.Right)
    R.Bottom = Math.Max(box1.Bottom, box2.Bottom)
    R.Top = Math.Min(box1.Top, box2.Top)
    Return R
  End Function
#End Region
#Region "Index Utilities"
  Public Sub clear()
    RootID = -1
    NodeList.Clear()
  End Sub
  Public Function nodePoint(ByVal nodeID As Integer) As sPoint
    Dim nInf As sNodeInfo = nodeInformation(nodeID)
    Return getPoint(nInf.X, nInf.Y)
  End Function
  Public Function nodeInformation(ByVal nodeID As Integer) As sNodeInfo
    If nodeID = -1 Then
      Return Nothing
    Else
      Return NodeList.Item(nodeID)
    End If
  End Function
  Public Function treeDepth(Optional ByVal treeRoot As Integer = -1) As Integer
    If treeRoot = -1 Then treeRoot = RootID
    Dim curNodeInfo As sNodeInfo
    If treeRoot = -1 Then Return 0
    curNodeInfo = NodeList(treeRoot)
    Dim lDepth, rDepth, mDepth As Integer
    Dim L, R, M As Integer
    L = curNodeInfo.LeftChild
    R = curNodeInfo.RightChild
    M = curNodeInfo.MiddleChild
    If L = -1 Then lDepth = 0 Else lDepth = treeDepth(L)
    If R = -1 Then rDepth = 0 Else rDepth = treeDepth(R)
    If M = -1 Then mDepth = 0 Else mDepth = treeDepth(M)
    Return 1 + Data.Numbers.maxVal(lDepth, rDepth, mDepth)
  End Function
  Public Function numCoincident(ByVal ptID As Integer, _
                                Optional ByVal includeInput As Boolean = False) As Integer
    ' returns number of points coincident with the input point
    Dim R As Integer = 0
    Dim curNodeID As Integer, curNodeInfo As sNodeInfo
    ' handle input point
    If includeInput Then R = 1
    curNodeID = ptID
    curNodeInfo = NodeList(curNodeID)
    ' work up
    Do While curNodeInfo.Slot = eSlot.middle
      R += 1
      curNodeID = curNodeInfo.Parent
      curNodeInfo = NodeList(curNodeID)
    Loop
    ' work down
    curNodeID = ptID
    curNodeInfo = NodeList(curNodeID)
    Do While curNodeInfo.MiddleChild > -1
      R += 1
      curNodeID = curNodeInfo.MiddleChild
      curNodeInfo = NodeList(curNodeID)
    Loop
    ' return result
    Return R
  End Function
  Public Function coincidentPoints(ByVal ptID As Integer, _
                                   Optional ByVal includeInput As Boolean = False) As Integer()
    ' returns an array of points coincident with the input point
    Dim nCo As Integer = 0
    Dim R() As Integer
    Dim curNodeID As Integer, curNodeInfo As sNodeInfo
    ' first calculate how many coincident points there are
    nCo = numCoincident(ptID, includeInput)
    ' set up result array
    ReDim R(nCo - 1)
    nCo = 0
    ' populate array
    curNodeID = ptID
    curNodeInfo = NodeList(curNodeID)
    ' starting point
    If includeInput Then
      nCo += 1
      R(nCo - 1) = ptID
    End If
    ' work up
    Do While curNodeInfo.Slot = eSlot.middle
      nCo += 1
      curNodeID = curNodeInfo.Parent
      curNodeInfo = NodeList(curNodeID)
      R(nCo - 1) = curNodeID
    Loop
    ' work down
    curNodeID = ptID
    curNodeInfo = NodeList(curNodeID)
    Do While curNodeInfo.MiddleChild > -1
      nCo += 1
      curNodeID = curNodeInfo.MiddleChild
      curNodeInfo = NodeList(curNodeID)
      R(nCo - 1) = curNodeID
    Loop
    ' return result
    Return R
  End Function
  Public Function oldestCoincidentParent(ByVal ofNodeID As Integer) As Integer
    Dim R As Integer
    R = ofNodeID
    Do While NodeList(R).Slot = eSlot.middle
      R = NodeList(R).Parent
    Loop
    Return R
  End Function
  'Public Function addShapefile(ByRef SF As Shapefile, _
  '                           Optional ByVal PT As Feedback.ProgressTracker = Nothing) _
  '                           As Integer()
  '  ' adds all points in a shapefile
  '  ' returns an array of indices
  '  ' input is randomized to ensure efficient indexing
  '  Dim i, j As Integer
  '  Dim curShpID, curPtIDinShp, curPtIDinSF As Integer
  '  Dim curSHP As Shape, curPT As MapWinGIS.Point
  '  Dim numPts As Integer
  '  Dim R() As Integer
  '  Dim shpIndex() As Integer
  '  Dim ptIndex() As Integer
  '  ' report start
  '  If Not PT Is Nothing Then PT.initializeTask("Indexing points in shapefile...")
  '  ' get number of points for resulting index
  '  numPts = Spatial.ShapefileUtils.numPointsInShapefile(SF)
  '  ' randomize shapes

  '  ' *** debugging - reinstate next line when you're done!!!
  '  '    shpIndex = Data.Sorting.randomOrder(SF.NumShapes)
  '  shpIndex = Data.Sorting.sequenceVector(SF.NumShapes)

  '  ' initialize variables
  '  curPtIDinSF = 0
  '  ReDim R(numPts - 1)
  '  If Not PT Is Nothing Then PT.setTotal(numPts)
  '  ' loop through shapes
  '  For i = 0 To SF.NumShapes - 1
  '    curShpID = shpIndex(i)
  '    curSHP = SF.Shape(curShpID)
  '    ptIndex = Data.Sorting.randomOrder(curSHP.numPoints)
  '    ' loop through points in shape
  '    For j = 0 To curSHP.numPoints - 1
  '      curPtIDinShp = ptIndex(j)
  '      curPT = curSHP.Point(curPtIDinShp)
  '      ' add to index
  '      R(curPtIDinSF) = Me.addPoint(curPT.x, curPT.y, curPtIDinSF, curShpID, curPtIDinShp)
  '      ' report results
  '      If curPtIDinSF Mod 1000 = 0 Then
  '        If Not PT Is Nothing Then
  '          PT.setCompleted(curPtIDinSF)
  '        End If
  '      End If
  '      ' increment total
  '      curPtIDinSF += 1
  '    Next
  '  Next
  '  ' report finish
  '  If Not PT Is Nothing Then PT.finishTask()
  '  ' return result
  '  Return R
  'End Function
  Private Function otherSlot(ByVal fromSlot As eSlot) As eSlot
    Select Case fromSlot
      Case Is = eSlot.left
        Return eSlot.right
      Case Is = eSlot.right
        Return eSlot.left
      Case Else
        Return eSlot.indeterminate
    End Select
  End Function
  Public Sub showInTreeView(ByVal inTree As TreeView)
    addChildrenToTreeView(inTree, RootID, Nothing)
  End Sub
  Private Sub addChildrenToTreeView(ByVal inTree As TreeView, _
                          ByVal myNodeID As Integer, _
                          ByVal parentTreeNode As System.Windows.Forms.TreeNode)
    ' recursive sub used by showInTreeView sub
    ' to populate treeView control
    Dim nI As sNodeInfo
    Dim nextTreeNode As System.Windows.Forms.TreeNode
    Dim nodeText As String
    ' get text for node
    nI = NodeList(myNodeID)
    nodeText = nI.UserIndex.ToString & " " & slotDescription(nI.Slot)
    ' place new node
    If parentTreeNode Is Nothing Then
      nextTreeNode = inTree.Nodes.Add(nodeText)
    Else
      nextTreeNode = parentTreeNode.Nodes.Add(nodeText)
    End If
    ' add children
    If nI.LeftChild <> -1 Then addChildrenToTreeView(inTree, nI.LeftChild, nextTreeNode)
    If nI.RightChild <> -1 Then addChildrenToTreeView(inTree, nI.RightChild, nextTreeNode)
    If nI.MiddleChild <> -1 Then addChildrenToTreeView(inTree, nI.MiddleChild, nextTreeNode)
  End Sub
  Private Function slotDescription(ByVal slot As eSlot) As String
    Select Case slot
      Case Is = eSlot.indeterminate
        Return "indeterminate"
      Case Is = eSlot.left
        Return "left"
      Case Is = eSlot.right
        Return "right"
      Case Is = eSlot.middle
        Return "middle"
      Case Else
        Return "error"
    End Select
  End Function
#End Region
#Region "Sorting Neighbors"
  Public Function indexOf(ByVal nbList As List(Of Neighbor), ByVal nodeID As Integer)
    Dim i As Integer
    For i = 0 To nbList.Count - 1
      If nbList.Item(i).ID = nodeID Then Return i
    Next i
    Return -1
  End Function
#End Region
End Class
Public Class toroidalIndex
  ' creates 9 copies of each point to simulate a toroid
  ' The toroid is a 3x3 grid of the original points
  ' Uses the "userIndex" to record the original point IDs
  ' The first of the nine points is always the real point
  Public realTree As New twoDTree
  Private bBox As twoDTree.sBox
  Private xShiftInterval As Double
  Private yShiftInterval As Double
  Public Sub New(ByVal boundingBox As twoDTree.sBox)
    bBox = boundingBox
    xShiftInterval = bBox.Right - bBox.Left
    yShiftInterval = bBox.Top - bBox.Bottom
  End Sub
  Public Sub addPoints(ByVal pt() As twoDTree.sPoint)
    ' randomizes point order, and then adds them

    ' create 9 copies of each point
    ' get center set
    Dim realPt() As twoDTree.sPoint = pt.Clone
    ReDim Preserve realPt(realPt.Length * 9 - 1)
    Dim numPts As Integer = pt.Length
    ' get 8 surrounding sets
    Dim thisCopy() As twoDTree.sPoint
    Dim startIndex As Integer = 0
    For hzSlot = -1 To 1
      For vtSlot = -1 To 1
        ' get copy
        thisCopy = pt.Clone
        If hzSlot <> 0 Or vtSlot <> 0 Then
          ' adjust values
          For i = 0 To thisCopy.Length - 1
            If hzSlot <> 0 Then thisCopy(i).x += hzSlot
            If vtSlot <> 0 Then thisCopy(i).y += vtSlot
          Next
          ' get next start index
          startIndex += numPts
          ' concatenate to original
          Array.Copy(thisCopy, 0, realPt, startIndex, thisCopy.Length)
        End If
      Next vtSlot
    Next hzSlot
    ' get original indices
    Dim myIndex() As Integer
    ReDim myIndex(numPts * 9 - 1)
    For i = 0 To myIndex.Length - 1
      myIndex(i) = i Mod numPts
    Next
    ' get random sort order
    Dim sortOrder() As Integer = Data.Sorting.randomOrder(myIndex.Length)
    ' add points in random order
    Dim curID As Integer
    For i = 0 To myIndex.Length - 1
      curID = sortOrder(i)
      addPoint(realPt(curID).x, realPt(curID).y, myIndex(curID))
    Next
  End Sub
  Private Function addPoint(ByVal X As Double, ByVal Y As Double, _
                            ByVal ptIndex As Integer) _
                            As Integer
    ' get index of next point by dividing current node count by 9
    Dim thisID As Integer = realTree.NodeList.Count / 9
    ' add first point
    realTree.addPoint(X, Y, thisID)
    ' add other 8 points
    For hzSlot = -1 To 1
      For vtSlot = -1 To 1
        If Not ((hzSlot = 0) And (vtSlot = 0)) Then
          realTree.addPoint(X + hzSlot * xShiftInterval, _
                          Y + vtSlot * yShiftInterval, _
                          thisID)
        End If
      Next
    Next
  End Function
  Public Function nearestNodeIDs(ByVal fromPt As twoDTree.sPoint, _
                                Optional ByVal minCount As Integer = 1, _
                                Optional ByVal minSearchRadius As Double = 0, _
                                Optional ByVal toroidalDistance As Boolean = True, _
                                Optional ByVal boundaryOffsetVec As twoDTree.cPoint = Nothing) _
                                As List(Of Neighbor)
    ' determines the nearest nodes
    ' if toroidalDistance is true, performs wrap-around search
    ' if toroidalDistance is false, performs search within boundary as offset by boundaryOffsetVec
    ' (used in ToroidalShift hypothesis testing)
    Dim P As New twoDTree.cPoint(fromPt.x, fromPt.y)
    ' set up basic query parameters
    Dim Q As New twoDTree.sQueryParam(P, minCount)
    ' set up minimum search radius
    If minSearchRadius > 0 Then
      Dim searchCircle As New twoDTree.cCircle(P, minSearchRadius)
      Q.compulsoryRegion = searchCircle
    End If
    ' set boundary condition
    If toroidalDistance = False Then
      ' initialize to boundary
      Dim boundingBox As New twoDTree.cRectangle(bBox)
      ' apply offset vector
      If Not boundaryOffsetVec Is Nothing Then
        boundingBox.myBox.Left += boundaryOffsetVec.X
        boundingBox.myBox.Right += boundaryOffsetVec.X
        boundingBox.myBox.Bottom += boundaryOffsetVec.Y
        boundingBox.myBox.Top += boundaryOffsetVec.Y
      End If
      ' add to query parameters
      Q.searchLimit = boundingBox
    End If
    ' run query
    Dim realNeighbors As List(Of Neighbor) = realTree.nearestNodeIDs(Q)
    Dim R As New List(Of Neighbor)
    For Each realNb As Neighbor In realNeighbors
      Dim fakeID As Integer = realTree.nodeInformation(realNb.ID).UserIndex
      Dim fakeNb As New Neighbor(fakeID, realNb.Distance)
      R.Add(fakeNb)
    Next
    Return R
  End Function

  
  Public Sub clear()
    realTree.clear()
  End Sub
  Public Function nodeInformation(ByVal nodeID As Integer) As twoDTree.sNodeInfo
    ' get actual ID
    ' remember there are 9 points for every real point
    ' and the real one is always the first of the 9
    Dim realID As Integer = nodeID * 9
    Return realTree.nodeInformation(realID)
  End Function
  Public Function coincidentPoints(ByVal ptID As Integer, _
                                   Optional ByVal includeInput As Boolean = False) As Integer()
    Dim R() As Integer
    Dim realPtIDs() As Integer = realTree.coincidentPoints(ptID, includeInput)
    ReDim R(realPtIDs.Length - 1)
    For i = 0 To R.Length - 1
      R(i) = realTree.nodeInformation(realPtIDs(i)).UserIndex
    Next
    Return R
  End Function
End Class
Public Class Neighbor
  Public ID As Integer
  Public Distance As Double
  Public Sub New(ByVal newID As Integer, ByVal newDistance As Double)
    ID = newID
    Distance = newDistance
  End Sub
  Shared Function compareNeighbors(ByVal a As Neighbor, ByVal b As Neighbor) As Integer
    Return a.Distance.CompareTo(b.Distance)
  End Function
End Class

