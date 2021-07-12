CREATE TABLE Quotes
(
  Id SERIAL PRIMARY KEY,
  Wisdom TEXT,
  Author TEXT
);

INSERT INTO Quotes
  (Wisdom, Author)
VALUES 
  ('The purpose of our lives is to be happy.', 'Dalai Lama'),
  ('Get busy living or get busy dying.', 'Stephen King'),
  ('You only live once, but if you do it right, once is enough.', 'Mae West'),
  ('Many of life’s failures are people who did not realize how close they were to success when they gave up.', 'Thomas A. Edison');

