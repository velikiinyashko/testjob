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
[r].[CountStocks],
COUNT(p.PartyId) AS CountParty,
ROUND(ISNULL(SUM(p.Price), 0),2) AS SumPrice
FROM
(
    SELECT 
        [r].[RetailId],
        [r].[Name],
        [r].[City],
        [r].[Address],
        [r].[Phone],
        COUNT(s.StockId) AS CountStocks
    FROM retail AS r
    LEFT JOIN Stock AS s on r.RetailId = s.RetailId
    GROUP BY
        [r].[RetailId],
        [r].[Name],
        [r].[City],
        [r].[Address],
        [r].[Phone]
) AS r
LEFT JOIN Party AS p ON r.RetailId = p.RetailId 
GROUP BY 
[r].[RetailId],
[r].[Name],
[r].[City],
[r].[Address],
[r].[Phone],
[r].[CountStocks]
