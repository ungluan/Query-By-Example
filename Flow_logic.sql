-- Lấy danh sách DatabaseName
SELECT database_id,name FROM sys.databases
WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb', 'distribution');

-- Lấy danh sách Table của database
use YENSAO
select Name as Table_Name, object_id as Table_Id from sys.tables
WHERE name != 'sysdiagrams'

select * from INFORMATION_SCHEMA.TABLES
-- Giả xử chọn bảng Orders 
-- Mục đích là lấy danh sách khóa ngoại của bảng Orders 

select object_id as Table_Id from sys.tables
WHERE name = 'Products' 
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='dbName'
-- Table có chứa khóa chính<Name, location> và Danh sách khóa ngoại
-- Lấy danh sách khóa chính gồm ColumnId và ColumnName
select referenced_column_id as ColumnId, (SELECT COL_NAME(930102354, referenced_column_id)) as ColumnName  
from sys.foreign_key_columns
WHERE referenced_object_id = 930102354
-- Lấy danh sách khóa ngoại gồm: TableId, TableName, danh sách khóa chính của table
select referenced_object_id as TableId,
(select name from sys.tables WHERE object_id = referenced_object_id) as TableName,
referenced_column_id as ColumnId, 
(SELECT COL_NAME(parent_object_id, referenced_column_id)) as ColumnName  
from sys.foreign_key_columns
WHERE parent_object_id in (select object_id from sys.tables)

-- OrderDetails 786101841 
-- Products 482100758
SELECT
'TableName.'+
(SELECT COL_NAME(referenced_object_id,referenced_column_id)) as ColumnPrimaryKey,
'TableName.'+(SELECT COL_NAME(parent_object_id, parent_column_id)) as ColumnForeignKey
from sys.foreign_key_columns
where (parent_object_id = 786101841  AND referenced_object_id =  482100758 )


select * from sys.foreign_key_columns
SELECT Column_Name
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = N'Customers'
-- Xử lý logic:
/*
	1. Load trang web thì xổ ra dropdown button data: danh sách database
	2. Nếu: ds db rỗng -> Hiển thị "Danh sách database rỗng"
	Mặc định: Load danh sách database đầu tiên và Danh sách tablename
	Click: Khi click thay đổi dsdb thì ta query get ds table
	3. Khi click vào table thì hiển thị table, hiển thị list action:
	- Chọn field tham gia báo cáo.
	- Chọn table_Name
	- select/COUNT, SUM, MIN, MAX, AVG
	- Chọn Where: ?? Nhập giá trị tại đây, làm sao để biết kiểu dữ liệu
	của mệnh đề where này để khi query cho đúng.
	- Chọn Đổi tên
	- Group by
	- Having
*/

-- Đang xử lý tại đây. lấy mã object_id và id cột của cha và con
-- Từ đó ta lấy được tên cột để join --> TableCha.ColName == TableCon.ColCon
select * from sys.foreign_key_columns
where parent_object_id =(SELECT object_id from sys.tables where Name = 'Orders') AND referenced_column_id in (select object_id as Table_Id from sys.tables
WHERE NAME = 'Orders'or NAME = 'Products' or Name = 'OrderDetails')
---- > Kết thúc xử lý join
-- Xử lý select này nọ