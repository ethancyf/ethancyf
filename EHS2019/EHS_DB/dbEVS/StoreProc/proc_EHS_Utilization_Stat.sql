IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_Utilization_Stat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_Utilization_Stat]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	14 Sept 2011
-- CR No.:			CRE11-024-01 (Enhancement on HCVS Extension)
-- Description:		- added profession 'ROP' for registered optometrists;
--					- added "Report Generation Time" statement for new report template
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Vincent YUEN
-- Modified date:	16 Dec 2009
-- Description:		Add HSVISS for the Scheme Combiniation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	1 Sept 2009
-- Description:		Get the following information from Table "Statistic Table"
--					1. Scheme enrolment
--					2. No. of MO
--					3. Relationship of MO
--					4. No. of Practice
--					5. Professional Type
--					6. Cumulative SP Approved Account
--					7. Cumulative SP Activated Account
--					8. Cumulative Data Entry Account
--					9. Cumulative Delisted SP
--					10. Approved Account Scheme Information
--					11. Activated Account Scheme Information
--					12. Approved Account (RMP Only) Scheme Information
--					13. Activated Account (RMP Only) Scheme Information
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Create date: 03 Oct 2008
-- Description:	Statistics
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_Utilization_Stat]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @start_date varchar(10)
declare @temp_date varchar(10) 
declare @no_of_days	int
declare @end_date	varchar(10)
declare @thirdReportStartDate varchar(10)
declare @RMPReportStartDate varchar(10)

--default 2 weeks
--if @no_of_days is null
--begin
select @no_of_days=14
--end
--default current date
--if @end_date is null
--begin
select @end_date = convert(varchar, getdate(), 120)
--end

Select @start_date = convert(varchar, dateadd(d, -(@no_of_days), @end_date), 120)

if @start_date < '2009-09-01'
begin
select @start_date = '2009-09-01'
end

select @thirdReportStartDate = '2009-10-03'

select @RMPReportStartDate = '2009-10-05'

declare @HCVS_E	int
declare @CIVSS_E	int
declare @EVSSHSIVSS_E	int
declare @CIVSS_HCVSE_E	int
declare @HCVS_EVSSHSIVSS_E	int
declare @CIVSS_EVSSHSIVSS_E	int
declare @CIVSS_EVSSHSIVSS_HCVS_E	int
declare @E_Total	int
declare @E_Others	int

declare @HCVS_P	int
declare @CIVSS_P	int
declare @EVSSHSIVSS_P	int
declare @HSIVSS_P int
declare @RVP_P	int
declare @HCVS_CIVSS_P	int
declare @HCVS_EVSSHSIVSS_P	int
declare @HCVS_HSIVSS_P int
declare @HCVS_RVP_P	int
declare @CIVSS_EVSSHSIVSS_P	int
declare @CIVSS_HSIVSS_P int
declare @CIVSS_RVP_P	int
declare @EVSSHSIVSS_HSIVSS_P int
declare @EVSSHSIVSS_RVP_P	int
declare @HSIVSS_RVP_P int
declare @CIVSS_EVSSHSIVSS_HCVS_P	int
declare @CIVSS_HCVS_HSIVSS_P int
declare @CIVSS_HCVS_RVP_P	int
declare @EVSSHSIVSS_HCVS_HSIVSS_P int
declare @EVSSHSIVSS_HCVS_RVP_P	int
declare @HCVS_HSIVSS_RVP_P int
declare @CIVSS_EVSSHSIVSS_HSIVSS_P int
declare @CIVSS_EVSSHSIVSS_RVP_P	int
declare @CIVSS_HSIVSS_RVP_P int
declare @EVSSHSIVSS_HSIVSS_RVP_P int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P int
declare @CIVSS_EVSSHSIVSS_HCVS_RVP_P	int
declare @CIVSS_HCVS_HSIVSS_RVP_P int
declare @EVSSHSIVSS_HCVS_HSIVSS_RVP_P int
declare @CIVSS_EVSSHSIVSS_HSIVSS_RVP_P int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P int
declare @None_P	int
declare @P_Total	int
declare @P_Others	int

declare @MO_0	int
declare @MO_1	int
declare @MO_2	int
declare @MO_3	int
declare @MO_4	int
declare @MO_5	int
declare @MO_6	int
declare @MO_A6	int
declare @MO_Total	int

declare @Solo	int
declare @Partenship	int
declare @Shareholder	int
declare @Director	int
declare @Employee	int
declare @Others	int
declare @MO_Rel_Total	int

declare @Practice_0	int
declare @Practice_1	int
declare @Practice_2	int
declare @Practice_3	int
declare @Practice_4	int
declare @Practice_5	int
declare @Practice_6	int
declare @Practice_A6	int
declare @Practice_Total	int

declare @ENU	int
declare @RCM	int
declare @RCP	int
declare @RDT	int
declare @RMP	int
declare @RMT	int
declare @RNU	int
declare @ROP	int		-- CRE11-024-01: added
declare @ROT	int
declare @RPT	int
declare @RRD	int

declare @Prof_Total	int
declare @App_ENU	int
declare @App_RCM	int
declare @App_RCP	int
declare @App_RDT	int
declare @App_RMP	int
declare @App_RMT	int
declare @App_RNU	int
declare @App_ROP	int		-- CRE11-024-01: added
declare @App_ROT	int
declare @App_RPT	int
declare @App_RRD	int
declare @Approved_Account	int
declare @Act_ENU	int
declare @Act_RCM	int
declare @Act_RCP	int
declare @Act_RDT	int
declare @Act_RMP	int
declare @Act_RMT	int
declare @Act_RNU	int
declare @Act_ROP	int		-- CRE11-024-01: added
declare @Act_ROT	int
declare @Act_RPT	int
declare @Act_RRD	int
declare @Activated_Account	int

declare @DEAcct0	int
declare @DEAcct1	int
declare @DEAcct2	int
declare @DEAcct3	int
declare @DEAcct4	int
declare @DEAcct5	int
declare @DEAcct6	int
declare @DEAcct7	int
declare @DEAcct8	int
declare @DEAcct9	int
declare @DEAcct10	int
declare @DEAcct11	int
declare @DEAcct12	int
declare @DEAcct13	int
declare @DEAcct14	int
declare @DEAcct15	int
declare @DEAcct16	int
declare @DEAcct17	int
declare @DEAcct18	int
declare @DEAcct19	int
declare @DEAcct20	int
declare @DEAcct21	int
declare @DEAcct22	int
declare @DEAcct23	int
declare @DEAcct24	int
declare @DEAcct25	int
declare @DEAcct26	int
declare @DEAcct27	int
declare @DEAcct28	int
declare @DEAcct29	int
declare @DEAcct30	int
declare @DelistSP_NA	int
declare @DelistSP_A	int

declare @HCVS_E_Total int
declare @CIVSS_E_Total int
declare @EVSSHSIVSS_E_Total int
declare @CIVSS_HCVSE_E_Total int
declare @HCVS_EVSSHSIVSS_E_Total int
declare @CIVSS_EVSSHSIVSS_E_Total int
declare @CIVSS_EVSSHSIVSS_HCVS_E_Total int
declare @E_Total_Total int
declare @E_Others_Total int

declare @HCVS_P_Total int
declare @CIVSS_P_Total int
declare @EVSSHSIVSS_P_Total int
declare @HSIVSS_P_Total int
declare @RVP_P_Total int
declare @HCVS_CIVSS_P_Total int
declare @HCVS_EVSSHSIVSS_P_Total int
declare @HCVS_HSIVSS_P_Total int
declare @HCVS_RVP_P_Total int
declare @CIVSS_EVSSHSIVSS_P_Total int
declare @CIVSS_HSIVSS_P_Total int
declare @CIVSS_RVP_P_Total int
declare @EVSSHSIVSS_HSIVSS_P_Total int
declare @EVSSHSIVSS_RVP_P_Total int
declare @HSIVSS_RVP_P_Total int
declare @CIVSS_EVSSHSIVSS_HCVS_P_Total int
declare @CIVSS_HCVS_HSIVSS_P_Total int
declare @CIVSS_HCVS_RVP_P_Total int
declare @EVSSHSIVSS_HCVS_HSIVSS_P_Total int
declare @EVSSHSIVSS_HCVS_RVP_P_Total int
declare @HCVS_HSIVSS_RVP_P_Total int
declare @CIVSS_EVSSHSIVSS_HSIVSS_P_Total int
declare @CIVSS_EVSSHSIVSS_RVP_P_Total int
declare @CIVSS_HSIVSS_RVP_P_Total int
declare @EVSSHSIVSS_HSIVSS_RVP_P_Total int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P_Total int
declare @CIVSS_EVSSHSIVSS_HCVS_RVP_P_Total int
declare @CIVSS_HCVS_HSIVSS_RVP_P_Total int
declare @EVSSHSIVSS_HCVS_HSIVSS_RVP_P_Total int
declare @CIVSS_EVSSHSIVSS_HSIVSS_RVP_P_Total int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P_Total int
declare @None_P_Total	int
declare @P_Total_Total	int
declare @P_Others_Total	int

declare @MO_0_Total int
declare @MO_1_Total int
declare @MO_2_Total int
declare @MO_3_Total int
declare @MO_4_Total int
declare @MO_5_Total int
declare @MO_6_Total int
declare @MO_A6_Total int
declare @MO_Total_Total int
declare @Solo_Total int
declare @Partenship_Total int
declare @Shareholder_Total int
declare @Director_Total int
declare @Employee_Total int
declare @Others_Total int
declare @MO_Rel_Total_Total int
declare @Practice_0_Total int
declare @Practice_1_Total int
declare @Practice_2_Total int
declare @Practice_3_Total int
declare @Practice_4_Total int
declare @Practice_5_Total int
declare @Practice_6_Total int
declare @Practice_A6_Total int
declare @Practice_Total_Total int
declare @ENU_Total int
declare @RCM_Total int
declare @RCP_Total int
declare @RDT_Total int
declare @RMP_Total int
declare @RMT_Total int
declare @RNU_Total int
declare @ROP_Total int		-- CRE11-024-01: added
declare @ROT_Total int
declare @RPT_Total int
declare @RRD_Total int
declare @Prof_Total_Total int

declare @HCVS_A	int
declare @CIVSS_A	int
declare @EVSSHSIVSS_A	int
declare @HSIVSS_A int
declare @RVP_A	int
declare @HCVS_CIVSS_A	int
declare @HCVS_EVSSHSIVSS_A	int
declare @HCVS_HSIVSS_A int
declare @HCVS_RVP_A	int
declare @CIVSS_EVSSHSIVSS_A	int
declare @CIVSS_HSIVSS_A int
declare @CIVSS_RVP_A	int
declare @EVSSHSIVSS_HSIVSS_A int
declare @EVSSHSIVSS_RVP_A	int
declare @HSIVSS_RVP_A int
declare @CIVSS_EVSSHSIVSS_HCVS_A	int
declare @CIVSS_HCVS_HSIVSS_A int
declare @CIVSS_HCVS_RVP_A	int
declare @EVSSHSIVSS_HCVS_HSIVSS_A int
declare @EVSSHSIVSS_HCVS_RVP_A	int
declare @HCVS_HSIVSS_RVP_A int
declare @CIVSS_EVSSHSIVSS_HSIVSS_A int
declare @CIVSS_EVSSHSIVSS_RVP_A	int
declare @CIVSS_HSIVSS_RVP_A int
declare @EVSSHSIVSS_HSIVSS_RVP_A int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_A int
declare @CIVSS_EVSSHSIVSS_HCVS_RVP_A	int
declare @CIVSS_HCVS_HSIVSS_RVP_A int
declare @EVSSHSIVSS_HCVS_HSIVSS_RVP_A int
declare @CIVSS_EVSSHSIVSS_HSIVSS_RVP_A int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_A int
declare @Deliat_Total_A int
declare @A_Total	int
declare @A_Others	int

declare @HCVS_AC	int
declare @CIVSS_AC	int
declare @EVSSHSIVSS_AC	int
declare @HSIVSS_AC int
declare @RVP_AC	int
declare @HCVS_CIVSS_AC	int
declare @HCVS_EVSSHSIVSS_AC	int
declare @HCVS_HSIVSS_AC int
declare @HCVS_RVP_AC	int
declare @CIVSS_EVSSHSIVSS_AC	int
declare @CIVSS_HSIVSS_AC int
declare @CIVSS_RVP_AC	int
declare @EVSSHSIVSS_HSIVSS_AC int
declare @EVSSHSIVSS_RVP_AC	int
declare @HSIVSS_RVP_AC int
declare @CIVSS_EVSSHSIVSS_HCVS_AC	int
declare @CIVSS_HCVS_HSIVSS_AC int
declare @CIVSS_HCVS_RVP_AC	int
declare @EVSSHSIVSS_HCVS_HSIVSS_AC int
declare @EVSSHSIVSS_HCVS_RVP_AC	int
declare @HCVS_HSIVSS_RVP_AC int
declare @CIVSS_EVSSHSIVSS_HSIVSS_AC int
declare @CIVSS_EVSSHSIVSS_RVP_AC	int
declare @CIVSS_HSIVSS_RVP_AC int
declare @EVSSHSIVSS_HSIVSS_RVP_AC int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC int
declare @CIVSS_EVSSHSIVSS_HCVS_RVP_AC	int
declare @CIVSS_HCVS_HSIVSS_RVP_AC int
declare @EVSSHSIVSS_HCVS_HSIVSS_RVP_AC int
declare @CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC int
declare @Deliat_Total_AC int
declare @AC_Total	int
declare @AC_Others	int

declare @HCVS_RMP_A	int
declare @CIVSS_RMP_A	int
declare @EVSSHSIVSS_RMP_A	int
declare @HSIVSS_RMP_A int
declare @RVP_RMP_A	int
declare @HCVS_CIVSS_RMP_A	int
declare @HCVS_EVSSHSIVSS_RMP_A	int
declare @HCVS_HSIVSS_RMP_A int
declare @HCVS_RVP_RMP_A	int
declare @CIVSS_EVSSHSIVSS_RMP_A	int
declare @CIVSS_HSIVSS_RMP_A int
declare @CIVSS_RVP_RMP_A	int
declare @EVSSHSIVSS_HSIVSS_RMP_A int
declare @EVSSHSIVSS_RVP_RMP_A	int
declare @HSIVSS_RVP_RMP_A int
declare @CIVSS_EVSSHSIVSS_HCVS_RMP_A	int
declare @CIVSS_HCVS_HSIVSS_RMP_A int
declare @CIVSS_HCVS_RVP_RMP_A	int
declare @EVSSHSIVSS_HCVS_HSIVSS_RMP_A int
declare @EVSSHSIVSS_HCVS_RVP_RMP_A	int
declare @HCVS_HSIVSS_RVP_RMP_A int
declare @CIVSS_EVSSHSIVSS_HSIVSS_RMP_A int
declare @CIVSS_EVSSHSIVSS_RVP_RMP_A	int
declare @CIVSS_HSIVSS_RVP_RMP_A int
declare @EVSSHSIVSS_HSIVSS_RVP_RMP_A int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A int
declare @CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A	int
declare @CIVSS_HCVS_HSIVSS_RVP_RMP_A int
declare @EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A int
declare @CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A int
declare @Deliat_Total_RMP_A int
declare @A_RMP_Total	int
declare @A_RMP_Others	int

declare @HCVS_RMP_AC	int
declare @CIVSS_RMP_AC	int
declare @EVSSHSIVSS_RMP_AC	int
declare @HSIVSS_RMP_AC int
declare @RVP_RMP_AC	int
declare @HCVS_CIVSS_RMP_AC	int
declare @HCVS_EVSSHSIVSS_RMP_AC	int
declare @HCVS_HSIVSS_RMP_AC int
declare @HCVS_RVP_RMP_AC	int
declare @CIVSS_EVSSHSIVSS_RMP_AC	int
declare @CIVSS_HSIVSS_RMP_AC int
declare @CIVSS_RVP_RMP_AC	int
declare @EVSSHSIVSS_HSIVSS_RMP_AC int
declare @EVSSHSIVSS_RVP_RMP_AC	int
declare @HSIVSS_RVP_RMP_AC int
declare @CIVSS_EVSSHSIVSS_HCVS_RMP_AC	int
declare @CIVSS_HCVS_HSIVSS_RMP_AC int
declare @CIVSS_HCVS_RVP_RMP_AC	int
declare @EVSSHSIVSS_HCVS_HSIVSS_RMP_AC int
declare @EVSSHSIVSS_HCVS_RVP_RMP_AC	int
declare @HCVS_HSIVSS_RVP_RMP_AC int
declare @CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC int
declare @CIVSS_EVSSHSIVSS_RVP_RMP_AC	int
declare @CIVSS_HSIVSS_RVP_RMP_AC int
declare @EVSSHSIVSS_HSIVSS_RVP_RMP_AC int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC int
declare @CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC	int
declare @CIVSS_HCVS_HSIVSS_RVP_RMP_AC int
declare @EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC int
declare @CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC int
declare @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC int
declare @Deliat_Total_RMP_AC int
declare @AC_RMP_Total	int
declare @AC_RMP_Others	int

--declare @HCVS_A_Total	int
--declare @CIVSS_A_Total	int
--declare @EVSSHSIVSS_A_Total	int
--declare @RVP_A_Total	int
--declare @HCVS_CIVSS_A_Total	int
--declare @HCVS_EVSSHSIVSS_A_Total	int
--declare @HCVS_RVP_A_Total	int
--declare @CIVSS_EVSSHSIVSS_A_Total	int
--declare @CIVSS_RVP_A_Total	int
--declare @EVSSHSIVSS_RVP_A_Total	int
--declare @CIVSS_EVSSHSIVSS_HCVS_A_Total	int
--declare @EVSSHSIVSS_HCVS_RVP_A_Total	int
--declare @CIVSS_HCVS_RVP_A_Total	int
--declare @CIVSS_EVSSHSIVSS_RVP_A_Total	int
--declare @CIVSS_EVSSHSIVSS_HCVS_RVP_A_Total	int
--declare @A_Total_Total	int

Declare @Stat table (Enrolment_Date varchar(10),
					HCVS_E	int,
					CIVSS_E	int,
					EVSSHSIVSS_E	int,
					CIVSS_HCVSE_E	int,
					HCVS_EVSSHSIVSS_E	int,
					CIVSS_EVSSHSIVSS_E	int,
					CIVSS_EVSSHSIVSS_HCVS_E	int,
					E_Others int,
					E_Total	int,

					HCVS_P	int,
					CIVSS_P	int,
					EVSSHSIVSS_P	int,
					HSIVSS_P	int,
					RVP_P	int,
					HCVS_CIVSS_P	int,
					HCVS_EVSSHSIVSS_P	int,
					HCVS_HSIVSS_P	int,
					HCVS_RVP_P	int,
					CIVSS_EVSSHSIVSS_P	int,
					CIVSS_HSIVSS_P	int,
					CIVSS_RVP_P	int,
					EVSSHSIVSS_HSIVSS_P	int,
					EVSSHSIVSS_RVP_P	int,
					HSIVSS_RVP_P	int,
					CIVSS_EVSSHSIVSS_HCVS_P	int,
					CIVSS_HCVS_HSIVSS_P	int,
					CIVSS_HCVS_RVP_P	int,
					EVSSHSIVSS_HCVS_HSIVSS_P	int,
					EVSSHSIVSS_HCVS_RVP_P	int,
					HCVS_HSIVSS_RVP_P	int,
					CIVSS_EVSSHSIVSS_HSIVSS_P	int,
					CIVSS_EVSSHSIVSS_RVP_P	int,
					CIVSS_HSIVSS_RVP_P	int,
					EVSSHSIVSS_HSIVSS_RVP_P	int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P	int,
					CIVSS_EVSSHSIVSS_HCVS_RVP_P	int,
					CIVSS_HCVS_HSIVSS_RVP_P int,
					EVSSHSIVSS_HCVS_HSIVSS_RVP_P int,
					CIVSS_EVSSHSIVSS_HSIVSS_RVP_P	int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P	int,
					None_P int,
					P_Others int,
					P_Total	int,

					MO_0	int,
					MO_1	int,
					MO_2	int,
					MO_3	int,
					MO_4	int,
					MO_5	int,
					MO_6	int,
					MO_A6	int,
					MO_Total	int,
					Solo	int,
					Partenship	int,
					Shareholder	int,
					Director	int,
					Employee	int,
					Others	int,
					MO_Rel_Total	int,

					Practice_0	int,
					Practice_1	int,
					Practice_2	int,
					Practice_3	int,
					Practice_4	int,
					Practice_5	int,
					Practice_6	int,
					Practice_A6	int,
					Practice_Total	int,

					ENU	int,
					RCM	int,
					RCP	int,
					RDT	int,
					RMP	int,
					RMT	int,
					RNU	int,
					ROP int,				-- CRE11-024-01: added
					ROT	int,
					RPT	int,
					RRD	int,
					Prof_Total	int,

					App_ENU	int,
					App_RCM	int,
					App_RCP	int,
					App_RDT	int,
					App_RMP	int,
					App_RMT	int,
					App_RNU	int,
					App_ROP int,			-- CRE11-024-01: added
					App_ROT	int,
					App_RPT	int,
					App_RRD	int,
					Approved_Account	int,

					Act_ENU	int,
					Act_RCM	int,
					Act_RCP	int,
					Act_RDT	int,
					Act_RMP	int,
					Act_RMT	int,
					Act_RNU	int,
					Act_ROP int,			-- CRE11-024-01: added
					Act_ROT	int,
					Act_RPT	int,
					Act_RRD	int,
					Activated_Account	int,

					DEAcct0	int,
					DEAcct1	int,
					DEAcct2	int,
					DEAcct3	int,
					DEAcct4	int,
					DEAcct5	int,
					DEAcct6	int,
					DEAcct7	int,
					DEAcct8	int,
					DEAcct9	int,
					DEAcct10	int,
					DEAcct11	int,
					DEAcct12	int,
					DEAcct13	int,
					DEAcct14	int,
					DEAcct15	int,
					DEAcct16	int,
					DEAcct17	int,
					DEAcct18	int,
					DEAcct19	int,
					DEAcct20	int,
					DEAcct21	int,
					DEAcct22	int,
					DEAcct23	int,
					DEAcct24	int,
					DEAcct25	int,
					DEAcct26	int,
					DEAcct27	int,
					DEAcct28	int,
					DEAcct29	int,
					DEAcct30	int,
					DelistSP_NA	int,
					DelistSP_A	int)
					
					
Declare @Stat2 table (Dtm varchar(10),
					HCVS_A	int,
					CIVSS_A	int,
					EVSSHSIVSS_A	int,
					HSIVSS_A int,
					RVP_A	int,
					HCVS_CIVSS_A	int,
					HCVS_EVSSHSIVSS_A	int,
					HCVS_HSIVSS_A int,
					HCVS_RVP_A	int,
					CIVSS_EVSSHSIVSS_A	int,
					CIVSS_HSIVSS_A int,
					CIVSS_RVP_A	int,
					EVSSHSIVSS_HSIVSS_A int,
					EVSSHSIVSS_RVP_A	int,
					HSIVSS_RVP_A int,
					CIVSS_EVSSHSIVSS_HCVS_A	int,
					CIVSS_HCVS_HSIVSS_A int,
					CIVSS_HCVS_RVP_A	int,
					EVSSHSIVSS_HCVS_HSIVSS_A int,
					EVSSHSIVSS_HCVS_RVP_A	int,
					HCVS_HSIVSS_RVP_A int,
					CIVSS_EVSSHSIVSS_HSIVSS_A int,
					CIVSS_EVSSHSIVSS_RVP_A	int,
					CIVSS_HSIVSS_RVP_A int,
					EVSSHSIVSS_HSIVSS_RVP_A int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_A int,
					CIVSS_EVSSHSIVSS_HCVS_RVP_A	int,
					CIVSS_HCVS_HSIVSS_RVP_A int,
					EVSSHSIVSS_HCVS_HSIVSS_RVP_A int,
					CIVSS_EVSSHSIVSS_HSIVSS_RVP_A int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_A int,
					Delist_Total_A int,					
					A_Others int,
					A_Total	int)
					
Declare @Stat3 table (Dtm varchar(10),
					HCVS_AC	int,
					CIVSS_AC	int,
					EVSSHSIVSS_AC	int,
					HSIVSS_AC int,
					RVP_AC	int,
					HCVS_CIVSS_AC	int,
					HCVS_EVSSHSIVSS_AC	int,
					HCVS_HSIVSS_AC int,
					HCVS_RVP_AC	int,
					CIVSS_EVSSHSIVSS_AC	int,
					CIVSS_HSIVSS_AC int,
					CIVSS_RVP_AC	int,
					EVSSHSIVSS_HSIVSS_AC int,
					EVSSHSIVSS_RVP_AC	int,
					HSIVSS_RVP_AC int,
					CIVSS_EVSSHSIVSS_HCVS_AC	int,
					CIVSS_HCVS_HSIVSS_AC int,
					CIVSS_HCVS_RVP_AC	int,
					EVSSHSIVSS_HCVS_HSIVSS_AC int,
					EVSSHSIVSS_HCVS_RVP_AC	int,
					HCVS_HSIVSS_RVP_AC int,
					CIVSS_EVSSHSIVSS_HSIVSS_AC int,
					CIVSS_EVSSHSIVSS_RVP_AC	int,
					CIVSS_HSIVSS_RVP_AC int,
					EVSSHSIVSS_HSIVSS_RVP_AC int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC int,
					CIVSS_EVSSHSIVSS_HCVS_RVP_AC	int,
					CIVSS_HCVS_HSIVSS_RVP_AC int,
					EVSSHSIVSS_HCVS_HSIVSS_RVP_AC int,
					CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC int,
					Delist_Total_AC int,
					AC_Others	int,
					AC_Total	int)

Declare @Stat4 table (Dtm varchar(10),
					HCVS_RMP_A	int,
					CIVSS_RMP_A	int,
					EVSSHSIVSS_RMP_A	int,
					HSIVSS_RMP_A int,
					RVP_RMP_A	int,
					HCVS_CIVSS_RMP_A	int,
					HCVS_EVSSHSIVSS_RMP_A	int,
					HCVS_HSIVSS_RMP_A int,
					HCVS_RVP_RMP_A	int,
					CIVSS_EVSSHSIVSS_RMP_A	int,
					CIVSS_HSIVSS_RMP_A int,
					CIVSS_RVP_RMP_A	int,
					EVSSHSIVSS_HSIVSS_RMP_A int,
					EVSSHSIVSS_RVP_RMP_A	int,
					HSIVSS_RVP_RMP_A int,
					CIVSS_EVSSHSIVSS_HCVS_RMP_A	int,
					CIVSS_HCVS_HSIVSS_RMP_A int,
					CIVSS_HCVS_RVP_RMP_A	int,
					EVSSHSIVSS_HCVS_HSIVSS_RMP_A int,
					EVSSHSIVSS_HCVS_RVP_RMP_A	int,
					HCVS_HSIVSS_RVP_RMP_A int,
					CIVSS_EVSSHSIVSS_HSIVSS_RMP_A int,
					CIVSS_EVSSHSIVSS_RVP_RMP_A	int,
					CIVSS_HSIVSS_RVP_RMP_A int,
					EVSSHSIVSS_HSIVSS_RVP_RMP_A int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A int,
					CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A	int,
					CIVSS_HCVS_HSIVSS_RVP_RMP_A int,
					EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A int,
					CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A int,
					Delist_Total_RMP_A int,
					A_RMP_Others int, 
					A_RMP_Total	int)
										
Declare @Stat5 table (Dtm varchar(10),
					HCVS_RMP_AC	int,
					CIVSS_RMP_AC	int,
					EVSSHSIVSS_RMP_AC	int,
					HSIVSS_RMP_AC int,
					RVP_RMP_AC	int,
					HCVS_CIVSS_RMP_AC	int,
					HCVS_EVSSHSIVSS_RMP_AC	int,
					HCVS_HSIVSS_RMP_AC int,
					HCVS_RVP_RMP_AC	int,
					CIVSS_EVSSHSIVSS_RMP_AC	int,
					CIVSS_HSIVSS_RMP_AC int,
					CIVSS_RVP_RMP_AC	int,
					EVSSHSIVSS_HSIVSS_RMP_AC int,
					EVSSHSIVSS_RVP_RMP_AC	int,
					HSIVSS_RVP_RMP_AC int,
					CIVSS_EVSSHSIVSS_HCVS_RMP_AC	int,
					CIVSS_HCVS_HSIVSS_RMP_AC int,
					CIVSS_HCVS_RVP_RMP_AC	int,
					EVSSHSIVSS_HCVS_HSIVSS_RMP_AC int,
					EVSSHSIVSS_HCVS_RVP_RMP_AC	int,
					HCVS_HSIVSS_RVP_RMP_AC int,
					CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC int,
					CIVSS_EVSSHSIVSS_RVP_RMP_AC	int,
					CIVSS_HSIVSS_RVP_RMP_AC int,
					EVSSHSIVSS_HSIVSS_RVP_RMP_AC int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC int,
					CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC	int,
					CIVSS_HCVS_HSIVSS_RVP_RMP_AC int,
					EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC int,
					CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC int,
					CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC int,
					Delist_Total_RMP_AC int,
					AC_RMP_Others	int,
					AC_RMP_Total	int)
										
-- Insert Date by Date
select @temp_date = @start_date
while datediff(d, @temp_date, @end_date) > 0
BEGIN

select @HCVS_E  = 0
select @CIVSS_E  = 0
select @EVSSHSIVSS_E = 0
select @CIVSS_HCVSE_E = 0
select @HCVS_EVSSHSIVSS_E = 0
select @CIVSS_EVSSHSIVSS_E = 0
select @CIVSS_EVSSHSIVSS_HCVS_E = 0
select @E_Total = 0
select @E_Others = 0

select @HCVS_P	= 0
select @CIVSS_P	= 0
select @EVSSHSIVSS_P	= 0
select @HSIVSS_P	= 0
select @RVP_P	= 0
select @HCVS_CIVSS_P	= 0
select @HCVS_EVSSHSIVSS_P	= 0
select @HCVS_HSIVSS_P	= 0
select @HCVS_RVP_P	= 0
select @CIVSS_EVSSHSIVSS_P	= 0
select @CIVSS_HSIVSS_P	= 0
select @CIVSS_RVP_P	= 0
select @EVSSHSIVSS_HSIVSS_P	= 0
select @EVSSHSIVSS_RVP_P	= 0
select @HSIVSS_RVP_P	= 0
select @CIVSS_EVSSHSIVSS_HCVS_P	= 0
select @CIVSS_HCVS_HSIVSS_P	= 0
select @CIVSS_HCVS_RVP_P	= 0
select @EVSSHSIVSS_HCVS_HSIVSS_P	= 0
select @EVSSHSIVSS_HCVS_RVP_P	= 0
select @HCVS_HSIVSS_RVP_P	= 0
select @CIVSS_EVSSHSIVSS_HSIVSS_P	= 0
select @CIVSS_EVSSHSIVSS_RVP_P	= 0
select @CIVSS_HSIVSS_RVP_P	= 0
select @EVSSHSIVSS_HSIVSS_RVP_P	= 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P	= 0
select @CIVSS_EVSSHSIVSS_HCVS_RVP_P	= 0
select @CIVSS_HCVS_HSIVSS_RVP_P		= 0
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_P	= 0
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_P	= 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P	= 0
select @None_P = 0
select @P_Total = 0
select @P_Others = 0

select @MO_0 = 0
select @MO_1 = 0
select @MO_2 = 0
select @MO_3 = 0
select @MO_4 = 0
select @MO_5 = 0
select @MO_6 = 0
select @MO_A6 = 0
select @MO_Total = 0
select @Solo = 0
select @Partenship = 0
select @Shareholder = 0
select @Director = 0
select @Employee = 0
select @Others = 0
select @MO_Rel_Total = 0

select @Practice_0 = 0
select @Practice_1 = 0
select @Practice_2 = 0
select @Practice_3 = 0
select @Practice_4 = 0
select @Practice_5 = 0
select @Practice_6 = 0
select @Practice_A6 = 0
select @Practice_Total = 0

select @ENU = 0
select @RCM = 0
select @RCP = 0
select @RDT = 0
select @RMP = 0
select @RMT = 0
select @RNU = 0
select @ROP = 0				-- CRE11-024-01: added
select @ROT = 0
select @RPT = 0
select @RRD = 0

select @Prof_Total = 0
select @App_ENU = 0
select @App_RCM = 0
select @App_RCP = 0
select @App_RDT = 0
select @App_RMP = 0
select @App_RMT = 0
select @App_RNU = 0
select @App_ROP	= 0			-- CRE11-024-01: added
select @App_ROT = 0
select @App_RPT = 0
select @App_RRD = 0
select @Approved_Account = 0

select @Act_ENU = 0
select @Act_RCM = 0
select @Act_RCP = 0
select @Act_RDT = 0
select @Act_RMP = 0
select @Act_RMT = 0
select @Act_RNU = 0
select @Act_ROP = 0			-- CRE11-024-01: added
select @Act_ROT = 0
select @Act_RPT = 0
select @Act_RRD = 0
select @Activated_Account = 0

select @DEAcct0 = 0
select @DEAcct1 = 0
select @DEAcct2 = 0
select @DEAcct3 = 0
select @DEAcct4 = 0
select @DEAcct5 = 0
select @DEAcct6 = 0
select @DEAcct7 = 0
select @DEAcct8 = 0
select @DEAcct9 = 0
select @DEAcct10 = 0
select @DEAcct11 = 0
select @DEAcct12 = 0
select @DEAcct13 = 0
select @DEAcct14 = 0
select @DEAcct15 = 0
select @DEAcct16 = 0
select @DEAcct17 = 0
select @DEAcct18 = 0
select @DEAcct19 = 0
select @DEAcct20 = 0
select @DEAcct21 = 0
select @DEAcct22 = 0
select @DEAcct23 = 0
select @DEAcct24 = 0
select @DEAcct25 = 0
select @DEAcct26 = 0
select @DEAcct27 = 0
select @DEAcct28 = 0
select @DEAcct29 = 0
select @DEAcct30 = 0
select @DelistSP_NA = 0
select @DelistSP_A = 0

select @HCVS_A	= 0
select @CIVSS_A	= 0
select @EVSSHSIVSS_A	= 0
select @HSIVSS_A = 0
select @RVP_A	= 0
select @HCVS_CIVSS_A	= 0
select @HCVS_EVSSHSIVSS_A	= 0
select @HCVS_HSIVSS_A = 0
select @HCVS_RVP_A	= 0
select @CIVSS_EVSSHSIVSS_A	= 0
select @CIVSS_HSIVSS_A = 0
select @CIVSS_RVP_A	= 0
select @EVSSHSIVSS_HSIVSS_A = 0
select @EVSSHSIVSS_RVP_A	= 0
select @HSIVSS_RVP_A = 0
select @CIVSS_EVSSHSIVSS_HCVS_A	= 0
select @CIVSS_HCVS_HSIVSS_A = 0
select @CIVSS_HCVS_RVP_A	= 0
select @EVSSHSIVSS_HCVS_HSIVSS_A = 0
select @EVSSHSIVSS_HCVS_RVP_A	= 0
select @HCVS_HSIVSS_RVP_A = 0
select @CIVSS_EVSSHSIVSS_HSIVSS_A = 0
select @CIVSS_EVSSHSIVSS_RVP_A	= 0
select @CIVSS_HSIVSS_RVP_A = 0
select @EVSSHSIVSS_HSIVSS_RVP_A = 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_A = 0
select @CIVSS_EVSSHSIVSS_HCVS_RVP_A	= 0
select @CIVSS_HCVS_HSIVSS_RVP_A = 0
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_A = 0
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_A = 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_A = 0
select @Deliat_Total_A= 0
select @A_Total= 0
select @A_Others = 0

select @HCVS_AC	= 0
select @CIVSS_AC	= 0
select @EVSSHSIVSS_AC	= 0
select @HSIVSS_AC = 0
select @RVP_AC	= 0
select @HCVS_CIVSS_AC	= 0
select @HCVS_EVSSHSIVSS_AC	= 0
select @HCVS_HSIVSS_AC = 0
select @HCVS_RVP_AC	= 0
select @CIVSS_EVSSHSIVSS_AC	= 0
select @CIVSS_HSIVSS_AC = 0
select @CIVSS_RVP_AC	= 0
select @EVSSHSIVSS_HSIVSS_AC = 0
select @EVSSHSIVSS_RVP_AC	= 0
select @HSIVSS_RVP_AC = 0
select @CIVSS_EVSSHSIVSS_HCVS_AC	= 0
select @CIVSS_HCVS_HSIVSS_AC = 0
select @CIVSS_HCVS_RVP_AC	= 0
select @EVSSHSIVSS_HCVS_HSIVSS_AC = 0
select @EVSSHSIVSS_HCVS_RVP_AC	= 0
select @HCVS_HSIVSS_RVP_AC = 0
select @CIVSS_EVSSHSIVSS_HSIVSS_AC = 0
select @CIVSS_EVSSHSIVSS_RVP_AC	= 0
select @CIVSS_HSIVSS_RVP_AC = 0
select @EVSSHSIVSS_HSIVSS_RVP_AC = 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC = 0
select @CIVSS_EVSSHSIVSS_HCVS_RVP_AC	= 0
select @CIVSS_HCVS_HSIVSS_RVP_AC = 0
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_AC = 0
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC = 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC = 0
select @Deliat_Total_AC = 0
select @AC_Total= 0
select @AC_Others = 0

select @HCVS_RMP_A	= 0
select @CIVSS_RMP_A	= 0
select @EVSSHSIVSS_RMP_A	= 0
select @HSIVSS_RMP_A = 0
select @RVP_RMP_A	= 0
select @HCVS_CIVSS_RMP_A	= 0
select @HCVS_EVSSHSIVSS_RMP_A	= 0
select @HCVS_HSIVSS_RMP_A = 0
select @HCVS_RVP_RMP_A	= 0
select @CIVSS_EVSSHSIVSS_RMP_A	= 0
select @CIVSS_HSIVSS_RMP_A = 0
select @CIVSS_RVP_RMP_A	= 0
select @EVSSHSIVSS_HSIVSS_RMP_A = 0
select @EVSSHSIVSS_RVP_RMP_A	= 0
select @HSIVSS_RVP_RMP_A = 0
select @CIVSS_EVSSHSIVSS_HCVS_RMP_A	= 0
select @CIVSS_HCVS_HSIVSS_RMP_A = 0
select @CIVSS_HCVS_RVP_RMP_A	= 0
select @EVSSHSIVSS_HCVS_HSIVSS_RMP_A = 0
select @EVSSHSIVSS_HCVS_RVP_RMP_A	= 0
select @HCVS_HSIVSS_RVP_RMP_A = 0
select @CIVSS_EVSSHSIVSS_HSIVSS_RMP_A = 0
select @CIVSS_EVSSHSIVSS_RVP_RMP_A	= 0
select @CIVSS_HSIVSS_RVP_RMP_A = 0
select @EVSSHSIVSS_HSIVSS_RVP_RMP_A = 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A = 0
select @CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A	= 0
select @CIVSS_HCVS_HSIVSS_RVP_RMP_A = 0
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A = 0
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A = 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A = 0
select @Deliat_Total_RMP_A= 0
select @A_RMP_Total= 0
select @A_RMP_Others = 0

select @HCVS_RMP_AC	= 0
select @CIVSS_RMP_AC	= 0
select @EVSSHSIVSS_RMP_AC	= 0
select @HSIVSS_RMP_AC = 0
select @RVP_RMP_AC	= 0
select @HCVS_CIVSS_RMP_AC	= 0
select @HCVS_EVSSHSIVSS_RMP_AC	= 0
select @HCVS_HSIVSS_RMP_AC = 0
select @HCVS_RVP_RMP_AC	= 0
select @CIVSS_EVSSHSIVSS_RMP_AC	= 0
select @CIVSS_HSIVSS_RMP_AC = 0
select @CIVSS_RVP_RMP_AC	= 0
select @EVSSHSIVSS_HSIVSS_RMP_AC = 0
select @EVSSHSIVSS_RVP_RMP_AC	= 0
select @HSIVSS_RVP_RMP_AC = 0
select @CIVSS_EVSSHSIVSS_HCVS_RMP_AC	= 0
select @CIVSS_HCVS_HSIVSS_RMP_AC = 0
select @CIVSS_HCVS_RVP_RMP_AC	= 0
select @EVSSHSIVSS_HCVS_HSIVSS_RMP_AC = 0
select @EVSSHSIVSS_HCVS_RVP_RMP_AC	= 0
select @HCVS_HSIVSS_RVP_RMP_AC = 0
select @CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC = 0
select @CIVSS_EVSSHSIVSS_RVP_RMP_AC	= 0
select @CIVSS_HSIVSS_RVP_RMP_AC = 0
select @EVSSHSIVSS_HSIVSS_RVP_RMP_AC = 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC = 0
select @CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC	= 0
select @CIVSS_HCVS_HSIVSS_RVP_RMP_AC = 0
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC = 0
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC = 0
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC = 0
select @Deliat_Total_RMP_AC = 0
select @AC_RMP_Total= 0
select @AC_RMP_Others = 0


select @HCVS_E = StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS_E'
select @CIVSS_E = StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS_E'
select @EVSSHSIVSS_E= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS_E'
select @CIVSS_HCVSE_E= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVSE_E'
select @HCVS_EVSSHSIVSS_E= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+EVSSHSIVSS_E'
select @CIVSS_EVSSHSIVSS_E= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS_E'
select @CIVSS_EVSSHSIVSS_HCVS_E= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS_E'
select @E_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'E_Total'
SET @E_Others = @E_Total - (
					@HCVS_E + @CIVSS_E + @EVSSHSIVSS_E + 
					@CIVSS_HCVSE_E + @HCVS_EVSSHSIVSS_E + @CIVSS_EVSSHSIVSS_E + @CIVSS_EVSSHSIVSS_HCVS_E)

select @HCVS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS_P'
select @CIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS_P'
select @EVSSHSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS_P'
select @HSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS_P'
select @RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RVP_P'
select @HCVS_CIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+CIVSS_P'
select @HCVS_EVSSHSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+EVSSHSIVSS_P'
select @HCVS_HSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS_P'
select @HCVS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+RVP_P'
select @CIVSS_EVSSHSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS_P'
select @CIVSS_HSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS_P'
select @CIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+RVP_P'
select @EVSSHSIVSS_HSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS_P'
select @EVSSHSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+RVP_P'
select @HSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS+RVP_P'
select @CIVSS_EVSSHSIVSS_HCVS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS_P'
select @CIVSS_HCVS_HSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS_P'
select @CIVSS_HCVS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+RVP_P'
select @EVSSHSIVSS_HCVS_HSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS_P'
select @EVSSHSIVSS_HCVS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+RVP_P'
select @HCVS_HSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS+RVP_P'
select @CIVSS_EVSSHSIVSS_HSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS_P'
select @CIVSS_EVSSHSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+RVP_P'
select @CIVSS_HSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS+RVP_P'
select @EVSSHSIVSS_HSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS+RVP_P'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_P'
select @CIVSS_EVSSHSIVSS_HCVS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+RVP_P'
select @CIVSS_HCVS_HSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS+RVP_P'
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS+RVP_P'
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_P'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_P'
select @None_P= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'None_P'
select @P_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'P_Total'
SET	@P_Others = @P_Total - (
					@HCVS_P + @CIVSS_P + @EVSSHSIVSS_P + @HSIVSS_P + @RVP_P + @HCVS_CIVSS_P + @HCVS_EVSSHSIVSS_P + 
					@HCVS_HSIVSS_P + @HCVS_RVP_P + @CIVSS_EVSSHSIVSS_P + @CIVSS_HSIVSS_P + @CIVSS_RVP_P + @EVSSHSIVSS_HSIVSS_P + 
					@EVSSHSIVSS_RVP_P + @HSIVSS_RVP_P + @CIVSS_EVSSHSIVSS_HCVS_P + @CIVSS_HCVS_HSIVSS_P + @CIVSS_HCVS_RVP_P + 
					@EVSSHSIVSS_HCVS_HSIVSS_P + @EVSSHSIVSS_HCVS_RVP_P + @HCVS_HSIVSS_RVP_P + @CIVSS_EVSSHSIVSS_HSIVSS_P + @CIVSS_EVSSHSIVSS_RVP_P + 
					@CIVSS_HSIVSS_RVP_P + @EVSSHSIVSS_HSIVSS_RVP_P + @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P + @CIVSS_EVSSHSIVSS_HCVS_RVP_P + @CIVSS_HCVS_HSIVSS_RVP_P + 
					@EVSSHSIVSS_HCVS_HSIVSS_RVP_P + @CIVSS_EVSSHSIVSS_HSIVSS_RVP_P + @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P + @None_P)

select @MO_0= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_0'
select @MO_1= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_1'
select @MO_2= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_2'
select @MO_3= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_3'
select @MO_4= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_4'
select @MO_5= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_5'
select @MO_6= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_6'
select @MO_A6= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_A6'
select @MO_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_Total'
select @Solo= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Solo'
select @Partenship= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Partenship'
select @Shareholder= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Shareholder'
select @Director= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Director'
select @Employee= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Employee'
select @Others= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Others'
select @MO_Rel_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'MO_Rel_Total'

select @Practice_0= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_0'
select @Practice_1= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_1'
select @Practice_2= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_2'
select @Practice_3= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_3'
select @Practice_4= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_4'
select @Practice_5= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_5'
select @Practice_6= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_6'
select @Practice_A6= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_A6'
select @Practice_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Practice_Total'

select @ENU= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'ENU'
select @RCM= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RCM'
select @RCP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RCP'
select @RDT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RDT'
select @RMP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RMP'
select @RMT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RMT'
select @RNU= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RNU'
select @ROP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'ROP'	-- CRE11-024-01: added
select @ROT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'ROT'
select @RPT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RPT'
select @RRD= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RRD'

select @Prof_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Prof_Total'
select @App_ENU= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_ENU'
select @App_RCM= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_RCM'
select @App_RCP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_RCP'
select @App_RDT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_RDT'
select @App_RMP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_RMP'
select @App_RMT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_RMT'
select @App_RNU= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_RNU'
select @App_ROP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_ROP'	-- CRE11-024-01: added
select @App_ROT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_ROT'
select @App_RPT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_RPT'
select @App_RRD= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'App_RRD'
select @Approved_Account= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'ApprovedAccount'

select @Act_ENU= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_ENU'
select @Act_RCM= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_RCM'
select @Act_RCP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_RCP'
select @Act_RDT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_RDT'
select @Act_RMP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_RMP'
select @Act_RMT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_RMT'
select @Act_RNU= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_RNU'
select @Act_ROP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_ROP'	-- CRE11-024-01: added
select @Act_ROT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_ROT'
select @Act_RPT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_RPT'
select @Act_RRD= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Act_RRD'
select @Activated_Account= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'ActivatedAccount'

select @DEAcct0= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct0'
select @DEAcct1= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct1'
select @DEAcct2= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct2'
select @DEAcct3= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct3'
select @DEAcct4= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct4'
select @DEAcct5= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct5'
select @DEAcct6= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct6'
select @DEAcct7= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct7'
select @DEAcct8= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct8'
select @DEAcct9= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct9'
select @DEAcct10= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct10'
select @DEAcct11= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct11'
select @DEAcct12= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct12'
select @DEAcct13= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct13'
select @DEAcct14= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct14'
select @DEAcct15= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct15'
select @DEAcct16= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct16'
select @DEAcct17= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct17'
select @DEAcct18= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct18'
select @DEAcct19= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct19'
select @DEAcct20= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct20'
select @DEAcct21= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct21'
select @DEAcct22= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct22'
select @DEAcct23= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct23'
select @DEAcct24= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct24'
select @DEAcct25= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct25'
select @DEAcct26= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct26'
select @DEAcct27= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct27'
select @DEAcct28= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct28'
select @DEAcct29= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct29'
select @DEAcct30= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DEAcct30'
select @DelistSP_NA= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DelistSP_NA'
select @DelistSP_A= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'DelistSP_A'

select @HCVS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS_A'
select @CIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS_A'
select @EVSSHSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS_A'
select @HSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS_A'
select @RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RVP_A'
select @HCVS_CIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+CIVSS_A'
select @HCVS_EVSSHSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+EVSSHSIVSS_A'
select @HCVS_HSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS_A'
select @HCVS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+RVP_A'
select @CIVSS_EVSSHSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS_A'
select @CIVSS_HSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS_A'
select @CIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+RVP_A'
select @EVSSHSIVSS_HSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS_A'
select @EVSSHSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+RVP_A'
select @HSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS+RVP_A'
select @CIVSS_EVSSHSIVSS_HCVS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS_A'
select @CIVSS_HCVS_HSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS_A'
select @CIVSS_HCVS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+RVP_A'
select @EVSSHSIVSS_HCVS_HSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS_A'
select @EVSSHSIVSS_HCVS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+RVP_A'
select @HCVS_HSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS+RVP_A'
select @CIVSS_EVSSHSIVSS_HSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS_A'
select @CIVSS_EVSSHSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+RVP_A'
select @CIVSS_HSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS+RVP_A'
select @EVSSHSIVSS_HSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS+RVP_A'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_A'
select @CIVSS_EVSSHSIVSS_HCVS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+RVP_A'
select @CIVSS_HCVS_HSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS+RVP_A'
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS+RVP_A'
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_A'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_A'
select @Deliat_Total_A= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Deliat_Total_A'
select @A_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'A_Total'
SET @A_Others = @A_Total - (
					@HCVS_A + @CIVSS_A + @EVSSHSIVSS_A + @HSIVSS_A + @RVP_A + 
					@HCVS_CIVSS_A + @HCVS_EVSSHSIVSS_A + @HCVS_HSIVSS_A + @HCVS_RVP_A + 
					@CIVSS_EVSSHSIVSS_A + @CIVSS_HSIVSS_A + @CIVSS_RVP_A + @EVSSHSIVSS_HSIVSS_A + 
					@EVSSHSIVSS_RVP_A + @HSIVSS_RVP_A + @CIVSS_EVSSHSIVSS_HCVS_A + @CIVSS_HCVS_HSIVSS_A + 
					@CIVSS_HCVS_RVP_A + @EVSSHSIVSS_HCVS_HSIVSS_A + @EVSSHSIVSS_HCVS_RVP_A + @HCVS_HSIVSS_RVP_A + 
					@CIVSS_EVSSHSIVSS_HSIVSS_A + @CIVSS_EVSSHSIVSS_RVP_A + @CIVSS_HSIVSS_RVP_A + @EVSSHSIVSS_HSIVSS_RVP_A + 
					@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_A + @CIVSS_EVSSHSIVSS_HCVS_RVP_A + @CIVSS_HCVS_HSIVSS_RVP_A + @EVSSHSIVSS_HCVS_HSIVSS_RVP_A + 
					@CIVSS_EVSSHSIVSS_HSIVSS_RVP_A + @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_A + @Deliat_Total_A)

select @HCVS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS_AC'
select @CIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS_AC'
select @EVSSHSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS_AC'
select @HSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS_AC'
select @RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RVP_AC'
select @HCVS_CIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+CIVSS_AC'
select @HCVS_EVSSHSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+EVSSHSIVSS_AC'
select @HCVS_HSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS_AC'
select @HCVS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+RVP_AC'
select @CIVSS_EVSSHSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS_AC'
select @CIVSS_HSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS_AC'
select @CIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+RVP_AC'
select @EVSSHSIVSS_HSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS_AC'
select @EVSSHSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+RVP_AC'
select @HSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS+RVP_AC'
select @CIVSS_EVSSHSIVSS_HCVS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS_AC'
select @CIVSS_HCVS_HSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS_AC'
select @CIVSS_HCVS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+RVP_AC'
select @EVSSHSIVSS_HCVS_HSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS_AC'
select @EVSSHSIVSS_HCVS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+RVP_AC'
select @HCVS_HSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS+RVP_AC'
select @CIVSS_EVSSHSIVSS_HSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS_AC'
select @CIVSS_EVSSHSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+RVP_AC'
select @CIVSS_HSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS+RVP_AC'
select @EVSSHSIVSS_HSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS+RVP_AC'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_AC'
select @CIVSS_EVSSHSIVSS_HCVS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+RVP_AC'
select @CIVSS_HCVS_HSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS+RVP_AC'
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS+RVP_AC'
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_AC'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_AC'
select @Deliat_Total_AC= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Deliat_Total_AC'
select @AC_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'AC_Total'
SET @AC_Others = @AC_Total - (
					@HCVS_AC + @CIVSS_AC + @EVSSHSIVSS_AC + @HSIVSS_AC + @RVP_AC + 
					@HCVS_CIVSS_AC + @HCVS_EVSSHSIVSS_AC + @HCVS_HSIVSS_AC + @HCVS_RVP_AC + 
					@CIVSS_EVSSHSIVSS_AC + @CIVSS_HSIVSS_AC + @CIVSS_RVP_AC + @EVSSHSIVSS_HSIVSS_AC + 
					@EVSSHSIVSS_RVP_AC + @HSIVSS_RVP_AC + @CIVSS_EVSSHSIVSS_HCVS_AC + @CIVSS_HCVS_HSIVSS_AC + 
					@CIVSS_HCVS_RVP_AC + @EVSSHSIVSS_HCVS_HSIVSS_AC + @EVSSHSIVSS_HCVS_RVP_AC + @HCVS_HSIVSS_RVP_AC + 
					@CIVSS_EVSSHSIVSS_HSIVSS_AC + @CIVSS_EVSSHSIVSS_RVP_AC + @CIVSS_HSIVSS_RVP_AC + @EVSSHSIVSS_HSIVSS_RVP_AC + 
					@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC + @CIVSS_EVSSHSIVSS_HCVS_RVP_AC + @CIVSS_HCVS_HSIVSS_RVP_AC + @EVSSHSIVSS_HCVS_HSIVSS_RVP_AC + 
					@CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC + @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC + @Deliat_Total_AC)


select @HCVS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS_RMP_A'
select @CIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS_RMP_A'
select @EVSSHSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS_RMP_A'
select @HSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS_RMP_A'
select @RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RVP_RMP_A'
select @HCVS_CIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+CIVSS_RMP_A'
select @HCVS_EVSSHSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+EVSSHSIVSS_RMP_A'
select @HCVS_HSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS_RMP_A'
select @HCVS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+RVP_RMP_A'
select @CIVSS_EVSSHSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS_RMP_A'
select @CIVSS_HSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS_RMP_A'
select @CIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+RVP_RMP_A'
select @EVSSHSIVSS_HSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS_RMP_A'
select @EVSSHSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+RVP_RMP_A'
select @HSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS+RVP_RMP_A'
select @CIVSS_EVSSHSIVSS_HCVS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS_RMP_A'
select @CIVSS_HCVS_HSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS_RMP_A'
select @CIVSS_HCVS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+RVP_RMP_A'
select @EVSSHSIVSS_HCVS_HSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS_RMP_A'
select @EVSSHSIVSS_HCVS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+RVP_RMP_A'
select @HCVS_HSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS+RVP_RMP_A'
select @CIVSS_EVSSHSIVSS_HSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS_RMP_A'
select @CIVSS_EVSSHSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+RVP_RMP_A'
select @CIVSS_HSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS+RVP_RMP_A'
select @EVSSHSIVSS_HSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS+RVP_RMP_A'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_RMP_A'
select @CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+RVP_RMP_A'
select @CIVSS_HCVS_HSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS+RVP_RMP_A'
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS+RVP_RMP_A'
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_RMP_A'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_RMP_A'
select @Deliat_Total_RMP_A= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Deliat_Total_RMP_A'
select @A_RMP_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'A_RMP_Total'
SET @A_RMP_Others = @A_RMP_Total - (
						@HCVS_RMP_A + @CIVSS_RMP_A + @EVSSHSIVSS_RMP_A + @HSIVSS_RMP_A + @RVP_RMP_A + 
						@HCVS_CIVSS_RMP_A + @HCVS_EVSSHSIVSS_RMP_A + @HCVS_HSIVSS_RMP_A + @HCVS_RVP_RMP_A + 
						@CIVSS_EVSSHSIVSS_RMP_A + @CIVSS_HSIVSS_RMP_A + @CIVSS_RVP_RMP_A + @EVSSHSIVSS_HSIVSS_RMP_A + 
						@EVSSHSIVSS_RVP_RMP_A + @HSIVSS_RVP_RMP_A + @CIVSS_EVSSHSIVSS_HCVS_RMP_A + @CIVSS_HCVS_HSIVSS_RMP_A + 
						@CIVSS_HCVS_RVP_RMP_A + @EVSSHSIVSS_HCVS_HSIVSS_RMP_A + @EVSSHSIVSS_HCVS_RVP_RMP_A + @HCVS_HSIVSS_RVP_RMP_A + 
						@CIVSS_EVSSHSIVSS_HSIVSS_RMP_A + @CIVSS_EVSSHSIVSS_RVP_RMP_A + @CIVSS_HSIVSS_RVP_RMP_A + @EVSSHSIVSS_HSIVSS_RVP_RMP_A + 
						@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A + @CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A + @CIVSS_HCVS_HSIVSS_RVP_RMP_A + @EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A + 
						@CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A + @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A + @Deliat_Total_RMP_A)

select @HCVS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS_RMP_AC'
select @CIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS_RMP_AC'
select @EVSSHSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS_RMP_AC'
select @HSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS_RMP_AC'
select @RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RVP_RMP_AC'
select @HCVS_CIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+CIVSS_RMP_AC'
select @HCVS_EVSSHSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+EVSSHSIVSS_RMP_AC'
select @HCVS_HSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS_RMP_AC'
select @HCVS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+RVP_RMP_AC'
select @CIVSS_EVSSHSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS_RMP_AC'
select @CIVSS_HSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS_RMP_AC'
select @CIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+RVP_RMP_AC'
select @EVSSHSIVSS_HSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS_RMP_AC'
select @EVSSHSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+RVP_RMP_AC'
select @HSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HSIVSS+RVP_RMP_AC'
select @CIVSS_EVSSHSIVSS_HCVS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS_RMP_AC'
select @CIVSS_HCVS_HSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS_RMP_AC'
select @CIVSS_HCVS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+RVP_RMP_AC'
select @EVSSHSIVSS_HCVS_HSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS_RMP_AC'
select @EVSSHSIVSS_HCVS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+RVP_RMP_AC'
select @HCVS_HSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'HCVS+HSIVSS+RVP_RMP_AC'
select @CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS_RMP_AC'
select @CIVSS_EVSSHSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+RVP_RMP_AC'
select @CIVSS_HSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HSIVSS+RVP_RMP_AC'
select @EVSSHSIVSS_HSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HSIVSS+RVP_RMP_AC'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS_RMP_AC'
select @CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+RVP_RMP_AC'
select @CIVSS_HCVS_HSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+HCVS+HSIVSS+RVP_RMP_AC'
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'EVSSHSIVSS+HCVS+HSIVSS+RVP_RMP_AC'
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HSIVSS+RVP_RMP_AC'
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC	= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'CIVSS+EVSSHSIVSS+HCVS+HSIVSS+RVP_RMP_AC'
select @Deliat_Total_RMP_AC= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Deliat_Total_RMP_AC'
select @AC_RMP_Total= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'AC_RMP_Total'
SET @AC_RMP_Others = @AC_RMP_Total - (
						@HCVS_RMP_AC + @CIVSS_RMP_AC + @EVSSHSIVSS_RMP_AC + @HSIVSS_RMP_AC + @RVP_RMP_AC + 
						@HCVS_CIVSS_RMP_AC + @HCVS_EVSSHSIVSS_RMP_AC + @HCVS_HSIVSS_RMP_AC + @HCVS_RVP_RMP_AC + 
						@CIVSS_EVSSHSIVSS_RMP_AC + @CIVSS_HSIVSS_RMP_AC + @CIVSS_RVP_RMP_AC + @EVSSHSIVSS_HSIVSS_RMP_AC + 
						@EVSSHSIVSS_RVP_RMP_AC + @HSIVSS_RVP_RMP_AC + @CIVSS_EVSSHSIVSS_HCVS_RMP_AC + @CIVSS_HCVS_HSIVSS_RMP_AC + 
						@CIVSS_HCVS_RVP_RMP_AC + @EVSSHSIVSS_HCVS_HSIVSS_RMP_AC + @EVSSHSIVSS_HCVS_RVP_RMP_AC + @HCVS_HSIVSS_RVP_RMP_AC + 
						@CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC + @CIVSS_EVSSHSIVSS_RVP_RMP_AC + @CIVSS_HSIVSS_RVP_RMP_AC + @EVSSHSIVSS_HSIVSS_RVP_RMP_AC + 
						@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC + @CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC + @CIVSS_HCVS_HSIVSS_RVP_RMP_AC + @EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC + 
						@CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC + @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC + @Deliat_Total_RMP_AC)

insert into @Stat
(
	enrolment_date,
	HCVS_E,
	CIVSS_E,
	EVSSHSIVSS_E,
	CIVSS_HCVSE_E,
	HCVS_EVSSHSIVSS_E,
	CIVSS_EVSSHSIVSS_E,
	CIVSS_EVSSHSIVSS_HCVS_E,
	E_Others,
	E_Total,

	HCVS_P,
	CIVSS_P,
	EVSSHSIVSS_P,
	HSIVSS_P,
	RVP_P,
	HCVS_CIVSS_P,
	HCVS_EVSSHSIVSS_P,
	HCVS_HSIVSS_P,
	HCVS_RVP_P,
	CIVSS_EVSSHSIVSS_P,
	CIVSS_HSIVSS_P,
	CIVSS_RVP_P,
	EVSSHSIVSS_HSIVSS_P,
	EVSSHSIVSS_RVP_P,
	HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HCVS_P,
	CIVSS_HCVS_HSIVSS_P,
	CIVSS_HCVS_RVP_P,
	EVSSHSIVSS_HCVS_HSIVSS_P,
	EVSSHSIVSS_HCVS_RVP_P,
	HCVS_HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HSIVSS_P,
	CIVSS_EVSSHSIVSS_RVP_P,
	CIVSS_HSIVSS_RVP_P,
	EVSSHSIVSS_HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P,
	CIVSS_EVSSHSIVSS_HCVS_RVP_P,
	CIVSS_HCVS_HSIVSS_RVP_P,
	EVSSHSIVSS_HCVS_HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P,
	None_P,
	P_Others,
	P_Total,

	MO_0,
	MO_1,
	MO_2,
	MO_3,
	MO_4,
	MO_5,
	MO_6,
	MO_A6,
	MO_Total,
	Solo,
	Partenship,
	Shareholder,
	Director,
	Employee,
	Others,
	MO_Rel_Total,

	Practice_0,
	Practice_1,
	Practice_2,
	Practice_3,
	Practice_4,
	Practice_5,
	Practice_6,
	Practice_A6,
	Practice_Total,

	ENU,
	RCM,
	RCP,
	RDT,
	RMP,
	RMT,
	RNU,
	ROP,			-- CRE11-024-01: added
	ROT,
	RPT,
	RRD,
	Prof_Total,

	App_ENU,
	App_RCM,
	App_RCP,
	App_RDT,
	App_RMP,
	App_RMT,
	App_RNU,
	App_ROP,		-- CRE11-024-01: added
	App_ROT,
	App_RPT,
	App_RRD,
	Approved_Account,

	Act_ENU,
	Act_RCM,
	Act_RCP,
	Act_RDT,
	Act_RMP,
	Act_RMT,
	Act_RNU,
	Act_ROP,		-- CRE11-024-01: added
	Act_ROT,
	Act_RPT,
	Act_RRD,
	Activated_Account,

	DEAcct0,
	DEAcct1,
	DEAcct2,
	DEAcct3,
	DEAcct4,
	DEAcct5,
	DEAcct6,
	DEAcct7,
	DEAcct8,
	DEAcct9,
	DEAcct10,
	DEAcct11,
	DEAcct12,
	DEAcct13,
	DEAcct14,
	DEAcct15,
	DEAcct16,
	DEAcct17,
	DEAcct18,
	DEAcct19,
	DEAcct20,
	DEAcct21,
	DEAcct22,
	DEAcct23,
	DEAcct24,
	DEAcct25,
	DEAcct26,
	DEAcct27,
	DEAcct28,
	DEAcct29,
	DEAcct30,
	DelistSP_NA,
	DelistSP_A
)
values
(
	@temp_date,
	isnull(@HCVS_E , 0),
	isnull(@CIVSS_E , 0),
	isnull(@EVSSHSIVSS_E, 0),
	isnull(@CIVSS_HCVSE_E, 0),
	isnull(@HCVS_EVSSHSIVSS_E, 0),
	isnull(@CIVSS_EVSSHSIVSS_E, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_E, 0),
	isnull(@E_Others, 0),
	isnull(@E_Total, 0),

	isnull(@HCVS_P, 0),
	isnull(@CIVSS_P, 0),
	isnull(@EVSSHSIVSS_P, 0),
	isnull(@HSIVSS_P, 0),
	isnull(@RVP_P, 0),
	isnull(@HCVS_CIVSS_P, 0),
	isnull(@HCVS_EVSSHSIVSS_P, 0),
	isnull(@HCVS_HSIVSS_P, 0),
	isnull(@HCVS_RVP_P, 0),
	isnull(@CIVSS_EVSSHSIVSS_P, 0),
	isnull(@CIVSS_HSIVSS_P, 0),
	isnull(@CIVSS_RVP_P, 0),
	isnull(@EVSSHSIVSS_HSIVSS_P, 0),
	isnull(@EVSSHSIVSS_RVP_P, 0),
	isnull(@HSIVSS_RVP_P, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_P, 0),
	isnull(@CIVSS_HCVS_HSIVSS_P, 0),
	isnull(@CIVSS_HCVS_RVP_P, 0),
	isnull(@EVSSHSIVSS_HCVS_HSIVSS_P, 0),
	isnull(@EVSSHSIVSS_HCVS_RVP_P, 0),
	isnull(@HCVS_HSIVSS_RVP_P, 0),
	isnull(@CIVSS_EVSSHSIVSS_HSIVSS_P, 0),
	isnull(@CIVSS_EVSSHSIVSS_RVP_P, 0),
	isnull(@CIVSS_HSIVSS_RVP_P, 0),
	isnull(@EVSSHSIVSS_HSIVSS_RVP_P, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_RVP_P, 0),
	isnull(@CIVSS_HCVS_HSIVSS_RVP_P, 0),
	isnull(@EVSSHSIVSS_HCVS_HSIVSS_RVP_P, 0),
	isnull(@CIVSS_EVSSHSIVSS_HSIVSS_RVP_P, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P, 0),
	isnull(@None_P, 0),
	isnull(@P_Others, 0),
	isnull(@P_Total, 0),

	isnull(@MO_0, 0),
	isnull(@MO_1, 0),
	isnull(@MO_2, 0),
	isnull(@MO_3, 0),
	isnull(@MO_4, 0),
	isnull(@MO_5, 0),
	isnull(@MO_6, 0),
	isnull(@MO_A6, 0),
	isnull(@MO_Total, 0),
	isnull(@Solo, 0),
	isnull(@Partenship, 0),
	isnull(@Shareholder, 0),
	isnull(@Director, 0),
	isnull(@Employee, 0),
	isnull(@Others, 0),
	isnull(@MO_Rel_Total, 0),

	isnull(@Practice_0, 0),
	isnull(@Practice_1, 0),
	isnull(@Practice_2, 0),
	isnull(@Practice_3, 0),
	isnull(@Practice_4, 0),
	isnull(@Practice_5, 0),
	isnull(@Practice_6, 0),
	isnull(@Practice_A6, 0),
	isnull(@Practice_Total, 0),

	isnull(@ENU, 0),
	isnull(@RCM, 0),
	isnull(@RCP, 0),
	isnull(@RDT, 0),
	isnull(@RMP, 0),
	isnull(@RMT, 0),
	isnull(@RNU, 0),
	isnull(@ROP, 0),			-- CRE11-024-01: added
	isnull(@ROT, 0),
	isnull(@RPT, 0),
	isnull(@RRD, 0),
	isnull(@Prof_Total, 0),

	isnull(@App_ENU, 0),
	isnull(@App_RCM, 0),
	isnull(@App_RCP, 0),
	isnull(@App_RDT, 0),
	isnull(@App_RMP, 0),
	isnull(@App_RMT, 0),
	isnull(@App_RNU, 0),
	isnull(@App_ROP, 0),		-- CRE11-024-01: added
	isnull(@App_ROT, 0),
	isnull(@App_RPT, 0),
	isnull(@App_RRD, 0),
	isnull(@Approved_Account, 0),

	isnull(@Act_ENU, 0),
	isnull(@Act_RCM, 0),
	isnull(@Act_RCP, 0),
	isnull(@Act_RDT, 0),
	isnull(@Act_RMP, 0),
	isnull(@Act_RMT, 0),
	isnull(@Act_RNU, 0),
	isnull(@Act_ROP, 0),		-- CRE11-024-01: added
	isnull(@Act_ROT, 0),
	isnull(@Act_RPT, 0),
	isnull(@Act_RRD, 0),
	isnull(@Activated_Account, 0),

	isnull(@DEAcct0, 0),
	isnull(@DEAcct1, 0),
	isnull(@DEAcct2, 0),
	isnull(@DEAcct3, 0),
	isnull(@DEAcct4, 0),
	isnull(@DEAcct5, 0),
	isnull(@DEAcct6, 0),
	isnull(@DEAcct7, 0),
	isnull(@DEAcct8, 0),
	isnull(@DEAcct9, 0),
	isnull(@DEAcct10, 0),
	isnull(@DEAcct11, 0),
	isnull(@DEAcct12, 0),
	isnull(@DEAcct13, 0),
	isnull(@DEAcct14, 0),
	isnull(@DEAcct15, 0),
	isnull(@DEAcct16, 0),
	isnull(@DEAcct17, 0),
	isnull(@DEAcct18, 0),
	isnull(@DEAcct19, 0),
	isnull(@DEAcct20, 0),
	isnull(@DEAcct21, 0),
	isnull(@DEAcct22, 0),
	isnull(@DEAcct23, 0),
	isnull(@DEAcct24, 0),
	isnull(@DEAcct25, 0),
	isnull(@DEAcct26, 0),
	isnull(@DEAcct27, 0),
	isnull(@DEAcct28, 0),
	isnull(@DEAcct29, 0),
	isnull(@DEAcct30, 0),
	isnull(@DelistSP_NA, 0),
	isnull(@DelistSP_A, 0)
)


insert into @Stat2
(
	Dtm,
	HCVS_A,
	CIVSS_A,
	EVSSHSIVSS_A,
	HSIVSS_A,
	RVP_A,
	HCVS_CIVSS_A,
	HCVS_EVSSHSIVSS_A,
	HCVS_HSIVSS_A,
	HCVS_RVP_A,
	CIVSS_EVSSHSIVSS_A,
	CIVSS_HSIVSS_A,
	CIVSS_RVP_A,
	EVSSHSIVSS_HSIVSS_A,
	EVSSHSIVSS_RVP_A,
	HSIVSS_RVP_A,
	CIVSS_EVSSHSIVSS_HCVS_A,
	CIVSS_HCVS_HSIVSS_A,
	CIVSS_HCVS_RVP_A,
	EVSSHSIVSS_HCVS_HSIVSS_A,
	EVSSHSIVSS_HCVS_RVP_A,
	HCVS_HSIVSS_RVP_A,
	CIVSS_EVSSHSIVSS_HSIVSS_A,
	CIVSS_EVSSHSIVSS_RVP_A,
	CIVSS_HSIVSS_RVP_A,
	EVSSHSIVSS_HSIVSS_RVP_A,
	CIVSS_EVSSHSIVSS_HCVS_HSIVSS_A,
	CIVSS_EVSSHSIVSS_HCVS_RVP_A,
	CIVSS_HCVS_HSIVSS_RVP_A,
	EVSSHSIVSS_HCVS_HSIVSS_RVP_A,
	CIVSS_EVSSHSIVSS_HSIVSS_RVP_A,
	CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_A,
	Delist_Total_A,
	A_Others,
	A_Total
)
values
(
	@temp_date,
	isnull(@HCVS_A, 0),
	isnull(@CIVSS_A, 0),
	isnull(@EVSSHSIVSS_A, 0),
	isnull(@HSIVSS_A, 0),
	isnull(@RVP_A, 0),
	isnull(@HCVS_CIVSS_A, 0),
	isnull(@HCVS_EVSSHSIVSS_A, 0),
	isnull(@HCVS_HSIVSS_A, 0),
	isnull(@HCVS_RVP_A, 0),
	isnull(@CIVSS_EVSSHSIVSS_A, 0),
	isnull(@CIVSS_HSIVSS_A, 0),
	isnull(@CIVSS_RVP_A, 0),
	isnull(@EVSSHSIVSS_HSIVSS_A, 0),
	isnull(@EVSSHSIVSS_RVP_A, 0),
	isnull(@HSIVSS_RVP_A, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_A, 0),
	isnull(@CIVSS_HCVS_HSIVSS_A, 0),
	isnull(@CIVSS_HCVS_RVP_A, 0),
	isnull(@EVSSHSIVSS_HCVS_HSIVSS_A, 0),
	isnull(@EVSSHSIVSS_HCVS_RVP_A, 0),
	isnull(@HCVS_HSIVSS_RVP_A, 0),
	isnull(@CIVSS_EVSSHSIVSS_HSIVSS_A, 0),
	isnull(@CIVSS_EVSSHSIVSS_RVP_A, 0),
	isnull(@CIVSS_HSIVSS_RVP_A, 0),
	isnull(@EVSSHSIVSS_HSIVSS_RVP_A, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_A, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_RVP_A, 0),
	isnull(@CIVSS_HCVS_HSIVSS_RVP_A, 0),
	isnull(@EVSSHSIVSS_HCVS_HSIVSS_RVP_A, 0),
	isnull(@CIVSS_EVSSHSIVSS_HSIVSS_RVP_A, 0),
	isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_A, 0),
	isnull(@Deliat_Total_A, 0),
	isnull(@A_Others, 0),
	isnull(@A_Total, 0)
)


if @temp_date < @thirdReportStartDate
begin
	insert into @Stat3
	(
		Dtm,
		HCVS_AC,
		CIVSS_AC,
		EVSSHSIVSS_AC,
		HSIVSS_AC,
		RVP_AC,
		HCVS_CIVSS_AC,
		HCVS_EVSSHSIVSS_AC,
		HCVS_HSIVSS_AC,
		HCVS_RVP_AC,
		CIVSS_EVSSHSIVSS_AC,
		CIVSS_HSIVSS_AC,
		CIVSS_RVP_AC,
		EVSSHSIVSS_HSIVSS_AC,
		EVSSHSIVSS_RVP_AC,
		HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HCVS_AC,
		CIVSS_HCVS_HSIVSS_AC,
		CIVSS_HCVS_RVP_AC,
		EVSSHSIVSS_HCVS_HSIVSS_AC,
		EVSSHSIVSS_HCVS_RVP_AC,
		HCVS_HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HSIVSS_AC,
		CIVSS_EVSSHSIVSS_RVP_AC,
		CIVSS_HSIVSS_RVP_AC,
		EVSSHSIVSS_HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC,
		CIVSS_EVSSHSIVSS_HCVS_RVP_AC,
		CIVSS_HCVS_HSIVSS_RVP_AC,
		EVSSHSIVSS_HCVS_HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC,
		Delist_Total_AC,
		AC_Others,
		AC_Total
	)
	values
	(
		@temp_date,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1
	)

end
else
begin

	insert into @Stat3
	(
		Dtm,
		HCVS_AC,
		CIVSS_AC,
		EVSSHSIVSS_AC,
		HSIVSS_AC,
		RVP_AC,
		HCVS_CIVSS_AC,
		HCVS_EVSSHSIVSS_AC,
		HCVS_HSIVSS_AC,
		HCVS_RVP_AC,
		CIVSS_EVSSHSIVSS_AC,
		CIVSS_HSIVSS_AC,
		CIVSS_RVP_AC,
		EVSSHSIVSS_HSIVSS_AC,
		EVSSHSIVSS_RVP_AC,
		HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HCVS_AC,
		CIVSS_HCVS_HSIVSS_AC,
		CIVSS_HCVS_RVP_AC,
		EVSSHSIVSS_HCVS_HSIVSS_AC,
		EVSSHSIVSS_HCVS_RVP_AC,
		HCVS_HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HSIVSS_AC,
		CIVSS_EVSSHSIVSS_RVP_AC,
		CIVSS_HSIVSS_RVP_AC,
		EVSSHSIVSS_HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC,
		CIVSS_EVSSHSIVSS_HCVS_RVP_AC,
		CIVSS_HCVS_HSIVSS_RVP_AC,
		EVSSHSIVSS_HCVS_HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC,
		Delist_Total_AC,
		AC_Others,
		AC_Total
	)
	values
	(
		@temp_date,
		isnull(@HCVS_AC, 0),
		isnull(@CIVSS_AC, 0),
		isnull(@EVSSHSIVSS_AC, 0),
		isnull(@HSIVSS_AC, 0),
		isnull(@RVP_AC, 0),
		isnull(@HCVS_CIVSS_AC, 0),
		isnull(@HCVS_EVSSHSIVSS_AC, 0),
		isnull(@HCVS_HSIVSS_AC, 0),
		isnull(@HCVS_RVP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_AC, 0),
		isnull(@CIVSS_HSIVSS_AC, 0),
		isnull(@CIVSS_RVP_AC, 0),
		isnull(@EVSSHSIVSS_HSIVSS_AC, 0),
		isnull(@EVSSHSIVSS_RVP_AC, 0),
		isnull(@HSIVSS_RVP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_AC, 0),
		isnull(@CIVSS_HCVS_HSIVSS_AC, 0),
		isnull(@CIVSS_HCVS_RVP_AC, 0),
		isnull(@EVSSHSIVSS_HCVS_HSIVSS_AC, 0),
		isnull(@EVSSHSIVSS_HCVS_RVP_AC, 0),
		isnull(@HCVS_HSIVSS_RVP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HSIVSS_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_RVP_AC, 0),
		isnull(@CIVSS_HSIVSS_RVP_AC, 0),
		isnull(@EVSSHSIVSS_HSIVSS_RVP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_RVP_AC, 0),
		isnull(@CIVSS_HCVS_HSIVSS_RVP_AC, 0),
		isnull(@EVSSHSIVSS_HCVS_HSIVSS_RVP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC, 0),
		isnull(@Deliat_Total_AC, 0),
		isnull(@AC_Others, 0),
		isnull(@AC_Total, 0)
	)
end

if @temp_date < @RMPReportStartDate
begin
	insert into @Stat4
	(
		Dtm,
		HCVS_RMP_A,
		CIVSS_RMP_A,
		EVSSHSIVSS_RMP_A,
		HSIVSS_RMP_A,
		RVP_RMP_A,
		HCVS_CIVSS_RMP_A,
		HCVS_EVSSHSIVSS_RMP_A,
		HCVS_HSIVSS_RMP_A,
		HCVS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_RMP_A,
		CIVSS_HSIVSS_RMP_A,
		CIVSS_RVP_RMP_A,
		EVSSHSIVSS_HSIVSS_RMP_A,
		EVSSHSIVSS_RVP_RMP_A,
		HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HCVS_RMP_A,
		CIVSS_HCVS_HSIVSS_RMP_A,
		CIVSS_HCVS_RVP_RMP_A,
		EVSSHSIVSS_HCVS_HSIVSS_RMP_A,
		EVSSHSIVSS_HCVS_RVP_RMP_A,
		HCVS_HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HSIVSS_RMP_A,
		CIVSS_EVSSHSIVSS_RVP_RMP_A,
		CIVSS_HSIVSS_RVP_RMP_A,
		EVSSHSIVSS_HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A,
		CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A,
		CIVSS_HCVS_HSIVSS_RVP_RMP_A,
		EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A,
		Delist_Total_RMP_A,
		A_RMP_Others,
		A_RMP_Total
	)
	values
	(
		@temp_date,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1
	)

	insert into @Stat5
	(
		Dtm,
		HCVS_RMP_AC,
		CIVSS_RMP_AC,
		EVSSHSIVSS_RMP_AC,
		HSIVSS_RMP_AC,
		RVP_RMP_AC,
		HCVS_CIVSS_RMP_AC,
		HCVS_EVSSHSIVSS_RMP_AC,
		HCVS_HSIVSS_RMP_AC,
		HCVS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_RMP_AC,
		CIVSS_HSIVSS_RMP_AC,
		CIVSS_RVP_RMP_AC,
		EVSSHSIVSS_HSIVSS_RMP_AC,
		EVSSHSIVSS_RVP_RMP_AC,
		HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HCVS_RMP_AC,
		CIVSS_HCVS_HSIVSS_RMP_AC,
		CIVSS_HCVS_RVP_RMP_AC,
		EVSSHSIVSS_HCVS_HSIVSS_RMP_AC,
		EVSSHSIVSS_HCVS_RVP_RMP_AC,
		HCVS_HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC,
		CIVSS_EVSSHSIVSS_RVP_RMP_AC,
		CIVSS_HSIVSS_RVP_RMP_AC,
		EVSSHSIVSS_HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC,
		CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC,
		CIVSS_HCVS_HSIVSS_RVP_RMP_AC,
		EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC,
		Delist_Total_RMP_AC,
		AC_RMP_Others,
		AC_RMP_Total
	)
	values
	(
		@temp_date,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1,
		-1
	)

end
else
begin

	insert into @Stat4
	(
		Dtm,
		HCVS_RMP_A,
		CIVSS_RMP_A,
		EVSSHSIVSS_RMP_A,
		HSIVSS_RMP_A,
		RVP_RMP_A,
		HCVS_CIVSS_RMP_A,
		HCVS_EVSSHSIVSS_RMP_A,
		HCVS_HSIVSS_RMP_A,
		HCVS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_RMP_A,
		CIVSS_HSIVSS_RMP_A,
		CIVSS_RVP_RMP_A,
		EVSSHSIVSS_HSIVSS_RMP_A,
		EVSSHSIVSS_RVP_RMP_A,
		HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HCVS_RMP_A,
		CIVSS_HCVS_HSIVSS_RMP_A,
		CIVSS_HCVS_RVP_RMP_A,
		EVSSHSIVSS_HCVS_HSIVSS_RMP_A,
		EVSSHSIVSS_HCVS_RVP_RMP_A,
		HCVS_HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HSIVSS_RMP_A,
		CIVSS_EVSSHSIVSS_RVP_RMP_A,
		CIVSS_HSIVSS_RVP_RMP_A,
		EVSSHSIVSS_HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A,
		CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A,
		CIVSS_HCVS_HSIVSS_RVP_RMP_A,
		EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A,
		Delist_Total_RMP_A,
		A_RMP_Others,
		A_RMP_Total
	)
	values
	(
		@temp_date,
		isnull(@HCVS_RMP_A, 0),
		isnull(@CIVSS_RMP_A, 0),
		isnull(@EVSSHSIVSS_RMP_A, 0),
		isnull(@HSIVSS_RMP_A, 0),
		isnull(@RVP_RMP_A, 0),
		isnull(@HCVS_CIVSS_RMP_A, 0),
		isnull(@HCVS_EVSSHSIVSS_RMP_A, 0),
		isnull(@HCVS_HSIVSS_RMP_A, 0),
		isnull(@HCVS_RVP_RMP_A, 0),
		isnull(@CIVSS_EVSSHSIVSS_RMP_A, 0),
		isnull(@CIVSS_HSIVSS_RMP_A, 0),
		isnull(@CIVSS_RVP_RMP_A, 0),
		isnull(@EVSSHSIVSS_HSIVSS_RMP_A, 0),
		isnull(@EVSSHSIVSS_RVP_RMP_A, 0),
		isnull(@HSIVSS_RVP_RMP_A, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_RMP_A, 0),
		isnull(@CIVSS_HCVS_HSIVSS_RMP_A, 0),
		isnull(@CIVSS_HCVS_RVP_RMP_A, 0),
		isnull(@EVSSHSIVSS_HCVS_HSIVSS_RMP_A, 0),
		isnull(@EVSSHSIVSS_HCVS_RVP_RMP_A, 0),
		isnull(@HCVS_HSIVSS_RVP_RMP_A, 0),
		isnull(@CIVSS_EVSSHSIVSS_HSIVSS_RMP_A, 0),
		isnull(@CIVSS_EVSSHSIVSS_RVP_RMP_A, 0),
		isnull(@CIVSS_HSIVSS_RVP_RMP_A, 0),
		isnull(@EVSSHSIVSS_HSIVSS_RVP_RMP_A, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A, 0),
		isnull(@CIVSS_HCVS_HSIVSS_RVP_RMP_A, 0),
		isnull(@EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A, 0),
		isnull(@CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A, 0),
		isnull(@Deliat_Total_RMP_A, 0),
		isnull(@A_RMP_Others, 0),
		isnull(@A_RMP_Total, 0)
	)

	insert into @Stat5
	(
		Dtm,
		HCVS_RMP_AC,
		CIVSS_RMP_AC,
		EVSSHSIVSS_RMP_AC,
		HSIVSS_RMP_AC,
		RVP_RMP_AC,
		HCVS_CIVSS_RMP_AC,
		HCVS_EVSSHSIVSS_RMP_AC,
		HCVS_HSIVSS_RMP_AC,
		HCVS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_RMP_AC,
		CIVSS_HSIVSS_RMP_AC,
		CIVSS_RVP_RMP_AC,
		EVSSHSIVSS_HSIVSS_RMP_AC,
		EVSSHSIVSS_RVP_RMP_AC,
		HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HCVS_RMP_AC,
		CIVSS_HCVS_HSIVSS_RMP_AC,
		CIVSS_HCVS_RVP_RMP_AC,
		EVSSHSIVSS_HCVS_HSIVSS_RMP_AC,
		EVSSHSIVSS_HCVS_RVP_RMP_AC,
		HCVS_HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC,
		CIVSS_EVSSHSIVSS_RVP_RMP_AC,
		CIVSS_HSIVSS_RVP_RMP_AC,
		EVSSHSIVSS_HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC,
		CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC,
		CIVSS_HCVS_HSIVSS_RVP_RMP_AC,
		EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC,
		CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC,
		Delist_Total_RMP_AC,
		AC_RMP_Others,
		AC_RMP_Total
	)
	values
	(
		@temp_date,
		isnull(@HCVS_RMP_AC, 0),
		isnull(@CIVSS_RMP_AC, 0),
		isnull(@EVSSHSIVSS_RMP_AC, 0),
		isnull(@HSIVSS_RMP_AC, 0),
		isnull(@RVP_RMP_AC, 0),
		isnull(@HCVS_CIVSS_RMP_AC, 0),
		isnull(@HCVS_EVSSHSIVSS_RMP_AC, 0),
		isnull(@HCVS_HSIVSS_RMP_AC, 0),
		isnull(@HCVS_RVP_RMP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_RMP_AC, 0),
		isnull(@CIVSS_HSIVSS_RMP_AC, 0),
		isnull(@CIVSS_RVP_RMP_AC, 0),
		isnull(@EVSSHSIVSS_HSIVSS_RMP_AC, 0),
		isnull(@EVSSHSIVSS_RVP_RMP_AC, 0),
		isnull(@HSIVSS_RVP_RMP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_RMP_AC, 0),
		isnull(@CIVSS_HCVS_HSIVSS_RMP_AC, 0),
		isnull(@CIVSS_HCVS_RVP_RMP_AC, 0),
		isnull(@EVSSHSIVSS_HCVS_HSIVSS_RMP_AC, 0),
		isnull(@EVSSHSIVSS_HCVS_RVP_RMP_AC, 0),
		isnull(@HCVS_HSIVSS_RVP_RMP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_RVP_RMP_AC, 0),
		isnull(@CIVSS_HSIVSS_RVP_RMP_AC, 0),
		isnull(@EVSSHSIVSS_HSIVSS_RVP_RMP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC, 0),
		isnull(@CIVSS_HCVS_HSIVSS_RVP_RMP_AC, 0),
		isnull(@EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC, 0),
		isnull(@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC, 0),
		isnull(@Deliat_Total_RMP_AC, 0),
		isnull(@AC_RMP_Others, 0),
		isnull(@AC_RMP_Total, 0)
	)
end

select @temp_date = convert(varchar, dateadd(d, 1, @temp_date), 120)
END


select @HCVS_E_Total = sum(HCVS_E) from @Stat
select @CIVSS_E_Total = sum(CIVSS_E) from @Stat
select @EVSSHSIVSS_E_Total = sum(EVSSHSIVSS_E) from @Stat
select @CIVSS_HCVSE_E_Total = sum(CIVSS_HCVSE_E) from @Stat
select @HCVS_EVSSHSIVSS_E_Total = sum(HCVS_EVSSHSIVSS_E) from @Stat
select @CIVSS_EVSSHSIVSS_E_Total = sum(CIVSS_EVSSHSIVSS_E) from @Stat
select @CIVSS_EVSSHSIVSS_HCVS_E_Total = sum(CIVSS_EVSSHSIVSS_HCVS_E) from @Stat
select @E_Total_Total = sum(E_Total) from @Stat
SET @E_Others_Total = @E_Total_Total - (
						@HCVS_E_Total + @CIVSS_E_Total + @EVSSHSIVSS_E_Total + @CIVSS_HCVSE_E_Total 
						+ @HCVS_EVSSHSIVSS_E_Total + @CIVSS_EVSSHSIVSS_E_Total + @CIVSS_EVSSHSIVSS_HCVS_E_Total)

select @HCVS_P_Total  = sum(HCVS_P) from @Stat
select @CIVSS_P_Total  = sum(CIVSS_P) from @Stat
select @EVSSHSIVSS_P_Total  = sum(EVSSHSIVSS_P) from @Stat
select @HSIVSS_P_Total  = sum(HSIVSS_P) from @Stat
select @RVP_P_Total  = sum(RVP_P) from @Stat
select @HCVS_CIVSS_P_Total  = sum(HCVS_CIVSS_P) from @Stat
select @HCVS_EVSSHSIVSS_P_Total  = sum(HCVS_EVSSHSIVSS_P) from @Stat
select @HCVS_HSIVSS_P_Total  = sum(HCVS_HSIVSS_P) from @Stat
select @HCVS_RVP_P_Total  = sum(HCVS_RVP_P) from @Stat
select @CIVSS_EVSSHSIVSS_P_Total  = sum(CIVSS_EVSSHSIVSS_P) from @Stat
select @CIVSS_HSIVSS_P_Total  = sum(CIVSS_HSIVSS_P) from @Stat
select @CIVSS_RVP_P_Total  = sum(CIVSS_RVP_P) from @Stat
select @EVSSHSIVSS_HSIVSS_P_Total  = sum(EVSSHSIVSS_HSIVSS_P) from @Stat
select @EVSSHSIVSS_RVP_P_Total  = sum(EVSSHSIVSS_RVP_P) from @Stat
select @HSIVSS_RVP_P_Total  = sum(HSIVSS_RVP_P) from @Stat
select @CIVSS_EVSSHSIVSS_HCVS_P_Total  = sum(CIVSS_EVSSHSIVSS_HCVS_P) from @Stat
select @CIVSS_HCVS_HSIVSS_P_Total  = sum(CIVSS_HCVS_HSIVSS_P) from @Stat
select @CIVSS_HCVS_RVP_P_Total  = sum(CIVSS_HCVS_RVP_P) from @Stat
select @EVSSHSIVSS_HCVS_HSIVSS_P_Total  = sum(EVSSHSIVSS_HCVS_HSIVSS_P) from @Stat
select @EVSSHSIVSS_HCVS_RVP_P_Total  = sum(EVSSHSIVSS_HCVS_RVP_P) from @Stat
select @HCVS_HSIVSS_RVP_P_Total  = sum(HCVS_HSIVSS_RVP_P) from @Stat
select @CIVSS_EVSSHSIVSS_HSIVSS_P_Total  = sum(CIVSS_EVSSHSIVSS_HSIVSS_P) from @Stat
select @CIVSS_EVSSHSIVSS_RVP_P_Total  = sum(CIVSS_EVSSHSIVSS_RVP_P) from @Stat
select @CIVSS_HSIVSS_RVP_P_Total  = sum(CIVSS_HSIVSS_RVP_P) from @Stat
select @EVSSHSIVSS_HSIVSS_RVP_P_Total  = sum(EVSSHSIVSS_HSIVSS_RVP_P) from @Stat
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P_Total  = sum(CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P) from @Stat
select @CIVSS_EVSSHSIVSS_HCVS_RVP_P_Total  = sum(CIVSS_EVSSHSIVSS_HCVS_RVP_P) from @Stat
select @CIVSS_HCVS_HSIVSS_RVP_P_Total  = sum(CIVSS_HCVS_HSIVSS_RVP_P) from @Stat
select @EVSSHSIVSS_HCVS_HSIVSS_RVP_P_Total  = sum(EVSSHSIVSS_HCVS_HSIVSS_RVP_P) from @Stat
select @CIVSS_EVSSHSIVSS_HSIVSS_RVP_P_Total  = sum(CIVSS_EVSSHSIVSS_HSIVSS_RVP_P) from @Stat
select @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P_Total  = sum(CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P) from @Stat
select @None_P_Total = sum(None_P) from @Stat
select @P_Total_Total = sum(P_Total) from @Stat
SET @P_Others_Total = @P_Total_Total - (
						@HCVS_P_Total + @CIVSS_P_Total + @EVSSHSIVSS_P_Total + @HSIVSS_P_Total + @RVP_P_Total + 
						@HCVS_CIVSS_P_Total + @HCVS_EVSSHSIVSS_P_Total + @HCVS_HSIVSS_P_Total + @HCVS_RVP_P_Total + 
						@CIVSS_EVSSHSIVSS_P_Total + @CIVSS_HSIVSS_P_Total + @CIVSS_RVP_P_Total + @EVSSHSIVSS_HSIVSS_P_Total + 
						@EVSSHSIVSS_RVP_P_Total + @HSIVSS_RVP_P_Total + @CIVSS_EVSSHSIVSS_HCVS_P_Total + @CIVSS_HCVS_HSIVSS_P_Total + 
						@CIVSS_HCVS_RVP_P_Total + @EVSSHSIVSS_HCVS_HSIVSS_P_Total + @EVSSHSIVSS_HCVS_RVP_P_Total + @HCVS_HSIVSS_RVP_P_Total + 
						@CIVSS_EVSSHSIVSS_HSIVSS_P_Total + @CIVSS_EVSSHSIVSS_RVP_P_Total + @CIVSS_HSIVSS_RVP_P_Total + @EVSSHSIVSS_HSIVSS_RVP_P_Total + 
						@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P_Total + @CIVSS_EVSSHSIVSS_HCVS_RVP_P_Total + @CIVSS_HCVS_HSIVSS_RVP_P_Total + @EVSSHSIVSS_HCVS_HSIVSS_RVP_P_Total + 
						@CIVSS_EVSSHSIVSS_HSIVSS_RVP_P_Total + @CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P_Total + @None_P_Total)

select @MO_0_Total = sum(MO_0) from @Stat
select @MO_1_Total = sum(MO_1) from @Stat
select @MO_2_Total = sum(MO_2) from @Stat
select @MO_3_Total = sum(MO_3) from @Stat
select @MO_4_Total = sum(MO_4) from @Stat
select @MO_5_Total = sum(MO_5) from @Stat
select @MO_6_Total = sum(MO_6) from @Stat
select @MO_A6_Total = sum(MO_A6) from @Stat
select @MO_Total_Total = sum(MO_Total) from @Stat
select @Solo_Total = sum(Solo) from @Stat
select @Partenship_Total = sum(Partenship) from @Stat
select @Shareholder_Total = sum(Shareholder) from @Stat
select @Director_Total = sum(Director) from @Stat
select @Employee_Total = sum(Employee) from @Stat
select @Others_Total = sum(Others) from @Stat
select @MO_Rel_Total_Total = sum(MO_Rel_Total) from @Stat

select @Practice_0_Total = sum(Practice_0) from @Stat
select @Practice_1_Total = sum(Practice_1) from @Stat
select @Practice_2_Total = sum(Practice_2) from @Stat
select @Practice_3_Total = sum(Practice_3) from @Stat
select @Practice_4_Total = sum(Practice_4) from @Stat
select @Practice_5_Total = sum(Practice_5) from @Stat
select @Practice_6_Total = sum(Practice_6) from @Stat
select @Practice_A6_Total = sum(Practice_A6) from @Stat
select @Practice_Total_Total = sum(Practice_Total) from @Stat

select @ENU_Total = sum(ENU) from @Stat
select @RCM_Total = sum(RCM) from @Stat
select @RCP_Total = sum(RCP) from @Stat
select @RDT_Total = sum(RDT) from @Stat
select @RMP_Total = sum(RMP) from @Stat
select @RMT_Total = sum(RMT) from @Stat 
select @RNU_Total = sum(RNU) from @Stat
select @ROP_Total = sum(ROP) from @Stat			-- CRE11-024-01: added
select @ROT_Total = sum(ROT) from @Stat
select @RPT_Total = sum(RPT) from @Stat
select @RRD_Total = sum(RRD) from @Stat
select @Prof_Total_Total = sum(Prof_Total) from @Stat

--select @HCVS_A_Total = sum(HCVS_A) from @Stat2
--select @CIVSS_A_Total = sum(CIVSS_A) from @Stat2
--select @EVSSHSIVSS_A_Total = sum(EVSSHSIVSS_A) from @Stat2
--select @RVP_A_Total = sum(RVP_A) from @Stat2
--select @HCVS_CIVSS_A_Total = sum(HCVS_CIVSS_A) from @Stat2
--select @HCVS_EVSSHSIVSS_A_Total = sum(HCVS_EVSSHSIVSS_A) from @Stat2
--select @HCVS_RVP_A_Total = sum(HCVS_RVP_A) from @Stat2
--select @CIVSS_EVSSHSIVSS_A_Total = sum(CIVSS_EVSSHSIVSS_A) from @Stat2
--select @CIVSS_RVP_A_Total = sum(CIVSS_RVP_A) from @Stat2
--select @EVSSHSIVSS_RVP_A_Total = sum(EVSSHSIVSS_RVP_A) from @Stat2
--select @CIVSS_EVSSHSIVSS_HCVS_A_Total = sum(CIVSS_EVSSHSIVSS_HCVS_A) from @Stat2
--select @EVSSHSIVSS_HCVS_RVP_A_Total = sum(EVSSHSIVSS_HCVS_RVP_A) from @Stat2
--select @CIVSS_HCVS_RVP_A_Total = sum(CIVSS_HCVS_RVP_A) from @Stat2
--select @CIVSS_EVSSHSIVSS_RVP_A_Total = sum(CIVSS_EVSSHSIVSS_RVP_A) from @Stat2
--select @CIVSS_EVSSHSIVSS_HCVS_RVP_A_Total = sum(CIVSS_EVSSHSIVSS_HCVS_RVP_A) from @Stat2
--select @A_Total_Total = sum(A_Total) from @Stat2



insert into @Stat
(
	enrolment_date,
	HCVS_E,
	CIVSS_E,
	EVSSHSIVSS_E,
	CIVSS_HCVSE_E,
	HCVS_EVSSHSIVSS_E,
	CIVSS_EVSSHSIVSS_E,
	CIVSS_EVSSHSIVSS_HCVS_E,
	E_Others,
	E_Total,

	HCVS_P,
	CIVSS_P,
	EVSSHSIVSS_P,
	HSIVSS_P,
	RVP_P,
	HCVS_CIVSS_P,
	HCVS_EVSSHSIVSS_P,
	HCVS_HSIVSS_P,
	HCVS_RVP_P,
	CIVSS_EVSSHSIVSS_P,
	CIVSS_HSIVSS_P,
	CIVSS_RVP_P,
	EVSSHSIVSS_HSIVSS_P,
	EVSSHSIVSS_RVP_P,
	HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HCVS_P,
	CIVSS_HCVS_HSIVSS_P,
	CIVSS_HCVS_RVP_P,
	EVSSHSIVSS_HCVS_HSIVSS_P,
	EVSSHSIVSS_HCVS_RVP_P,
	HCVS_HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HSIVSS_P,
	CIVSS_EVSSHSIVSS_RVP_P,
	CIVSS_HSIVSS_RVP_P,
	EVSSHSIVSS_HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P,
	CIVSS_EVSSHSIVSS_HCVS_RVP_P,
	CIVSS_HCVS_HSIVSS_RVP_P,
	EVSSHSIVSS_HCVS_HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HSIVSS_RVP_P,
	CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P,
	None_P,
	P_Others,
	P_Total,

	MO_0,
	MO_1,
	MO_2,
	MO_3,
	MO_4,
	MO_5,
	MO_6,
	MO_A6,
	MO_Total,
	Solo,
	Partenship,
	Shareholder,
	Director,
	Employee,
	Others,
	MO_Rel_Total,

	Practice_0,
	Practice_1,
	Practice_2,
	Practice_3,
	Practice_4,
	Practice_5,
	Practice_6,
	Practice_A6,
	Practice_Total,

	ENU,
	RCM,
	RCP,
	RDT,
	RMP,
	RMT,
	RNU,
	ROP,				-- CRE11-024-01: added
	ROT,
	RPT,
	RRD,
	Prof_Total
)
values
(
	'Total',
	@HCVS_E_Total,
	@CIVSS_E_Total,
	@EVSSHSIVSS_E_Total,
	@CIVSS_HCVSE_E_Total,
	@HCVS_EVSSHSIVSS_E_Total,
	@CIVSS_EVSSHSIVSS_E_Total,
	@CIVSS_EVSSHSIVSS_HCVS_E_Total,
	@E_Others_Total,
	@E_Total_Total,

	@HCVS_P_Total,
	@CIVSS_P_Total,
	@EVSSHSIVSS_P_Total,
	@HSIVSS_P_Total,
	@RVP_P_Total,
	@HCVS_CIVSS_P_Total,
	@HCVS_EVSSHSIVSS_P_Total,
	@HCVS_HSIVSS_P_Total,
	@HCVS_RVP_P_Total,
	@CIVSS_EVSSHSIVSS_P_Total,
	@CIVSS_HSIVSS_P_Total,
	@CIVSS_RVP_P_Total,
	@EVSSHSIVSS_HSIVSS_P_Total,
	@EVSSHSIVSS_RVP_P_Total,
	@HSIVSS_RVP_P_Total,
	@CIVSS_EVSSHSIVSS_HCVS_P_Total,
	@CIVSS_HCVS_HSIVSS_P_Total,
	@CIVSS_HCVS_RVP_P_Total,
	@EVSSHSIVSS_HCVS_HSIVSS_P_Total,
	@EVSSHSIVSS_HCVS_RVP_P_Total,
	@HCVS_HSIVSS_RVP_P_Total,
	@CIVSS_EVSSHSIVSS_HSIVSS_P_Total,
	@CIVSS_EVSSHSIVSS_RVP_P_Total,
	@CIVSS_HSIVSS_RVP_P_Total,
	@EVSSHSIVSS_HSIVSS_RVP_P_Total,
	@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P_Total,
	@CIVSS_EVSSHSIVSS_HCVS_RVP_P_Total,
	@CIVSS_HCVS_HSIVSS_RVP_P_Total,
	@EVSSHSIVSS_HCVS_HSIVSS_RVP_P_Total,
	@CIVSS_EVSSHSIVSS_HSIVSS_RVP_P_Total,
	@CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P_Total,
	@None_P_Total,
	@P_Others_Total,
	@P_Total_Total,

	@MO_0_Total,
	@MO_1_Total,
	@MO_2_Total,
	@MO_3_Total,
	@MO_4_Total,
	@MO_5_Total,
	@MO_6_Total,
	@MO_A6_Total,
	@MO_Total_Total,
	@Solo_Total,
	@Partenship_Total,
	@Shareholder_Total,
	@Director_Total,
	@Employee_Total,
	@Others_Total,
	@MO_Rel_Total_Total,

	@Practice_0_Total,
	@Practice_1_Total,
	@Practice_2_Total,
	@Practice_3_Total,
	@Practice_4_Total,
	@Practice_5_Total,
	@Practice_6_Total,
	@Practice_A6_Total,
	@Practice_Total_Total,

	@ENU_Total,
	@RCM_Total,
	@RCP_Total,
	@RDT_Total,
	@RMP_Total,
	@RMT_Total,
	@RNU_Total,
	@ROP_Total,			-- CRE11-024-01: added
	@ROT_Total,
	@RPT_Total,
	@RRD_Total,
	@Prof_Total_Total
)


-- CRE11-024-01: begin of added
-- begin of block to:
--		- determine whether the Total shows 'NA' for profession 'ROP'
--		- determine the indicator to show 'NA' for profession 'ROP' on that date
declare @ShowROPTotal char(1)
declare @ShowROPind char(1)
declare @ShowROPTable Table (
		Dtm datetime,
		ShowInd char(1)
)

select @ShowROPTotal='N'
select @temp_date = @start_date
while datediff(d, @temp_date, @end_date) > 0
BEGIN
	SELECT @ShowROPind=CASE 
				WHEN datediff(d,
					isnull((select Enrol_Period_From from Profession where Service_Category_Code='ROP'), dateadd(d,1,@temp_date)),
					@temp_date) >= 0 then
						'Y'
					ELSE
						'N'
				END
	INSERT INTO @ShowROPTable(Dtm,ShowInd) VALUES (@temp_date, @ShowROPind)
	select @temp_date = convert(varchar, dateadd(d, 1, @temp_date), 120)
END
SELECT @ShowROPTotal=CASE WHEN 0 < (SELECT COUNT(1) FROM @ShowROPTable where ShowInd='Y') THEN 'Y' END
-- CRE11-024-01: end of added

-- CRE11-024-01: begin of modification
-- ---------------------------------------------
-- To Excel sheet: Content
-- ---------------------------------------------

select 'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114)

-- ---------------------------------------------
-- To Excel sheet: eHSD0009-01: EnrolSummary
-- ---------------------------------------------
select enrolment_date,
isnull(convert(char(6), HCVS_E), ''),
isnull(convert(char(6), CIVSS_E), ''),
isnull(convert(char(6), EVSSHSIVSS_E), ''),
isnull(convert(char(6), CIVSS_HCVSE_E), ''),
isnull(convert(char(6), HCVS_EVSSHSIVSS_E), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_E), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_E), ''),
isnull(convert(char(6), E_Others), ''),
isnull(convert(char(6), E_Total), ''),

isnull(convert(char(6), HCVS_P), ''),
isnull(convert(char(6), CIVSS_P), ''),
isnull(convert(char(6), EVSSHSIVSS_P), ''),
isnull(convert(char(6), HSIVSS_P), ''),
isnull(convert(char(6), RVP_P), ''),
isnull(convert(char(6), HCVS_CIVSS_P), ''),
isnull(convert(char(6), HCVS_EVSSHSIVSS_P), ''),
isnull(convert(char(6), HCVS_HSIVSS_P), ''),
isnull(convert(char(6), HCVS_RVP_P), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_P), ''),
isnull(convert(char(6), CIVSS_HSIVSS_P), ''),
isnull(convert(char(6), CIVSS_RVP_P), ''),
isnull(convert(char(6), EVSSHSIVSS_HSIVSS_P), ''),
isnull(convert(char(6), EVSSHSIVSS_RVP_P), ''),
isnull(convert(char(6), HSIVSS_RVP_P), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_P), ''),
isnull(convert(char(6), CIVSS_HCVS_HSIVSS_P), ''),
isnull(convert(char(6), CIVSS_HCVS_RVP_P), ''),
isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_P), ''),
isnull(convert(char(6), EVSSHSIVSS_HCVS_RVP_P), ''),
isnull(convert(char(6), HCVS_HSIVSS_RVP_P), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_P), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_RVP_P), ''),
isnull(convert(char(6), CIVSS_HSIVSS_RVP_P), ''),
isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RVP_P), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_P), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RVP_P), ''),
isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RVP_P ), ''),
isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RVP_P ), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RVP_P), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_P), ''),
isnull(convert(char(6), None_P), ''),
isnull(convert(char(6), P_Others), ''),
isnull(convert(char(6), P_Total), ''),

isnull(convert(char(6), MO_0), ''),
isnull(convert(char(6), MO_1), ''),
isnull(convert(char(6), MO_2), ''),
isnull(convert(char(6), MO_3), ''),
isnull(convert(char(6), MO_4), ''),
isnull(convert(char(6), MO_5), ''),
isnull(convert(char(6), MO_6), ''),
isnull(convert(char(6), MO_A6), ''),
isnull(convert(char(6), MO_Total), ''),
isnull(convert(char(6), Solo), ''),
isnull(convert(char(6), Partenship), ''),
isnull(convert(char(6), Shareholder), ''),
isnull(convert(char(6), Director), ''),
isnull(convert(char(6), Employee), ''),
isnull(convert(char(6), Others), ''),
isnull(convert(char(6), MO_Rel_Total), ''),

isnull(convert(char(6), Practice_0), ''),
isnull(convert(char(6), Practice_1), ''),
isnull(convert(char(6), Practice_2), ''),
isnull(convert(char(6), Practice_3), ''),
isnull(convert(char(6), Practice_4), ''),
isnull(convert(char(6), Practice_5), ''),
isnull(convert(char(6), Practice_6), ''),
isnull(convert(char(6), Practice_A6), ''),
isnull(convert(char(6), Practice_Total), ''),

isnull(convert(char(6), ENU), ''),
isnull(convert(char(6), RCM), ''),
isnull(convert(char(6), RCP), ''),
isnull(convert(char(6), RDT), ''),
isnull(convert(char(6), RMP), ''),
isnull(convert(char(6), RMT), ''),
isnull(convert(char(6), RNU), ''),		
		-- CRE11-024-01: begin of added
		case
			when enrolment_date like '%TOTAL%' AND @showROPTotal='Y' then
				isnull(convert(char(6), ROP), '')
			when enrolment_date not like '%TOTAL%' AND 0 < (select count(1) from @ShowROPTable where datediff(d,Dtm,convert(datetime,enrolment_date))=0 and ShowInd='Y') then
				isnull(convert(char(6), ROP), '')
			else
				'NA'
		end,
		
		-- CRE11-024-01: end of added
isnull(convert(char(6), ROT), ''),
isnull(convert(char(6), RPT), ''),
isnull(convert(char(6), RRD), ''),
isnull(convert(char(6), Prof_Total), ''),
isnull(convert(char(6), App_ENU), ''),
isnull(convert(char(6), App_RCM), ''),
isnull(convert(char(6), App_RCP), ''),
isnull(convert(char(6), App_RDT), ''),
isnull(convert(char(6), App_RMP), ''),
isnull(convert(char(6), App_RMT), ''),
isnull(convert(char(6), App_RNU), ''),
		-- CRE11-024-01: begin of added
		case
			when enrolment_date like '%TOTAL%' AND @showROPTotal='Y' then
				isnull(convert(char(6), App_ROP), '')
			when enrolment_date not like '%TOTAL%' AND 0 < (select count(1) from @ShowROPTable where datediff(d,Dtm,convert(datetime,enrolment_date))=0 and ShowInd='Y') then
				isnull(convert(char(6), App_ROP), '')
			else
				case when enrolment_date like '%TOTAL%' then
					''
				else
					'NA'
				end
		end,
		-- CRE11-024-01: end of added
isnull(convert(char(6), App_ROT), ''),
isnull(convert(char(6), App_RPT), ''),
isnull(convert(char(6), App_RRD), ''),
isnull(convert(char(6), Approved_Account), ''),

isnull(convert(char(6), Act_ENU), ''),
isnull(convert(char(6), Act_RCM), ''),
isnull(convert(char(6), Act_RCP), ''),
isnull(convert(char(6), Act_RDT), ''),
isnull(convert(char(6), Act_RMP), ''),
isnull(convert(char(6), Act_RMT), ''),
isnull(convert(char(6), Act_RNU), ''),
		-- CRE11-024-01: begin of added
		case
			when enrolment_date like '%TOTAL%' AND @showROPTotal='Y' then
				isnull(convert(char(6), Act_ROP), '')
			when enrolment_date not like '%TOTAL%' AND 0 < (select count(1) from @ShowROPTable where datediff(d,Dtm,convert(datetime,enrolment_date))=0 and ShowInd='Y') then
				isnull(convert(char(6), Act_ROP), '')
			else
				case when enrolment_date like '%TOTAL%' then
					''
				else
					'NA'
				end
		end,
		-- CRE11-024-01: end of added
isnull(convert(char(6), Act_ROT), ''),
isnull(convert(char(6), Act_RPT), ''),
isnull(convert(char(6), Act_RRD), ''),
isnull(convert(char(6), Activated_Account), ''),

isnull(convert(char(6), DEAcct0), ''),
isnull(convert(char(6), DEAcct1), ''),
isnull(convert(char(6), DEAcct2), ''),
isnull(convert(char(6), DEAcct3), ''),
isnull(convert(char(6), DEAcct4), ''),
isnull(convert(char(6), DEAcct5), ''),
isnull(convert(char(6), DEAcct6), ''),
isnull(convert(char(6), DEAcct7), ''),
isnull(convert(char(6), DEAcct8), ''),
isnull(convert(char(6), DEAcct9), ''),
isnull(convert(char(6), DEAcct10), ''),
isnull(convert(char(6), DEAcct11), ''),
isnull(convert(char(6), DEAcct12), ''),
isnull(convert(char(6), DEAcct13), ''),
isnull(convert(char(6), DEAcct14), ''),
isnull(convert(char(6), DEAcct15), ''),
isnull(convert(char(6), DEAcct16), ''),
isnull(convert(char(6), DEAcct17), ''),
isnull(convert(char(6), DEAcct18), ''),
isnull(convert(char(6), DEAcct19), ''),
isnull(convert(char(6), DEAcct20), ''),
isnull(convert(char(6), DEAcct21), ''),
isnull(convert(char(6), DEAcct22), ''),
isnull(convert(char(6), DEAcct23), ''),
isnull(convert(char(6), DEAcct24), ''),
isnull(convert(char(6), DEAcct25), ''),
isnull(convert(char(6), DEAcct26), ''),
isnull(convert(char(6), DEAcct27), ''),
isnull(convert(char(6), DEAcct28), ''),
isnull(convert(char(6), DEAcct29), ''),
isnull(convert(char(6), DEAcct30), ''),
isnull(convert(char(6), DelistSP_A), ''),
isnull(convert(char(6), DelistSP_NA), '')
from @Stat

-- ---------------------------------------------
-- To Excel sheet: eHSD0009-02: Approved SP
-- ---------------------------------------------

select DTM,
isnull(convert(char(6), HCVS_A), ''),
isnull(convert(char(6), CIVSS_A), ''),
isnull(convert(char(6), EVSSHSIVSS_A), ''),
isnull(convert(char(6), HSIVSS_A), ''),
isnull(convert(char(6), RVP_A), ''),
isnull(convert(char(6), HCVS_CIVSS_A), ''),
isnull(convert(char(6), HCVS_EVSSHSIVSS_A), ''),
isnull(convert(char(6), HCVS_HSIVSS_A), ''),
isnull(convert(char(6), HCVS_RVP_A), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_A), ''),
isnull(convert(char(6), CIVSS_HSIVSS_A), ''),
isnull(convert(char(6), CIVSS_RVP_A), ''),
isnull(convert(char(6), EVSSHSIVSS_HSIVSS_A), ''),
isnull(convert(char(6), EVSSHSIVSS_RVP_A), ''),
isnull(convert(char(6), HSIVSS_RVP_A), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_A), ''),
isnull(convert(char(6), CIVSS_HCVS_HSIVSS_A), ''),
isnull(convert(char(6), CIVSS_HCVS_RVP_A), ''),
isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_A), ''),
isnull(convert(char(6), EVSSHSIVSS_HCVS_RVP_A), ''),
isnull(convert(char(6), HCVS_HSIVSS_RVP_A), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_A), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_RVP_A), ''),
isnull(convert(char(6), CIVSS_HSIVSS_RVP_A), ''),
isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RVP_A), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_A), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RVP_A), ''),
isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RVP_A ), ''),
isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RVP_A ), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RVP_A), ''),
isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_A), ''),
isnull(convert(char(6), Delist_Total_A), ''),
isnull(convert(char(6), A_Others), ''),
isnull(convert(char(6), A_Total), '')
from @Stat2

-- ---------------------------------------------
-- To Excel sheet: eHSD0009-03: Activated SP
-- ---------------------------------------------

select DTM,
	case isnull(convert(char(6), HCVS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_AC), '') end,
	case isnull(convert(char(6), CIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_AC), '') end,
	case isnull(convert(char(6), HSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HSIVSS_AC), '') end,
	case isnull(convert(char(6), RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), RVP_AC), '') end,
	case isnull(convert(char(6), HCVS_CIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_CIVSS_AC), '') end,
	case isnull(convert(char(6), HCVS_EVSSHSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_EVSSHSIVSS_AC), '') end,
	case isnull(convert(char(6), HCVS_HSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_HSIVSS_AC), '') end,
	case isnull(convert(char(6), HCVS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_RVP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_AC), '') end,
	case isnull(convert(char(6), CIVSS_HSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HSIVSS_AC), '') end,
	case isnull(convert(char(6), CIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HSIVSS_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), HSIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_AC), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_HSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_HSIVSS_AC), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_RVP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_RVP_AC), '') end,
	case isnull(convert(char(6), HCVS_HSIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_HSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), CIVSS_HSIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RVP_AC), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RVP_AC ), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RVP_AC ), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_AC), '') end,
	case isnull(convert(char(6), Delist_Total_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), Delist_Total_AC), '') end,
	case isnull(convert(char(6), AC_Others), '') when '-1' then 'N/A' else isnull(convert(char(6), AC_Others), '') end,
	case isnull(convert(char(6), AC_Total), '') when '-1' then 'N/A' else isnull(convert(char(6), AC_Total), '') end
from @Stat3

-- ---------------------------------------------
-- To Excel sheet: eHSD0009-04: Approved RMP SP
-- ---------------------------------------------

select DTM,
	case isnull(convert(char(6), HCVS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_RMP_A), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), HSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), HSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), RVP_RMP_A), '') end,
	case isnull(convert(char(6), HCVS_CIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_CIVSS_RMP_A), '') end,
	case isnull(convert(char(6), HCVS_EVSSHSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_EVSSHSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), HCVS_HSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_HSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), HCVS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_HSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), HSIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), HSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), HCVS_HSIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_HSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_HSIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RVP_RMP_A ), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A ), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_A), '') end,
	case isnull(convert(char(6), Delist_Total_RMP_A), '') when '-1' then 'N/A' else isnull(convert(char(6), Delist_Total_RMP_A), '') end,
	case isnull(convert(char(6), A_RMP_Others), '') when '-1' then 'N/A' else isnull(convert(char(6), A_RMP_Others), '') end,
	case isnull(convert(char(6), A_RMP_Total), '') when '-1' then 'N/A' else isnull(convert(char(6), A_RMP_Total), '') end
from @Stat4

-- ---------------------------------------------
-- To Excel sheet: eHSD0009-05: Activated RMP SP
-- ---------------------------------------------

select DTM,
	case isnull(convert(char(6), HCVS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), HSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), RVP_RMP_AC), '') end,
	case isnull(convert(char(6), HCVS_CIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_CIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), HCVS_EVSSHSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_EVSSHSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), HCVS_HSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_HSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), HCVS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_HSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), HSIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), HCVS_HSIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), HCVS_HSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_HSIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RVP_RMP_AC ), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_HCVS_HSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC ), '') when '-1' then 'N/A' else isnull(convert(char(6), EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), CIVSS_EVSSHSIVSS_HCVS_HSIVSS_RVP_RMP_AC), '') end,
	case isnull(convert(char(6), Delist_Total_RMP_AC), '') when '-1' then 'N/A' else isnull(convert(char(6), Delist_Total_RMP_AC), '') end,
	case isnull(convert(char(6), AC_RMP_Others), '') when '-1' then 'N/A' else isnull(convert(char(6), AC_RMP_Others), '') end,
	case isnull(convert(char(6), AC_RMP_Total), '') when '-1' then 'N/A' else isnull(convert(char(6), AC_RMP_Total), '') end
from @Stat5

-- ---------------------------------------------
-- To Excel sheet: eHSD0009-06: Dummy SP Accounts
-- ---------------------------------------------

select sp_id from SPExceptionList
-- CRE11-024-01: end of modification
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_Utilization_Stat] TO HCVU
GO
