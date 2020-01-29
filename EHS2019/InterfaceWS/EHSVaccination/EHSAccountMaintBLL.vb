Imports Microsoft.VisualBasic

Imports Common.DataAccess
Imports System.Data
Imports System.Data.SqlClient
Imports Common.Format

Namespace EHSVaccination

    Public Class EHSAccountMaintBLL

        Private Const SOURCE_SYSTEM As String = "EHS"
        Private Const TARGET_SYSTEM As String = "CMS"
        Private udtFormatter As New Formatter

        Public Sub New()
            ' Do Nothing
        End Sub

        ''' <summary>
        ''' Retrieve eHS vaccine record base on CMS web service request (xml)
        ''' </summary>
        ''' <param name="strDocType">Document Type, e.g. "HKID", "HKBC"</param>
        ''' <param name="strIdentityNum">Identity Number</param>
        ''' <param name="strEname">Patient english name, e.g. "CHAN, TAI MAN"</param>
        ''' <param name="strSex">M, F, U=Unknown</param>
        ''' <param name="dtDOB">Date of birth</param>
        ''' <param name="strExactDOB">Date of birth format, only support "D", "Y" on CMS request</param>
        ''' <param name="strVaccineCode">CMS vaccine code list, e.g. "001,002,003,004,005"</param>
        ''' <param name="dInjectionDateStart">Query vaccine From date</param>
        ''' <param name="dInjectionDateEnd">Query vaccine To date</param>
        ''' <param name="enumRequestSystem">Request System</param>
        ''' <param name="iPatientResultCode">Output value for query return code, e.g. 0,1,2</param>
        ''' <param name="iVaccineResultCode">Output value for query return code, e.g. 0,1,2</param>
        ''' <returns>eHS vaccine records</returns>
        ''' <remarks></remarks>
        Public Function GeteVaccinationbyCMSRequest(ByVal strDocType As String, ByVal strIdentityNum As String, ByVal strEname As String, ByVal strSex As String, _
                                                ByVal dtDOB As Nullable(Of DateTime), ByVal strExactDOB As String, ByVal strVaccineCode As String, _
                                                ByVal dInjectionDateStart As Nullable(Of DateTime), _
                                                ByVal dInjectionDateEnd As Nullable(Of DateTime), _
                                                ByVal enumRequestSystem As CMSRequest.RequestSystemFrom, _
                                                ByRef iPatientResultCode As Integer, ByRef iVaccineResultCode As Integer, ByVal enumCMSXMLVersion As CMSRequest.CMS_XML_Version) As DataTable
            Dim dtRes As DataTable = New DataTable
            Dim db As New Database
            Try
                strIdentityNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Dim parms() As SqlParameter = { _
                                db.MakeInParam("@Doc_Code", SqlDbType.Char, 20, strDocType), _
                                db.MakeInParam("@IdentityNum", SqlDbType.VarChar, 20, strIdentityNum), _
                                db.MakeInParam("@Eng_Name", SqlDbType.VarChar, 320, strEname), _
                                db.MakeInParam("@Sex", SqlDbType.Char, 1, strSex), _
                                db.MakeInParam("@DOB", SqlDbType.DateTime, 8, IIf(Not dtDOB.HasValue, DBNull.Value, dtDOB)), _
                                db.MakeInParam("@ExactDOB", SqlDbType.Char, 1, strExactDOB), _
                                db.MakeInParam("@VaccineCode", SqlDbType.NVarChar, 100, strVaccineCode), _
                                db.MakeInParam("@InjectionDateStart", SqlDbType.DateTime, 8, IIf(Not dInjectionDateStart.HasValue, DBNull.Value, dInjectionDateStart)), _
                                db.MakeInParam("@InjectionDateEnd", SqlDbType.DateTime, 8, IIf(Not dInjectionDateEnd.HasValue, DBNull.Value, dInjectionDateEnd)), _
                                db.MakeInParam("@Source_System", SqlDbType.VarChar, 10, SOURCE_SYSTEM), _
                                db.MakeInParam("@Target_System", SqlDbType.VarChar, 10, enumRequestSystem.ToString), _
                                db.MakeOutParam("@Out_PatientResultCode", SqlDbType.Int, 10), _
                                db.MakeOutParam("@Out_VaccineResultCode", SqlDbType.Int, 10)}
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                Select Case enumCMSXMLVersion
                    Case CMSRequest.CMS_XML_Version.ONE
                        db.RunProc("proc_eVaccination_get_byAccountDemographic", parms, dtRes)
                    Case CMSRequest.CMS_XML_Version.TWO
                        db.RunProc("proc_eVaccination_get_byAccountDemographic_v2", parms, dtRes)
                End Select

                iPatientResultCode = parms(parms.Length - 2).Value
                iVaccineResultCode = parms(parms.Length - 1).Value
            Catch ex As Exception
                dtRes = Nothing
                Throw
            End Try

            Return dtRes
        End Function


    End Class
End Namespace