Imports KodiBuddy.Common.Functions
Imports System.IO

Public Class FrmMasterList

    Public Dirs() As String
    Public KodiSettings As Common.KodiBuddyData = Common.Functions.LoadXML()

    Private Sub FrmMasterList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim rootDir As String = String.Empty

        rootDir = KodiSettings.MoviePath
        TreeView1.Nodes.Add(rootDir, rootDir, 1)
        PopulateTreeView(rootDir, TreeView1.Nodes(0))
        TreeView1.Nodes(0).Expand()
        Dim ImageList1 As New ImageList
        Dim i1 As New Icon(My.Resources.Resource1.crystal_xml, 32, 32)
        ImageList1.Images.Add(0, i1)
        Dim i2 As New Icon(My.Resources.Resource1.folder, 32, 32)
        ImageList1.Images.Add(1, i2)
        TreeView1.ImageList = ImageList1
    End Sub

    Private Sub PopulateTreeView(ByVal dir As String, ByVal parentNode As TreeNode)
        Dim folder As String = String.Empty
        Try
            Dim TxtErrorMessage As New TextBox
            Dim folders() As String = IO.Directory.GetDirectories(dir)
            If folders.Length <> 0 Then
                Dim childNode As TreeNode = Nothing
                For Each folder In folders
                    Dim MovieName As String = folder.Remove(0, folder.LastIndexOf("\") + 1)
                    If MovieName <> "$RECYCLE.BIN" AndAlso
                       MovieName <> "extrafanart" AndAlso
                       MovieName <> "extrathumbs" Then
                        Dim fileEntries As String() = Directory.GetFiles(folder, "*.kb")
                        If fileEntries.Count = 1 Then
                            childNode = New TreeNode(MovieName, 0, 0)
                            parentNode.Nodes.Add(childNode)
                        Else
                            If IsNumeric(getMovieYear(folder, TxtErrorMessage)) Then
                                childNode = New TreeNode(MovieName, 0, 0)
                            Else
                                childNode = New TreeNode(MovieName, 1, 1)
                            End If
                            Common.Functions.getMovieInfo(folder, getMovieYear(folder, TxtErrorMessage), TxtErrorMessage)
                            parentNode.Nodes.Add(childNode)
                            PopulateTreeView(folder, childNode)
                        End If
                        Me.Refresh()
                    End If
                Next
            End If
        Catch ex As UnauthorizedAccessException
            parentNode.Nodes.Add(folder & ": Access Denied")
        End Try
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Hide()
    End Sub

    Sub treeView1_NodeMouseClick(ByVal sender As Object,
    ByVal e As TreeNodeMouseClickEventArgs) _
    Handles TreeView1.NodeMouseClick
        Dim txtMessage As New TextBox
        If IsNumeric(Common.Functions.getMovieYear(e.Node.FullPath, txtMessage)) Then
            Dim box = New FrmMovieDetails()
            box.DirectoryPath = e.Node.FullPath
            box.ParentWindow = Me
            box.Show()
        End If
    End Sub 'treeView1_NodeMouseClick
End Class