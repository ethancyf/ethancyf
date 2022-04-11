-- =============================================
-- Modification History
-- CR No.:			CRE20-023-XXX (Immu record)
-- Modified by:		Chris YIM
-- Modified date:	28 Mar 2022
-- Description:		Add 4 new columns
-- =============================================
-- =============================================
-- Author
-- CR No.:			CRE20-023 (Immu record)
-- Created by:		Raiman Chong
-- Creation date:	11 May 2021
-- Description:		TableType for COVID19DischargeImporter
-- =============================================

IF TYPE_ID('[dbo].[COVID19DischargePatientTableType]') IS NOT NULL
    BEGIN
        DROP TYPE [dbo].[COVID19DischargePatientTableType];
    END;
GO

CREATE TYPE COVID19DischargePatientTableType AS TABLE
(	[CHP_Index_No]		[VARCHAR](255)		NOT NULL,
	[Surname_eng]		[VARCHAR](150)		NOT NULL,
	[Given_name_eng]	[VARCHAR](150)		NOT NULL,
	[HKID]				[VARCHAR](70)		NOT NULL,
	[Passport_No]		[VARCHAR](170)		NOT NULL,
	[Sex]				[VARCHAR](1)		NOT NULL,
	[Phone1_No]			[NVARCHAR](30)		NOT NULL,
	[Phone2_No]			[NVARCHAR](30)		NOT NULL,
	[Phone3_No]			[NVARCHAR](30)		NOT NULL,
	[DOB_Format]		[VARCHAR](4)		NOT NULL,
	[DOB]				[DateTime]			NULL,
	[Discharge_Date]	[DateTime]			NULL,
	[Infection_Date]	[DateTime]			NULL,
	[Recovery_Date]		[DateTime]			NULL,
	[Death_Indicator]	[VARCHAR](255)		NULL,
	[Data_Source]		[VARCHAR](255)		NULL,
	[File_ID]			[VARCHAR](100)		NOT NULL,
	[Import_Remark]		[NVARCHAR](MAX)		NOT NULL,
	[Row_No]			[INT]				NOT NULL
);

GRANT EXEC ON TYPE ::[dbo].[COVID19DischargePatientTableType] TO [HCVU];
GO

