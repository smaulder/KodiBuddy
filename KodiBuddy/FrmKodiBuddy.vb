Option Explicit On
Option Strict On

Imports System.IO
Imports System.Xml
Imports KodiBuddy.Common.Functions
Imports KodiBuddy.Common.MovieInfo

Public Class FrmKodiBuddy

    Public KodiBuddyLibrary As New Collection()

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

            Dirs = getAllFolders(TxtImportFolder.Text, TxtErrorMessage)

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

                    Dim pMovieInfo As Common.MovieInfo = getMovieInfo(Dir, movieyear, TxtErrorMessage)
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

            Dirs = getAllFolders(txtFolderPath.Text, TxtErrorMessage)

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

                        Dim pMovieInfo As Common.MovieInfo = getMovieInfo(Dir, movieyear, TxtErrorMessage)
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

            Dirs = getAllFolders(txtFolderPath.Text, TxtErrorMessage)

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
                    Dim pMovieInfo As Common.MovieInfo = Nothing
                    Try
                        pMovieInfo = getMovieInfo(Dir, movieyear, TxtErrorMessage)
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

    Private Sub BtnMasterList_Click(sender As Object, e As EventArgs) Handles BtnMasterList.Click

        Dim box = New FrmMasterList()
        box.Show()
    End Sub

#End Region

End Class