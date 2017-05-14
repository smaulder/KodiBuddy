Imports KodiBuddy.Common.Functions

Public Class FrmMovieDetails

    Private _DirectoryPath As String
    Public Property DirectoryPath() As String
        Get
            Return _DirectoryPath
        End Get
        Set(ByVal value As String)
            _DirectoryPath = value
        End Set
    End Property

    Private _parentWindow As Object
    Public Property ParentWindow() As Object
        Get
            Return _parentWindow
        End Get
        Set(ByVal value As Object)
            _parentWindow = value
        End Set
    End Property

    Private Sub FrmMovieDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim movieyear As String = ""
        Dim TxtErrorMessage As New TextBox

        Dim pMovieInfo As Common.Info = getMovieInfo(2, DirectoryPath, movieyear, TxtErrorMessage)
        If pMovieInfo.Success Then

            MovieNameLbl.Text = pMovieInfo.Name
            MovieIDLbl.Text = pMovieInfo.ID
            YearLbl.Text = pMovieInfo.ReleaseYear
            OverviewTxt.Text = pMovieInfo.Overview.Replace("&hellip;", "...")

        End If
        Me.BringToFront()
        ParentWindow.Visible = False

    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Hide()
        ParentWindow.Visible = True
    End Sub
End Class