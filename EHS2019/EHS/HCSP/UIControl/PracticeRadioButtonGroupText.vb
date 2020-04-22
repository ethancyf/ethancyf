Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.VoucherScheme
Imports Common.Format
Imports Common.Component
Imports Common.Component.Scheme

Public Class PracticeRadioButtonGroupText
    Inherits System.Web.UI.WebControls.RadioButtonList

    Public Enum DisplayMode
        BankAccount
        Address
    End Enum

    'Values
    Private _blnMaskBankAccountNo As Boolean

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        MyBase.OnInit(e)
    End Sub

    Public Sub BuildRadioButtonGroup(ByVal blnRetainSelection As Boolean, ByVal practiceDisplays As BLL.PracticeDisplayModelCollection, ByVal practices As PracticeModelCollection, ByVal strlanguage As String, ByVal displayMode As DisplayMode)

        Dim strSelectedValue As String = Me.SelectedValue
        Dim blnIsFound As Boolean = False

        Me.Items.Clear()

        If Not practiceDisplays Is Nothing Then
            Dim formatter As Formatter = New Formatter()
            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()

            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing

            For Each practiceDisplay As BLL.PracticeDisplayModel In practiceDisplays

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
                ' -----------------------------------------------------------------------------------------
                ' Filter practice if profession is not available for claim
                If Not practiceDisplay.Profession.IsClaimPeriod() Then Continue For
                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

                If practiceDisplay.PracticeID.ToString() = strSelectedValue Then
                    blnIsFound = True
                End If

                Dim strAreaCode As String = IIf(practiceDisplay.AddressCode.HasValue, practiceDisplay.AddressCode.GetValueOrDefault(0), String.Empty).ToString()
                Dim bankAccountNo As String = IIf(Me._blnMaskBankAccountNo, formatter.maskBankAccount(practiceDisplay.BankAccountNo), practiceDisplay.BankAccountNo)

                'Create Practice Item
                Dim li As ListItem = New ListItem()
                li.Attributes("DataTextField") = practiceDisplay.PracticeName
                li.Attributes("DataValueField") = practiceDisplay.BankAccountNo
                li.Attributes("PracticeDisplaySeq") = practiceDisplay.PracticeID

                Dim sbDescription As StringBuilder = New StringBuilder()

                ' Bank Account
                If Me._blnMaskBankAccountNo Then
                    bankAccountNo = formatter.maskBankAccount(practiceDisplay.BankAccountNo)
                Else
                    bankAccountNo = practiceDisplay.BankAccountNo
                End If

                ' Practice Name
                Dim strPracticeName As String = String.Empty
                If strlanguage = Common.Component.CultureLanguage.TradChinese Then
                    strPracticeName = practiceDisplay.PracticeNameChi
                Else
                    strPracticeName = practiceDisplay.PracticeName
                End If

                ' Practice Name (Practice ID) [Bank Account]
                ' [Practice Address]
                If strlanguage = Common.Component.CultureLanguage.TradChinese AndAlso Not practiceDisplay.DisplayEngOnly AndAlso Not practiceDisplay.BuildingChi Is Nothing AndAlso practiceDisplay.BuildingChi.Trim() <> "" Then
                    sbDescription.Append(String.Format("{0} ({1}) [{2}]{3}[{4}]", strPracticeName, practiceDisplay.PracticeID, bankAccountNo, "<br>", _
                        formatter.formatAddressChi(practiceDisplay.Room, practiceDisplay.Floor, practiceDisplay.Block, practiceDisplay.BuildingChi, practiceDisplay.District, strAreaCode)))
                Else                    
                    sbDescription.Append(String.Format("{0} ({1}) [{2}]{3}[{4}]", strPracticeName, practiceDisplay.PracticeID, bankAccountNo, "<br>", _
                        formatter.formatAddress(practiceDisplay.Room, practiceDisplay.Floor, practiceDisplay.Block, practiceDisplay.Building, practiceDisplay.District, strAreaCode)))
                End If

                li.Text = sbDescription.ToString()
                li.Value = practiceDisplay.PracticeID
                Me.Items.Add(li)
            Next
        End If

        If blnRetainSelection AndAlso blnIsFound Then
            Me.SelectedValue = strSelectedValue
        End If

    End Sub

    Public Property MaskBankAccountNo() As Boolean
        Get
            Return Me._blnMaskBankAccountNo
        End Get
        Set(ByVal value As Boolean)
            Me._blnMaskBankAccountNo = value
        End Set
    End Property

End Class
