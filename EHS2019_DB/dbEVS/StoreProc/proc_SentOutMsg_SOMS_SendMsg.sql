IF EXISTS (
SELECT 1 FROM Sys.Objects
WHERE [name] = 'proc_SentOutMsg_SOMS_SendMsg' AND [type] = 'P')
BEGIN
	DROP PROCEDURE proc_SentOutMsg_SOMS_SendMsg
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ==========================================================================================
-- Author:	Nick POON
-- CR No.:  CRE12-012
-- Create Date:	08 Aug 2012
-- Description:	Send out pre-defined inbox message to user
-- ==========================================================================================

CREATE PROCEDURE [dbo].[proc_SentOutMsg_SOMS_SendMsg]
	@sent_out_msg_id VARCHAR(15),
	@inbox_msg_id CHAR(12)
AS
BEGIN
-- ============================================================
-- Declaration
-- ============================================================
DECLARE @update_by VARCHAR(20)
DECLARE @RecordExist int
-- ============================================================
-- Validation
-- ============================================================
-- ============================================================
-- Initialization
-- ============================================================
SET @update_by = 'eHS'
-- ============================================================
-- Return results
-- ============================================================

-- Prechecking
SELECT @RecordExist=COUNT(1) FROM SentOutMsg_SOMS WITH (NOLOCK)
WHERE SOMS_SentOutMsg_ID=@sent_out_msg_id 
	  AND SOMS_Record_Status = 'T'

IF @RecordExist <> 1
BEGIN
	RAISERROR('00011', 16, 1)
	RETURN @@ERROR
END

-- Lock Record
UPDATE SentOutMsg_SOMS SET SOMS_Record_Status=SOMS_Record_Status 
WHERE SOMS_SentOutMsg_ID=@sent_out_msg_id 
	  AND SOMS_Record_Status = 'T'

IF @@ROWCOUNT <> 1
BEGIN
	RAISERROR ('00011', 16, 1)
	RETURN @@ERROR
END

-- Part 1: Insert a sent out message to Message table

INSERT INTO [Message]
SELECT @inbox_msg_id, 
		SOMS_SentOutMsgSubject,
		SOMS_SentOutMsgContent,
		@update_by,
		GETDATE()
FROM SentOutMsg_SOMS
WHERE SOMS_SentOutMsg_ID = @sent_out_msg_id AND SOMS_Record_Status = 'T'

-- Part 2: Add message record into Message Reader table

INSERT INTO MessageReader
SELECT @inbox_msg_id,
		sptable.SP_ID,
		'U',
		@update_by,
		GETDATE()
FROM (
	-- case 1 NA NA (All)
	SELECT DISTINCT sp.SP_ID
	FROM (
		SELECT SP_ID, 'NA' AS 'Service_Category_Code', 'NA' AS 'Scheme_Code'
		FROM ServiceProvider
		WHERE
			Record_Status IN ('A','S')
	) AS sp
	INNER JOIN (
		SELECT SOMR_SentOutMsg_ID, SOMR_Profession, SOMR_Scheme
		FROM SentOutMsgRecipient_SOMR
		WHERE
			SOMR_SentOutMsg_ID = @sent_out_msg_id
			AND SOMR_Profession = 'NA'
			AND SOMR_Scheme = 'NA'
	) AS r
	ON sp.Service_Category_Code = r.SOMR_Profession AND sp.Scheme_Code = r.SOMR_Scheme

	UNION

	-- case 2 Value NA (Profession only)
	SELECT DISTINCT sp.SP_ID
	FROM (
		SELECT SP_ID, Service_Category_Code, 'NA' AS 'Scheme_Code'
		FROM Professional
		WHERE
			Record_Status IN ('A','S')
	) AS sp
	INNER JOIN (
		SELECT SOMR_SentOutMsg_ID, SOMR_Profession, SOMR_Scheme
		FROM SentOutMsgRecipient_SOMR
		WHERE
			SOMR_SentOutMsg_ID = @sent_out_msg_id
			AND SOMR_Profession <> 'NA'
			AND SOMR_Scheme = 'NA' 
	) AS r
	ON sp.Service_Category_Code = r.SOMR_Profession

	UNION

	-- case 3 NA Value (Scheme only)
	SELECT DISTINCT sp.SP_ID
	FROM (
		SELECT SP_ID, 'NA' AS 'Service_Category_Code', Scheme_Code
		FROM SchemeInformation
		WHERE
			GETDATE() >= Effective_Dtm
			AND Record_Status IN ('A','S')
	) AS sp
	INNER JOIN (
		SELECT SOMR_SentOutMsg_ID, SOMR_Profession, SOMR_Scheme
		FROM SentOutMsgRecipient_SOMR
		WHERE
			SOMR_SentOutMsg_ID = @sent_out_msg_id
			AND SOMR_Profession = 'NA'
			AND SOMR_Scheme <> 'NA'
	) AS r
	ON sp.Scheme_Code = r.SOMR_Scheme

	UNION

	-- case 4 Value Value (Specific)
	SELECT DISTINCT sp.SP_ID
	FROM (
		SELECT DISTINCT p.SP_ID, pro.Service_Category_Code, psi.Scheme_Code
		FROM Practice p, Professional pro, PracticeSchemeInfo psi
		WHERE
			p.SP_ID = pro.SP_ID
			AND p.Professional_Seq = pro.Professional_Seq
			AND p.SP_ID = psi.SP_ID
			AND p.Display_Seq = psi.Practice_Display_Seq
			AND GETDATE() >= psi.Effective_Dtm
			AND p.Record_Status IN ('A','S')
			AND pro.Record_Status IN ('A','S')
			AND psi.Record_Status IN ('A','S')
	) AS sp
	INNER JOIN (
		SELECT SOMR_SentOutMsg_ID, SOMR_Profession, SOMR_Scheme
		FROM SentOutMsgRecipient_SOMR
		WHERE
			SOMR_SentOutMsg_ID = @sent_out_msg_id
			AND SOMR_Profession <> 'NA'
			AND SOMR_Scheme <> 'NA'
	) AS r
	ON sp.Service_Category_Code = r.SOMR_Profession AND sp.Scheme_Code = r.SOMR_Scheme		

) AS sptable

-- Part 3: Set Sent Out Message Status to S (SENT)

UPDATE SentOutMsg_SOMS
SET
	SOMS_Record_Status = 'S',
	SOMS_Sent_Dtm = GETDATE(),
	SOMS_Message_ID = @inbox_msg_id
WHERE SOMS_SentOutMsg_ID = @sent_out_msg_id

IF @@ROWCOUNT <> 1
BEGIN
	RAISERROR('00011', 16, 1)
	RETURN @@ERROR
END

END
GO

GRANT EXECUTE ON [dbo].[proc_SentOutMsg_SOMS_SendMsg] TO HCVU
GO