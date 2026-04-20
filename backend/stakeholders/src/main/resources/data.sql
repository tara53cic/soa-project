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
           '$2a$10$osFzLsQCiaEhIemHIFPNf.w1g48hKI0WDJxTInRxqIO0k2m886pcu',
           'pera@example.com',
           'Pera',
           'Peric',
           false,
           (SELECT id FROM role WHERE name='ROLE_GUIDE')
       ) ON CONFLICT (username) DO NOTHING;

INSERT INTO users (username, password, email, "first_name", "last_name", blocked, role_id)
VALUES (
           'MARE',
           '$2a$10$osFzLsQCiaEhIemHIFPNf.w1g48hKI0WDJxTInRxqIO0k2m886pcu',
           'turista@example.com',
           'Marko',
           'Markovic',
           false,
           (SELECT id FROM role WHERE name='ROLE_TOURIST')
       ) ON CONFLICT (username) DO NOTHING;