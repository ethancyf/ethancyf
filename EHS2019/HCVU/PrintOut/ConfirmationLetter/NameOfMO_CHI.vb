Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component
Imports GrapeCity.ActiveReports.Document.Section

Namespace PrintOut.ConfirmationLetter
    Public Class NameOfMO_CHI

        Private udtSPModel As ServiceProvider.ServiceProviderModel
        Private blnSPPermanent As Boolean
        Private strSchemeCodeArrayList As ArrayList

        Public Sub New(ByVal udtSPModel As ServiceProvider.ServiceProviderModel, ByVal blnSPPermanent As Boolean, ByVal strSchemeCodeArrayList As ArrayList)
            InitializeComponent()

            Me.udtSPModel = udtSPModel
            Me.blnSPPermanent = blnSPPermanent
            'Me.strSchemeCodeArrayList = strSchemeCodeArrayList
            Me.strSchemeCodeArrayList = New ArrayList
            For Each strSchemeCode As String In strSchemeCodeArrayList
                Me.strSchemeCodeArrayList.Add(strSchemeCode)
            Next
        End Sub


        Private Sub NameOfMO_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim btnFirstPrintedMO As Boolean = False
            Dim btnLastPrintedMO As Boolean = False
            Dim MODisplayName As String = String.Empty
            Dim sngStartTop As Single = 0.0!
            Me.PrintWidth = 6.563!
            Me.dtlNameOfMO.Height = sngStartTop

            Dim intMODisplaySeqArrayList As New System.Collections.ArrayList()

            'For Each SchemeCode As String In strSchemeCodeArrayList
            If blnSPPermanent Then
                'Permanent Service Provider
                For Each udtPracticeModel As Practice.PracticeModel In Me.udtSPModel.PracticeList.Values
                    For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
                        If strSchemeCodeArrayList.Contains(udtPracticeSchemeInfoModel.SchemeCode) Then
                            If (udtPracticeSchemeInfoModel.RecordStatus <> SchemeInformationStatus.Delisted) And (udtPracticeSchemeInfoModel.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary) And (udtPracticeSchemeInfoModel.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary) Then
                                If Not intMODisplaySeqArrayList.Contains(udtPracticeModel.MODisplaySeq) Then
                                    intMODisplaySeqArrayList.Add(udtPracticeModel.MODisplaySeq)
                                End If
                            End If
                        End If
                    Next
                Next
            Else
                'Staging Service Provider
                For Each udtPracticeModel As Practice.PracticeModel In Me.udtSPModel.PracticeList.Values
                    For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
                        If strSchemeCodeArrayList.Contains(udtPracticeSchemeInfoModel.SchemeCode) Then
                            If udtPracticeSchemeInfoModel.RecordStatus <> SchemeInformationStagingStatus.DelistedVoluntary AndAlso udtPracticeSchemeInfoModel.RecordStatus <> SchemeInformationStagingStatus.DelistedInvoluntary Then
                                If Not intMODisplaySeqArrayList.Contains(udtPracticeModel.MODisplaySeq) Then
                                    intMODisplaySeqArrayList.Add(udtPracticeModel.MODisplaySeq)
                                End If
                            End If
                        End If
                    Next
                Next
            End If
            ' Next

            'sort the practice MO in order (Only valid record exists)
            intMODisplaySeqArrayList.Sort()

            Dim intMOPrintOrder As Integer = 1
            For Each intMODisplaySeq As Integer In intMODisplaySeqArrayList
                For Each udtMOModel As MedicalOrganization.MedicalOrganizationModel In Me.udtSPModel.MOList.Values
                    If (udtMOModel.DisplaySeq.Value = intMODisplaySeq) Then
                        If blnSPPermanent Then
                            If udtMOModel.RecordStatus <> MedicalOrganizationStatus.Delisted Then
                                'Permanent Service Provider
                                If intMOPrintOrder = 1 Then
                                    btnFirstPrintedMO = True
                                Else
                                    btnFirstPrintedMO = False
                                End If
                                If intMOPrintOrder = intMODisplaySeqArrayList.Count Then
                                    btnLastPrintedMO = True
                                Else
                                    btnLastPrintedMO = False
                                End If
                                If IsNothing(udtMOModel.MOChiName) Or udtMOModel.MOChiName.Trim.Equals(String.Empty) Then
                                    MODisplayName = udtMOModel.MOEngName
                                Else
                                    MODisplayName = udtMOModel.MOChiName
                                End If
                                Me.CreateNameOfMOTextBox(MODisplayName, udtMOModel.DisplaySeq, sngStartTop, btnFirstPrintedMO, btnLastPrintedMO)

                                sngStartTop += 0.219!
                            End If
                            'intMOPrintOrder += intMOPrintOrder
                        Else
                            If udtMOModel.RecordStatus <> MedicalOrganizationStagingStatus.Delisted Then
                                'staging Service Provider
                                If intMOPrintOrder = 1 Then
                                    btnFirstPrintedMO = True
                                Else
                                    btnFirstPrintedMO = False
                                End If
                                If intMOPrintOrder = intMODisplaySeqArrayList.Count Then
                                    btnLastPrintedMO = True
                                Else
                                    btnLastPrintedMO = False
                                End If
                                If IsNothing(udtMOModel.MOChiName) Or udtMOModel.MOChiName.Trim.Equals(String.Empty) Then
                                    MODisplayName = udtMOModel.MOEngName
                                Else
                                    MODisplayName = udtMOModel.MOChiName
                                End If
                                Me.CreateNameOfMOTextBox(MODisplayName, udtMOModel.DisplaySeq, sngStartTop, btnFirstPrintedMO, btnLastPrintedMO)

                                sngStartTop += 0.219!
                            End If
                            'intMOPrintOrder += intMOPrintOrder
                        End If
                    End If
                Next
                intMOPrintOrder = intMOPrintOrder + 1
            Next

            Me.dtlNameOfMO.Height = sngStartTop - 0.219!
        End Sub


        Private Sub CreateNameOfMOTextBox(ByVal strNameOfMO As String, ByVal intMODisplaySeq As Integer, ByVal sngStartTop As Single, ByVal btnFirst As Boolean, ByVal btnlast As Boolean)
            Dim textBox As TextBox
            Dim strSchemeDesc As String = String.Empty
            Dim strSchemeDisplayName As String = String.Empty


            textBox = New TextBox()
            CType(textBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.dtlNameOfMO.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {textBox})
            textBox.Width = 6.563!
            'textBox.Width = 4.032!
            'If strNameOfMO.Length > 35 Then
            If strNameOfMO.Length > 90 Then
                'textBox.Height = 0.438!
                textBox.Height = 0.4!
            Else
                'textBox.Height = 0.219!
                textBox.Height = 0.2!
            End If
            'textBox.Location = New Drawing.PointF(2.531!, sngStartTop)
            textBox.Location = New Drawing.PointF(0.0!, sngStartTop)
            If btnFirst And btnlast Then
                textBox.Text = String.Format("({0})", strNameOfMO)
            Else
                If btnFirst And Not btnlast Then
                    textBox.Text = String.Format("({0},", strNameOfMO)
                Else
                    If Not btnFirst And Not btnlast Then
                        textBox.Text = String.Format("{0},", strNameOfMO)
                    Else
                        If Not btnFirst And btnlast Then
                            textBox.Text = String.Format("{0})", strNameOfMO)
                        End If
                    End If
                End If
            End If
            'textBox.Text = strNameOfMO
            textBox.Name = "txtMO" + intMODisplaySeq.ToString().Trim() + "Text"
            'textBox.Location = New System.Drawing.PointF(0.0!, sngStartTop)
            textBox.Font = New System.Drawing.Font("HA_MingLiU", 14.25!, Drawing.FontStyle.Bold)
            textBox.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
            CType(textBox, System.ComponentModel.ISupportInitialize).EndInit()

            If strNameOfMO.Length > 80 Then
                'sngStartTop += 0.438!
                sngStartTop += 0.4!
            Else
                'sngStartTop += 0.219!
                sngStartTop += 0.2!
            End If
        End Sub
    End Class
End Namespace

