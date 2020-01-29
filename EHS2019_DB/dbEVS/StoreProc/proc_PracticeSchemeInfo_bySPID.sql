IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInfo_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInfo_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	5 August 2016
-- Description:		Add Clinic_Type
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Winnie SUEN
-- Modified date:	19 June 2015
-- Description:		1. Add field Provide_Service
--					2. Don't retrieve those subsidizegroup which record_status = 'I' 
-- =============================================
-- =============================================
-- Modification History
-- CR No:			CRE13-019-02 Extend HCVS to China
-- Modified by:		Chris YIM
-- Modified date:	16 Feb 2015
-- Description:		Add two columns [Scheme_Display_Seq] and [Subsidize_Display_Seq]
-- =============================================
-- =============================================
-- Author:		Pak Ho LEE
-- Create date: 05 Aug 2009
-- Description:	Retrieve the PracticeSchemeInfo of SP
-- =============================================


CREATE PROCEDURE [dbo].[proc_PracticeSchemeInfo_bySPID] 
	@SP_ID char(8)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

SELECT
	PSI.[SP_ID],
	PSI.[Subsidize_Code],
	PSI.[Practice_Display_Seq],
	PSI.[Service_Fee],
	PSI.[Delist_Status],
	PSI.[Delist_Dtm],
	PSI.[Effective_Dtm],
	PSI.[Record_Status], 
	PSI.[Remark],
	PSI.[Create_Dtm],
	PSI.[Create_By],
	PSI.[Update_Dtm],
	PSI.[Update_By],
	PSI.[TSMP],
	PSI.[Scheme_Code],
	PSI.[ProvideServiceFee],
	SB.Display_Seq AS [Scheme_Display_Seq],
	SGB.Display_Seq AS [Subsidize_Display_Seq],
	PSI.[Provide_Service],
	PSI.[Clinic_Type]
FROM
	[PracticeSchemeInfo] PSI
		INNER JOIN [Practice] P 
			ON PSI.SP_ID = P.SP_ID
				AND PSI.Practice_Display_Seq = P.Display_Seq
				AND PSI.Record_Status = 'A'
				AND P.Record_Status = 'A'
		INNER JOIN SchemeBackOffice SB
			ON PSI.Scheme_Code = SB.Scheme_Code
				AND PSI.Effective_Dtm BETWEEN SB.Effective_Dtm AND SB.Expiry_Dtm
				AND SB.Record_Status = 'A'
		INNER JOIN SubsidizeGroupBackOffice SGB
			ON SB.Scheme_Code = SGB.Scheme_Code
				AND SB.Scheme_Seq = SGB.Scheme_Seq
				AND PSI.Subsidize_Code = SGB.Subsidize_Code
				AND SGB.Record_Status <> 'I' --= 'A'
WHERE
	PSI.[SP_ID] = @SP_ID

END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInfo_bySPID] TO HCSP
GO
