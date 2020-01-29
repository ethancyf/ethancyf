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

    Public Class UploadClaimRequest
        Inherits BaseWSClaimRequest

#Region "Private Constant"

        'Private Const TAG_CLAIM_INFO As String = "ClaimInfo"
        'Private Const TAG_CLAIM_DETAIL As String = "ClaimDetail"
        'Private Const TAG_SERVICE_DATE As String = "ServiceDate"
        'Private Const TAG_SCHEME_CODE As String = "SchemeCode"

        'Private Const TAG_VOUCHER_INFO As String = "VoucherInfo"
        'Private Const TAG_VOUCHER_CLAIMED As String = "VoucherClaimed"
        'Private Const TAG_REASON_FOR_VISIT As String = "ReasonForVisit"
        'Private Const TAG_PROF_CODE As String = "ProfCode"
        'Private Const TAG_L1_CODE As String = "L1Code"
        'Private Const TAG_L1_DESC_ENG As String = "L1DescEng"
        'Private Const TAG_L2_CODE As String = "L2Code"
        'Private Const TAG_L2_DESC_ENG As String = "L2DescEng"

        'Private Const TAG_VACCINE_INFO As String = "VaccineInfo"
        'Private Const TAG_SUBSIDY_CODE As String = "SubsidyCode"
        'Private Const TAG_DOSE_SEQ As String = "DoseSeq"
        'Private Const TAG_RCH_CODE As String = "RCHCode"

        'Private Const TAG_INDICATOR As String = "Indicator"
        'Private Const TAG_WARN_CODE As String = "WarnCode"
        'Private Const TAG_WARN_INDICATOR As String = "WarnIndicator"

        'Private Const ERR_TAG_NOT_FOUND As String = "{0} tag not found"
        'Private Const ERR_TAG_DUPLICATE As String = "Duplicate {0} tag found"
        'Private Const ERR_TAG_INVALID_VALUE As String = "Invalid {0} tag value"
        'Private Const ERR_ITEM_NOT_MATCH_COUNT As String = "Number of {0} is not match {1}"

#End Region

#Region "Properties"

        ''Claim
        'Private _udtWSClaimDetaillList As WSClaimDetailModelCollection
        'Public Property WSClaimDetailList() As WSClaimDetailModelCollection
        '    Get
        '        Return Me._udtWSClaimDetaillList
        '    End Get
        '    Set(ByVal value As WSClaimDetailModelCollection)
        '        Me._udtWSClaimDetaillList = value
        '    End Set
        'End Property

#End Region

#Region "Constructor"

        Public Sub New()
            Me._bIsValid = True
        End Sub

        Public Sub New(ByVal xmlRequest As String, ByRef udtAuditLog As ExtAuditLogEntry)
            Dim xml As New XmlDocument()
            'For Logging
            Me.ExtAuditLogEntry = udtAuditLog

            Try
                '---------------------------------
                '(Step 1) Read XML
                '---------------------------------
                xml.LoadXml(xmlRequest)

                ReadSPInfo(xml, Me.Errors)
                WriteLogWithErrorList(LogID.LOG00054)

                ReadAccountInfo(xml, Me.Errors)
                WriteLogWithErrorList(LogID.LOG00055)

                ReadClaimInfo(xml, Me.Errors)
                WriteLogWithErrorList(LogID.LOG00056)

                '---------------------------------
                '(Step 2) Check whether there is missing or duplicate fields 
                '---------------------------------
                If Me.Errors.Count = 0 Then
                    Me._bIsValid = CheckSPXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00057)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = CheckEHSAccountXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00058)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = CheckClaimXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00059)
                End If

                '---------------------------------
                '(Step 3) Check SP / eHS account / Claim fields format 
                '---------------------------------
                If Me._bIsValid Then
                    Me._bIsValid = ValidatServiceProviderInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00060)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = ValidateEHSAccountInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00061)
                End If


                If Me._bIsValid Then
                    Me._bIsValid = ValidateClaimInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00062)
                End If


                '---------------------------------
                '(Step 4) Addtional Checking on Document Limit
                'e.g. Certificate of Exemption would be accepted for person at the age of 11 or above. / Document Limit
                '---------------------------------
                If Me._bIsValid Then
                    Me._bIsValid = Me.CheckDocumentLimitByAge(Me.Errors, WSClaimDetailList, Me.ServiceDate)
                    WriteLogWithErrorList(LogID.LOG00065)
                End If

            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.UploadClaim).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Me.Errors.Add(ErrorCodeList.I99999)
                Me._bIsValid = False
            End Try

        End Sub

#End Region

    End Class

End Namespace


