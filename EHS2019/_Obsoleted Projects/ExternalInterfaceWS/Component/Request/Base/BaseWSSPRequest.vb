Imports Microsoft.VisualBasic
Imports System.Xml
Imports ExternalInterfaceWS.Component
Imports ExternalInterfaceWS.BLL
Imports ExternalInterfaceWS.Component.ErrorInfo

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
        Public Property SPID_Received() As Boolean
            Get
                Return _blnSPID_Received
            End Get
            Set(ByVal value As Boolean)
                _blnSPID_Received = value
            End Set
        End Property

        Private _blnPracticeID_Received As Boolean = False
        Public Property PracticeID_Received() As Boolean
            Get
                Return _blnPracticeID_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPracticeID_Received = value
            End Set
        End Property

        Private _blnPracticeName_Received As Boolean = False
        Public Property PracticeName_Received() As Boolean
            Get
                Return _blnPracticeName_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPracticeName_Received = value
            End Set
        End Property

#End Region

#Region "(Step 3) Service Provider Validation"

        Protected Function ValidatServiceProviderInfo(ByRef udtErrorList As ErrorInfoModelCollection)

            '--------- SPID -------------------
            If Me._strSPID = String.Empty Then
                udtErrorList.Add(ErrorCodeList.I00012) 'Incorrect Input parameter of "SP ID"
                Return False
            End If

            If Me._strSPID.Trim.Length > 8 Then
                udtErrorList.Add(ErrorCodeList.I00012) 'Incorrect Input parameter of "SP ID"
                Return False
            End If

            '--------- Practice ID -------------------
            Dim intPracticeID As Integer
            If Not Integer.TryParse(Me._strPracticeID, intPracticeID) Then
                udtErrorList.Add(ErrorCodeList.I00014) 'Incorrect Input parameter of "Practice ID"
                Return False
            Else
                If intPracticeID < 1 And intPracticeID > 99 Then
                    udtErrorList.Add(ErrorCodeList.I00014) 'Incorrect Input parameter of "Practice ID"
                    Return False
                End If
            End If

            '--------- Practice Name -------------------
            If Me._strPracticeName = String.Empty Then
                udtErrorList.Add(ErrorCodeList.I00015) 'Incorrect Input parameter of "Practice Name"
                Return False
            End If

            If Me._strPracticeName.Trim.Length > 100 Then
                udtErrorList.Add(ErrorCodeList.I00015) 'Incorrect Input parameter of "Practice Name"
                Return False
            End If

            Return True

        End Function

#End Region

#Region "(Step 2) Check whether there is missing or duplicate fields"

        Protected Function CheckSPXMLField(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean

            If _blnSPID_Received = False Or _blnPracticeID_Received = False Or _blnPracticeName_Received = False Then
                udtErrorList.Add(ErrorCodeList.I00004)  'Incorrect XML format (missing tag)
                Return False
            End If

            Return True
        End Function

#End Region

#Region "(Step 1) Read XML"

#Region "Read SP Info"
        Protected Overridable Sub ReadSPInfo(ByVal xml As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)

            Dim nlSPinfo As XmlNodeList = xml.GetElementsByTagName(TAG_SP_INFO)

            If nlSPinfo.Count = 0 Then
                udtErrorList.Add(ErrorCodeList.I00004)
                Exit Sub
            End If

            ReadSPID(nlSPinfo.Item(0), udtErrorList)

            If udtErrorList.Count = 0 Then
                ReadPracticeID(nlSPinfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If

            If udtErrorList.Count = 0 Then
                ReadPracticeName(nlSPinfo.Item(0), udtErrorList)
            Else
                Exit Sub
            End If

        End Sub

        Protected Overridable Sub ReadSPID(ByVal nodePatientDocument As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            'Incorrect XML format of SP information
            SPID = ReadString(nodePatientDocument, TAG_SPID, udtErrorList, ErrorCodeList.I00006, True, _blnSPID_Received)
        End Sub

        Protected Overridable Sub ReadPracticeID(ByVal nodePatientDocument As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            'Incorrect XML format of SP information
            PracticeID = ReadString(nodePatientDocument, TAG_PRACTICE_ID, udtErrorList, ErrorCodeList.I00006, True, _blnPracticeID_Received)
        End Sub

        Protected Overridable Sub ReadPracticeName(ByVal nodePatientDocument As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            'Incorrect XML format of SP information
            PracticeName = ReadString(nodePatientDocument, TAG_PRACTICE_NAME, udtErrorList, ErrorCodeList.I00006, True, _blnPracticeName_Received)
        End Sub

#End Region

#End Region

    End Class

End Namespace


