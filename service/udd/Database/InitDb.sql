INSERT INTO ScientificPaper (Id, DOI, MagazineTitle, Title, Abstract, pdfFileName)
VALUES (1, "10.1000/zxy123", "Магазин 1", "Бајесова вероватноћа", "Бајесова вероватноћа је тумачење концепта вероватноће. За разлику од тумачења вероватноће kao фреквенције или склоности неке појаве, Бајесова вероватноћа је количина коју доделимо представљеном стању знања или стању веровања.", "Бајесова вероватноћа.pdf");

INSERT INTO ScientificPaper (Id, DOI, MagazineTitle, Title, Abstract, pdfFileName)
VALUES (2, "10.2000/xzz234", "Magazin 1", "Bajesova verovatnoća", "Bajesova verovatnoća je tumačenje koncepta verovatnoće. Za razliku od tumačenja verovatnoće kao frekvencije ili sklonosti neke pojave, Bajesova verovatnoća je količina koju dodelimo predstavljenom stanju znanja ili stanju verovanja.", "Bajesova verovatnoća.pdf");

INSERT INTO ScientificPaper (Id, DOI, MagazineTitle, Title, Abstract, pdfFileName)
VALUES (3, "10.3000/xyz345", "Magazine 1", "Bayes network", "A Bayesian network (also known as a Bayes network, belief network, or decision network) is a probabilistic graphical model that represents a set of variables and their conditional dependencies via a directed acyclic graph (DAG).", "Bayes network.pdf");


INSERT INTO Author (Id, Firstname, Lastname, ScientificPaperId)
VALUES (1, "википедија", "ср", 1);

INSERT INTO Author (Id, Firstname, Lastname, ScientificPaperId) 
VALUES (2, "vikipedija", "sh", 2);

INSERT INTO Author (Id, Firstname, Lastname, ScientificPaperId) 
VALUES (3, "wikipedia", "en", 3);

INSERT INTO Keyword (Id, Title, ScientificPaperId) 
VALUES (1, "вероватноћа", 1);

INSERT INTO Keyword (Id, Title, ScientificPaperId) 
VALUES (2, "verovatnoća", 2);
INSERT INTO Keyword (Id, Title, ScientificPaperId) 
VALUES (3, "simulacija", 2);

INSERT INTO Keyword (Id, Title, ScientificPaperId) 
VALUES (4, "probability", 3);
INSERT INTO Keyword (Id, Title, ScientificPaperId) 
VALUES (5, "simulation", 3);
INSERT INTO Keyword (Id, Title, ScientificPaperId) 
VALUES (6, "graph", 3);

INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (1,"рачунарске науке", 1);
INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (2,"машинско учење", 1);
INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (3,"вероватноћа", 1);

INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (4,"računarske nauke", 2);
INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (5,"mašinsko učenje", 2);
INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (6,"verovatnoća", 2);

INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (7,"computer science", 3);
INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (8,"machine learning", 3);
INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (9,"deep learning", 3);
INSERT INTO ScientificField (Id, Title, ScientificPaperId) 
VALUES (10,"probability", 3);