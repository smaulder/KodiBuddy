<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.BtnSelectFolder = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.txtFolderPath = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CkBxUseBracket = New System.Windows.Forms.CheckBox()
        Me.CkBxUseParens = New System.Windows.Forms.CheckBox()
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.LblMessage = New System.Windows.Forms.Label()
        Me.BtnIMDB = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblRecordCount = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BtnSelectFolder
        '
        Me.BtnSelectFolder.Location = New System.Drawing.Point(612, 21)
        Me.BtnSelectFolder.Name = "BtnSelectFolder"
        Me.BtnSelectFolder.Size = New System.Drawing.Size(21, 23)
        Me.BtnSelectFolder.TabIndex = 0
        Me.BtnSelectFolder.Text = "..."
        Me.BtnSelectFolder.UseVisualStyleBackColor = True
        '
        'txtFolderPath
        '
        Me.txtFolderPath.Location = New System.Drawing.Point(295, 21)
        Me.txtFolderPath.Name = "txtFolderPath"
        Me.txtFolderPath.Size = New System.Drawing.Size(314, 20)
        Me.txtFolderPath.TabIndex = 1
        Me.txtFolderPath.Text = "V:\"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(222, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Folder Path:"
        '
        'CkBxUseBracket
        '
        Me.CkBxUseBracket.AutoSize = True
        Me.CkBxUseBracket.Location = New System.Drawing.Point(298, 87)
        Me.CkBxUseBracket.Name = "CkBxUseBracket"
        Me.CkBxUseBracket.Size = New System.Drawing.Size(77, 17)
        Me.CkBxUseBracket.TabIndex = 7
        Me.CkBxUseBracket.Text = "Use [Date]"
        Me.CkBxUseBracket.UseVisualStyleBackColor = True
        '
        'CkBxUseParens
        '
        Me.CkBxUseParens.AutoSize = True
        Me.CkBxUseParens.Location = New System.Drawing.Point(384, 87)
        Me.CkBxUseParens.Name = "CkBxUseParens"
        Me.CkBxUseParens.Size = New System.Drawing.Size(77, 17)
        Me.CkBxUseParens.TabIndex = 8
        Me.CkBxUseParens.Text = "Use (Date)"
        Me.CkBxUseParens.UseVisualStyleBackColor = True
        '
        'BtnStart
        '
        Me.BtnStart.Location = New System.Drawing.Point(295, 123)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(123, 23)
        Me.BtnStart.TabIndex = 9
        Me.BtnStart.Text = "Change Date Brackets"
        Me.BtnStart.UseVisualStyleBackColor = True
        '
        'LblMessage
        '
        Me.LblMessage.AutoSize = True
        Me.LblMessage.Location = New System.Drawing.Point(222, 160)
        Me.LblMessage.Name = "LblMessage"
        Me.LblMessage.Size = New System.Drawing.Size(0, 13)
        Me.LblMessage.TabIndex = 10
        '
        'BtnIMDB
        '
        Me.BtnIMDB.Location = New System.Drawing.Point(434, 123)
        Me.BtnIMDB.Name = "BtnIMDB"
        Me.BtnIMDB.Size = New System.Drawing.Size(84, 23)
        Me.BtnIMDB.TabIndex = 11
        Me.BtnIMDB.Text = "IMDB Update"
        Me.BtnIMDB.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(292, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Folder Count:"
        '
        'LblRecordCount
        '
        Me.LblRecordCount.AutoSize = True
        Me.LblRecordCount.Location = New System.Drawing.Point(368, 56)
        Me.LblRecordCount.Name = "LblRecordCount"
        Me.LblRecordCount.Size = New System.Drawing.Size(0, 13)
        Me.LblRecordCount.TabIndex = 13
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(534, 123)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 23)
        Me.BtnCancel.TabIndex = 14
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(910, 209)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.LblRecordCount)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnIMDB)
        Me.Controls.Add(Me.LblMessage)
        Me.Controls.Add(Me.BtnStart)
        Me.Controls.Add(Me.CkBxUseParens)
        Me.Controls.Add(Me.CkBxUseBracket)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtFolderPath)
        Me.Controls.Add(Me.BtnSelectFolder)
        Me.Name = "Form1"
        Me.Text = "Movie Folder Cleaner"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnSelectFolder As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents txtFolderPath As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents CkBxUseBracket As CheckBox
    Friend WithEvents CkBxUseParens As CheckBox
    Friend WithEvents BtnStart As Button
    Friend WithEvents LblMessage As Label
    Friend WithEvents BtnIMDB As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents LblRecordCount As Label
    Friend WithEvents BtnCancel As Button
End Class
