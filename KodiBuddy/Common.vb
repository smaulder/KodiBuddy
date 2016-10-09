Imports System.IO
Imports System.Xml

Namespace Common

    Public Class Functions
        Public Shared Function getAllFolders(ByVal directory As String, ByRef TxtErrorMessage As TextBox) As String()
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

                        For Each s As String In getAllFolders(subfolder.FullName, TxtErrorMessage)
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


        Public Shared Function addMovieYear(ByRef CkBxUseParens As Object, ByVal pFolderPath As String, ByVal pMovieYear As String, ByRef TxtErrorMessage As TextBox) As String
            Dim pMovieInfo As MovieInfo = getMovieInfo(pFolderPath, pMovieYear, TxtErrorMessage)

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

        Public Shared Function getMovieYear(ByVal pFolderPath As String, ByRef TxtErrorMessage As TextBox) As String
            Try
                Dim tempPath As String = pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1).Replace("_", " ")
                Dim iPos = tempPath.LastIndexOf("(") + 1
                'Get the position of the end of the year
                Dim iPos2 = tempPath.LastIndexOf(")")
                tempPath = tempPath.Remove(0, iPos).Remove(iPos2 - iPos)
                If IsNumeric(tempPath) AndAlso tempPath >= 1900 AndAlso tempPath <= Year(Now()) Then
                    Return tempPath
                Else
                    Return ""
                End If
            Catch ex As Exception
                Return ""
            End Try
        End Function


        Public Shared Function getMovieInfo(ByVal pFolderPath As String, ByVal pMovieYear As String, ByRef TxtErrorMessage As TextBox) As MovieInfo
            Dim pMovieInfo As New MovieInfo
            Try

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
            Catch ex As Exception
                pMovieInfo.Success = False
            End Try
            getMovieInfo = pMovieInfo
        End Function


    End Class

End Namespace