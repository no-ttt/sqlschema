CREATE PROCEDURE xp_insertArchive  -- 插入 Archive
	@FileName nvarchar(255),  -- 檔案名稱 
	@FileExtension nvarchar(100),
	@ContentType nvarchar(100),
	@ContentLen int,
	@MID int,
	@NewOID int output,
	@NewUUID varchar(100) output
as
begin 
	BEGIN TRY  
		BEGIN TRAN insertArchive
           

			declare @CTID int = (select CTID from ContentType  where Title =@ContentType) 
			if(@CTID is null) 
			begin 
				insert into ContentType (Title)  values (@ContentType)
				set @CTID =SCOPE_IDENTITY() 
			end
			insert into Object (Type,CName,OwnerMID) values (7,@FileName,@MID) 
			set @NewOID =SCOPE_IDENTITY() 
			set @NewUUID= NEWID()
			insert into Archive (AID, FileName, FileExtension, ContentType,ContentLen, MD5,UUID) values (@NewOID,@FileName, @FileExtension, @CTID, @ContentLen,hashbytes('MD5',CONVERT(varchar(10),@NewOID)+CONVERT(varchar(10),@CTID)+@FileName),@NewUUID)
		COMMIT TRAN insertArchive
	END TRY  
	BEGIN CATCH  
		ROLLBACK TRAN insertArchive
		DECLARE @ErrorMessage As VARCHAR(1000) = CHAR(10)+'錯誤代碼：' +CAST(ERROR_NUMBER() AS VARCHAR)
													+CHAR(10)+'錯誤訊息：'+	ERROR_MESSAGE()
													+CHAR(10)+'錯誤行號：'+	CAST(ERROR_LINE() AS VARCHAR)
													+CHAR(10)+'錯誤程序名稱：'+	ISNULL(ERROR_PROCEDURE(),'')
		DECLARE @ErrorSeverity As Numeric = ERROR_SEVERITY()
		DECLARE @ErrorState As Numeric = ERROR_STATE()
		RAISERROR( @ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH;  
end