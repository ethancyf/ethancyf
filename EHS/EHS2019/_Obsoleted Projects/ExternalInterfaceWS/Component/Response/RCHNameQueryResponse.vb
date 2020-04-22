Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports Common.Component.RVPHomeList
Imports Common.ComFunction.Generator
Imports ExternalInterfaceWS.XMLGenerator
Imports ExternalInterfaceWS.Component.Request
Imports ExternalInterfaceWS.BLL
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject
Imports Common.Component


Namespace Component.Response

    Public Class RCHNameQueryResponse

#Region "Private Constant"

        Private Const TAG_EHS_RESPONSE As String = "Response"
        Private Const TAG_OUTPUT As String = "Output"
        Private Const TAG_ERROR_INFO As String = "ErrorInfo"
        Private Const TAG_ERROR_CODE As String = "ErrorCode"
        Private Const TAG_ERROR_MESSAGE As String = "ErrorMessage"
        Private Const TAG_HOMENAME_ENG As String = "HomeNameEng"
        Private Const TAG_HOMENAME_CHI As String = "HomeNameChi"
        Private Const TAG_ADDRESS_ENG As String = "AddressEng"
        Private Const TAG_ADDRESS_CHI As String = "AddressChi"

#End Region

#Region "Properties"

        Private _udtReturnErrorCodes As ErrorInfoModelCollection = New ErrorInfoModelCollection
        Public ReadOnly Property ReturnErrorCodes() As ErrorInfoModelCollection
            Get
                Return _udtReturnErrorCodes
            End Get
        End Property

        Private _dtResult As DataTable = Nothing
        Public ReadOnly Property Result() As DataTable
            Get
                Return _dtResult
            End Get
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            'Do Nothing
        End Sub

#End Region

        Public Function ProcessRequest(ByVal oRequest As RCHNameQueryRequest, ByVal strSystemName As String, ByRef udtAuditLog As ExtAuditLogEntry) As Boolean
            Try
                '------------------------------------------------------
                ' Check Request valid format (Base on XML format, field validation)
                '------------------------------------------------------
                If Not oRequest.IsValid Then
                    _udtReturnErrorCodes = oRequest.Errors
                    Return False
                End If

                '------------------------------------------------------
                ' Check SP Information
                '------------------------------------------------------
                Dim blnIsValid As Boolean = True
                blnIsValid = SPPracticeBLL.chkServiceProviderInfo(oRequest.SPID, _
                                                        oRequest.PracticeID, _
                                                        oRequest.PracticeName, _
                                                        _udtReturnErrorCodes, _
                                                        String.Empty, _
                                                        strSystemName, _
                                                        False)

                If Not blnIsValid Then
                    Return False
                End If

                udtAuditLog.AddDescripton("IsValid", blnIsValid.ToString())
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00051, _udtReturnErrorCodes)

                '------------------------------------------------------
                ' Retrieve RCH details
                '------------------------------------------------------
                Dim udtRVPHomeListBLL As RVPHomeListBLL = New RVPHomeListBLL()

                _dtResult = udtRVPHomeListBLL.getRVPHomeListActiveByCode(oRequest.RCHCode)

                udtAuditLog.AddDescripton("NoOfRecord", _dtResult.Rows.Count.ToString())
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00052, _udtReturnErrorCodes)

                If _dtResult.Rows.Count = 0 Then
                    _udtReturnErrorCodes.Add(ErrorCodeList.I00000)
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.RCHNameQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                _udtReturnErrorCodes.Add(ErrorCodeList.I99999)
                Return False
            End Try


        End Function

#Region "Generate XML Result"

        Public Function GenXMLResult() As XmlDocument
            Dim xml As New XmlDocument()

            Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement)

            Dim nodeResponse As XmlElement
            nodeResponse = xml.CreateElement(TAG_EHS_RESPONSE)
            xml.AppendChild(nodeResponse)

            Dim nodeResult As XmlElement
            nodeResult = xml.CreateElement(TAG_OUTPUT)
            nodeResponse.AppendChild(nodeResult)

            If _udtReturnErrorCodes.Count = 0 Then
                GenRCHRecord(xml, nodeResult, _dtResult.Rows(0))

                'Dim objXmlGenerator As AbstractXmlGenerator = Nothing
                'Dim objXmlDocument As XmlDocument = Nothing
                'Dim strXmlSchemaFilePath As String = ""

                'strXmlSchemaFilePath = System.Configuration.ConfigurationManager.AppSettings("RCHNameQueryOutput_XmlSchema_FilePath").ToString()
                'objXmlGenerator = New RCHNameQueryXmlGenerator(_dtResult, strXmlSchemaFilePath)

                'objXmlDocument = objXmlGenerator.Convert()

                ''Avoid XML sharthand closing tag
                'For Each el As XmlElement In objXmlDocument.SelectNodes("descendant::*[not(node())]")
                '    el.IsEmpty = False
                'Next

                'xml = objXmlDocument
            Else
                'Error exists
                GenReturnCode(xml, nodeResult)
            End If

            Return xml
        End Function

        Private Sub GenReturnCode(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim nodeInfo As XmlElement
            nodeInfo = xml.CreateElement(TAG_ERROR_INFO)
            nodeResult.AppendChild(nodeInfo)

            Dim nodeTmp As XmlElement

            '----------------------------------------------
            'Retrieve External Code and Message
            '----------------------------------------------
            Dim strErrorCode As String = String.Empty
            Dim strErrorMessage As String = String.Empty
            _udtReturnErrorCodes.RetrieveExternalCodeAndMessageToOutside(strErrorCode, strErrorMessage)
            '----------------------------------------------

            'Error Code
            nodeTmp = xml.CreateElement(TAG_ERROR_CODE)
            'nodeTmp.InnerText = udtErrorInfo.ErrorCode
            nodeTmp.InnerText = strErrorCode
            nodeInfo.AppendChild(nodeTmp)

            'Error Message
            nodeTmp = xml.CreateElement(TAG_ERROR_MESSAGE)
            'nodeTmp.InnerText = udtErrorInfo.ErrorMessage
            nodeTmp.InnerText = strErrorMessage
            nodeInfo.AppendChild(nodeTmp)


        End Sub

        Private Sub GenRCHRecord(ByVal xml As XmlDocument, ByVal nodeReturnData As XmlElement, ByVal drRCHRecord As DataRow)

            Dim nodeTmp As XmlElement

            'Homename_english
            nodeTmp = xml.CreateElement(TAG_HOMENAME_ENG)
            nodeTmp.InnerText = drRCHRecord("Homename_Eng")
            nodeReturnData.AppendChild(nodeTmp)
            'Homename_chinese
            nodeTmp = xml.CreateElement(TAG_HOMENAME_CHI)
            nodeTmp.InnerText = drRCHRecord("Homename_Chi")
            nodeReturnData.AppendChild(nodeTmp)
            'Address_chinese
            nodeTmp = xml.CreateElement(TAG_ADDRESS_ENG)
            nodeTmp.InnerText = drRCHRecord("Address_Eng")
            nodeReturnData.AppendChild(nodeTmp)
            'Address_chinese
            nodeTmp = xml.CreateElement(TAG_ADDRESS_CHI)
            nodeTmp.InnerText = drRCHRecord("Address_Chi")
            nodeReturnData.AppendChild(nodeTmp)
        End Sub

#End Region

    End Class

End Namespace

