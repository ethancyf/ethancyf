Namespace Component.EHSAccount
    Partial Public Class EHSAccountModel
        <Serializable()> Public Class EHSPersonalInformationDemographicModel

#Region "Memeber"

            Private _dtmDOB As Date
            Private _strExact_DOB As String
            Private _strSex As String
            Private _strDoc_Code As String

            ' -----------------
            'Encrypt_Field
            Private _strIdentityNum As String
            Private _strEName As String

#End Region

#Region "Property"

            Public Property DOB() As Date
                Get
                    Return Me._dtmDOB
                End Get
                Set(ByVal value As Date)
                    Me._dtmDOB = value
                End Set
            End Property

            Public Property ExactDOB() As String
                Get
                    Return Me._strExact_DOB
                End Get
                Set(ByVal value As String)
                    Me._strExact_DOB = value
                End Set

            End Property

            Public Property Gender() As String
                Get
                    Return Me._strSex
                End Get
                Set(ByVal value As String)
                    Me._strSex = value
                End Set

            End Property

            Public Property IdentityNum() As String
                Get
                    Return Me._strIdentityNum
                End Get
                Set(ByVal value As String)
                    Me._strIdentityNum = value
                End Set
            End Property

            Public ReadOnly Property EName() As String
                Get
                    Return _strEName
                End Get
            End Property

            Public Property DocCode() As String
                Get
                    Return Me._strDoc_Code
                End Get
                Set(ByVal value As String)
                    Me._strDoc_Code = value
                End Set
            End Property

#End Region


#Region "Constructor"

            Sub New()
            End Sub

            ''' <summary>
            ''' Private Constructor For Clone
            ''' </summary>
            ''' <param name="udtEHSPersonalInformationDemographicModel"></param>
            ''' <remarks></remarks>
            ''' 
            Private Sub New(ByVal udtEHSPersonalInformationDemographicModel As EHSPersonalInformationDemographicModel)


                Me._dtmDOB = udtEHSPersonalInformationDemographicModel._dtmDOB
                Me._strExact_DOB = udtEHSPersonalInformationDemographicModel._strExact_DOB
                Me._strSex = udtEHSPersonalInformationDemographicModel._strSex
                Me._strDoc_Code = udtEHSPersonalInformationDemographicModel._strDoc_Code

                ' -----------------
                'Encrypt_Field
                Me._strIdentityNum = udtEHSPersonalInformationDemographicModel._strIdentityNum
                Me._strEName = udtEHSPersonalInformationDemographicModel._strEName
            End Sub

            ''' <summary>
            ''' Constructor of PersonalInformationDemographic
            ''' </summary>
            ''' <param name="dtmDOB"></param>
            ''' <param name="strExactDOB"></param>
            ''' <param name="strGender"></param>
            ''' <param name="strIdentityNum"></param>
            ''' <param name="strDocCode"></param>
            ''' <remarks></remarks>
            Sub New(ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strGender As String, _
                ByVal strIdentityNum As String, ByVal strDocCode As String, ByVal strEName As String)

                Me._dtmDOB = dtmDOB
                Me._strExact_DOB = strExactDOB
                Me._strSex = strGender
                Me._strDoc_Code = strDocCode

                ' -----------------
                'Encrypt_Field
                Me._strIdentityNum = strIdentityNum
                Me._strEName = strEName

            End Sub

#End Region

#Region "Supporting Function"

            Function Clone() As EHSPersonalInformationDemographicModel
                Return New EHSPersonalInformationDemographicModel(Me)
            End Function

#End Region

#Region "Public Function"

            Public Overloads Function Equals(ByVal udtPersonalInfo As EHSPersonalInformationModel) As Boolean
                If IsNothing(udtPersonalInfo.ENameSurName) Then Return False
                If IsNothing(udtPersonalInfo.ENameFirstName) Then Return False

                If _strEName.Trim <> udtPersonalInfo.EName.Trim Then Return False
                If _strSex.Trim <> udtPersonalInfo.Gender.Trim Then Return False
                If _dtmDOB <> udtPersonalInfo.DOB Then Return False
                If _strExact_DOB.Trim <> udtPersonalInfo.ExactDOB.Trim Then Return False

                Return True

            End Function

#End Region

        End Class
    End Class
End Namespace