Imports Common.Component

Namespace ComObject

    Public Class ExtAuditLogMaster

        Private Shared _udtInstance As ExtAuditLogMaster

        Private _hashMessage As Hashtable

        Public Sub New()
            _hashMessage = New Hashtable(100)

            ' *******************************************
            ' Web Services
            ' *******************************************
            _hashMessage.Add(LogID.LOG00000, "")
            _hashMessage.Add(LogID.LOG00001, "WS call       - UploadClaim (start)")
            _hashMessage.Add(LogID.LOG00002, "WS response   - UploadClaim (end)")
            _hashMessage.Add(LogID.LOG00003, "WS call       - GetReasonForVisitList (start)")
            _hashMessage.Add(LogID.LOG00004, "WS response   - GetReasonForVisitList (end)")
            _hashMessage.Add(LogID.LOG00005, "WS call       - RCHNameQuery (start)")
            _hashMessage.Add(LogID.LOG00006, "WS response   - RCHNameQuery (end)")
            _hashMessage.Add(LogID.LOG00007, "WS call       - eHSValidatedAccountQuery (start)")
            _hashMessage.Add(LogID.LOG00008, "WS response   - eHSValidatedAccountQuery (end)")
            _hashMessage.Add(LogID.LOG00009, "WS call       - eHSAccountVoucherQuery (start)")
            _hashMessage.Add(LogID.LOG00010, "WS response   - eHSAccountVoucherQuery (end)")
            _hashMessage.Add(LogID.LOG00011, "WS call       - SPPracticeValidation (start)")
            _hashMessage.Add(LogID.LOG00012, "WS response   - SPPracticeValidation (end)")

            _hashMessage.Add(LogID.LOG00013, "WS call       - UploadClaim")
            _hashMessage.Add(LogID.LOG00014, "WS response   - UploadClaim")
            _hashMessage.Add(LogID.LOG00015, "WS call       - GetReasonForVisitList")
            _hashMessage.Add(LogID.LOG00016, "WS response   - GetReasonForVisitList")
            _hashMessage.Add(LogID.LOG00017, "WS call       - RCHNameQuery")
            _hashMessage.Add(LogID.LOG00018, "WS response   - RCHNameQuery")
            _hashMessage.Add(LogID.LOG00019, "WS call       - eHSValidatedAccountQuery")
            _hashMessage.Add(LogID.LOG00020, "WS response   - eHSValidatedAccountQuery")
            _hashMessage.Add(LogID.LOG00021, "WS call       - eHSAccountVoucherQuery")
            _hashMessage.Add(LogID.LOG00022, "WS response   - eHSAccountVoucherQuery")
            _hashMessage.Add(LogID.LOG00023, "WS call       - SPPracticeValidation")
            _hashMessage.Add(LogID.LOG00024, "WS response   - SPPracticeValidation")

            'For SPValidation
            _hashMessage.Add(LogID.LOG00025, "SPPracticeValidation - Read SP Info")
            _hashMessage.Add(LogID.LOG00026, "SPPracticeValidation - Check SP missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00027, "SPPracticeValidation - Check SP fields format")
            _hashMessage.Add(LogID.LOG00028, "SPPracticeValidation - Check SP Name field format")
            _hashMessage.Add(LogID.LOG00029, "SPPracticeValidation - Validate Service Provider Information")

            'For eHSValidatedAccountQuery
            _hashMessage.Add(LogID.LOG00030, "eHSValidatedAccountQuery - Read SP Info")
            _hashMessage.Add(LogID.LOG00031, "eHSValidatedAccountQuery - Read Account Info")
            _hashMessage.Add(LogID.LOG00032, "eHSValidatedAccountQuery - Check SP missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00033, "eHSValidatedAccountQuery - Check Account missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00034, "eHSValidatedAccountQuery - Check SP fields format")
            _hashMessage.Add(LogID.LOG00035, "eHSValidatedAccountQuery - Check Account fields format")
            _hashMessage.Add(LogID.LOG00036, "eHSValidatedAccountQuery - Validate Service Provider Information")
            _hashMessage.Add(LogID.LOG00037, "eHSValidatedAccountQuery - Validate Account Information")

            'For eHSAccountVoucherQuery
            _hashMessage.Add(LogID.LOG00038, "eHSAccountVoucherQuery - Read SP Info")
            _hashMessage.Add(LogID.LOG00039, "eHSAccountVoucherQuery - Read Account Info")
            _hashMessage.Add(LogID.LOG00040, "eHSAccountVoucherQuery - Check SP missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00041, "eHSAccountVoucherQuery - Check Account missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00042, "eHSAccountVoucherQuery - Check SP fields format")
            _hashMessage.Add(LogID.LOG00043, "eHSAccountVoucherQuery - Check Account fields format")
            _hashMessage.Add(LogID.LOG00044, "eHSAccountVoucherQuery - Validate Service Provider Information")
            _hashMessage.Add(LogID.LOG00045, "eHSAccountVoucherQuery - Validate Account Information")
            _hashMessage.Add(LogID.LOG00046, "eHSAccountVoucherQuery - Check Voucher Remained")

            'For RCHNameQueryRequest
            _hashMessage.Add(LogID.LOG00047, "RCHNameQueryRequest - Read SP Info")
            _hashMessage.Add(LogID.LOG00048, "RCHNameQueryRequest - Read RCH Code")
            _hashMessage.Add(LogID.LOG00049, "RCHNameQueryRequest - Check SP missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00050, "RCHNameQueryRequest - Check SP fields format")
            _hashMessage.Add(LogID.LOG00051, "RCHNameQueryRequest - Validate Service Provider Information")
            _hashMessage.Add(LogID.LOG00052, "RCHNameQueryRequest - Get RVP Home List Active By Code")

            'For GetReasonForVisitList
            _hashMessage.Add(LogID.LOG00053, "GetReasonForVisitList - Retrieve List of Reason For Visit")

            'For UploadClaim
            _hashMessage.Add(LogID.LOG00054, "UploadClaim - Read SP Info")
            _hashMessage.Add(LogID.LOG00055, "UploadClaim - Read Account Info")
            _hashMessage.Add(LogID.LOG00056, "UploadClaim - Read Claim Info")
            _hashMessage.Add(LogID.LOG00057, "UploadClaim - Check SP missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00058, "UploadClaim - Check Account missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00059, "UploadClaim - Check Claim missing / duplicate fields")
            _hashMessage.Add(LogID.LOG00060, "UploadClaim - Check SP fields format")
            _hashMessage.Add(LogID.LOG00061, "UploadClaim - Check Account fields format")
            _hashMessage.Add(LogID.LOG00062, "UploadClaim - Check Claim fields format")
            _hashMessage.Add(LogID.LOG00063, "UploadClaim - End Validate Service Provider Information")
            _hashMessage.Add(LogID.LOG00064, "UploadClaim - Validate Account Information")
            _hashMessage.Add(LogID.LOG00065, "UploadClaim - Check Documnet Limit on Age")
            _hashMessage.Add(LogID.LOG00066, "UploadClaim - Validate Service Provider Information")
            _hashMessage.Add(LogID.LOG00067, "UploadClaim - Start Check Personal Information")
            _hashMessage.Add(LogID.LOG00068, "UploadClaim - Finish Check Personal Information")
            _hashMessage.Add(LogID.LOG00069, "UploadClaim - Finish Validate Account")
            _hashMessage.Add(LogID.LOG00070, "UploadClaim - Start Load Validate Account")
            _hashMessage.Add(LogID.LOG00071, "UploadClaim - Finish Load Validate Account")
            _hashMessage.Add(LogID.LOG00072, "UploadClaim - Start Fill EHS Transaction Model")
            _hashMessage.Add(LogID.LOG00073, "UploadClaim - Start Fill Warning Indicator List")
            _hashMessage.Add(LogID.LOG00074, "UploadClaim - End Fill Warning Indicator List")
            _hashMessage.Add(LogID.LOG00075, "UploadClaim - Start Upload Claim Main")
            _hashMessage.Add(LogID.LOG00076, "UploadClaim - End Upload Claim Main")


            'General Purpose
            _hashMessage.Add(LogID.LOG00080, "Unauthorized Party")

            ' UploadClaim BLL
            _hashMessage.Add(LogID.LOG00100, "UploadClaimBLL Finish Obtain Max Transactions Allowed")
            _hashMessage.Add(LogID.LOG00101, "UploadClaimBLL Finish Check Exceed Max Transactions")
            _hashMessage.Add(LogID.LOG00102, "UploadClaimBLL Begin Validate Transaction")
            _hashMessage.Add(LogID.LOG00103, "UploadClaimBLL Validate Reason For Visit")
            _hashMessage.Add(LogID.LOG00104, "UploadClaimBLL Validate RCH Code")
            _hashMessage.Add(LogID.LOG00105, "UploadClaimBLL Validate Scheme")
            _hashMessage.Add(LogID.LOG00106, "UploadClaimBLL Validate Subsidy")
            _hashMessage.Add(LogID.LOG00107, "UploadClaimBLL End Validate Transaction")
            _hashMessage.Add(LogID.LOG00108, "UploadClaimBLL Start Get Service Provider")
            _hashMessage.Add(LogID.LOG00109, "UploadClaimBLL End Get Service Provider")
            _hashMessage.Add(LogID.LOG00110, "UploadClaimBLL Start Get Validated Account")
            _hashMessage.Add(LogID.LOG00111, "UploadClaimBLL End Get Validated Account")
            _hashMessage.Add(LogID.LOG00112, "UploadClaimBLL Start Validate Rule Checking")
            _hashMessage.Add(LogID.LOG00113, "UploadClaimBLL Get HA Vaccine Record")
            _hashMessage.Add(LogID.LOG00114, "UploadClaimBLL Validation Rule Check")
            _hashMessage.Add(LogID.LOG00115, "UploadClaimBLL Finish Validation Rule Check")
            _hashMessage.Add(LogID.LOG00116, "UploadClaimBLL Get Timestamp")
            _hashMessage.Add(LogID.LOG00117, "UploadClaimBLL Start Insert Transaction")
            _hashMessage.Add(LogID.LOG00118, "UploadClaimBLL Start Create Temp Account")
            _hashMessage.Add(LogID.LOG00119, "UploadClaimBLL End Create Temp Account")
            _hashMessage.Add(LogID.LOG00120, "UploadClaimBLL Start Insert Transaction to DB")
            _hashMessage.Add(LogID.LOG00121, "UploadClaimBLL Finish Insert Transaction to DB")
            _hashMessage.Add(LogID.LOG00122, "UploadClaimBLL Check if Scheme enrolled by the Practice")
            _hashMessage.Add(LogID.LOG00123, "UploadClaimBLL Check if SP joined the Professional")
            _hashMessage.Add(LogID.LOG00124, "UploadClaimBLL Skip CMS Call - HA Vaccine Result already obtained")
            _hashMessage.Add(LogID.LOG00125, "UploadClaimBLL Skip CMS Call - Document is not accepted by HA CMS")
            _hashMessage.Add(LogID.LOG00126, "UploadClaimBLL Skip CMS Call - Not vaccine claiim")

            ' ValidateAccountBLL
            _hashMessage.Add(LogID.LOG00140, "ValidateAccountBLL Start ValidatedAccountQuery")
            _hashMessage.Add(LogID.LOG00141, "ValidateAccountBLL End ValidatedAccountQuery")
            _hashMessage.Add(LogID.LOG00142, "ValidateAccountBLL Finish Search Temp Account")
            _hashMessage.Add(LogID.LOG00143, "ValidateAccountBLL Finish ValidatedAccountQuery")

        End Sub

        Public Shared Function Instance() As ExtAuditLogMaster
            If _udtInstance Is Nothing Then
                _udtInstance = New ExtAuditLogMaster
            End If

            Return _udtInstance
        End Function

        Public Shared Function Messages(ByVal strLogID As String) As String
            Return Instance().GetMessages(strLogID)
        End Function

        Protected Function GetMessages(ByVal strLogID As String)
            Return _hashMessage(strLogID)
        End Function

    End Class


End Namespace