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
                Return pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1) & " (" & pMovieInfo.ReleaseYear & ")"
            Else
                Return pFolderPath
            End If
        End Function


        Public Shared Function addTVYear(ByRef CkBxUseParens As Object, ByVal pFolderPath As String, ByVal pTVName As String, ByVal pTVYear As String, ByRef TxtErrorMessage As TextBox) As String
            Dim pTVInfo As TVInfo = getTVInfo(pFolderPath, pTVName, pTVYear, TxtErrorMessage)

            If IsDate(pTVInfo.ReleaseDate) Then
                Return pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1) & " (" & pTVInfo.ReleaseYear & ")"
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

                Dim pMovieName As String = pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1).Replace("_", " ").Replace(".", " ")

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
                Dim result As String = webClient.DownloadString("http://www.themoviedb.org/search?query=" & shortMovieName.Replace(" ", "+"))
                Try
                    If pMovieYear <> "" And IsDate(pMovieYear) Then
                        sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 100)
                        If Not IsDate(sDate) Then
                            sDate = ""
                        End If
                        While sDate = "" OrElse CInt(pMovieYear) <> Year(CDate(sDate))
                            sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10) '
                            If Not IsDate(sDate) Then
                                result = result.Remove(0, result.IndexOf("<span class=""release_date"">") + 85)
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
                        If IsDate(Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)) Then
                            sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)
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

        Public Shared Sub getTVEpisodeAndSeason(ByRef pTVName As String, ByRef pEpisode As String, ByRef pSeason As String, ByRef pErrorMessage As String)
            pErrorMessage = ""
            pEpisode = ""
            pSeason = ""
            Try
                Dim TvParts() As String = pTVName.Replace(".", " ").Replace("_", " ").Split(CType(" ", Char()))
                Dim d As Boolean = False
                Dim e As Boolean = False
                Dim f As Boolean = False
                For Each part In TvParts
                    d = UCase(part) Like ("S##E##")
                    e = UCase(part) Like ("####")
                    f = UCase(part) Like ("###")
                    If d Then
                        pEpisode = UCase(part)
                        pTVName = pTVName.Remove(pTVName.LastIndexOf(part)).Replace(".", " ").Replace("_", " ").Trim()
                        'Get season number from episode
                        pSeason = pEpisode.Remove(0, 1).Remove(2).TrimStart("0"c)
                        Exit For
                    ElseIf e Then
                        pEpisode = UCase(part)
                        pEpisode = "S" & Left(pEpisode, 2) & "E" & Right(pEpisode, 2)
                        pTVName = pTVName.Remove(pTVName.LastIndexOf(part)).Replace(".", " ").Replace("_", " ").Trim()
                        'Get season number from episode
                        pSeason = pEpisode.Remove(0, 1).Remove(2).TrimStart("0"c)
                        Exit For
                    ElseIf f Then
                        pEpisode = UCase(part)
                        pEpisode = "S0" & Left(pEpisode, 1) & "E" & Right(pEpisode, 2)
                        pTVName = pTVName.Remove(pTVName.LastIndexOf(part)).Replace(".", " ").Replace("_", " ").Trim()
                        'Get season number from episode
                        pSeason = pEpisode.Remove(0, 1).Remove(2).TrimStart("0"c)
                        Exit For
                    End If
                Next

            Catch ex As Exception
                pErrorMessage = ex.Message
            End Try
        End Sub

        Public Shared Function getTVInfo(ByVal pFolderPath As String, ByVal pTVName As String, ByVal pTVYear As String, ByRef TxtErrorMessage As TextBox) As TVInfo
            Dim pTVInfo As New TVInfo
            Try

                Dim fileEntries As String() = Directory.GetFiles(pFolderPath, "*.kbtv")
                If fileEntries.Count = 1 Then
                    pTVInfo.LoadKodiBuddyFile(fileEntries(0))
                    Return pTVInfo
                    Exit Function
                End If

                fileEntries = Directory.GetFiles(pFolderPath)

                Dim pTVName2 As String = pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1).Replace("_", " ")

                'Get the position of the end of the year
                Dim iPos = pTVName2.LastIndexOf(")") + 1

                'Trim anything after the year
                If Not pTVName2.Length = iPos AndAlso iPos <> 0 Then
                    pTVName2 = pTVName2.Remove(iPos, pTVName2.Length - iPos)
                End If

                pTVInfo.TVName = pTVName

                'Dim shortTVName As String
                'If pTVName2.LastIndexOf("(") > 0 Then
                '    shortTVName = pTVName2.Remove(pTVName2.LastIndexOf("("), pTVName2.LastIndexOf(")") - pTVName2.LastIndexOf("(") + 1).Trim()
                'Else
                '    shortTVName = pTVName2
                'End If
                'pTVInfo.ShortTVName = shortTVName

                Dim dateyear As String = ""
                Dim webClient As New System.Net.WebClient
                Dim sDate As String = "1800"
                Dim bError As Boolean = False
                Dim result As String = webClient.DownloadString("https://www.themoviedb.org/search?query=" & pTVName.Replace(" ", "+"))
                Try
                    If pTVYear <> "" And IsDate(pTVYear) Then
                        sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)
                        If Not IsDate(sDate) Then
                            sDate = ""
                        End If
                        While sDate = "" OrElse CInt(pTVYear) <> Year(CDate(sDate))
                            sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10) '
                            If Not IsDate(sDate) Then
                                result = result.Remove(0, result.IndexOf("<span class=""release_date"">") + 85)
                                sDate = ""
                                If result.IndexOf("<span class=""release_date"">") > 0 Then
                                    'continue on
                                Else
                                    bError = True
                                    TxtErrorMessage.Text = vbCrLf & "Date Not Found for - " & pTVName & " Verify that the date on the folder is correct." & TxtErrorMessage.Text & vbCrLf
                                    Exit While
                                End If
                            Else

                                If Year(CDate(sDate)) <> CInt(pTVYear) Then
                                    result = result.Remove(0, result.IndexOf("view_more") + 9)
                                End If
                            End If

                        End While
                    Else
                        If IsDate(Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)) Then
                            sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)
                        Else
                            bError = True
                            pTVInfo.Success = False
                        End If
                    End If
                Catch ex As Exception
                    bError = True
                    TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                End Try
                If Not bError Then
                    Try
                        Dim sTVID As String = Mid(result, result.IndexOf("class=""title result"" href=""/tv/") + 32, 20)
                        pTVInfo.TVID = Mid(sTVID, 1, InStr(sTVID, Chr(34)) - 1)
                        pTVInfo.Rating = Mid(result, result.IndexOf("<span class=""vote_average"">") + 28, 3)
                        If IsDate(sDate) Then
                            pTVInfo.ReleaseDate = sDate
                            pTVInfo.ReleaseYear = CType(DateAndTime.Year(CDate(sDate)), String)
                            pTVInfo.ReleaseMonth = CType(DateAndTime.Month(CDate(sDate)), String)
                            pTVInfo.ReleaseDay = CType(DateAndTime.Day(CDate(sDate)), String)
                        End If
                        Dim sGenres As String = Mid(result, result.IndexOf("<span class=""genres"">") + 22, 200)
                        pTVInfo.Genres = Mid(sGenres, 1, InStr(sGenres, "<") - 1)
                        Dim sOverview As String = Mid(result, result.IndexOf("<p class=""overview"">") + 21, 2000)
                        pTVInfo.Overview = Mid(sOverview, 1, InStr(sOverview, "<") - 1)
                        pTVInfo.Success = True
                        ' Write XML data to .kob file so we can retrieve in the future.
                        If pTVInfo.Success AndAlso fileEntries.Count > 0 Then pTVInfo.CreateKodiBuddyFile(pFolderPath)
                    Catch ex As Exception
                        bError = True
                        TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                    End Try
                Else
                    pTVInfo.Success = False
                End If
            Catch ex As Exception
                pTVInfo.Success = False
            End Try
            getTVInfo = pTVInfo
        End Function

    End Class

    Public Class KodiBuddyData
        Private _MoviePath As String
        Public Property MoviePath() As String
            Get
                Return _MoviePath
            End Get
            Set(ByVal value As String)
                _MoviePath = value
            End Set
        End Property
        Private _MovieImportPath As String
        Public Property MovieImportPath() As String
            Get
                Return _MovieImportPath
            End Get
            Set(ByVal value As String)
                _MovieImportPath = value
            End Set
        End Property
        Private _NoOfGenres As String
        Public Property NoOfGenres() As String
            Get
                Return _NoOfGenres
            End Get
            Set(ByVal value As String)
                _NoOfGenres = value
            End Set
        End Property
        Private _GenreOrYear As String
        Public Property GenreOrYear() As String
            Get
                Return _GenreOrYear
            End Get
            Set(ByVal value As String)
                _GenreOrYear = value
            End Set
        End Property
        Private _TVPath As String
        Public Property TVPath() As String
            Get
                Return _TVPath
            End Get
            Set(ByVal value As String)
                _TVPath = value
            End Set
        End Property
        Private _TVImportPath As String
        Public Property TVImportPath() As String
            Get
                Return _TVImportPath
            End Get
            Set(ByVal value As String)
                _TVImportPath = value
            End Set
        End Property
    End Class
End Namespace