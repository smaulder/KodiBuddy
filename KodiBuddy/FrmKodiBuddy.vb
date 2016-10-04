﻿Option Explicit On
Option Strict On

Imports System.IO
Imports System.Xml

Public Class FrmKodiBuddy

    Public KodiBuddyLibrary As New Collection()

    Public Dirs() As String

    Private _ProcessingStatus As Boolean = False
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
        FolderBrowserDialog.SelectedPath = My.Settings.MoviePath
        FolderBrowserDialog.ShowDialog()
        txtFolderPath.Text = FolderBrowserDialog.SelectedPath
        My.Settings.MoviePath = FolderBrowserDialog.SelectedPath
        My.Settings.Save()
    End Sub

    Private Sub BtnSelectImportFolder_Click(sender As Object, e As EventArgs) Handles BtnSelectImportFolder.Click
        FolderBrowserDialog.SelectedPath = My.Settings.MovieImportPath
        FolderBrowserDialog.ShowDialog()
        TxtImportFolder.Text = FolderBrowserDialog.SelectedPath
        My.Settings.MovieImportPath = FolderBrowserDialog.SelectedPath
        My.Settings.Save()
    End Sub

    Private Sub txtFolderPath_TextChanged(sender As Object, e As EventArgs) Handles txtFolderPath.TextChanged
        If My.Settings.MoviePath <> txtFolderPath.Text Then
            My.Settings.MoviePath = txtFolderPath.Text
            My.Settings.Save()
        End If
    End Sub

    Private Sub TxtImportFolder_TextChanged(sender As Object, e As EventArgs) Handles TxtImportFolder.TextChanged
        If My.Settings.MovieImportPath = txtFolderPath.Text Then
            My.Settings.MovieImportPath = txtFolderPath.Text
            My.Settings.Save()
        End If
    End Sub

    Private Sub CbxGenres_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxGenres.SelectedIndexChanged
        My.Settings.NoOfGenres = CType(CbxGenres.SelectedItem, String)
        My.Settings.Save()
    End Sub

    Private Sub CbxUseGenres_CheckedChanged(sender As Object, e As EventArgs) Handles CbxUseGenres.CheckedChanged
        If CbxUseGenres.Checked And CbxUseYear.Checked Then
            CbxUseYear.Checked = False
            My.Settings.GenreOrYear = "1"
            My.Settings.Save()
        End If
    End Sub

    Private Sub CbxUseYear_CheckedChanged(sender As Object, e As EventArgs) Handles CbxUseYear.CheckedChanged
        If CbxUseGenres.Checked And CbxUseYear.Checked Then
            CbxUseGenres.Checked = False
            My.Settings.GenreOrYear = "2"
            My.Settings.Save()
        End If

    End Sub

#End Region

    Private Sub FrmKodiBuddy_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Load settings from settings in app.config 
        TxtImportFolder.Text = My.Settings.MovieImportPath
        txtFolderPath.Text = My.Settings.MoviePath
        CbxGenres.Items.Add("1")
        CbxGenres.Items.Add("2")
        CbxGenres.Items.Add("3")
        CbxGenres.Items.Add("4")
        CbxGenres.SelectedItem = My.Settings.NoOfGenres
        If My.Settings.GenreOrYear = "2" Then
            CbxUseYear.Checked = True
        Else
            CbxUseGenres.Checked = True
        End If
    End Sub


#Region "Functions"

    Private Sub BtnVideoImport_MouseHover(sender As Object, e As EventArgs) Handles BtnVideoImport.MouseHover
        If Not ProcessingStatus Then
            LblDescription.Text = "Video Import does a TheMovieDB.org lookup on folders in the movie import folder path you have set in the options tab and cleans up the folder and file names to match and then copies the folder to the movie folder path under the genre associated with the movie."
        End If
    End Sub

    Private Sub BtnVideoImport_Mouseleave(sender As Object, e As EventArgs) Handles BtnVideoImport.MouseLeave
        If Not ProcessingStatus Then
            LblDescription.Text = ""
        End If
    End Sub

    Private Sub BtnVideoImport_Click(sender As Object, e As EventArgs) Handles BtnVideoImport.Click
        Dim DirList As New ArrayList
        Dim ResultName As String = ""
        Dim Resultant As String = ""
        Dim bNeedYear As Boolean = False
        Dim errorCount As Integer = 0
        ProcessingStatus = True

        Try
            BtnVideoImport.Text = "Processing"
            BtnVideoImport.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(TxtImportFolder.Text)

            TxtMessageDisplay.Text = "Starting processing on " & Dirs.Count & " folders to import." & vbCrLf
            Me.Refresh()
            For i As Integer = Dirs.Count - 1 To 0 Step -1
                bNeedYear = False
                Dim Dir As String = Dirs(i)
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

                    Dim pMovieInfo As MovieInfo = getMovieInfo(Dir, movieyear)
                    If pMovieInfo.Success Then

                        If bNeedYear Then
                            MovieName = MovieName & " (" & pMovieInfo.ReleaseYear & ")"
                            pMovieInfo.MovieName = MovieName
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
                                        Dim tmpfile2 As String = MovieName & Microsoft.VisualBasic.Right(tmpFileName, tmpFileName.Length - iPos2)

                                        Try
                                            If Not File.Exists(tmpfile2) AndAlso fileName.Remove(0, fileName.LastIndexOf("\") + 1) <> tmpfile2 Then
                                                My.Computer.FileSystem.RenameFile(fileName, tmpfile2)
                                            End If
                                        Catch ex As Exception
                                            If Not ex.Message.Contains("since a file already exists") Then
                                                TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                                            End If
                                        End Try
                                    Else
                                        tmpFileName = MovieName & x.Extension
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

                        End If

                        'Use genres to move file to final folder.
                        Dim Genres() As String = Split(pMovieInfo.Genres, ",")
                        Dim DestFolder As String = ""
                        Dim SourceFolder As String = ""
                        Dim iCnt As Integer = 0
                        For Each Genre As String In Genres
                            DestFolder = DestFolder & "-" & Genre.Trim()
                            iCnt = iCnt + 1
                            If iCnt >= CInt(My.Settings.NoOfGenres) Then
                                Exit For
                            End If
                        Next
                        If My.Settings.GenreOrYear = "1" Then
                            If DestFolder = "-" Then
                                If Microsoft.VisualBasic.Right(txtFolderPath.Text, 1) = "\" Then
                                    DestFolder = txtFolderPath.Text & "Other"
                                Else
                                    DestFolder = txtFolderPath.Text & "\" & "Other"
                                End If
                            Else
                                If Microsoft.VisualBasic.Right(txtFolderPath.Text, 1) = "\" Then
                                    DestFolder = txtFolderPath.Text & DestFolder.Remove(0, 1)
                                Else
                                    DestFolder = txtFolderPath.Text & "\" & DestFolder.Remove(0, 1)
                                End If
                            End If
                        Else
                            If Microsoft.VisualBasic.Right(txtFolderPath.Text, 1) = "\" Then
                                DestFolder = txtFolderPath.Text & pMovieInfo.ReleaseYear
                            Else
                                DestFolder = txtFolderPath.Text & "\" & pMovieInfo.ReleaseYear
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
                        Else
                            errorCount = errorCount + 1
                            'Message folder not found.
                            If Directory.Exists(DestFolder) Then
                                TxtErrorMessage.Text = "Duplicate folder exists. - " & DestFolder & vbCrLf & TxtErrorMessage.Text
                            End If
                            If Not Directory.Exists(Dir) Then
                                TxtErrorMessage.Text = "Source folder not found. - " & SourceFolder & vbCrLf & TxtErrorMessage.Text
                            End If
                        End If

                        TxtMessageDisplay.Text = MovieName & vbCrLf & TxtMessageDisplay.Text
                        Me.Refresh()
                    Else
                        errorCount = errorCount + 1
                        TxtErrorMessage.Text = "Failed to load Movie info for - " & MovieName & vbCrLf & TxtErrorMessage.Text
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

    Private Sub BtnFileReName_MouseHover(sender As Object, e As EventArgs) Handles BtnFileReName.MouseHover
        If Not ProcessingStatus Then
            LblDescription.Text = "File ReName does a TheMovieDB.org lookup on folders in the movie import folder path you have set in the options tab and cleans up the folder and file names to match and then copies the folder to the movie folder path under the genre associated with the movie."
        End If
    End Sub

    Private Sub BtnFileReName_Mouseleave(sender As Object, e As EventArgs) Handles BtnFileReName.MouseLeave
        If Not ProcessingStatus Then
            LblDescription.Text = ""
        End If
    End Sub

    Private Sub BtnFileReName_Click(sender As Object, e As EventArgs) Handles BtnFileReName.Click
        Dim DirList As New ArrayList
        Dim ResultName As String = ""
        Dim Resultant As String = ""
        Dim bNeedYear As Boolean = False
        Dim errorCount As Integer = 0
        ProcessingStatus = True

        Try
            BtnFileReName.Text = "Processing"
            BtnFileReName.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(txtFolderPath.Text)

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

                        Dim pMovieInfo As MovieInfo = getMovieInfo(Dir, movieyear)
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
                                            Dim tmpfile2 As String = pMovieInfo.MovieName & Microsoft.VisualBasic.Right(tmpFileName, tmpFileName.Length - iPos2)
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
                                            tmpFileName = pMovieInfo.MovieName & x.Extension
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

        BtnFileReName.Text = "FileReName"
        BtnFileReName.Enabled = True
        ProcessingStatus = False
    End Sub

    Private Sub BtnRemapFolders_MouseHover(sender As Object, e As EventArgs) Handles BtnRemapFolders.MouseHover
        If Not ProcessingStatus Then
            LblDescription.Text = "Re-map folder does a TheMovieDB.org lookup on folders in the movie import folder path you have set in the options tab and cleans up the folder and file names to match and then copies the folder to the movie folder path under the genre associated with the movie."
        End If
    End Sub

    Private Sub BtnRemapFolders_Mouseleave(sender As Object, e As EventArgs) Handles BtnRemapFolders.MouseLeave
        If Not ProcessingStatus Then
            LblDescription.Text = ""
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

            Dirs = getAllFolders(txtFolderPath.Text)

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
                    Dim pMovieInfo As MovieInfo = Nothing
                    Try
                        pMovieInfo = getMovieInfo(Dir, movieyear)
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
                                If iCnt >= CInt(My.Settings.NoOfGenres) Then
                                    Exit For
                                End If
                            Next
                        Catch ex As Exception
                            TxtErrorMessage.Text = "Error at 7"
                        End Try

                        If My.Settings.GenreOrYear = "1" Then
                            If DestFolder = "-" Then
                                If Microsoft.VisualBasic.Right(txtFolderPath.Text, 1) = "\" Then
                                    DestFolder = txtFolderPath.Text & "Other"
                                Else
                                    DestFolder = txtFolderPath.Text & "\" & "Other"
                                End If
                            Else
                                If Microsoft.VisualBasic.Right(txtFolderPath.Text, 1) = "\" Then
                                    DestFolder = txtFolderPath.Text & DestFolder.Remove(0, 1)
                                Else
                                    DestFolder = txtFolderPath.Text & "\" & DestFolder.Remove(0, 1)
                                End If
                            End If
                        Else
                            If Microsoft.VisualBasic.Right(txtFolderPath.Text, 1) = "\" Then
                                DestFolder = txtFolderPath.Text & pMovieInfo.ReleaseYear
                            Else
                                DestFolder = txtFolderPath.Text & "\" & pMovieInfo.ReleaseYear
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

    Private Function getAllFolders(ByVal directory As String) As String()
        'Create object
        Dim fi As New IO.DirectoryInfo(directory)
        'Array to store paths
        Dim path() As String = {}
        Dim ellipsis As String = ""
        Try
            'Loop through subfolders
            For Each subfolder As IO.DirectoryInfo In fi.GetDirectories()

                If UCase(subfolder.FullName.Remove(0, subfolder.FullName.LastIndexOf("\") + 1)) <> UCase("$recycle.bin") And
                    UCase(subfolder.FullName.Remove(0, subfolder.FullName.LastIndexOf("\") + 1)) <> UCase("extrafanart") And
                    UCase(subfolder.FullName.Remove(0, subfolder.FullName.LastIndexOf("\") + 1)) <> UCase("extrathumbs") And
                    UCase(subfolder.FullName.Remove(0, subfolder.FullName.LastIndexOf("\") + 1)) <> UCase("subs") And
                    UCase(subfolder.FullName.Remove(0, subfolder.FullName.LastIndexOf("\") + 1)) <> UCase("subtitles") Then

                    'Add this folders name
                    Array.Resize(path, path.Length + 1)
                    path(path.Length - 1) = subfolder.FullName
                    'Recall function with each subdirectory

                    For Each s As String In getAllFolders(subfolder.FullName)
                        Array.Resize(path, path.Length + 1)
                        path(path.Length - 1) = s
                    Next
                End If
            Next
        Catch ex As Exception
            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
        End Try

        Return path
    End Function


    Private Function addMovieYear(ByVal pFolderPath As String, ByVal pMovieYear As String) As String
        Dim pMovieInfo As MovieInfo = getMovieInfo(pFolderPath, pMovieYear)

        If IsDate(pMovieInfo.ReleaseDate) Then
            If CkBxUseParens.Checked = True Then
                Return pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1) & " (" & pMovieInfo.ReleaseYear & ")"
            Else
                Return pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1) & " [" & pMovieInfo.ReleaseYear & "]"
            End If
        Else
            Return pFolderPath
        End If
    End Function


    Private Function getMovieInfo(ByVal pFolderPath As String, ByVal pMovieYear As String) As MovieInfo
        Dim pMovieInfo As New MovieInfo
        Dim fileEntries As String() = Directory.GetFiles(pFolderPath, "*.kb")
        If fileEntries.Count = 1 Then
            pMovieInfo.LoadKodiBuddyFile(fileEntries(0))
            Return pMovieInfo
            Exit Function
        End If

        fileEntries = Directory.GetFiles(pFolderPath)

        Dim pMovieName As String = pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1).Replace("_", " ")

        'Get the position of the end of the year
        Dim iPos = pMovieName.LastIndexOf(")") + 1

        'Trim anything after the year
        If Not pMovieName.Length = iPos AndAlso iPos <> 0 Then
            pMovieName = pMovieName.Remove(iPos, pMovieName.Length - iPos)
        End If
        pMovieInfo.MovieName = pMovieName
        Dim shortMovieName As String
        If pMovieName.LastIndexOf("(") > 0 Then
            shortMovieName = pMovieName.Remove(pMovieName.LastIndexOf("("), pMovieName.LastIndexOf(")") - pMovieName.LastIndexOf("(") + 1).Trim()
        Else
            shortMovieName = pMovieName
        End If
        pMovieInfo.ShortMovieName = shortMovieName

        Dim dateyear As String = ""
        Dim webClient As New System.Net.WebClient
        Dim sDate As String = "1800"
        Dim bError As Boolean = False
        Dim result As String = webClient.DownloadString("https://www.themoviedb.org/search?query=" & shortMovieName.Replace(" ", "+"))
        Try
            If pMovieYear <> "" And IsDate(pMovieYear) Then
                sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10)
                If Not IsDate(sDate) Then
                    sDate = ""
                End If
                While sDate = "" OrElse CInt(pMovieYear) <> Year(CDate(sDate))
                    sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10) '
                    If Not IsDate(sDate) Then
                        result = result.Remove(0, result.IndexOf("<span class=""release_date"">") + 29)
                        sDate = ""
                        If result.IndexOf("<span class=""release_date"">") > 0 Then
                            'continue on
                        Else
                            bError = True
                            TxtErrorMessage.Text = vbCrLf & "Date Not Found for - " & pMovieName & " Verify that the date on the folder is correct." & TxtErrorMessage.Text & vbCrLf
                            Exit While
                        End If
                    Else

                        If Year(CDate(sDate)) <> CInt(pMovieYear) Then
                            result = result.Remove(0, result.IndexOf("view_more") + 9)
                        End If
                    End If

                End While
            Else
                If IsDate(Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10)) Then
                    sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10)
                Else
                    bError = True
                    pMovieInfo.Success = False
                End If
            End If
        Catch ex As Exception
            bError = True
            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
        End Try
        If Not bError Then
            Try
                Dim sMovieID As String = Mid(result, result.IndexOf("class=""title result"" href=""/movie/") + 35, 20)
                pMovieInfo.MovieID = Mid(sMovieID, 1, InStr(sMovieID, Chr(34)) - 1)
                pMovieInfo.Rating = Mid(result, result.IndexOf("<span class=""vote_average"">") + 28, 3)
                If IsDate(sDate) Then
                    pMovieInfo.ReleaseDate = sDate
                    pMovieInfo.ReleaseYear = CType(DateAndTime.Year(CDate(sDate)), String)
                    pMovieInfo.ReleaseMonth = CType(DateAndTime.Month(CDate(sDate)), String)
                    pMovieInfo.ReleaseDay = CType(DateAndTime.Day(CDate(sDate)), String)
                End If
                Dim sGenres As String = Mid(result, result.IndexOf("<span class=""genres"">") + 22, 200)
                pMovieInfo.Genres = Mid(sGenres, 1, InStr(sGenres, "<") - 1)
                Dim sOverview As String = Mid(result, result.IndexOf("<p class=""overview"">") + 21, 2000)
                pMovieInfo.Overview = Mid(sOverview, 1, InStr(sOverview, "<") - 1)
                pMovieInfo.Success = True
                ' Write XML data to .kob file so we can retrieve in the future.
                If pMovieInfo.Success AndAlso fileEntries.Count > 0 Then pMovieInfo.CreateKodiBuddyFile(pFolderPath)
            Catch ex As Exception
                bError = True
                TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
            End Try
        Else
            pMovieInfo.Success = False
        End If
        getMovieInfo = pMovieInfo
    End Function

#End Region

End Class




Class MovieInfo
    Private _Success As Boolean

    Public Property Success() As Boolean
        Get
            Return _Success
        End Get
        Set(ByVal value As Boolean)
            _Success = value
        End Set
    End Property

    Private _ReleaseDate As String

    Public Property ReleaseDate() As String
        Get
            Return _ReleaseDate
        End Get
        Set(ByVal value As String)
            _ReleaseDate = value
        End Set
    End Property

    Private _ReleaseYear As String

    Public Property ReleaseYear() As String
        Get
            Return _ReleaseYear
        End Get
        Set(ByVal value As String)
            _ReleaseYear = value
        End Set
    End Property

    Private _ReleaseMonth As String

    Public Property ReleaseMonth() As String
        Get
            Return _ReleaseMonth
        End Get
        Set(ByVal value As String)
            _ReleaseMonth = value
        End Set
    End Property

    Private _ReleaseDay As String

    Public Property ReleaseDay() As String
        Get
            Return _ReleaseDay
        End Get
        Set(ByVal value As String)
            _ReleaseDay = value
        End Set
    End Property

    Private _MovieID As String

    Public Property MovieID() As String
        Get
            Return _MovieID
        End Get
        Set(ByVal value As String)
            _MovieID = value
        End Set
    End Property

    Private _MovieName As String

    Public Property MovieName() As String
        Get
            Return _MovieName
        End Get
        Set(ByVal value As String)
            _MovieName = StrConv(value, vbProperCase)
        End Set
    End Property

    Private _ShortMovieName As String

    Public Property ShortMovieName() As String
        Get
            Return _ShortMovieName
        End Get
        Set(ByVal value As String)
            _ShortMovieName = StrConv(value, vbProperCase)
        End Set
    End Property

    Private _Rating As String

    Public Property Rating() As String
        Get
            Return _Rating
        End Get
        Set(ByVal value As String)
            _Rating = value
        End Set
    End Property


    Private _Genres As String

    Public Property Genres() As String
        Get
            Return _Genres
        End Get
        Set(ByVal value As String)
            _Genres = value
        End Set
    End Property


    Private _Overview As String

    Public Property Overview() As String
        Get
            Return _Overview
        End Get
        Set(ByVal value As String)
            _Overview = value
        End Set
    End Property

    Public Sub CreateKodiBuddyFile(ByRef filepath As String)
        Dim outputfile As String = filepath & "\" & ShortMovieName & ".kb"
        Dim XMLDoc As New Xml.XmlDocument()

        Dim docNode As XmlNode = XMLDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)
        XMLDoc.AppendChild(docNode)

        Dim productsNode As XmlNode = XMLDoc.CreateElement("KodiBuddy")
        XMLDoc.AppendChild(productsNode)

        Dim productNode As XmlNode = XMLDoc.CreateElement("Movie")
        Dim productAttribute As XmlAttribute = XMLDoc.CreateAttribute("MovieID")
        productAttribute.Value = MovieID
        productNode.Attributes.Append(productAttribute)
        productsNode.AppendChild(productNode)

        Dim MovieNameNode As XmlNode = XMLDoc.CreateElement("MovieName")
        MovieNameNode.AppendChild(XMLDoc.CreateTextNode(MovieName))
        productNode.AppendChild(MovieNameNode)
        Dim MovieIDNode As XmlNode = XMLDoc.CreateElement("MovieID")
        MovieIDNode.AppendChild(XMLDoc.CreateTextNode(MovieID))
        productNode.AppendChild(MovieIDNode)
        Dim SuccessNode As XmlNode = XMLDoc.CreateElement("Success")
        SuccessNode.AppendChild(XMLDoc.CreateTextNode(CType(Success, String)))
        productNode.AppendChild(SuccessNode)
        Dim ReleaseDateNode As XmlNode = XMLDoc.CreateElement("ReleaseDate")
        ReleaseDateNode.AppendChild(XMLDoc.CreateTextNode(ReleaseDate))
        productNode.AppendChild(ReleaseDateNode)
        Dim ReleaseYearNode As XmlNode = XMLDoc.CreateElement("ReleaseYear")
        ReleaseYearNode.AppendChild(XMLDoc.CreateTextNode(ReleaseYear))
        productNode.AppendChild(ReleaseYearNode)
        Dim ReleaseMonthNode As XmlNode = XMLDoc.CreateElement("ReleaseMonth")
        ReleaseMonthNode.AppendChild(XMLDoc.CreateTextNode(ReleaseMonth))
        productNode.AppendChild(ReleaseMonthNode)
        Dim ReleaseDayNode As XmlNode = XMLDoc.CreateElement("ReleaseDay")
        ReleaseDayNode.AppendChild(XMLDoc.CreateTextNode(ReleaseDay))
        productNode.AppendChild(ReleaseDayNode)
        Dim ShortMovieNameNode As XmlNode = XMLDoc.CreateElement("ShortMovieName")
        ShortMovieNameNode.AppendChild(XMLDoc.CreateTextNode(ShortMovieName))
        productNode.AppendChild(ShortMovieNameNode)
        Dim RatingNode As XmlNode = XMLDoc.CreateElement("Rating")
        RatingNode.AppendChild(XMLDoc.CreateTextNode(Rating))
        productNode.AppendChild(RatingNode)
        Dim GenresNode As XmlNode = XMLDoc.CreateElement("Genres")
        GenresNode.AppendChild(XMLDoc.CreateTextNode(Genres))
        productNode.AppendChild(GenresNode)
        Dim OverviewNode As XmlNode = XMLDoc.CreateElement("Overview")
        OverviewNode.AppendChild(XMLDoc.CreateTextNode(Overview))
        productNode.AppendChild(OverviewNode)

        XMLDoc.Save(outputfile)

    End Sub
    Public Sub LoadKodiBuddyFile(ByRef filepath As String)

        Dim XMLDoc As New Xml.XmlDocument()
        XMLDoc.Load(filepath)
        MovieName = XMLDoc.GetElementsByTagName("MovieName").Item(0).InnerText
        MovieID = XMLDoc.GetElementsByTagName("MovieID").Item(0).InnerText
        Success = CBool(XMLDoc.GetElementsByTagName("Success").Item(0).InnerText)
        ReleaseDate = XMLDoc.GetElementsByTagName("ReleaseDate").Item(0).InnerText
        ReleaseYear = XMLDoc.GetElementsByTagName("ReleaseYear").Item(0).InnerText
        ReleaseMonth = XMLDoc.GetElementsByTagName("ReleaseMonth").Item(0).InnerText
        ReleaseDay = XMLDoc.GetElementsByTagName("ReleaseDay").Item(0).InnerText
        ShortMovieName = XMLDoc.GetElementsByTagName("ShortMovieName").Item(0).InnerText
        Rating = XMLDoc.GetElementsByTagName("Rating").Item(0).InnerText
        Genres = XMLDoc.GetElementsByTagName("Genres").Item(0).InnerText
        Overview = XMLDoc.GetElementsByTagName("Overview").Item(0).InnerText
    End Sub
End Class