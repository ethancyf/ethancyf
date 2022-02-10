
Imports Common.ComFunction

Namespace Component.COVID19.PrintOut.Common

    Public Class PrintoutHelper

        Public Enum FormType
            Vaccination
            Exemption
        End Enum

        Public Const PrintoutDisplayRefNoParameter As String = "COVID19_Vaccination_Record_Printout_DisplayRefNo"
        Public Const QRCodeDisplayRefNoParameter As String = "COVID19_Vaccination_Record_QRCode_DisplayRefNo"

        ' Printout Ref No
        Public Function DisplayPrintoutRefNo(ByVal FormType As FormType) As Boolean
            Dim blnDisplay As Boolean = True
            Dim udtGeneralFunction As New GeneralFunction
            Dim strParm1 As String = String.Empty
            Dim strParm2 As String = String.Empty
            Dim strParmValue As String = String.Empty

            'Get QRCode Version
            udtGeneralFunction.getSystemParameter(PrintoutDisplayRefNoParameter, strParm1, strParm2)

            Select Case FormType
                Case FormType.Vaccination
                    strParmValue = strParm1

                Case FormType.Exemption
                    strParmValue = strParm2
            End Select

            If strParmValue.Equals(YesNo.Yes) Then
                blnDisplay = True
            Else
                blnDisplay = False
            End If

            Return blnDisplay

        End Function

        ' QR Code Ref No
        Public Function DisplayQRCodeRefNo(ByVal FormType As FormType) As Boolean
            Dim blnDisplay As Boolean = True
            Dim udtGeneralFunction As New GeneralFunction
            Dim strParm1 As String = String.Empty
            Dim strParm2 As String = String.Empty
            Dim strParmValue As String = String.Empty

            'Get QRCode Version
            udtGeneralFunction.getSystemParameter(QRCodeDisplayRefNoParameter, strParm1, strParm2)

            Select Case FormType
                Case FormType.Vaccination
                    strParmValue = strParm1

                Case FormType.Exemption
                    strParmValue = strParm2
            End Select

            If strParmValue.Equals(YesNo.Yes) Then
                blnDisplay = True
            Else
                blnDisplay = False
            End If

            Return blnDisplay

        End Function

    End Class

End Namespace