DROP DATABASE IF EXISTS "Keyzerov";
CREATE DATABASE "Keyzerov" WITH OWNER = postgres ENCODING = 'UTF8';

\c "Keyzerov"

DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_catalog.pg_roles WHERE rolname = 'app') THEN
        CREATE ROLE app WITH LOGIN PASSWORD '123456789';
    ELSE
        ALTER ROLE app WITH LOGIN PASSWORD '123456789';
    END IF;
END
$$;

DROP SCHEMA IF EXISTS app CASCADE;
CREATE SCHEMA app AUTHORIZATION app;

CREATE TABLE app.partner_types_keyzerov (
    id SERIAL PRIMARY KEY,
    type_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE app.partners_keyzerov (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    type_id INTEGER NOT NULL REFERENCES app.partner_types_keyzerov(id) ON DELETE RESTRICT,
    inn VARCHAR(20),
    logo BYTEA,
    rating INTEGER NOT NULL DEFAULT 0 CHECK (rating >= 0),
    address VARCHAR(500),
    director_name VARCHAR(200),
    phone VARCHAR(50),
    email VARCHAR(100),
    sales_places VARCHAR(500)
);

CREATE TABLE app.sales_history_keyzerov (
    id SERIAL PRIMARY KEY,
    partner_id INTEGER NOT NULL REFERENCES app.partners_keyzerov(id) ON DELETE CASCADE,
    product_name VARCHAR(200) NOT NULL,
    quantity INTEGER NOT NULL CHECK (quantity > 0),
    sale_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_sales_partner_keyzerov ON app.sales_history_keyzerov(partner_id);
CREATE INDEX idx_partners_type_keyzerov ON app.partners_keyzerov(type_id);

GRANT USAGE ON SCHEMA app TO app;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA app TO app;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA app TO app;

INSERT INTO app.partner_types_keyzerov (type_name) VALUES 
('Retail Store'),
('Wholesale Store'),
('Online Shop'),
('Corporate Client');

INSERT INTO app.partners_keyzerov (name, type_id, inn, rating, address, director_name, phone, email, sales_places) VALUES
('IP Smirnov', 1, '590123456789', 5, 'Perm, Lenina str 10', 'Smirnov A.V.', '+7 342 111 22 33', 'smirnov@mail.ru', 'Perm'),
('OOO Torgovy Dom Stroymaster', 2, '590234567890', 10, 'Moscow, Stroiteley str 25', 'Ivanov I.I.', '+7 495 222 33 44', 'info@stroymaster.ru', 'Moscow, SPB'),
('ZAO Severnaya Kompaniya', 2, '590345678901', 15, 'Ekaterinburg, Lenina pr 50', 'Petrov P.P.', '+7 343 333 44 55', 'petrov@north.ru', 'Ekaterinburg, Chelyabinsk'),
('AO Federalnaya Set', 4, '590456789012', 20, 'Moscow, Kutuzovsky pr 100', 'Sidorov S.S.', '+7 495 444 55 66', 'federal@stroyset.ru', 'Russia'),
('OOO Granitsa', 1, '590567890123', 8, 'Kazan, Baumana str 15', 'Kozlov K.K.', '+7 843 555 66 77', 'granitsa@mail.ru', 'Kazan'),
('IP Granitsyn', 3, '590678901234', 12, 'Novosibirsk, Marksa pr 30', 'Granitsyn G.G.', '+7 383 666 77 88', 'granitsyn@sib.ru', 'Novosibirsk, Omsk'),
('OOO Maximum Plus', 4, '590789012345', 18, 'Sochi, Navaginskaya str 5', 'Maximov M.M.', '+7 862 777 88 99', 'maximum@sochi.ru', 'Sochi, Krasnodar'),
('OOO Online Shop', 3, '590890123456', 7, 'Moscow, Online', 'Vebov V.V.', '+7 495 888 99 00', 'shop@online.ru', 'Internet');

INSERT INTO app.sales_history_keyzerov (partner_id, product_name, quantity, sale_date) VALUES
(1, 'Laminate Oak Light', 2000, '2025-01-10'),
(1, 'Laminate Oak Dark', 1500, '2025-01-20'),
(1, 'PVC Baseboard White', 1500, '2025-02-05'),
(2, 'Laminate 33 Natural', 10000, '2025-01-15'),
(2, 'Parquet Board Ash', 8000, '2025-02-10'),
(2, 'Ceramic Tile 30x30', 7000, '2025-03-01'),
(3, 'Laminate Wholesale Select', 50000, '2025-01-05'),
(3, 'Parquet Bulk Beech', 60000, '2025-02-15'),
(3, 'Floor Tile 40x40', 40000, '2025-03-20'),
(4, 'Laminate Federal 33', 200000, '2025-01-01'),
(4, 'Parquet Elite Oak', 150000, '2025-02-01'),
(4, 'Tile Premium 60x60', 150000, '2025-03-01'),
(5, 'Laminate Border 32', 5000, '2025-01-25'),
(5, 'Baseboard Border PVC', 5000, '2025-02-25'),
(6, 'Laminate Siberia 33', 25000, '2025-01-30'),
(6, 'Parquet Siberia Larch', 25000, '2025-03-10'),
(7, 'Laminate Maximum 33', 100000, '2025-01-08'),
(7, 'Parquet Maximum Oak', 100000, '2025-02-18'),
(7, 'Tile Maximum 50x50', 100000, '2025-03-28'),
(8, 'Laminate Online 32', 4999, '2025-02-01'),
(8, 'Baseboard Online PVC', 5000, '2025-03-05');