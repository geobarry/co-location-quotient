Imports DotSpatial
Imports DotSpatial.Data
Imports DotSpatial.Topology
Imports DotSpatial.Controls
Imports DotSpatial.Symbology

Public Class SpatialIndexTestWindow
  Dim frameLayer As IMapPolygonLayer
  Dim compulsoryLayer As IMapPolygonLayer
  Dim ptLayer As IMapPointLayer
  Dim ptIndex As New twoDTree
  Private Sub SpatialIndexTestWindow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ' set the extent of the map
    mapMain.Projection = standardProjection()
    mapMain.ViewExtents = New Extent(0, 0, 1, 1)
    ' create frame polygon layer
    Dim coordList As New List(Of Coordinate)
    coordList.Add(New Coordinate(0, 0))
    coordList.Add(New Coordinate(0, 1))
    coordList.Add(New Coordinate(1, 1))
    coordList.Add(New Coordinate(1, 0))
    Dim frameFeat As New Polygon(coordList)
    Dim frameFS As New FeatureSet(FeatureType.Point)
    frameFS.AddFeature(frameFeat)
    frameFS.Projection = standardProjection()
    frameLayer = New MapPolygonLayer(frameFS)
    Dim polySim As New PolygonSymbolizer(Color.Transparent, Color.Black)
    frameLayer.Symbolizer = polySim
    mapMain.Layers.Add(frameLayer)
    mapMain.ZoomToMaxExtent()
    mapMain.ZoomOut()
    ' create compulsory region
    coordList.Clear()
    coordList.Add(New Coordinate(0.75, 0.75))
    coordList.Add(New Coordinate(0.75, 0.9))
    coordList.Add(New Coordinate(0.9, 0.9))
    coordList.Add(New Coordinate(0.9, 0.75))
    Dim compulsoryFeat As New Polygon(coordList)
    Dim compulsoryFS As New FeatureSet(FeatureType.Point)
    compulsoryFS.AddFeature(compulsoryFeat)
    compulsoryFS.Projection = standardProjection()
    compulsoryLayer = New MapPolygonLayer(compulsoryFS)
    polySim = New PolygonSymbolizer(Color.Transparent, Color.Gray)
    compulsoryLayer.Symbolizer = polySim
    mapMain.Layers.Add(compulsoryLayer)
    mapMain.ZoomToMaxExtent()
    mapMain.ZoomOut()
    ' create new point layer
    Dim ptFS As New FeatureSet(FeatureType.Point)
    ptFS.Projection = standardProjection()
    ptLayer = New MapPointLayer(ptFS)
    Dim ptSz As Integer = 10
    Dim ps As New DotSpatial.Symbology.PointSymbolizer(Color.Blue, Symbology.PointShape.Ellipse, ptSz)
    ptLayer.Symbolizer = ps
    Dim ss As New DotSpatial.Symbology.PointSymbolizer(Color.Red, Symbology.PointShape.Ellipse, ptSz)
    ptLayer.SelectionSymbolizer = ss
    ' add to map
    mapMain.Layers.Add(ptLayer)
  End Sub
  Private Function standardProjection() As Projections.ProjectionInfo
    Return Projections.KnownCoordinateSystems.Projected.UtmNad1983.NAD1983UTMZone17N
  End Function
  Private Sub mapMain_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapMain.MouseMove
    ' show coordinates
    Dim mouseLoc As Coordinate = mapMain.PixelToProj(e.Location)
    lblInfo.Text = "X: " & mouseLoc.X.ToString & vbCrLf & "Y: " & mouseLoc.Y.ToString
  End Sub
  Private Sub mapMain_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mapMain.MouseUp
    ' determine coordinate location
    Dim mouseCoord As Coordinate = mapMain.PixelToProj(e.Location)
    ' take action
    If radAddPoint.Checked Then addPoint(mouseCoord)
    If radSelectPoint.Checked Then selectPoints(mouseCoord)
    mapMain.Refresh()
  End Sub
  Private Sub addPoint(ByVal mouseLoc As Coordinate)
    ' add a point to the point layer
    ptLayer.DataSet.AddFeature(New Feature(mouseLoc))
    ptIndex.addPoint(mouseLoc.X, mouseLoc.Y)
  End Sub
  Private Sub selectPoints(ByVal C As Coordinate)
    ' make sure number of points is not too high
    Dim numToSelect As Integer = udNumPts.Value
    If numToSelect > ptLayer.DataSet.NumRows Then numToSelect = ptLayer.DataSet.NumRows
    ' get list of points
    Dim P As New twoDTree.cPoint(C.X, C.Y)
    ' get query conditions
    Dim searchLimit As New twoDTree.cRectangle(0, 1, 0, 1)
    Dim compulsoryArea As New twoDTree.cRectangle(0.75, 0.9, 0.75, 0.9)
    Dim Q As New twoDTree.sQueryParam(P, numToSelect, , searchLimit)
    ' run query
    lblRootChecked.Text = "root not checked..."
    Dim nbList As List(Of Neighbor) = ptIndex.nearestNodeIDs(Q)
    ' report number of points searched
    lblPtsSearched.Text = ptIndex.numNodesChecked.Last.ToString
    ' highlight results
    ptLayer.Selection.Clear()
    For Each nb In nbList
      ptLayer.Select(nb.ID)
    Next
  End Sub

  Private Sub btnAddRandom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddRandom.Click
    ' for now, add 100 random points
    Randomize()
    For i = 0 To 12
      Dim C As New Coordinate
      C.X = Rnd()
      C.Y = Rnd()
      For xShift = -1 To 1
        For yShift = -1 To 1
          Dim D As New Coordinate
          D.X = C.X + xShift
          D.Y = C.Y + yShift
          addPoint(D)
        Next
      Next
    Next
    mapMain.Refresh()
  End Sub
End Class