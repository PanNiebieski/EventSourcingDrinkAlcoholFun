BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Events" (
	"Id"	INTEGER NOT NULL UNIQUE,
	"Key_StreamId"	TEXT NOT NULL,
	"AssemblyQualifiedName_Type"	TEXT NOT NULL,
	"Value_Data"	TEXT NOT NULL,
	"Version_SerialNumber"	INTEGER,
	"TimeStamp"	TEXT,
	PRIMARY KEY("Id" AUTOINCREMENT)
);
COMMIT;
