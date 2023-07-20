Imports System.Windows.Forms
Imports DotSpatial.Data
Public Class dlgTorusDefinition
  Private left As Integer
  Private top As Integer
  Private right As Integer
  Private bottom As Integer

  Public Sub New(ptFS As FeatureSet)

    ' This call is required by the designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    Dim xt As Extent = ptFS.Extent
    left = xt.MinX
    right = xt.MaxX
    top = xt.MaxY
    bottom = xt.MinY
  End Sub

  Public Function getExtent()
    Dim E As New DotSpatial.Data.Extent(left, bottom, right, top)
    Return (E)
  End Function
  Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

  Private Sub txtLeft_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLeft.TextChanged

  End Sub
  Private Sub validateTextBoxes()
    ' check all text boxes
    Dim isValid As Boolean = True
    ' make sure values are numeric
    If Not IsNumeric(txtLeft.Text) Then isValid = False
    If Not IsNumeric(txtBottom.Text) Then isValid = False
    If Not IsNumeric(txtRight.Text) Then isValid = False
    If Not IsNumeric(txtTop.Text) Then isValid = False
    ' get values
    Dim newTop As Double = Val(txtTop.Text)
    Dim newBottom As Double = Val(txtBottom.Text)
    Dim newLeft As Double = Val(txtLeft.Text)
    Dim newRight As Double = Val(txtRight.Text)
    ' check validity
    If newTop <= newbottom Then isValid = False
    If newright <= newleft Then isValid = False
    ' if valid, update variables
    If isValid Then
      top = newTop
      right = newRight
      bottom = newBottom
      left = newLeft
    Else ' otherwise, revert
      txtTop.Text = top.ToString
      txtBottom.Text = bottom.ToString
      txtLeft.Text = left.ToString
      txtRight.Text = right.ToString
    End If
  End Sub

  Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

  End Sub

  Private Sub dlgTorusDefinition_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Me.AcceptButton = OK_Button
    txtTop.Text = top.ToString
    txtBottom.Text = bottom.ToString
    txtLeft.Text = left.ToString
    txtRight.Text = right.ToString
  End Sub
End Class
