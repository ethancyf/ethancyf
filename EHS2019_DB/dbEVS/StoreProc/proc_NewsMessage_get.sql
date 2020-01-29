IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_NewsMessage_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_NewsMessage_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	9 December 2014
-- CR No.:			CRE13-019-01
-- Description:		Support Simplified Chinese
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   07 JAN 2009
-- Description:	    Add back the platform in the search criteria
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark YIP
-- Modified date:   25 MAR 2009
-- Description:	    The effective and expiry date comparison is up to date and time
-- =============================================
CREATE Procedure dbo.proc_NewsMessage_get	@platform	char(2)
as

select 	Msg_ID
		,Description
		,Chinese_Description
		,CN_Description
		,Create_Dtm
		,Effective_Dtm
		,Expiry_Dtm
   from NewsMessage
		 where effective_dtm <=  getdate() and expiry_dtm > getdate()
       --where convert(varchar(10), effective_dtm,102) <=  convert(varchar(10), getdate(),102)
       --    and convert(varchar(10), expiry_dtm,102) >  convert(varchar(10), getdate(),102)
           and (Platform = '99' or Platform = @platform)
   order by create_dtm desc

GO

GRANT EXECUTE ON [dbo].[proc_NewsMessage_get] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_NewsMessage_get] TO HCVU
GO
