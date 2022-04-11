IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'tri_COVID19DischargePatient_after_ins')
	DROP TRIGGER [dbo].[tri_COVID19DischargePatient_after_ins]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	28 Mar 2022
-- Description:		Add column filler 01 - 04
-- =============================================
-- =============================================
-- Author:		Winnie SUEN
-- CR No.		CRE20-023 (Immu record)
-- Create date: 6 May 2021
-- Description:	Add COVID19DischargePatientLOG
-- =============================================

CREATE Trigger [dbo].[tri_COVID19DischargePatient_after_ins]
	ON [dbo].[COVID19DischargePatient]
	AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON;

DECLARE @AddLog VARCHAR(1) = ''
SELECT @AddLog = Parm_Value1 FROM [SystemParameters] WHERE [Scheme_Code] = 'ALL' AND [Parameter_Name] = 'TurnOnCOVID19DischargePatientLOGTrigger'

IF @AddLog <> 'Y'
BEGIN
	RETURN
END

INSERT INTO [dbo].[COVID19DischargePatientLOG]
(
	[System_Dtm],	
	[Row_No],
	[CHP_Index_No],		
	[Encrypt_Field1],
	[Encrypt_Field2_1],
	[Encrypt_Field2_2],
	[Encrypt_Field12],
	[Sex],
	[Phone1_No],
	[Phone2_No],	
	[Phone3_No],	
	[DOB_Format],	
	[DOB],	
	[Discharge_Date],
	[File_ID],
	[Import_Remark],
	[Create_Dtm],
	[Infection_Date],
	[Recovery_Date],
	[Death_Indicator],
	[DataSource]
)
(
	SELECT
		GetDate(),
		[Row_No],
		[CHP_Index_No],		
		[Encrypt_Field1],
		[Encrypt_Field2_1],
		[Encrypt_Field2_2],
		[Encrypt_Field12],
		[Sex],
		[Phone1_No],
		[Phone2_No],	
		[Phone3_No],	
		[DOB_Format],	
		[DOB],	
		[Discharge_Date],
		[File_ID],
		[Import_Remark],
		[Create_Dtm],
		[Infection_Date],
		[Recovery_Date],
		[Death_Indicator],
		[DataSource]
	FROM inserted	
)

END 
GO

