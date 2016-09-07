Imports System.IO


Public Class FrmFileReNamer
    Public Dirs() As String

#Region "Options"

    Private Sub BtnSelectFolder_Click(sender As Object, e As EventArgs)
        FolderBrowserDialog.SelectedPath = My.Settings.MoviePath
        FolderBrowserDialog.ShowDialog()
        txtFolderPath.Text = FolderBrowserDialog.SelectedPath
        My.Settings.MoviePath = FolderBrowserDialog.SelectedPath
    End Sub

    Private Sub BtnSelectImportFolder_Click(sender As Object, e As EventArgs) Handles BtnSelectImportFolder.Click
        FolderBrowserDialog.SelectedPath = My.Settings.ImportPath2
        FolderBrowserDialog.ShowDialog()
        TxtImportFolder.Text = FolderBrowserDialog.SelectedPath
        My.Settings.ImportPath2 = FolderBrowserDialog.SelectedPath
    End Sub

    Private Sub CkBxUseBracket_CheckedChanged(sender As Object, e As EventArgs)
        If CkBxUseParens.Checked And CkBxUseBracket.Checked Then
            CkBxUseParens.Checked = False
            My.Settings.BracketsParens = 1
        End If
    End Sub


    Private Sub CkBxUseParens_CheckedChanged(sender As Object, e As EventArgs)
        If CkBxUseParens.Checked And CkBxUseBracket.Checked Then
            CkBxUseBracket.Checked = False
            My.Settings.BracketsParens = 2
        End If
    End Sub


    Private Sub txtFolderPath_TextChanged(sender As Object, e As EventArgs) Handles txtFolderPath.TextChanged
        If My.Settings.MoviePath <> txtFolderPath.Text Then
            My.Settings.MoviePath = txtFolderPath.Text
        End If
    End Sub


    Private Sub TxtImportFolder_TextChanged(sender As Object, e As EventArgs) Handles TxtImportFolder.TextChanged
        If My.Settings.ImportPath2 = txtFolderPath.Text Then
            My.Settings.ImportPath2 = txtFolderPath.Text
        End If
    End Sub
#End Region



    Private Sub FrmFileReNamer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Load settings from settings in app.config 
        TxtImportFolder.Text = My.Settings.ImportPath2
        txtFolderPath.Text = My.Settings.MoviePath
        If My.Settings.BracketsParens = 2 Then
            CkBxUseParens.Checked = True
        Else
            CkBxUseBracket.Checked = True
        End If
    End Sub


#Region "Functions"
    Private Sub BtnStart_Click(sender As Object, e As EventArgs)
        Try
            BtnStart.Text = "Processing"
            BtnStart.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(txtFolderPath.Text)

            Dim DirList As New ArrayList
            Dim ResultName As String = ""

            TxtMessageDisplay.Text += "Starting processing" & vbCrLf
            TxtMessageDisplay.ForeColor = Color.Black
            Me.Refresh()

            For i As Integer = Dirs.Count - 1 To 0 Step -1
                Dim Dir As String = Dirs(i)
                If CkBxUseParens.Checked = True And (Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("[") > 0 Or Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("]") > 0) Then
                    ResultName = Dir.Remove(0, Dir.LastIndexOf("\") + 1).Replace("[", "(").Replace("]", ")")
                    TxtMessageDisplay.Text += Dir & vbCrLf
                    My.Computer.FileSystem.RenameDirectory(Dir, ResultName)
                    Me.Refresh()
                ElseIf CkBxUseBracket.Checked = True And (Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("(") > 0 Or Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf(")") > 0) Then
                    ResultName = Dir.Remove(0, Dir.LastIndexOf("\") + 1).Replace("(", "[").Replace(")", "]")
                    TxtMessageDisplay.Text += Dir & vbCrLf
                    My.Computer.FileSystem.RenameDirectory(Dir, ResultName)
                    Me.Refresh()
                End If
            Next i

            BtnStart.Text = "Start"
            BtnStart.Enabled = True

            TxtMessageDisplay.Text += "Processing done on " & Dirs.Count & " Directories." & vbCrLf
            TxtMessageDisplay.ForeColor = Color.Green
            Me.Refresh()

            'Reload Folders
            Dirs = getAllFolders(txtFolderPath.Text)

        Catch ex As Exception
            TxtMessageDisplay.Text += ex.Message & vbCrLf
            TxtMessageDisplay.ForeColor = Color.OrangeRed
        End Try


    End Sub


    Private Sub BtnIMDB_Click(sender As Object, e As EventArgs)
        Try
            BtnStart.Text = "Processing"
            BtnStart.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(txtFolderPath.Text)

            Dim DirList As New ArrayList
            Dim ResultName As String = ""
            Dim Resultant As String = ""

            TxtMessageDisplay.Text += "Starting processing" & vbCrLf
            TxtMessageDisplay.ForeColor = Color.Black
            Me.Refresh()

            For i As Integer = Dirs.Count - 1 To 0 Step -1
                Dim Dir As String = Dirs(i)
                If (Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("[") > 0 Or Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("]") > 0) Or (Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("(") > 0 Or Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf(")") > 0) Then
                    'date is already on the folder
                Else

                    Resultant = Dir.Remove(0, Dir.LastIndexOf("\") + 1)
                    ResultName = Dir.Remove(0, Dir.LastIndexOf("\") + 1)
                    If ResultName.ToUpper() <> UCase("Western") And
                        ResultName.ToUpper() <> UCase("War") And
                        ResultName.ToUpper() <> UCase("Video") And
                        ResultName.ToUpper() <> UCase("Thriller") And
                        ResultName.ToUpper() <> UCase("Super Hero") And
                        ResultName.ToUpper() <> UCase("Sports") And
                        ResultName.ToUpper() <> UCase("SciFi-Fantasy") And
                        ResultName.ToUpper() <> UCase("Romance") And
                        ResultName.ToUpper() <> UCase("Mystery") And
                        ResultName.ToUpper() <> UCase("Musical") And
                        ResultName.ToUpper() <> UCase("Horror-Thriller") And
                        ResultName.ToUpper() <> UCase("Holiday") And
                        ResultName.ToUpper() <> UCase("Historical Drama") And
                        ResultName.ToUpper() <> UCase("Foreign") And
                        ResultName.ToUpper() <> UCase("Drama") And
                        ResultName.ToUpper() <> UCase("Documentary") And
                        ResultName.ToUpper() <> UCase("Cult Classics") And
                        ResultName.ToUpper() <> UCase("Crime Dramas") And
                        ResultName.ToUpper() <> UCase("Concerts") And
                        ResultName.ToUpper() <> UCase("Comedy") And
                        ResultName.ToUpper() <> UCase("Childrens") And
                        ResultName.ToUpper() <> UCase("Biography") And
                        ResultName.ToUpper() <> UCase("Apocalyptic") And
                        ResultName.ToUpper() <> UCase("Action-Adventure") Then

                        TxtMessageDisplay.Text += Dir & vbCrLf
                        TxtMessageDisplay.ForeColor = Color.OrangeRed
                        Me.Refresh()

                        Try
                            My.Computer.FileSystem.RenameDirectory(Dir, addMovieYear(Dir.Remove(0, Dir.LastIndexOf("\") + 1), ""))
                        Catch ex As Exception
                            Dim a = ex.Message
                        End Try
                    End If
                End If
            Next i

            BtnStart.Text = "Start"
            BtnStart.Enabled = True

            TxtMessageDisplay.Text += "Processing done on " & Dirs.Count & " Directories." & vbCrLf
            TxtMessageDisplay.ForeColor = Color.Black
            Me.Refresh()

            'Reload Folders
            Dirs = getAllFolders(txtFolderPath.Text)

        Catch ex As Exception
            TxtMessageDisplay.Text += ex.Message & vbCrLf
            TxtMessageDisplay.ForeColor = Color.Red
        End Try
    End Sub


    Private Sub BtnVideoImport_Click(sender As Object, e As EventArgs) Handles BtnVideoImport.Click
        Dim DirList As New ArrayList
        Dim ResultName As String = ""
        Dim Resultant As String = ""
        Try
            BtnVideoImport.Text = "Processing"
            BtnVideoImport.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(TxtImportFolder.Text)

            TxtMessageDisplay.Text = "Starting processing on " & Dirs.Count & " folders to import." & vbCrLf
            Me.Refresh()

            For i As Integer = Dirs.Count - 1 To 0 Step -1
                Dim Dir As String = Dirs(i)
                Dim bUpdateFolderName As Boolean = False
                Dim MovieName As String = Dir.Remove(0, Dir.LastIndexOf("\") + 1)
                Dim movieyear As String = Dir.Remove(0, Dir.LastIndexOf("(") + 1).Trim().Remove(4)
                'movieyear = movieyear.Remove(4, movieyear.Length - 4)
                Dim iPos = MovieName.LastIndexOf(")") + 1
                If iPos < MovieName.Length Then bUpdateFolderName = True Else bUpdateFolderName = False
                If Not MovieName.Length = iPos Then
                    MovieName = MovieName.Remove(iPos, MovieName.Length - iPos)
                End If
                Dim shortMovieName As String = MovieName.Remove(MovieName.LastIndexOf("("), MovieName.LastIndexOf(")") - MovieName.LastIndexOf("(") + 1).Trim()
                Dim pMovieInfo As MovieInfo = getMovieInfo(shortMovieName, movieyear)

                If pMovieInfo.Success Then

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
                                    Dim tmpfile2 As String = Mid(tmpFileName, 1, iPos1).Replace(".", " ") & Microsoft.VisualBasic.Right(tmpFileName, tmpFileName.Length - iPos2)

                                    Try
                                        My.Computer.FileSystem.RenameFile(fileName, tmpfile2)
                                    Catch ex As Exception
                                        Dim a = ex.Message
                                    End Try
                                End If
                            Else
                                Dim a As String = ""
                            End If

                        Next fileName

                    End If

                    'Rename Folder
                    If bUpdateFolderName Then
                        Try
                            My.Computer.FileSystem.RenameDirectory(Dir, MovieName)
                        Catch ex As Exception
                            TxtMessageDisplay.Text += ex.Message & vbCrLf
                        End Try
                    End If


                    'ToDo: Use genres to move file to final folder.
                    Dim Genres() As String = Split(pMovieInfo.Genres, ",")
                    Dim DestFolder As String = ""
                    Dim SourceFolder As String = ""
                    For Each Genre As String In Genres
                        DestFolder = DestFolder & "-" & Genre.Trim()
                    Next
                    If Microsoft.VisualBasic.Right(txtFolderPath.Text, 1) = "\" Then
                        DestFolder = txtFolderPath.Text & DestFolder.Remove(0, 1)
                    Else
                        DestFolder = txtFolderPath.Text & "\" & DestFolder.Remove(0, 1)
                    End If

                    'Create Destination Directory if missing 
                    If Not Directory.Exists(DestFolder) Then
                        Try
                            Dim di As DirectoryInfo = Directory.CreateDirectory(DestFolder)
                        Catch e2 As Exception
                            TxtMessageDisplay.Text += e2.Message & vbCrLf
                        End Try
                    End If

                    If Microsoft.VisualBasic.Right(DestFolder, 1) = "\" Then
                        DestFolder = DestFolder & MovieName
                    Else
                        DestFolder = DestFolder & "\" & MovieName
                    End If

                    If Microsoft.VisualBasic.Right(TxtImportFolder.Text, 1) = "\" Then
                        SourceFolder = TxtImportFolder.Text & MovieName
                    Else
                        SourceFolder = TxtImportFolder.Text & "\" & MovieName
                    End If

                    If Not Directory.Exists(DestFolder) AndAlso Directory.Exists(SourceFolder) Then
                        Try
                            Directory.Move(SourceFolder, DestFolder)
                        Catch e2 As Exception
                            TxtMessageDisplay.Text += e2.Message & vbCrLf
                        End Try
                    Else
                        'Message folder not found.
                        If Directory.Exists(DestFolder) Then
                            TxtMessageDisplay.Text += "Duplicate folder exists. - " & DestFolder & vbCrLf
                        End If
                        If Not Directory.Exists(SourceFolder) Then
                            TxtMessageDisplay.Text += "Source folder not found. - " & SourceFolder & vbCrLf
                        End If
                    End If

                    TxtMessageDisplay.Text += MovieName & vbCrLf
                    Me.Refresh()
                Else
                    TxtMessageDisplay.Text += "Failed to load Movie info for - " & MovieName & vbCrLf
                    TxtMessageDisplay.ForeColor = Color.Red
                    Me.Refresh()
                End If
            Next i

            BtnVideoImport.Text = "Video Import"
            BtnVideoImport.Enabled = True

            TxtMessageDisplay.Text += "Processing done on " & Dirs.Count & " Folders." & vbCrLf
            Me.Refresh()

        Catch ex As Exception
            TxtMessageDisplay.Text += ex.Message & vbCrLf
            TxtMessageDisplay.ForeColor = Color.Red
        End Try

    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs)
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
            TxtMessageDisplay.Text += ex.Message & vbCrLf
            TxtMessageDisplay.ForeColor = Color.Red
        End Try

        Return path
    End Function


    Private Function addMovieYear(ByVal pFolderPath As String, ByVal pMovieYear As String) As String
        Dim pMovieInfo As MovieInfo = getMovieInfo(Dir.Remove(0, pFolderPath.LastIndexOf("\") + 1), pMovieYear)

        If IsDate(pMovieInfo.ReleaseDate) Then
            If CkBxUseParens.Checked = True Then
                Return Dir.Remove(0, pFolderPath.LastIndexOf("\") + 1) & " (" & pMovieInfo.ReleaseYear & ")"
            Else
                Return Dir.Remove(0, pFolderPath.LastIndexOf("\") + 1) & " [" & pMovieInfo.ReleaseYear & "]"
            End If
        Else
            Return pFolderPath
        End If
    End Function


    Private Function getMovieInfo(ByVal pMovieName As String, ByVal pMovieYear As String) As MovieInfo
        Dim dateyear As String = ""
        Dim webClient As New System.Net.WebClient
        Dim sDate As String = "1800"
        Dim bError As Boolean = False
        Dim result As String = webClient.DownloadString("https://www.themoviedb.org/search?query=" & pMovieName.Replace(" ", "+"))
        Try
            If pMovieYear <> "" Then
                sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10)
                While sDate = "" OrElse pMovieYear <> Year(sDate)
                    sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10) '
                    If Not IsDate(sDate) Then
                        result = result.Remove(0, result.IndexOf("<span class=""release_date"">") + 29)
                        sDate = ""
                        If result.IndexOf("<span class=""release_date"">") > 0 Then
                            'continue on
                        Else
                            bError = True
                            TxtMessageDisplay.Text += "Date Not Found for - " & pMovieName & " Verify that the date on the folder is correct." & vbCrLf
                            TxtMessageDisplay.ForeColor = Color.Red
                            Exit While
                        End If
                    Else

                            If Year(sDate) <> pMovieYear Then
                            result = result.Remove(0, result.IndexOf("view_more") + 9)
                        End If
                    End If

                End While
            Else
                sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10)
            End If
        Catch ex As Exception
            bError = True
            TxtMessageDisplay.Text += ex.Message & vbCrLf
            TxtMessageDisplay.ForeColor = Color.Red
        End Try
        Dim pMovieInfo As New MovieInfo
        If Not bError Then
            Try
                Dim sMovieID As String = Mid(result, result.IndexOf("class=""title result"" href=""/movie/") + 35, 20)
                pMovieInfo.MovieID = Mid(sMovieID, 1, InStr(sMovieID, Chr(34)) - 1)
                pMovieInfo.Rating = Mid(result, result.IndexOf("<span class=""vote_average"">") + 28, 3)
                If IsDate(sDate) Then
                    pMovieInfo.ReleaseDate = sDate
                    pMovieInfo.ReleaseYear = DateAndTime.Year(sDate)
                    pMovieInfo.ReleaseMonth = DateAndTime.Month(sDate)
                    pMovieInfo.ReleaseDay = DateAndTime.Day(sDate)
                End If
                Dim sGenres As String = Mid(result, result.IndexOf("<span class=""genres"">") + 22, 200)
                pMovieInfo.Genres = Mid(sGenres, 1, InStr(sGenres, "<") - 1)
                Dim sOverview As String = Mid(result, result.IndexOf("<p class=""overview"">") + 21, 2000)
                pMovieInfo.Overview = Mid(sOverview, 1, InStr(sOverview, "<") - 1)
                pMovieInfo.Success = True
            Catch ex As Exception
                bError = True
                TxtMessageDisplay.Text += ex.Message & vbCrLf
                TxtMessageDisplay.ForeColor = Color.Red
            End Try
        Else
            pMovieInfo.Success = False
        End If



        getMovieInfo = pMovieInfo
    End Function

    Private Sub BtnCancel_Click_1(sender As Object, e As EventArgs) Handles BtnCancel.Click
        End
    End Sub

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

End Class