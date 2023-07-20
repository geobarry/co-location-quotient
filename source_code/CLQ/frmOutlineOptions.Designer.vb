<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOutlineOptions
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
    Me.txtLeft = New System.Windows.Forms.TextBox()
    Me.bottom = New System.Windows.Forms.TextBox()
    Me.lblOutline = New System.Windows.Forms.Label()
    Me.txtTop = New System.Windows.Forms.TextBox()
    Me.txtRight = New System.Windows.Forms.TextBox()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.Label3 = New System.Windows.Forms.Label()
    Me.Label4 = New System.Windows.Forms.Label()
    Me.btnOK = New System.Windows.Forms.Button()
    Me.btnCancel = New System.Windows.Forms.Button()
    Me.SuspendLayout()
    '
    'txtLeft
    '
    Me.txtLeft.Location = New System.Drawing.Point(33, 127)
    Me.txtLeft.Name = "txtLeft"
    Me.txtLeft.Size = New System.Drawing.Size(43, 22)
    Me.txtLeft.TabIndex = 0
    Me.txtLeft.Text = "0"
    Me.txtLeft.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    '
    'bottom
    '
    Me.bottom.Location = New System.Drawing.Point(82, 152)
    Me.bottom.Name = "bottom"
    Me.bottom.Size = New System.Drawing.Size(43, 22)
    Me.bottom.TabIndex = 1
    Me.bottom.Text = "0"
    '
    'lblOutline
    '
    Me.lblOutline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
    Me.lblOutline.Location = New System.Drawing.Point(82, 49)
    Me.lblOutline.Name = "lblOutline"
    Me.lblOutline.Size = New System.Drawing.Size(100, 100)
    Me.lblOutline.TabIndex = 2
    Me.lblOutline.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtTop
    '
    Me.txtTop.Location = New System.Drawing.Point(185, 49)
    Me.txtTop.Name = "txtTop"
    Me.txtTop.Size = New System.Drawing.Size(43, 22)
    Me.txtTop.TabIndex = 3
    Me.txtTop.Text = "1"
    Me.txtTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
    '
    'txtRight
    '
    Me.txtRight.Location = New System.Drawing.Point(139, 27)
    Me.txtRight.Name = "txtRight"
    Me.txtRight.Size = New System.Drawing.Size(43, 22)
    Me.txtRight.TabIndex = 4
    Me.txtRight.Text = "1"
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(21, 107)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(55, 17)
    Me.Label1.TabIndex = 5
    Me.Label1.Text = "x1 (left)"
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(188, 74)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(64, 17)
    Me.Label2.TabIndex = 6
    Me.Label2.Text = "x2 (right)"
    '
    'Label3
    '
    Me.Label3.AutoSize = True
    Me.Label3.Location = New System.Drawing.Point(79, 177)
    Me.Label3.Name = "Label3"
    Me.Label3.Size = New System.Drawing.Size(80, 17)
    Me.Label3.TabIndex = 7
    Me.Label3.Text = "y1 (bottom)"
    '
    'Label4
    '
    Me.Label4.AutoSize = True
    Me.Label4.Location = New System.Drawing.Point(136, 7)
    Me.Label4.Name = "Label4"
    Me.Label4.Size = New System.Drawing.Size(57, 17)
    Me.Label4.TabIndex = 8
    Me.Label4.Text = "y2 (top)"
    '
    'btnOK
    '
    Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.btnOK.Location = New System.Drawing.Point(98, 224)
    Me.btnOK.Name = "btnOK"
    Me.btnOK.Size = New System.Drawing.Size(89, 28)
    Me.btnOK.TabIndex = 9
    Me.btnOK.Text = "OK"
    Me.btnOK.UseVisualStyleBackColor = True
    '
    'btnCancel
    '
    Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.btnCancel.Location = New System.Drawing.Point(192, 224)
    Me.btnCancel.Name = "btnCancel"
    Me.btnCancel.Size = New System.Drawing.Size(89, 28)
    Me.btnCancel.TabIndex = 10
    Me.btnCancel.Text = "Cancel"
    Me.btnCancel.UseVisualStyleBackColor = True
    '
    'frmOutlineOptions
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(292, 268)
    Me.Controls.Add(Me.btnCancel)
    Me.Controls.Add(Me.btnOK)
    Me.Controls.Add(Me.Label4)
    Me.Controls.Add(Me.Label3)
    Me.Controls.Add(Me.Label2)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.txtRight)
    Me.Controls.Add(Me.txtTop)
    Me.Controls.Add(Me.lblOutline)
    Me.Controls.Add(Me.bottom)
    Me.Controls.Add(Me.txtLeft)
    Me.Name = "frmOutlineOptions"
    Me.Text = "Torus Definition"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents txtLeft As System.Windows.Forms.TextBox
  Friend WithEvents bottom As System.Windows.Forms.TextBox
  Friend WithEvents lblOutline As System.Windows.Forms.Label
  Friend WithEvents txtTop As System.Windows.Forms.TextBox
  Friend WithEvents txtRight As System.Windows.Forms.TextBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents Label3 As System.Windows.Forms.Label
  Friend WithEvents Label4 As System.Windows.Forms.Label
  Friend WithEvents btnOK As System.Windows.Forms.Button
  Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
