Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Validation
Imports ExternalInterfaceWS.Component.Request.Base
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject

Namespace Component.Request

    Public Class eHSAccountSubsidyQueryRequest
        Inherits BaseWSAccountRequest

#Region "Private Constant"

        Private Const TAG_CLAIM_INFO As String = "ClaimInfo"
        Private Const TAG_CLAIM_DETAIL As String = "ClaimDetail"
        Private Const TAG_SERVICE_DATE As String = "ServiceDate"
        Private Const TAG_SCHEME_CODE As String = "SchemeCode"

        Private Const TAG_VOUCHER_INFO As String = "VoucherInfo"
        Private Const TAG_VOUCHER_CLAIMED As String = "VoucherClaimed"
        Private Const TAG_REASON_FOR_VISIT As String = "ReasonForVisit"
        Private Const TAG_PROF_CODE As String = "ProfCode"
        Private Const TAG_L1_CODE As String = "L1Code"
        Private Const TAG_L1_DESC_ENG As String = "L1DescEng"
        Private Const TAG_L2_CODE As String = "L2Code"
        Private Const TAG_L2_DESC_ENG As String = "L2DescEng"

        Private Const TAG_VACCINE_INFO As String = "VaccineInfo"
        Private Const TAG_SUBSIDY_CODE As String = "SubsidyCode"
        Private Const TAG_DOSE_SEQ As String = "DoseSeq"
        Private Const TAG_RCH_CODE As String = "RCHCode"

        Private Const TAG_INDICATOR As String = "Indicator"
        Private Const TAG_WARN_CODE As String = "WarnCode"
        Private Const TAG_WARN_INDICATOR As String = "WarnIndicator"

#End Region

#Region "Constructor"

        Public Sub New(ByVal xmlRequest As String, ByRef udtAuditLog As ExtAuditLogEntry)
            Dim xml As New XmlDocument()
            'For Logging
            Me.ExtAuditLogEntry = udtAuditLog

            Try
                '---------------------------------
                '(Step 1) Read XML
                '---------------------------------
                xml.LoadXml(xmlRequest)
            Catch ex As Exception
                Me._bIsValid = False
                Me.Errors.Add(ErrorCodeList.I00003)
                Exit Sub
            End Try

            Try
                'Message ID
                ReadMessageIDandValidate(xml, Me.Errors)
                'Assign Message ID to Audit Log
                If Me.Errors.Count = 0 Then
                    udtAuditLog.MessageID = _strMessageID
                End If

                'SP Info
                ReadSPInfo(xml, Me.Errors)
                'Assign SP ID to Audit Log
                If Not SPID Is Nothing AndAlso Not SPID.Trim = String.Empty Then
                    udtAuditLog.UserID = SPID
                End If
                WriteLogWithErrorList(LogID.LOG00038)

                'Account Info
                ReadAccountInfo(xml, Me.Errors)
                WriteLogWithErrorList(LogID.LOG00039)
                '---------------------------------
                '(Step 2) Check whether there is missing or duplicate fields 
                '---------------------------------
                If Me.Errors.Count = 0 Then
                    Me._bIsValid = CheckSPXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00040)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = CheckEHSAccountXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00041)
                End If
                '---------------------------------
                '(Step 3) Check eHS account fields format & SP info
                '---------------------------------
                If Me._bIsValid Then
                    Me._bIsValid = ValidatServiceProviderInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00042)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = ValidateEHSAccountInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00043)
                End If

            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.eHSAccountSubsidyQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Me.Errors.Add(ErrorCodeList.I99999)
                Me._bIsValid = False
            End Try

        End Sub

#End Region

    End Class

End Namespace


