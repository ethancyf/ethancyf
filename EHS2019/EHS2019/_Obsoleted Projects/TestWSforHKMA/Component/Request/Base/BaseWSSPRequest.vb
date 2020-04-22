Imports Microsoft.VisualBasic
Imports System.Xml
Imports TestWSforHKMA.Component

Namespace Component.Request.Base

    Public MustInherit Class BaseWSSPRequest
        Inherits BaseWSRequest

#Region "Protected Constant"

        Protected Const TAG_SP_INFO As String = "SPInfo"
        Protected Const TAG_SPID As String = "SPID"
        Protected Const TAG_PRACTICE_ID As String = "PracticeID"
        Protected Const TAG_PRACTICE_NAME As String = "PracticeName"

#End Region

#Region "Properties"

        Protected _strSPID As String = String.Empty
        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Protected _strSPSurname As String = String.Empty
        Public Property SPSurname() As String
            Get
                Return _strSPSurname
            End Get
            Set(ByVal value As String)
                _strSPSurname = value
            End Set
        End Property

        Protected _strSPGivenName As String = String.Empty
        Public Property SPGivenName() As String
            Get
                Return _strSPGivenName
            End Get
            Set(ByVal value As String)
                _strSPGivenName = value
            End Set
        End Property

        Protected _strPracticeID As String = String.Empty
        Public Property PracticeID() As String
            Get
                Return _strPracticeID
            End Get
            Set(ByVal value As String)
                _strPracticeID = value
            End Set
        End Property

        Protected _strPracticeName As String = String.Empty
        Public Property PracticeName() As String
            Get
                Return _strPracticeName
            End Get
            Set(ByVal value As String)
                _strPracticeName = value
            End Set
        End Property




        Private _blnSPID_Received As Boolean = False
        Public Property SPID_Included() As Boolean
            Get
                Return _blnSPID_Received
            End Get
            Set(ByVal value As Boolean)
                _blnSPID_Received = value
            End Set
        End Property

        Private _blnSPSurname_Received As Boolean = False
        Public Property SPSurname_Included() As Boolean
            Get
                Return _blnSPSurname_Received
            End Get
            Set(ByVal value As Boolean)
                _blnSPSurname_Received = value
            End Set
        End Property

        Private _blnSPGivenName_Received As Boolean = False
        Public Property SPGivenName_Included() As Boolean
            Get
                Return _blnSPGivenName_Received
            End Get
            Set(ByVal value As Boolean)
                _blnSPGivenName_Received = value
            End Set
        End Property

        Private _blnPracticeID_Received As Boolean = False
        Public Property PracticeID_included() As Boolean
            Get
                Return _blnPracticeID_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPracticeID_Received = value
            End Set
        End Property

        Private _blnPracticeName_Received As Boolean = False
        Public Property PracticeName_included() As Boolean
            Get
                Return _blnPracticeName_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPracticeName_Received = value
            End Set
        End Property



        Private _blnGenSP As Boolean = False
        Public Property SPInfo_inXML() As Boolean
            Get
                Return _blnGenSP
            End Get
            Set(ByVal value As Boolean)
                _blnGenSP = value
            End Set
        End Property

        Private _blnGenAccount As Boolean = False
        Public Property AccountInfo_inXML() As Boolean
            Get
                Return _blnPracticeID_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPracticeID_Received = value
            End Set
        End Property

        Private _blnGenClaim As Boolean = False
        Public Property ClaimInfo_inXML() As Boolean
            Get
                Return _blnPracticeName_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPracticeName_Received = value
            End Set
        End Property

        Private _blnGenIndicatorInfo As Boolean = False
        Public Property IndicatorInfo_inXML() As Boolean
            Get
                Return _blnGenIndicatorInfo
            End Get
            Set(ByVal value As Boolean)
                _blnGenIndicatorInfo = value
            End Set
        End Property

#End Region





    End Class

End Namespace


