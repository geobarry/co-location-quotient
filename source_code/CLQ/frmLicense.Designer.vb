﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLicense
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.WB = New System.Windows.Forms.WebBrowser()
    Me.SuspendLayout()
    '
    'WB
    '
    Me.WB.AllowNavigation = False
    Me.WB.AllowWebBrowserDrop = False
    Me.WB.Dock = System.Windows.Forms.DockStyle.Fill
    Me.WB.Location = New System.Drawing.Point(0, 0)
    Me.WB.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
    Me.WB.MinimumSize = New System.Drawing.Size(27, 25)
    Me.WB.Name = "WB"
    Me.WB.Size = New System.Drawing.Size(959, 292)
    Me.WB.TabIndex = 0
    '
    'frmLicense
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(959, 292)
    Me.Controls.Add(Me.WB)
    Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
    Me.Name = "frmLicense"
    Me.Text = "About this software:"
    Me.ResumeLayout(False)

  End Sub
  Friend WithEvents WB As System.Windows.Forms.WebBrowser
End Class
