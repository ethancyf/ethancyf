IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TempVoucherAccountDeletionList_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TempVoucherAccountDeletionList_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- =============================================  
-- Modification History      
-- CR No.:		   CRE12-014  
-- Modified by:    Karl LAM   
-- Modified date:  03 Jan 2013  
-- Description:    Add parameters: @result_limit_1st_enable,@result_limit_override_enable, @override_result_limit  for relax 500 rows limitation  
-- =============================================      
-- =============================================   
-- Modification History
-- Modified by:	    Paul YIP
-- Modified date:	20 July 2010
-- Description:		Add Create_by_bo col
-- =============================================
-- =============================================
-- Author:		Clark YIP
-- Create date: 2008 10 15
-- Description:	Retrieve the fail validated Temp Voucher Account list for Deletion
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:	01 Jan 2009
-- Description:		Add Create_by col

-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Kathy LEE
-- Modified date:	14 Sep 2009
-- Description:		Remove Scheme_Code in the search route
-- =============================================

--exec proc_TempVoucherAccountDeletionList_get null,'2010-01-01','2012-12-31', 1,1,0

CREATE PROCEDURE [dbo].[proc_TempVoucherAccountDeletionList_get]
	-- Add the parameters for the stored procedure here
	@Eng_Name varchar(40),
	@From_Date datetime,
	@To_Date datetime,
	@result_limit_1st_enable BIT, 
	@result_limit_override_enable BIT,
	@override_result_limit BIT

AS
BEGIN

-- =============================================
-- Declaration
-- =============================================

DECLARE @day_level int
SELECT @day_level=convert(int, Parm_Value1) from SystemParameters where Parameter_Name='Alert_L5_OutstandingDay'	--28day

DECLARE @rowcount INT
DECLARE @row_cnt_error varchar(max)

DECLARE @tmp_VR_Delete table ( Voucher_Acc_ID char(15),
						Scheme_Code char(10),
						Account_Status char(1),
						Remark nvarchar(255),
						Public_Enquiry_Status char(1),
						Public_Enq_Status_Remark nvarchar(255),
						IdentityNum varchar(20),
						EName varchar(50),
						CName nvarchar(50),
						CCcode1 char(5),
						CCcode2 char(5),
						CCcode3 char(5),
						CCcode4 char(5),
						CCcode5 char(5),
						CCcode6 char(5),
						DOB datetime,
						Exact_DOB char(1),
						Sex char(1),
						Date_of_Issue datetime,
						PersonalInformation_Status char(1),
						Create_Dtm datetime,
						AccountType char(1),
						SP_ID varchar(50),
						SP_Practice_Display_Seq smallint,
						HKID_Card char(1),
						EC_Age smallint,
						EC_Date_of_Registration datetime,
						Create_by varchar(20),
						Adoption_Prefix_Num char(7),
						Doc_Code char(20),
						Doc_Display_Code varchar(20),
						Source char(1),
						Other_Info varchar(10),
						Create_By_BO char(1))

-- =============================================
-- Initialization
-- =============================================
	SET NOCOUNT ON;

	OPEN SYMMETRIC KEY sym_Key 
		DECRYPTION BY ASYMMETRIC KEY asym_Key

	insert into @tmp_VR_Delete ( Voucher_Acc_ID ,
							Scheme_Code,
							Account_Status,
							Remark,
							Public_Enquiry_Status,
							Public_Enq_Status_Remark,
							IdentityNum,
							EName,
							CName,
							CCcode1,
							CCcode2,
							CCcode3,
							CCcode4,
							CCcode5,
							CCcode6,
							DOB,
							Exact_DOB,
							Sex,
							Date_of_Issue,
							PersonalInformation_Status,
							Create_Dtm,
							AccountType,
							SP_ID,
							SP_Practice_Display_Seq,
							EC_Age,
							EC_Date_of_Registration,
							Create_by,
							Adoption_Prefix_Num,
							Doc_Code,
							Doc_Display_Code,
							Source,
							Other_Info,
							Create_By_BO)
	SELECT TOP ([dbo].[func_get_top_row](@result_limit_1st_enable,@result_limit_override_enable)) 
		VA.Voucher_Acc_ID, 
		VA.Scheme_Code, 
		VA.Record_Status as Account_Status,
		--VA.Remark as Remark,
		--VA.Public_Enquiry_Status as Public_Enquiry_Status,
		--VA.Public_Enq_Status_Remark as Public_Enq_Status_Remark,
		'' as Remark,
		'A' as Public_Enquiry_Status,
		'' as Public_Enq_Status_Remark,
		convert(varchar, DecryptByKey(P.Encrypt_Field1)),
		convert(varchar, DecryptByKey(P.Encrypt_Field2)),
		convert(nvarchar, DecryptByKey(P.Encrypt_Field3)),
		convert(varchar, DecryptByKey(P.Encrypt_Field4)),
		convert(varchar, DecryptByKey(P.Encrypt_Field5)),
		convert(varchar, DecryptByKey(P.Encrypt_Field6)),
		convert(varchar, DecryptByKey(P.Encrypt_Field7)),
		convert(varchar, DecryptByKey(P.Encrypt_Field8)),
		convert(varchar, DecryptByKey(P.Encrypt_Field9)),
		P.DOB,
		P.Exact_DOB,
		P.Sex,
		P.Date_of_Issue,
		P.Record_Status as PersonalInformation_Status,
		VA.Create_Dtm,
		'T' as AccountType,	
		C.SP_ID,
		C.SP_practice_display_seq,
		P.EC_Age,
		P.EC_Date_of_Registration,
		--c.sp_id + '('+ convert(varchar,C.SP_Practice_Display_Seq) +')',
		C.Create_By,
		isnull(convert(char, DecryptByKey(P.Encrypt_Field11)),''),
		P.Doc_Code,
		DT.Doc_Display_Code,
		'T',
		P.Other_Info,
		C.Create_By_BO
		from TempVoucherAccount VA, TempPersonalInformation P, VoucherAccountCreationLOG C, TempVoucherAccPendingVerify PV, DocType DT
		where 
		VA.Voucher_Acc_ID = P.Voucher_Acc_ID 
		and P.Voucher_Acc_ID = C.Voucher_Acc_ID and VA.Voucher_Acc_ID = C.Voucher_Acc_ID
		and VA.Voucher_Acc_ID = PV.Voucher_Acc_ID	
		and VA.Record_Status='I' and (VA.Account_Purpose='C' or VA.Account_Purpose='V')
		and P.Doc_Code = DT.Doc_Code
		and DATEDIFF(Day, PV.First_Validate_Dtm, getdate()) > @day_level	
		and (@Eng_Name is null or P.Encrypt_Field2 = EncryptByKey(KEY_GUID('sym_Key'), @Eng_Name))
		and (VA.Create_Dtm >= @From_Date or @From_Date is null)
		and (VA.Create_Dtm < DateAdd(Day,1,@To_Date) or @To_Date is null)
		order by convert(varchar, DecryptByKey(P.Encrypt_Field1))
		

	-- =============================================    
	-- Max Row Checking  
	-- =============================================  
	BEGIN TRY       
			SELECT	@rowcount = COUNT(1)
			FROM	@tmp_VR_Delete

		EXEC proc_CheckFeatureResultRowLimit @row_count=@rowcount, @result_limit_1st_enable=@result_limit_1st_enable, @result_limit_override_enable=@result_limit_override_enable,@override_result_limit=@override_result_limit     
	END TRY

	BEGIN CATCH    	    
		SET @row_cnt_error = ERROR_MESSAGE()    

		RAISERROR (@row_cnt_error,16,1)    
		CLOSE SYMMETRIC KEY sym_Key  
		RETURN
	END CATCH 


	CLOSE SYMMETRIC KEY sym_Key

-- =============================================
-- Return results
-- =============================================
	select tvr.*, t.transaction_id as trans_id from @tmp_VR_Delete tvr left join vouchertransaction t on tvr.voucher_acc_id=t.temp_voucher_acc_id	
END
GO

GRANT EXECUTE ON [dbo].[proc_TempVoucherAccountDeletionList_get] TO HCVU
GO
