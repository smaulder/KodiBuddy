Imports System.IO


Public Class Form1
    Public Dirs() As String

#Region "Options"

    Private Sub BtnSelectFolder_Click(sender As Object, e As EventArgs)
        FolderBrowserDialog1.RootFolder = My.Settings.MoviePath
        FolderBrowserDialog1.ShowDialog()
        txtFolderPath.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub BtnSelectImportFolder_Click(sender As Object, e As EventArgs) Handles BtnSelectImportFolder.Click
        FolderBrowserDialog1.RootFolder = My.Settings.ImportPath
        FolderBrowserDialog1.ShowDialog()
        TxtImportFolder.Text = FolderBrowserDialog1.SelectedPath
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
        My.Settings.MoviePath = txtFolderPath.Text
    End Sub

    Private Sub TxtImportFolder_TextChanged(sender As Object, e As EventArgs) Handles TxtImportFolder.TextChanged
        My.Settings.ImportPath = txtFolderPath.Text
    End Sub
#End Region



    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFolderPath.Text = My.Settings.MoviePath
        TxtImportFolder.Text = My.Settings.ImportPath
        If My.Settings.BracketsParens = 2 Then
            CkBxUseParens.Checked = True
        Else
            CkBxUseBracket.Checked = True
        End If
        'LblRecordCount.Text = Dirs.Count
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

            LblMessage.Text = "Starting processing"
            LblMessage.ForeColor = Color.Black
            Me.Refresh()

            For i As Integer = Dirs.Count - 1 To 0 Step -1
                Dim Dir As String = Dirs(i)
                If CkBxUseParens.Checked = True And (Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("[") > 0 Or Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("]") > 0) Then
                    ResultName = Dir.Remove(0, Dir.LastIndexOf("\") + 1).Replace("[", "(").Replace("]", ")")
                    LblMessage.Text = Dir
                    LblMessage.ForeColor = Color.Black
                    My.Computer.FileSystem.RenameDirectory(Dir, ResultName)
                    Me.Refresh()
                ElseIf CkBxUseBracket.Checked = True And (Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("(") > 0 Or Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf(")") > 0) Then
                    ResultName = Dir.Remove(0, Dir.LastIndexOf("\") + 1).Replace("(", "[").Replace(")", "]")
                    LblMessage.Text = Dir
                    LblMessage.ForeColor = Color.Black
                    My.Computer.FileSystem.RenameDirectory(Dir, ResultName)
                    Me.Refresh()
                End If
            Next i

            BtnStart.Text = "Start"
            BtnStart.Enabled = True

            LblMessage.Text = "Processing done on " & Dirs.Count & " Directories."
            LblMessage.ForeColor = Color.Black
            Me.Refresh()

            'Reload Folders
            Dirs = getAllFolders(txtFolderPath.Text)

        Catch ex As Exception
            LblMessage.Text = ex.Message
            LblMessage.ForeColor = Color.OrangeRed
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

            LblMessage.Text = "Starting processing"
            LblMessage.ForeColor = Color.Black
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

                        LblMessage.Text = Dir
                        LblMessage.ForeColor = Color.OrangeRed
                        Me.Refresh()

                        Try
                            My.Computer.FileSystem.RenameDirectory(Dir, addMovieYear(Dir.Remove(0, Dir.LastIndexOf("\") + 1)))
                        Catch ex As Exception
                            Dim a = ex.Message
                        End Try
                    End If
                End If
            Next i

            BtnStart.Text = "Start"
            BtnStart.Enabled = True

            LblMessage.Text = "Processing done on " & Dirs.Count & " Directories."
            LblMessage.ForeColor = Color.Black
            Me.Refresh()

            'Reload Folders
            Dirs = getAllFolders(txtFolderPath.Text)

        Catch ex As Exception
            LblMessage.Text = ex.Message
            LblMessage.ForeColor = Color.OrangeRed
        End Try
    End Sub


    Private Sub BtnVideoImport_Click(sender As Object, e As EventArgs) Handles BtnVideoImport.Click
        Dim DirList As New ArrayList
        Dim ResultName As String = ""
        Dim Resultant As String = ""
        Try
            BtnStart.Text = "Processing"
            BtnStart.Enabled = False
            Me.Refresh()

            Dirs = getAllFolders(TxtImportFolder.Text)

            LblMessage.Text = "Starting processing"
            Me.Refresh()

            For i As Integer = Dirs.Count - 1 To 0 Step -1
                Dim Dir As String = Dirs(i)

                Dim MovieName As String = Dir.Remove(0, Dir.LastIndexOf("\") + 1)
                Dim iPos = MovieName.LastIndexOf(")") + 1
                MovieName = MovieName.Remove(iPos, MovieName.Length - iPos)

                Dim pMovieInfo As MovieInfo = getMovieInfo(MovieName)

                Try
                    My.Computer.FileSystem.RenameDirectory(Dir, addMovieYear(Dir.Remove(0, Dir.LastIndexOf("\") + 1)))
                Catch ex As Exception
                    Dim a = ex.Message
                End Try

                'ToDo: Rename Files Before Folder


                'Rename Folder
                Try
                    My.Computer.FileSystem.RenameDirectory(Dir, MovieName)
                Catch ex As Exception
                    Dim a = ex.Message
                End Try

                LblMessage.Text = MovieName
                LblMessage.ForeColor = Color.OrangeRed
                Me.Refresh()
            Next i

            BtnStart.Text = "Start"
            BtnStart.Enabled = True

            LblMessage.Text = "Processing done on " & Dirs.Count & " Directories."
            LblMessage.ForeColor = Color.Black
            Me.Refresh()

        Catch ex As Exception
            LblMessage.Text = ex.Message
        LblMessage.ForeColor = Color.OrangeRed
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
            LblMessage.Text = ex.Message
            LblMessage.ForeColor = Color.OrangeRed
        End Try

        Return path
    End Function


    Private Function addMovieYear(ByVal pFolderPath As String) As String
        Dim pMovieInfo As MovieInfo = getMovieInfo(Dir.Remove(0, pFolderPath.LastIndexOf("\") + 1))

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


    'Private Function getMovieYear(ByVal pFolderPath As String) As String
    '    Dim dateyear As String = ""
    '    Dim webClient As New System.Net.WebClient
    '    Dim result As String = webClient.DownloadString("https://www.themoviedb.org/search?query=" & Dir.Remove(0, pFolderPath.LastIndexOf("\") + 1).Replace(" ", "+"))
    '    Dim sDate As String = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10)

    '    If IsDate(sDate) Then
    '        dateyear = Year(sDate)
    '    End If
    '    getMovieYear = dateyear
    'End Function


    Private Function getMovieInfo(ByVal pMovieName As String) As MovieInfo
        Dim dateyear As String = ""
        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString("https://www.themoviedb.org/search?query=" & pMovieName.Replace(" ", "+"))
        Dim pMovieInfo As New MovieInfo
        Dim sMovieID As String = Mid(result, result.IndexOf("class=""title result"" href=""/movie/") + 35, 20)
        pMovieInfo.MovieID = Mid(sMovieID, 1, InStr(sMovieID, Chr(34)) - 1)
        pMovieInfo.Rating = Mid(result, result.IndexOf("<span class=""vote_average"">") + 28, 3)
        Dim sDate As String = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10)
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



        getMovieInfo = pMovieInfo
    End Function

#End Region

End Class

Class MovieInfo
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