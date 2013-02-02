'*************************************
'** SearchCloud
'*************************************
'** Designer: Kamran Ayub
'** Website: http://www.intrepidstudios.com/projects/SearchCloud/
'** Credits: Reciprocity (http://reciprocity.be/ctc/) - PHP tag cloud for colors and font size calculations
'** - 
'****************
'** Inputs:
'****************
' ATTRIBUTES
'** - DataSource        : Required      Specifies dataset to use for parsing Keyword Data
'** - DataIDField       : Required      Specifies field name for Keyword ID
'** - DataKeywordField  : Required      Specifies field name for Keyword Title
'** - DataURLField      : Optional      Specifies field name for Keyword URL (if any)
'** - DataCountField    : Required      Specifies field name for Keyword Count
'** - SortBy            : Optional      Sort by a field name, optional DESC (e.g. keyword_title DESC)
'** - MaxFontSize       : Optional      Specifies Max Font Size (Default: 22)
'** - MinFontSize       : Optional      Specifies Min Font Size (Default: 10)
'** - FontUnit          : Optional      Specifies Font Unit (em,px,pt,%) (Default:pt)
'** - MaxColor          : Optional      Specifies Max Color in Hex
'** - MinColor          : Optional      Specifies Min Color in Hex
'** - KeywordTitleFormat : Optional     Specifies a Custom Keyword Title Format
'** - KeywordURLFormat  : Optional      Specifies a Custom Keyword URL Format
'** - CssClass          : Optional      Specifies a CSS Class for the containing Div
'** - Debug             : Optional      Will throw actual exception instead of friendly error

' METHODS
'** - AddAttribute(attr, value)     : Optional      Add a custom HTML attribute like 'onclick' to the keyword link

'*****************
'** Outputs:
'*****************
'** - CloudHTML     :   The output HTML. Will output an error if there has been one.

'***************
'** Notes
'***************
' You can use the following variables in the KeywordTitleFormat and AddAttribute method
' %k = Keyword Title
' %c = Keyword Count
' %u = Keyword URL
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace SearchCloud

    <ToolboxData("<{0}:Cloud runat=server></{0}:Cloud>")> _
        Public Class Cloud
        Inherits WebControl

#Region " Properties "

        ' Apperance Properties
#Region " Appearance "
        <Bindable(True), Category("Appearance"), Localizable(True)> Property MinFontSize() As Integer
            Get
                If CInt(ViewState("MinFontSize")) = 0 Then
                    Return 12 '10
                Else
                    Return CInt(ViewState("MinFontSize"))
                End If

            End Get

            Set(ByVal Value As Integer)
                ViewState("MinFontSize") = Value
            End Set
        End Property
        <Bindable(True), Category("Appearance"), Localizable(True)> Property MaxFontSize() As Integer
            Get
                If CInt(ViewState("MaxFontSize")) = 0 Then
                    Return 30 '22
                Else
                    Return CInt(ViewState("MaxFontSize"))
                End If

            End Get

            Set(ByVal Value As Integer)
                ViewState("MaxFontSize") = Value
            End Set
        End Property
        <Bindable(False), Category("Appearance"), Localizable(True)> Property FontUnit() As String
            Get
                Dim s As String = ViewState("FontUnit")
                If s = String.Empty Then
                    Return "pt"
                Else
                    Return s
                End If

            End Get

            Set(ByVal Value As String)
                Select Case Value
                    Case "pt" : ViewState("FontUnit") = Value
                    Case "em" : ViewState("FontUnit") = Value
                    Case "%" : ViewState("FontUnit") = Value
                    Case "px" : ViewState("FontUnit") = Value
                    Case Else : ViewState("FontUnit") = "px"
                End Select

            End Set
        End Property
        <Bindable(True), Category("Appearance"), Localizable(True)> Property MaxColor() As String
            Get
                Dim s As String = CStr(ViewState("MaxColor"))

                If s = String.Empty Then
                    Return "#00f"
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("MaxColor") = Value
            End Set
        End Property
        <Bindable(True), Category("Appearance"), Localizable(True)> Property MinColor() As String
            Get
                Dim s As String = CStr(ViewState("MinColor"))

                If s = String.Empty Then
                    Return "#A5E543"
                Else
                    Return s
                End If
            End Get

            Set(ByVal Value As String)
                ViewState("MinColor") = Value
            End Set
        End Property
#End Region

        ' Data Properties
#Region " Data "
        <Bindable(True), Category("Data"), DefaultValue("")> Property DataSource() As DataSet
            Get
                Return CType(ViewState("DataSource"), DataSet)
            End Get
            Set(ByVal value As DataSet)
                ViewState("DataSource") = value
            End Set
        End Property
        <Bindable(True), Category("Data"), DefaultValue(""), Localizable(True)> Property DataIDField() As String
            Get
                Return CStr(ViewState("DataIDField"))
            End Get

            Set(ByVal Value As String)
                ViewState("DataIDField") = Value
            End Set
        End Property
        <Bindable(True), Category("Data"), DefaultValue(""), Localizable(True)> Property DataKeywordField() As String
            Get
                Return CStr(ViewState("DataKeywordField"))
            End Get

            Set(ByVal Value As String)
                ViewState("DataKeywordField") = Value
            End Set
        End Property
        <Bindable(True), Category("Data"), Localizable(True)> Property DataURLField() As String
            Get
                Dim s As String = CStr(ViewState("DataURLField"))
                Return s
            End Get

            Set(ByVal Value As String)
                ViewState("DataURLField") = Value
            End Set
        End Property
        <Bindable(True), Category("Data"), DefaultValue(""), Localizable(True)> Property DataCountField() As String
            Get
                Return CStr(ViewState("DataCountField"))
            End Get

            Set(ByVal Value As String)
                ViewState("DataCountField") = Value
            End Set
        End Property
        <Bindable(True), Category("Data"), Localizable(True)> Property KeywordTitleFormat() As String
            Get
                If CStr(ViewState("KeywordTitleFormat")) = String.Empty Then
                    Return "%k = %c"
                Else
                    Return CStr(ViewState("KeywordTitleFormat"))
                End If

            End Get

            Set(ByVal Value As String)
                ViewState("KeywordTitleFormat") = Value
            End Set
        End Property
        <Bindable(True), Category("Data"), Localizable(True)> Property KeywordURLFormat() As String
            Get
                Return CStr(ViewState("KeywordURLFormat"))
            End Get

            Set(ByVal Value As String)
                ViewState("KeywordURLFormat") = Value
            End Set
        End Property
        <Bindable(True), Category("Data"), DefaultValue(""), Localizable(True)> Property SortBy() As String
            Get
                Return CStr(ViewState("SortBy"))
            End Get

            Set(ByVal Value As String)
                ViewState("SortBy") = Value
            End Set
        End Property
#End Region

        <Bindable(False), Category("Debug"), DefaultValue(False), Localizable(True)> Property Debug() As Boolean
            Get
                Dim s As Boolean = CBool(ViewState("Debug"))

                Return s
            End Get

            Set(ByVal Value As Boolean)
                ViewState("Debug") = Value
            End Set
        End Property

        '****************
        '** Private Properties
        '****************
        Private arrAttributes As Hashtable

        Public Property CloudHTML() As String
            Get
                Return CStr(ViewState("CloudHTML"))
            End Get
            Set(ByVal value As String)
                ViewState("CloudHTML") = value
            End Set
        End Property
        Private Property KeyAttributes() As Hashtable
            Get
                Return arrAttributes
            End Get
            Set(ByVal value As Hashtable)
                arrAttributes = value
            End Set
        End Property

#End Region

        ' ** Render Control

        ' Will output something like:
        ' <div [attributes]>
        ' <a href="url" title="title">keyword</a>
        ' [...]
        ' </div>

        Protected Overrides Sub Render(ByVal output As HtmlTextWriter)
            If CloudHTML <> String.Empty Then
                output.WriteBeginTag("div")

                ' Write attributes, if any
                If Not CssClass = String.Empty Then
                    output.WriteAttribute("class", CssClass)
                End If
                output.Write(">") ' Close div

                output.Write(CloudHTML)

                output.WriteEndTag("div")

            Else
                output.Write("There was no generated HTML. An unhandled error must have occurred.")
            End If

        End Sub

        '********************
        '** Main Functions **
        '********************

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            ' Validate Fields
            If DataSource Is Nothing Then
                CloudHTML = "Please specify a DataSet to read from"
                Exit Sub
            End If
            If DataIDField = String.Empty Then
                CloudHTML = "Please specify an ID Data Field"
                Exit Sub
            End If
            If DataKeywordField = String.Empty Then
                CloudHTML = "Please specify a Keyword Data Field"
                Exit Sub
            End If
            If DataCountField = String.Empty Then
                CloudHTML = "Please specify a Keyword Count Field"
                Exit Sub
            End If
            If Not RegularExpressions.Regex.IsMatch(MinColor, "^#([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$") Then
                CloudHTML = "MinColor must be a HEX code and must have leading #. Example: #000 or #ff99cc"
                Exit Sub
            End If
            If Not RegularExpressions.Regex.IsMatch(MaxColor, "^#([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$") Then
                CloudHTML = "MaxColor must be a HEX code and must have leading #. Example: #000 or #ff99cc"
                Exit Sub
            End If

            Try
                Dim sb As New StringBuilder
                Dim dv As New DataView(DataSource.Tables(0))
                Dim row As DataRowView

                ' Sort keywords in descending format
                dv.Sort = String.Format("{0} DESC", DataCountField)

                ' Row count
                Dim Count As Integer = dv.Count

                If Count = 0 Then
                    CloudHTML = "No values to generate cloud"
                    Exit Sub
                End If

                ' Get Largest and Smallest Count Values
                Dim MaxQty As Integer = dv(0).Item(DataCountField)
                Dim MinQty As Integer = dv(dv.Count - 1).Item(DataCountField)

                ' Find range of values
                Dim Spread As Integer = MaxQty - MinQty

                ' We don't want to divide by zero
                If Spread = 0 Then
                    Spread = 1
                End If

                ' Determine font size increment
                Dim FontSpread As Integer = MaxFontSize - MinFontSize
                If FontSpread = 0 Then
                    FontSpread = 1
                End If

                Dim FontStep As Double = FontSpread / Spread

                FontStep = 0.7


                ' Sort alphabetically
                If SortBy <> String.Empty Then
                    dv.Sort = SortBy
                Else
                    dv.Sort = String.Format("{0} ASC", DataKeywordField)
                End If


                ' DEBUG: Max Min Avg
                'sb.Append(String.Format("Max: {0}, Min: {1}, Spread: {2}, MaxColor: {3}, MinColor: {4}, MaxFontSize: {5}, MinFontSize: {6}<br />", MaxQty, MinQty, Spread, MaxColor, MinColor, MaxFontSize, MinFontSize))
                For Each row In dv
                    Dim sKeyID As Integer = row.Item(DataIDField)
                    Dim sKeyWord As String = row.Item(DataKeywordField)
                    Dim sKeyCount As Integer = row.Item(DataCountField)
                    Dim sKeyURL As String
                    Dim ColorRGB As String
                    Dim Weight As Double = MinFontSize + ((sKeyCount - MinQty) * FontStep)
                    Dim FontDiff As Integer = MaxFontSize - MinFontSize
                    Dim ColorWeight As Double = Math.Round(99 * (Weight - MinFontSize) / (FontDiff) + 1)

                    ' Create Color
                    ' Don't want to calculate if colors = each other
                    If MinColor = MaxColor Then
                        ColorRGB = MinColor
                    Else
                        ColorRGB = Colorize(MinColor, MaxColor, ColorWeight)
                    End If

                    ' Handle URL
                    If DataURLField = String.Empty Then
                        If KeywordURLFormat <> String.Empty Then
                            sKeyURL = ReplaceKeyValues(KeywordURLFormat, sKeyID, sKeyWord, "", sKeyCount)
                        Else
                            sKeyURL = "#"
                        End If

                    Else
                        sKeyURL = row.Item(DataURLField)
                    End If
                    'sb.Append(String.Format("<a href=""{0}"" style=""font-size:{1}{5};color:{4};"" title=""{2}""{6}>{3}</a> " & vbNewLine, _
                    '                        sKeyURL, _
                    '                        IIf(Math.Round(Weight) > MaxFontSize, MaxFontSize, Math.Round(Weight)), _
                    '                        ReplaceKeyValues(KeywordTitleFormat, sKeyID, sKeyWord, sKeyURL, sKeyCount), _
                    '                        HttpContext.Current.Server.HtmlEncode(sKeyWord), _
                    '                        ColorRGB, _
                    '                        FontUnit, _
                    '                        GenerateAttributes(sKeyWord, sKeyID, sKeyURL, sKeyCount)))


                    sb.Append(String.Format("<a href=""{0}"" style=""font-size:{1}{5};"" title=""{2}""{6}>{3}</a> " & vbNewLine, _
                                            sKeyURL, _
                                            IIf(Math.Round(Weight) > MaxFontSize, MaxFontSize, Math.Round(Weight)), _
                                            ReplaceKeyValues(KeywordTitleFormat, sKeyID, sKeyWord, sKeyURL, sKeyCount), _
                                            HttpContext.Current.Server.HtmlEncode(sKeyWord), _
                                            ColorRGB, _
                                            FontUnit, _
                                            GenerateAttributes(sKeyWord, sKeyID, sKeyURL, sKeyCount)))

                Next

                CloudHTML = sb.ToString
            Catch ex As Exception
                If Not Debug Then
                    CloudHTML = "Error generating cloud"
                Else
                    Throw ex
                End If
            Finally
                MyBase.OnLoad(e)
            End Try

        End Sub

        ' Add custom HTML attributes
        Public Sub AddAttribute(ByVal value As String, ByVal text As String)
            If KeyAttributes Is Nothing Then
                KeyAttributes = New Hashtable
            End If

            KeyAttributes.Add(value, text)
        End Sub









        Public Function HTML() As String
            ' Validate Fields
            If DataSource Is Nothing Then
                CloudHTML = "Please specify a DataSet to read from"
                Return Nothing
            End If
            If DataIDField = String.Empty Then
                CloudHTML = "Please specify an ID Data Field"
                Return Nothing
            End If
            If DataKeywordField = String.Empty Then
                CloudHTML = "Please specify a Keyword Data Field"
                Return Nothing
            End If
            If DataCountField = String.Empty Then
                CloudHTML = "Please specify a Keyword Count Field"
                Return Nothing
            End If
            If Not RegularExpressions.Regex.IsMatch(MinColor, "^#([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$") Then
                CloudHTML = "MinColor must be a HEX code and must have leading #. Example: #000 or #ff99cc"
                Return Nothing
            End If
            If Not RegularExpressions.Regex.IsMatch(MaxColor, "^#([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$") Then
                CloudHTML = "MaxColor must be a HEX code and must have leading #. Example: #000 or #ff99cc"
                Return Nothing
            End If

            Try
                Dim sb As New StringBuilder
                Dim dv As New DataView(DataSource.Tables(0))
                Dim row As DataRowView

                ' Sort keywords in descending format
                dv.Sort = String.Format("{0} DESC", DataCountField)

                ' Row count
                Dim Count As Integer = dv.Count

                If Count = 0 Then
                    CloudHTML = "No values to generate cloud"
                    Exit Function
                End If

                ' Get Largest and Smallest Count Values
                Dim MaxQty As Integer = dv(0).Item(DataCountField)
                Dim MinQty As Integer = dv(dv.Count - 1).Item(DataCountField)

                ' Find range of values
                Dim Spread As Integer = MaxQty - MinQty

                ' We don't want to divide by zero
                If Spread = 0 Then
                    Spread = 1
                End If

                ' Determine font size increment
                Dim FontSpread As Integer = MaxFontSize - MinFontSize
                If FontSpread = 0 Then
                    FontSpread = 1
                End If

                Dim FontStep As Double = FontSpread / Spread

                'FontStep = 0.7
                FontStep = 3


                ' Sort alphabetically
                If SortBy <> String.Empty Then
                    dv.Sort = SortBy
                Else
                    dv.Sort = String.Format("{0} ASC", DataKeywordField)
                End If


                ' DEBUG: Max Min Avg
                'sb.Append(String.Format("Max: {0}, Min: {1}, Spread: {2}, MaxColor: {3}, MinColor: {4}, MaxFontSize: {5}, MinFontSize: {6}<br />", MaxQty, MinQty, Spread, MaxColor, MinColor, MaxFontSize, MinFontSize))
                For Each row In dv
                    Dim sKeyID As Integer = row.Item(DataIDField)
                    Dim sKeyWord As String = row.Item(DataKeywordField)
                    Dim sKeyCount As Integer = row.Item(DataCountField)
                    Dim sKeyURL As String
                    Dim ColorRGB As String
                    Dim Weight As Double = MinFontSize + ((sKeyCount - MinQty) * FontStep)
                    Dim FontDiff As Integer = MaxFontSize - MinFontSize
                    Dim ColorWeight As Double = Math.Round(99 * (Weight - MinFontSize) / (FontDiff) + 1)

                    ' Create Color
                    ' Don't want to calculate if colors = each other
                    If MinColor = MaxColor Then
                        ColorRGB = MinColor
                    Else
                        ColorRGB = Colorize(MinColor, MaxColor, ColorWeight)
                    End If

                    ' Handle URL
                    If DataURLField = String.Empty Then
                        If KeywordURLFormat <> String.Empty Then
                            sKeyURL = ReplaceKeyValues(KeywordURLFormat, sKeyID, sKeyWord, "", sKeyCount)
                        Else
                            sKeyURL = "#"
                        End If

                    Else
                        sKeyURL = row.Item(DataURLField)
                    End If

                    sb.Append(String.Format("<a href=""{0}"" style=""font-size:{1}{5};"" title=""{2}""{6}>{3}</a> " & vbNewLine, _
                                        sKeyURL, _
                                        IIf(Math.Round(Weight) > MaxFontSize, MaxFontSize, Math.Round(Weight)), _
                                        ReplaceKeyValues(KeywordTitleFormat, sKeyID, sKeyWord, sKeyURL, sKeyCount), _
                                        HttpContext.Current.Server.HtmlEncode(sKeyWord), _
                                        ColorRGB, _
                                        FontUnit, _
                                        GenerateAttributes(sKeyWord, sKeyID, sKeyURL, sKeyCount)))



                    'sb.Append(String.Format("<a href=""{0}"" style=""font-size:{1}{5};color:{4};"" title=""{2}""{6}>{3}</a> " & vbNewLine, _
                    '                        sKeyURL, _
                    '                        IIf(Math.Round(Weight) > MaxFontSize, MaxFontSize, Math.Round(Weight)), _
                    '                        ReplaceKeyValues(KeywordTitleFormat, sKeyID, sKeyWord, sKeyURL, sKeyCount), _
                    '                        HttpContext.Current.Server.HtmlEncode(sKeyWord), _
                    '                        ColorRGB, _
                    '                        FontUnit, _
                    '                        GenerateAttributes(sKeyWord, sKeyID, sKeyURL, sKeyCount)))


                    'sb.Append(String.Format("<a href=""{0}"" style=""font-size:{1}{5};"" title=""{2}""{6}>{3}</a> " & vbNewLine, _
                    '                        sKeyURL, _
                    '                        IIf(Math.Round(Weight) > MaxFontSize, MaxFontSize, Math.Round(Weight)), _
                    '                        ReplaceKeyValues(KeywordTitleFormat, sKeyID, sKeyWord, sKeyURL, sKeyCount), _
                    '                        HttpContext.Current.Server.HtmlEncode(sKeyWord), _
                    '                        ColorRGB, _
                    '                        FontUnit, _
                    '                        GenerateAttributes(sKeyWord, sKeyID, sKeyURL, sKeyCount)))

                Next

                Return sb.ToString
            Catch ex As Exception
                If Not Debug Then
                    CloudHTML = "Error generating cloud"
                Else
                    Throw ex
                End If
            Finally
                '  MyBase.OnLoad(e)
            End Try

        End Function




















        Private Function GenerateAttributes(ByVal k As String, ByVal id As Integer, ByVal u As String, ByVal c As Integer) As String

            If KeyAttributes Is Nothing Then
                Return String.Empty
            End If

            Dim s As New StringBuilder
            Dim keys As ICollection = KeyAttributes.Keys

            For Each key As Object In keys
                s.Append(String.Format(" {0}=""{1}""", key, ReplaceKeyValues(KeyAttributes(key), id, k, u, c)))
            Next

            Return s.ToString
        End Function

        ' Replace %keyvalues with proper value
        Function ReplaceKeyValues(ByVal txt As String, ByVal id As Integer, ByVal k As String, ByVal u As String, ByVal c As Integer) As String
            ' In case using k in javascript
            k = k.Replace("'", "&apos;")

            txt = txt.Replace("%i", id)
            txt = txt.Replace("%k", HttpContext.Current.Server.HtmlEncode(k))
            txt = txt.Replace("%u", u)
            txt = txt.Replace("%c", c)

            Return txt
        End Function

        ' Generate Color Based on Weight
        ' Thanks to Reciprocity for a structure for this code
        Private Function Colorize(ByVal minc As String, ByVal maxc As String, ByVal w As Double) As String
            w = w / 100
            Dim rs, gs, bs As String
            Dim r, g, b As String
            Dim minr, ming, minb, maxr, maxg, maxb As Integer

            ' Make #000 into #000000

            If Len(minc) = 4 Then
                rs = minc.Substring(1, 1)
                gs = minc.Substring(2, 1)
                bs = minc.Substring(3, 1)

                minc = "#" & rs & rs & gs & gs & bs & bs
            End If

            If Len(maxc) = 4 Then
                rs = maxc.Substring(1, 1)
                gs = maxc.Substring(2, 1)
                bs = maxc.Substring(3, 1)

                maxc = "#" & rs & rs & gs & gs & bs & bs
            End If

            minr = CLng("&H" & minc.Substring(1, 2))
            ming = CLng("&H" & minc.Substring(3, 2))
            minb = CLng("&H" & minc.Substring(5, 2))

            maxr = CLng("&H" & maxc.Substring(1, 2))
            maxg = CLng("&H" & maxc.Substring(3, 2))
            maxb = CLng("&H" & maxc.Substring(5, 2))

            r = Hex(Math.Round(((maxr - minr) * w) + minr)).ToString
            g = Hex(Math.Round(((maxg - ming) * w) + ming)).ToString
            b = Hex(Math.Round(((maxb - minb) * w) + minb)).ToString

            If Len(r) = 1 Then r = "0" & r
            If Len(g) = 1 Then g = "0" & g
            If Len(b) = 1 Then b = "0" & b

            Dim color As String
            color = "#" & r & g & b

            Return color
        End Function
    End Class

End Namespace