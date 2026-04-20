INSERT INTO role (name) VALUES ('ROLE_ADMIN') ON CONFLICT DO NOTHING;
INSERT INTO role (name) VALUES ('ROLE_GUIDE') ON CONFLICT DO NOTHING;
INSERT INTO role (name) VALUES ('ROLE_TOURIST') ON CONFLICT DO NOTHING;

INSERT INTO users (username, password, email, "first_name", "last_name", blocked, role_id)
VALUES (
           'admin',
           '$2a$10$osFzLsQCiaEhIemHIFPNf.w1g48hKI0WDJxTInRxqIO0k2m886pcu',
           'admin@example.com',
           'System',
           'Admin',
           false,
           (SELECT id FROM role WHERE name='ROLE_ADMIN')
       ) ON CONFLICT (username) DO NOTHING;

INSERT INTO users (username, password, email, "first_name", "last_name", blocked, role_id)
VALUES (
           'pera',
           '$2a$10$f9Y.Sj3On6D7T9Z6KzUu/.M6v1E3Q7P6fVvM7F7G7vG7vG7vG7vG', //pera123
           'pera@example.com',
           'Pera',
           'Peric',
           false,
           (SELECT id FROM role WHERE name='ROLE_GUIDE')
       ) ON CONFLICT (username) DO NOTHING;

INSERT INTO users (username, password, email, "first_name", "last_name", blocked, role_id)
VALUES (
           'MARE',
           '$2a$10$w8T.Gj3On6D7T9Z6KzUu/.L6v1E3Q7P6fVvM7F7G7vG7vG7vG7vG', //marko123
           'turista@example.com',
           'Marko',
           'Markovic',
           false,
           (SELECT id FROM role WHERE name='ROLE_TOURIST')
       ) ON CONFLICT (username) DO NOTHING;