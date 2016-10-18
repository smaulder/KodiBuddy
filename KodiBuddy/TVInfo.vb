Imports System.IO
Imports System.Xml

Namespace Common


    Public Class TVInfo
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

        Private _TVID As String

        Public Property TVID() As String
            Get
                Return _TVID
            End Get
            Set(ByVal value As String)
                _TVID = value
            End Set
        End Property

        Private _TVName As String

        Public Property TVName() As String
            Get
                Return _TVName
            End Get
            Set(ByVal value As String)
                _TVName = StrConv(value, vbProperCase)
            End Set
        End Property

        'Private _ShortTVName As String

        'Public Property ShortTVName() As String
        '    Get
        '        Return _ShortTVName
        '    End Get
        '    Set(ByVal value As String)
        '        _ShortTVName = StrConv(value, vbProperCase)
        '    End Set
        'End Property

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
            Dim outputfile As String = filepath & "\" & TVName & ".kbtv"
            Dim XMLDoc As New Xml.XmlDocument()

            Dim docNode As XmlNode = XMLDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing)
            XMLDoc.AppendChild(docNode)

            Dim productsNode As XmlNode = XMLDoc.CreateElement("KodiBuddy")
            XMLDoc.AppendChild(productsNode)

            Dim productNode As XmlNode = XMLDoc.CreateElement("TV")
            Dim productAttribute As XmlAttribute = XMLDoc.CreateAttribute("TVID")
            productAttribute.Value = TVID
            productNode.Attributes.Append(productAttribute)
            productsNode.AppendChild(productNode)

            Dim MovieNameNode As XmlNode = XMLDoc.CreateElement("TVName")
            MovieNameNode.AppendChild(XMLDoc.CreateTextNode(TVName))
            productNode.AppendChild(MovieNameNode)
            Dim MovieIDNode As XmlNode = XMLDoc.CreateElement("TVID")
            MovieIDNode.AppendChild(XMLDoc.CreateTextNode(TVID))
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
            'Dim ShortMovieNameNode As XmlNode = XMLDoc.CreateElement("ShortTVName")
            'ShortMovieNameNode.AppendChild(XMLDoc.CreateTextNode(ShortTVName))
            'productNode.AppendChild(ShortMovieNameNode)
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
            TVName = XMLDoc.GetElementsByTagName("TVName").Item(0).InnerText
            TVID = XMLDoc.GetElementsByTagName("TVID").Item(0).InnerText
            Success = CBool(XMLDoc.GetElementsByTagName("Success").Item(0).InnerText)
            ReleaseDate = XMLDoc.GetElementsByTagName("ReleaseDate").Item(0).InnerText
            ReleaseYear = XMLDoc.GetElementsByTagName("ReleaseYear").Item(0).InnerText
            ReleaseMonth = XMLDoc.GetElementsByTagName("ReleaseMonth").Item(0).InnerText
            ReleaseDay = XMLDoc.GetElementsByTagName("ReleaseDay").Item(0).InnerText
            'ShortTVName = XMLDoc.GetElementsByTagName("ShortTVName").Item(0).InnerText
            Rating = XMLDoc.GetElementsByTagName("Rating").Item(0).InnerText
            Genres = XMLDoc.GetElementsByTagName("Genres").Item(0).InnerText
            Overview = XMLDoc.GetElementsByTagName("Overview").Item(0).InnerText
        End Sub
    End Class

End Namespace