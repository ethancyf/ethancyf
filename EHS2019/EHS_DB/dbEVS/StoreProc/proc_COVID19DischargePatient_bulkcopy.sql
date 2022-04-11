
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19DischargePatient_bulkcopy]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19DischargePatient_bulkcopy]
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No.:			INT22-0011 (Revise COVID19DischargeImporter to support giant amount)
-- Modified by:		Winnie SUEN
-- Modified date:	23 May 2021
-- Description:		Add "@Del" to control whether to remove records before insert
--					Add column Filler_01 to Filler_04
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (Immu record)
-- Modified by:		Raiman Chong
-- Modified date:	11 May 2021
-- Description:		insert record for COVID19DischargePatient and COVID19DischargePatientLog Table
-- =============================================

CREATE PROCEDURE [dbo].[proc_COVID19DischargePatient_bulkcopy]	@PatientTable COVID19DischargePatientTableType READONLY,
																@Del BIT
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
        exec [proc_SymmetricKey_open]
        -- =============================================
        -- Return results
        -- =============================================
        --Clear the record in HAServicePatient table
		IF @Del = 1
		BEGIN
			DELETE FROM dbo.COVID19DischargePatient;
		END

        INSERT INTO dbo.COVID19DischargePatient
               ([Row_No],
				[CHP_Index_No], 
                [Encrypt_Field1], 
                [Encrypt_Field2_1], 
                [Encrypt_Field2_2], 
				[Encrypt_Field12],
                [SEX], 
                [Phone1_No], 
                [Phone2_No], 
                [Phone3_No], 
				[DOB_Format],
				[DOB],
				[Discharge_Date],
				[File_ID],
				[Import_Remark],
                Create_Dtm,
				[Infection_Date],
				[Recovery_Date],
				[Death_Indicator],
				[DataSource]
               )
        SELECT [Row_No],
			   [CHP_Index_No], 
			   ENCRYPTBYKEY(KEY_GUID('sym_Key'), [HKID]), 
               ENCRYPTBYKEY(KEY_GUID('sym_Key'), [Surname_eng]), 
               ENCRYPTBYKEY(KEY_GUID('sym_Key'), [Given_name_eng]),                
			   ENCRYPTBYKEY(KEY_GUID('sym_Key'), [Passport_No]), 
               [Sex], 
               [Phone1_No], 
               [Phone2_No], 
               [Phone3_No], 
			   [DOB_Format],
			   [DOB],
			   [Discharge_Date],
			   [File_ID],
			   [Import_Remark],
               GETDATE(),
			   [Infection_Date],
		       [Recovery_Date],
			   [Death_Indicator],
			   [Data_Source]
        FROM @PatientTable;


       exec [proc_SymmetricKey_close]
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19DischargePatient_bulkcopy] TO HCVU;
GO