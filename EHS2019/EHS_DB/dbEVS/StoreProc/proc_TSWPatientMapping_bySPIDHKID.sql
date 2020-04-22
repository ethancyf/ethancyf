IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_TSWPatientMapping_bySPIDHKID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_TSWPatientMapping_bySPIDHKID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Timothy LEUNG
-- Create date: 11 August 2008
-- Description:	Retrieve the TSW Patient Mapping record
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Derek LEUNG
-- Modified date:	28 July 2010
-- Description:		Grant execute permission to HKVU
-- =============================================

CREATE PROCEDURE proc_TSWPatientMapping_bySPIDHKID 
	@GP_SPID char(8),
	@HKID char(9)
AS
BEGIN
	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	declare @Read_Transit char(1)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SELECT @Read_Transit = Parm_Value1
	from SystemParameters
	where [Parameter_Name] = 'ReadTSWTransit' AND
		[Record_Status] = 'A' AND [Scheme_Code] = 'ALL'
	
-- =============================================
-- Return results
-- =============================================
	OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key

	if @Read_Transit = 'Y' 
	begin
		select count(1) as [rowcount] from
		TSWPatientMappingTransition
		where 
		GP_SPID = @GP_SPID  and  
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID)
	end 
	else
	begin
		select count(1) as [rowcount] from
		TSWPatientMapping
		where 
		GP_SPID = @GP_SPID  and  
		Encrypt_Field1 = EncryptByKey(KEY_GUID('sym_Key'), @HKID)
	end

	CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_TSWPatientMapping_bySPIDHKID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_TSWPatientMapping_bySPIDHKID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_TSWPatientMapping_bySPIDHKID] TO WSEXT
GO