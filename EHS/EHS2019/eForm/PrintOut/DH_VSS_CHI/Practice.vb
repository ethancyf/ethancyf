Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Format
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.DH_VSS_CHI
    Public Class Practice

#Region "Variables"

        Private _blnBuiltSchemeInfo As Boolean
        Private _intPracticeNo As Integer
        Private _udtPractice As PracticeModel
        Private _udtSchemeEFList As SchemeEFormModelCollection
        Private _udtStaticDataList As StaticDataModelCollection

#End Region

#Region "Fields"

        Private udtFormatter As New Formatter
        Private udtReportFunction As New ReportFunction

#End Region

        Public Sub New(ByVal intPracticeNo As Integer, ByVal udtPractice As PracticeModel, ByVal udtSchemeEFList As SchemeEFormModelCollection, ByVal udtStaticDataList As StaticDataModelCollection, ByVal blnBuiltSchemeInfo As Boolean)
            InitializeComponent()

            _intPracticeNo = intPracticeNo
            _udtPractice = udtPractice
            _udtSchemeEFList = udtSchemeEFList
            _udtStaticDataList = udtStaticDataList
            _blnBuiltSchemeInfo = blnBuiltSchemeInfo
        End Sub

        Private Sub dtlPractice_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlPractice.Format
        End Sub

        Private Sub Practice_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            dtlPractice.KeepTogether = True
            txtPracticeNoText.Text = String.Format("執業地點（{0}）：(請提供執業地址的證明文件，例如公共事業機構單據或銀行結單的副本。)", _intPracticeNo)

            udtReportFunction.formatUnderLineTextBox(_udtPractice.PracticeName, txtPracticeEngName, 3.0!)
            udtReportFunction.formatUnderLineTextBox(_udtPractice.PracticeNameChi, txtPracticeChiName, 3.0!)
            udtReportFunction.formatUnderLineTextBox(udtFormatter.formatAddress(_udtPractice.PracticeAddress.Room, _udtPractice.PracticeAddress.Floor, _udtPractice.PracticeAddress.Block, _udtPractice.PracticeAddress.Building, _udtPractice.PracticeAddress.District, _udtPractice.PracticeAddress.AreaCode), txtPracticeEngAddress, 5.375!)
            udtReportFunction.formatUnderLineTextBox(udtFormatter.formatAddressChi(_udtPractice.PracticeAddress), txtPracticeChiAddress, 5.375!)
            udtReportFunction.formatUnderLineTextBox(_udtPractice.PracticeAddress.DistrictChiDesc, txtPracticeDistrict, 3.0!)
            udtReportFunction.formatUnderLineTextBox(_udtPractice.PhoneDaytime, txtPracticeTeleNo, 3.0!)

            Dim strJoinedSchemeCode As String = String.Empty
            Dim strJoinedSchemeCodeCHI As String = String.Empty
            Dim udtJoinedSchemeEFList As New SchemeEFormModelCollection

            For Each udtPracticeScheme As PracticeSchemeInfoModel In _udtPractice.PracticeSchemeInfoList.Values
                Dim udtSchemeEF As SchemeEFormModel = _udtSchemeEFList.Filter(udtPracticeScheme.SchemeCode)

                If Not strJoinedSchemeCode.Contains(udtSchemeEF.DisplayCode.Trim) Then
                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    If udtPracticeScheme.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                        strJoinedSchemeCodeCHI += String.Format("{0}（{1}）, ", udtSchemeEF.SchemeDescChi.Trim, HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo("zh-tw")))
                    Else
                        strJoinedSchemeCodeCHI += String.Format("{0}, ", udtSchemeEF.SchemeDescChi.Trim)
                    End If
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]

                    strJoinedSchemeCode += String.Format("{0}, ", udtSchemeEF.DisplayCode.Trim)
                    udtJoinedSchemeEFList.Add(udtSchemeEF)
                End If
            Next

            strJoinedSchemeCodeCHI = strJoinedSchemeCodeCHI.Remove(strJoinedSchemeCodeCHI.Length - 2)
            udtReportFunction.formatUnderLineTextBox(strJoinedSchemeCodeCHI, Me.txtPracticeJoinedScheme, 3.0!)

            ' CRE16-025-04 (Lowering voucher eligibility age - Static Doc) [Start][Winnie]
            ' Add group footer, so practice info can keep in same page while scheme info could display on new page
            If _blnBuiltSchemeInfo Then
                'dtlPractice.KeepTogether = False
                sreJoinedSchemeInfo.Report = New AvailPracticeMasterSchemeBatch(_udtPractice.PracticeSchemeInfoList, udtJoinedSchemeEFList, _udtStaticDataList)
            Else
                gftJoinedSchemeInfo.Height = 0
            End If
            ' CRE16-025-04 (Lowering voucher eligibility age - Static Doc) [End][Winnie]
        End Sub

    End Class
End Namespace