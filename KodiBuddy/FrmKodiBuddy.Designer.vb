﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.BtnMasterList = New System.Windows.Forms.Button()
        Me.LblFolderCounts = New System.Windows.Forms.Label()
        Me.ErrorTabControl = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TxtMessageDisplay = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TxtErrorMessage = New System.Windows.Forms.TextBox()
        Me.LblCurrentDir = New System.Windows.Forms.Label()
        Me.BtnRemapFolders = New System.Windows.Forms.Button()
        Me.BtnMovieFileReName = New System.Windows.Forms.Button()
        Me.BtnVideoImport = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.TabOptions = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CancelBtn = New System.Windows.Forms.Button()
        Me.SaveBtn = New System.Windows.Forms.Button()
        Me.TxtOptionDescription = New System.Windows.Forms.TextBox()
        Me.txtTVFolderPath = New System.Windows.Forms.TextBox()
        Me.BtnSelectTVFolder = New System.Windows.Forms.Button()
        Me.LblTVOutputFolder = New System.Windows.Forms.Label()
        Me.CbxUseYear = New System.Windows.Forms.CheckBox()
        Me.CbxUseGenres = New System.Windows.Forms.CheckBox()
        Me.CbxGenres = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtImportFolder = New System.Windows.Forms.TextBox()
        Me.BtnSelectImportFolder = New System.Windows.Forms.Button()
        Me.LblImportFolder = New System.Windows.Forms.Label()
        Me.txtMovieFolderPath = New System.Windows.Forms.TextBox()
        Me.BtnSelectFolder = New System.Windows.Forms.Button()
        Me.LblRecordCount = New System.Windows.Forms.Label()
        Me.LblMovieFolder = New System.Windows.Forms.Label()
        Me.LblFolderCount = New System.Windows.Forms.Label()
        Me.TabControl.SuspendLayout()
        Me.TabExecute.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ErrorTabControl.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
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
        Me.TabControl.Size = New System.Drawing.Size(790, 362)
        Me.TabControl.TabIndex = 15
        '
        'TabExecute
        '
        Me.TabExecute.Controls.Add(Me.Panel2)
        Me.TabExecute.Location = New System.Drawing.Point(4, 22)
        Me.TabExecute.Name = "TabExecute"
        Me.TabExecute.Padding = New System.Windows.Forms.Padding(3)
        Me.TabExecute.Size = New System.Drawing.Size(782, 336)
        Me.TabExecute.TabIndex = 0
        Me.TabExecute.Text = "KodiBuddy"
        Me.TabExecute.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.TxtDescription)
        Me.Panel2.Controls.Add(Me.BtnMasterList)
        Me.Panel2.Controls.Add(Me.LblFolderCounts)
        Me.Panel2.Controls.Add(Me.ErrorTabControl)
        Me.Panel2.Controls.Add(Me.LblCurrentDir)
        Me.Panel2.Controls.Add(Me.BtnRemapFolders)
        Me.Panel2.Controls.Add(Me.BtnMovieFileReName)
        Me.Panel2.Controls.Add(Me.BtnVideoImport)
        Me.Panel2.Controls.Add(Me.BtnCancel)
        Me.Panel2.Location = New System.Drawing.Point(7, 7)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(769, 323)
        Me.Panel2.TabIndex = 19
        '
        'TxtDescription
        '
        Me.TxtDescription.BackColor = System.Drawing.Color.White
        Me.TxtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtDescription.Enabled = False
        Me.TxtDescription.Location = New System.Drawing.Point(151, 249)
        Me.TxtDescription.Multiline = True
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.Size = New System.Drawing.Size(608, 71)
        Me.TxtDescription.TabIndex = 30
        '
        'BtnMasterList
        '
        Me.BtnMasterList.Location = New System.Drawing.Point(17, 112)
        Me.BtnMasterList.Name = "BtnMasterList"
        Me.BtnMasterList.Size = New System.Drawing.Size(122, 23)
        Me.BtnMasterList.TabIndex = 29
        Me.BtnMasterList.Text = "Master List"
        Me.BtnMasterList.UseVisualStyleBackColor = True
        '
        'LblFolderCounts
        '
        Me.LblFolderCounts.AutoSize = True
        Me.LblFolderCounts.Location = New System.Drawing.Point(149, 216)
        Me.LblFolderCounts.Name = "LblFolderCounts"
        Me.LblFolderCounts.Size = New System.Drawing.Size(0, 13)
        Me.LblFolderCounts.TabIndex = 27
        '
        'ErrorTabControl
        '
        Me.ErrorTabControl.Controls.Add(Me.TabPage1)
        Me.ErrorTabControl.Controls.Add(Me.TabPage2)
        Me.ErrorTabControl.Location = New System.Drawing.Point(147, 3)
        Me.ErrorTabControl.Name = "ErrorTabControl"
        Me.ErrorTabControl.SelectedIndex = 0
        Me.ErrorTabControl.Size = New System.Drawing.Size(619, 210)
        Me.ErrorTabControl.TabIndex = 26
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TxtMessageDisplay)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(611, 184)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Processed"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TxtMessageDisplay
        '
        Me.TxtMessageDisplay.Location = New System.Drawing.Point(1, 3)
        Me.TxtMessageDisplay.Multiline = True
        Me.TxtMessageDisplay.Name = "TxtMessageDisplay"
        Me.TxtMessageDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TxtMessageDisplay.Size = New System.Drawing.Size(607, 178)
        Me.TxtMessageDisplay.TabIndex = 23
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TxtErrorMessage)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(611, 184)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Errors"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TxtErrorMessage
        '
        Me.TxtErrorMessage.Location = New System.Drawing.Point(2, 2)
        Me.TxtErrorMessage.Multiline = True
        Me.TxtErrorMessage.Name = "TxtErrorMessage"
        Me.TxtErrorMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TxtErrorMessage.Size = New System.Drawing.Size(607, 175)
        Me.TxtErrorMessage.TabIndex = 23
        '
        'LblCurrentDir
        '
        Me.LblCurrentDir.AutoSize = True
        Me.LblCurrentDir.Location = New System.Drawing.Point(156, 214)
        Me.LblCurrentDir.Name = "LblCurrentDir"
        Me.LblCurrentDir.Size = New System.Drawing.Size(0, 13)
        Me.LblCurrentDir.TabIndex = 25
        '
        'BtnRemapFolders
        '
        Me.BtnRemapFolders.Location = New System.Drawing.Point(17, 54)
        Me.BtnRemapFolders.Name = "BtnRemapFolders"
        Me.BtnRemapFolders.Size = New System.Drawing.Size(122, 23)
        Me.BtnRemapFolders.TabIndex = 24
        Me.BtnRemapFolders.Text = "ReMap Folders"
        Me.BtnRemapFolders.UseVisualStyleBackColor = True
        '
        'BtnMovieFileReName
        '
        Me.BtnMovieFileReName.Location = New System.Drawing.Point(17, 83)
        Me.BtnMovieFileReName.Name = "BtnMovieFileReName"
        Me.BtnMovieFileReName.Size = New System.Drawing.Size(122, 23)
        Me.BtnMovieFileReName.TabIndex = 23
        Me.BtnMovieFileReName.Text = "FileReName"
        Me.BtnMovieFileReName.UseVisualStyleBackColor = True
        '
        'BtnVideoImport
        '
        Me.BtnVideoImport.Location = New System.Drawing.Point(17, 25)
        Me.BtnVideoImport.Name = "BtnVideoImport"
        Me.BtnVideoImport.Size = New System.Drawing.Size(122, 23)
        Me.BtnVideoImport.TabIndex = 21
        Me.BtnVideoImport.Text = "Video Import"
        Me.BtnVideoImport.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(17, 141)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(122, 23)
        Me.BtnCancel.TabIndex = 20
        Me.BtnCancel.Text = "Exit"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'TabOptions
        '
        Me.TabOptions.Controls.Add(Me.Panel1)
        Me.TabOptions.Location = New System.Drawing.Point(4, 22)
        Me.TabOptions.Name = "TabOptions"
        Me.TabOptions.Padding = New System.Windows.Forms.Padding(3)
        Me.TabOptions.Size = New System.Drawing.Size(782, 336)
        Me.TabOptions.TabIndex = 1
        Me.TabOptions.Text = "Options"
        Me.TabOptions.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CancelBtn)
        Me.Panel1.Controls.Add(Me.SaveBtn)
        Me.Panel1.Controls.Add(Me.TxtOptionDescription)
        Me.Panel1.Controls.Add(Me.txtTVFolderPath)
        Me.Panel1.Controls.Add(Me.BtnSelectTVFolder)
        Me.Panel1.Controls.Add(Me.LblTVOutputFolder)
        Me.Panel1.Controls.Add(Me.CbxUseYear)
        Me.Panel1.Controls.Add(Me.CbxUseGenres)
        Me.Panel1.Controls.Add(Me.CbxGenres)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.TxtImportFolder)
        Me.Panel1.Controls.Add(Me.BtnSelectImportFolder)
        Me.Panel1.Controls.Add(Me.LblImportFolder)
        Me.Panel1.Controls.Add(Me.txtMovieFolderPath)
        Me.Panel1.Controls.Add(Me.BtnSelectFolder)
        Me.Panel1.Controls.Add(Me.LblRecordCount)
        Me.Panel1.Controls.Add(Me.LblMovieFolder)
        Me.Panel1.Controls.Add(Me.LblFolderCount)
        Me.Panel1.Location = New System.Drawing.Point(7, 7)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(769, 323)
        Me.Panel1.TabIndex = 0
        '
        'CancelBtn
        '
        Me.CancelBtn.Location = New System.Drawing.Point(223, 218)
        Me.CancelBtn.Name = "CancelBtn"
        Me.CancelBtn.Size = New System.Drawing.Size(75, 23)
        Me.CancelBtn.TabIndex = 43
        Me.CancelBtn.Text = "Cancel"
        Me.CancelBtn.UseVisualStyleBackColor = True
        '
        'SaveBtn
        '
        Me.SaveBtn.Location = New System.Drawing.Point(119, 219)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(75, 23)
        Me.SaveBtn.TabIndex = 42
        Me.SaveBtn.Text = "Save"
        Me.SaveBtn.UseVisualStyleBackColor = True
        '
        'TxtOptionDescription
        '
        Me.TxtOptionDescription.BackColor = System.Drawing.Color.White
        Me.TxtOptionDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtOptionDescription.Cursor = System.Windows.Forms.Cursors.No
        Me.TxtOptionDescription.Enabled = False
        Me.TxtOptionDescription.Location = New System.Drawing.Point(119, 206)
        Me.TxtOptionDescription.Multiline = True
        Me.TxtOptionDescription.Name = "TxtOptionDescription"
        Me.TxtOptionDescription.Size = New System.Drawing.Size(635, 69)
        Me.TxtOptionDescription.TabIndex = 41
        '
        'txtTVFolderPath
        '
        Me.txtTVFolderPath.Location = New System.Drawing.Point(118, 93)
        Me.txtTVFolderPath.Name = "txtTVFolderPath"
        Me.txtTVFolderPath.Size = New System.Drawing.Size(314, 20)
        Me.txtTVFolderPath.TabIndex = 36
        '
        'BtnSelectTVFolder
        '
        Me.BtnSelectTVFolder.Location = New System.Drawing.Point(435, 93)
        Me.BtnSelectTVFolder.Name = "BtnSelectTVFolder"
        Me.BtnSelectTVFolder.Size = New System.Drawing.Size(21, 23)
        Me.BtnSelectTVFolder.TabIndex = 35
        Me.BtnSelectTVFolder.Text = "..."
        Me.BtnSelectTVFolder.UseVisualStyleBackColor = True
        '
        'LblTVOutputFolder
        '
        Me.LblTVOutputFolder.AutoSize = True
        Me.LblTVOutputFolder.Location = New System.Drawing.Point(6, 96)
        Me.LblTVOutputFolder.Name = "LblTVOutputFolder"
        Me.LblTVOutputFolder.Size = New System.Drawing.Size(91, 13)
        Me.LblTVOutputFolder.TabIndex = 37
        Me.LblTVOutputFolder.Text = "TV Output Folder:"
        '
        'CbxUseYear
        '
        Me.CbxUseYear.AutoSize = True
        Me.CbxUseYear.Location = New System.Drawing.Point(10, 193)
        Me.CbxUseYear.Name = "CbxUseYear"
        Me.CbxUseYear.Size = New System.Drawing.Size(70, 17)
        Me.CbxUseYear.TabIndex = 34
        Me.CbxUseYear.Text = "Use Year"
        Me.CbxUseYear.UseVisualStyleBackColor = True
        '
        'CbxUseGenres
        '
        Me.CbxUseGenres.AutoSize = True
        Me.CbxUseGenres.Location = New System.Drawing.Point(11, 145)
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
        Me.CbxGenres.Location = New System.Drawing.Point(159, 162)
        Me.CbxGenres.Name = "CbxGenres"
        Me.CbxGenres.Size = New System.Drawing.Size(121, 21)
        Me.CbxGenres.TabIndex = 32
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 165)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 13)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "Number of Genres to use:"
        '
        'TxtImportFolder
        '
        Me.TxtImportFolder.Location = New System.Drawing.Point(118, 13)
        Me.TxtImportFolder.Name = "TxtImportFolder"
        Me.TxtImportFolder.Size = New System.Drawing.Size(314, 20)
        Me.TxtImportFolder.TabIndex = 29
        '
        'BtnSelectImportFolder
        '
        Me.BtnSelectImportFolder.Location = New System.Drawing.Point(435, 13)
        Me.BtnSelectImportFolder.Name = "BtnSelectImportFolder"
        Me.BtnSelectImportFolder.Size = New System.Drawing.Size(21, 23)
        Me.BtnSelectImportFolder.TabIndex = 28
        Me.BtnSelectImportFolder.Text = "..."
        Me.BtnSelectImportFolder.UseVisualStyleBackColor = True
        '
        'LblImportFolder
        '
        Me.LblImportFolder.AutoSize = True
        Me.LblImportFolder.Location = New System.Drawing.Point(6, 16)
        Me.LblImportFolder.Name = "LblImportFolder"
        Me.LblImportFolder.Size = New System.Drawing.Size(71, 13)
        Me.LblImportFolder.TabIndex = 30
        Me.LblImportFolder.Text = "Import Folder:"
        '
        'txtMovieFolderPath
        '
        Me.txtMovieFolderPath.Location = New System.Drawing.Point(118, 51)
        Me.txtMovieFolderPath.Name = "txtMovieFolderPath"
        Me.txtMovieFolderPath.Size = New System.Drawing.Size(314, 20)
        Me.txtMovieFolderPath.TabIndex = 22
        '
        'BtnSelectFolder
        '
        Me.BtnSelectFolder.Location = New System.Drawing.Point(435, 51)
        Me.BtnSelectFolder.Name = "BtnSelectFolder"
        Me.BtnSelectFolder.Size = New System.Drawing.Size(21, 23)
        Me.BtnSelectFolder.TabIndex = 21
        Me.BtnSelectFolder.Text = "..."
        Me.BtnSelectFolder.UseVisualStyleBackColor = True
        '
        'LblRecordCount
        '
        Me.LblRecordCount.AutoSize = True
        Me.LblRecordCount.Location = New System.Drawing.Point(83, 107)
        Me.LblRecordCount.Name = "LblRecordCount"
        Me.LblRecordCount.Size = New System.Drawing.Size(0, 13)
        Me.LblRecordCount.TabIndex = 25
        '
        'LblMovieFolder
        '
        Me.LblMovieFolder.AutoSize = True
        Me.LblMovieFolder.Location = New System.Drawing.Point(6, 54)
        Me.LblMovieFolder.Name = "LblMovieFolder"
        Me.LblMovieFolder.Size = New System.Drawing.Size(106, 13)
        Me.LblMovieFolder.TabIndex = 23
        Me.LblMovieFolder.Text = "Movie Output Folder:"
        '
        'LblFolderCount
        '
        Me.LblFolderCount.AutoSize = True
        Me.LblFolderCount.Location = New System.Drawing.Point(7, 123)
        Me.LblFolderCount.Name = "LblFolderCount"
        Me.LblFolderCount.Size = New System.Drawing.Size(70, 13)
        Me.LblFolderCount.TabIndex = 24
        Me.LblFolderCount.Text = "Folder Count:"
        '
        'FrmKodiBuddy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(814, 387)
        Me.Controls.Add(Me.TabControl)
        Me.Name = "FrmKodiBuddy"
        Me.Text = "KodiBuddy"
        Me.TabControl.ResumeLayout(False)
        Me.TabExecute.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ErrorTabControl.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
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
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TxtImportFolder As TextBox
    Friend WithEvents BtnSelectImportFolder As Button
    Friend WithEvents LblImportFolder As Label
    Friend WithEvents txtMovieFolderPath As TextBox
    Friend WithEvents BtnSelectFolder As Button
    Friend WithEvents LblRecordCount As Label
    Friend WithEvents LblMovieFolder As Label
    Friend WithEvents LblFolderCount As Label
    Friend WithEvents BtnMovieFileReName As Button
    Friend WithEvents BtnRemapFolders As Button
    Friend WithEvents CbxGenres As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents CbxUseYear As CheckBox
    Friend WithEvents CbxUseGenres As CheckBox
    Friend WithEvents LblCurrentDir As Label
    Friend WithEvents ErrorTabControl As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TxtMessageDisplay As TextBox
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TxtErrorMessage As TextBox
    Friend WithEvents LblFolderCounts As Label
    Friend WithEvents BtnMasterList As Button
    Friend WithEvents txtTVFolderPath As TextBox
    Friend WithEvents BtnSelectTVFolder As Button
    Friend WithEvents LblTVOutputFolder As Label
    Friend WithEvents TxtDescription As TextBox
    Friend WithEvents TxtOptionDescription As TextBox
    Friend WithEvents SaveBtn As Button
    Friend WithEvents CancelBtn As Button
End Class
