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
    Private Sub FrmMovieDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim movieyear As String = ""
        Dim TxtErrorMessage As New TextBox

        Dim pMovieInfo As Common.MovieInfo = getMovieInfo(DirectoryPath, movieyear, TxtErrorMessage)
        If pMovieInfo.Success Then

            MovieNameLbl.Text = pMovieInfo.MovieName
            MovieIDLbl.Text = pMovieInfo.MovieID
            YearLbl.Text = pMovieInfo.ReleaseYear
            OverviewTxt.Text = pMovieInfo.Overview.Replace("&hellip;", "...")

        End If

    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Hide()
        Dim box = New FrmMasterList()
        box.Show()
    End Sub
End Class