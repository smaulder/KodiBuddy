﻿Option Explicit On
Option Strict On

Imports System.IO
Imports System.Xml
Imports KodiBuddy.Common
Imports KodiBuddy.Common.Functions
Imports KodiBuddy.Common.Info
Imports System.Text.RegularExpressions

Public Class FrmKodiBuddy

    Public KodiBuddyLibrary As New Collection()
    Public KodiSettings As Common.KodiBuddyData = LoadXML()
    Public Dirs() As String

    Private _ProcessingStatus As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Property ProcessingStatus() As Boolean
        Get
            Return _ProcessingStatus
        End Get
        Set(ByVal value As Boolean)
            _ProcessingStatus = value
        End Set
    End Property

#Region "Options"

    Private Sub BtnSelectFolder_Click(sender As Object, e As EventArgs) Handles BtnSelectFolder.Click
        FolderBrowserDialog.SelectedPath = KodiSettings.MovieOutPutPath
        FolderBrowserDialog.ShowDialog()
        txtMovieFolderPath.Text = FolderBrowserDialog.SelectedPath
        KodiSettings.MovieOutPutPath = FolderBrowserDialog.SelectedPath
    End Sub

    Private Sub BtnSelectImportFolder_Click(sender As Object, e As EventArgs) Handles BtnSelectImportFolder.Click
        FolderBrowserDialog.SelectedPath = KodiSettings.ImportPath
        FolderBrowserDialog.ShowDialog()
        TxtImportFolder.Text = FolderBrowserDialog.SelectedPath
        KodiSettings.ImportPath = FolderBrowserDialog.SelectedPath
    End Sub

    Private Sub txtFolderPath_TextChanged(sender As Object, e As EventArgs) Handles txtMovieFolderPath.TextChanged
        If KodiSettings.MovieOutPutPath <> txtMovieFolderPath.Text Then
            KodiSettings.MovieOutPutPath = txtMovieFolderPath.Text
        End If
    End Sub

    Private Sub TxtImportFolder_TextChanged(sender As Object, e As EventArgs) Handles TxtImportFolder.TextChanged
        If KodiSettings.ImportPath = txtMovieFolderPath.Text Then
            KodiSettings.ImportPath = txtMovieFolderPath.Text
        End If
    End Sub

    Private Sub CbxGenres_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxGenres.SelectedIndexChanged
        KodiSettings.NoOfGenres = CType(CbxGenres.SelectedItem, String)
    End Sub

    Private Sub CbxUseGenres_CheckedChanged(sender As Object, e As EventArgs) Handles CbxUseGenres.CheckedChanged
        If CbxUseGenres.Checked And CbxUseYear.Checked Then
            CbxUseYear.Checked = False
            KodiSettings.GenreOrYear = "1"
        End If
    End Sub

    Private Sub CbxUseYear_CheckedChanged(sender As Object, e As EventArgs) Handles CbxUseYear.CheckedChanged
        If CbxUseGenres.Checked And CbxUseYear.Checked Then
            CbxUseGenres.Checked = False
            KodiSettings.GenreOrYear = "2"
            SaveXML(KodiSettings)
        End If

    End Sub

#End Region

    Private Sub FrmKodiBuddy_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Load settings from settings from XML
        TxtImportFolder.Text = KodiSettings.ImportPath
        txtMovieFolderPath.Text = KodiSettings.MovieOutPutPath
        'TxtTVImportFolder.Text = KodiSettings.TVImportPath
        txtTVFolderPath.Text = KodiSettings.TVOutPutPath
        CbxGenres.Items.Add("1")
        CbxGenres.Items.Add("2")
        CbxGenres.Items.Add("3")
        CbxGenres.Items.Add("4")
        CbxGenres.SelectedItem = KodiSettings.NoOfGenres
        If KodiSettings.GenreOrYear = "2" Then
            CbxUseYear.Checked = True
        Else
            CbxUseGenres.Checked = True
        End If
    End Sub


#Region "Functions"

    Private Sub BtnVideoImport_MouseHover(sender As Object, e As EventArgs) Handles BtnVideoImport.MouseHover
        If Not ProcessingStatus Then
            TxtDescription.Text = "Video Import does a TheMovieDB.org lookup on folders in the movie import folder path you have set in the options tab and cleans up the folder and file names to match and then copies the folder to the movie folder path under the genre associated with the movie."
        End If
    End Sub

    Private Sub BtnVideoImport_Mouseleave(sender As Object, e As EventArgs) Handles BtnVideoImport.MouseLeave
        If Not ProcessingStatus Then
            TxtDescription.Text = ""
        End If
    End Sub

    Private Sub BtnVideoImport_Click(sender As Object, e As EventArgs) Handles BtnVideoImport.Click
        Dim DirList As New ArrayList
        Dim ResultName As String = ""
        Dim Resultant As String = ""
        Dim errorCount As Integer = 0

        ProcessingStatus = True

        Try
            BtnVideoImport.Text = "Processing"
            BtnVideoImport.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(TxtImportFolder.Text, TxtErrorMessage)

            TxtMessageDisplay.Text = "Starting processing on " & Dirs.Count & " folders to import." & vbCrLf
            Me.Refresh()
            For i As Integer = Dirs.Count - 1 To 0 Step -1
                Try
                    'bNeedYear = False
                    Dim DirPath As String = Dirs(i)
                    LblFolderCounts.Text = "Processing folder " & (Dirs.Count - i) & " of " & Dirs.Count & " - " & errorCount & " Errors."

                    If IsTVShow(DirPath) Then
                        ProcessTVDirectory(DirPath, KodiSettings, txtTVFolderPath, txtMovieFolderPath, TxtMessageDisplay, TxtErrorMessage)
                        Me.Refresh()
                    Else
                        ProcessMovieDirectory(DirPath, KodiSettings, txtTVFolderPath, txtMovieFolderPath, TxtMessageDisplay, TxtErrorMessage)
                        Me.Refresh()
                    End If
                Catch ex As Exception
                    errorCount = errorCount + 1
                    TxtMessageDisplay.Text = ex.Message & vbCrLf & TxtMessageDisplay.Text
                End Try
            Next i

            LblFolderCounts.Text = "Processed " & (Dirs.Count) & "  folders " & " - " & errorCount & " Errors."

            TxtMessageDisplay.Text = "Processing done on " & Dirs.Count & " Folders." & vbCrLf & TxtMessageDisplay.Text
            Me.Refresh()

        Catch ex As Exception
            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
        End Try
        BtnVideoImport.Text = "Video Import"
        BtnVideoImport.Enabled = True
        ProcessingStatus = False
    End Sub

    Private Sub BtnMovieFileReName_MouseHover(sender As Object, e As EventArgs) Handles BtnMovieFileReName.MouseHover
        If Not ProcessingStatus Then
            TxtDescription.Text = "File ReName does a TheMovieDB.org lookup on folders in the movie import folder path you have set in the options tab and cleans up the folder and file names to match and then copies the folder to the movie folder path under the genre associated with the movie."
        End If
    End Sub

    Private Sub BtnMovieFileReName_Mouseleave(sender As Object, e As EventArgs) Handles BtnMovieFileReName.MouseLeave
        If Not ProcessingStatus Then
            TxtDescription.Text = ""
        End If
    End Sub

    Private Sub BtnMovieFileReName_Click(sender As Object, e As EventArgs) Handles BtnMovieFileReName.Click
        Dim DirList As New ArrayList
        Dim ResultName As String = ""
        Dim Resultant As String = ""
        Dim bNeedYear As Boolean = False
        Dim errorCount As Integer = 0
        ProcessingStatus = True

        Try
            BtnMovieFileReName.Text = "Processing"
            BtnMovieFileReName.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(txtMovieFolderPath.Text, TxtErrorMessage)

            TxtMessageDisplay.Text = "Starting processing on " & Dirs.Count & " folders to import." & vbCrLf
            Me.Refresh()

            For i As Integer = Dirs.Count - 1 To 0 Step -1
                bNeedYear = False
                Dim Dir As String = Dirs(i)
                'skip System Volume Information
                If Not Dir.Contains("System Volume Information") Then
                    LblFolderCounts.Text = "Processing folder " & (Dirs.Count - i) & " of " & Dirs.Count & " - " & errorCount & " Errors."
                    Dim shortMovieName As String = ""
                    Dim bUpdateFolderName As Boolean = False
                    Dim MovieName As String = StrConv(Dir.Remove(0, Dir.LastIndexOf("\") + 1).Replace("_", " "), VbStrConv.ProperCase)
                    Dim OriginalMovieName As String = Dir.Remove(0, Dir.LastIndexOf("\") + 1)
                    Dim movieyear As String = Dir.Remove(0, Dir.LastIndexOf("(") + 1).Trim().Remove(4)
                    If Not IsNumeric(movieyear) OrElse (CInt(movieyear) < 1900 Or CInt(movieyear) > Year(Now)) Then
                        movieyear = ""
                        bNeedYear = True
                    End If
                    Dim iPos = MovieName.LastIndexOf(")") + 1
                    If (iPos > 0 AndAlso iPos < MovieName.Length) OrElse (String.Compare(MovieName, OriginalMovieName, False) <> 0) Then bUpdateFolderName = True Else bUpdateFolderName = False
                    If iPos > 0 AndAlso Not MovieName.Length = iPos Then
                        MovieName = MovieName.Remove(iPos, MovieName.Length - iPos)
                    End If

                    Try
                        If MovieName.LastIndexOf("(") > 0 Then
                            shortMovieName = MovieName.Remove(MovieName.LastIndexOf("("), MovieName.LastIndexOf(")") - MovieName.LastIndexOf("(") + 1).Trim()
                        Else
                            shortMovieName = MovieName
                        End If

                        Dim pMovieInfo As Common.Info = getMovieInfo(2, Dir, movieyear, TxtErrorMessage, shortMovieName)
                        If pMovieInfo.Success Then

                            If bNeedYear Then
                                MovieName = MovieName & " (" & pMovieInfo.ReleaseYear & ")"
                            End If

                            'Rename Files Before Folder
                            Dim fileEntries As String() = Directory.GetFiles(Dir)
                            ' Process the list of files found in the directory.
                            Dim fileName As String
                            If fileEntries.Count > 0 Then
                                For Each fileName In fileEntries
                                    Dim x As FileInfo = New FileInfo(fileName)
                                    If UCase(x.Extension.ToString()) = UCase(".mp4") Or UCase(x.Extension.ToString()) = UCase(".avi") Or UCase(x.Extension.ToString()) = UCase(".mkv") Or UCase(x.Extension.ToString()) = UCase(".srt") Then

                                        Dim tmpFileName As String = fileName.Remove(0, fileName.LastIndexOf("\") + 1)
                                        Dim iPos1 As Integer = tmpFileName.LastIndexOf(pMovieInfo.ReleaseYear) - 1
                                        Dim iPos2 As Integer = tmpFileName.LastIndexOf(".")
                                        If iPos1 > 0 Then
                                            Dim tmpfile2 As String = pMovieInfo.Name & Microsoft.VisualBasic.Right(tmpFileName, tmpFileName.Length - iPos2)
                                            Try
                                                If Not File.Exists(tmpFileName) AndAlso fileName.Remove(0, fileName.LastIndexOf("\") + 1) <> tmpfile2 Then
                                                    My.Computer.FileSystem.RenameFile(fileName, tmpfile2)
                                                End If
                                            Catch ex As Exception
                                                If Not ex.Message.Contains("since a file already exists") Then
                                                    TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                                                End If
                                            End Try
                                        Else
                                            tmpFileName = pMovieInfo.Name & x.Extension
                                            Try
                                                If Not File.Exists(tmpFileName) AndAlso fileName.Remove(0, fileName.LastIndexOf("\") + 1) <> tmpFileName Then
                                                    My.Computer.FileSystem.RenameFile(fileName, tmpFileName)
                                                End If
                                            Catch ex As Exception
                                                If Not ex.Message.Contains("since a file already exists") Then
                                                    TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                                                End If
                                            End Try
                                        End If
                                    Else
                                        'delete torrent file.
                                        If fileName.Contains("TorrentPartFile") AndAlso x.Extension = ".dat" Then
                                            My.Computer.FileSystem.DeleteFile(fileName)
                                            TxtMessageDisplay.Text = "Deleted File " & fileName & vbCrLf & TxtErrorMessage.Text
                                        End If
                                    End If

                                Next fileName

                                'Rename Folder
                                If bUpdateFolderName Then
                                    Try
                                        My.Computer.FileSystem.RenameDirectory(Dir, MovieName)
                                    Catch ex As Exception
                                        If Not ex.Message.Contains("since a directory already exists") Then
                                            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                                        End If
                                    End Try
                                End If

                                TxtMessageDisplay.Text = MovieName & vbCrLf & TxtMessageDisplay.Text
                                Me.Refresh()
                            End If

                            'Else
                            '    errorCount = errorCount + 1
                            '    TxtErrorMessage.Text = "Failed to load Movie info for - " & MovieName & vbCrLf & TxtErrorMessage.Text
                            '    Me.Refresh()
                        End If
                    Catch ex As Exception
                        errorCount = errorCount + 1
                        TxtMessageDisplay.Text = ex.Message & vbCrLf & TxtMessageDisplay.Text
                    End Try
                End If
            Next i

            LblFolderCounts.Text = "Processed " & (Dirs.Count) & "  folders " & " - " & errorCount & " Errors."

            TxtMessageDisplay.Text = "Processing done on " & Dirs.Count & " Folders." & vbCrLf & TxtMessageDisplay.Text
            Me.Refresh()

        Catch ex As Exception
            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
        End Try

        BtnMovieFileReName.Text = "FileReName"
        BtnMovieFileReName.Enabled = True
        ProcessingStatus = False
    End Sub

    Private Sub BtnRemapFolders_MouseHover(sender As Object, e As EventArgs) Handles BtnRemapFolders.MouseHover
        If Not ProcessingStatus Then
            TxtDescription.Text = "Re-map folder does a TheMovieDB.org lookup on folders in the movie import folder path you have set in the options tab and cleans up the folder and file names to match and then copies the folder to the movie folder path under the genre associated with the movie."
        End If
    End Sub

    Private Sub BtnRemapFolders_Mouseleave(sender As Object, e As EventArgs) Handles BtnRemapFolders.MouseLeave
        If Not ProcessingStatus Then
            TxtDescription.Text = ""
        End If
    End Sub

    Private Sub BtnRemapFolders_Click(sender As Object, e As EventArgs) Handles BtnRemapFolders.Click
        Dim DirList As New ArrayList
        Dim ResultName As String = ""
        Dim Resultant As String = ""
        Dim bNeedYear As Boolean = False
        Dim errorCount As Integer = 0
        ProcessingStatus = True
        Try
            BtnRemapFolders.Text = "Processing"
            BtnRemapFolders.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(txtMovieFolderPath.Text, TxtErrorMessage)

            LblFolderCounts.Text = "Starting processing on " & Dirs.Count & " folders to ReMap." & vbCrLf
            Me.Refresh()

            For i As Integer = Dirs.Count - 1 To 0 Step -1
                Dim Dir As String = Dirs(i)
                TxtMessageDisplay.Text = i & " Starting on directory " & Dir & vbCrLf & TxtMessageDisplay.Text

                Dim fileEntries As String() = Directory.GetFiles(Dir)
                If fileEntries.Count > 0 Then

                    Dim bUpdateFolderName As Boolean = False
                    Dim MovieName As String = ""
                    Dim movieyear As String = ""
                    Dim iPos As Integer = 0
                    Try
                        MovieName = Dir.Remove(0, Dir.LastIndexOf("\") + 1)
                    Catch ex As Exception
                        TxtErrorMessage.Text = "Error at 1"
                    End Try
                    Try
                        movieyear = Dir.Remove(0, Dir.LastIndexOf("(") + 1).Trim().Remove(4)
                    Catch ex As Exception
                        TxtErrorMessage.Text = "Error at 2"
                    End Try
                    Try
                        iPos = MovieName.LastIndexOf(")") + 1
                    Catch ex As Exception
                        TxtErrorMessage.Text = "Error at 3"
                    End Try
                    Try
                        If iPos <> 0 AndAlso iPos < MovieName.Length Then bUpdateFolderName = True Else bUpdateFolderName = False
                        If Not iPos = 0 AndAlso Not MovieName.Length = iPos Then
                            MovieName = MovieName.Remove(iPos, MovieName.Length - iPos)
                        End If
                    Catch ex As Exception
                        TxtErrorMessage.Text = "Error at 4"
                    End Try
                    Dim shortMovieName As String = ""
                    Try
                        'this is where the error hit on folder john adams Part 7
                        shortMovieName = MovieName.Remove(MovieName.LastIndexOf("("), MovieName.LastIndexOf(")") - MovieName.LastIndexOf("(") + 1).Trim()
                    Catch ex As Exception
                        shortMovieName = MovieName
                    End Try
                    Dim pMovieInfo As Common.Info = Nothing
                    Try
                        pMovieInfo = getMovieInfo(2, Dir, movieyear, TxtErrorMessage, shortMovieName)
                    Catch ex As Exception
                        TxtErrorMessage.Text = "Error at 6"
                    End Try

                    If pMovieInfo.Success Then
                        TxtMessageDisplay.Text = "Folder - " & MovieName & vbCrLf & TxtMessageDisplay.Text
                        Me.Refresh()

                        Dim Genres() As String = Nothing
                        Dim DestFolder As String = ""
                        Dim iCnt As Integer = 0
                        Try
                            'Use genres to move file to final folder.
                            Genres = Split(pMovieInfo.Genres, ",")
                            For Each Genre As String In Genres
                                DestFolder = DestFolder & "-" & Genre.Trim()
                                iCnt = iCnt + 1
                                If iCnt >= CInt(KodiSettings.NoOfGenres) Then
                                    Exit For
                                End If
                            Next
                        Catch ex As Exception
                            TxtErrorMessage.Text = "Error at 7"
                        End Try

                        If KodiSettings.GenreOrYear = "1" Then
                            If DestFolder = "-" Then
                                If Microsoft.VisualBasic.Right(txtMovieFolderPath.Text, 1) = "\" Then
                                    DestFolder = txtMovieFolderPath.Text & "Other"
                                Else
                                    DestFolder = txtMovieFolderPath.Text & "\" & "Other"
                                End If
                            Else
                                If Microsoft.VisualBasic.Right(txtMovieFolderPath.Text, 1) = "\" Then
                                    DestFolder = txtMovieFolderPath.Text & DestFolder.Remove(0, 1)
                                Else
                                    DestFolder = txtMovieFolderPath.Text & "\" & DestFolder.Remove(0, 1)
                                End If
                            End If
                        Else
                            If Microsoft.VisualBasic.Right(txtMovieFolderPath.Text, 1) = "\" Then
                                DestFolder = txtMovieFolderPath.Text & pMovieInfo.ReleaseYear
                            Else
                                DestFolder = txtMovieFolderPath.Text & "\" & pMovieInfo.ReleaseYear
                            End If
                        End If


                        'Create Destination Directory if missing 
                        If Not Directory.Exists(DestFolder) Then
                            Try
                                Dim di As DirectoryInfo = Directory.CreateDirectory(DestFolder)
                            Catch e2 As Exception
                                TxtErrorMessage.Text = e2.Message & vbCrLf & TxtErrorMessage.Text
                            End Try
                        End If

                        If Microsoft.VisualBasic.Right(DestFolder, 1) = "\" Then
                            DestFolder = DestFolder & MovieName
                        Else
                            DestFolder = DestFolder & "\" & MovieName
                        End If

                        If Not Directory.Exists(DestFolder) AndAlso Directory.Exists(Dir) Then
                            Try
                                Directory.Move(Dir, DestFolder)
                            Catch e2 As Exception
                                TxtErrorMessage.Text = e2.Message & vbCrLf & TxtErrorMessage.Text
                            End Try
                        End If

                        TxtMessageDisplay.Text = MovieName & vbCrLf & TxtMessageDisplay.Text
                        Me.Refresh()
                    End If
                End If
            Next i

            TxtMessageDisplay.Text = "Processing done on " & Dirs.Count & " Folders." & vbCrLf & TxtMessageDisplay.Text
            Me.Refresh()

        Catch ex As Exception
            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
        End Try
        BtnRemapFolders.Text = "ReMap Files"
        BtnRemapFolders.Enabled = True
        ProcessingStatus = False
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        End
    End Sub

#End Region


#Region "Common Methods"

    Private Sub BtnMasterList_Click(sender As Object, e As EventArgs) Handles BtnMasterList.Click

        Dim box = New FrmMasterList()
        box.Show()
    End Sub

    Private Sub BtnSelectTVFolder_Click(sender As Object, e As EventArgs) Handles BtnSelectTVFolder.Click
        FolderBrowserDialog.SelectedPath = KodiSettings.TVOutPutPath
        FolderBrowserDialog.ShowDialog()
        txtTVFolderPath.Text = FolderBrowserDialog.SelectedPath
        KodiSettings.TVOutPutPath = FolderBrowserDialog.SelectedPath
    End Sub

    Private Sub txtTVFolderPath_TextChanged(sender As Object, e As EventArgs) Handles txtTVFolderPath.TextChanged
        If KodiSettings.TVOutPutPath <> txtTVFolderPath.Text Then
            KodiSettings.TVOutPutPath = txtTVFolderPath.Text
        End If
    End Sub

    Private Sub LblMovieFolder_MouseHover(sender As Object, e As EventArgs) Handles LblMovieFolder.MouseHover
        TxtOptionDescription.Text = "Movie Output Folder"
    End Sub

    Private Sub LblMovieFolder_MouseLeave(sender As Object, e As EventArgs) Handles LblMovieFolder.MouseLeave
        TxtOptionDescription.Text = ""
    End Sub

    Private Sub LblImportFolder_MouseHover(sender As Object, e As EventArgs) Handles LblImportFolder.MouseHover
        TxtOptionDescription.Text = "Movie Import Folder"

    End Sub

    Private Sub LblImportFolder_MouseLeave(sender As Object, e As EventArgs) Handles LblImportFolder.MouseLeave
        TxtOptionDescription.Text = ""
    End Sub

    Private Sub LblTVOutputFolder_MouseLeave(sender As Object, e As EventArgs) Handles LblTVOutputFolder.MouseLeave
        TxtOptionDescription.Text = ""
    End Sub

    Private Sub LblTVOutputFolder_MouseHover(sender As Object, e As EventArgs) Handles LblTVOutputFolder.MouseHover
        TxtOptionDescription.Text = "TV Output Folder"
    End Sub

    Private Sub LblTVImportFolder_MouseLeave(sender As Object, e As EventArgs)
        TxtOptionDescription.Text = ""
    End Sub

    Private Sub LblTVImportFolder_MouseHover(sender As Object, e As EventArgs)
        TxtOptionDescription.Text = "TV Import Folder"
    End Sub


    Private Sub SaveBtn_Click(sender As Object, e As EventArgs) Handles SaveBtn.Click

        KodiSettings.ImportPath = TxtImportFolder.Text
        KodiSettings.MovieOutPutPath = txtMovieFolderPath.Text
        KodiSettings.TVOutPutPath = txtTVFolderPath.Text
        KodiSettings.NoOfGenres = CbxGenres.SelectedItem.ToString()
        If CbxUseYear.Checked = True Then
            KodiSettings.GenreOrYear = "2"
        Else
            KodiSettings.GenreOrYear = "1"
        End If
        SaveXML(KodiSettings)
    End Sub

    Private Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
        KodiSettings = LoadXML()
        TxtImportFolder.Text = KodiSettings.ImportPath
        txtMovieFolderPath.Text = KodiSettings.MovieOutPutPath
        txtTVFolderPath.Text = KodiSettings.TVOutPutPath
        CbxGenres.Items.Add("1")
        CbxGenres.Items.Add("2")
        CbxGenres.Items.Add("3")
        CbxGenres.Items.Add("4")
        CbxGenres.SelectedItem = KodiSettings.NoOfGenres
        If KodiSettings.GenreOrYear = "2" Then
            CbxUseYear.Checked = True
        Else
            CbxUseGenres.Checked = True
        End If
    End Sub


#End Region

End Class