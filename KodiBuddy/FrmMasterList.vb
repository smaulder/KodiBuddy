Imports KodiBuddy.Common.Functions
Imports System.IO

Public Class FrmMasterList

    Public Dirs() As String

    Private Sub FrmMasterList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim rootDir As String = String.Empty

        rootDir = My.Settings.MoviePath
        TreeView1.Nodes.Add(rootDir, rootDir, 1)
        PopulateTreeView(rootDir, TreeView1.Nodes(0))
        TreeView1.ExpandAll()
        Dim ImageList1 As New ImageList
        Dim i1 As New Icon("..\..\Images\crystal_xml.ico")
        ImageList1.Images.Add(0, i1)
        Dim i2 As New Icon("..\..\Images\folder.ico")
        ImageList1.Images.Add(1, i2)
        TreeView1.ImageList = ImageList1
    End Sub

    Private Sub PopulateTreeView(ByVal dir As String, ByVal parentNode As TreeNode)
        Dim folder As String = String.Empty
        Try
            Dim folders() As String = IO.Directory.GetDirectories(dir)
            If folders.Length <> 0 Then
                Dim childNode As TreeNode = Nothing
                For Each folder In folders
                    Dim MovieName As String = folder.Remove(0, folder.LastIndexOf("\") + 1)
                    Dim fileEntries As String() = Directory.GetFiles(folder, "*.kb")
                    If fileEntries.Count = 1 Then
                        childNode = New TreeNode(MovieName, 0, 0)
                        parentNode.Nodes.Add(childNode)
                    Else
                        childNode = New TreeNode(MovieName, 1, 1)
                        parentNode.Nodes.Add(childNode)
                        PopulateTreeView(folder, childNode)
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

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect

    End Sub

    Sub treeView1_NodeMouseClick(ByVal sender As Object,
    ByVal e As TreeNodeMouseClickEventArgs) _
    Handles TreeView1.NodeMouseClick

        Dim box = New FrmMovieDetails()
        box.DirectoryPath = e.Node.FullPath
        Me.Hide()
        box.Show()

    End Sub 'treeView1_NodeMouseClick
End Class