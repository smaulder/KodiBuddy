Imports System.IO
Imports System.Xml

Namespace Common

    Public Class MovieInfo
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

    Private _MovieName As String

    Public Property MovieName() As String
        Get
            Return _MovieName
        End Get
        Set(ByVal value As String)
            _MovieName = StrConv(value, vbProperCase)
        End Set
    End Property

    Private _ShortMovieName As String

    Public Property ShortMovieName() As String
        Get
            Return _ShortMovieName
        End Get
        Set(ByVal value As String)
            _ShortMovieName = StrConv(value, vbProperCase)
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

        Public Sub CreateKodiBuddyFile(ByRef filepath As String)
            Dim outputfile As String = filepath & "\" & ShortMovieName & ".kb"
            Dim XMLDoc As New Xml.XmlDocument()

            Dim docNode As XmlNode = XMLDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)
            XMLDoc.AppendChild(docNode)

            Dim productsNode As XmlNode = XMLDoc.CreateElement("KodiBuddy")
            XMLDoc.AppendChild(productsNode)

            Dim productNode As XmlNode = XMLDoc.CreateElement("Movie")
            Dim productAttribute As XmlAttribute = XMLDoc.CreateAttribute("MovieID")
            productAttribute.Value = MovieID
            productNode.Attributes.Append(productAttribute)
            productsNode.AppendChild(productNode)

            Dim MovieNameNode As XmlNode = XMLDoc.CreateElement("MovieName")
            MovieNameNode.AppendChild(XMLDoc.CreateTextNode(MovieName))
            productNode.AppendChild(MovieNameNode)
            Dim MovieIDNode As XmlNode = XMLDoc.CreateElement("MovieID")
            MovieIDNode.AppendChild(XMLDoc.CreateTextNode(MovieID))
            productNode.AppendChild(MovieIDNode)
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
            Dim ShortMovieNameNode As XmlNode = XMLDoc.CreateElement("ShortMovieName")
            ShortMovieNameNode.AppendChild(XMLDoc.CreateTextNode(ShortMovieName))
            productNode.AppendChild(ShortMovieNameNode)
            Dim RatingNode As XmlNode = XMLDoc.CreateElement("Rating")
            RatingNode.AppendChild(XMLDoc.CreateTextNode(Rating))
            productNode.AppendChild(RatingNode)
            Dim GenresNode As XmlNode = XMLDoc.CreateElement("Genres")
            GenresNode.AppendChild(XMLDoc.CreateTextNode(Genres))
            productNode.AppendChild(GenresNode)
            Dim OverviewNode As XmlNode = XMLDoc.CreateElement("Overview")
            OverviewNode.AppendChild(XMLDoc.CreateTextNode(Overview))
            productNode.AppendChild(OverviewNode)

            XMLDoc.Save(outputfile)

        End Sub

        Public Sub LoadKodiBuddyFile(ByRef filepath As String)

            Dim XMLDoc As New Xml.XmlDocument()
            XMLDoc.Load(filepath)
            MovieName = XMLDoc.GetElementsByTagName("MovieName").Item(0).InnerText
            MovieID = XMLDoc.GetElementsByTagName("MovieID").Item(0).InnerText
            Success = CBool(XMLDoc.GetElementsByTagName("Success").Item(0).InnerText)
            ReleaseDate = XMLDoc.GetElementsByTagName("ReleaseDate").Item(0).InnerText
            ReleaseYear = XMLDoc.GetElementsByTagName("ReleaseYear").Item(0).InnerText
            ReleaseMonth = XMLDoc.GetElementsByTagName("ReleaseMonth").Item(0).InnerText
            ReleaseDay = XMLDoc.GetElementsByTagName("ReleaseDay").Item(0).InnerText
            ShortMovieName = XMLDoc.GetElementsByTagName("ShortMovieName").Item(0).InnerText
            Rating = XMLDoc.GetElementsByTagName("Rating").Item(0).InnerText
            Genres = XMLDoc.GetElementsByTagName("Genres").Item(0).InnerText
            Overview = XMLDoc.GetElementsByTagName("Overview").Item(0).InnerText
        End Sub

    End Class

End Namespace