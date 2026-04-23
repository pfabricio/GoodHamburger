-- Adicionar coluna Quantity à tabela OrderItems
ALTER TABLE `OrderItems` ADD COLUMN `Quantity` int NOT NULL DEFAULT 1 AFTER `UnitPrice`;
