IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_AR_Report_Schedule]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_AR_Report_Schedule]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 Jul 2020
-- CR No.			CRE19-031 (VSS MMR Upload)
-- Description:		VSS Non-immune Adults Weekly Statistic Report (eHSW0006)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Raiman Chong
-- Modified date:	10 Nov 2020
-- CR No.:			CRE20-014-02 (Gov SIV 2020_21) 
-- Description:		New Weekly Report eHSW0008
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin TANG
-- Modified date:	22 JULY 2020
-- CR No.:			CRE19-022 (Inspection Schedule Weekly Report) 
-- Description:		New Weekly Report eHSW0005
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	26 Sep 2019
-- CR No.:			CRE19-001 (PPP 2019/20) 
-- Description:		New Weekly Report eHSW0004 (replace eHSSF004)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	27 Aug 2019
-- CR No.:			CRE19-006 (DHC)
-- Description:		New Monthly Report eHSM0011
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	15 Mar 2019
-- CR No.:			CRE18-015 (Enable PCV13 weekly report eHS(S)W003 upon request)
-- Description:		Add input Param for eHS(S)W0003 to show 02 connect fail report
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	26 Sep 2018
-- CR No.:			CRE17-010 (OCSSS integration) 
-- Description:		New Monthly Report eHSM0009,eHSM0010
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	10 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		New Weekly Report eHSSF004
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	15 Mar 2018
-- CR No.:			CRE16-014 to 016 (Voucher aberrant and new monitoring)
-- Description:		1. New Daily Aberrant Report eHSD0030
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	14 Mar 2018
-- CR No.:			INT17-0023
-- Description:		Fix generation date on eHS(S)W0003 PCV13 Statistic Report - Weekly Basis
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	12 Jan 2018
-- CR No.:			CRS17-038
-- Description:		1. New Monthly Pneumococcal Claim Statistic Report eHSM0008
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	16 Nov 2017
-- CR No.:			CRS17-042
-- Description:		1. Change PCV13 Statistic Report eHS(S)W0003 generation date from Thursday to Monday weekly
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	23 Oct 2017
-- CR No.:			CRS17-038
-- Description:		1. Change PCV13 Statistic Report eHS(S)W0003 generation date from Monday to Thursday weekly
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Marco CHOI
-- Modified date:	3 Aug 2017
-- CR No.:			CRE16-026
-- Description:		1. New Weekly PCV13 Statistic Report eHS(S)W0003
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	11 Apr 2017
-- CR No.:			CRE17-001
-- Description:		1. New Monthly Service Provider Profile Data Report eHSM0005
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Chris YIM	
-- Modified date:	06 Feb 2017
-- CR No.:			CRE16-019
-- Description:		1. New Daily Deactivated eHRSS Token Report eHSD0029
--					2. New Monthly Deactivated eHRSS Token Report eHSM0004
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN		
-- Modified date:	26 Oct 2016
-- CR No.:			CRE16-010
-- Description:		1. New Monthly Aberrant Report eHSM0003
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN		
-- Modified date:	27 Apr 2016
-- CR No.:			CRE16-001
-- Description:		1. New Weekly Aberrant Report for Vaccine eHSW0002	
--					2. Change Request_by to 'eHS(S)'
-- =============================================
-- =============================================
-- Author:			Vincent YUEN
-- Create date:		22 Jan 2010
-- Description:		Run by Scheduler for adding file generation quene for AR reports 
-- =============================================


CREATE PROCEDURE [dbo].[proc_AR_Report_Schedule]
AS BEGIN
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

	---------------------------------------------------------------------------------------------
	-- Daily Report
	
	-- Aberrant Pattern in Use of Vouchers - eHealth (Subsidies) Account perspective File
	EXEC proc_FileGenerationQueue_add_byFileID  'eHSD0015' ,'' ,'eHS(S)'

	-- Aberrant Pattern in Use of Vouchers - HCSP perspective File
	EXEC proc_FileGenerationQueue_add_byFileID  'eHSD0016' ,'' ,'eHS(S)'

	-- Report of Deactivated Tokens by eHRSS - Daily Basis
	EXEC proc_FileGenerationQueue_add_byFileID  'eHSD0029' ,'' ,'eHS(S)'

	-- Aberrant Pattern in Use of Vouchers - eHealth (Subsidies) Accounts who used vouchers at the targeted age
	EXEC proc_FileGenerationQueue_add_byFileID  'eHSD0030' ,'' ,'eHS(S)'

	---------------------------------------------------------------------------------------------
	-- Weekly Report: Generated on Monday
	
	-- Check Date name avoid the effect of SET DATEFIRST 
	IF DATENAME(weekday, getdate()) = 'Monday'
	BEGIN
		-- HCSP with Frequent Rejection of Temporary eHealth (Subsidies) Account File
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSW0001' ,'' ,'eHS(S)'
		
		-- Aberrant Pattern in Vaccination Subsidies Schemes - Weekly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSW0002' ,'' ,'eHS(S)'

		-- PCV13 Statistic Report - Weekly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSW0003' ,'@Show_02_Report===3;;;1;;;Y' ,'eHS(S)'
		
		-- Weekly Vaccination Schedule Report
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSW0004' ,'' ,'eHS(S)'

		-- Inspection Schedule Weekly Report
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSW0005' ,'' ,'eHS(S)'

		-- Weekly VSS Non-immune Adults Statistic - Weekly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSW0006' ,'' ,'eHS(S)'

		-- Report_Using_Government_Supplied_Vaccine_Template
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSW0008' ,'' ,'eHS(S)'
	END

	---------------------------------------------------------------------------------------------
	-- Monthly Report: Generated 3rd of each month

	IF DATEPART(DAY, getdate()) = '3'
	BEGIN
		-- Aberrant Pattern in Vaccination Subsidy Schemes - Monthly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSM0003' ,'' ,'eHS(S)'
	END

	-- Monthly Report: Generated 1st of each month
	IF DATEPART(DAY, getdate()) = '1'
	BEGIN
		-- Report of Deactivated Tokens by eHRSS - Monthly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSM0004' ,'' ,'eHS(S)'
		
		-- Report of Service Provider Profile Data - Monthly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSM0005' ,'' ,'eHS(S)'

		-- Report of Pneumococcal Claim Statistic - Monthly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSM0008' ,'' ,'eHS(S)'

		-- Report of VSS with OCSSS Checking - Monthly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSM0009' ,'' ,'eHS(S)'

		-- Report of Voucher with OCSSS Checking - Monthly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSM0010' ,'' ,'eHS(S)'

		-- Report of Use of Voucher in DHC - Monthly Basis
		EXEC proc_FileGenerationQueue_add_byFileID  'eHSM0011' ,'' ,'eHS(S)'
	END

END
GO
