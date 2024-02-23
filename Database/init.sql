CREATE TABLE IF NOT EXISTS clothes (
  id UUID PRIMARY KEY,
  size TEXT NOT NULL,
  name TEXT NOT NULL,
  color TEXT,
  brand TEXT,
  price REAL NOT NULL,
  tags TEXT[],
  description TEXT,
  "type" CHAR NOT NULL,
  quantity INTEGER NOT NULL,
  gender CHAR
);

CREATE TABLE IF NOT EXISTS "users" (
  id UUID PRIMARY KEY,
  name TEXT NOT NULL,
  email TEXT NOT NULL,
  password TEXT NOT NULL,
  address TEXT NOT NULL 
);

CREATE TABLE IF NOT EXISTS "orders" (
  id UUID PRIMARY KEY,
  timeofsale TIMESTAMP NOT NULL,
  paymethod CHAR NOT NULL,
  orderaddress TEXT NOT NULL,
  lastchanged TIMESTAMP NOT NULL,
  user_id UUID NOT NULL,
  CONSTRAINT fk_user 
    FOREIGN KEY(user_id) REFERENCES "users"(id)
);

CREATE TABLE IF NOT EXISTS orderclothes (
  id UUID PRIMARY KEY,
  totalclothingquantity INTEGER NOT NULL,
  orders_id UUID NOT NULL,
  clothes_id UUID NOT NULL,
  CONSTRAINT fk_clothes
    FOREIGN KEY(clothes_id) REFERENCES clothes(id),
  CONSTRAINT fk_orders
    FOREIGN KEY(orders_id) REFERENCES "orders"(id)
);

CREATE OR REPLACE FUNCTION atualiza_quantidade() RETURNS TRIGGER AS 
$$
BEGIN
  UPDATE clothes AS c
    SET quantity = 
      CASE WHEN TG_OP = 'UPDATE' THEN
        CASE 
          WHEN OLD.totalclothingquantity > NEW.totalclothingquantity 
          THEN c.quantity + OLD.totalclothingquantity - NEW.totalclothingquantity
          WHEN OLD.totalclothingquantity < NEW.totalclothingquantity
          THEN c.quantity - NEW.totalclothingquantity 
          ELSE c.quantity
        END
        WHEN TG_OP = 'INSERT' THEN c.quantity - NEW.totalclothingquantity
      END;
  RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tg_atualiza_quantidade
  AFTER INSERT OR UPDATE
  ON "orderclothes"
FOR EACH ROW
  EXECUTE FUNCTION atualiza_quantidade();
