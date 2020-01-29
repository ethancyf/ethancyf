IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SpecialAccount_Add_ByDate]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SpecialAccount_Add_ByDate]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE14-016 (To introduce 'Deceased' status into eHS)
-- Modified by:	    Winnie SUEN
-- Modified date:   15 Nov 2017
-- Description:		(1) Add VoucherAccount - [Deceased]
--					(2) Add PersonalInformation - [Deceased], [DOD], [Exact_DOD]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	01 Mar 2010
-- Description:		1. Roll back to 25Jan2010
--					2. 'REPMT' can be validated electronically (for all Date Of Issue)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	25 Feb 2010
-- Description:		1. Remove the criteria of the Doc Code,
--					2. The follow docType will be convert under existing condition
--						('ADOPC', 'Doc/I', 'HKBC', 'REPMT', 'VISA')
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	25 Jan 2010
-- Description:		1. Update Transaction where clause, transaction may Suspended
--					2. Fix the bug in select max(system_dtm) Where Clause
--					3. Confirm date < Specific_dtm
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 30 Oct 2009
-- Description:	convert the temporary account special account before specific date
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	30 Nov 2009
-- Description:		1. Only those temporary account with transaction will be
--					   convert to special account
--					2. Set Original_record_status in VoucherTranSuspendLOG
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	30 Nov 2009
-- Description:		1. Do not count the following case: 
--					Account: Confirmed, Transaction: Pending Confirmation
-- =============================================

CREATE proc [dbo].[proc_SpecialAccount_Add_ByDate] @Specific_Dtm datetime
as  
begin  
 declare @vid char(15)  
 declare @chkdgt char(1)  
 declare @spvid char(15)  
 declare @seq varchar(8), @prefix varchar(8)  
 declare @d varchar(2), @m varchar(2), @y varchar(2)   
 declare @sidprefix varchar(7)  
 DECLARE @mynumber varchar(8), @padding  INT  
  
 create table #AccounttoSpecial  
 (  
 Voucher_Acc_Id char(15) collate database_default primary key ,  
 SP_ID char(8),   
 SP_Practice_Display_Seq smallint,  
 DataEntry_By varchar(20) null,  
 Create_by varchar(20),  
 Create_dtm datetime,   
 Update_by varchar(20),  
 Update_dtm datetime,  
 Special_Acc_Id char(15) null  
 )  
                                             
                                         
	DECLARE @DI_DOI AS DATETIME
	DECLARE @REPMT_DOI AS DATETIME
 
	SELECT @DI_DOI = CONVERT(DateTime, Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'DI_DOI' AND [Scheme_Code] = 'ALL'
	SELECT @REPMT_DOI = CONVERT(DateTime, Parm_Value1) FROM [SystemParameters] WHERE [Parameter_Name] = 'REPMT_DOI' AND [Scheme_Code] = 'ALL'

  
 insert into #AccounttoSpecial  
 (Voucher_Acc_ID, SP_ID, SP_Practice_Display_Seq, DataEntry_By, Create_by, Create_dtm, Update_by, Update_dtm)  
 select TA.Voucher_Acc_ID, TAL.SP_ID, TAL.SP_Practice_Display_Seq, TAL.DataEntry_By,   
     TAL.Create_by, TAL.Create_dtm, TAL.Update_by, TAL.Update_dtm   
 from TempVoucherAccount TA, TempPersonalInformation TP, VoucherAccountCreationLOG TAL, DocType doc
		, VoucherTransaction VT
  where TA.Voucher_Acc_Id = TP.VOucher_Acc_ID   
 and TA.Voucher_Acc_ID = TAL.Voucher_Acc_ID   
 and TA.Voucher_Acc_ID = VT.Temp_Voucher_Acc_ID
 and isNull(VT.Voucher_Acc_ID, '') = ''
 and VT.Record_Status not in ('I', 'W', 'P')
 and VT.Confirmed_Dtm is not Null -- for Record_Status 'S'
 and TAL.Voucher_Acc_Type = 'T'  
 and TA.record_status = 'P'
 and TA.account_purpose not in ('A', 'O')  
 AND doc.[Doc_Code] = TP.[Doc_Code] 
 AND doc.[IMMD_Validate_Avail] = 'N' 
AND doc.[Force_Manual_Validate] = 'N'
 /*
 AND TP.Doc_Code in ('ADOPC', 'Doc/I', 'HKBC', 'REPMT', 'VISA')   
 */
 
 and
 (
 (TP.Doc_Code = 'HKBC' and (TP.Exact_DOB='D' or TP.Exact_DOB='M' or TP.Exact_DOB= 'Y'))   
 or   
 (TP.Doc_Code = 'ADOPC' and (TP.Exact_DOB='D' or TP.Exact_DOB='M' or TP.Exact_DOB= 'Y'))  
 or  
 (TP.Doc_Code = 'Doc/I' and TP.Date_of_Issue >= @DI_DOI)  
 or   
 (TP.Doc_Code = 'REPMT')-- and TP.Date_of_Issue >= @REPMT_DOI)  
 or  
 (TP.Doc_Code = 'VISA')  
 )
 
 AND VT.[Confirmed_Dtm] < @Specific_Dtm
 
-- and TA.Voucher_Acc_ID in 
--	(	
--		select temp_voucher_acc_id
--		from vouchertransaction 
--		where isnull(voucher_acc_id,'') ='' 
--		and record_status not in ('I','W')
--	)
 order by TA.create_dtm  
  
 --------------  
 select @d = convert(varchar(2),datepart(d,getdate())),   
 @m = convert(varchar(2), datepart(m,getdate())),   
 @y = substring(convert(varchar(4),datepart(yy,getdate())),3,2)  
  
 if len(@d) = 1   
 begin  
  set @d = '0' + @d  
 end  
  
 if @m ='10'   
 begin  
  set @m = 'A'  
 end  
 if @m ='11'   
 begin  
  set @m = 'B'  
 end  
 if @m ='12'   
 begin  
  set @m = 'C'  
 end  
  
 set @sidprefix = @y + @m + @d  
 --set @chkdgt = 0  
 set @padding = 8  
  
 declare x cursor for   
 select voucher_acc_id  
 from #AccounttoSpecial  
  
 open x  
 fetch next from x into @vid  
  while @@fetch_status = 0  
  begin  
     
   exec proc_SystemProfile_get_byProfileIDWithSchemeandOutput 'SAID', 'ALL', @seq output, @prefix output  
     
   SELECT @mynumber = @seq  
   SELECT @mynumber = REPLACE(STR(@mynumber, @padding), ' ', '0')  
     
   set @spvid = @prefix + @sidprefix + @mynumber --+ convert(char(1),@chkdgt)  

   exec proc_generateChkDgt @spvid, @chkdgt output
   set @spvid = rtrim(ltrim(@spvid)) + @chkdgt
  
   update #AccounttoSpecial  
   set Special_Acc_Id = @spvid  
   where voucher_acc_id = @vid  
  
   fetch next from x into  
   @vid  
  
   --set @chkdgt = (@chkdgt + 1) % 10  
  end   
 close x  
 deallocate x  
 --------------  
  
 update TempPersonalInformation   
 set Validating = 'Y', update_dtm = getdate()  
 where voucher_acc_id in   
 (select voucher_acc_id   
  from #AccounttoSpecial  
 )  
   
 insert into SpecialAccount  
 (Special_Acc_ID,   
  Scheme_Code,   
  Record_Status,   
  Account_Purpose,  
  Confirmed_Dtm,   
  Last_Fail_Validate_Dtm,  
  Original_Acc_ID,  
     Create_Dtm,   
     Create_By,  
  Update_Dtm,  
  Update_By,  
  DataEntry_By,  
  Temp_Voucher_Acc_ID,
  Deceased
 )  
  select   
  A.Special_Acc_ID,  
  TVA.Scheme_Code,   
  'P',   
  TVA.Account_Purpose,  
  TVA.Confirm_Dtm,   
  NULL,  
 case isNull(TVA.Original_Acc_ID,'') when '' then TVA.Voucher_Acc_ID  
        else TVA.Original_Acc_ID end,  
  TVA.Create_Dtm,   
     TVA.Create_By,  
  TVA.Update_Dtm,  
  TVA.Update_By,  
  TVA.DataEntry_By,  
  case isNull(TVA.Original_Acc_ID,'') when '' then NULL   
         else TVA.Voucher_Acc_ID end,
  TVA.Deceased
  from TempVoucherAccount TVA, #AccounttoSpecial A  
  where TVA.Voucher_Acc_ID = A.Voucher_Acc_ID  
     
  insert into SpecialPersonalInformation  
 (Special_Acc_ID,   
  Doc_Code,  
  DOB,  
  Exact_DOB,  
  Sex,  
  Date_of_Issue,  
  Check_Dtm,  
  Validating,  
  EC_Serial_no,  
  EC_Reference_no,  
  EC_Age,  
  EC_Date_of_Registration,  
  Foreign_Passport_No,  
  Permit_to_Remain_Until,  
     Record_Status,  
  Other_Info,  
     Create_dtm,   
  Create_by,  
  Update_Dtm,  
  Update_by,  
     DataEntry_By,  
  Encrypt_Field1,  
   Encrypt_Field2,  
   Encrypt_Field3,  
  Encrypt_Field4,  
  Encrypt_Field5,  
  Encrypt_Field6,  
  Encrypt_Field7,  
  Encrypt_Field8,  
  Encrypt_Field9,  
  Encrypt_Field10,  
  Encrypt_Field11,
  Deceased,
  DOD,
  Exact_DOD  
     )  
  select   
  A.Special_Acc_ID,   
  TP.Doc_Code,  
  TP.DOB,  
  TP.Exact_DOB,  
  TP.Sex,  
  TP.Date_of_Issue,  
  TP.Check_Dtm,  
  'N',  
  TP.EC_Serial_no,  
  TP.EC_Reference_no,  
  TP.EC_Age,  
  TP.EC_Date_of_Registration,  
  TP.Foreign_Passport_No,  
  TP.Permit_to_Remain_Until,  
     TP.Record_Status,  
  TP.Other_Info,  
     TP.Create_dtm,   
  TP.Create_by,  
  TP.Update_Dtm,  
  TP.Update_by,  
     TP.DataEntry_By,  
  TP.Encrypt_Field1,  
   TP.Encrypt_Field2,  
   TP.Encrypt_Field3,  
  TP.Encrypt_Field4,  
  TP.Encrypt_Field5,  
  TP.Encrypt_Field6,  
  TP.Encrypt_Field7,  
  TP.Encrypt_Field8,  
  TP.Encrypt_Field9,  
  TP.Encrypt_Field10,  
  TP.Encrypt_Field11,
  TP.Deceased,
  TP.DOD,
  TP.Exact_DOD  
  from TempPersonalInformation TP, #AccounttoSpecial A  
  where TP.Voucher_Acc_ID = A.Voucher_Acc_ID  
    
 insert into VoucherAccountCreationLog  
 (Transaction_Dtm,  
     Voucher_Acc_ID,   
  Voucher_Acc_Type,  
  SP_ID,  
  SP_Practice_Display_Seq,  
  Create_Dtm,   
  Create_by,  
  Update_dtm,  
     Update_by,  
     DataEntry_By  
 )  
 select   
 getdate(),  
 Special_Acc_Id,  
 'S',  
 SP_ID,   
 SP_Practice_Display_Seq,  
 Create_dtm,   
 Create_by,  
 Update_dtm,  
 Update_by,  
 DataEntry_By  
 from #AccounttoSpecial  
  

-- Update The Special Account Field of Transaction (Include in #AccounttoSpecial)
UPDATE [VoucherTransaction]
SET
	[Special_Acc_ID] = A.[Special_Acc_ID]
FROM [VoucherTransaction] VT, #AccounttoSpecial A
WHERE
	A.[Voucher_Acc_ID] = VT.[Temp_Voucher_Acc_ID]

-- Update The Transaction Status to ready to reimb (if the Transaction status = 'V' not 'S')	
UPDATE [VoucherTransaction]
SET
	[Record_Status] = 'A'
FROM [VoucherTransaction] VT, #AccounttoSpecial A
WHERE
	A.[Voucher_Acc_ID] = VT.[Temp_Voucher_Acc_ID] AND
	VT.Record_Status = 'V'

 
UPDATE [VoucherTranSuspendLOG]
SET
	[Original_Record_Status] = 'A'
FROM
	[VoucherTransaction] VT, #AccounttoSpecial A, VoucherTranSuspendLOG VTL
WHERE
	VT.[Temp_Voucher_Acc_Id] = A.[Voucher_Acc_Id] AND
	VT.[Transaction_ID] = VTL.[Transaction_ID] AND	
	VT.[Record_Status] = 'S' AND 
	VTL.[Original_Record_Status] = 'V' AND
	[System_dtm] = 
		(
			Select MAX(System_Dtm)
			FROM
				[VoucherTranSuspendLOG] VTL1
			WHERE 
				VTL1.[Transaction_ID] = VTL.[Transaction_ID] AND
				VTL1.[Record_Status] = 'S' AND
				VTL1.[Original_Record_Status] = 'V'				
		)	

  
 update TempVoucherAccount  
 set Record_Status = 'D', update_dtm = getdate()  
 where voucher_acc_id in   
 (select voucher_acc_id   
  from #AccounttoSpecial  
 )  
    
 update TempPersonalInformation   
 set Validating = 'N', update_dtm = getdate()  
 where voucher_acc_id in   
 (select voucher_acc_id   
  from #AccounttoSpecial  
 )  
    
end 
GO
