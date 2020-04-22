IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EnrolmentPrintOut_get]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EnrolmentPrintOut_get]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
 CREATE procedure [dbo].[proc_EnrolmentPrintOut_get]
	@enrolment_ref_no char(15),
	@sp_id char (8),
	@token_serial_no varchar(20),
	@address varchar(255)
 as
 
select @enrolment_ref_no Enrol_Ref_No,
	@sp_id SP_ID,
	@token_serial_no Token_Serial_No,
	@address Address,
	convert(curdate,103) Date
from SPAccountUpdaate;
GO
