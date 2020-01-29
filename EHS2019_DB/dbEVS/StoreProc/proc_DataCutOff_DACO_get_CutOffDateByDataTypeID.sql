IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_DataCutOff_DACO_get_CutOffDateByDataTypeID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_DataCutOff_DACO_get_CutOffDateByDataTypeID
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE16-002
-- Modified by:		Lawrence TSANG
-- Modified date:	16 August 2016
-- Description:		Grant EXECUTE to HCPUBLIC
-- =============================================
-- ==========================================================================================
-- Author:	Nick POON
-- Create Date:	17 Dec 2012
-- Description:	Get cut off date by datatype ID - [DataCutOff_DACO]
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_DataCutOff_DACO_get_CutOffDateByDataTypeID]
	@datatype_id varchar(20)
AS
BEGIN
-- ============================================================
-- Declaration
-- ============================================================
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
-- ============================================================
-- Return results
-- ============================================================
SELECT
	DACO_DataType_ID,
	DACO_CutOff_Dtm
FROM
	DataCutOff_DACO
WHERE
	DACO_DataType_ID = @datatype_id
	
END
GO

GRANT EXECUTE ON [dbo].[proc_DataCutOff_DACO_get_CutOffDateByDataTypeID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_DataCutOff_DACO_get_CutOffDateByDataTypeID] TO HCPUBLIC
GO
 