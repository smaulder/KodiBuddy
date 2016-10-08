<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMovieDetails
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.MovieNameLbl = New System.Windows.Forms.Label()
        Me.MovieIDLbl = New System.Windows.Forms.Label()
        Me.YearLbl = New System.Windows.Forms.Label()
        Me.OverviewTxt = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Movie Name:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Movie ID:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Overview:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Year:"
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(395, 162)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 23)
        Me.BtnClose.TabIndex = 4
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'MovieNameLbl
        '
        Me.MovieNameLbl.AutoSize = True
        Me.MovieNameLbl.Location = New System.Drawing.Point(86, 9)
        Me.MovieNameLbl.Name = "MovieNameLbl"
        Me.MovieNameLbl.Size = New System.Drawing.Size(0, 13)
        Me.MovieNameLbl.TabIndex = 5
        '
        'MovieIDLbl
        '
        Me.MovieIDLbl.AutoSize = True
        Me.MovieIDLbl.Location = New System.Drawing.Point(86, 37)
        Me.MovieIDLbl.Name = "MovieIDLbl"
        Me.MovieIDLbl.Size = New System.Drawing.Size(0, 13)
        Me.MovieIDLbl.TabIndex = 6
        '
        'YearLbl
        '
        Me.YearLbl.AutoSize = True
        Me.YearLbl.Location = New System.Drawing.Point(86, 65)
        Me.YearLbl.Name = "YearLbl"
        Me.YearLbl.Size = New System.Drawing.Size(0, 13)
        Me.YearLbl.TabIndex = 8
        '
        'OverviewTxt
        '
        Me.OverviewTxt.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.OverviewTxt.Enabled = False
        Me.OverviewTxt.Location = New System.Drawing.Point(89, 92)
        Me.OverviewTxt.Multiline = True
        Me.OverviewTxt.Name = "OverviewTxt"
        Me.OverviewTxt.Size = New System.Drawing.Size(683, 64)
        Me.OverviewTxt.TabIndex = 9
        '
        'FrmMovieDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(828, 194)
        Me.Controls.Add(Me.OverviewTxt)
        Me.Controls.Add(Me.YearLbl)
        Me.Controls.Add(Me.MovieIDLbl)
        Me.Controls.Add(Me.MovieNameLbl)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "FrmMovieDetails"
        Me.Text = "FrmMovieDetails"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents BtnClose As Button
    Friend WithEvents MovieNameLbl As Label
    Friend WithEvents MovieIDLbl As Label
    Friend WithEvents YearLbl As Label
    Friend WithEvents OverviewTxt As TextBox
End Class
