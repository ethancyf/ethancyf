Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.ServiceProvider
Imports Common.Component.SchemeInformation
Imports Common.Component.Scheme
Imports Common.ComFunction.GeneralFunction
Imports Common.Component
Imports System.Web
Imports Common.Component.ClaimCategory

Namespace Printout.EnrolmentInformation.Component

    Public Class GovProg

        Private GeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Private udtSchemeEFormBLL As SchemeEFormBLL = New SchemeEFormBLL

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, ByVal strLanguage As String)
            Me.New()
            DataBind(udtPracticeSchemeInfoList, strLanguage)
        End Sub

        Private Sub DataBind(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, ByVal strLanguage As String)
            Dim udtPrintoutHelper As New PrintoutHelper(strLanguage)

            Dim startTop As Single = 0.0!
            Dim udtPSI As PracticeSchemeInfoModel = udtPracticeSchemeInfoList.GetByIndex(0)

            Dim udtSchemeEFormList As SchemeEFormModelCollection
            udtSchemeEFormList = udtSchemeEFormBLL.GetAllSchemeEForm

            Dim udtSchemeEForm As SchemeEFormModel = Nothing
            Dim strSchemeDisplay As String = String.Empty

            rtxtBullet.SelectionBullet = True

            udtSchemeEForm = udtSchemeEFormList.Filter(udtPSI.SchemeCode, GeneralFunction.GetSystemDateTime)

            ' Gov Prog Name
            If strLanguage = CultureLanguage.TradChinese Then
                strSchemeDisplay = udtSchemeEForm.SchemeDescChi
            Else
                strSchemeDisplay = udtSchemeEForm.SchemeDesc
            End If

            strSchemeDisplay = strSchemeDisplay + " (" + udtSchemeEForm.DisplayCode + ")"

            ' Non Clinic Setting
            If udtPracticeSchemeInfoList.IsNonClinic Then
                strSchemeDisplay += String.Format("{0}({1})", Environment.NewLine, udtPrintoutHelper.RenderText("ProvideServiceAtNonClinicSetting"))
            End If

            udtPrintoutHelper.RenderText(txtScheme, strSchemeDisplay)

            startTop += txtScheme.Height!


            ' Category
            Dim udtCategoryList As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getDistinctCategoryByPracticeScheme(udtPracticeSchemeInfoList)

            If udtCategoryList.Count > 0 Then

                For Each udtClaimCategory As ClaimCategoryModel In udtCategoryList
                    Dim strCategoryDisplay As String = String.Empty
                    Dim txtCategory As New TextBox
                    txtCategory.Top = startTop
                    txtCategory.Left = 0.2!
                    txtCategory.Height = 0.2!
                    txtCategory.Width = txtScheme.Width!

                    If strLanguage = CultureLanguage.TradChinese Then
                        strCategoryDisplay = udtClaimCategory.CategoryNameChi
                    Else
                        strCategoryDisplay = udtClaimCategory.CategoryName
                    End If

                    strCategoryDisplay = "-   " + strCategoryDisplay

                    udtPrintoutHelper.RenderText(txtCategory, strCategoryDisplay)
                    Detail1.Controls.Add(txtCategory)

                    startTop += txtCategory.Height!
                Next

                startTop += 0.01!
            End If

            Detail1.Height = startTop

        End Sub
    End Class

End Namespace
