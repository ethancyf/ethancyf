Namespace Component.Token
    <Serializable()> Public Class TokenModel

        Private _strUserID As String
        Private _strTokenSerialNo As String
        Private _strProject As String
        Private _strIssueBy As String
        Private _strIssueDtm As Nullable(Of DateTime)
        Private _strTokenSerialNoReplacement As String
        Private _strRecordStatus As String
        Private _strUpdateBy As String
        Private _strUpdateDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()
        '[CRE13-003 Token Replacement] [Start] [Karl]
        Private _strLastReplacementDtm As Nullable(Of DateTime)
        Private _strLastReplacementActivateDtm As Nullable(Of DateTime)
        Private _strLastReplacementReason As String
        Private _strLastReplacementBy As String
        '[CRE13-003 Token Replacement] [End] [Karl]
        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Private _strProjectReplacement As String
        Private _blnIsShareTokenReplacement As Nullable(Of Boolean)
        Private _blnIsShareToken As Boolean
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        Public Const UserIDDataType As SqlDbType = SqlDbType.Char
        Public Const UserIDDataSize As Integer = 20

        Public Const TokenSerialNoDataType As SqlDbType = SqlDbType.VarChar
        Public Const TokenSerialNoDataSize As Integer = 20

        Public Const ProjectDataType As SqlDbType = SqlDbType.Char
        Public Const ProjectDataSize As Integer = 10

        Public Const IssueByDataType As SqlDbType = SqlDbType.VarChar
        Public Const IssueByDataSize As Integer = 20

        Public Const IssueDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const IssueDtmDataSize As Integer = 8

        Public Const TokenSerialNoReplacementDataType As SqlDbType = SqlDbType.VarChar
        Public Const TokenSerialNoReplacementDataSize As Integer = 20

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const UpdateDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const UpdateDtmDataSize As Integer = 8

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        '[CRE13-003 Token Replacement] [Start] [Karl]
        Public Const LastReplacementDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const LastReplacementDtmDataSize As Integer = 8

        Public Const LastReplacementActivateDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const LastReplacementActivateDtmDataSize As Integer = 8

        Public Const LastReplacementReasonDataType As SqlDbType = SqlDbType.VarChar
        Public Const LastReplacementReasonDataSize As Integer = 10

        Public Const LastReplacementByDataType As SqlDbType = SqlDbType.VarChar
        Public Const LastReplacementByDataSize As Integer = 20
        '[CRE13-003 Token Replacement] [End] [Karl]

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Const ProjectReplacementDataType As SqlDbType = SqlDbType.Char
        Public Const ProjectReplacementDataSize As Integer = 10

        Public Const IsShareTokenReplacementDataType As SqlDbType = SqlDbType.Char
        Public Const IsShareTokenReplacementDataSize As Integer = 1

        Public Const IsShareTokenDataType As SqlDbType = SqlDbType.Char
        Public Const IsShareTokenDataSize As Integer = 1
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        Public Property UserID() As String
            Get
                Return _strUserID
            End Get
            Set(ByVal value As String)
                _strUserID = value
            End Set
        End Property

        Public Property TokenSerialNo() As String
            Get
                Return _strTokenSerialNo
            End Get
            Set(ByVal value As String)
                _strTokenSerialNo = value
            End Set
        End Property

        Public Property Project() As String
            Get
                Return _strProject
            End Get
            Set(ByVal value As String)
                _strProject = value
            End Set
        End Property

        Public Property IssueBy() As String
            Get
                Return _strIssueBy
            End Get
            Set(ByVal value As String)
                _strIssueBy = value
            End Set
        End Property

        Public Property IssueDtm() As Nullable(Of DateTime)
            Get
                Return _strIssueDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _strIssueDtm = value
            End Set
        End Property

        Public Property TokenSerialNoReplacement() As String
            Get
                Return _strTokenSerialNoReplacement
            End Get
            Set(ByVal value As String)
                _strTokenSerialNoReplacement = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As Nullable(Of DateTime)
            Get
                Return _strUpdateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _strUpdateDtm = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _byteTSMP
            End Get
            Set(ByVal value As Byte())
                _byteTSMP = value
            End Set
        End Property
        '[CRE13-003 Token Replacement] [Start] [Karl]
        Public Property LastReplacementDtm() As Nullable(Of DateTime)
            Get
                Return _strLastReplacementDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _strLastReplacementDtm = value
            End Set
        End Property


        Public Property LastReplacementActivateDtm() As Nullable(Of DateTime)
            Get
                Return _strLastReplacementActivateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _strLastReplacementActivateDtm = value
            End Set
        End Property

        Public Property LastReplacementReason() As String
            Get
                Return _strLastReplacementReason
            End Get
            Set(ByVal value As String)
                _strLastReplacementReason = value
            End Set
        End Property


        Public Property LastReplacementBy() As String
            Get
                Return _strLastReplacementBy
            End Get
            Set(ByVal value As String)
                _strLastReplacementBy = value
            End Set
        End Property
        '[CRE13-003 Token Replacement] [End] [Karl]

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Property ProjectReplacement() As String
            Get
                Return _strProjectReplacement
            End Get
            Set(ByVal value As String)
                _strProjectReplacement = value
            End Set
        End Property

        Public Property IsShareTokenReplacement() As Nullable(Of Boolean)
            Get
                Return _blnIsShareTokenReplacement
            End Get
            Set(ByVal value As Nullable(Of Boolean))
                _blnIsShareTokenReplacement = value
            End Set
        End Property

        Public Property IsShareToken() As Boolean
            Get
                Return _blnIsShareToken
            End Get
            Set(ByVal value As Boolean)
                _blnIsShareToken = value
            End Set
        End Property
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtTokenModel As TokenModel)
            _strUserID = udtTokenModel.UserID
            _strTokenSerialNo = udtTokenModel.TokenSerialNo
            _strProject = udtTokenModel.Project
            _strIssueBy = udtTokenModel.IssueBy
            _strIssueDtm = udtTokenModel.IssueDtm
            _strTokenSerialNoReplacement = udtTokenModel.TokenSerialNoReplacement
            _strRecordStatus = udtTokenModel.RecordStatus
            _strUpdateBy = udtTokenModel.UpdateBy
            _strUpdateDtm = udtTokenModel.UpdateDtm
            _byteTSMP = udtTokenModel.TSMP
            '[CRE13-003 Token Replacement] [Start] [Karl]
            _strLastReplacementDtm = udtTokenModel.LastReplacementDtm
            _strLastReplacementActivateDtm = udtTokenModel.LastReplacementActivateDtm
            _strLastReplacementReason = udtTokenModel.LastReplacementReason
            _strLastReplacementBy = udtTokenModel.LastReplacementBy
            '[CRE13-003 Token Replacement] [End] [Karl]
            ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            _strProjectReplacement = udtTokenModel.ProjectReplacement
            _blnIsShareTokenReplacement = udtTokenModel.IsShareTokenReplacement
            _blnIsShareToken = udtTokenModel.IsShareToken
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
        End Sub

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Public Sub New(ByVal strUserID As String, ByVal strTokenSerialNo As String, ByVal strProject As String, ByVal strIssueBy As String, _
        '               ByVal strIssueDtm As Nullable(Of DateTime), ByVal strTokenSerialNoReplacement As String, ByVal strRecordStatus As String, ByVal strUpdateBy As String, _
        '               ByVal strUpdateDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal strLastReplacementDtm As Nullable(Of DateTime), _
        '               ByVal strLastReplacementActivateDtm As Nullable(Of DateTime), ByVal strLastReplacementReason As String, ByVal strLastReplacementBy As String)
        Public Sub New(ByVal strUserID As String, ByVal strTokenSerialNo As String, ByVal strProject As String, ByVal strIssueBy As String, _
                       ByVal strIssueDtm As Nullable(Of DateTime), ByVal strTokenSerialNoReplacement As String, ByVal strRecordStatus As String, ByVal strUpdateBy As String, _
                       ByVal strUpdateDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal strLastReplacementDtm As Nullable(Of DateTime), _
                       ByVal strLastReplacementActivateDtm As Nullable(Of DateTime), ByVal strLastReplacementReason As String, ByVal strLastReplacementBy As String, _
                       ByVal strProjectReplacement As String, ByVal blnIsShareTokenReplacement As Nullable(Of Boolean), ByVal blnIsShareToken As Boolean)
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

            _strUserID = strUserID
            _strTokenSerialNo = strTokenSerialNo
            _strProject = strProject
            _strIssueBy = strIssueBy
            _strIssueDtm = strIssueDtm
            _strTokenSerialNoReplacement = strTokenSerialNoReplacement
            _strRecordStatus = strRecordStatus
            _strUpdateBy = strUpdateBy
            _strUpdateDtm = strUpdateDtm
            _byteTSMP = byteTSMP
            '[CRE13-003 Token Replacement] [Start] [Karl]
            _strLastReplacementDtm = strLastReplacementDtm
            _strLastReplacementActivateDtm = strLastReplacementActivateDtm
            _strLastReplacementReason = strLastReplacementReason
            _strLastReplacementBy = strLastReplacementBy
            '[CRE13-003 Token Replacement] [End] [Karl]
            ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            _strProjectReplacement = strProjectReplacement
            _blnIsShareTokenReplacement = blnIsShareTokenReplacement
            _blnIsShareToken = blnIsShareToken
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
        End Sub

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]
        'Public Shared Function DisplayTokenSerialNo(ByVal strTokenSN As String, ByVal Project As String) As String
        '    Dim strTokenSerialNo As String = String.Empty

        '    If Not strTokenSN = String.Empty Then
        '        If Project.Trim = TokenProjectType.PPIEPR Then
        '            strTokenSerialNo = "******" + HttpContext.GetGlobalResourceObject("Text", "IssuedByPPIePR")
        '        ElseIf Project.Trim = TokenProjectType.EHCVS Then
        '            strTokenSerialNo = strTokenSN
        '        Else
        '            strTokenSerialNo = strTokenSN
        '        End If
        '    Else
        '        strTokenSerialNo = HttpContext.GetGlobalResourceObject("Text", "N/A")
        '    End If

        '    'strTokenSerialNo += " For Testing Only : (" + strTokenSN + "," + Project + ") "

        '    Return strTokenSerialNo
        'End Function
        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

        ' To display the Token Serial No#
        ' The Input Param [blnIsForceToShowTokenSN] would override [blnIsMaskTokenSN]
        Public Shared Function DisplayTokenSerialNo(ByVal udtToken As TokenModel, ByVal blnIsMaskTokenSN As Boolean, ByVal blnShowDesc As Boolean, ByVal blnIsForceToShowTokenSN As Boolean, Optional ByVal strCultureLanguage As String = "") As String
            Return DisplayTokenSerialNo(udtToken.TokenSerialNo, udtToken.Project, blnIsMaskTokenSN, blnShowDesc, blnIsForceToShowTokenSN, strCultureLanguage)
        End Function

        ' To display the Token Serial No#
        ' The Input Param [blnIsForceToShowTokenSN] would override [blnIsMaskTokenSN]
        Public Shared Function DisplayTokenSerialNo(ByVal strTokenSN As String, ByVal strProject As String, ByVal blnIsMaskTokenSN As Boolean, ByVal blnShowDesc As Boolean, ByVal blnIsForceToShowTokenSN As Boolean, Optional ByVal strCultureLanguage As String = "") As String
            Dim strTokenSN_Result As String = ""
            Dim strDesc As String = ""
            Dim strIssueBy As String = ""
            Dim udtStaticDataBLL As Common.Component.StaticData.StaticDataBLL
            Dim udtStaticData As Common.Component.StaticData.StaticDataModel

            If strTokenSN Is Nothing OrElse strTokenSN.Trim().Equals("") Then
                strTokenSN_Result = HttpContext.GetGlobalResourceObject("Text", "N/A")
            Else
                ' The Token S/N# must be shown
                If blnIsForceToShowTokenSN Then
                    strTokenSN_Result = strTokenSN.Trim()
                Else
                    ' The Token S/N# should be mask if it is NOT issued by eHS
                    If blnIsMaskTokenSN OrElse Not (strProject.Trim().Equals(TokenProjectType.EHCVS)) Then
                        strTokenSN_Result = "******"
                    Else
                        strTokenSN_Result = strTokenSN.Trim()
                    End If
                End If

                ' If true, show the description after the Token S/N#
                If blnShowDesc Then
                    ' To get the description of Token Issue By from [StaticData]
                    udtStaticDataBLL = New Common.Component.StaticData.StaticDataBLL()
                    udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_ISSUE_BY", strProject)
                    Select Case strCultureLanguage
                        Case Common.Component.CultureLanguage.English
                            strIssueBy = CStr(udtStaticData.DataValue)

                        Case Common.Component.CultureLanguage.TradChinese
                            strIssueBy = CStr(udtStaticData.DataValueChi)

                        Case ""
                            strIssueBy = CStr(udtStaticData.DataValue)

                        Case Else
                            Throw New Exception("Error: Class = [Common.Component.Token.TokenModel], Method = [DisplayTokenSerialNo], Message = Input Param - [strCultureLanguage] is invalid")

                    End Select

                    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'If strProject.Trim = TokenProjectType.PPIEPR And strCultureLanguage.Equals("") Then
                    If strProject.Trim <> TokenProjectType.EHCVS And strCultureLanguage.Equals("") Then
                        'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

                        strDesc = "(" + strIssueBy + ")"
                        strTokenSN_Result += " " + strDesc
                    Else
                        ' To apply the format of the description
                        strDesc = HttpContext.GetGlobalResourceObject("Text", "TokenIssueBy_Desc")
                        strDesc = strDesc.Replace("%s", strIssueBy)
                        strTokenSN_Result += " " + strDesc
                    End If

                End If
            End If

            Return strTokenSN_Result
        End Function
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        ' To display the Token Serial No# on SP Platform
        ' The Input Param [blnIsForceToShowTokenSN] would override [blnIsMaskTokenSN]
        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Shared Function DisplayTokenSerialNoSP(ByVal strTokenSN As String, ByVal strProject As String, ByVal blnIsMaskTokenSN As Boolean, ByVal blnShowDesc As Boolean, ByVal blnIsForceToShowTokenSN As Boolean, Optional ByVal strCultureLanguage As String = "") As String
            Dim strTokenSN_Result As String = ""
            Dim strDesc As String = ""
            Dim strIssueBy As String = ""
            Dim udtStaticDataBLL As Common.Component.StaticData.StaticDataBLL
            Dim udtStaticData As Common.Component.StaticData.StaticDataModel

            If strTokenSN Is Nothing OrElse strTokenSN.Trim().Equals("") Then
                strTokenSN_Result = HttpContext.GetGlobalResourceObject("Text", "N/A")
            Else
                ' The Token S/N# must be shown
                If blnIsForceToShowTokenSN Then
                    strTokenSN_Result = strTokenSN.Trim()
                Else
                    ' The Token S/N# should be mask if it is NOT issued by eHS
                    If blnIsMaskTokenSN OrElse Not (strProject.Trim().Equals(TokenProjectType.EHCVS)) Then
                        strTokenSN_Result = "******"
                    Else
                        strTokenSN_Result = strTokenSN.Trim()
                    End If
                End If

                ' If true, show the description after the Token S/N#
                If blnShowDesc Then
                    ' To get the description of Token Issue By from [StaticData]
                    udtStaticDataBLL = New Common.Component.StaticData.StaticDataBLL()
                    udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_ISSUE_BY", strProject)
                    Select Case strCultureLanguage
                        Case Common.Component.CultureLanguage.English
                            strIssueBy = CStr(udtStaticData.DataValue)

                        Case Common.Component.CultureLanguage.TradChinese
                            strIssueBy = CStr(udtStaticData.DataValueChi)

                            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                        Case Common.Component.CultureLanguage.SimpChinese
                            strIssueBy = CStr(udtStaticData.DataValueCN)
                            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                        Case ""
                            strIssueBy = CStr(udtStaticData.DataValue)

                        Case Else
                            Throw New Exception("Error: Class = [Common.Component.Token.TokenModel], Method = [DisplayTokenSerialNo], Message = Input Param - [strCultureLanguage] is invalid")

                    End Select

                    ' To apply the format of the description
                    strDesc = HttpContext.GetGlobalResourceObject("Text", "TokenIssueBy_Desc")
                    strDesc = strDesc.Replace("%s", strIssueBy)
                    strTokenSN_Result += " " + strDesc

                End If
            End If

            Return strTokenSN_Result
        End Function
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

        '[CRE13-003 Token Replacement] [Start] [TommyL]
        Public Function GetLastReplacementReasonDesc(Optional ByVal strCultureLanguage As String = "") As String
            Dim udtStaticDataBLL As New Common.Component.StaticData.StaticDataBLL
            Dim udtStaticData As Common.Component.StaticData.StaticDataModel

            If LastReplacementReason.Equals("") Then
                Return ""
            Else
                udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_REPLACE_REASON", LastReplacementReason)
            End If

            Select Case strCultureLanguage
                Case Common.Component.CultureLanguage.English
                    Return CStr(udtStaticData.DataValue)

                Case Common.Component.CultureLanguage.TradChinese
                    Return CStr(udtStaticData.DataValueChi)

                Case ""
                    Return CStr(udtStaticData.DataValue)

                Case Else
                    Throw New Exception("Error: Class = [Common.Component.Token.TokenModel], Method = [GetLastReplacementReasonDesc], Message = Input Param - [strCultureLanguage] is invalid")

            End Select
        End Function
        '[CRE13-003 Token Replacement] [End] [TommyL]
    End Class
End Namespace


