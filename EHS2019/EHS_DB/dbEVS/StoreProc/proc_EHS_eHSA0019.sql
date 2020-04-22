IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0019]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0019]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	30 March 2011
-- Description:		Adjust the datetime output format to code 120 (yyyy-mm-dd hh:mi:ss(24h))
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	25 March 2011
-- Description:		Adjust for report generation layout
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 March 2011
-- Description:		Handle deleted transactions and invalidated transactions
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 February 2011
-- Description:		eHSA0019 - Death Record Comparison
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0019]
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================

	DECLARE @Content table (
		Result_Seq		int,
		Result_Value1	varchar(100),
		Result_Value2	varchar(500)
	)
	
	DECLARE @Summary table (
		Result_Seq		int,
		Result_Value1	varchar(300),
		Result_Value2	varchar(50)
	)

	DECLARE @ResultTable1 table (
		Result_Seq		int,
		Result_Value1	varchar(250),	-- Seq No
		Result_Value2	varchar(50),	-- Date of death
		Result_Value3	varchar(50),	-- Date of registration
		Result_Value4	varchar(100),	-- Name
		Result_Value5	varchar(50),	-- eHealth Account ID
		Result_Value6	varchar(50),	-- eHA Record Status
		Result_Value7	varchar(50),	-- eHA with same HKID and name
		Result_Value8	varchar(100),	-- eHA with same HKID but with different name
		Result_Value9	varchar(50),	-- Last Service Date
		Result_Value10	varchar(50),	-- Last Service Date > Date of death
		Result_Value11	varchar(50),	-- No. of Days Different
		Result_Value12	varchar(50),	-- SPID of claim with last service date
		Result_Value13	varchar(50)		-- Transaction ID of claim with last service date
	)
	
	DECLARE @ResultTable2 table (
		Result_Seq		int,
		Result_Value1	varchar(250),	-- Seq No
		Result_Value2	varchar(50),	-- Date of death
		Result_Value3	varchar(50),	-- Date of registration
		Result_Value4	varchar(100),	-- Name
		Result_Value5	varchar(50),	-- Temporary Account Reference No.
		Result_Value6	varchar(50),	-- Temporary Account Record Status
		Result_Value7	varchar(50),	-- Temporary eHA with same HKID and name
		Result_Value8	varchar(100),	-- Temporary eHA with same HKID but with different name
		Result_Value9	varchar(50),	-- Temporary Account created by SPID
		Result_Value10	varchar(50),	-- Last Service Date
		Result_Value11	varchar(50),	-- Last Service Date > Date of death
		Result_Value12	varchar(50),	-- No. of Days Different
		Result_Value13	varchar(50),	-- SPID of claim with last service date
		Result_Value14	varchar(50)		-- Transaction ID of claim with last service date
	)
	
	DECLARE @ResultTable3 table (
		Result_Seq		int,
		Result_Value1	varchar(250),	-- Seq No
		Result_Value2	varchar(50),	-- Date of death
		Result_Value3	varchar(50),	-- Date of registration
		Result_Value4	varchar(100),	-- Name
		Result_Value5	varchar(50),	-- eHealth Account ID
		Result_Value6	varchar(50)		-- eHA Record Status
	)
	
	DECLARE @ResultTable4 table (
		Result_Seq		int,
		Result_Value1	varchar(250),	-- Seq No
		Result_Value2	varchar(50),	-- Date of death
		Result_Value3	varchar(50),	-- Date of registration
		Result_Value4	varchar(100),	-- Name
		Result_Value5	varchar(50),	-- Temporary Account Reference No.
		Result_Value6	varchar(50),	-- Temporary Account Record Status
		Result_Value7	varchar(50)		-- Created by SPID
	)


-- =============================================
-- Retrieve data
-- =============================================

OPEN SYMMETRIC KEY sym_key
DECRYPTION BY ASYMMETRIC KEY asym_Key

--Improve Performance 
--if temp #vouchertransaction table exists drop it 
IF EXISTS (SELECT TOP 1 name 
           FROM   tempdb.sys.tables 
           WHERE  name LIKE '%#vouchertransaction%') 
  BEGIN 
      DROP TABLE #vouchertransaction 
  END 

--Create temp. table for voucher transaction 
CREATE TABLE #vouchertransaction 
  ( 
     transaction_id      CHAR(20) COLLATE chinese_taiwan_stroke_ci_as, 
     voucher_acc_id      CHAR(15) COLLATE chinese_taiwan_stroke_ci_as, 
     service_receive_dtm DATETIME, 
     transaction_dtm     DATETIME 
  ) 

--Prepare records for voucher transaction 
INSERT INTO #vouchertransaction 
SELECT transaction_id, 
       voucher_acc_id, 
       service_receive_dtm, 
       transaction_dtm 
FROM   vouchertransaction vt 
WHERE  vt.Record_Status NOT IN ('I' , 'D')
		AND ISNULL(vt.Invalidation, '') <> 'I'
       AND EXISTS(SELECT voucher_acc_id 
                  FROM   personalinformation perinf 
                         INNER JOIN _deathrecord dr 
                           ON dr.encrypt_field1 = perinf.encrypt_field1 
                  WHERE  perinf.voucher_acc_id = vt.voucher_acc_id) 
ORDER  BY service_receive_dtm DESC, 
          transaction_dtm DESC 

--Delete those not the latest service receive date 
DELETE #vouchertransaction 
WHERE  voucher_acc_id = (SELECT TOP 1 voucher_acc_id 
                         FROM   #vouchertransaction tvt 
                         WHERE  tvt.voucher_acc_id = 
                               #vouchertransaction.voucher_acc_id) 
       AND service_receive_dtm != (SELECT MAX(service_receive_dtm) 
                                   FROM   #vouchertransaction tvt 
                                   WHERE  tvt.voucher_acc_id = 
                                          #vouchertransaction.voucher_acc_id) 
       AND transaction_dtm != (SELECT MAX(transaction_dtm) 
                               FROM   #vouchertransaction tvt 
                               WHERE  tvt.voucher_acc_id = 
                                      #vouchertransaction.voucher_acc_id) 

--If there are more than one records have the same service recevie date 
--Delete those not the latest transaction date 
IF EXISTS (SELECT COUNT(1) 
           FROM   #vouchertransaction 
           GROUP  BY voucher_acc_id 
           HAVING COUNT(1) > 2) 
  BEGIN 
      DELETE #vouchertransaction 
      WHERE  voucher_acc_id IN (SELECT voucher_acc_id 
                                FROM   #vouchertransaction 
                                GROUP  BY voucher_acc_id 
                                HAVING COUNT(1) > 2) 
             AND transaction_dtm != (SELECT MAX(transaction_dtm) 
                                     FROM   #vouchertransaction tvt 
                                     WHERE  tvt.voucher_acc_id = 
                                            #vouchertransaction.voucher_acc_id) 
  END 


-- ---------------------------------------------
-- eHSA0019-01: Validated eHealth accounts matched with deceased records
-- ---------------------------------------------
	
-- Content

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13)
SELECT
	   [Seq No] + 10 AS [Result_Seq], 
	   [Seq No], 
       [Date of Death], 
       [Date Of Registration], 
       [Name], 
       [eHS Voucher A/C ID], 
       [Voucher A/C Record Status], 
       [eHA with same HKID and name], 
       [eHA with same HKID but with different name], 
       [Last Service Date], 
       [Last Service Date > Date of death], 
       [No. of Days Difference], 
       [SP ID], 
       [Transaction ID] 
FROM  ( 
      -- 1 Main tables -- 
      -- 1A - (1) HKID same as name, (2) Last Service > Death, (3) Death Date is exact  
      -- 1B - (1) HKID same as name, (2) Last Service > Death, (3) Death Date contains X 
      SELECT drec.seqno 
             AS 
             [Seq No], 
             Isnull(dbo.Func_format_voucher_account_number('V', 
             pinfo.voucher_acc_id), '') AS 
             [eHS Voucher A/C ID],
			CASE CHARINDEX('X', drec.Date_Of_Death)
				WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, drec.Date_Of_Death, 105), 120)
				ELSE drec.Date_Of_Death
			END AS [Date of Death], 
			CONVERT(varchar(10), CONVERT(datetime, drec.Date_Of_Registration, 105), 120) AS [Date Of Registration], 
             drec.eng_name 
             AS [Name], 
             'Y' 
             AS [eHA with same HKID and name], 
             ' ' 
             AS [eHA with same HKID but with different name], 
             Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 120), '') 
             AS 
             [Last Service Date], 
			CASE 
				WHEN Date_Of_Death NOT LIKE '%X%' THEN 'Y' 
				ELSE 'S' 
			END AS [Last Service Date > Date of death], 
             Datediff(DAY, CONVERT(DATETIME, CONVERT(VARCHAR(10), 
                           REPLACE(drec.date_of_death, 'XX', '01'), 
                           112), 105), CONVERT(DATETIME, CONVERT(VARCHAR(10), 
                                       MAX( 
                                       service_receive_dtm), 112), 105)) 
             AS 
             [No. of Days Difference], 
             vtran.sp_id 
             AS [SP ID], 
             dbo.Func_format_system_number(vtran.transaction_id) 
             AS [Transaction ID], 
             vacc.record_status 
             AS [Voucher A/C Record Status] 
      FROM   _deathrecord AS drec 
             INNER JOIN personalinformation AS pinfo 
               ON drec.encrypt_field1 = pinfo.encrypt_field1 
             LEFT JOIN vouchertransaction AS vtran 
               ON pinfo.voucher_acc_id = vtran.voucher_acc_id 
                  AND pinfo.doc_code = vtran.doc_code 
                  AND vtran.Record_Status NOT IN ('I', 'D')
                  AND ISNULL(vtran.Invalidation, '') <> 'I'
             INNER JOIN voucheraccount AS vacc 
               ON vacc.voucher_acc_id = pinfo.voucher_acc_id 
      WHERE  pinfo.encrypt_field1 = drec.encrypt_field1 
             AND pinfo.encrypt_field1 IN (SELECT encrypt_field1 
                                          FROM   personalinformation 
                                          WHERE 
                     personalinformation.encrypt_field2 = drec.encrypt_field2) 
             AND ( date_of_death NOT LIKE '%X%' 
                    OR date_of_death LIKE '%X%' ) 
             AND transaction_id = (SELECT TOP 1 transaction_id 
                                   FROM   #vouchertransaction 
                                   WHERE  voucher_acc_id = pinfo.voucher_acc_id) 
      GROUP  BY pinfo.voucher_acc_id, 
                date_of_death, 
                date_of_registration, 
                eng_name, 
                sp_id, 
                seqno, 
                transaction_id, 
                vacc.record_status 
      HAVING CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX(service_receive_dtm), 
             112), 
             105) > 
             CONVERT(DATETIME, CONVERT(VARCHAR(10), 
             REPLACE(drec.date_of_death, 'XX', '01'), 
                    112), 105) 
      UNION ALL 
      -- 1C - (1) HKID same as name, (2) Last Service <= Death 
      SELECT drec.seqno 
             AS 
             [Seq No], 
             Isnull(dbo.Func_format_voucher_account_number('V', 
             pinfo.voucher_acc_id), '') AS 
             [eHS Voucher A/C ID], 
             CASE CHARINDEX('X', drec.Date_Of_Death)
				WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, drec.Date_Of_Death, 105), 120)
				ELSE drec.Date_Of_Death
             END AS [Date of Death],
             CONVERT(varchar(10), CONVERT(datetime, drec.Date_Of_Registration, 105), 120) AS [Date Of Registration], 
             drec.eng_name 
             AS [Name], 
             'Y' 
             AS [eHA with same HKID and name], 
             ' ' 
             AS [eHA with same HKID but with different name], 
             Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 120), '') 
             AS 
             [Last Service Date], 
             ' ' 
             AS [Last Service Date > Date of death], 
             ' ' 
             AS [No. of Days Difference], 
             ' ' 
             AS [SP ID], 
             ' ' 
             AS [Transaction ID], 
             vacc.record_status 
             AS [Voucher A/C Record Status] 
      FROM   _deathrecord AS drec 
             INNER JOIN personalinformation AS pinfo 
               ON drec.encrypt_field1 = pinfo.encrypt_field1 
             LEFT JOIN vouchertransaction AS vtran 
               ON pinfo.voucher_acc_id = vtran.voucher_acc_id 
                  AND pinfo.doc_code = vtran.doc_code 
					AND vtran.Record_Status NOT IN ('I', 'D')
					AND ISNULL(vtran.Invalidation, '') <> 'I'
             INNER JOIN voucheraccount AS vacc 
               ON vacc.voucher_acc_id = pinfo.voucher_acc_id 
      WHERE  pinfo.encrypt_field1 = drec.encrypt_field1 
             AND pinfo.encrypt_field1 IN (SELECT encrypt_field1 
                                          FROM   personalinformation 
                                          WHERE 
                     personalinformation.encrypt_field2 = drec.encrypt_field2) 
      GROUP  BY pinfo.voucher_acc_id, 
                date_of_death, 
                date_of_registration, 
                eng_name, 
                seqno, 
                vacc.record_status 
      HAVING CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX(service_receive_dtm), 
             112), 
             105) <= 
                    CONVERT(DATETIME, CONVERT(VARCHAR(10), 
                    REPLACE(drec.date_of_death, 'XX', '01'), 
                    112), 105) 
              OR Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 105), '') 
                 = '' 
      UNION ALL 
      -- 1D - (1) HKID different as name, (2) Last Service > Death, (3) Death Date is exact  
      -- 1E - (1) HKID different as name, (2) Last Service > Death, (3) Death Date contains X 
      SELECT drec.seqno 
             AS 
             [Seq No], 
             Isnull(dbo.Func_format_voucher_account_number('V', 
             pinfo.voucher_acc_id), '') AS 
             [eHS Voucher A/C ID], 
             CASE CHARINDEX('X', drec.date_of_death)
				WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, drec.date_of_death, 105), 120)
				ELSE drec.date_of_death
             END AS [Date of Death], 
             CONVERT(varchar(10), CONVERT(datetime, drec.date_of_registration, 105), 120) AS [Date Of Registration], 
             drec.eng_name 
             AS [Name], 
             ' ' 
             AS [eHA with same HKID and name], 
             Decryptbykey(pinfo.encrypt_field2) 
             AS 
             [eHA with same HKID but with different name], 
             Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 120), '') 
             AS 
             [Last Service Date], 
             CASE 
               WHEN date_of_death NOT LIKE '%X%' THEN 'Y' 
               ELSE CASE 
                      WHEN date_of_death LIKE '%X%' THEN 'S' 
                    END 
             END 
             AS [Last Service Date > Date of death], 
             Datediff(DAY, CONVERT(DATETIME, CONVERT(VARCHAR(10), 
                           REPLACE(drec.date_of_death, 'XX', '01'), 
                           112), 105), CONVERT(DATETIME, CONVERT(VARCHAR(10), 
                                       MAX( 
                                       service_receive_dtm), 112), 105)) 
             AS 
             [No. of Days Difference], 
             vtran.sp_id 
             AS [SP ID], 
             dbo.Func_format_system_number(vtran.transaction_id) 
             AS [Transaction ID], 
             vacc.record_status 
             AS [Voucher A/C Record Status] 
      FROM   _deathrecord AS drec 
             INNER JOIN personalinformation AS pinfo 
               ON drec.encrypt_field1 = pinfo.encrypt_field1 
             LEFT JOIN vouchertransaction AS vtran 
               ON pinfo.voucher_acc_id = vtran.voucher_acc_id 
                  AND pinfo.doc_code = vtran.doc_code 
					AND vtran.Record_Status NOT IN ('I', 'D')
					AND ISNULL(vtran.Invalidation, '') <> 'I'
             INNER JOIN voucheraccount AS vacc 
               ON vacc.voucher_acc_id = pinfo.voucher_acc_id 
      WHERE  pinfo.encrypt_field1 = drec.encrypt_field1 
             AND pinfo.encrypt_field1 NOT IN (SELECT encrypt_field1 
                                              FROM   personalinformation 
                                              WHERE 
                     personalinformation.encrypt_field2 = drec.encrypt_field2) 
             AND ( date_of_death NOT LIKE '%X%' 
                    OR date_of_death LIKE '%X%' ) 
             AND transaction_id = (SELECT TOP 1 transaction_id 
                                   FROM   #vouchertransaction 
                                   WHERE  voucher_acc_id = pinfo.voucher_acc_id) 
      GROUP  BY pinfo.voucher_acc_id, 
                date_of_death, 
                date_of_registration, 
                eng_name, 
                pinfo.encrypt_field2, 
                sp_id, 
                seqno, 
                transaction_id, 
                vacc.record_status 
      HAVING CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX(service_receive_dtm), 
             112), 
             105) > 
             CONVERT(DATETIME, CONVERT(VARCHAR(10), 
             REPLACE(drec.date_of_death, 'XX', '01'), 
                    112), 105) 
       UNION ALL 
       -- 1F - (1) HKID different as name, (2) Last Service <= Death 
       SELECT drec.seqno 
              AS 
              [Seq No], 
              Isnull(dbo.Func_format_voucher_account_number('V', 
              pinfo.voucher_acc_id), '') AS 
              [eHS Voucher A/C ID], 
              CASE CHARINDEX('X', drec.date_of_death)
				WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, drec.date_of_death, 105), 120)
				ELSE drec.date_of_death
			END AS [Date of Death], 
			CONVERT(varchar(10), CONVERT(datetime, drec.date_of_registration, 105), 120) AS [Date Of Registration], 
              drec.eng_name 
              AS [Name], 
              ' ' 
              AS [eHA with same HKID and name], 
              Decryptbykey(pinfo.encrypt_field2) 
              AS 
              [eHA with same HKID but with different name], 
              Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 120), '') 
              AS 
              [Last Service Date], 
              ' ' 
              AS [Last Service Date > Date of death], 
              ' ' 
              AS [No. of Days Difference], 
              ' ' 
              AS [SP ID], 
              ' ' 
              AS [Transaction ID], 
              vacc.record_status 
              AS [Voucher A/C Record Status] 
       FROM   _deathrecord AS drec 
              INNER JOIN personalinformation AS pinfo 
                ON drec.encrypt_field1 = pinfo.encrypt_field1 
              LEFT JOIN vouchertransaction AS vtran 
                ON pinfo.voucher_acc_id = vtran.voucher_acc_id 
                   AND pinfo.doc_code = vtran.doc_code 
					AND vtran.Record_Status NOT IN ('I', 'D')
					AND ISNULL(vtran.Invalidation, '') <> 'I'
              INNER JOIN voucheraccount AS vacc 
                ON vacc.voucher_acc_id = pinfo.voucher_acc_id 
       WHERE  pinfo.encrypt_field1 = drec.encrypt_field1 
              AND pinfo.encrypt_field1 NOT IN (SELECT encrypt_field1 
                                               FROM   personalinformation 
                                               WHERE 
                      personalinformation.encrypt_field2 = drec.encrypt_field2) 
       GROUP  BY pinfo.voucher_acc_id, 
                 date_of_death, 
                 date_of_registration, 
                 eng_name, 
                 pinfo.encrypt_field2, 
                 seqno, 
                 vacc.record_status 
       HAVING CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX(service_receive_dtm), 
              112), 
              105) <= 
                     CONVERT(DATETIME, CONVERT(VARCHAR(10), 
                     REPLACE(drec.date_of_death, 'XX', '01'), 
                     112), 105) 
               OR Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 105), '' 
                  ) = '' 
      ) AS aaaa 
ORDER  BY [Seq no] 


-- ---------------------------------------------
-- eHSA0019-02: Temporary eHealth accounts matched with deceased records
-- ---------------------------------------------

-- Content
	
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14)
SELECT
	   [Seq No] + 10 AS [Result_Seq], 
	   [Seq No], 
       [Date of Death], 
       [Date Of Registration], 
       [Name], 
       [Temp Voucher A/C ID], 
       [Voucher A/C Record Status], 
       [eHA with same HKID and name], 
       [eHA with same HKID but with different name], 
       [Created By], 
       [Last Service Date], 
       [Last Service Date > Date of death], 
       [No. of Days Difference], 
       [SP ID], 
       [Transaction ID] 
FROM   ( 
       -- 2A - (1) HKID same as name, (2) Last Service > Death, (3) Death Date is exact  
       -- 2B - (1) HKID same as name, (2) Last Service > Death, (3) Death Date contains X 
       SELECT drec.seqno 
              AS 
              [Seq No], 
              Isnull(dbo.Func_format_voucher_account_number('V', 
              pinfo.voucher_acc_id), '') AS 
              [eHS Voucher A/C ID], 
             CASE CHARINDEX('X', drec.date_of_death)
				WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, drec.date_of_death, 105), 120)
				ELSE drec.date_of_death
             END AS [Date of Death], 
             CONVERT(varchar(10), CONVERT(datetime, drec.date_of_registration, 105), 120) AS [Date Of Registration], 
              drec.eng_name 
              AS [Name], 
              'Y' 
              AS [eHA with same HKID and name], 
              ' ' 
              AS [eHA with same HKID but with different name], 
              Isnull(dbo.Func_format_voucher_account_number('T', 
              vacc.voucher_acc_id), '')  AS 
              [Temp Voucher A/C ID], 
              vacc.create_by 
              AS [Created By], 
              Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 120), '') 
              AS 
              [Last Service Date], 
              CASE 
                WHEN date_of_death NOT LIKE '%X%' THEN 'Y' 
                ELSE CASE 
                       WHEN date_of_death LIKE '%X%' THEN 'S' 
                     END 
              END 
              AS [Last Service Date > Date of death], 
              Datediff(DAY, CONVERT(DATETIME, CONVERT(VARCHAR(10), 
                            REPLACE(drec.date_of_death, 'XX', '01'), 
                            112), 105), CONVERT(DATETIME, CONVERT(VARCHAR(10), 
                                        MAX( 
                                        service_receive_dtm), 112), 105)) 
              AS 
              [No. of Days Difference], 
              vtran.sp_id 
              AS [SP ID], 
              dbo.Func_format_system_number(vtran.transaction_id) 
              AS [Transaction ID], 
              vacc.record_status 
              AS [Voucher A/C Record Status] 
       FROM   _deathrecord AS drec 
              INNER JOIN temppersonalinformation AS pinfo 
                ON drec.encrypt_field1 = pinfo.encrypt_field1 
              LEFT JOIN vouchertransaction AS vtran 
                ON pinfo.voucher_acc_id = vtran.temp_voucher_acc_id 
                   AND pinfo.doc_code = vtran.doc_code 
              INNER JOIN tempvoucheraccount AS vacc 
                ON pinfo.voucher_acc_id = vacc.voucher_acc_id 
       WHERE  pinfo.encrypt_field1 = drec.encrypt_field1 
              AND pinfo.encrypt_field1 IN (SELECT encrypt_field1 
                                           FROM   temppersonalinformation 
                                           WHERE 
temppersonalinformation.encrypt_field2 = drec.encrypt_field2) 
AND ( date_of_death NOT LIKE '%X%' 
OR date_of_death LIKE '%X%' ) 
AND vacc.record_status NOT IN ( 'V', 'D' ) 
GROUP  BY pinfo.voucher_acc_id, 
date_of_death, 
date_of_registration, 
eng_name, 
vacc.voucher_acc_id, 
vacc.create_by, 
sp_id, 
transaction_id, 
seqno, 
vacc.record_status 
HAVING CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX(service_receive_dtm), 112), 
105) > 
CONVERT(DATETIME, CONVERT(VARCHAR(10), 
REPLACE(drec.date_of_death, 'XX', '01'), 
112), 105) 
UNION ALL 
-- 2C - (1) HKID same as name, (2) Last Service <= Death 
SELECT drec.seqno 
AS 
[Seq No], 
Isnull(dbo.Func_format_voucher_account_number('V', 
pinfo.voucher_acc_id), '') AS 
[eHS Voucher A/C ID], 
CASE CHARINDEX('X', drec.date_of_death)
				WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, drec.date_of_death, 105), 120)
				ELSE drec.date_of_death
             END AS [Date of Death], 
             CONVERT(varchar(10), CONVERT(datetime, drec.date_of_registration, 105), 120) AS [Date Of Registration], 
drec.eng_name 
AS [Name], 
'Y' 
AS [eHA with same HKID and name], 
' ' 
AS [eHA with same HKID but with different name], 
Isnull(dbo.Func_format_voucher_account_number('T', 
vacc.voucher_acc_id), '')  AS 
[Temp Voucher A/C ID], 
vacc.create_by 
AS [Created By], 
Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 120), '') 
AS 
[Last Service Date], 
' ' 
AS [Last Service Date > Date of death], 
' ' 
AS [No. of Days Difference], 
' ' 
AS [SP ID], 
' ' 
AS [Transaction ID], 
vacc.record_status 
AS [Voucher A/C Record Status] 
FROM   _deathrecord AS drec 
INNER JOIN temppersonalinformation AS pinfo 
ON drec.encrypt_field1 = pinfo.encrypt_field1 
LEFT JOIN vouchertransaction AS vtran 
ON pinfo.voucher_acc_id = vtran.temp_voucher_acc_id 
AND pinfo.doc_code = vtran.doc_code 
INNER JOIN tempvoucheraccount AS vacc 
ON pinfo.voucher_acc_id = vacc.voucher_acc_id 
WHERE  pinfo.encrypt_field1 = drec.encrypt_field1 
AND pinfo.encrypt_field1 IN (SELECT encrypt_field1 
                     FROM   temppersonalinformation 
                     WHERE 
temppersonalinformation.encrypt_field2 = drec.encrypt_field2) 
AND vacc.record_status NOT IN ( 'V', 'D' ) 
GROUP  BY pinfo.voucher_acc_id, 
date_of_death, 
date_of_registration, 
eng_name, 
vacc.voucher_acc_id, 
vacc.create_by, 
sp_id, 
transaction_id, 
seqno, 
vacc.record_status 
HAVING CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX(service_receive_dtm), 112), 
105) <= 
CONVERT(DATETIME, CONVERT(VARCHAR(10), 
REPLACE(drec.date_of_death, 'XX', '01'), 
112), 105) 
OR Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 105), '') = '' 
UNION ALL 
-- 2D - (1) HKID different as name, (2) Last Service > Death, (3) Death Date is exact  
-- 2E - (1) HKID different as name, (2) Last Service > Death, (3) Death Date contains X 
SELECT drec.seqno 
AS 
[Seq No], 
Isnull(dbo.Func_format_voucher_account_number('V', 
pinfo.voucher_acc_id), '') AS 
[eHS Voucher A/C ID], 
CASE CHARINDEX('X', drec.date_of_death)
				WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, drec.date_of_death, 105), 120)
				ELSE drec.date_of_death
             END AS [Date of Death], 
             CONVERT(varchar(10), CONVERT(datetime, drec.date_of_registration, 105), 120) AS [Date Of Registration], 
drec.eng_name 
AS [Name], 
' ' 
AS [eHA with same HKID and name], 
Decryptbykey(pinfo.encrypt_field2) 
AS 
[eHA with same HKID but with different name], 
Isnull(dbo.Func_format_voucher_account_number('T', 
vacc.voucher_acc_id), '')  AS 
[Temp Voucher A/C ID], 
vacc.create_by 
AS [Created By], 
Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 120), '') 
AS 
[Last Service Date], 
CASE 
WHEN date_of_death NOT LIKE '%X%' THEN 'Y' 
ELSE CASE 
 WHEN date_of_death LIKE '%X%' THEN 'S' 
END 
END 
AS [Last Service Date > Date of death], 
Datediff(DAY, CONVERT(DATETIME, CONVERT(VARCHAR(10), 
      REPLACE(drec.date_of_death, 'XX', '01'), 
      112), 105), CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX( 
                  service_receive_dtm), 112), 105)) 
AS 
[No. of Days Difference], 
vtran.sp_id 
AS [SP ID], 
dbo.Func_format_system_number(vtran.transaction_id) 
AS [Transaction ID], 
vacc.record_status 
AS [Voucher A/C Record Status] 
FROM   _deathrecord AS drec 
INNER JOIN temppersonalinformation AS pinfo 
ON drec.encrypt_field1 = pinfo.encrypt_field1 
LEFT JOIN vouchertransaction AS vtran 
ON pinfo.voucher_acc_id = vtran.temp_voucher_acc_id 
AND pinfo.doc_code = vtran.doc_code 
INNER JOIN tempvoucheraccount AS vacc 
ON pinfo.voucher_acc_id = vacc.voucher_acc_id 
WHERE  pinfo.encrypt_field1 = drec.encrypt_field1 
AND pinfo.encrypt_field1 NOT IN (SELECT encrypt_field1 
                         FROM   temppersonalinformation 
                         WHERE 
temppersonalinformation.encrypt_field2 = drec.encrypt_field2) 
AND ( date_of_death NOT LIKE '%X%' 
OR date_of_death NOT LIKE '%X%' ) 
AND vacc.record_status NOT IN ( 'V', 'D' ) 
GROUP  BY pinfo.voucher_acc_id, 
date_of_death, 
date_of_registration, 
eng_name, 
pinfo.encrypt_field2, 
vacc.voucher_acc_id, 
vacc.create_by, 
sp_id, 
transaction_id, 
seqno, 
vacc.record_status 
HAVING CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX(service_receive_dtm), 112), 
105) > 
CONVERT(DATETIME, CONVERT(VARCHAR(10), 
REPLACE(drec.date_of_death, 'XX', '01'), 
112), 105) 
UNION ALL 
-- 2F - (1) HKID different as name, (2) Last Service <= Death 
SELECT drec.seqno 
AS 
[Seq No], 
Isnull(dbo.Func_format_voucher_account_number('V', 
pinfo.voucher_acc_id), '') AS 
[eHS Voucher A/C ID], 
CASE CHARINDEX('X', drec.date_of_death)
				WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, drec.date_of_death, 105), 120)
				ELSE drec.date_of_death
             END AS [Date of Death], 
             CONVERT(varchar(10), CONVERT(datetime, drec.date_of_registration, 105), 120) AS [Date Of Registration], 
drec.eng_name 
AS [Name], 
' ' 
AS [eHA with same HKID and name], 
Decryptbykey(pinfo.encrypt_field2) 
AS 
[eHA with same HKID but with different name], 
Isnull(dbo.Func_format_voucher_account_number('T', 
vacc.voucher_acc_id), '')  AS 
[Temp Voucher A/C ID], 
vacc.create_by 
AS [Created By], 
Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 120), '') 
AS 
[Last Service Date], 
' ' 
AS [Last Service Date > Date of death], 
' ' 
AS [No. of Days Difference], 
' ' 
AS [SP ID], 
' ' 
AS [Transaction ID], 
vacc.record_status 
AS [Voucher A/C Record Status] 
FROM   _deathrecord AS drec 
INNER JOIN temppersonalinformation AS pinfo 
ON drec.encrypt_field1 = pinfo.encrypt_field1 
LEFT JOIN vouchertransaction AS vtran 
ON pinfo.voucher_acc_id = vtran.temp_voucher_acc_id 
AND pinfo.doc_code = vtran.doc_code 
INNER JOIN tempvoucheraccount AS vacc 
ON pinfo.voucher_acc_id = vacc.voucher_acc_id 
WHERE  pinfo.encrypt_field1 = drec.encrypt_field1 
AND pinfo.encrypt_field1 NOT IN (SELECT encrypt_field1 
                          FROM   temppersonalinformation 
                          WHERE 
 temppersonalinformation.encrypt_field2 = drec.encrypt_field2) 
AND vacc.record_status NOT IN ( 'V', 'D' ) 
GROUP  BY pinfo.voucher_acc_id, 
date_of_death, 
date_of_registration, 
eng_name, 
pinfo.encrypt_field2, 
vacc.voucher_acc_id, 
vacc.create_by, 
sp_id, 
transaction_id, 
seqno, 
vacc.record_status 
HAVING CONVERT(DATETIME, CONVERT(VARCHAR(10), MAX(service_receive_dtm), 112), 
105) <= 
CONVERT(DATETIME, CONVERT(VARCHAR(10), 
REPLACE(drec.date_of_death, 'XX', '01'), 
112), 105) 
OR Isnull(CONVERT(VARCHAR(10), MAX(service_receive_dtm), 105), '') = '' 
) AS bbbb 
ORDER  BY [seq no] 


-- ---------------------------------------------
-- eHSA0019-03: Validated eHealth accounts with name matched with deceased records (HKID not provided)
-- ---------------------------------------------

-- Content

	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6)
	SELECT
		DR.SeqNo + 10 AS [Result_Seq],
		DR.SeqNo,
		CASE CHARINDEX('X', DR.Date_Of_Death)
			WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, DR.Date_Of_Death, 105), 120)
			ELSE DR.Date_Of_Death
		END AS [Date_Of_Death], 
		CONVERT(varchar(10), CONVERT(datetime, DR.Date_Of_Registration, 105), 120) AS [Date_Of_Registration], 
		DR.Eng_Name,
		dbo.func_format_voucher_account_number('V', VP.Voucher_Acc_ID) AS [Voucher_Acc_ID], 
		VA.Record_Status
	FROM
		_DeathRecord DR
			INNER JOIN PersonalInformation VP
				ON DR.Encrypt_Field2 = VP.Encrypt_Field2 
			INNER JOIN VoucherAccount VA
				ON VP.Voucher_Acc_ID = VA.Voucher_Acc_ID
	WHERE
		DR.HKID = 'XXXXXXXXX' 
	ORDER BY
		DR.SeqNo,
		VA.Voucher_Acc_ID
	

-- ---------------------------------------------
-- eHSA0019-04: Temporary eHealth accounts with name matched with deceased records (HKID not provided)
-- ---------------------------------------------

-- Content

	INSERT INTO @ResultTable4 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)
	SELECT
		DR.SeqNo + 10 AS [Result_Seq],
		DR.SeqNo,
		CASE CHARINDEX('X', DR.Date_Of_Death)
			WHEN 0 THEN CONVERT(varchar(10), CONVERT(datetime, DR.Date_Of_Death, 105), 120)
			ELSE DR.Date_Of_Death
		END AS [Date_Of_Death], 
		CONVERT(varchar(10), CONVERT(datetime, DR.Date_Of_Registration, 105), 120) AS [Date_Of_Registration], 
		DR.Eng_Name,
		dbo.func_format_voucher_account_number('T', TA.Voucher_Acc_ID) AS [Temp_Voucher_Acc_ID], 
		TA.Record_Status,
		TA.Create_By
	FROM
		_DeathRecord DR
			INNER JOIN TempPersonalInformation TP
				ON DR.Encrypt_Field2 = TP.Encrypt_Field2 
			INNER JOIN TempVoucherAccount TA
				ON TP.Voucher_Acc_ID = TA.Voucher_Acc_ID 
					AND TA.Record_Status NOT IN ('V', 'D') 
	WHERE
		DR.HKID = 'XXXXXXXXX' 
	ORDER BY
		DR.SeqNo,
		TA.Voucher_Acc_ID



-- =============================================
-- Process data
-- =============================================

-- ---------------------------------------------
-- Content
-- ---------------------------------------------
	
	INSERT INTO @Content (Result_Seq, Result_Value1, Result_Value2) VALUES (1, 'Sub Report ID', 'Sub Report Name')
	INSERT INTO @Content (Result_Seq, Result_Value1, Result_Value2) VALUES (10, 'eHSA0019-01', 'Validated eHealth accounts matched with deceased records')
	INSERT INTO @Content (Result_Seq, Result_Value1, Result_Value2) VALUES (11, 'eHSA0019-02', 'Temporary eHealth accounts matched with deceased records')
	INSERT INTO @Content (Result_Seq, Result_Value1, Result_Value2) VALUES (12, 'eHSA0019-03', 'Validated eHealth accounts with name matched with deceased records (HKID not provided)')
	INSERT INTO @Content (Result_Seq, Result_Value1, Result_Value2) VALUES (13, 'eHSA0019-04', 'Temporary eHealth accounts with name matched with deceased records (HKID not provided)')

	INSERT INTO @Content (Result_Seq) VALUES (30)
	INSERT INTO @Content (Result_Seq) VALUES (31)
	
	INSERT INTO @Content (Result_Seq, Result_Value1, Result_Value2) VALUES (40, 'Report Generation Time: ' + CONVERT(varchar, GETDATE(), 111) + ' ' + CONVERT(varchar(5), GETDATE(), 114), '')


-- ---------------------------------------------
-- Summary
-- ---------------------------------------------

	INSERT INTO @Summary (Result_Seq, Result_Value1) VALUES (1, 'Summary:')
	
	INSERT INTO @Summary (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		10,
		'Total no. of original death records for matching with eHealth Accounts (eHA) in eHS:',
		COUNT(1)
	FROM
		_DeathRecord
		
	INSERT INTO @Summary (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		11,
		'Total no. of validated eHA matched with deceased records:',
		COUNT(1)
	FROM
		@ResultTable1
		
	INSERT INTO @Summary (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		12,
		'Total no. of temporary eHA matched with deceased records:',
		COUNT(1)
	FROM
		@ResultTable2
		
	INSERT INTO @Summary (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		13,
		'Total no. of validated eHA with name matched with deceased records (HKID not provided):',
		COUNT(1)
	FROM
		@ResultTable3
		
	INSERT INTO @Summary (Result_Seq, Result_Value1, Result_Value2)
	SELECT
		14,
		'Total no. of temporary eHA with name matched with deceased records (HKID not provided):',
		COUNT(1)
	FROM
		@ResultTable4


-- ---------------------------------------------
-- eHSA0019-01: Validated eHealth accounts matched with deceased records
-- ---------------------------------------------

-- Header

	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1) VALUES
	(1, 'eHSA0019-01: Validated eHealth accounts matched with deceased records')
	
	INSERT INTO @ResultTable1 (Result_Seq) VALUES
	(2)
	
	INSERT INTO @ResultTable1 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13) VALUES
	(3, 'Seq No.', 'Date of death', 'Date of registration', 'Name', 'eHealth Account ID', 'eHA Record Status', 'eHA with same HKID and name', 
	'eHA with same HKID but with different name', 'Last Service Date', 'Last Service Date > Date of death', 'No. of Days Different', 
	'SPID of claim with last service date', 'Transaction ID of claim with last service date')


-- ---------------------------------------------
-- eHSA0019-02: Temporary eHealth accounts matched with deceased records
-- ---------------------------------------------

-- Header

	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1) VALUES 
	(1, 'eHSA0019-02: Temporary eHealth accounts matched with deceased records')
	
	INSERT INTO @ResultTable2 (Result_Seq) VALUES 
	(2)
	
	INSERT INTO @ResultTable2 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7, Result_Value8, Result_Value9, Result_Value10, Result_Value11, Result_Value12, Result_Value13, Result_Value14) VALUES 
	(3, 'Seq No.', 'Date of death', 'Date of registration', 'Name', 'Temporary Account Reference No.', 'Temporary Account Record Status', 'Temporary eHA with same HKID and name', 'Temporary eHA with same HKID but with different name', 'Temporary Account created by SPID', 'Last Service Date', 'Last Service Date > Date of death', 'No. of Days Different', 'SPID of claim with last service date', 'Transaction ID of claim with last service date')
	

-- ---------------------------------------------
-- eHSA0019-03: Validated eHealth accounts with name matched with deceased records (HKID not provided)
-- ---------------------------------------------

-- Header

	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1) VALUES
	(1, 'eHSA0019-03: Validated eHealth accounts with name matched with deceased records (HKID not provided)')
	
	INSERT INTO @ResultTable3 (Result_Seq) VALUES
	(2)
	
	INSERT INTO @ResultTable3 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
	(3, 'Seq No.', 'Date of death', 'Date of registration', 'Name', 'eHealth Account ID', 'eHA Record Status')


-- ---------------------------------------------
-- eHSA0019-04: Temporary eHealth accounts with name matched with deceased records (HKID not provided)
-- ---------------------------------------------

-- Header

	INSERT INTO @ResultTable4 (Result_Seq, Result_Value1) VALUES
	(1, 'eHSA0019-04: Temporary eHealth accounts with name matched with deceased records (HKID not provided)')
	
	INSERT INTO @ResultTable4 (Result_Seq) VALUES
	(2)
	
	INSERT INTO @ResultTable4 (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7) VALUES
	(3, 'Seq No.', 'Date of death', 'Date of registration', 'Name', 'Temporary Account Reference No.', 'Temporary Account Record Status', 'Created by SPID')
	


-- =============================================
-- Return result
-- =============================================

	SELECT
		Result_Value1,
		Result_Value2
	FROM
		@Content
	ORDER BY
		Result_Seq
	
	SELECT
		Result_Value1,
		Result_Value2
	FROM
		@Summary
	ORDER BY
		Result_Seq
	
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13
	FROM
		@ResultTable1
	ORDER BY
		Result_Seq

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12,
		Result_Value13,
		Result_Value14
	FROM
		@ResultTable2
	ORDER BY
		Result_Seq

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6
	FROM
		@ResultTable3
	ORDER BY
		Result_Seq
		
	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7
	FROM
		@ResultTable4
	ORDER BY
		Result_Seq	

		
-- =============================================
-- Finalizer
-- =============================================

IF EXISTS (SELECT TOP 1 name 
           FROM   tempdb.sys.tables 
           WHERE  name LIKE '%#vouchertransaction%') 
  BEGIN 
      DROP TABLE #vouchertransaction 
  END 

CLOSE SYMMETRIC KEY sym_key  


END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0019] TO HCVU
GO
