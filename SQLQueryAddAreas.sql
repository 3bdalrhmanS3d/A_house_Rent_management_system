-- 1. Turn on the explicit value insertion mode

SET IDENTITY_INSERT dbo.Areas ON;

-- 2. Add regions while manually selecting the Id

INSERT INTO dbo.Areas (Id, AreaName, SurroundingArea)
VALUES
  (1,  'Region A', 'Surrounding A'),
  (2,  'Region B', 'Surrounding B'),
  (3,  'Region C', 'Surrounding C'),
  (4,  'Region D', 'Surrounding D'),
  (5,  'Region E', 'Surrounding E'),
  (6,  'Region F', 'Surrounding F'),
  (7,  'Region G', 'Surrounding G'),
  (8,  'Region H', 'Surrounding H'),
  (9,  'Region I', 'Surrounding I'),
  (10, 'Region J', 'Surrounding J'),
  (11, 'Region K', 'Surrounding K'),
  (12, 'Region L', 'Surrounding L'),
  (13, 'Region M', 'Surrounding M'),
  (14, 'Region N', 'Surrounding N'),
  (15, 'Region O', 'Surrounding O');

-- 3. Stop explicit insertion mode

SET IDENTITY_INSERT dbo.Areas OFF;
