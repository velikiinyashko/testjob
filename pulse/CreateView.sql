-- Create the view in the specified schema
CREATE OR ALTER VIEW dbo.v_retail
AS
    -- body of the view
SELECT 
[r].[RetailId],
[r].[Name],
[r].[City],
[r].[Address],
[r].[Phone],
COUNT(s.StockId) AS CountStocks,
COUNT(p.PartyId) AS CountParty,
ISNULL(SUM(p.Price), 0) AS SumPrice
FROM retail AS r
LEFT JOIN Stock AS s on r.RetailId = s.RetailId
LEFT JOIN Party AS p on s.StockId = p.StockId
GROUP BY
[r].[RetailId],
[r].[Name],
[r].[City],
[r].[Address],
[r].[Phone]
