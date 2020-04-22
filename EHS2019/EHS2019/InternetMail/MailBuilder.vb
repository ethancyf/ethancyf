Public Class MailBuilder

    Private Shared _mailBuilder As MailBuilder = Nothing

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As MailBuilder
        If _mailBuilder Is Nothing Then _mailBuilder = New MailBuilder()
        Return _mailBuilder
    End Function

#End Region

    Public Function ConstructEmail(ByVal udtInternetMail As Common.Component.InternetMail.InternetMailModel, ByVal udtMailTemplate As Common.Component.InternetMail.MailTemplateModel) As Email

        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim email As New Email()

        email.EmailAddress = udtInternetMail.MailAddress

        ' Both = Chi + Eng Subject, Chi + Eng Content
        ' ChiHeader = Chi Subject,  Chi + Eng Content
        ' EngHeader = Eng Subject,  Chi + Eng Content
        If udtInternetMail.MailLanguage = Common.Component.InternetMailLanguage.Both Then

            email.Subject = udtMailTemplate.MailSubjectChi + " " + udtMailTemplate.MailSubjectEng
            email.Content = udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyChi, udtParamFunction.GetParameterCollection(udtInternetMail.ChiParameter))
            email.Content += udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyEng, udtParamFunction.GetParameterCollection(udtInternetMail.EngParameter))

        ElseIf udtInternetMail.MailLanguage = Common.Component.InternetMailLanguage.ChiHeader Then

            email.Subject = udtMailTemplate.MailSubjectChi
            email.Content = udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyChi, udtParamFunction.GetParameterCollection(udtInternetMail.ChiParameter))
            email.Content += udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyEng, udtParamFunction.GetParameterCollection(udtInternetMail.EngParameter))

        ElseIf udtInternetMail.MailLanguage = Common.Component.InternetMailLanguage.EngHeader Then

            email.Subject = udtMailTemplate.MailSubjectEng
            email.Content = udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyChi, udtParamFunction.GetParameterCollection(udtInternetMail.ChiParameter))
            email.Content += udtParamFunction.GetParsedStringByparameter(udtMailTemplate.MailBodyEng, udtParamFunction.GetParameterCollection(udtInternetMail.EngParameter))

        End If

        '#If DEBUG Then

        '        If Not Configuration.ConfigurationManager.AppSettings("ApplyFilter") Is Nothing Then
        '            Dim strApplyFilter As String = Configuration.ConfigurationManager.AppSettings("ApplyFilter").Trim()
        '            If strApplyFilter.ToUpper() = "On".ToUpper() Then
        '                Dim blnValidEmail As Boolean = False

        '                If email.EmailAddress.Trim().ToLower.IndexOf("@ha.org.hk") >= 0 Then
        '                    blnValidEmail = True
        '                Else
        '                    If Not Configuration.ConfigurationManager.AppSettings("ValidEmailList") Is Nothing Then
        '                        Dim strEmailList As String = Configuration.ConfigurationManager.AppSettings("ValidEmailList").ToString()
        '                        Dim arrStrEmail() As String = strEmailList.Split(",")
        '                        For Each strEmail As String In arrStrEmail
        '                            If strEmail.Trim().ToLower() = email.EmailAddress.Trim().ToLower() Then
        '                                blnValidEmail = True
        '                            End If
        '                        Next
        '                    End If
        '                End If

        '                If Not blnValidEmail Then
        '                    email.EmailAddress = "lph377@ha.org.hk"
        '                    email.Content = udtInternetMail.MailAddress + " " + email.Content
        '                End If
        '            End If
        '        End If
        '#End If

        Return email
    End Function

    Partial Class Email

        Private strChanType As String = WSSetting.WSEmail.ChannelType
        Private strCharSet As String = WSSetting.WSEmail.CharSet
        Private strContentType As String = WSSetting.WSEmail.ContentType.HTML
        Private strSubject As String = ""
        Private strContent As String = ""

        ' Single Recipient / Enhance Later?
        Private strEmailAddress As String = ""

        Friend Sub New()

        End Sub

        Public ReadOnly Property ChannelType() As String
            Get
                Return Me.strChanType
            End Get
        End Property

        Public ReadOnly Property CharacterSet() As String
            Get
                Return Me.strCharSet
            End Get
        End Property

        Public ReadOnly Property ContentType() As String
            Get
                Return Me.strContentType
            End Get
        End Property

        Public Property Subject() As String
            Get
                Return Me.strSubject
            End Get
            Set(ByVal value As String)
                Me.strSubject = value
            End Set
        End Property

        Public Property Content() As String
            Get
                Return Me.strContent
            End Get
            Set(ByVal value As String)
                Me.strContent = value
            End Set
        End Property

        Public Property EmailAddress() As String
            Get
                Return Me.strEmailAddress
            End Get
            Set(ByVal value As String)
                Me.strEmailAddress = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Dim strReturn As String = ""
            strReturn += "<Email>" + Environment.NewLine
            strReturn += "<ChannelType>" + Me.strChanType + "</ChannelType>" + Environment.NewLine
            strReturn += "<CharacterSet>" + Me.strCharSet + "</CharacterSet>" + Environment.NewLine
            strReturn += "<ContentType>" + Me.strContentType + "</ContentType>" + Environment.NewLine
            strReturn += "<Email>" + Me.strEmailAddress + "</Email>" + Environment.NewLine
            strReturn += "<Subject>" + Me.strSubject + "</Subject>" + Environment.NewLine
            strReturn += "<Content>" + Me.strContent + "</Content>" + Environment.NewLine
            strReturn += "</Email>" + Environment.NewLine
            Return strReturn
        End Function
    End Class
End Class
