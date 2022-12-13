CREATE OR REPLACE FUNCTION log_new_users() RETURNS TRIGGER AS $$
DECLARE
    namestr varchar(30);
    emailstr varchar(100);
    messtr varchar(100);
    secmesstr varchar(100);
    resstr varchar(254);
BEGIN
    namestr = NEW."UserName";
    emailstr = NEW."Email";
    messtr := 'New user. Username: ';
    secmesstr := ', Email: ';
    resstr := messtr || namestr || secmesstr || emailstr;
    INSERT INTO logs("Text", "Added") values (resstr, NOW());
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;