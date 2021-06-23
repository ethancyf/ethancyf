

Namespace Component.PassportIssueRegion

    <Serializable()> Public Class PassportIssueRegionModel
        Private _strNationalCode As String
        Private _strNationalDesc As String
        Private _strNationalChineseDesc As String
        Private _strNationalCNDesc As String

        Private _strDESC_Display As String
        Private _strDESC_Display_Chi As String
        Private _strDESC_Display_CN As String

#Region "Constants"

        Public Class National
            Public Const HK As String = "HK"
        End Class

#End Region

        Public Property NationalCode() As String
            Get
                Return _strNationalCode
            End Get
            Set(ByVal value As String)
                _strNationalCode = value
            End Set
        End Property

        Public Property NationalDesc() As String
            Get
                Return _strNationalDesc

            End Get
            Set(ByVal value As String)
                _strNationalDesc = value
            End Set
        End Property

        Public Property NationalDescChineseDesc() As String
            Get
                Return _strNationalChineseDesc

            End Get
            Set(ByVal value As String)
                _strNationalChineseDesc = value
            End Set
        End Property

        Public Property NationalDescCNDesc() As String
            Get
                Return _strNationalCNDesc

            End Get
            Set(ByVal value As String)
                _strNationalCNDesc = value
            End Set
        End Property

        Public ReadOnly Property NationalDisplay(ByVal strLanguage As String) As String
            Get
                Select Case strLanguage.Trim.ToLower
                    Case CultureLanguage.English
                        Return Me._strDESC_Display
                    Case CultureLanguage.TradChinese
                        Return Me._strDESC_Display_Chi
                    Case CultureLanguage.SimpChinese
                        Return Me._strDESC_Display_CN
                    Case Else
                        Throw New Exception(String.Format("NationalDesc.DocIdentityDesc: Unexpected value (strLanguage={0})", strLanguage))
                End Select
            End Get
        End Property

        Public Property NationalDisplay() As String
            Get
                Return _strDESC_Display + " - " + _strDESC_Display_Chi

            End Get
            Set(ByVal value As String)
                _strDESC_Display = value
            End Set
        End Property

        'Public Property NationalDisplayChi() As String
        '    Get
        '        Return _strDESC_Display_Chi

        '    End Get
        '    Set(ByVal value As String)
        '        _strDESC_Display_Chi = value
        '    End Set
        'End Property

        'Public Property NationalDisplayCN() As String
        '    Get
        '        Return _strDESC_Display_CN

        '    End Get
        '    Set(ByVal value As String)
        '        _strDESC_Display_CN = value
        '    End Set
        'End Property


        Public Sub New(ByVal udtPassportIssueRegionModel As PassportIssueRegionModel)
            _strNationalCode = udtPassportIssueRegionModel._strNationalCode
            _strNationalDesc = udtPassportIssueRegionModel._strNationalDesc
            _strNationalChineseDesc = udtPassportIssueRegionModel._strNationalChineseDesc
            _strNationalCNDesc = udtPassportIssueRegionModel._strNationalCNDesc
            _strDESC_Display = udtPassportIssueRegionModel._strDESC_Display
            _strDESC_Display_Chi = udtPassportIssueRegionModel._strDESC_Display_Chi
            _strDESC_Display_CN = udtPassportIssueRegionModel._strDESC_Display_CN

        End Sub

        Public Sub New(ByVal strNationalCode As String, ByVal strNationalDesc As String, ByVal strNationalChineseDesc As String, ByVal strNationalCNDesc As String _
                       , ByVal strDESC_Display As String, ByVal strDESC_Display_Chi As String, ByVal strDESC_Display_CN As String)
            _strNationalCode = strNationalCode
            _strNationalDesc = strNationalDesc
            _strNationalChineseDesc = strNationalChineseDesc
            _strNationalCNDesc = strNationalCNDesc
            _strDESC_Display = strDESC_Display
            _strDESC_Display_Chi = strDESC_Display_Chi
            _strDESC_Display_CN = strDESC_Display_CN
        End Sub

    End Class

End Namespace
