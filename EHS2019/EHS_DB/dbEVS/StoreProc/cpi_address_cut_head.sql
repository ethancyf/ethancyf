IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[cpi_address_cut_head]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[cpi_address_cut_head]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[cpi_address_cut_head] (@in_name varchar(255), @out_name varchar(255) output  )
AS
begin
	declare @record_type varchar(50), @string_pos int, @space_pos int
	declare @temp_record_type varchar(50)
	declare @temp_in_name varchar(255)
	select @in_name = upper(@in_name)
/*****   24 Nov 2003: Prevent 'Null' string Concaterated with Char String   ****/
	-- select @out_name = null
	select @out_name = ''
	select @temp_in_name = @in_name
   while charindex('.', rtrim(@temp_in_name)) != 0
   select @temp_in_name = 
      stuff(@temp_in_name, charindex('.', @temp_in_name), 1, space(1))
	select @in_name = @temp_in_name
	select @temp_in_name = @in_name
   while charindex(',', rtrim(@temp_in_name)) != 0
   select @temp_in_name = 
      stuff(@temp_in_name, charindex(',', @temp_in_name), 1, space(1))
	select @in_name = @temp_in_name
	select @temp_in_name = @in_name
   while charindex(';', rtrim(@temp_in_name)) != 0
   select @temp_in_name = 
      stuff(@temp_in_name, charindex(';', @temp_in_name), 1, space(1))
	select @in_name = @temp_in_name 
   while  @in_name is not null
   begin
		select @record_type = null
		select @string_pos = 0
		select @string_pos = charindex(' ', ltrim(rtrim(@in_name)))
      -- Cut the last word for record type checking
		if @string_pos > 1
		begin
			select @record_type = substring(ltrim(rtrim(@in_name)), 1, @string_pos-1)
			select @in_name = substring(ltrim(rtrim(@in_name)), @string_pos+1, len(ltrim(rtrim(@in_name))))
		end
		else
		begin
			select @record_type = @in_name
			select @in_name = null
		end
		/* check if record_type contains only one character skip it */
		if len(@record_type) = 1
			continue
		if substring(@record_type , 1, 1) in (',','1', '2','3','4','5','6','7','8','9','0')
			continue
		else 
		if exists (select * from address_type 
				  where full_name = @record_type or abbreviation = @record_type)
		begin
			--  25 Nov 2005:   if @out_name = NULL
			if @out_name = ''
			     continue
			else
				break
		end
		else
		if substring(@record_type , 2, 1) in (',','1', '2','3','4','5','6','7','8','9','0')
			continue
		select @out_name = @out_name + @record_type +space(1)
   end
	select @out_name = rtrim(@out_name)
end
GO

GRANT EXECUTE ON [dbo].[cpi_address_cut_head] TO HCPUBLIC
GO

GRANT EXECUTE ON [dbo].[cpi_address_cut_head] TO HCSP
GO

GRANT EXECUTE ON [dbo].[cpi_address_cut_head] TO HCVU
GO
