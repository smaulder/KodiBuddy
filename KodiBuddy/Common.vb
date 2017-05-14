Imports System.IO
Imports System.Xml
Imports System.Text.RegularExpressions

Namespace Common

    Public Class Functions

        Public Shared Sub SaveXML(ByVal KodiSettings As Common.KodiBuddyData)
            Dim doc As New XmlDocument()
            Dim docNode As XmlNode = doc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)
            doc.AppendChild(docNode)

            Dim KodiBuddyInfo As XmlNode = doc.CreateElement("KodiBuddyInfo")
            Dim KodiBuddyAttribute As XmlAttribute

            KodiBuddyAttribute = doc.CreateAttribute("ImportPath")
            KodiBuddyAttribute.Value = KodiSettings.ImportPath
            KodiBuddyInfo.Attributes.Append(KodiBuddyAttribute)

            KodiBuddyAttribute = doc.CreateAttribute("MovieOutputPath")
            KodiBuddyAttribute.Value = KodiSettings.MovieOutPutPath
            KodiBuddyInfo.Attributes.Append(KodiBuddyAttribute)

            KodiBuddyAttribute = doc.CreateAttribute("TVOutputPath")
            KodiBuddyAttribute.Value = KodiSettings.TVOutPutPath
            KodiBuddyInfo.Attributes.Append(KodiBuddyAttribute)

            KodiBuddyAttribute = doc.CreateAttribute("NoOfGenres")
            KodiBuddyAttribute.Value = KodiSettings.NoOfGenres
            KodiBuddyInfo.Attributes.Append(KodiBuddyAttribute)

            KodiBuddyAttribute = doc.CreateAttribute("GenreOrYear")
            KodiBuddyAttribute.Value = KodiSettings.GenreOrYear
            KodiBuddyInfo.Attributes.Append(KodiBuddyAttribute)

            doc.AppendChild(KodiBuddyInfo)
            If (Not System.IO.Directory.Exists(Application.StartupPath)) Then
                System.IO.Directory.CreateDirectory(Application.StartupPath)
            End If

            doc.Save(Application.StartupPath & "\KodiBuddyInfo.xml")

        End Sub

        Public Shared Function LoadXML() As Common.KodiBuddyData
            Dim output As New Common.KodiBuddyData()
            Dim doc As New XmlDocument()

            doc.Load(Application.StartupPath & "\KodiBuddyInfo.xml")

            Dim root As XmlElement = doc.DocumentElement

            output.ImportPath = root.Attributes("ImportPath").Value
            output.MovieOutPutPath = root.Attributes("MovieOutPutPath").Value
            output.TVOutPutPath = root.Attributes("TVOutPutPath").Value
            output.NoOfGenres = root.Attributes("NoOfGenres").Value
            output.GenreOrYear = root.Attributes("GenreOrYear").Value

            Return output
        End Function

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
                If InStr(ex.Message, "Could not find a part of the path") > 0 Then
                    TxtErrorMessage.Text = "No files found to process."
                Else
                    TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                End If
            End Try

            Return path
        End Function

        Public Shared Function addMovieYear(ByRef CkBxUseParens As Object, ByVal pFolderPath As String, ByVal pMovieYear As String, ByRef TxtErrorMessage As TextBox) As String
            Dim pMovieInfo As Info = getMovieInfo(2, pFolderPath, pMovieYear, TxtErrorMessage)

            If IsDate(pMovieInfo.ReleaseDate) Then
                Return pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1) & " (" & pMovieInfo.ReleaseYear & ")"
            Else
                Return pFolderPath
            End If
        End Function

        Public Shared Function addTVYear(ByRef CkBxUseParens As Object, ByVal pFolderPath As String, ByVal pTVName As String, ByVal pTVYear As String, ByRef TxtErrorMessage As TextBox) As String
            Dim pTVInfo As Info = getTVInfo(pFolderPath, pTVName, pTVYear, TxtErrorMessage)

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

        Public Shared Function getMovieInfo(ByVal Type As Integer, ByVal pFolderPath As String, ByVal pYear As String, ByRef TxtErrorMessage As TextBox, Optional ByVal pName As String = "") As Info
            Dim pInfo As New Info
            Try

                Dim fileEntries As String() = Directory.GetFiles(pFolderPath, "*.kb")
                If fileEntries.Count = 1 Then
                    pInfo.LoadKodiBuddyFile(fileEntries(0))
                    Return pInfo
                    Exit Function
                End If

                fileEntries = Directory.GetFiles(pFolderPath)
                If pName = "" Then
                    pName = pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1).Replace("_", " ").Replace(".", " ")
                    pInfo.Name = pName
                    Dim Pattern As String = "([S##E##])\w+"
                    pInfo.Type = 2
                    For Each m As Match In Regex.Matches(pName, Pattern)
                        pInfo.ShortName = pName
                        getTVEpisodeAndSeason(pInfo.ShortName, pInfo.SeasonEpisode, pInfo.Episode, pInfo.Season, TxtErrorMessage.Text)
                        pName = pName.Substring(0, m.Index).TrimEnd()
                        pInfo.Type = 1
                        Exit For
                    Next

                    Pattern = "([S##])\w+"
                    For Each m As Match In Regex.Matches(pName, Pattern)
                        pInfo.ShortName = pName
                        getTVEpisodeAndSeason(pInfo.ShortName, pInfo.SeasonEpisode, pInfo.Episode, pInfo.Season, TxtErrorMessage.Text)
                        pName = pName.Substring(1, m.Index)
                        pInfo.Type = 1
                        Exit For
                    Next

                    'Get the position of the end of the year
                    Dim iPos = pName.LastIndexOf(")") + 1

                    'Trim anything after the year
                    If Not pName.Length = iPos AndAlso iPos <> 0 Then
                        pName = pName.Remove(iPos, pName.Length - iPos)
                    End If
                    pInfo.Name = pName
                    Dim shortName As String
                    If pName.LastIndexOf("(") > 0 Then
                        shortName = pName.Remove(pName.LastIndexOf("("), pName.LastIndexOf(")") - pName.LastIndexOf("(") + 1).Trim()
                    Else
                        shortName = pName
                    End If
                    pInfo.ShortName = shortName
                Else
                    pInfo.Name = pName
                    pInfo.ShortName = pInfo.Name
                    ' getTVEpisodeAndSeason(pInfo.ShortName, pInfo.SeasonEpisode, pInfo.Episode, pInfo.Season, TxtErrorMessage.Text)
                    pInfo.Type = Type
                End If

                Dim dateyear As String = ""
                Dim webClient As New System.Net.WebClient
                Dim sDate As String = "1800"
                Dim bError As Boolean = False
                Dim result As String = ""
                If Type = 1 Then
                    result = webClient.DownloadString("http://www.themoviedb.org/search/tv?query=" & pInfo.ShortName.Replace(" ", "+"))
                ElseIf Type = 2 Then
                    result = webClient.DownloadString("http://www.themoviedb.org/search/movie?query=" & pInfo.ShortName.Replace(" ", "+"))
                Else
                    ''https://www.themoviedb.org/search?query=legion
                End If
                Try
                    If pYear <> "" And IsDate(pYear) Then
                        sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 100)
                        If Not IsDate(sDate) Then
                            sDate = ""
                        End If
                        While sDate = "" OrElse CInt(pYear) <> Year(CDate(sDate))
                            sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10) '
                            If Not IsDate(sDate) Then
                                result = result.Remove(0, result.IndexOf("<span class=""release_date"">") + 85)
                                sDate = ""
                                If result.IndexOf("<span class=""release_date"">") > 0 Then
                                    'continue on
                                Else
                                    bError = True
                                    TxtErrorMessage.Text = vbCrLf & "Date Not Found for - " & pName & " Verify that the date on the folder is correct." & TxtErrorMessage.Text & vbCrLf
                                    Exit While
                                End If
                            Else

                                If Year(CDate(sDate)) <> CInt(pYear) Then
                                    result = result.Remove(0, result.IndexOf("view_more") + 9)
                                End If
                            End If

                        End While
                    Else
                        If IsDate(Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)) Then

                            sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)
                        Else
                            bError = True
                            pInfo.Success = False
                        End If
                    End If
                Catch ex As Exception
                    bError = True
                    TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                End Try
                If Not bError Then
                    Try
                        pInfo.Type = Type

                        Dim sID As String = ""
                        If Type = 1 Then
                            sID = Mid(result, result.IndexOf("class=""title result"" href=""/tv/") + ("class=""title result"" href=""/tv/").Length + 1, 20)

                        ElseIf Type = 2 Then
                            sID = Mid(result, result.IndexOf("class=""title result"" href=""/movie/") + ("class=""title result"" href=""/movie/").Length + 1, 20)
                        End If

                        pInfo.ID = Mid(sID, 1, InStr(sID, Chr(34)) - 1)
                        pInfo.Rating = Mid(result, result.IndexOf("<span class=""vote_average"">") + 28, 3)
                        If IsDate(sDate) Then
                            pInfo.ReleaseDate = sDate
                            pInfo.ReleaseYear = CType(DateAndTime.Year(CDate(sDate)), String)
                            pInfo.ReleaseMonth = CType(DateAndTime.Month(CDate(sDate)), String)
                            pInfo.ReleaseDay = CType(DateAndTime.Day(CDate(sDate)), String)
                        End If
                        Dim sGenres As String = Mid(result, result.IndexOf("<span class=""genres"">") + 22, 200)
                        pInfo.Genres = Mid(sGenres, 1, InStr(sGenres, "<") - 1)
                        Dim sOverview As String = Mid(result, result.IndexOf("<p class=""overview"">") + 21, 2000)
                        pInfo.Overview = Mid(sOverview, 1, InStr(sOverview, "<") - 1)
                        pInfo.Success = True
                        If pInfo.Type = "2" Then
                            ' Write XML data to .kob file so we can retrieve in the future.
                            If pInfo.Success AndAlso fileEntries.Count > 0 Then pInfo.CreateKodiBuddyFile(pFolderPath)
                        End If
                    Catch ex As Exception
                        bError = True
                        TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                    End Try
                Else
                    pInfo.Success = False
                End If
            Catch ex As Exception
                pInfo.Success = False
            End Try
            getMovieInfo = pInfo
        End Function

        Public Shared Function getTVInfo(ByVal Type As Integer, ByVal pFolderPath As String, ByVal pYear As String, ByRef TxtErrorMessage As TextBox, Optional ByVal pName As String = "") As Info
            Dim pInfo As New Info
            Try

                Dim fileEntries As String() = Directory.GetFiles(pFolderPath, "*.kb")
                If fileEntries.Count = 1 Then
                    pInfo.LoadKodiBuddyFile(fileEntries(0))
                    Return pInfo
                    Exit Function
                End If

                fileEntries = Directory.GetFiles(pFolderPath)
                If pName = "" Then
                    pName = pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1).Replace("_", " ").Replace(".", " ")
                    pInfo.Name = pName
                    Dim Pattern As String = "([S##E##])\w+"
                    pInfo.Type = 2
                    For Each m As Match In Regex.Matches(pName, Pattern)
                        pInfo.ShortName = pName
                        getTVEpisodeAndSeason(pInfo.ShortName, pInfo.SeasonEpisode, pInfo.Episode, pInfo.Season, TxtErrorMessage.Text)
                        pName = pName.Substring(0, m.Index).TrimEnd()
                        pInfo.Type = 1
                        Exit For
                    Next

                    Pattern = "([S##])\w+"
                    For Each m As Match In Regex.Matches(pName, Pattern)
                        pInfo.ShortName = pName
                        getTVEpisodeAndSeason(pInfo.ShortName, pInfo.SeasonEpisode, pInfo.Episode, pInfo.Season, TxtErrorMessage.Text)
                        pName = pName.Substring(1, m.Index)
                        pInfo.Type = 1
                        Exit For
                    Next

                    'Get the position of the end of the year
                    Dim iPos = pName.LastIndexOf(")") + 1

                    'Trim anything after the year
                    If Not pName.Length = iPos AndAlso iPos <> 0 Then
                        pName = pName.Remove(iPos, pName.Length - iPos)
                    End If
                    pInfo.Name = pName
                    Dim shortName As String
                    If pName.LastIndexOf("(") > 0 Then
                        shortName = pName.Remove(pName.LastIndexOf("("), pName.LastIndexOf(")") - pName.LastIndexOf("(") + 1).Trim()
                    Else
                        shortName = pName
                    End If
                    pInfo.ShortName = shortName
                Else
                    pInfo.Name = pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1)
                    pInfo.ShortName = pInfo.Name
                    getTVEpisodeAndSeason(pInfo.ShortName, pInfo.SeasonEpisode, pInfo.Episode, pInfo.Season, TxtErrorMessage.Text)
                    pInfo.Type = Type
                End If

                Dim dateyear As String = ""
                Dim webClient As New System.Net.WebClient
                Dim sDate As String = "1800"
                Dim bError As Boolean = False
                Dim result As String = ""
                If Type = 1 Then
                    result = webClient.DownloadString("http://www.themoviedb.org/search/tv?query=" & pInfo.ShortName.Replace(" ", "+"))
                ElseIf Type = 2 Then
                    result = webClient.DownloadString("http://www.themoviedb.org/search/movie?query=" & pInfo.ShortName.Replace(" ", "+"))
                Else
                    ''https://www.themoviedb.org/search?query=legion
                End If
                Try
                    If pYear <> "" And IsDate(pYear) Then
                        sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 100)
                        If Not IsDate(sDate) Then
                            sDate = ""
                        End If
                        While sDate = "" OrElse CInt(pYear) <> Year(CDate(sDate))
                            sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10) '
                            If Not IsDate(sDate) Then
                                result = result.Remove(0, result.IndexOf("<span class=""release_date"">") + 85)
                                sDate = ""
                                If result.IndexOf("<span class=""release_date"">") > 0 Then
                                    'continue on
                                Else
                                    bError = True
                                    TxtErrorMessage.Text = vbCrLf & "Date Not Found for - " & pName & " Verify that the date on the folder is correct." & TxtErrorMessage.Text & vbCrLf
                                    Exit While
                                End If
                            Else

                                If Year(CDate(sDate)) <> CInt(pYear) Then
                                    result = result.Remove(0, result.IndexOf("view_more") + 9)
                                End If
                            End If

                        End While
                    Else
                        If IsDate(Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)) Then

                            sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)
                        Else
                            bError = True
                            pInfo.Success = False
                        End If
                    End If
                Catch ex As Exception
                    bError = True
                    TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                End Try
                If Not bError Then
                    Try
                        pInfo.Type = Type

                        Dim sID As String = ""
                        If Type = 1 Then
                            sID = Mid(result, result.IndexOf("class=""title result"" href=""/tv/") + ("class=""title result"" href=""/tv/").Length + 1, 20)

                        ElseIf Type = 2 Then
                            sID = Mid(result, result.IndexOf("class=""title result"" href=""/movie/") + ("class=""title result"" href=""/movie/").Length + 1, 20)
                        End If

                        pInfo.ID = Mid(sID, 1, InStr(sID, Chr(34)) - 1)
                        pInfo.Rating = Mid(result, result.IndexOf("<span class=""vote_average"">") + 28, 3)
                        If IsDate(sDate) Then
                            pInfo.ReleaseDate = sDate
                            pInfo.ReleaseYear = CType(DateAndTime.Year(CDate(sDate)), String)
                            pInfo.ReleaseMonth = CType(DateAndTime.Month(CDate(sDate)), String)
                            pInfo.ReleaseDay = CType(DateAndTime.Day(CDate(sDate)), String)
                        End If
                        Dim sGenres As String = Mid(result, result.IndexOf("<span class=""genres"">") + 22, 200)
                        pInfo.Genres = Mid(sGenres, 1, InStr(sGenres, "<") - 1)
                        Dim sOverview As String = Mid(result, result.IndexOf("<p class=""overview"">") + 21, 2000)
                        pInfo.Overview = Mid(sOverview, 1, InStr(sOverview, "<") - 1)
                        pInfo.Success = True
                        If pInfo.Type = "2" Then
                            ' Write XML data to .kob file so we can retrieve in the future.
                            If pInfo.Success AndAlso fileEntries.Count > 0 Then pInfo.CreateKodiBuddyFile(pFolderPath)
                        End If
                    Catch ex As Exception
                        bError = True
                        TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                    End Try
                Else
                    pInfo.Success = False
                End If
            Catch ex As Exception
                pInfo.Success = False
            End Try
            getTVInfo = pInfo
        End Function

        Public Shared Sub getTVEpisodeAndSeason(ByRef pTVName As String, ByRef pSeasonEpisode As String, ByRef pEpisode As String, ByRef pSeason As String, ByRef pErrorMessage As String)
            pErrorMessage = ""
            pEpisode = ""
            pSeason = ""
            Try
                Dim TvParts() As String = pTVName.Replace(".", " ").Replace("_", " ").Split(CType(" ", Char()))
                Dim d As Boolean = False
                Dim e As Boolean = False
                Dim f As Boolean = False
                Dim SeasonEpisode As String = ""
                For Each part In TvParts
                    d = UCase(part) Like ("S##E##")
                    e = UCase(part) Like ("S##")
                    If d Then
                        pSeasonEpisode = UCase(part)
                        pTVName = pTVName.Remove(pTVName.LastIndexOf(part)).Replace(".", " ").Replace("_", " ").Trim()
                        'Get season number from episode
                        pSeason = pSeasonEpisode.Remove(0, 1).Remove(2).TrimStart("0"c)
                        pEpisode = pSeasonEpisode.Remove(0, 4).TrimStart("0"c)
                        Exit For
                    ElseIf e Then
                        pSeason = UCase(part)
                        pTVName = pTVName.Remove(pTVName.LastIndexOf(part)).Replace(".", " ").Replace("_", " ").Trim()
                        'Get season number from episode
                        pSeason = pSeason.Remove(0, 1).TrimStart("0"c)
                        pSeasonEpisode = pSeason
                        pEpisode = ""
                        Exit For
                    End If
                Next

            Catch ex As Exception
                pErrorMessage = ex.Message
            End Try
        End Sub

        Public Shared Sub ProcessTVDirectory(ByVal FullPath As String, ByRef KodiSettings As Common.KodiBuddyData, ByRef txtTVFolderPath As TextBox, ByRef txtMovieFolderPath As TextBox, ByRef TxtMessageDisplay As TextBox, ByRef TxtErrorMessage As TextBox)
            Dim errorCount As Integer = 0
            Dim shortName As String = ""
            Dim Type As String = "1"
            Dim bUpdateFolderName As Boolean = False
            Dim DirName As String = FullPath
            DirName = StrConv(FullPath.Remove(0, DirName.LastIndexOf("\") + 1).Replace("_", " ").Replace(".", " ").Replace("[", "(").Replace("{", "(").Replace("]", ")").Replace("}", ")"), VbStrConv.ProperCase)
            Dim OriginalDirName As String = DirName.Remove(0, DirName.LastIndexOf("\") + 1)
            Dim DirYear As String = DirName.Remove(0, FullPath.LastIndexOf("(") + 1).Trim().Remove(4)
            If Not IsNumeric(DirYear) OrElse (CInt(DirYear) < 1900 Or CInt(DirYear) > Year(Now)) Then
                DirYear = ""
            End If
            Dim iPos = DirName.LastIndexOf(")") + 1
            If (iPos > 0 AndAlso iPos < DirName.Length) OrElse (String.Compare(DirName, OriginalDirName, False) <> 0) Then bUpdateFolderName = True Else bUpdateFolderName = False
            If iPos > 0 AndAlso Not DirName.Length = iPos Then
                DirName = DirName.Remove(iPos, DirName.Length - iPos)
            End If
            Try
                If DirName.LastIndexOf("(") > 0 Then
                    shortName = DirName.Remove(DirName.LastIndexOf("("), DirName.LastIndexOf(")") - DirName.LastIndexOf("(") + 1).Trim()
                Else
                    shortName = DirName
                End If

                Dim pInfo As Common.Info = getTVInfo(Type, FullPath, DirYear, TxtErrorMessage, shortName)
                If pInfo.Success Then

                    DirName = pInfo.ShortName & "\Season " & pInfo.Season
                    pInfo.Name = DirName

                    'Rename Files Before Folder
                    Dim fileEntries As String() = Directory.GetFiles(FullPath)
                    ' Process the list of files found in the directory.
                    Dim fileName As String
                    If fileEntries.Count > 0 Then
                        For Each fileName In fileEntries
                            Dim x As FileInfo = New FileInfo(fileName)
                            If UCase(x.Extension.ToString()) = UCase(".mp4") Or UCase(x.Extension.ToString()) = UCase(".avi") Or UCase(x.Extension.ToString()) = UCase(".mkv") Or UCase(x.Extension.ToString()) = UCase(".srt") Then

                                Dim tmpFileName As String = fileName.Remove(0, fileName.LastIndexOf("\") + 1)
                                Dim iPos1 As Integer = tmpFileName.LastIndexOf(pInfo.ReleaseYear) - 1
                                Dim iPos2 As Integer = tmpFileName.LastIndexOf(".")
                                If iPos1 > 0 Then
                                    Dim tmpfile2 As String = ""
                                    Dim pTVName As String = fileName
                                    Dim pSeasonEpisode As String = ""
                                    Dim pEpisode As String = ""
                                    Dim pSeason As String = ""
                                    Dim pErrorMessage As String = ""
                                    getTVEpisodeAndSeason(pTVName, pSeasonEpisode, pEpisode, pSeason, pErrorMessage)
                                    tmpfile2 = pSeasonEpisode & Microsoft.VisualBasic.Right(tmpFileName, tmpFileName.Length - iPos2)
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
                                    Dim pTVName As String = fileName.Remove(0, fileName.LastIndexOf("\") + 1)
                                    Dim pSeasonEpisode As String = ""
                                    Dim pEpisode As String = ""
                                    Dim pSeason As String = ""
                                    Dim pErrorMessage As String = ""
                                    getTVEpisodeAndSeason(pTVName, pSeasonEpisode, pEpisode, pSeason, pErrorMessage)
                                    If pErrorMessage.Length > 0 Then
                                        TxtErrorMessage.Text = pErrorMessage & vbCrLf & TxtErrorMessage.Text
                                    End If
                                    If pSeasonEpisode <> "" Then
                                        tmpFileName = pSeasonEpisode & x.Extension
                                    Else
                                        tmpFileName = pTVName & x.Extension
                                    End If
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
                                    Try
                                        My.Computer.FileSystem.DeleteFile(fileName)
                                        TxtErrorMessage.Text = "Deleted File " & fileName & vbCrLf & TxtErrorMessage.Text
                                    Catch ex As Exception
                                        TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                                    End Try
                                End If
                            End If
                        Next fileName
                    End If

                    'Use genres to move file to final folder.
                    Dim Genres() As String = Split(pInfo.Genres, ",")
                    Dim DestFolder As String = ""
                    Dim SourceFolder As String = ""
                    Dim iCnt As Integer = 0

                    If Microsoft.VisualBasic.Right(txtTVFolderPath.Text, 1) = "\" Then
                        DestFolder = txtTVFolderPath.Text
                    Else
                        DestFolder = txtTVFolderPath.Text & "\"
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
                        DestFolder = DestFolder & DirName
                    Else
                        DestFolder = DestFolder & "\" & DirName
                    End If

                    If Not Directory.Exists(DestFolder) Then
                        Directory.CreateDirectory(DestFolder)
                    End If
                    For Each fileName In Directory.GetFiles(FullPath)
                        Try
                            Dim DestFile As String = DestFolder + "\" + Path.GetFileName(fileName)
                            File.Move(fileName, DestFile)
                        Catch ex As Exception
                            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                        End Try
                    Next
                    If Directory.Exists(FullPath) AndAlso Directory.GetFiles(FullPath).Count = 0 Then
                        Try
                            Directory.Delete(DestFolder)
                        Catch ex As Exception
                            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
                        End Try
                    End If

                    TxtMessageDisplay.Text = DirName & vbCrLf & TxtMessageDisplay.Text
                Else
                    errorCount = errorCount + 1
                    TxtErrorMessage.Text = "Failed to load info for - " & DirName & vbCrLf & TxtErrorMessage.Text
                End If

            Catch ex As Exception
                TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
            End Try
        End Sub

        Public Shared Sub ProcessMovieDirectory(ByVal FullPath As String, ByRef KodiSettings As Common.KodiBuddyData, ByRef txtTVFolderPath As TextBox, ByRef txtMovieFolderPath As TextBox, ByRef TxtMessageDisplay As TextBox, ByRef TxtErrorMessage As TextBox)
            Dim bNeedYear As Boolean = False
            Dim shortName As String = ""
            Dim errorCount As Integer = 0
            Dim Type As String = "2"
            Dim bUpdateFolderName As Boolean = False
            Dim DirName As String = FullPath
            DirName = StrConv(DirName.Remove(0, DirName.LastIndexOf("\") + 1).Replace("_", " ").Replace(".", " ").Replace("[", "(").Replace("{", "(").Replace("]", ")").Replace("}", ")"), VbStrConv.ProperCase)
            Dim OriginalDirName As String = DirName.Remove(0, DirName.LastIndexOf("\") + 1)
            Dim DirYear As String = DirName.Remove(0, DirName.LastIndexOf("(") + 1).Trim().Remove(4)
            If Not IsNumeric(DirYear) OrElse (CInt(DirYear) < 1900 Or CInt(DirYear) > Year(Now)) Then
                DirYear = ""
                bNeedYear = True
            End If
            Dim iPos = DirName.LastIndexOf(")") + 1
            If (iPos > 0 AndAlso iPos < DirName.Length) OrElse (String.Compare(DirName, OriginalDirName, False) <> 0) Then bUpdateFolderName = True Else bUpdateFolderName = False
            If iPos > 0 AndAlso Not DirName.Length = iPos Then
                DirName = DirName.Remove(iPos, DirName.Length - iPos)
            End If
            Try

                If DirName.LastIndexOf("(") > 0 Then
                    shortName = DirName.Remove(DirName.IndexOf("("), DirName.Length - DirName.IndexOf("(")).Trim()
                Else
                    shortName = DirName
                End If

                Dim pInfo As Common.Info = getMovieInfo(Type, FullPath, DirYear, TxtErrorMessage, shortName)
                If pInfo.Success Then

                    If bNeedYear Then
                        'If pInfo.Type = "1" Then
                        'DirName = DirName & "\Season " & pInfo.Season
                        'ElseIf pInfo.Type = "2" Then
                        DirName = DirName & " (" & pInfo.ReleaseYear & ")"
                        'End If
                        pInfo.Name = DirName
                    End If

                    'Rename Files Before Folder
                    Dim fileEntries As String() = Directory.GetFiles(FullPath)
                    ' Process the list of files found in the directory.
                    Dim fileName As String
                    If fileEntries.Count > 0 Then
                        For Each fileName In fileEntries
                            Dim x As FileInfo = New FileInfo(fileName)
                            If UCase(x.Extension.ToString()) = UCase(".mp4") Or UCase(x.Extension.ToString()) = UCase(".avi") Or UCase(x.Extension.ToString()) = UCase(".mkv") Or UCase(x.Extension.ToString()) = UCase(".srt") Then

                                Dim tmpFileName As String = fileName.Remove(0, fileName.LastIndexOf("\") + 1)
                                Dim iPos1 As Integer = tmpFileName.LastIndexOf(pInfo.ReleaseYear) - 1
                                Dim iPos2 As Integer = tmpFileName.LastIndexOf(".")
                                If iPos1 > 0 Then
                                    Dim tmpfile2 As String = ""
                                    'If pInfo.Type = "1" Then
                                    'Dim pTVName As String = fileName
                                    '    Dim pSeasonEpisode As String = ""
                                    '    Dim pEpisode As String = ""
                                    '    Dim pSeason As String = ""
                                    '    Dim pErrorMessage As String = ""
                                    '    getTVEpisodeAndSeason(pTVName, pSeasonEpisode, pEpisode, pSeason, pErrorMessage)
                                    '    tmpfile2 = pSeasonEpisode & Microsoft.VisualBasic.Right(tmpFileName, tmpFileName.Length - iPos2)
                                    'Else
                                    tmpfile2 = DirName & Microsoft.VisualBasic.Right(tmpFileName, tmpFileName.Length - iPos2)
                                    'End If
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
                                    'If pInfo.Type = "1" Then
                                    'Dim pTVName As String = fileName.Remove(0, fileName.LastIndexOf("\") + 1)
                                    '    Dim pSeasonEpisode As String = ""
                                    '    Dim pEpisode As String = ""
                                    '    Dim pSeason As String = ""
                                    '    Dim pErrorMessage As String = ""
                                    '    getTVEpisodeAndSeason(pTVName, pSeasonEpisode, pEpisode, pSeason, pErrorMessage)
                                    '    If pSeasonEpisode <> "" Then
                                    '        tmpFileName = pSeasonEpisode & x.Extension
                                    '    Else
                                    '        tmpFileName = pTVName & x.Extension
                                    '    End If
                                    'Else
                                    tmpFileName = DirName & x.Extension
                                    'End If
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
                    Dim Genres() As String = Split(pInfo.Genres, ",")
                    Dim DestFolder As String = ""
                    Dim SourceFolder As String = ""
                    Dim iCnt As Integer = 0

                    'If pInfo.Type = "1" Then
                    '   If Microsoft.VisualBasic.Right(txtTVFolderPath.Text, 1) = "\" Then
                    '        DestFolder = txtTVFolderPath.Text
                    '    Else
                    '        DestFolder = txtTVFolderPath.Text & "\"
                    '    End If
                    'Else
                    For Each Genre As String In Genres
                        DestFolder = DestFolder & "-" & Genre.Trim()
                        iCnt = iCnt + 1
                        If iCnt >= CInt(KodiSettings.NoOfGenres) Then
                            Exit For
                        End If
                    Next

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
                            DestFolder = txtMovieFolderPath.Text & pInfo.ReleaseYear
                        Else
                            DestFolder = txtMovieFolderPath.Text & "\" & pInfo.ReleaseYear
                        End If
                    End If
                    'End If

                    'Create Destination Directory if missing 
                    If Not Directory.Exists(DestFolder) Then
                        Try
                            Dim di As DirectoryInfo = Directory.CreateDirectory(DestFolder)
                        Catch e2 As Exception
                            TxtErrorMessage.Text = e2.Message & vbCrLf & TxtErrorMessage.Text
                        End Try
                    End If

                    If Microsoft.VisualBasic.Right(DestFolder, 1) = "\" Then
                        DestFolder = DestFolder & DirName
                    Else
                        DestFolder = DestFolder & "\" & DirName
                    End If

                    'If pInfo.Type = "1" Then
                    'If Not Directory.Exists(DestFolder) Then
                    '        Directory.CreateDirectory(DestFolder)
                    '    End If
                    '    For Each fileName In Directory.GetFiles(Dir)
                    '        Dim DestFile As String = DestFolder + "\" + Path.GetFileName(fileName)
                    '        File.Move(fileName, DestFile)
                    '    Next
                    'Else
                    If Not Directory.Exists(DestFolder) AndAlso Directory.Exists(FullPath) Then
                        Try
                            Directory.Move(FullPath, DestFolder)
                        Catch e2 As Exception
                            TxtErrorMessage.Text = e2.Message & vbCrLf & TxtErrorMessage.Text
                        End Try
                    Else
                        errorCount = errorCount + 1
                        'Message folder not found.
                        If Directory.Exists(DestFolder) Then
                            TxtErrorMessage.Text = "Duplicate folder exists. - " & DestFolder & vbCrLf & TxtErrorMessage.Text
                        End If
                        If Not Directory.Exists(FullPath) Then
                            TxtErrorMessage.Text = "Source folder not found. - " & SourceFolder & vbCrLf & TxtErrorMessage.Text
                        End If
                    End If
                    'End If

                    TxtMessageDisplay.Text = DirName & vbCrLf & TxtMessageDisplay.Text
                Else
                    errorCount = errorCount + 1
                    TxtErrorMessage.Text = "Failed to load info for - " & DirName & vbCrLf & TxtErrorMessage.Text
                End If
            Catch ex As Exception
                TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
            End Try
        End Sub

        Public Shared Function IsTVShow(ByVal DirName As String) As Boolean
            DirName = StrConv(DirName.Remove(0, DirName.LastIndexOf("\") + 1).Replace("_", " ").Replace(".", " ").Replace("[", "(").Replace("{", "(").Replace("]", ")").Replace("}", ")"), VbStrConv.ProperCase)
            ''http://regexr.com/
            Dim Pattern As String = "([S]\d{0,99}[E]\d{0,99})\w"
            Dim Type As Boolean = False
            For Each m As Match In Regex.Matches(DirName, Pattern)
                DirName = DirName.Substring(0, m.Index).TrimEnd()
                Type = True
                Exit For
            Next

            Pattern = "([S]\d{0,99})\s+"
            For Each m As Match In Regex.Matches(DirName, Pattern)
                DirName = DirName.Substring(1, m.Index)
                Type = True
                Exit For
            Next

            Return Type
        End Function

        'Public Shared Function getTVInfo(ByVal pFolderPath As String, ByVal pTVName As String, ByVal pTVYear As String, ByRef TxtErrorMessage As TextBox) As Info
        '    Dim pTVInfo As New Info
        '    Try

        '        Dim fileEntries As String() = Directory.GetFiles(pFolderPath, "*.kbtv")
        '        If fileEntries.Count = 1 Then
        '            pTVInfo.LoadKodiBuddyFile(fileEntries(0))
        '            Return pTVInfo
        '            Exit Function
        '        End If

        '        fileEntries = Directory.GetFiles(pFolderPath)

        '        Dim pTVName2 As String = pFolderPath.Remove(0, pFolderPath.LastIndexOf("\") + 1).Replace("_", " ")

        '        'Get the position of the end of the year
        '        Dim iPos = pTVName2.LastIndexOf(")") + 1

        '        'Trim anything after the year
        '        If Not pTVName2.Length = iPos AndAlso iPos <> 0 Then
        '            pTVName2 = pTVName2.Remove(iPos, pTVName2.Length - iPos)
        '        End If

        '        pTVInfo.Name = pTVName

        '        'Dim shortTVName As String
        '        'If pTVName2.LastIndexOf("(") > 0 Then
        '        '    shortTVName = pTVName2.Remove(pTVName2.LastIndexOf("("), pTVName2.LastIndexOf(")") - pTVName2.LastIndexOf("(") + 1).Trim()
        '        'Else
        '        '    shortTVName = pTVName2
        '        'End If
        '        'pTVInfo.ShortTVName = shortTVName

        '        Dim dateyear As String = ""
        '        Dim webClient As New System.Net.WebClient
        '        Dim sDate As String = "1800"
        '        Dim bError As Boolean = False
        '        Dim result As String = webClient.DownloadString("https://www.themoviedb.org/search?query=" & pTVName.Replace(" ", "+"))
        '        Try
        '            If pTVYear <> "" And IsDate(pTVYear) Then
        '                sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)
        '                If Not IsDate(sDate) Then
        '                    sDate = ""
        '                End If
        '                While sDate = "" OrElse CInt(pTVYear) <> Year(CDate(sDate))
        '                    sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10) '
        '                    If Not IsDate(sDate) Then
        '                        result = result.Remove(0, result.IndexOf("<span class=""release_date"">") + 85)
        '                        sDate = ""
        '                        If result.IndexOf("<span class=""release_date"">") > 0 Then
        '                            'continue on
        '                        Else
        '                            bError = True
        '                            TxtErrorMessage.Text = vbCrLf & "Date Not Found for - " & pTVName & " Verify that the date on the folder is correct." & TxtErrorMessage.Text & vbCrLf
        '                            Exit While
        '                        End If
        '                    Else

        '                        If Year(CDate(sDate)) <> CInt(pTVYear) Then
        '                            result = result.Remove(0, result.IndexOf("view_more") + 9)
        '                        End If
        '                    End If

        '                End While
        '            Else
        '                If IsDate(Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)) Then
        '                    sDate = Mid(result, result.IndexOf("<span class=""release_date"">") + 85, 10)
        '                Else
        '                    bError = True
        '                    pTVInfo.Success = False
        '                End If
        '            End If
        '        Catch ex As Exception
        '            bError = True
        '            TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
        '        End Try
        '        If Not bError Then
        '            Try
        '                Dim sTVID As String = Mid(result, result.IndexOf("class=""title result"" href=""/tv/") + 32, 20)
        '                pTVInfo.ID = Mid(sTVID, 1, InStr(sTVID, Chr(34)) - 1)
        '                pTVInfo.Rating = Mid(result, result.IndexOf("<span class=""vote_average"">") + 28, 3)
        '                If IsDate(sDate) Then
        '                    pTVInfo.ReleaseDate = sDate
        '                    pTVInfo.ReleaseYear = CType(DateAndTime.Year(CDate(sDate)), String)
        '                    pTVInfo.ReleaseMonth = CType(DateAndTime.Month(CDate(sDate)), String)
        '                    pTVInfo.ReleaseDay = CType(DateAndTime.Day(CDate(sDate)), String)
        '                End If
        '                Dim sGenres As String = Mid(result, result.IndexOf("<span class=""genres"">") + 22, 200)
        '                pTVInfo.Genres = Mid(sGenres, 1, InStr(sGenres, "<") - 1)
        '                Dim sOverview As String = Mid(result, result.IndexOf("<p class=""overview"">") + 21, 2000)
        '                pTVInfo.Overview = Mid(sOverview, 1, InStr(sOverview, "<") - 1)
        '                pTVInfo.Success = True
        '                ' Write XML data to .kob file so we can retrieve in the future.
        '                If pTVInfo.Success AndAlso fileEntries.Count > 0 Then pTVInfo.CreateKodiBuddyFile(pFolderPath)
        '            Catch ex As Exception
        '                bError = True
        '                TxtErrorMessage.Text = ex.Message & vbCrLf & TxtErrorMessage.Text
        '            End Try
        '        Else
        '            pTVInfo.Success = False
        '        End If
        '    Catch ex As Exception
        '        pTVInfo.Success = False
        '    End Try
        '    getTVInfo = pTVInfo
        'End Function

    End Class

    Public Class KodiBuddyData
        Private _ImportPath As String
        Public Property ImportPath() As String
            Get
                Return _ImportPath
            End Get
            Set(ByVal value As String)
                _ImportPath = value
            End Set
        End Property
        Private _MovieOutPutPath As String
        Public Property MovieOutPutPath() As String
            Get
                Return _MovieOutPutPath
            End Get
            Set(ByVal value As String)
                _MovieOutPutPath = value
            End Set
        End Property
        Private _TVOutPutPath As String
        Public Property TVOutPutPath() As String
            Get
                Return _TVOutPutPath
            End Get
            Set(ByVal value As String)
                _TVOutPutPath = value
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
    End Class


    Public Class Info
        Private _Success As Boolean
        Private _ReleaseDate As String
        Private _ReleaseYear As String
        Private _ReleaseMonth As String
        Private _ReleaseDay As String
        Private _ID As String
        Private _Name As String
        Private _ShortName As String
        Private _Rating As String
        Private _Genres As String
        Private _Overview As String
        Private _Season As String
        Private _Type As String
        Private _Episode As String
        Private _SeasonEpisode As String

        Public Property Success() As Boolean
            Get
                Return _Success
            End Get
            Set(ByVal value As Boolean)
                _Success = value
            End Set
        End Property

        Public Property ReleaseDate() As String
            Get
                Return _ReleaseDate
            End Get
            Set(ByVal value As String)
                _ReleaseDate = value
            End Set
        End Property

        Public Property ReleaseYear() As String
            Get
                Return _ReleaseYear
            End Get
            Set(ByVal value As String)
                _ReleaseYear = value
            End Set
        End Property

        Public Property ReleaseMonth() As String
            Get
                Return _ReleaseMonth
            End Get
            Set(ByVal value As String)
                _ReleaseMonth = value
            End Set
        End Property

        Public Property ReleaseDay() As String
            Get
                Return _ReleaseDay
            End Get
            Set(ByVal value As String)
                _ReleaseDay = value
            End Set
        End Property

        Public Property SeasonEpisode() As String
            Get
                Return _SeasonEpisode
            End Get
            Set(ByVal value As String)
                _SeasonEpisode = value
            End Set
        End Property

        Public Property Season() As String
            Get
                Return _Season
            End Get
            Set(ByVal value As String)
                _Season = value
            End Set
        End Property

        Public Property Episode() As String
            Get
                Return _Episode
            End Get
            Set(ByVal value As String)
                _Episode = value
            End Set
        End Property

        Public Property ID() As String
            Get
                Return _ID
            End Get
            Set(ByVal value As String)
                _ID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = StrConv(value, vbProperCase)
            End Set
        End Property

        Public Property ShortName() As String
            Get
                Return _ShortName
            End Get
            Set(ByVal value As String)
                _ShortName = StrConv(value, vbProperCase)
            End Set
        End Property

        Public Property Rating() As String
            Get
                Return _Rating
            End Get
            Set(ByVal value As String)
                _Rating = value
            End Set
        End Property

        Public Property Genres() As String
            Get
                Return _Genres
            End Get
            Set(ByVal value As String)
                _Genres = value
            End Set
        End Property

        Public Property Overview() As String
            Get
                Return _Overview
            End Get
            Set(ByVal value As String)
                _Overview = value
            End Set
        End Property

        Public Property Type() As String
            Get
                Return _Type
            End Get
            Set(ByVal value As String)
                _Type = value
            End Set
        End Property

        Public Sub CreateKodiBuddyFile(ByRef filepath As String)
            Dim outputfile As String = filepath & "\" & ShortName & ".kb"
            Dim XMLDoc As New Xml.XmlDocument()

            Dim docNode As XmlNode = XMLDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)
            XMLDoc.AppendChild(docNode)

            Dim productsNode As XmlNode = XMLDoc.CreateElement("KodiBuddy")
            XMLDoc.AppendChild(productsNode)

            Dim productNode As XmlNode = XMLDoc.CreateElement("Movie")
            Dim productAttribute As XmlAttribute = XMLDoc.CreateAttribute("ID")
            productAttribute.Value = ID
            productNode.Attributes.Append(productAttribute)
            productsNode.AppendChild(productNode)

            Dim NameNode As XmlNode = XMLDoc.CreateElement("Name")
            NameNode.AppendChild(XMLDoc.CreateTextNode(Name))
            productNode.AppendChild(NameNode)
            Dim IDNode As XmlNode = XMLDoc.CreateElement("ID")
            IDNode.AppendChild(XMLDoc.CreateTextNode(ID))
            productNode.AppendChild(IDNode)
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
            Dim ShortNameNode As XmlNode = XMLDoc.CreateElement("ShortName")
            ShortNameNode.AppendChild(XMLDoc.CreateTextNode(ShortName))
            productNode.AppendChild(ShortNameNode)
            Dim RatingNode As XmlNode = XMLDoc.CreateElement("Rating")
            RatingNode.AppendChild(XMLDoc.CreateTextNode(Rating))
            productNode.AppendChild(RatingNode)
            Dim GenresNode As XmlNode = XMLDoc.CreateElement("Genres")
            GenresNode.AppendChild(XMLDoc.CreateTextNode(Genres))
            productNode.AppendChild(GenresNode)
            Dim OverviewNode As XmlNode = XMLDoc.CreateElement("Overview")
            OverviewNode.AppendChild(XMLDoc.CreateTextNode(Overview))
            productNode.AppendChild(OverviewNode)
            Dim TypeNode As XmlNode = XMLDoc.CreateElement("Type")
            OverviewNode.AppendChild(XMLDoc.CreateTextNode(Type))
            productNode.AppendChild(OverviewNode)
            Dim SeasonNode As XmlNode = XMLDoc.CreateElement("Season")
            OverviewNode.AppendChild(XMLDoc.CreateTextNode(Season))
            productNode.AppendChild(OverviewNode)
            Dim EpisodeNode As XmlNode = XMLDoc.CreateElement("Episode")
            OverviewNode.AppendChild(XMLDoc.CreateTextNode(Episode))
            productNode.AppendChild(OverviewNode)
            XMLDoc.Save(outputfile)
        End Sub

        Public Sub LoadKodiBuddyFile(ByRef filepath As String)

            Dim XMLDoc As New Xml.XmlDocument()
            XMLDoc.Load(filepath)
            Name = XMLDoc.GetElementsByTagName("Name").Item(0).InnerText
            ID = XMLDoc.GetElementsByTagName("ID").Item(0).InnerText
            Success = CBool(XMLDoc.GetElementsByTagName("Success").Item(0).InnerText)
            ReleaseDate = XMLDoc.GetElementsByTagName("ReleaseDate").Item(0).InnerText
            ReleaseYear = XMLDoc.GetElementsByTagName("ReleaseYear").Item(0).InnerText
            ReleaseMonth = XMLDoc.GetElementsByTagName("ReleaseMonth").Item(0).InnerText
            ReleaseDay = XMLDoc.GetElementsByTagName("ReleaseDay").Item(0).InnerText
            ShortName = XMLDoc.GetElementsByTagName("ShortName").Item(0).InnerText
            Rating = XMLDoc.GetElementsByTagName("Rating").Item(0).InnerText
            Genres = XMLDoc.GetElementsByTagName("Genres").Item(0).InnerText
            Overview = XMLDoc.GetElementsByTagName("Overview").Item(0).InnerText
        End Sub

    End Class

End Namespace