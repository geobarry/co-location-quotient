<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgTorusDefinition
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
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.OK_Button = New System.Windows.Forms.Button()
    Me.Cancel_Button = New System.Windows.Forms.Button()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.txtRight = New System.Windows.Forms.TextBox()
    Me.txtTop = New System.Windows.Forms.TextBox()
    Me.lblOutline = New System.Windows.Forms.Label()
    Me.txtBottom = New System.Windows.Forms.TextBox()
    Me.txtLeft = New System.Windows.Forms.TextBox()
    Me.Label5 = New System.Windows.Forms.Label()
    Me.Label6 = New System.Windows.Forms.Label()
    Me.Label7 = New System.Windows.Forms.Label()
    Me.Label8 = New System.Windows.Forms.Label()
    Me.TableLayoutPanel1.SuspendLayout()
    Me.SuspendLayout()
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.TableLayoutPanel1.ColumnCount = 2
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(117, 234)
    Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 1
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(195, 36)
    Me.TableLayoutPanel1.TabIndex = 0
    '
    'OK_Button
    '
    Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.OK_Button.Location = New System.Drawing.Point(4, 4)
    Me.OK_Button.Margin = New System.Windows.Forms.Padding(4)
    Me.OK_Button.Name = "OK_Button"
    Me.OK_Button.Size = New System.Drawing.Size(89, 28)
    Me.OK_Button.TabIndex = 13
    Me.OK_Button.Text = "OK"
    '
    'Cancel_Button
    '
    Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.Cancel_Button.Location = New System.Drawing.Point(101, 4)
    Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4)
    Me.Cancel_Button.Name = "Cancel_Button"
    Me.Cancel_Button.Size = New System.Drawing.Size(89, 28)
    Me.Cancel_Button.TabIndex = 1
    Me.Cancel_Button.Text = "Cancel"
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Location = New System.Drawing.Point(156, 5)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(38, 17)
    Me.Label4.TabIndex = 17
    Me.Label4.Text = "(top)"
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(99, 175)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(61, 17)
    Me.Label3.TabIndex = 16
    Me.Label3.Text = "(bottom)"
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(208, 72)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(46, 17)
    Me.Label2.TabIndex = 15
    Me.Label2.Text = "(right)"
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(59, 105)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(37, 17)
    Me.Label1.TabIndex = 14
    Me.Label1.Text = "(left)"
    '
    'txtRight
    '
    Me.txtRight.Location = New System.Drawing.Point(159, 25)
    Me.txtRight.Name = "txtRight"
    Me.txtRight.Size = New System.Drawing.Size(43, 22)
    Me.txtRight.TabIndex = 12
    Me.txtRight.Text = "1"
    '
    'txtTop
    '
    Me.txtTop.Location = New System.Drawing.Point(205, 47)
    Me.txtTop.Name = "txtTop"
    Me.txtTop.Size = New System.Drawing.Size(43, 22)
    Me.txtTop.TabIndex = 11
    Me.txtTop.Text = "1"
    Me.txtTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    '
    'lblOutline
    '
    Me.lblOutline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.lblOutline.Location = New System.Drawing.Point(102, 47)
    Me.lblOutline.Name = "lblOutline"
    Me.lblOutline.Size = New System.Drawing.Size(100, 100)
    Me.lblOutline.TabIndex = 11
    Me.lblOutline.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtBottom
    '
    Me.txtBottom.Location = New System.Drawing.Point(102, 150)
    Me.txtBottom.Name = "txtBottom"
    Me.txtBottom.Size = New System.Drawing.Size(43, 22)
    Me.txtBottom.TabIndex = 10
    Me.txtBottom.Text = "0"
    '
    'txtLeft
    '
    Me.txtLeft.Location = New System.Drawing.Point(53, 125)
    Me.txtLeft.Name = "txtLeft"
    Me.txtLeft.Size = New System.Drawing.Size(43, 22)
    Me.txtLeft.TabIndex = 9
    Me.txtLeft.Text = "0"
    Me.txtLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    '
    'Label5
    '
    Me.Label5.AutoSize = True
    Me.Label5.Location = New System.Drawing.Point(28, 128)
    Me.Label5.Name = "Label5"
    Me.Label5.Size = New System.Drawing.Size(22, 17)
    Me.Label5.TabIndex = 18
    Me.Label5.Text = "x1"
    Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label6
    '
    Me.Label6.AutoSize = True
    Me.Label6.Location = New System.Drawing.Point(79, 153)
    Me.Label6.Name = "Label6"
    Me.Label6.Size = New System.Drawing.Size(23, 17)
    Me.Label6.TabIndex = 19
    Me.Label6.Text = "y1"
    '
    'Label7
    '
    Me.Label7.AutoSize = True
    Me.Label7.Location = New System.Drawing.Point(135, 27)
    Me.Label7.Name = "Label7"
    Me.Label7.Size = New System.Drawing.Size(23, 17)
    Me.Label7.TabIndex = 20
    Me.Label7.Text = "y2"
    '
    'Label8
    '
    Me.Label8.AutoSize = True
    Me.Label8.Location = New System.Drawing.Point(253, 47)
    Me.Label8.Name = "Label8"
    Me.Label8.Size = New System.Drawing.Size(22, 17)
    Me.Label8.TabIndex = 21
    Me.Label8.Text = "x2"
    '
    'dlgTorusDefinition
    '
    Me.AcceptButton = Me.OK_Button
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.Cancel_Button
    Me.ClientSize = New System.Drawing.Size(325, 283)
    Me.Controls.Add(Me.Label8)
    Me.Controls.Add(Me.Label7)
    Me.Controls.Add(Me.Label6)
    Me.Controls.Add(Me.Label5)
    Me.Controls.Add(Me.Label4)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.txtRight)
    Me.Controls.Add(Me.txtTop)
    Me.Controls.Add(Me.lblOutline)
    Me.Controls.Add(Me.txtBottom)
    Me.Controls.Add(Me.txtLeft)
    Me.Controls.Add(Me.TableLayoutPanel1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Margin = New System.Windows.Forms.Padding(4)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "dlgTorusDefinition"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Torus Boundary"
    Me.TableLayoutPanel1.ResumeLayout(False)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents OK_Button As System.Windows.Forms.Button
  Friend WithEvents Cancel_Button As System.Windows.Forms.Button
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents txtRight As System.Windows.Forms.TextBox
  Friend WithEvents txtTop As System.Windows.Forms.TextBox
  Friend WithEvents lblOutline As System.Windows.Forms.Label
  Friend WithEvents txtBottom As System.Windows.Forms.TextBox
  Friend WithEvents txtLeft As System.Windows.Forms.TextBox
  Friend WithEvents Label5 As System.Windows.Forms.Label
  Friend WithEvents Label6 As System.Windows.Forms.Label
  Friend WithEvents Label7 As System.Windows.Forms.Label
  Friend WithEvents Label8 As System.Windows.Forms.Label

End Class
