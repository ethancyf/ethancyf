Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports ExternalInterfaceWS.Component
Imports ExternalInterfaceWS.Component.ErrorInfo

Namespace BLL

    Public Class SPPracticeBLL

#Region "Checking Function"

        ''' <summary>
        ''' Check SP information
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function chkServiceProviderInfo(ByVal strSPID As String, ByVal strPracticeID As String, ByVal strPracticeName As String, _
                    ByRef udtErrorList As ErrorInfoModelCollection, ByVal strSPName As String, ByVal strSystemName As String, Optional ByVal blnCheckSPName As Boolean = True) As Boolean

            Dim udtDB As Database = New Database()
            Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL()
            Dim udtServiceProvider As ServiceProviderModel = Nothing
            Dim udtPracticeModel As PracticeModel = Nothing
            Dim blnIsValid As Boolean = True

            'SP (Active)
            Try
                udtServiceProvider = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strSPID)
            Catch ex As Exception
                udtErrorList.Add(ErrorInfo.ErrorCodeList.I00046)   'Invalid SP/Practice information or SP/Practice Status
                Return False
            End Try

            If Not IsNothing(udtServiceProvider) AndAlso udtServiceProviderBLL.CheckServiceProviderPracticeEnroledExternalUpload(udtDB, strSPID, strPracticeID, strSystemName) Then
                If Not udtServiceProvider.RecordStatus = ServiceProviderStatus.Active Then
                    udtErrorList.Add(ErrorInfo.ErrorCodeList.I00046)   'Invalid SP/Practice information or SP/Practice Status
                    blnIsValid = False
                End If

                If blnCheckSPName And Not udtServiceProvider.EnglishName.Trim = strSPName.Trim Then
                    udtErrorList.Add(ErrorInfo.ErrorCodeList.I00046)   'Invalid SP/Practice information or SP/Practice Status
                    blnIsValid = False
                End If
            Else
                udtErrorList.Add(ErrorInfo.ErrorCodeList.I00046)   'Invalid SP/Practice information or SP/Practice Status
                blnIsValid = False
            End If

            'Practice (Active)
            If blnIsValid Then
                Try
                    udtPracticeModel = udtServiceProvider.PracticeList.Item(CInt(strPracticeID))
                Catch ex As Exception
                    udtErrorList.Add(ErrorInfo.ErrorCodeList.I00046)   'Invalid SP/Practice information or SP/Practice Status
                    Return False
                End Try

                If Not IsNothing(udtPracticeModel) Then
                    If Not udtPracticeModel.RecordStatus = PracticeStatus.Active Then
                        udtErrorList.Add(ErrorInfo.ErrorCodeList.I00046)   'Invalid SP/Practice information or SP/Practice Status
                        blnIsValid = False
                    End If

                    If Not udtPracticeModel.PracticeName.Trim = strPracticeName.Trim Then
                        udtErrorList.Add(ErrorInfo.ErrorCodeList.I00046)   'Invalid SP/Practice information or SP/Practice Status
                        blnIsValid = False
                    End If
                Else
                    udtErrorList.Add(ErrorInfo.ErrorCodeList.I00046)   'Invalid SP/Practice information or SP/Practice Status
                    blnIsValid = False
                End If
            End If

            Return blnIsValid

        End Function



        ''' <summary>
        ''' Check SP information
        ''' </summary>
        ''' <param name="strSPID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function chkServiceProviderInfo(ByVal strSPID As String, ByVal strPracticeID As String, ByVal strPracticeName As String, _
                    ByVal strSPName As String, Optional ByVal blnCheckSPName As Boolean = True) As Boolean

            Dim udtDB As Database = New Database()
            Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL()
            Dim udtServiceProvider As ServiceProviderModel = Nothing
            Dim udtPracticeModel As PracticeModel = Nothing
            Dim blnIsValid As Boolean = True

            'SP (Active)
            Try
                udtServiceProvider = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strSPID)
            Catch ex As Exception
                Return False
            End Try

            If Not IsNothing(udtServiceProvider) Then
                If Not udtServiceProvider.RecordStatus = ServiceProviderStatus.Active Then
                    blnIsValid = False
                End If

                If blnCheckSPName And Not udtServiceProvider.EnglishName.Trim = strSPName.Trim Then
                    blnIsValid = False
                End If
            Else
                blnIsValid = False
            End If

            'Practice (Active)
            If blnIsValid Then
                udtPracticeModel = udtServiceProvider.PracticeList.Item(CInt(strPracticeID))

                If Not IsNothing(udtPracticeModel) Then
                    If Not udtPracticeModel.RecordStatus = PracticeStatus.Active Then
                        blnIsValid = False
                    End If

                    If Not udtPracticeModel.PracticeName.Trim = strPracticeName.Trim Then
                        blnIsValid = False
                    End If
                Else
                    blnIsValid = False
                End If
            End If

            Return blnIsValid

        End Function
#End Region

    End Class

End Namespace



