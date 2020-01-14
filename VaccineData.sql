CREATE TABLE [VaccineData] (
	[Id] integer COLLATE BINARY NOT NULL PRIMARY KEY AUTOINCREMENT, 
	[VaccineCode] text COLLATE BINARY, 
	[VaccineNo] text COLLATE BINARY, 
	[VaccBatchNo] text COLLATE BINARY, 
	[ChName] text COLLATE BINARY, 
	[BatchType] integer COLLATE BINARY
)
