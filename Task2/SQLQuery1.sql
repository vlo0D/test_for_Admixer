/******1 Необхідно додати  в таблицю DMS унікальне ключове поле ID,  та поле KTOV зробити вторичним для звязку з таблицею TOV.
Також  в таблицю DMS можливо потрібно додати вторичний ключ NDM, та зробити його унікальним, 
для реалізації звязку один до одного між Заголовками та Строками Документів пересування товару.
******/
/******2 Одним із найпомітніших порушенням нормалізації БД є те, що поле SORT присутнє відразу в двох таблицях,
адже для зв'язку таблиць достатньо тільки вторичного ключа.
Також, таблиця DMS не має унікального поля(ID).
 
******/
/******3.1   У разі, якщо до DMS було добавлено зв'язок з DNZ (а саме поле DMS.NDM)******/
SELECT TOV.NTOV, SUM(DMS.KOL) as Count, SUM(DMS.CENA) as Sum
FROM DMS
INNER JOIN DMZ ON DMS.NDM = DMZ.NDM
INNER JOIN TOV ON DMS.KTOV = TOV.KTOV
WHERE DMZ.PR = 2, DMZ.DDM = 04.05.2014
ORDER BY Sum DESC
  /******3.2******/
  UPDATE DMS
  SET SORT = (SELECT SORT
  FROM TOV
  WHERE DMS.KTOV = TOV.KTOV)
  /******3.3 У разі, якщо до DMS було добавлено зв'язок з DNZ (а саме поле DMS.NDM)******/
  SELECT NTOV, (t1.count1 - t2.count2) AS Remainder
  FROM (SELECT COUNT(*) AS count1
	FROM DMZ
	WHERE DMZ.PR = 1) AS t1
  INNER JOIN ( SELECT COUNT(*) AS count2
	FROM DMZ
	WHERE DMZ.PR = 2) AS t2
  ON t1.NDM = t2.NDM
  INNER JOIN ( SELECT CENA
	FROM DMS) ON DMZ.NDM = DMS.NDM
INNER JOIN  (SELECT NTOV
FROM TOV) ON DMS.KTOV = TOV.KTOV
ORDER BY NTOV


  /******3.4******/

 INSERT INTO DMZ (DDM, NDM, PR)
 SELECT CURRENT_TIMESTAMP, MAX(NDM)+1, IIF(((SELECT COUNT(PR) WHERE PR = 1) > (SELECT COUNT(PR) WHERE PR = 2)), 2, 1)
 FROM DMZ)

  /******3.5******/
  DECLARE @min_ndm INT, @max_ndm INT;
SELECT @min_ndm = min(NDM), @max_ndm = max(NDM) FROM DMS;

INSERT INTO DMS (KTOV, NDM, KOL, CENA, SORT)
OUTPUT INSERTED.*
SELECT o.KTOV, NDM=@max_ndm, o.KOL, o.CENA, o.SORT
FROM DMS
WHERE o.NDM = @min_ndm
  AND NOT EXISTS (
    SELECT *
    FROM DMS i
    WHERE i.ndm = @max_ndm
      AND i.KTOV = o.KTOV
      AND i.KOL  = o.KOL
    )