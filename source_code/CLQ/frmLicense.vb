Public Class frmLicense

  Private Sub frmLicense_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    WB.DocumentText = "<div backcolor='#555555'> Copyright (c) 2014, Barry J. Kronenfeld (bjkronenfeld@eiu.edu)<br />" & _
                      "<p>This software was developed to support calculation of the co-location quotient " & _
                      "(CLQ). For more information about the CLQ, please refer to the following article:</p> " & _
                      "<p><em>Tim Leslie and Barry Kronenfeld. 2011. The Colocation Quotient: " & _
                      "A New Measure of Spatial Association Between Categorical Subsets of Points.</em> " & _
                      "Geographical Analysis<em>, vol. 43, pp. 306-326.</p><BR></em>" & _
                      "This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0" & _
                      "International License. To view a copy of this license, visit " & _
                      "<a hlink='http://creativecommons.org/licenses/by-nc-sa/4.0/'>http://creativecommons.org/licenses/by-nc-sa/4.0/</a></div>"
  End Sub


End Class