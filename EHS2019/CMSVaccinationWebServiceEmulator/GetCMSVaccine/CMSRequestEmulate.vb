Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml

Namespace EHSVaccination

    Public Class CMSRequestEmulate
        Inherits InterfaceWS.EHSVaccination.CMSRequest

#Region "Constants"
        Private Const TAG_MESSAGE_ID As String = "message_id"
#End Region

#Region "Constructor"
        Sub New(ByVal xmlRequest As String, ByVal enumCMSXMLVersion As CMS_XML_Version)
            Dim xml As New XmlDocument()

            Try
                xml.LoadXml(xmlRequest)

                ReadMessageID(xml)
                ReadHealthCheck(xml)
                ReadRequestSystem(xml)
                MyBase.SetXMLWSVersion(enumCMSXMLVersion)

                If Not HealthCheck Then

                    'CheckRequestSystem(strRequestSystem)

                    'intPatientCount = CheckEnquiryPatientLimit(xml)

                    'blnInOpenHour = CheckBatchEnquiryOpeningHour()

                    ''Read patient in XML when there is single patient or in opening period(hour)
                    'If AllowToReadPatient(intPatientCount, blnInOpenHour) Then
                    'Select Case RequestSystem
                    '    Case RequestSystemFrom.CMS
                    '        'Set XML WS Version
                    '        _enumCMSXMLVersion = GetCMSWSVersion()

                    '    Case RequestSystemFrom.CIMS
                    '        'Set XML WS Version
                    '        _enumCMSXMLVersion = CMS_XML_Version.TWO

                    'End Select

                    'Read patient from XML
                    ReadPatientList(xml)
                    'Read vaccine from XML
                    ReadVaccine(xml)
                    'Read injection start date and end date from XML
                    ReadOthers(xml)

                    'End If

                End If

                MyBase.SetIsValid(True)
            Catch ex As Exception
                MyBase.SetException(ex)
                MyBase.SetIsValid(False)
            End Try
        End Sub
#End Region

#Region "Read XML"
        Private Sub ReadMessageID(ByVal xml As XmlDocument)
            MyBase.MessageID() = ReadRootString(xml, TAG_MESSAGE_ID)
        End Sub

#End Region

    End Class

End Namespace
