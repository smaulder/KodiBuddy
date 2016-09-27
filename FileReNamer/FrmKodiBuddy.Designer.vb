<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmKodiBuddy
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
        Me.FolderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.TabExecute = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.LblCurrentDir = New System.Windows.Forms.Label()
        Me.BtnRemap = New System.Windows.Forms.Button()
        Me.BtnFileReName = New System.Windows.Forms.Button()
        Me.TxtMessageDisplay = New System.Windows.Forms.TextBox()
        Me.BtnVideoImport = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnMovieDBUpdate = New System.Windows.Forms.Button()
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.TabOptions = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CbxUseYear = New System.Windows.Forms.CheckBox()
        Me.CbxUseGenres = New System.Windows.Forms.CheckBox()
        Me.CbxGenres = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtImportFolder = New System.Windows.Forms.TextBox()
        Me.BtnSelectImportFolder = New System.Windows.Forms.Button()
        Me.LblImportFolder = New System.Windows.Forms.Label()
        Me.CkBxUseParens = New System.Windows.Forms.CheckBox()
        Me.CkBxUseBracket = New System.Windows.Forms.CheckBox()
        Me.txtFolderPath = New System.Windows.Forms.TextBox()
        Me.BtnSelectFolder = New System.Windows.Forms.Button()
        Me.LblRecordCount = New System.Windows.Forms.Label()
        Me.LblMovieFolder = New System.Windows.Forms.Label()
        Me.LblFolderCount = New System.Windows.Forms.Label()
        Me.TabControl.SuspendLayout()
        Me.TabExecute.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.TabOptions.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.TabExecute)
        Me.TabControl.Controls.Add(Me.TabOptions)
        Me.TabControl.Location = New System.Drawing.Point(12, 13)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(790, 278)
        Me.TabControl.TabIndex = 15
        '
        'TabExecute
        '
        Me.TabExecute.Controls.Add(Me.Panel2)
        Me.TabExecute.Location = New System.Drawing.Point(4, 22)
        Me.TabExecute.Name = "TabExecute"
        Me.TabExecute.Padding = New System.Windows.Forms.Padding(3)
        Me.TabExecute.Size = New System.Drawing.Size(782, 252)
        Me.TabExecute.TabIndex = 0
        Me.TabExecute.Text = "KodiBuddy"
        Me.TabExecute.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.LblCurrentDir)
        Me.Panel2.Controls.Add(Me.BtnRemap)
        Me.Panel2.Controls.Add(Me.BtnFileReName)
        Me.Panel2.Controls.Add(Me.TxtMessageDisplay)
        Me.Panel2.Controls.Add(Me.BtnVideoImport)
        Me.Panel2.Controls.Add(Me.BtnCancel)
        Me.Panel2.Controls.Add(Me.BtnMovieDBUpdate)
        Me.Panel2.Controls.Add(Me.BtnStart)
        Me.Panel2.Location = New System.Drawing.Point(7, 7)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(769, 242)
        Me.Panel2.TabIndex = 19
        '
        'LblCurrentDir
        '
        Me.LblCurrentDir.AutoSize = True
        Me.LblCurrentDir.Location = New System.Drawing.Point(156, 214)
        Me.LblCurrentDir.Name = "LblCurrentDir"
        Me.LblCurrentDir.Size = New System.Drawing.Size(0, 13)
        Me.LblCurrentDir.TabIndex = 25
        '
        'BtnRemap
        '
        Me.BtnRemap.Location = New System.Drawing.Point(18, 136)
        Me.BtnRemap.Name = "BtnRemap"
        Me.BtnRemap.Size = New System.Drawing.Size(122, 23)
        Me.BtnRemap.TabIndex = 24
        Me.BtnRemap.Text = "ReMap Folders"
        Me.BtnRemap.UseVisualStyleBackColor = True
        '
        'BtnFileReName
        '
        Me.BtnFileReName.Location = New System.Drawing.Point(17, 104)
        Me.BtnFileReName.Name = "BtnFileReName"
        Me.BtnFileReName.Size = New System.Drawing.Size(122, 23)
        Me.BtnFileReName.TabIndex = 23
        Me.BtnFileReName.Text = "FileReName"
        Me.BtnFileReName.UseVisualStyleBackColor = True
        '
        'TxtMessageDisplay
        '
        Me.TxtMessageDisplay.Location = New System.Drawing.Point(159, 5)
        Me.TxtMessageDisplay.Multiline = True
        Me.TxtMessageDisplay.Name = "TxtMessageDisplay"
        Me.TxtMessageDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TxtMessageDisplay.Size = New System.Drawing.Size(607, 206)
        Me.TxtMessageDisplay.TabIndex = 22
        '
        'BtnVideoImport
        '
        Me.BtnVideoImport.Location = New System.Drawing.Point(17, 75)
        Me.BtnVideoImport.Name = "BtnVideoImport"
        Me.BtnVideoImport.Size = New System.Drawing.Size(122, 23)
        Me.BtnVideoImport.TabIndex = 21
        Me.BtnVideoImport.Text = "Video Import"
        Me.BtnVideoImport.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(17, 165)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(122, 23)
        Me.BtnCancel.TabIndex = 20
        Me.BtnCancel.Text = "Exit"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnMovieDBUpdate
        '
        Me.BtnMovieDBUpdate.Location = New System.Drawing.Point(17, 45)
        Me.BtnMovieDBUpdate.Name = "BtnMovieDBUpdate"
        Me.BtnMovieDBUpdate.Size = New System.Drawing.Size(123, 23)
        Me.BtnMovieDBUpdate.TabIndex = 19
        Me.BtnMovieDBUpdate.Text = "The Movie DB Update"
        Me.BtnMovieDBUpdate.UseVisualStyleBackColor = True
        '
        'BtnStart
        '
        Me.BtnStart.Location = New System.Drawing.Point(17, 16)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(123, 23)
        Me.BtnStart.TabIndex = 18
        Me.BtnStart.Text = "Change Date Brackets"
        Me.BtnStart.UseVisualStyleBackColor = True
        '
        'TabOptions
        '
        Me.TabOptions.Controls.Add(Me.Panel1)
        Me.TabOptions.Location = New System.Drawing.Point(4, 22)
        Me.TabOptions.Name = "TabOptions"
        Me.TabOptions.Padding = New System.Windows.Forms.Padding(3)
        Me.TabOptions.Size = New System.Drawing.Size(782, 252)
        Me.TabOptions.TabIndex = 1
        Me.TabOptions.Text = "Options"
        Me.TabOptions.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CbxUseYear)
        Me.Panel1.Controls.Add(Me.CbxUseGenres)
        Me.Panel1.Controls.Add(Me.CbxGenres)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.TxtImportFolder)
        Me.Panel1.Controls.Add(Me.BtnSelectImportFolder)
        Me.Panel1.Controls.Add(Me.LblImportFolder)
        Me.Panel1.Controls.Add(Me.CkBxUseParens)
        Me.Panel1.Controls.Add(Me.CkBxUseBracket)
        Me.Panel1.Controls.Add(Me.txtFolderPath)
        Me.Panel1.Controls.Add(Me.BtnSelectFolder)
        Me.Panel1.Controls.Add(Me.LblRecordCount)
        Me.Panel1.Controls.Add(Me.LblMovieFolder)
        Me.Panel1.Controls.Add(Me.LblFolderCount)
        Me.Panel1.Location = New System.Drawing.Point(7, 7)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(769, 239)
        Me.Panel1.TabIndex = 0
        '
        'CbxUseYear
        '
        Me.CbxUseYear.AutoSize = True
        Me.CbxUseYear.Location = New System.Drawing.Point(79, 206)
        Me.CbxUseYear.Name = "CbxUseYear"
        Me.CbxUseYear.Size = New System.Drawing.Size(70, 17)
        Me.CbxUseYear.TabIndex = 34
        Me.CbxUseYear.Text = "Use Year"
        Me.CbxUseYear.UseVisualStyleBackColor = True
        '
        'CbxUseGenres
        '
        Me.CbxUseGenres.AutoSize = True
        Me.CbxUseGenres.Location = New System.Drawing.Point(79, 155)
        Me.CbxUseGenres.Name = "CbxUseGenres"
        Me.CbxUseGenres.Size = New System.Drawing.Size(82, 17)
        Me.CbxUseGenres.TabIndex = 33
        Me.CbxUseGenres.Text = "Use Genres"
        Me.CbxUseGenres.UseVisualStyleBackColor = True
        '
        'CbxGenres
        '
        Me.CbxGenres.AutoCompleteCustomSource.AddRange(New String() {"1", "2", "3", "4"})
        Me.CbxGenres.FormattingEnabled = True
        Me.CbxGenres.Location = New System.Drawing.Point(228, 178)
        Me.CbxGenres.Name = "CbxGenres"
        Me.CbxGenres.Size = New System.Drawing.Size(121, 21)
        Me.CbxGenres.TabIndex = 32
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(77, 181)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 13)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "Number of Genres to use:"
        '
        'TxtImportFolder
        '
        Me.TxtImportFolder.Location = New System.Drawing.Point(79, 52)
        Me.TxtImportFolder.Name = "TxtImportFolder"
        Me.TxtImportFolder.Size = New System.Drawing.Size(314, 20)
        Me.TxtImportFolder.TabIndex = 29
        '
        'BtnSelectImportFolder
        '
        Me.BtnSelectImportFolder.Location = New System.Drawing.Point(396, 52)
        Me.BtnSelectImportFolder.Name = "BtnSelectImportFolder"
        Me.BtnSelectImportFolder.Size = New System.Drawing.Size(21, 23)
        Me.BtnSelectImportFolder.TabIndex = 28
        Me.BtnSelectImportFolder.Text = "..."
        Me.BtnSelectImportFolder.UseVisualStyleBackColor = True
        '
        'LblImportFolder
        '
        Me.LblImportFolder.AutoSize = True
        Me.LblImportFolder.Location = New System.Drawing.Point(6, 52)
        Me.LblImportFolder.Name = "LblImportFolder"
        Me.LblImportFolder.Size = New System.Drawing.Size(71, 13)
        Me.LblImportFolder.TabIndex = 30
        Me.LblImportFolder.Text = "Import Folder:"
        '
        'CkBxUseParens
        '
        Me.CkBxUseParens.AutoSize = True
        Me.CkBxUseParens.Location = New System.Drawing.Point(166, 132)
        Me.CkBxUseParens.Name = "CkBxUseParens"
        Me.CkBxUseParens.Size = New System.Drawing.Size(77, 17)
        Me.CkBxUseParens.TabIndex = 27
        Me.CkBxUseParens.Text = "Use (Date)"
        Me.CkBxUseParens.UseVisualStyleBackColor = True
        '
        'CkBxUseBracket
        '
        Me.CkBxUseBracket.AutoSize = True
        Me.CkBxUseBracket.Location = New System.Drawing.Point(80, 132)
        Me.CkBxUseBracket.Name = "CkBxUseBracket"
        Me.CkBxUseBracket.Size = New System.Drawing.Size(77, 17)
        Me.CkBxUseBracket.TabIndex = 26
        Me.CkBxUseBracket.Text = "Use [Date]"
        Me.CkBxUseBracket.UseVisualStyleBackColor = True
        '
        'txtFolderPath
        '
        Me.txtFolderPath.Location = New System.Drawing.Point(79, 12)
        Me.txtFolderPath.Name = "txtFolderPath"
        Me.txtFolderPath.Size = New System.Drawing.Size(314, 20)
        Me.txtFolderPath.TabIndex = 22
        '
        'BtnSelectFolder
        '
        Me.BtnSelectFolder.Location = New System.Drawing.Point(396, 12)
        Me.BtnSelectFolder.Name = "BtnSelectFolder"
        Me.BtnSelectFolder.Size = New System.Drawing.Size(21, 23)
        Me.BtnSelectFolder.TabIndex = 21
        Me.BtnSelectFolder.Text = "..."
        Me.BtnSelectFolder.UseVisualStyleBackColor = True
        '
        'LblRecordCount
        '
        Me.LblRecordCount.AutoSize = True
        Me.LblRecordCount.Location = New System.Drawing.Point(152, 94)
        Me.LblRecordCount.Name = "LblRecordCount"
        Me.LblRecordCount.Size = New System.Drawing.Size(0, 13)
        Me.LblRecordCount.TabIndex = 25
        '
        'LblMovieFolder
        '
        Me.LblMovieFolder.AutoSize = True
        Me.LblMovieFolder.Location = New System.Drawing.Point(6, 12)
        Me.LblMovieFolder.Name = "LblMovieFolder"
        Me.LblMovieFolder.Size = New System.Drawing.Size(71, 13)
        Me.LblMovieFolder.TabIndex = 23
        Me.LblMovieFolder.Text = "Movie Folder:"
        '
        'LblFolderCount
        '
        Me.LblFolderCount.AutoSize = True
        Me.LblFolderCount.Location = New System.Drawing.Point(76, 94)
        Me.LblFolderCount.Name = "LblFolderCount"
        Me.LblFolderCount.Size = New System.Drawing.Size(70, 13)
        Me.LblFolderCount.TabIndex = 24
        Me.LblFolderCount.Text = "Folder Count:"
        '
        'FrmKodiBuddy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(814, 303)
        Me.Controls.Add(Me.TabControl)
        Me.Name = "FrmKodiBuddy"
        Me.Text = "Movie Folder Cleaner"
        Me.TabControl.ResumeLayout(False)
        Me.TabExecute.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.TabOptions.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FolderBrowserDialog As FolderBrowserDialog
    Friend WithEvents TabControl As TabControl
    Friend WithEvents TabExecute As TabPage
    Friend WithEvents TabOptions As TabPage
    Friend WithEvents Panel2 As Panel
    Friend WithEvents BtnVideoImport As Button
    Friend WithEvents BtnCancel As Button
    Friend WithEvents BtnMovieDBUpdate As Button
    Friend WithEvents BtnStart As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TxtImportFolder As TextBox
    Friend WithEvents BtnSelectImportFolder As Button
    Friend WithEvents LblImportFolder As Label
    Friend WithEvents CkBxUseParens As CheckBox
    Friend WithEvents CkBxUseBracket As CheckBox
    Friend WithEvents txtFolderPath As TextBox
    Friend WithEvents BtnSelectFolder As Button
    Friend WithEvents LblRecordCount As Label
    Friend WithEvents LblMovieFolder As Label
    Friend WithEvents LblFolderCount As Label
    Friend WithEvents TxtMessageDisplay As TextBox
    Friend WithEvents BtnFileReName As Button
    Friend WithEvents BtnRemap As Button
    Friend WithEvents CbxGenres As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents CbxUseYear As CheckBox
    Friend WithEvents CbxUseGenres As CheckBox
    Friend WithEvents LblCurrentDir As Label
End Class
