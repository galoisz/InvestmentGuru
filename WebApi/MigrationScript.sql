CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Protfolios" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Budget" numeric(18,2) NOT NULL,
    CONSTRAINT "PK_Protfolios" PRIMARY KEY ("Id")
);

CREATE TABLE "Stocks" (
    "Id" uuid NOT NULL,
    "Symbol" character varying(10) NOT NULL,
    "Name" character varying(255) NOT NULL,
    "MinPriceDate" timestamp with time zone,
    "MaxPriceDate" timestamp with time zone,
    CONSTRAINT "PK_Stocks" PRIMARY KEY ("Id")
);

CREATE TABLE "ProtfolioPeriods" (
    "Id" uuid NOT NULL,
    "ProtfolioId" uuid NOT NULL,
    "FromDate" date NOT NULL,
    "ToDate" date NOT NULL,
    CONSTRAINT "PK_ProtfolioPeriods" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ProtfolioPeriods_Protfolios_ProtfolioId" FOREIGN KEY ("ProtfolioId") REFERENCES "Protfolios" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Prices" (
    "Id" uuid NOT NULL,
    "StockId" uuid NOT NULL,
    "Value" text NOT NULL,
    CONSTRAINT "PK_Prices" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Prices_Stocks_StockId" FOREIGN KEY ("StockId") REFERENCES "Stocks" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ProtfolioStocks" (
    "Id" uuid NOT NULL,
    "ProtfolioId" uuid NOT NULL,
    "StockId" uuid NOT NULL,
    "Ratio" numeric(18,4) NOT NULL,
    CONSTRAINT "PK_ProtfolioStocks" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ProtfolioStocks_Protfolios_ProtfolioId" FOREIGN KEY ("ProtfolioId") REFERENCES "Protfolios" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ProtfolioStocks_Stocks_StockId" FOREIGN KEY ("StockId") REFERENCES "Stocks" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Prices_StockId" ON "Prices" ("StockId");

CREATE INDEX "IX_ProtfolioPeriods_ProtfolioId" ON "ProtfolioPeriods" ("ProtfolioId");

CREATE INDEX "IX_ProtfolioStocks_ProtfolioId" ON "ProtfolioStocks" ("ProtfolioId");

CREATE INDEX "IX_ProtfolioStocks_StockId" ON "ProtfolioStocks" ("StockId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241117163603_InitialCreate', '8.0.10');

COMMIT;

