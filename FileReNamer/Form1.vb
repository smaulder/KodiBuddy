Imports System.IO


Public Class Form1
    Public Dirs() As String

    Private Sub BtnSelectFolder_Click(sender As Object, e As EventArgs) Handles BtnSelectFolder.Click
        FolderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer
        FolderBrowserDialog1.ShowDialog()
        txtFolderPath.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub CkBxUseBracket_CheckedChanged(sender As Object, e As EventArgs) Handles CkBxUseBracket.CheckedChanged
        If CkBxUseParens.Checked And CkBxUseBracket.Checked Then
            CkBxUseParens.Checked = False
        End If
    End Sub

    Private Sub CkBxUseParens_CheckedChanged(sender As Object, e As EventArgs) Handles CkBxUseParens.CheckedChanged
        If CkBxUseParens.Checked And CkBxUseBracket.Checked Then
            CkBxUseBracket.Checked = False
        End If
    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        Try
            BtnStart.Text = "Processing"
            BtnStart.Enabled = False
            Me.Refresh()

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
                    'ElseIf (Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("[") = 0 And Dir.Remove(0, Dir.LastIndexOf("\") + 1).LastIndexOf("]") = 0) Then
                    '    'ToDo: lookup directory on imdb.
                    '    ResultName = Dir.Remove(0, Dir.LastIndexOf("\") + 1)
                    '    LblMessage.Text = Dir
                    '    LblMessage.ForeColor = Color.OrangeRed
                    '    Me.Refresh()
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


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFolderPath.Text = "V:\"
        Dirs = getAllFolders(txtFolderPath.Text)
        CkBxUseParens.Checked = True
        LblRecordCount.Text = Dirs.Count
    End Sub


    Private Sub BtnIMDB_Click(sender As Object, e As EventArgs) Handles BtnIMDB.Click
        Try
            BtnStart.Text = "Processing"
            BtnStart.Enabled = False
            Me.Refresh()

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

                        'ToDo: lookup directory on imdb.
                        Dim webClient As New System.Net.WebClient
                        Dim result As String = webClient.DownloadString("https://www.themoviedb.org/search?query=" & Resultant.Replace(" ", "+"))
                        Dim dateA As String = Mid(result, result.IndexOf("<span class=""release_date"">") + 29, 10)
                        If IsDate(dateA) Then
                            Dim dateyear As String = Year(dateA)
                            If CkBxUseParens.Checked = True Then
                                ResultName = ResultName & " (" & dateyear & ")"
                            Else
                                ResultName = ResultName & " [" & dateyear & "]"
                            End If
                            Try
                                My.Computer.FileSystem.RenameDirectory(Dir, ResultName)
                            Catch ex As Exception
                                Dim a = ex.Message
                            End Try
                        End If
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

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        End
    End Sub
End Class
